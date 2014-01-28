using System;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.Interfaces;
using GU.BL;
using GU.DataModel;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.TaskViewModel
{

    /// <summary>
    /// Класс View Model для окна отклонения заявки
    /// </summary>
    public class RejectTaskVM : NotificationObject, IConfirmableVM
    {
        /// <summary>
        /// Отклоняемая заявка.
        /// </summary>
        public Task Task { get; private set; }

        /// <summary>
        /// Класс View Model для окна отклонения заявки
        /// </summary>
        /// <param name="task">Заявка</param>
        public RejectTaskVM(Task task)
        {
            this.Task = task;
            this.Task.AcceptChanges();
        }

        #region Binding Properties

        /// <summary>
        /// Возвращает дату подачи заявки.
        /// </summary>
        public string TaskStamp{get{return Task.CreateDate.HasValue ? this.Task.CreateDate.Value.ToLongDateString() : "Неизвестно";}}

        /// <summary>
        /// Возвращает наименование услуги указанной в заявке.
        /// </summary>
        public string ServiceName{get{return Task.Service.Name;}}

        /// <summary>
        /// Вовзвращает имя заявителя.
        /// </summary>
        public string TaskHolderName{get{return this.Task.CustomerFio;}}

        /// <summary>
        /// Комментарий к отклонению.
        /// </summary>
        private string _rejectNotice;

        /// <summary>
        /// Возвращает или устанавливает комментарий к отклонению.
        /// </summary>
        public string RejectNotice
        {
            get
            {
                return this._rejectNotice;
            }
            set
            {
                if (this._rejectNotice != value)
                {
                    this._rejectNotice = value;
                    RaisePropertyChanged(() => this.RejectNotice);
                }
            }
        }

        #endregion


        #region IConfirmableVM

        /// <summary>
        /// Проводит процедуру подтверждения диалога.
        /// </summary>
        public void Confirm()
        {
            try
            {
                //Отклоняем заявку 
                var taskPolicy = GuFacade.GetTaskPolicy();
                taskPolicy.SetStatus(TaskStatusType.Rejected, _rejectNotice, Task);

                Task = GuFacade.GetDataMapper<Task>().Save(Task);

                IsConfirmed = true;
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденна ошибка", ex));
            }
        }

        /// <summary>
        /// Восстанавливает состояние после ошибки в процедуре подтверждения диалога.
        /// </summary>
        public void ResetAfterFail()
        {
            this.Task.RejectChanges();
        }

        /// <summary>
        /// Флаг подтвержденённости диалога. 
        /// </summary>
        public bool IsConfirmed { get; private set; }

        #endregion
    }
}
