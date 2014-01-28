using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;

using Common.BL;
using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using Common.BL.DictionaryManagement.DictionaryException;
using Common.BL.Search.SearchSpecification;
using Common.DA;
using Common.DA.Interface;
using Common.Types;
using Common.Types.Exceptions;
using Common.UI.ViewModel.Interfaces;
using Common.UI.WeakEvent.EventSubscriber;

namespace Common.UI
{
    /// <summary>
    /// Класс-контейнер типов ViewModel и View.
    /// </summary>
    /// <remarks>
    /// Контейнер регистрирует и ресолвит наследников от <c>ISearchVM</c>, <c>ISearchResultVM</c>, <c>IListItemVM</c>, <c>IEditableVM</c>,
    /// а также BL-классы для доменных объектов(<c>ISearchStrategy</c>, <c>IDomainObjectSearcher</c>, <c>IDomainDataMapper</c>, <c>IDomainObjectEventSubscriber</c>,), 
    /// которые инжектятся в ViewModel'ы  поиска.
    /// UPDATE 05.10.2012 дополнительно регистриурет и ресолвит View для EditableVM редактирования агрегатных доменных объектов.
    /// UPDATE 11.10.2012 дополнительно регистриурет и ресолвит пресеты, спецификации условий и свойств кастомного поиска
    /// </remarks>
    internal class UIContainer
    {
        /// <summary>
        /// IoC-контейнер, который разрешает все зависимости.
        /// </summary>
        private Autofac.IContainer IocContainer;

        #region Singleton

        /// <summary>
        /// Lazy singleton instance of <see cref="IocContainer"/>
        /// </summary>
        private static readonly Lazy<UIContainer> Lazy = new Lazy<UIContainer>(() => new UIContainer());

        /// <summary>
        /// Returns singleton-instance of <see cref="IocContainer"/>
        /// </summary>
        internal static UIContainer Instance
        {
            get
            {
                return Lazy.Value;
            }
        }

        /// <summary>
        /// Класс-контейнер типов ViewModel.
        /// </summary>
        private UIContainer()
        {

        }

        #endregion

        #region Register

        /// <summary>
        /// Инициализирует контейнер UI-классов
        /// </summary>
        /// <param name="domainLogicContainer">Контейнер BL-классов</param>
        internal void InitializeUIContainer(IDomainLogicContainer domainLogicContainer)
        {
            this.IocContainer = domainLogicContainer.IocContainer;
            if (!IocContainer.IsRegistered<ISearchPresetContainer>())
            {
                var containerBuilder = new ContainerBuilder();
                var searchSpecificationContainer = new SearchSpecificationContainer();
                containerBuilder.RegisterInstance(searchSpecificationContainer);
                containerBuilder.RegisterInstance<ISearchPresetContainer>(searchSpecificationContainer);
                containerBuilder.Update(this.IocContainer);
            }
        }

        /// <summary>
        /// Регистрирует типы ViewModel из UI-сборки  
        /// </summary>
        /// <param name="uiAssemblies">UI-сборки</param>
        /// <exception cref="GUException">UI контейнер не был инициализирован.</exception>
        internal void RegisterVMTypes(IEnumerable<Assembly> uiAssemblies)
        {
            if (IocContainer == null)
            {
                throw new GUException("UI контейнер не был инициализирован.");
            }

            this.RegisterImplementsInterfase(uiAssemblies, typeof(ISearchResultVM<>));
            this.RegisterImplementsInterfase(uiAssemblies, typeof(ISearchVM<>));
            this.RegisterImplementsInterfase(uiAssemblies, typeof(IEditableVM<>));
            this.RegisterImplementsInterfase(uiAssemblies, typeof(IDomainObjectEventSubscriber<>));
            this.RegisterImplementsInterfase(uiAssemblies, typeof(IDomainValidateableVM<>), typeof(IListItemVM<>));
            this.RegisterImplementsInterfase(uiAssemblies, typeof(IListItemVM<>));
        }

        /// <summary>
        /// Регистрирует тип TView редактирования сущностей типа TEntity
        /// </summary>
        /// <typeparam name="TView">Тип View</typeparam>
        /// <typeparam name="TEntity">Доменный тип</typeparam>
        /// <exception cref="GUException">UI контейнер не был инициализирован.</exception>
        internal void RegisterEditableView<TView, TEntity>()
            where TView : UserControl
            where TEntity : IPersistentObject
        {
            if (IocContainer == null)
            {
                throw new GUException("UI контейнер не был инициализирован.");
            }

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<TView>().Named<UserControl>(typeof(TEntity).FullName);
            containerBuilder.Update(this.IocContainer);
        }

