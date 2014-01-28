using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Win32;

using SpecManager.BL.Interface;
using SpecManager.BL.Model;
using SpecManager.BL.SpecSource;
using SpecManager.UI.View;
using SpecManager.UI.View.SpecSourceView;
using SpecManager.UI.View.SpecView;
using SpecManager.UI.ViewModel.Exceptions;
using SpecManager.UI.ViewModel.SpecSourceViewModel;
using SpecManager.UI.ViewModel.SpecViewModel;

namespace SpecManager.UI.ViewModel
{
    public class MainVm : NotificationObject    
    {
        private readonly ISpecViewFactory _specViewFactory;

        private readonly ISpecSourceFactory _specSourceFactory;

        private readonly Func<DbConfigVm> _dbConfigVmFactory;

        private readonly Func<OpenSpecFromDbVm> _openFromDbVmFactory;

        public MainVm(ISpecViewFactory specViewFactory, 
                      ISpecSourceFactory specSourceFactory,
                      Func<DbConfigVm> dbConfigVmFactory,
                      Func<OpenSpecFromDbVm> openFromDbVmFactory)
        {
            _specViewFactory = specViewFactory;
            _specSourceFactory = specSourceFactory;
            _dbConfigVmFactory = dbConfigVmFactory;
            _openFromDbVmFactory = openFromDbVmFactory;
            this.NewSpecCommand = new DelegateCommand(this.NewSpec);
            OpenFromDbCommand = new DelegateCommand(this.OpenFromDb);
            OpenFromXmlCommand = new DelegateCommand(this.OpenFromXml);
            SaveToDbCommand = new DelegateCommand(this.SaveToDb, () => CanSaveAs);
            SaveToXmlCommand = new DelegateCommand(this.SaveToXml, () => CanSaveAs);
            this.SaveSpecCommand = new DelegateCommand(this.SaveSpec, () => CanSave);
            Workspaces = new ObservableCollection<DockableVm>();
        }

        #region Workspaces's Management

        private void AddDockable(string displayName, UserControl view, INotifyPropertyChanged viewModel)
        {
            view.DataContext = viewModel;
            AddDockable(displayName, view);
        }

        private void AddDockable(SpecDockableVm specDockableVm)
        {
            var specView = new SpecDockableView { DataContext = specDockableVm };
            var dockableVm = new DockableVm { View = specView };
            specDockableVm.NameChanged += name => dockableVm.DisplayName = name;
            specDockableVm.HintChanged += hint => dockableVm.Hint = hint;
            specDockableVm.OnNameChanged();
            specDockableVm.OnHintChanged();
            Workspaces.Add(dockableVm);
            ActiveWorkspace = dockableVm;
        }

        private void AddDockable(string displayName, UserControl view)
        {
            var dockableVm = new DockableVm { View = view, DisplayName = displayName };
            Workspaces.Add(dockableVm);
            ActiveWorkspace = dockableVm;
        }

        #endregion  

        #region Binding Properties

        private ObservableCollection<DockableVm> _workspaces;

        public ObservableCollection<DockableVm> Workspaces
        {
            get
            {
                return _workspaces;
            }
            set
            {
                if (_workspaces == null || !_workspaces.Equals(value))
                {
                    _workspaces = value;
                    RaisePropertyChanged(() => Workspaces);
                }
            }
        }

        private DockableVm _activeWorkspace;

        public DockableVm ActiveWorkspace
        {
            get
            {
                return this._activeWorkspace;
            }
            set
            {
                if (!Equals(value, this._activeWorkspace))
                {
                    this._activeWorkspace = value;
                    this.RaisePropertyChanged(() => this.ActiveWorkspace);
                    this.RaisePropertyChanged(() => this.ActiveSpecDockable);
                    SetupCommandCanExecute();
                }
            }
        }

        private void SetupCommandCanExecute()
        {
           CanSave = CanSaveAs = IsActiveSpecDockable;
        }

        private bool IsActiveSpecDockable
        {
            get
            {
                if (this.ActiveWorkspace != null)
                {
                    var dockableVm = this.ActiveWorkspace;
                    return dockableVm.View.DataContext is SpecDockableVm;
                }

                return false;
            }
        }

        private SpecDockableVm ActiveSpecDockable
        {
            get
            {
                if (IsActiveSpecDockable)
                {
                    return this.ActiveWorkspace.View.DataContext as SpecDockableVm;
                }
                return null;
            }
        }

        #endregion

        #region Binding Commands

        public DelegateCommand NewSpecCommand { get; private set; }

