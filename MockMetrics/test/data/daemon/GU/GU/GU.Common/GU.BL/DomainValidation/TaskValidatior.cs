using System;
using System.Linq;
using System.Text.RegularExpressions;

using Common.BL.Validation;
using Common.Types;

using GU.DataModel;

namespace GU.BL.DomainValidation
{
    /// <summary>
    /// Класс валидатор данных заявки.
    /// </summary>
    public class TaskValidatior : AbstractDomainValidator<Task>
    {

        private readonly IDomainValidator<ContentNode> _contentNodeValidator;

        /// <summary>
        /// Класс валидатор данных заявки.
        /// </summary>
        public TaskValidatior(IDomainValidator<ContentNode> contentNodeValidator)
        {
            _contentNodeValidator = contentNodeValidator;
            var t = Task.CreateInstance();
            _validationActions[Util.GetPropertyName(() => t.CustomerFio)] = ValidateCustomerFio;
            _validationActions[Util.GetPropertyName(() => t.CustomerPhone)] = ValidateCustomerPhone;
            _validationActions[Util.GetPropertyName(() => t.CustomerEmail)] = ValidateCustomerEmail;
        }

        /// <summary>
        /// Валидирует свойства доменного объекта. Возвращает результаты валидации.
        /// </summary>
        /// <param name="domainObject">Доменный объект</param>
        /// <returns>Объект хранящий результаты валидации</returns>
        public override ValidationErrorInfo Validate(Task domainObject)
        {
            var errorInfo = base.Validate(domainObject);
            
            if (domainObject.CurrentState != TaskStatusType.NotFilled &&
                domainObject.CurrentState != TaskStatusType.None)
            {
                /* TODO: obsolete
                domainObject.TaskDocList.ForEach(
                    doc =>
                    doc.TaskDocSectList.ForEach(
                        sec => sec.TaskAttrList.ForEach(attr => errorInfo.AddResult(_taskAttrValidator.Validate(attr)))));
                 */

                if (domainObject.Content != null &&
                                domainObject.Content.RootContentNodes != null &&
                                domainObject.Content.RootContentNodes.Any())
                {
                    errorInfo.AddResult(_contentNodeValidator.Validate(domainObject.Content.RootContentNodes.First()));
                }
            }

            return errorInfo;
        }

        private string ValidateCustomerFio(Task task)
        {
            /*if (!Regex.IsMatch(task.CustomerFio, @"^[а-яА-Я\s\-\.]{1,500}$"))
            {
                return "Поле ФИО может содержать только русские символы";
            }*/
            if (string.IsNullOrEmpty(task.CustomerFio))
            {
                return "Поле ФИО не заполнено";
            }

            if (!Regex.IsMatch(task.CustomerFio, @"^.{1,500}$"))
            {
                return "Максимальная длина поля ФИО 500 символов";
            }
            return null;
        }

        private string ValidateCustomerPhone(Task task)
        {
            if (!string.IsNullOrEmpty(task.CustomerPhone)
                && !Regex.IsMatch(task.CustomerPhone, @"^\+?[\d\-\(\)]{7,20}$"))
            {
                return "Поле Телефон должно быть заполнено корректно, например 8(902)123-1234 или 2-123-123";
            }
            return null;
        }

        private string ValidateCustomerEmail(Task task)
        {
            if (!string.IsNullOrEmpty(task.CustomerEmail)
                && !Regex.IsMatch(task.CustomerEmail, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
            {
                return "Поле Email должно быть заполнено корректно, например username@example.org";
            }
            return null;
        }

    }
}