        /// <summary>
        /// Регистрирует тип ViewModel
        /// </summary>
        /// <typeparam name="TInterface">Тип интерфейса ViewModel</typeparam>
        /// <typeparam name="TViewModel">Конкретный тип ViewModel</typeparam>
        /// <param name="tag">Ключ для ресолва</param>
        internal void RegisterType<TInterface, TViewModel>(string tag)
            where TInterface : INotifyPropertyChanged
            where TViewModel : INotifyPropertyChanged
        {
            if (IocContainer == null)
            {
                throw new GUException("UI контейнер не был инициализирован.");
            }

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<TViewModel>().Keyed<TInterface>(tag);
            containerBuilder.Update(this.IocContainer);
        }

        /// <summary>
        /// Регистрирует типы наследующие заданому интерфейсу.
        /// </summary>
        /// <param name="assemblies">Сборки с конкретными типами</param>
        /// <param name="interfaceType">Базовый интерфейс</param>
        private void RegisterImplementsInterfase(IEnumerable<Assembly> assemblies, Type interfaceType)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterAssemblyTypes(assemblies.ToArray())
                             .Where(x => x.IsAssignableToGenericType(interfaceType))
                             .AsClosedTypesOf(interfaceType);

            containerBuilder.Update(IocContainer);
        }

        /// <summary>
        /// Регистрирует типы наследующие заданому интерфейсу.
        /// </summary>
        /// <param name="assemblies">Сборки с конкретными типами</param>
        /// <param name="interfaceType">Базовый интерфейс</param>
        private void RegisterImplementsInterfase(IEnumerable<Assembly> assemblies, Type interfaceType, Type exceptType)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterAssemblyTypes(assemblies.ToArray())
                             .Where(x => x.IsAssignableToGenericType(interfaceType) && !x.IsAssignableToGenericType(exceptType))
                             .AsClosedTypesOf(interfaceType);


