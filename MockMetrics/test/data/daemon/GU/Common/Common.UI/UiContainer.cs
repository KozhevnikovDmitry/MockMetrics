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
    /// �����-��������� ����� ViewModel � View.
    /// </summary>
    /// <remarks>
    /// ��������� ������������ � �������� ����������� �� <c>ISearchVM</c>, <c>ISearchResultVM</c>, <c>IListItemVM</c>, <c>IEditableVM</c>,
    /// � ����� BL-������ ��� �������� ��������(<c>ISearchStrategy</c>, <c>IDomainObjectSearcher</c>, <c>IDomainDataMapper</c>, <c>IDomainObjectEventSubscriber</c>,), 
    /// ������� ���������� � ViewModel'�  ������.
    /// UPDATE 05.10.2012 ������������� ������������ � �������� View ��� EditableVM �������������� ���������� �������� ��������.
    /// UPDATE 11.10.2012 ������������� ������������ � �������� �������, ������������ ������� � ������� ���������� ������
    /// </remarks>
    internal class UIContainer
    {
        /// <summary>
        /// IoC-���������, ������� ��������� ��� �����������.
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
        /// �����-��������� ����� ViewModel.
        /// </summary>
        private UIContainer()
        {

        }

        #endregion

        #region Register

        /// <summary>
        /// �������������� ��������� UI-�������
        /// </summary>
        /// <param name="domainLogicContainer">��������� BL-�������</param>
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
        /// ������������ ���� ViewModel �� UI-������  
        /// </summary>
        /// <param name="uiAssemblies">UI-������</param>
        /// <exception cref="GUException">UI ��������� �� ��� ���������������.</exception>
        internal void RegisterVMTypes(IEnumerable<Assembly> uiAssemblies)
        {
            if (IocContainer == null)
            {
                throw new GUException("UI ��������� �� ��� ���������������.");
            }

            this.RegisterImplementsInterfase(uiAssemblies, typeof(ISearchResultVM<>));
            this.RegisterImplementsInterfase(uiAssemblies, typeof(ISearchVM<>));
            this.RegisterImplementsInterfase(uiAssemblies, typeof(IEditableVM<>));
            this.RegisterImplementsInterfase(uiAssemblies, typeof(IDomainObjectEventSubscriber<>));
            this.RegisterImplementsInterfase(uiAssemblies, typeof(IDomainValidateableVM<>), typeof(IListItemVM<>));
            this.RegisterImplementsInterfase(uiAssemblies, typeof(IListItemVM<>));
        }

        /// <summary>
        /// ������������ ��� TView �������������� ��������� ���� TEntity
        /// </summary>
        /// <typeparam name="TView">��� View</typeparam>
        /// <typeparam name="TEntity">�������� ���</typeparam>
        /// <exception cref="GUException">UI ��������� �� ��� ���������������.</exception>
        internal void RegisterEditableView<TView, TEntity>()
            where TView : UserControl
            where TEntity : IPersistentObject
        {
            if (IocContainer == null)
            {
                throw new GUException("UI ��������� �� ��� ���������������.");
            }

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<TView>().Named<UserControl>(typeof(TEntity).FullName);
            containerBuilder.Update(this.IocContainer);
        }

        /// <summary>
        /// ������������ ��� ViewModel
        /// </summary>
        /// <typeparam name="TInterface">��� ���������� ViewModel</typeparam>
        /// <typeparam name="TViewModel">���������� ��� ViewModel</typeparam>
        /// <param name="tag">���� ��� �������</param>
        internal void RegisterType<TInterface, TViewModel>(string tag)
            where TInterface : INotifyPropertyChanged
            where TViewModel : INotifyPropertyChanged
        {
            if (IocContainer == null)
            {
                throw new GUException("UI ��������� �� ��� ���������������.");
            }

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterType<TViewModel>().Keyed<TInterface>(tag);
            containerBuilder.Update(this.IocContainer);
        }

        /// <summary>
        /// ������������ ���� ����������� �������� ����������.
        /// </summary>
        /// <param name="assemblies">������ � ����������� ������</param>
        /// <param name="interfaceType">������� ���������</param>
        private void RegisterImplementsInterfase(IEnumerable<Assembly> assemblies, Type interfaceType)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterAssemblyTypes(assemblies.ToArray())
                             .Where(x => x.IsAssignableToGenericType(interfaceType))
                             .AsClosedTypesOf(interfaceType);

            containerBuilder.Update(IocContainer);
        }

        /// <summary>
        /// ������������ ���� ����������� �������� ����������.
        /// </summary>
        /// <param name="assemblies">������ � ����������� ������</param>
        /// <param name="interfaceType">������� ���������</param>
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
        /// ���������� ��������� <c>IListItemVM</c> ��� ��������� ��������� �������.
        /// </summary>
        /// <typeparam name="T">��� ��������� �������</typeparam>
        /// <param name="item">�������� ������</param>
        /// <returns>��������� <c>IListItemVM</c></returns>
        /// <exception cref="VMException">������ ��� �������� ���������� ���� AbstractListItemVM</exception>
        internal IListItemVM<T> ResolveListItemVMType<T>(T item) where T : IDomainObject
        {
            try
            {
                return this.IocContainer.Resolve<IListItemVM<T>>(new NamedParameter("entity", item), new NamedParameter("isValidateable", true));
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format("����� IListItemVM<{0}> �� ��������������� � ����������", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("������ ��� �������� ���������� ���� IListItemVM<{0}>", typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// ���������� ��������� <c>ISearchResultVM</c> ��� ��������� ��������� �������.
        /// </summary>
        /// <typeparam name="T">��� ��������� �������</typeparam>
        /// <param name="result">�������� ������</param>
        /// <returns>��������� <c>ISearchResultVM</c></returns>
        /// <exception cref="VMException">������ ��� �������� ���������� ���� AbstractSearchResultVM</exception>
        internal ISearchResultVM<T> ResolveSearchResultVMType<T>(T result) where T : IDomainObject
        {
            try
            {
                return this.IocContainer.Resolve<ISearchResultVM<T>>(new NamedParameter("entity", result));
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format("����� ISearchResultVM<{0}> �� ��������������� � ����������", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("������ ��� �������� ���������� ���� ISearchResultVM<{0}>", typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// ���������� ��������� <c>ISearchVM</c> ��� ��������� ���� �������� ��������.
        /// </summary>
        /// <typeparam name="T">��� �������� ��������</typeparam>
        /// <returns>��������� <c>ISearchVM</c></returns>
        /// <exception cref="VMException">������ ��� �������� ���������� ���� AbstractSearchVM</exception>
        internal ISearchVM<T> ResolveSearchVMType<T>() where T : IPersistentObject
        {
            try
            {
                return this.IocContainer.Resolve<ISearchVM<T>>();
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format("����� ISearchVM<{0}> �� ��������������� � ����������", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("������ ��� �������� ���������� ���� ISearchVM<{0}>", typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// ���������� ��������� <c>ISearchVM</c> ��� ��������� ���� �������� ��������.
        /// </summary>
        /// <param name="domainType">�������� ���</param>
        /// <returns>��������� <c>ISearchVM</c></returns>
        /// <exception cref="VMException">������ ��� �������� ���������� ���� AbstractSearchVM</exception>
        internal ISearchVM ResolveSearchVMType(Type domainType)
        {
            try
            {
                var searchVmType = typeof(ISearchVM<>).MakeGenericType(domainType);
                return (ISearchVM)this.IocContainer.Resolve(searchVmType);
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format("����� ISearchVM<{0}> �� ��������������� � ����������", domainType.Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("������ ��� �������� ���������� ���� ISearchVM<{0}>", domainType.Name), ex);
            }
        }

        /// <summary>
        /// ���������� ��������� ViewModel ��� ���� �������������� ��������� �������.
        /// </summary>
        /// <typeparam name="T">�������� ���</typeparam>
        /// <param name="entity">������������� ������</param>
        /// <param name="isEditable">���� ���������� �������������� ������ �������</param>
        /// <returns>ViewModel ��� ���� �������������� ��������� �������</returns>
        /// <exception cref="VMException">������ ��� �������� ���������� ���� EditableVM</exception>
        internal IEditableVM<T> ResolveEditableVM<T>(T entity, bool isEditable = true) where T : DomainObject<T>, IPersistentObject
        {
            try
            {
                return this.IocContainer.Resolve<IEditableVM<T>>(new NamedParameter("entity", entity),
                                                                 new NamedParameter("isEditable", isEditable));
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format("����� IEditableVM<{0}> �� ��������������� � ����������", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("������ ��� �������� ���������� ���� IEditableVM<{0}>", typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// ���������� ��������� ViewModel ��� ���� �������������� ��������� �������.
        /// </summary>
        /// <param name="domainType">�������� ���</param>
        /// <param name="entity">������������� ������</param>
        /// <param name="isEditable">���� ���������� �������������� ������ �������</param>
        /// <returns>ViewModel ��� ���� �������������� ��������� �������</returns>
        /// <exception cref="VMException">������ ��� �������� ���������� ���� EditableVM</exception>
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
                throw new VMException(string.Format("����� IEditableVM<{0}> �� ��������������� � ����������", domainType.Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("������ ��� �������� ���������� ���� IEditableVM<{0}>", domainType.Name), ex);
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
                throw new VMException(string.Format("����� IEditableVM<{0}> �� ��������������� � ����������", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("������ ��� �������� ���������� ���� IEditableVM<{0}>", typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// ���������� ��������� View �������������� �������� ���� domainType.
        /// </summary>
        /// <param name="domainType">�������� ���</param>
        /// <returns>��������� View ��������������</returns>
        /// <exception cref="VMException">����� �� ��������������� � ����������</exception>
        internal UserControl ResolveEditableView(Type domainType)
        {
            try
            {
                return this.IocContainer.ResolveNamed<UserControl>(domainType.FullName);
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException("����� �� ��������������� � ����������", ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException("������ ��� �������� ���������� ����", ex);
            }
        }


        /// <summary>
        /// ���������� ��������� ViewModel � ������������ ����������� ��������� ��������� ������� ���� T.
        /// </summary>
        /// <typeparam name="T">�������� ���</typeparam>
        /// <param name="entity">�������� ������</param>
        /// <returns>��������� ViewModel � ������������ ����������� ��������� ��������� �������</returns>
        /// <exception cref="VMException"></exception>
        internal IDomainValidateableVM<T> ResolveDomainValidateableVM<T>(T entity) where T : IDomainObject
        {
            try
            {
                return this.IocContainer.Resolve<IDomainValidateableVM<T>>(new NamedParameter("entity", entity), new NamedParameter("isValidateable", true));
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format("����� IDomainValidateableVM<{0}> �� ��������������� � ����������", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("������ ��� �������� ���������� ���� IDomainValidateableVM<{0}>", typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// ���������� ��������� ViewModel � ������������ ����������� ��������� ��������� ������� ���� T.
        /// </summary>
        /// <typeparam name="T">�������� ���</typeparam>
        /// <param name="entity">�������� ������</param>
        /// <param name="tag">���� ��� �������</param>
        /// <returns>��������� ViewModel � ������������ ����������� ��������� ��������� �������</returns>
        /// <exception cref="VMException"></exception>
        internal IDomainValidateableVM<T> ResolveDomainValidateableVM<T>(T entity, string tag) where T : IDomainObject
        {
            try
            {
                return this.IocContainer.ResolveKeyed<IDomainValidateableVM<T>>(tag, new NamedParameter("entity", entity), new NamedParameter("isValidateable", true));
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format("����� IDomainValidateableVM<{0}> �� ��������������� � ����������", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("������ ��� �������� ���������� ���� IDomainValidateableVM<{0}>", typeof(T).Name), ex);
            }
        }

        #endregion

        #region Custom Search Stuff

        #region Register

        /// <summary>
        /// ������������ ������ �������� ������
        /// </summary>
        /// <param name="presetList">������ �������� ������</param>
        internal void RegisterSearchPresetList(List<SearchPresetSpec> presetList)
        {
            this.IocContainer.Resolve<SearchSpecificationContainer>().RegisterSearchPresetList(presetList);
        }

        /// <summary>
        /// ����������� ��� �������� (������� ����� �������������� � �������� ������) ���� �������� ����� �� ������ ������.
        /// </summary>
        /// <param name="dmAssemblies">������ ������ � ��������� ������</param>
        /// <exception cref="BLLException">������ ��� ����������� ������� ������</exception>
        internal void RegidterSearchProperties(IEnumerable<Assembly> dmAssemblies)
        {
            this.IocContainer.Resolve<SearchSpecificationContainer>().RegisterSearchProperties(dmAssemblies);
        }

        #endregion

        #region Resolve

        /// <summary>
        /// ���������� ������ ������ �������� �������� ���� T
        /// </summary>
        /// <typeparam name="T">�������� ���</typeparam>
        /// <exception cref="VMException">������ ��� �������� ���������� ������� ������ ��� ����</exception>
        /// <returns>������ ������</returns>
        internal SearchPreset ResolveSearchPreset<T>() where T : IPersistentObject
        {
            return this.IocContainer.Resolve<SearchSpecificationContainer>().ResolveSearchPreset<T>();
        }

        /// <summary>
        /// ���������� ������ ������������ ������� �������������� ������.
        /// </summary>
        /// <returns></returns>
        internal List<SearchExpressionSpec> ResolveSearchExpressionSpec()
        {
            return this.IocContainer.Resolve<SearchSpecificationContainer>().ExpressionList;
        }

        /// <summary>
        /// ��� ��������� ���� ���������� ������ ������������ �������, ������� ����� �������������� � �������� ������.
        /// </summary>
        /// <param name="domainType">�������� ���</param>
        /// <exception cref="VMException">"������ ��� �������� ���������� ������ ������������ ������� ������ ��� ����</exception>
        /// <returns>������ ������������ �������</returns>
        internal List<SearchPropertySpec> ResolveSearchPropertySpecs(Type domainType)
        {
            return this.IocContainer.Resolve<SearchSpecificationContainer>().ResolveSearchPropertySpecs(domainType);
        }

        /// <summary>
        /// ���������� ���������� �������� ��� ���������� ��������� ����.
        /// </summary>
        /// <param name="domainType">��� ����������</param>
        /// <exception cref="BLLException">������ ��������� ���������� �������� ��� ����</exception>
        /// <returns>���������� �������� ��� ����������</returns>
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
                throw new BLLException(string.Format("������ ��������� ���������� �������� ��� ���� {0}", domainType.Name), ex);
            }
        }

        #endregion

        #endregion

    }
}