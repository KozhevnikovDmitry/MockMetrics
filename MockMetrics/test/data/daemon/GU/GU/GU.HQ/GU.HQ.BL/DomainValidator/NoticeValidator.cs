using System;
using System.Text.RegularExpressions;
using Common.BL.Validation;
using Common.Types;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DomainValidator
{
    public class NoticeValidator : AbstractDomainValidator<Notice>
    {
         public NoticeValidator()
         {
             var t = Notice.CreateInstance();
             _validationActions[Util.GetPropertyName(() => t.DocumentDate)] = ValidateDocumentDate;
             _validationActions[Util.GetPropertyName(() => t.DocumentNumber)] = ValidateDocumentNumber;
         }

         /// <summary>
         /// Валидация даты уведомления
         /// </summary>
         /// <param name="notice"></param>
         /// <returns></returns>
         private string ValidateDocumentDate(Notice notice)
         {
             return notice.DocumentDate == DateTime.MinValue ? "Поле 'Дата уведомления' не заполнено или заполнено не верно!" : null;
         }

         /// <summary>
         /// Валидация номера уведомления
         /// </summary>
         /// <param name="notice"></param>
         /// <returns></returns>
         private string ValidateDocumentNumber(Notice notice)
         {
             return notice.DocumentNumber.Length <= 0 ? "Поле 'Исходящий номер уведомления' не заполнено или заполнено не верно!" : null;
         }
    }
}