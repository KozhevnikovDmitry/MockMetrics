using System.Windows;

using Common.UI.ViewModel.Interfaces;

using Microsoft.Practices.Prism.Commands;

namespace Common.UI.ViewModel.WorkspaceViewModel
{
    /// <summary>
    /// Класс, представляющий Модель Представления окна диалога. 
    /// </summary>
    internal class DialogVM : BaseWorkspaceVM, IDialogVM
    {
        /// <summary>
        /// Класс, представляющий Модель Представления окна диалога. 
        /// </summary>
        public DialogVM()
        {
            IsOkResult = false;
            OkCommand = new DelegateCommand(OkMethod);
            DialogResizeMode = ResizeMode.NoResize;
            DialogSizeToContentMode = SizeToContent.WidthAndHeight;
        }

        /// <summary>
        /// Флаг указывающий на результат диалога.
        /// </summary>
        public bool IsOkResult { get; protected set; }

        #region Binding Properties

        /// <summary>
        /// Флаг отображения окна в панели задач
        /// </summary>
        private bool _showInTaskBar;

        /// <summary>
        /// Возвращает или устанавливает флаг отображения окна в панели задач
        /// </summary>
        public bool ShowInTaskBar
        {
            get
            {
                return _showInTaskBar;
            }
            set
            {
                if (_showInTaskBar != value)
                {
                    _showInTaskBar = value;
                    RaisePropertyChanged(() => ShowInTaskBar);
                }
            }
        }

        /// <summary>
        /// Режим изменения размеров окна
        /// </summary>
        private ResizeMode _dialogResizeMode;

        /// <summary>
        /// Возвращает или устанавливает режим изменения размеров окна
        /// </summary>
        public ResizeMode DialogResizeMode
        {
            get
            {
                return _dialogResizeMode;
            }
            set
            {
                if (_dialogResizeMode != value)
                {
                    _dialogResizeMode = value;
                    RaisePropertyChanged(() => DialogResizeMode);
                }
            }
        }

        /// <summary>
        /// Режим привязки размеров к контенту
        /// </summary>
        private SizeToContent _dialogSizeToContentMode;

        /// <summary>
        /// Возвращает или устанавливает режим привязки размеров к контенту
        /// </summary>
        public SizeToContent DialogSizeToContentMode
        {
            get
            {
                return _dialogSizeToContentMode;
            }
            set
            {
                if (_dialogSizeToContentMode != value)
                {
                    _dialogSizeToContentMode = value;
                    RaisePropertyChanged(() => DialogSizeToContentMode);
                }
            }
        }

        /// <summary>
        /// Флаг закрытия диалога.
        /// </summary>
        private bool _closeView;
        
        /// <summary>
        /// Поле привязки для закрытия диалога. 
        /// </summary>
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

        /// <summary>
        /// Команда проставления положительного результата диалога.
        /// </summary>
        public DelegateCommand OkCommand { get; protected set; }

        /// <summary>
        /// Проставляет положительный результата диалога.
        /// </summary>
        protected virtual void OkMethod()
        {
            IsOkResult = true;
            CloseView = true;
        }
        
        #endregion
    }
}
