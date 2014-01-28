using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;

using Common.DA.Interface;
using Common.Types;
using Common.Types.Exceptions;

namespace Common.BL.Search.SearchSpecification
{
    /// <summary>
    /// Класс контейнер для хранения спецификаций настраиваемого поиска.
    /// </summary>
    public class SearchSpecificationContainer : ISearchPresetContainer
    {
        public string Marker { get; set; }

        /// <summary>
        /// IoC-контейнер, который разрешает все зависимости.
        /// </summary>
        private readonly IContainer IocContainer;

        /// <summary>
        /// Возвращает список спецификаций условий настраиваемого поиска.
        /// </summary>
        public List<SearchExpressionSpec> ExpressionList { get; private set; }

        /// <summary>
        /// Класс контейнер для хранения спецификаций условий настраиваемого поиска.
        /// </summary>
        public SearchSpecificationContainer()
        {
            this.ExpressionList = new List<SearchExpressionSpec>
            {
                new SearchExpressionSpec("включает", "{0}.ToUpper().Contains(@0.ToUpper())", SearchTypeSpec.String, SearchConditionType.Includes, ExpressionQuantity.Single),
                new SearchExpressionSpec("начинается с ", "{0}.ToUpper().StartsWith(@0.ToUpper())", SearchTypeSpec.String, SearchConditionType.StartsWith,  ExpressionQuantity.Single),
                new SearchExpressionSpec("заканчивается на", "{0}.ToUpper().EndsWith(@0.ToUpper())", SearchTypeSpec.String, SearchConditionType.EndsWith,  ExpressionQuantity.Single),
                new SearchExpressionSpec("cовпадает", "{0}.ToUpper() == @0.ToUpper()", SearchTypeSpec.String, SearchConditionType.Equals,  ExpressionQuantity.Single),
                new SearchExpressionSpec("не совпадает", "{0}.ToUpper() != @0.ToUpper()", SearchTypeSpec.String, SearchConditionType.NotEquals,  ExpressionQuantity.Single),
                new SearchExpressionSpec("равно", "!({0} != @0 && @0 != 0)", SearchTypeSpec.Number, SearchConditionType.Equals, ExpressionQuantity.Single),
                new SearchExpressionSpec("не равно", "({0} != @0 && @0 != 0)", SearchTypeSpec.Number, SearchConditionType.NotEquals, ExpressionQuantity.Single),
                new SearchExpressionSpec("больше", "{0} >= @0", SearchTypeSpec.Number, SearchConditionType.More,  ExpressionQuantity.Single),
                new SearchExpressionSpec("меньше", "{0} <= @0", SearchTypeSpec.Number, SearchConditionType.Less, ExpressionQuantity.Single),
                new SearchExpressionSpec("в пределах", "{0} >= @0 && {0} <= @1", SearchTypeSpec.Number, SearchConditionType.Range, ExpressionQuantity.Double),
                new SearchExpressionSpec("совпадает", "{0} == @0", SearchTypeSpec.Date, SearchConditionType.Equals, ExpressionQuantity.Single),
                new SearchExpressionSpec("не совпадает", "{0} == @0", SearchTypeSpec.Date, SearchConditionType.NotEquals, ExpressionQuantity.Single),
                new SearchExpressionSpec("позднее", "{0} >= @0", SearchTypeSpec.Date, SearchConditionType.More, ExpressionQuantity.Single),
                new SearchExpressionSpec("раньше", "{0} <= @0", SearchTypeSpec.Date, SearchConditionType.Less, ExpressionQuantity.Single),
                new SearchExpressionSpec("в пределах", "{0} >= @0 && {0} <= @1", SearchTypeSpec.Date, SearchConditionType.Range, ExpressionQuantity.Double),
                new SearchExpressionSpec("истина/ложь", "{0} == @0", SearchTypeSpec.Bool, SearchConditionType.Equals, ExpressionQuantity.Single),
                new SearchExpressionSpec("является", "{0}.Id == Convert.ToInt32(@0)", SearchTypeSpec.Dictionary, SearchConditionType.Equals, ExpressionQuantity.Single),
                new SearchExpressionSpec("не является", "{0}.Id != Convert.ToInt32(@0)", SearchTypeSpec.Dictionary, SearchConditionType.NotEquals, ExpressionQuantity.Single),
                new SearchExpressionSpec("является", "{0} == @0", SearchTypeSpec.Enum, SearchConditionType.Equals, ExpressionQuantity.Single),
                new SearchExpressionSpec("не является", "{0} != @0", SearchTypeSpec.Enum, SearchConditionType.NotEquals, ExpressionQuantity.Single)
            };

            IocContainer = new ContainerBuilder().Build();
        }

        #region Register

        /// <summary>
        /// Регистрирует список пресетов поиска
        /// </summary>
        /// <param name="presetList">Список пресетов поиска</param>
        public void RegisterSearchPresetList(List<SearchPresetSpec> presetList)
        {
            foreach (var presetSpec in presetList)
            {
                var domainType = presetSpec.GetType().GetGenericArguments().Single();

                var containerBuilder = new ContainerBuilder();

                containerBuilder.RegisterInstance(presetSpec).Named<SearchPresetSpec>(domainType.FullName);

                containerBuilder.Update(IocContainer);
            }
        }

