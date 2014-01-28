using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Common.BL.DataMapping;
using Common.BL.ReportMapping;
using Common.BL.Validation;
using Common.DA;
using Common.DA.Interface;
using Common.Types.Exceptions;
using Common.UI.Interface;
using Common.UI.Report;
using Common.UI.View;
using Common.UI.ViewModel;
using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.ValidationViewModel;
using Common.UI.ViewModel.WorkspaceViewModel;
using Common.UI.Views;
using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI
{
    public class UiFactory : IDockableDialogFactory, IListDialogUiFactory, ISearchDialogFactory, IValidateableDialogUiFactory
    {
        protected readonly ILifetimeScope _context;

        public UiFactory(ILifetimeScope context)
        {
            _context = context;
        }

        #region IListVmUiFactory

        public virtual IListItemVM<T> GetListItemVm<T>(T item) where T : IDomainObject
        {
            try
            {
                return this._context.Resolve<IListItemVM<T>>(new NamedParameter("entity", item), new NamedParameter("isValidateable", true));
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format(" ласс IListItemVM<{0}> не зарегистрирован в контейнере", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("ќшибка при создании экземпл€ра типа IListItemVM<{0}>", typeof(T).Name), ex);
            }
        }

        #endregion


        #region IDockableUiFactory

        public ISearchVM<T> GetSearchVm<T>() where T : IPersistentObject
        {
            try
            {
                return _context.Resolve<ISearchVM<T>>();
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format(" ласс ISearchVM<{0}> не зарегистрирован в контейнере", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("ќшибка при создании экземпл€ра типа ISearchVM<{0}>", typeof(T).Name), ex);
            }
        }

        public ISearchVM GetSearchVmType(Type domainType)
        {
            try
            {
                var searchVmType = typeof(ISearchVM<>).MakeGenericType(domainType);
                return (ISearchVM)_context.Resolve(searchVmType);
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format(" ласс ISearchVM<{0}> не зарегистрирован в контейнере", domainType.Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("ќшибка при создании экземпл€ра типа ISearchVM<{0}>", domainType.Name), ex);
            }
        }

        public virtual IEditableVM<T> GetEditableVm<T>(T entity, bool isEditable = true) where T : DomainObject<T>, IPersistentObject
        {
            try
            {
                return _context.Resolve<IEditableVM<T>>(new NamedParameter("entity", entity),
                    new NamedParameter("isEditable", isEditable));
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format(" ласс IEditableVM<{0}> не зарегистрирован в контейнере", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("ќшибка при создании экземпл€ра типа IEditableVM<{0}>", typeof(T).Name), ex);
            }
        }

        public IEditableVM GetEditableVm(Type domainType, IDomainObject entity, bool isEditable = true)
        {
            try
            {
                MethodInfo method = GetType().GetMethods().Single(t => t.Name == "GetEditableVm" && t.IsGenericMethod)
                               .MakeGenericMethod(new[] { domainType });

                return (IEditableVM)method.Invoke(this, new object[] { entity, isEditable });
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format(" ласс IEditableVM<{0}> не зарегистрирован в контейнере", domainType.Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("ќшибка при создании экземпл€ра типа IEditableVM<{0}>", domainType.Name), ex);
            }
        }

        public IEditableVM GetEditableVm(Type domainType, string entityKey, bool isEditable = true)
        {
            try
            {
                MethodInfo method = GetType().GetMethod("GetEditableVmByKey")
                               .MakeGenericMethod(new[] { domainType });
                return (IEditableVM)method.Invoke(this, new object[] { entityKey, isEditable });
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format(" ласс IEditableVM<{0}> не зарегистрирован в контейнере", domainType.Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("ќшибка при создании экземпл€ра типа IEditableVM<{0}>", domainType.Name), ex);
            }
        }

        public IEditableVM GetEditableVmByKey<T>(string entityKey, bool isEditable = true) where T : DomainObject<T>, IPersistentObject
        {
            try
            {
                var mapper = _context.Resolve<IDomainDataMapper<T>>();
                var entity = mapper.Retrieve(entityKey);
                return GetEditableVm(entity, isEditable);
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format(" ласс IEditableVM<{0}> не зарегистрирован в контейнере", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("ќшибка при создании экземпл€ра типа IEditableVM<{0}>", typeof(T).Name), ex);
            }
        }

        public UserControl GetEditableView(Type domainType)
        {
            try
            {
                return _context.ResolveNamed<UserControl>(domainType.FullName);
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(" ласс не зарегистрирован в контейнере", ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException("ќшибка при создании экземпл€ра типа", ex);
            }
        }

        public UserControl GetReportPresenter(IReport report)
        {
#if DEBUG
            var isDesigner = true;
#else
            var isDesigner = false;
#endif
            var data = report.RetrieveData();
            return new StiReportEngine().GetReportPresenter(report.ViewPath, report.DataAlias, data, isDesigner);
        }

        #endregion


        #region IValidateableUiFactory

        public virtual IDomainValidateableVM<T> GetDomainValidateableVm<T>(T entity) where T : IDomainObject
        {
            try
            {
                return _context.Resolve<IDomainValidateableVM<T>>(new NamedParameter("entity", entity), new NamedParameter("isValidateable", true));
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format(" ласс IDomainValidateableVM<{0}> не зарегистрирован в контейнере", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("ќшибка при создании экземпл€ра типа IDomainValidateableVM<{0}>", typeof(T).Name), ex);
            }
        }

        public void ShowValidationErrorsView(ValidationErrorInfo validationErrorInfo)
        {
            ShowToolView(new ValidationsView(), new ValidationsVM(validationErrorInfo.Errors), "ќшибочно заполненные пол€");
        }

        #endregion


        #region Resolve View and ViewModel


        public ISearchResultVM<T> GetSearchResultVm<T>(T result) where T : IDomainObject
        {
            try
            {
                return _context.Resolve<ISearchResultVM<T>>(new NamedParameter("entity", result));
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format(" ласс ISearchResultVM<{0}> не зарегистрирован в контейнере", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("ќшибка при создании экземпл€ра типа ISearchResultVM<{0}>", typeof(T).Name), ex);
            }
        }



        public IAvalonDockVM GetAvalonDockVM<T>() where T : BaseAvalonDockVM
        {
            try
            {
                return _context.Resolve<T>();
            }
            catch (ComponentNotRegisteredException ex)
            {
                throw new VMException(string.Format(" ласс {0} не зарегистрирован в контейнере", typeof(T).Name), ex);
            }
            catch (DependencyResolutionException ex)
            {
                throw new VMException(string.Format("ќшибка при создании экземпл€ра типа {0}", typeof(T).Name), ex);
            }
        }

        public ISearchTemplateVM GetSearchTemplateVM(UserControl searchTemplateView,
                                                        INotifyPropertyChanged searchTemplateVm,
                                                        IDomainObject searchObject,
                                                        string displayName,
                                                        int templateWidth = 400)
        {
            var factory = new WorkspaceFactory();
            return factory.CreateSearchTemplateVM(searchTemplateView, searchTemplateVm, searchObject, displayName, templateWidth);
        }

        #endregion


        #region IDialogUiFactory

        public void ShowToolView(UserControl view,
            NotificationObject viewModel,
            string displayName)
        {
            try
            {
                view.DataContext = viewModel;
                var toolVm = new ToolVM { View = view, DisplayName = displayName };
                var toolView = new ToolView { DataContext = toolVm, Owner = Application.Current.MainWindow };
                toolView.ShowDialog();
            }
            catch (GUException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new VMException("ќшибка при создании диалога.", ex);
            }
        }

        public bool ShowDialogView(UserControl view,
            INotifyPropertyChanged viewModel,
            string displayName,
            ResizeMode resizeMode = ResizeMode.NoResize,
            SizeToContent sizeToContent = SizeToContent.WidthAndHeight,
            bool showInTaskbar = false)
        {
            var factory = new WorkspaceFactory();
            var dialog = factory.CreateDialogView(view, viewModel, displayName, resizeMode, sizeToContent, showInTaskbar);
            dialog.ShowDialog();
            return (dialog.DataContext as IDialogVM).IsOkResult;
        }

        public bool ShowValidateableDialogView(UserControl view,
            IValidateableVM viewModel,
            string displayName,
            ResizeMode resizeMode = ResizeMode.NoResize,
            SizeToContent sizeToContent = SizeToContent.WidthAndHeight)
        {
            var factory = new WorkspaceFactory();
            var dialog = factory.CreateValidateableDialogView(view, viewModel, displayName, resizeMode, sizeToContent);
            dialog.ShowDialog();
            return (dialog.DataContext as IDialogVM).IsOkResult;
        }

        public bool ShowConfirmableDialogView(UserControl view,
            IConfirmableVM viewModel,
            string displayName,
            ResizeMode resizeMode = ResizeMode.NoResize,
            SizeToContent sizeToContent = SizeToContent.WidthAndHeight,
            bool showInTaskbar = false)
        {
            var factory = new WorkspaceFactory();
            var dialog = factory.CreateConfirmableDialogView(view, viewModel, displayName, resizeMode: resizeMode, sizeToContent: sizeToContent, showInTaskbar: showInTaskbar);
            dialog.ShowDialog();
            return (dialog.DataContext as IDialogVM).IsOkResult;
        }

        public bool ShowSearchDialogView(ISearchVM viewModel,
            string displayName,
            ResizeMode resizeMode = ResizeMode.NoResize,
            SizeToContent sizeToContent = SizeToContent.WidthAndHeight,
            Size? size = null,
            Size? minSize = null)
        {
            var factory = new WorkspaceFactory();
            var dialog = factory.CreateSearchDialogView(viewModel, displayName, resizeMode, sizeToContent, size, minSize);
            dialog.ShowDialog();
            return (dialog.DataContext as IDialogVM).IsOkResult;
        }

        #endregion
    }
}