using System.Windows.Controls;

using Common.UI.ViewModel.Interfaces;

using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI.ViewModel.WorkspaceViewModel
{
    /// <summary>
    /// Базовый класс Модели Представления рабочей области.
    /// </summary>
    public abstract class BaseWorkspaceVM : NotificationObject, IWorkspaceVM
    {
        #region Binding Properties

        /// <summary>
        /// Отображаемое имя рабочей области.
        /// </summary>
        protected string _displayName;

        /// <summary>
        /// View рабочей области.
        /// </summary>
        protected UserControl _view;

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
                if (_displayName == null || !_displayName.Equals(value))
                {
                    _view = value;
                    RaisePropertyChanged(() => View);
                }
            }
        }

        #endregion
    }
}
