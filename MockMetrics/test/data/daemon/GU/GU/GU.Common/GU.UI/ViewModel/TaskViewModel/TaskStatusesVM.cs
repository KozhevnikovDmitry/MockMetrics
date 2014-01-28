using System;
using System.Collections.Generic;
using System.ComponentModel;

using Common.Types;
using Common.UI.ViewModel.ListViewModel;
using GU.DataModel;

namespace GU.UI.ViewModel.TaskViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения списка статусов заявки
    /// </summary>
    public class TaskStatusesVM : AbstractListVM<TaskStatus>
    {
        /// <summary>
        /// Класс ViewModel для отображения списка статусов заявки
        /// </summary>
        /// <param name="task">Объект заявка</param>
        public TaskStatusesVM(Task task)
            : base(task.StatusList)
        {
            Title = "История статусов";
        }     

        #region AbstractListVM

        /// <summary>
        /// Устанавливает кастомные настройки списка.
        /// </summary>
        protected override void SetListOptions()
        {
            base.SetListOptions();
            CanAddItems = false;
            CanRemoveItems = false;
            SortDirection = ListSortDirection.Ascending;
            SortProperties.Add(Util.GetPropertyName(() => TaskStatus.CreateInstance().Stamp));
        }

        /// <summary>
        /// Заглушка на метод добавления элемента списка.
        /// </summary>
        protected override void AddItem()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Заглушка на метод удаления элемента из списка.
        /// </summary>
        protected override void RemoveItem()
        {
            throw new NotImplementedException();
        }
        
        #endregion
    }
}
