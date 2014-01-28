using System;
using Common.BL.DataMapping;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.Interfaces;

using GU.DataModel;
using GU.MZ.BL.DomainLogic.AcceptTask;
using GU.MZ.BL.DomainLogic.AcceptTask.AcceptException;
using GU.MZ.DataModel.Dossier;

using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.TaskViewModel
{

    /// <summary>
    /// Класс View Model для окна отклонения заявки
    /// </summary>
    public class RejectTaskVm : NotificationObject, IConfirmableVM
    {
        /// <summary>
        /// Отклоняемая заявка.
        /// </summary>
        public Task Task { get; private set; }

        private readonly IDomainDataMapper<Task> _taskMapper;

        /// <summary>
        /// Конфигуратор тома лицензионного дела.
        /// </summary>
        private readonly DossierFileBuilder _builder;

        /// <summary>
        /// Класс View Model для окна отклонения заявки
        /// </summary>
        /// <param name="taskMapper"></param>
        /// <param name="builder">Сборщик тома лицензионного дела</param>
        public RejectTaskVm(IDomainDataMapper<Task> taskMapper, DossierFileBuilder builder)
        {
            _taskMapper = taskMapper;
            _builder = builder;
        }

        public void Initialize(Task task)
        {
            Task = task;
            Task.AcceptChanges();
        }

        #region Binding Properties

        /// <summary>
        /// Возвращает дату подачи заявки.
        /// </summary>
        public string TaskStamp
        {
            get
            {
                return Task.CreateDate.HasValue ? Task.CreateDate.Value.ToLongDateString() : "Неизвестно";
            }
        }

        /// <summary>
        /// Возвращает наименование услуги указанной в заявке.
        /// </summary>
        public string ServiceName
        {
            get
            {
                return Task.Service.Name;
            }
        }

        /// <summary>
        /// Вовзвращает имя заявителя.
        /// </summary>
        public string TaskHolderName
        {
            get
            {
                return Task.CustomerFio;
            }
        }

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
                return _rejectNotice;
            }
            set
            {
                if (_rejectNotice != value)
                {
                    _rejectNotice = value;
                    RaisePropertyChanged(() => RejectNotice);
                }
            }
        }

        #endregion

        #region Binding Commands

        #endregion

        #region IConfirmableVM

        /// <summary>
        /// Проводит процедуру подтверждения диалога.
        /// </summary>
        public void Confirm()
        {
            try
            {
                DossierFile dossierFile = null;
                try
                {
                    dossierFile = _builder.FromTask(Task)
                                          .WithRejectedStatus(RejectNotice)
                                          .Create();
                }
                catch (BuildingDataNotCompleteException)
                {
                    NoticeUser.ShowWarning("Необходимо оставить комментарий поясняющий причины отклонения заявки.");
                    IsConfirmed = false;
                    return;
                }

                Task = _taskMapper.Save(dossierFile.Task);
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
            Task.RejectChanges();
        }

        /// <summary>
        /// Флаг подтвержденённости диалога. 
        /// </summary>
        public bool IsConfirmed { get; private set; }

        #endregion
    }
}
