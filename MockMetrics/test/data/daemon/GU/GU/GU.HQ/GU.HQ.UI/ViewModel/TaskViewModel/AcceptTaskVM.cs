using System;

using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.Interfaces;
using GU.BL;
using GU.DataModel;
using GU.HQ.BL;
using GU.HQ.BL.DomainLogic.AcceptTask.Interface;
using GU.HQ.DataModel;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.TaskViewModel
{

    /// <summary>
    /// Класс View Model для окна принятия заявления к рассмотрению
    /// </summary>
    public class AcceptTaskVM : NotificationObject, IConfirmableVM
    {
        /// <summary>
        /// Заявка, принимаемая к рассмотрению.
        /// </summary>
        private readonly Task _task;

        /// <summary>
        /// Заявка в предметной области HQ
        /// </summary>
        public Claim Claim { get; private set; }

        private ITaskDataParser _taskParser;

        /// <summary>
        /// Класс View Model для окна принятия заявления к рассмотрению
        /// </summary>
        /// <param name="task">Заявка</param>
        public AcceptTaskVM(Task task, ITaskDataParser taskParser)
        {
            _task = task;
            _taskParser = taskParser;
            _task.AcceptChanges();
        }

        #region Binding Properties

        /// <summary>
        /// Возвращает дату подачи заявки.
        /// </summary>
        public string TaskStamp { get {return _task.CreateDate.Value.ToLongDateString(); } }

        /// <summary>
        /// Возвращает наименование услуги указанной в заявке.
        /// </summary>
        public string ServiceName {  get { return _task.Service.Name; } }

        /// <summary>
        /// Вовзвращает имя заявителя.
        /// </summary>
        public string TaskHolderName { get { return _task.CustomerFio; } }
        
        /// <summary>
        /// Комментарий к принятию.
        /// </summary>
        private string _acceptNotice;

        /// <summary>
        /// Возвращает или устанавливает комментарий к принятию.
        /// </summary>
        public string AcceptNotice
        {
            get
            {
                return _acceptNotice;
            }
            set
            {
                if (_acceptNotice != value)
                {
                    _acceptNotice = value;
                    RaisePropertyChanged(() => AcceptNotice);
                }
            }
        }
        
        #endregion Binding Properties

        
        
        #region IConfirmableVM

        /// <summary>
        /// Проводит процедуру подтверждения диалога.
        /// </summary>
        /// TODO: Было бы не плохо перенести в BL сборку
        public void Confirm()
        {
            try
            {
                // save task state before change
                var task = _task.Clone();

                // change Task state
                var taskPolicy = GuFacade.GetTaskPolicy();
                taskPolicy.SetStatus(TaskStatusType.Accepted, _acceptNotice, task);

                // create Claim
                Claim = _taskParser.GetClaim(task);
                Claim = HqFacade.GetDataMapper<Claim>().Save(Claim);

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
            _task.RejectChanges();
        }

        /// <summary>
        /// Флаг подтвержденённости диалога. 
        /// </summary>
        public bool IsConfirmed { get; private set; }
        
        #endregion
    }
}
