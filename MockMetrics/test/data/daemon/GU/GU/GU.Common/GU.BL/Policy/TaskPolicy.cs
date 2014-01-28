using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Common.BL.Exceptions;
using Common.BL.Validation;
using Common.Types.Exceptions;

using GU.BL.Extensions;
using GU.BL.Policy.Interface;
using GU.DataModel;

namespace GU.BL.Policy
{
    public class TaskPolicy : ITaskPolicy
    {
        private readonly DbUser _dbUser;

        private readonly IContentPolicy _contentPolicy;

        private static readonly Dictionary<TaskStatusType, TaskStatusType[]> 
            _validStatusTransitions = new Dictionary<TaskStatusType, TaskStatusType[]>()
            {
                {TaskStatusType.None, new[] { TaskStatusType.NotFilled, TaskStatusType.Rejected }},
                {TaskStatusType.NotFilled, new[] { TaskStatusType.CheckupWaiting, TaskStatusType.Rejected }},
                {TaskStatusType.CheckupWaiting, new[] { TaskStatusType.Accepted, TaskStatusType.Rejected }},
                {TaskStatusType.Accepted, new[] { TaskStatusType.Working, TaskStatusType.Rejected }},
                {TaskStatusType.Working, new[] { TaskStatusType.Ready, TaskStatusType.Rejected }},
                {TaskStatusType.Ready, new[] { TaskStatusType.Done, TaskStatusType.Rejected }}
            };

        private static readonly Random _random = new Random(); //для генерации кода авторизации

        public TaskPolicy(DbUser dbUser, IContentPolicy contentPolicy)
        {
            _dbUser = dbUser;
            _contentPolicy = contentPolicy;
        }

        #region State management


        public Task SetStatus(TaskStatusType taskStatusType, string comment, Task task)
        {
            var validationResult = ValidateSetStatus(taskStatusType, task);
            if (!validationResult.IsValid)
                throw new DomainBLLException(validationResult.ToString(), task);

            DateTime stamp = DateTime.Now;

            // доп. действия при окончании заведения заявки
            if (taskStatusType == TaskStatusType.CheckupWaiting)
            {
                task.CreateDate = stamp;

                // заполнение предполагаемой даты окончания (дата создания + макс. кол-во дней услуги с учетом рабочих дней и округлением до дней вверх)
                if (task.CreateDate.HasValue && task.Service.MaxDuration.HasValue)
                    task.DueDate = task.CreateDate.Value.AddWorkingDays(task.Service.MaxDuration.Value).Ceil(new TimeSpan(1, 0, 0, 0));

                //заполнение кода авторизации чтобы можно было посмотреть заявку на сайте
                task.AuthCode = GenerateAuthCode();
            }
            else if (taskStatusType == TaskStatusType.Done)
            {
                task.CloseDate = stamp;
            }
            else if (taskStatusType == TaskStatusType.Rejected)
            {
                task.CloseDate = stamp;
            }

            task.CurrentState = taskStatusType;

            TaskStatus ts = TaskStatus.CreateInstance();
            ts.Stamp = stamp;
            ts.Task = task;
            ts.TaskId = task.Id;
            ts.UserId = _dbUser.Id;
            ts.User = _dbUser;
            ts.State = taskStatusType;
            ts.Note = comment;
            task.StatusList.Add(ts);

            return task;
        }

        public bool CanSetStatus(TaskStatusType taskStatusType, Task task)
        {
            return ValidateSetStatus(taskStatusType, task).IsValid;
        }

        public bool CanSetAgency(Agency agency, Task task)
        {
            return true;
        }

        public ValidationErrorInfo ValidateSetStatus(TaskStatusType taskStatusType, Task task)
        {
            if (!IsValidStatusTransition(task.CurrentState, taskStatusType))
                throw new DomainBLLException("Неверный статус заявки", task);

            var validationResult = new ValidationErrorInfo();
            if (task.CurrentState != TaskStatusType.None)
            {
                validationResult.AddResult(ValidateBasic(task));
            }

            if (taskStatusType == TaskStatusType.CheckupWaiting)
            {
                validationResult.AddResult(ValidateDocumentsData(task));
            }

            return validationResult;
        }

        public bool IsValidStatusTransition(TaskStatusType oldStatusType, TaskStatusType newStatusType)
        {
            if (!_validStatusTransitions.ContainsKey(oldStatusType))
                return false;

            return _validStatusTransitions[oldStatusType].Contains(newStatusType);
        }

        #endregion

        #region Validation

        public ValidationErrorInfo ValidateDocumentsData(Task task)
        {
            var result = new ValidationErrorInfo();
            
            if (task.Content == null)
                throw new DomainBLLException("Отсутствуют данные заявки", task);

            _contentPolicy.Validate(task.Content);

            /* TODO: obsolete
            foreach (var doc in task.TaskDocList)
            {
                foreach (var docSect in doc.TaskDocSectList)
                {
                    foreach (var attr in docSect.TaskAttrList)
                    {
                        result.AddResult(new TaskAttrPolicy(attr).Validate());
                    }
                }
            }*/
            return result;
        }

        private ValidationErrorInfo ValidateFieldRegex(string fieldName, string value, bool isRequired, string fieldRegexFormat, string formatMessage)
        {
            var result = new ValidationErrorInfo();

            if (string.IsNullOrEmpty(value))
            {
                if (isRequired)
                    result.AddError(String.Format("Поле \"{0}\" не заполнено", fieldName));
            }
            else
            {
                if (!Regex.IsMatch(value, fieldRegexFormat))
                    result.AddError(String.Format(formatMessage, fieldName));
            }
            return result;
        }
        