        /// <summary>
        /// Регистриует все свойства (которые могут использоваться в условиях поиска) всех доменных типов из списка сборок.
        /// </summary>
        /// <param name="dmAssemblies">Список сборок с доменными типами</param>
        /// <exception cref="BLLException">Ошибка при регистрации свойств поиска</exception>
        public void RegisterSearchProperties(IEnumerable<Assembly> dmAssemblies)
        {
            try
            {
                foreach (var dmAssembly in dmAssemblies)
                {
                    RegisterSearchProperties(dmAssembly);
                }
            }
            catch (Exception ex)
            {
                throw new BLLException("Ошибка при регистрации свойств поиска", ex);
            }
        }

        /// <summary>
        /// Регистриует все свойства (которые могут использоваться в условиях поиска) всех доменных типов из сборки.
        /// </summary>
        /// <param name="dmAssembly">Сборка с доменными типами</param>
        private void RegisterSearchProperties(Assembly dmAssembly)
        {
            var containerBuilder = new ContainerBuilder();

            foreach (var type in dmAssembly.GetTypes().Where(t => typeof(IDomainObject).IsAssignableFrom(t)))
            {
                var classAttr =
                    type.GetCustomAttributes(typeof(SearchClassAttribute), false).SingleOrDefault() as
                    SearchClassAttribute;
                string displayName = classAttr == null ? type.Name : classAttr.DisplayName;

                RegisterSearchProperties(containerBuilder, type, displayName);
            }

            containerBuilder.Update(IocContainer);
        }

        /// <summary>
        /// Регистриует все свойства (которые могут использоваться в условиях поиска) доменного типа.
        /// </summary>
        /// <param name="containerBuilder">Билдер IoC-контейнера</param>
        /// <param name="type">Доменный тип</param>
        /// <param name="displyaName">Отображаемое имя типа</param>
        private void RegisterSearchProperties(ContainerBuilder containerBuilder, Type type, string displyaName)
        {
            var propertySpecList = new List<SearchPropertySpec>();
            foreach (var prop in type.GetProperties())
            {
                var attr = prop.GetCustomAttributes(typeof(SearchFieldAttribute), false).SingleOrDefault() as SearchFieldAttribute;
                if (attr == null) continue;
                propertySpecList.Add(this.CreateSearchPropertySpec(attr, prop, displyaName));
            }
            containerBuilder.RegisterInstance(propertySpecList).Named<List<SearchPropertySpec>>(type.FullName);
        }

        /// <summary>
        /// Возвращает спецификацию свойства поиска.
        /// </summary>
        /// <param name="searchFieldAttribute">Атрибут свойства поиска</param>
        /// <param name="propertyInfo">Информация о свойстве</param>
        /// <param name="classDisplayName">Отображаемое имя класса</param>
        /// <returns>Спецификация свойства поиска</returns>
        private SearchPropertySpec CreateSearchPropertySpec(SearchFieldAttribute searchFieldAttribute,
                                                            PropertyInfo propertyInfo,
                                                            string classDisplayName)
        {
            return new SearchPropertySpec(searchFieldAttribute.Name,
                                          propertyInfo.Name,
                                          classDisplayName,
                                          searchFieldAttribute.SearchTypeSpec,
                                          propertyInfo.PropertyType,
                                          propertyInfo.ReflectedType);
        }

        #endregion

        #region Resolve

        /// <summary>
        /// Возвращает пресет поиска доменных объектов типа T
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <exception cref="VMException">Ошибка при создании экземпляра пресета поиска для типа</exception>
        /// <returns>Пресет поиска</returns>
        public SearchPreset ResolveSearchPreset<T>() where T : IPersistentObject
        {
            try
            {
                var presetSpec = IocContainer.ResolveNamed<SearchPresetSpec>(typeof(T).FullName);

                var domainType = presetSpec.GetType().GetGenericArguments().Single();
                var presetType = typeof(SearchPreset<>).MakeGenericType(domainType);
                var preset = (SearchPreset)Activator.CreateInstance(presetType, this);

                foreach (var presetSpecdet in presetSpec.PresetSpecDets)
                {
                    preset.AddExpression(presetSpecdet);
                }

                preset.OrderFieldList = presetSpec.OrderFieldList;

                return preset;
            }
            catch (DependencyResolutionException)
            {
                return new SearchPreset<T>(this);
            }
        }

        /// <summary>
        /// Для доменного типа Возвращает список спецификаций свойств, которые могут использоваться в условиях поиска.
        /// </summary>
        /// <param name="domainType">Доменный тип</param>
        /// <returns>Список спецификаций свойств</returns>
        /// <exception cref="BLLException">Ошибка при создании экземпляра списка спецификаций свойств поиска</exception>
        public List<SearchPropertySpec> ResolveSearchPropertySpecs(Type domainType)
        {
            try
            {
                return IocContainer.ResolveNamed<List<SearchPropertySpec>>(domainType.FullName);
            }

            catch (ComponentNotRegisteredException ex)
            {
                throw new BLLException(string.Format("Для типа {0} в контейнере не зарегистрировано спецификаций свойств поиска", domainType.Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new BLLException(string.Format("Ошибка при создании экземпляра списка спецификаций свойств поиска для типа {0}", domainType.Name), ex);
            }
        }

        #endregion
    }
}
