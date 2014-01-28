using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Common.Types.Exceptions;
using Common.UI;
using GU.MZ.Import;
using GU.MZ.UI.View.Import;
using GU.MZ.UI.ViewModel.Attachable.Behavior;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Win32;

namespace GU.MZ.UI.ViewModel.Import 
{
    public class ImportVm : NotificationObject, IExitVm
    {
        private readonly ImportSession _importSession;

        public ImportVm(ImportSession importSession)
        {
            _importSession = importSession;
            IsBusy = false;
            BrowseRegistrFileCommand = new DelegateCommand(BrowseRegistrFile, () => !IsBusy);
            BrowseLogFileCommand = new DelegateCommand(BrowseLogFile, () => !IsBusy);
            StartImportCommand = new DelegateCommand(StartImport, () => !IsBusy);
            CancelImportCommand = new DelegateCommand(CancelImport, () => IsBusy);
            CommitImportCommand = new DelegateCommand(CommitImport, () => !IsBusy && _importSession.NotCommitedSession);
            ExitCommand = new DelegateCommand(ExitForCommand);
            NewLogNameCommand = new DelegateCommand(NewLogName, () => !IsBusy && !_importSession.NotCommitedSession);
            ShowLogCommand = new DelegateCommand(ShowLog, () => !IsBusy);
            NewLogNameCommand.Execute();
        }

        #region Binding Properties

        private string _registrPath;

        public string RegistrPath
        {
            get
            {
                return _registrPath;
            }
            set
            {
                if (_registrPath != value)
                {
                    _registrPath = value;
                    RaisePropertyChanged(() => RegistrPath);
                }
            }
        }

        private string _logPath;

