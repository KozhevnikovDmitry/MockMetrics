using System;

using Common.UI.ViewModel.Interfaces;

namespace Common.UI.ViewModel.WorkspaceViewModel
{
    /// <summary>
    /// Базовый класс для классов ViewModel для вкладки с рабочей областью редактирования доменного объекта.
    /// </summary>
    public class EditableDockableVM : DockableVM
    {
        /// <summary>
        /// Базовый класс для классов ViewModel для вкладки с рабочей областью редактирования доменного объекта.
        /// </summary>
        /// <param name="editableVM">ViewModel рабочей области редактирования доменного объекта</param>
        public EditableDockableVM(IEditableVM editableVM)
        {
            EditableDataContext = editableVM;
            EditableDataContext.IsDirtyChanged += this.OnEditableDataContextIsDirtyChanged;
            EditableDataContext.DisplayNameChanged += new Action<string>(this.OnEditableDataContextDisplayNameChanged);
        }

        /// <summary>
        /// Выставляет новое отображаемое имя для вкладыки.
        /// </summary>
        /// <param name="newDisplayName">Новое отображаемое имя</param>
        private void OnEditableDataContextDisplayNameChanged(string newDisplayName)
        {
            DisplayName = newDisplayName;
            RaisePropertyChanged(() => DisplayName);
        }

        /// <summary>
        /// Меняет отображаемое имя вкладки по изменению IsDirty редактируемого объекта.
        /// </summary>
        public void OnEditableDataContextIsDirtyChanged()
        {
            RaisePropertyChanged(() => DisplayName);
        }

        /// <summary>
        /// ViewModel рабочей области редактирования доменного объекта.
        /// </summary>
        private IEditableVM _editableDataContext;

        /// <summary>
        /// Возвращает или устанавливает ViewModel рабочей области редактирования доменного объекта.
        /// </summary>
        public IEditableVM EditableDataContext 
        {
            get
            {
                return _editableDataContext;
            }
            set
            {
                if (_editableDataContext == null || !_editableDataContext.Equals(value))
                {
                    if (_editableDataContext != null)
                    {
                        _editableDataContext.IsDirtyChanged -= this.OnEditableDataContextIsDirtyChanged;
                        _editableDataContext.DisplayNameChanged -= this.OnEditableDataContextDisplayNameChanged;
                    }
                    _editableDataContext = value;
                    if(View != null) View.DataContext = _editableDataContext;
                    _editableDataContext.IsDirtyChanged += new Action(this.OnEditableDataContextIsDirtyChanged);
                    _editableDataContext.DisplayNameChanged += new Action<string>(this.OnEditableDataContextDisplayNameChanged);
                    RaisePropertyChanged(() => EditableDataContext);
                }
            }
        }

        /// <summary>
        /// Возвращает или устанавливает отображаемое имя вкладки.
        /// </summary>
        public override string DisplayName
        {
            get
            {
                string result = _displayName;
                if (EditableDataContext.IsDirty) 
                {
                    result += "*";
                }
                return result;
            }
            set
            {
                base.DisplayName = value;
            }
        }
    }
}