            containerBuilder.Update(IocContainer);
        }


        #endregion

        #region Resolve

        /// <summary>
        /// Возвращает экземпляр <c>IListItemVM</c> для заданного доменного объекта.
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <param name="item">Доменный объект</param>
        /// <returns>Экземпляр <c>IListItemVM</c></returns>
        /// <exception cref="VMException">Ошибка при создании экземпляра типа AbstractListItemVM</exception>
        internal IListItemVM<T> ResolveListItemVMType<T>(T item) where T : IDomainObject
        {
            try
            {
                return this.IocContainer.Resolve<IListItemVM<T>>(new NamedParameter("entity", item), new NamedParameter("isValidateable", true));
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format("Класс IListItemVM<{0}> не зарегистрирован в контейнере", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("Ошибка при создании экземпляра типа IListItemVM<{0}>", typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// Возвращает экземпляр <c>ISearchResultVM</c> для заданного доменного объекта.
        /// </summary>
        /// <typeparam name="T">Тип доменного объекта</typeparam>
        /// <param name="result">Доменный объект</param>
        /// <returns>Экземпляр <c>ISearchResultVM</c></returns>
        /// <exception cref="VMException">Ошибка при создании экземпляра типа AbstractSearchResultVM</exception>
        internal ISearchResultVM<T> ResolveSearchResultVMType<T>(T result) where T : IDomainObject
        {
            try
            {
                return this.IocContainer.Resolve<ISearchResultVM<T>>(new NamedParameter("entity", result));
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format("Класс ISearchResultVM<{0}> не зарегистрирован в контейнере", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("Ошибка при создании экземпляра типа ISearchResultVM<{0}>", typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// Возвращает экземпляр <c>ISearchVM</c> для заданного типа доменных объектов.
        /// </summary>
        /// <typeparam name="T">Тип доменных объектов</typeparam>
        /// <returns>Экземпляр <c>ISearchVM</c></returns>
        /// <exception cref="VMException">Ошибка при создании экземпляра типа AbstractSearchVM</exception>
        internal ISearchVM<T> ResolveSearchVMType<T>() where T : IPersistentObject
        {
            try
            {
                return this.IocContainer.Resolve<ISearchVM<T>>();
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format("Класс ISearchVM<{0}> не зарегистрирован в контейнере", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("Ошибка при создании экземпляра типа ISearchVM<{0}>", typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// Возвращает экземпляр <c>ISearchVM</c> для заданного типа доменных объектов.
        /// </summary>
        /// <param name="domainType">Доменный тип</param>
        /// <returns>Экземпляр <c>ISearchVM</c></returns>
        /// <exception cref="VMException">Ошибка при создании экземпляра типа AbstractSearchVM</exception>
        internal ISearchVM ResolveSearchVMType(Type domainType)
        {
            try
            {
                var searchVmType = typeof(ISearchVM<>).MakeGenericType(domainType);
                return (ISearchVM)this.IocContainer.Resolve(searchVmType);
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format("Класс ISearchVM<{0}> не зарегистрирован в контейнере", domainType.Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("Ошибка при создании экземпляра типа ISearchVM<{0}>", domainType.Name), ex);
            }
        }

        /// <summary>
        /// Возвращает экземпляр ViewModel для окна редактирования доменного объекта.
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <param name="entity">Редактируемый объект</param>
        /// <param name="isEditable">Флаг возможноти редактирования данных объекта</param>
        /// <returns>ViewModel для окна редактирования доменного объекта</returns>
        /// <exception cref="VMException">Ошибка при создании экземпляра типа EditableVM</exception>
        internal IEditableVM<T> ResolveEditableVM<T>(T entity, bool isEditable = true) where T : DomainObject<T>, IPersistentObject
        {
            try
            {
                return this.IocContainer.Resolve<IEditableVM<T>>(new NamedParameter("entity", entity),
                                                                 new NamedParameter("isEditable", isEditable));
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format("Класс IEditableVM<{0}> не зарегистрирован в контейнере", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("Ошибка при создании экземпляра типа IEditableVM<{0}>", typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// Возвращает экземпляр ViewModel для окна редактирования доменного объекта.
        /// </summary>
        /// <param name="domainType">Доменный тип</param>
        /// <param name="entity">Редактируемый объект</param>
        /// <param name="isEditable">Флаг возможноти редактирования данных объекта</param>
        /// <returns>ViewModel для окна редактирования доменного объекта</returns>
        /// <exception cref="VMException">Ошибка при создании экземпляра типа EditableVM</exception>
        internal IEditableVM ResolveEditableVM(Type domainType, IDomainObject entity, bool isEditable = true)
        {
            try
            {
                var editableVmType = typeof(IEditableVM<>).MakeGenericType(domainType);
                return (IEditableVM)this.IocContainer.Resolve(editableVmType,
                                                              new NamedParameter("entity", entity),
                                                              new NamedParameter("isEditable", isEditable));
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format("Класс IEditableVM<{0}> не зарегистрирован в контейнере", domainType.Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("Ошибка при создании экземпляра типа IEditableVM<{0}>", domainType.Name), ex);
            }
        }

        public IEditableVM ResolveEditableVM(Type domainType, string entityKey, bool isEditable)
        {
            MethodInfo method = GetType().GetMethod("ResolveEditableVmByKey", new Type[] { })
                              .MakeGenericMethod(new[] { domainType });
            return (IEditableVM)method.Invoke(this, new object[] { entityKey, isEditable });
        }

        protected IEditableVM ResolveEditableVmByKey<T>(string entityKey, bool isEditable = true) where T : DomainObject<T>, IPersistentObject
        {
            try
            {
                var mapper = IocContainer.Resolve<IDomainDataMapper<T>>();
                var entity = mapper.Retrieve(entityKey);
                return ResolveEditableVM(typeof(T), entity, isEditable);
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format("Класс IEditableVM<{0}> не зарегистрирован в контейнере", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("Ошибка при создании экземпляра типа IEditableVM<{0}>", typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// Возвращает экземпляр View редактирования сущности типа domainType.
        /// </summary>
        /// <param name="domainType">Доменный тип</param>
        /// <returns>Экземпляр View редактирования</returns>
        /// <exception cref="VMException">Класс не зарегистрирован в контейнере</exception>
        internal UserControl ResolveEditableView(Type domainType)
        {
            try
            {
                return this.IocContainer.ResolveNamed<UserControl>(domainType.FullName);
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException("Класс не зарегистрирован в контейнере", ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException("Ошибка при создании экземпляра типа", ex);
            }
        }


        /// <summary>
        /// Возвращает экземпляр ViewModel с функционалом отображения валидации доменного объекта типа T.
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <param name="entity">Доменный объект</param>
        /// <returns>экземпляр ViewModel с функционалом отображения валидации доменного объекта</returns>
        /// <exception cref="VMException"></exception>
        internal IDomainValidateableVM<T> ResolveDomainValidateableVM<T>(T entity) where T : IDomainObject
        {
            try
            {
                return this.IocContainer.Resolve<IDomainValidateableVM<T>>(new NamedParameter("entity", entity), new NamedParameter("isValidateable", true));
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format("Класс IDomainValidateableVM<{0}> не зарегистрирован в контейнере", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("Ошибка при создании экземпляра типа IDomainValidateableVM<{0}>", typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// Возвращает экземпляр ViewModel с функционалом отображения валидации доменного объекта типа T.
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <param name="entity">Доменный объект</param>
        /// <param name="tag">Ключ для ресолва</param>
        /// <returns>экземпляр ViewModel с функционалом отображения валидации доменного объекта</returns>
        /// <exception cref="VMException"></exception>
        internal IDomainValidateableVM<T> ResolveDomainValidateableVM<T>(T entity, string tag) where T : IDomainObject
        {
            try
            {
                return this.IocContainer.ResolveKeyed<IDomainValidateableVM<T>>(tag, new NamedParameter("entity", entity), new NamedParameter("isValidateable", true));
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format("Класс IDomainValidateableVM<{0}> не зарегистрирован в контейнере", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("Ошибка при создании экземпляра типа IDomainValidateableVM<{0}>", typeof(T).Name), ex);
            }
        }

        #endregion

        #region Custom Search Stuff

        #region Register

        /// <summary>
        /// Регистрирует список пресетов поиска
        /// </summary>
        /// <param name="presetList">Список пресетов поиска</param>
        internal void RegisterSearchPresetList(List<SearchPresetSpec> presetList)
        {
            this.IocContainer.Resolve<SearchSpecificationContainer>().RegisterSearchPresetList(presetList);
        }

        /// <summary>
        /// Регистриует все свойства (которые могут использоваться в условиях поиска) всех доменных типов из списка сборок.
        /// </summary>
        /// <param name="dmAssemblies">Список сборок с доменными типами</param>
        /// <exception cref="BLLException">Ошибка при регистрации свойств поиска</exception>
        internal void RegidterSearchProperties(IEnumerable<Assembly> dmAssemblies)
        {
            this.IocContainer.Resolve<SearchSpecificationContainer>().RegisterSearchProperties(dmAssemblies);
        }

        #endregion

        #region Resolve

        /// <summary>
        /// Возвращает пресет поиска доменных объектов типа T
        /// </summary>
        /// <typeparam name="T">Доменный тип</typeparam>
        /// <exception cref="VMException">Ошибка при создании экземпляра пресета поиска для типа</exception>
        /// <returns>Пресет поиска</returns>
        internal SearchPreset ResolveSearchPreset<T>() where T : IPersistentObject
        {
            return this.IocContainer.Resolve<SearchSpecificationContainer>().ResolveSearchPreset<T>();
        }

        /// <summary>
        /// Возвращает список спецификаций условий настраиваемого поиска.
        /// </summary>
        /// <returns></returns>
        internal List<SearchExpressionSpec> ResolveSearchExpressionSpec()
        {
            return this.IocContainer.Resolve<SearchSpecificationContainer>().ExpressionList;
        }

        /// <summary>
        /// Для доменного типа Возвращает список спецификаций свойств, которые могут использоваться в условиях поиска.
        /// </summary>
        /// <param name="domainType">Доменный тип</param>
        /// <exception cref="VMException">"Ошибка при создании экземпляра списка спецификаций свойств поиска для типа</exception>
        /// <returns>Список спецификаций свойств</returns>
        internal List<SearchPropertySpec> ResolveSearchPropertySpecs(Type domainType)
        {
            return this.IocContainer.Resolve<SearchSpecificationContainer>().ResolveSearchPropertySpecs(domainType);
        }

        /// <summary>
        /// Возвращает справочные значения для ассоциации доменного типа.
        /// </summary>
        /// <param name="domainType">Тип ассоциации</param>
        /// <exception cref="BLLException">Ошибка получения справочных значений для типа</exception>
        /// <returns>Справочные значения для ассоциации</returns>
        internal Dictionary<int, object> ResolveDictionaryValues(Type domainType)
        {
            try
            {
                var dictionaryManager = this.IocContainer.Resolve<IDictionaryManager>();
                if (domainType.IsEnum)
                {
                    return dictionaryManager.GetEnumDictionary(domainType).ToDictionary(t => t.Key, t => (object)t.Value);
                }
                return dictionaryManager.GetDictionary(domainType).ToDictionary(t => Convert.ToInt32(t.GetKeyValue()), t => (object)t);
            }
            catch (DictionaryNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BLLException(string.Format("Ошибка получения справочных значений для типа {0}", domainType.Name), ex);
            }
        }

        #endregion

        #endregion

    }
}