        public string LogPath
        {
            get
            {
                return _logPath;
            }
            set
            {
                if (_logPath != value)
                {
                    _logPath = value;
                    RaisePropertyChanged(() => LogPath);
                }
            }
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    RaisePropertyChanged(() => IsBusy);
                    RaiseCommandsCanExecuteChanged();
                }
            }
        }

        private int _percenatage;

        public int Percentage
        {
            get
            {
                return _percenatage;
            }
            set
            {
                if (_percenatage != value)
                {
                    _percenatage = value;
                    RaisePropertyChanged(() => Percentage);
                }
            }
        }

        private string _message;

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    RaisePropertyChanged(() => Message);
                }
            }
        }

        private bool _closeView;

        public bool CloseView
        {
            get
            {
                return _closeView;
            }
            set
            {
                if (_closeView != value)
                {
                    _closeView = value;
                    RaisePropertyChanged(() => CloseView);
                }
            }
        }

        #endregion

        #region Binding Commands

        public DelegateCommand BrowseRegistrFileCommand { get; set; }

        private void BrowseRegistrFile()
        {
            try
            {
                var dialog = new OpenFileDialog
                {
                    Filter = "Excel files (*.xls)|*.xls",
                    RestoreDirectory = false,
                    CheckFileExists = true,
                    CheckPathExists = true
                };

                var result = dialog.ShowDialog(Application.Current.Windows.OfType<ImportView>().Single());

                if (result.Value)
                {
                    RegistrPath = dialog.FileName;
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

        public DelegateCommand BrowseLogFileCommand { get; set; }

        private void BrowseLogFile()
        {
            try
            {
                var dialog = new SaveFileDialog
                {
                    InitialDirectory = Path.GetDirectoryName(LogPath),
                    Filter = "XML files (*.xml)|*.xml",
                    RestoreDirectory = false,
                    CheckFileExists = true,
                    CheckPathExists = true
                };

                var result = dialog.ShowDialog(Application.Current.Windows.OfType<ImportView>().Single());

                if (result.Value)
                {
                    LogPath = dialog.FileName;
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

        public DelegateCommand StartImportCommand { get; set; }

        private void StartImport()
        {
            try
            {
                if (!File.Exists(RegistrPath))
                {
                    NoticeUser.ShowWarning("Файл реестра не найден.");
                    return;
                }
                
                if (_importSession.NotCommitedSession)
                {
                    var proceed = NoticeUser.ShowQuestionYesNo(
                        "Есть не сохранённые результаты сеанса импорта. Если начать новый сеанс, импортированные данные будут потеряны. Хотите продолжить?", "Новый сеанс импорта");

                    if (proceed == MessageBoxResult.Yes)
                    {
                        _importSession.Cancel();
                    }
                    else
                    {
                        return;
                    }
                }

                IsBusy = true;
                Percentage = 0;
                Message = "Инициализация импорта";

                _importSession.Progress += ImportSessionProgress;
                _importSession.Complete += ImportSessionComplete;
                _importSession.Error += ImportSessionError;

                _importSession.Import(RegistrPath, LogPath);
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

        void ImportSessionComplete(string message)
        {
            Message = message;
            IsBusy = false;
            _importSession.Progress -= ImportSessionProgress;
            _importSession.Complete -= ImportSessionComplete;
            _importSession.Error -= ImportSessionError;
            RaiseCommandsCanExecuteChanged();
        }

        void ImportSessionError(Exception ex)
        {
            NoticeUser.ShowError(ex);
        }

        void ImportSessionProgress(int percentage)
        {
            Percentage = percentage;
            Message = string.Format("Готово на {0}%", percentage);
        }

        public DelegateCommand CommitImportCommand { get; set; }

        private void CommitImport()
        {
            try
            {
                _importSession.CommitImport();
                CommitImportCommand.RaiseCanExecuteChanged();
                Percentage = 0;
                Message = "Данные сохранены";
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

        public DelegateCommand CancelImportCommand { get; set; }

        private void CancelImport()
        {
            try
            {
                var proceed = NoticeUser.ShowQuestionYesNo(
                "Данные текущего сеанса будут потеряны. Хотите прервать сеанс?", "Прерывание сеанса импорта");

                if (proceed != MessageBoxResult.Yes)
                {
                    return;
                }
                _importSession.Cancel();
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

        public DelegateCommand ExitCommand { get; set; }

        private void ExitForCommand()
        {
            if (CanExit())
            {
                Exit();
            }
        }

        public void Exit()
        {
            if (_importSession.NotCommitedSession)
            {
                _importSession.Cancel();
            }
            CloseView = true;
        }

        public bool CanExit()
        {
            if (_importSession.NotCommitedSession)
            {
                var proceed = NoticeUser.ShowQuestionYesNo(
                    "Есть не сохранённые результаты сеанса импорта. Данные не будут сохранены. Хотите продолжить?", "Выход");

                return proceed == MessageBoxResult.Yes;
            }

            return true;
        }

        public DelegateCommand NewLogNameCommand { get; set; }

        private void NewLogName()
        {
            var stamp = DateTime.Now;
            var defaultLogName = string.Format("{0}-{1}-{2}-{3}-{4}-{5}_imp_log.xml",
                stamp.Year,
                stamp.Month,
                stamp.Day,
                stamp.Hour,
                stamp.Minute,
                stamp.Second);
            LogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GU.MZ",
                defaultLogName);
        }

        public DelegateCommand ShowLogCommand { get; set; }

        private void ShowLog()
        {
            if (!File.Exists(LogPath))
            {
                NoticeUser.ShowWarning("Файл протокола импорта не найден.");
                return;
            }
            Process.Start(@"notepad.exe", LogPath);
        }

        private void RaiseCommandsCanExecuteChanged()
        {
            BrowseRegistrFileCommand.RaiseCanExecuteChanged();
            BrowseLogFileCommand.RaiseCanExecuteChanged();
            StartImportCommand.RaiseCanExecuteChanged();
            CancelImportCommand.RaiseCanExecuteChanged();
            CommitImportCommand.RaiseCanExecuteChanged();
            ExitCommand.RaiseCanExecuteChanged();
            NewLogNameCommand.RaiseCanExecuteChanged();
            ShowLogCommand.RaiseCanExecuteChanged();
        }

        #endregion
    }
}