        public ValidationErrorInfo ValidateBasic(Task task)
        {
            //TODO: проверка этих полей есть и тут и в валидаторах, надо бы объединить
            // в customerFio может быть и название организации, поэтому нельзя оставлять только русские символы
            const string customerFioRegexFormat = @"^.{1,500}$"; //@"^[а-яА-Я\s\-\.]{1,500}$";
            const string customerPhoneRegexFormat = @"^\+?[\d\-\(\)]{7,20}$";
            const string customerEmailRegexFormat = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

            const string customerFioFormatMessage = "Максимальная длина поля {0} 500 символов"; //"Поле {0} может содержать только русские символы";
            const string customerPhoneFormatMessage = "Поле {0} должно быть заполнено корректно, например 8(902)123-1234 или 2-123-123";
            const string customerEmailFormatMessage = "Поле {0} должно быть заполнено корректно, например username@example.org";

            var result = new ValidationErrorInfo();
            result.AddResult(ValidateFieldRegex("ФИО заявителя", task.CustomerFio, true, customerFioRegexFormat, customerFioFormatMessage));
            result.AddResult(ValidateFieldRegex("Телефон заявителя", task.CustomerPhone, false, customerPhoneRegexFormat, customerPhoneFormatMessage));
            result.AddResult(ValidateFieldRegex("Email заявителя", task.CustomerEmail, false, customerEmailRegexFormat, customerEmailFormatMessage));

            return result;
        }

        public ValidationErrorInfo Validate(Task task)
        {
            var result = ValidateBasic(task);

            // для начальных статусов не проверяется заполеннность документов
            if (task.CurrentState != TaskStatusType.NotFilled &&
               task.CurrentState != TaskStatusType.None)
            {
                result.AddResult(ValidateDocumentsData(task));
            }
            return result;
        }

        public ValidationErrorInfo CanSave(Task task)
        {
            return Validate(task);
        }

        #endregion

        /// <summary>
        /// Создание пустой заявки
        /// </summary>
        /// <param name="service">Оказываемая услуга</param>
        /// <param name="agency">
        ///     Ведомство, которому направляется заявка.
        ///     Должно быть подведомством service.ServiceGroup.Agency
        ///     Если null, то в заявке используется service.ServiceGroup.Agency
        /// </param>
        /// <returns>Созданная заявка</returns>
        public Task CreateEmptyTask(Service service, Agency agency = null)
        {
            if (service == null) throw new ArgumentNullException("service");

            Task task = Task.CreateInstance();
            task.ServiceId = service.Id;
            task.Service = service;
            if (agency == null)
            {
                task.AgencyId = service.ServiceGroup.AgencyId;
                task.Agency = service.ServiceGroup.Agency;
            }
            else
            {
                if (!AgencyPolicy.IsSubAgency(service.ServiceGroup.Agency, agency.Id))
                    throw new DomainBLLException("Ведомство заявки не является подчиненным ведомству услуги", task, task.Service.ServiceGroup, agency);
                task.AgencyId = agency.Id;
                task.Agency = agency;
            }
            // дата создания проставляется когда заявка будет полностью заполнена (переведена в след. статус)
            task.CreateDate = null;
            task.CurrentState = TaskStatusType.None;

            task.StatusList = new BLToolkit.EditableObjects.EditableList<TaskStatus>();
            this.SetStatus(TaskStatusType.NotFilled, string.Empty, task);

            /* TODO: obsolete
            task.TaskDocList = new BLToolkit.EditableObjects.EditableList<TaskDoc>();
             */

            task.AcceptChanges();

            return task;
        }

        /// <summary>
        /// Создание заявки c дефолтовым заявлением
        /// </summary>
        /// <param name="service">Оказываемая услуга</param>
        /// <param name="agency">
        ///     Ведомство, которому направляется заявка.
        ///     Должно быть подведомством service.ServiceGroup.Agency
        ///     Если null, то в заявке используется service.ServiceGroup.Agency
        /// </param>
        /// <returns>Созданная заявка</returns>
        public Task CreateDefaultTask(Service service, Agency agency = null)
        {
            var task = CreateEmptyTask(service, agency);

            if (task.Service.Spec != null)
                task.Content = _contentPolicy.CreateDefault(task.Service.Spec);

            /* TODO: obsolete
            //создание обязательных к заполнению документов заявки с дефолтовыми значениями
            var docSpecList = service.DocSpecList
                .Where(t => t.IsRequired)
                .OrderBy(t => t.Order).ThenBy(t => t.Name);

            foreach (var docSpec in docSpecList)
                this.AddDefaultDoc(docSpec, task);*/

            task.AcceptChanges();

            return task;
        }

        private String GenerateAuthCode()
        {
            const int authCodeLen = 8;
            const string chars = "123456789";

            char[] buffer = new char[authCodeLen];

            for (int i = 0; i < authCodeLen; i++)
                buffer[i] = chars[_random.Next(chars.Length)];
            
            return new string(buffer);
        }
        
        public bool IsEditable(Task task)
        {
            return task.CurrentState != TaskStatusType.Done && task.CurrentState != TaskStatusType.Rejected;
        }
    }
}
