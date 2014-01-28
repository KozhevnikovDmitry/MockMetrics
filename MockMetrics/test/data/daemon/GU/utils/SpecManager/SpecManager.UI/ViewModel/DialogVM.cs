using System.Windows;
using System.Windows.Controls;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace SpecManager.UI.ViewModel
{
    /// <summary>
    /// Класс, представляющий Модель Представления окна диалога. 
    /// </summary>
    internal class DialogVm : NotificationObject
    {
        /// <summary>
        /// Класс, представляющий Модель Представления окна диалога. 
        /// </summary>
        public DialogVm()
        {
            this.IsOkResult = false;
            this.OkCommand = new DelegateCommand(this.OkMethod);
            this.DialogResizeMode = ResizeMode.NoResize;
            this.DialogSizeToContentMode = SizeToContent.WidthAndHeight;
        }

        /// <summary>
        /// Флаг указывающий на результат диалога.
        /// </summary>
        public bool IsOkResult { get; private set; }

        #region Binding Properties

        /// <summary>
        /// Отображаемое имя рабочей области.
        /// </summary>
        private string _displayName;

        /// <summary>
        /// Возвращает или устанавливает отображаемое имя рабочей области.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                if (_displayName != value)
                {
                    _displayName = value;
                    RaisePropertyChanged(() => DisplayName);
                }
            }
        }

        /// <summary>
        /// View рабочей области.
        /// </summary>
        private UserControl _view;

        /// <summary>
        /// Возвращает или устанавливает View рабочей области.
        /// </summary>
        public UserControl View
        {
            get
            {
                return _view;
            }
            set
            {
                if (_view == null || !_view.Equals(value))
                {
                    _view = value;
                    RaisePropertyChanged(() => View);
                }
            }
        }

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
                return this._showInTaskBar;
            }
            set
            {
                if (this._showInTaskBar != value)
                {
                    this._showInTaskBar = value;
                    this.RaisePropertyChanged(() => this.ShowInTaskBar);
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
                return this._dialogResizeMode;
            }
            set
            {
                if (this._dialogResizeMode != value)
                {
                    this._dialogResizeMode = value;
                    this.RaisePropertyChanged(() => this.DialogResizeMode);
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
                return this._dialogSizeToContentMode;
            }
            set
            {
                if (this._dialogSizeToContentMode != value)
                {
                    this._dialogSizeToContentMode = value;
                    this.RaisePropertyChanged(() => this.DialogSizeToContentMode);
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
                return this._closeView;
            }
            set
            {
                if (this._closeView != value)
                {
                    this._closeView = value;
                    this.RaisePropertyChanged(() => this.CloseView);
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
        private void OkMethod()
        {
            this.IsOkResult = true;
            this.CloseView = true;
        }
        
        #endregion
    }
}
