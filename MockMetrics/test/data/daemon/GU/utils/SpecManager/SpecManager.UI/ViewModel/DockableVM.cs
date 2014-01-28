using System.Windows.Controls;

using Microsoft.Practices.Prism.ViewModel;

namespace SpecManager.UI.ViewModel
{
    /// <summary>
    /// Класс представлющий Модель Представления рабочей области AvalonDock конрола DockingManager.
    /// </summary>
    public class DockableVm : NotificationObject
    {
        /// <summary>
        /// Класс представлющий Модель Представления рабочей области AvalonDock конрола DockingManager.
        /// </summary>
        public DockableVm()
        {
            
        }

        #region Binding Properties

        /// <summary>
        /// Отображаемое имя рабочей области.
        /// </summary>
        private string _displayName;

        /// <summary>
        /// Возвращает или устанавливает отображаемое имя рабочей области.
        /// </summary>
        public virtual string DisplayName
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

        private string _hint;

        /// <summary>
        /// Возвращает или устанавливает отображаемое имя рабочей области.
        /// </summary>
        public string Hint
        {
            get
            {
                return _hint;
            }
            set
            {
                if (_hint != value)
                {
                    _hint = value;
                    RaisePropertyChanged(() => Hint);
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

        #endregion

    }
}