        private void NewSpec()
        {
            try
            {
                var saveFileDialog = new SaveFileDialog
                                         {
                                             RestoreDirectory = true,
                                             Filter = "XML documents (*.xml)|*.xml"
                                         };
                var isOk = saveFileDialog.ShowDialog(Application.Current.MainWindow);
                if (isOk.Value)
                {
                    var specVm = _specViewFactory.GetSpecDockableVm(new Spec { Name = "Новая спека" }, saveFileDialog.FileName);
                    AddDockable(specVm);
                }
                
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        public DelegateCommand OpenFromDbCommand { get; private set; }

        private void OpenFromDb()
        {
            try
            {
                var specFromDbVm = _openFromDbVmFactory();
                var specFromDbView = new OpenSpecFromDbView { DataContext = specFromDbVm };
                var dialogVm = new DialogVm { View = specFromDbView, DisplayName = "Открыть из БД"};
                var dialogView = new DialogView { DataContext = dialogVm, Owner = Application.Current.MainWindow };
                dialogView.ShowDialog();
                if (dialogVm.IsOkResult)
                {
                    AddDockable(GetSpecDockableVm(specFromDbVm));
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        private SpecDockableVm GetSpecDockableVm(OpenSpecFromDbVm specFromDbVm)
        {
            if (specFromDbVm.OpenById)
            {
                return _specViewFactory.GetSpecDockableVm(
                        specFromDbVm.SpecId, specFromDbVm.DbConfigVm.GetConnectionString());
            }
            
            return this._specViewFactory.GetSpecDockableVm(
                specFromDbVm.Uri, specFromDbVm.DbConfigVm.GetConnectionString());
        }

        public DelegateCommand OpenFromXmlCommand { get; private set; }

        private void OpenFromXml()
        {
            try
            {
                var openFileDialog = new OpenFileDialog
                                         {
                                             RestoreDirectory = true,
                                             Multiselect = true,
                                             Filter = "XML documents (*.xml)|*.xml"
                                         };
                var isOk = openFileDialog.ShowDialog(Application.Current.MainWindow);
                if (isOk.Value)
                {
                    foreach (var fileName in openFileDialog.FileNames)
                    {
                        var specVm = _specViewFactory.GetSpecDockableVm(fileName);
                        AddDockable(specVm);
                    }
                }
               
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        public DelegateCommand SaveSpecCommand { get; private set; }

        private void SaveSpec()
        {
            try
            {
                if (ActiveWorkspace != null)
                {
                    var dockableVm = this.ActiveWorkspace;
                    if (dockableVm.View.DataContext is SpecDockableVm)
                    {
                        (dockableVm.View.DataContext as SpecDockableVm).Save();
                    }
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        public DelegateCommand SaveToDbCommand { get; private set; }

        private void SaveToDb()
        {
            try
            {
                var dbSourceVm = _dbConfigVmFactory();
                var dbSourceView = new DbConfigView { DataContext = dbSourceVm };
                var dialogVm = new DialogVm { View = dbSourceView, DisplayName = "Сохранить в БД"};
                var dialogView = new DialogView { DataContext = dialogVm, Owner = Application.Current.MainWindow };
                dialogView.ShowDialog();
                if (dialogVm.IsOkResult)
                {
                    if (IsActiveSpecDockable)
                    {
                        ActiveSpecDockable.SetSource(_specSourceFactory.GetSpecSource(0, dbSourceVm.GetConnectionString()));
                        ActiveSpecDockable.Save();
                    }
                    else
                    {
                        throw new VmException("Нет доступной спеки для сохранения.");
                    }
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        public DelegateCommand SaveToXmlCommand { get; private set; }

        private void SaveToXml()
        {
            try
            {
                var saveFileDialog = new SaveFileDialog
                {
                    RestoreDirectory = true,
                    Filter = "XML documents (*.xml)|*.xml"
                };
                var isOk = saveFileDialog.ShowDialog(Application.Current.MainWindow);
                if (isOk.Value)
                {
                    if (IsActiveSpecDockable)
                    {
                        ActiveSpecDockable.SetSource(_specSourceFactory.GetSpecSource(saveFileDialog.FileName));
                        ActiveSpecDockable.Save();
                    }
                    else
                    {
                        throw new VmException("Нет доступной спеки для сохранения.");
                    }
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        private bool _canSave;

        public bool CanSave
        {
            get
            {
                return this._canSave;
            }
            set
            {
                if (!value.Equals(this._canSave))
                {
                    this._canSave = value;
                    this.RaisePropertyChanged(() => this.CanSave);
                    this.SaveSpecCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private bool _canSaveAs;

        public bool CanSaveAs
        {
            get
            {
                return this._canSaveAs;
            }
            set
            {
                if (!value.Equals(this._canSaveAs))
                {
                    this._canSaveAs = value;
                    this.RaisePropertyChanged(() => this.CanSaveAs);
                    this.SaveToDbCommand.RaiseCanExecuteChanged();
                    this.SaveToXmlCommand.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion
    }
}
