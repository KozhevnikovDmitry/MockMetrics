using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.WeakEvent;
using GU.Trud.BL.Export;
using GU.Trud.BL.Export.Event;
using GU.Trud.BL.Export.Interface;
using GU.Trud.UI.ViewModel.Event;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.Trud.UI.ViewModel
{
    public class ProgressVM : NotificationObject
    {
        private readonly WeakEventListener<PercentageProgressEventArgs> _progressEventListner;

        private readonly IGenerateExportService _generateExportService;

        private bool _isProgressCompleted = false;

        public ProgressVM(IGenerateExportService generateExportService)
        {
            _generateExportService = generateExportService;
            _progressEventListner = new WeakEventListener<PercentageProgressEventArgs>(OnProgressed);
            PercentageProgressWeakEventManager.AddListener(_generateExportService, _progressEventListner);
            ExitExportCommand = new DelegateCommand(ExitExport, () => _canExitExport);
            ExitCommandTitle = "Отмена";
            Percentage = 5;
            DisplayMessage = "Инициация экспорта";
        }
        
        private void OnProgressed(object sender, PercentageProgressEventArgs e)
        {
            Percentage = e.Percentage;
            DisplayMessage = e.Message;
            if (Percentage == 100)
            {
                _isProgressCompleted = true;
                ExitCommandTitle = "ОК";
            }
        }

        #region Binding Properties

        private int _percentage;

        public int Percentage
        {
            get
            {
                return _percentage;
            }
            set
            {
                if (_percentage != value)
                {
                    _percentage = value;
                    RaisePropertyChanged(() => Percentage);
                }
            }
        }

        private string _displayMessage;

        public string DisplayMessage
        {
            get
            {
                return _displayMessage;
            }
            set
            {
                if (_displayMessage != value)
                {
                    _displayMessage = value;
                    RaisePropertyChanged(() => DisplayMessage);
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

        private string _title;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    RaisePropertyChanged(() => Title);
                }
            }
        }

        private string _exitCommandTitle;

        public string ExitCommandTitle
        {
            get
            {
                return _exitCommandTitle;
            }
            set
            {
                if (_exitCommandTitle != value)
                {
                    _exitCommandTitle = value;
                    RaisePropertyChanged(() => ExitCommandTitle);
                }
            }
        }


        #endregion

        #region Binding Commands

        public DelegateCommand ExitExportCommand { get; protected set; }

        private void ExitExport()
        {
            try
            {
                if (!_isProgressCompleted)
                    _generateExportService.Cancel();
                CloseView = true;
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка", ex));
            }
        }

        private bool _canExitExport = true;

        #endregion

    }
}
