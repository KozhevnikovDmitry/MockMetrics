using System;
using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.Notifying;

namespace GU.MZ.BL.Validation
{
    /// <summary>
    /// Валидатор уведомлений
    /// </summary>
    public class NoticeValidator : AbstractDomainValidator<Notice>
    {
        public NoticeValidator()
        {
            var notice = Notice.CreateInstance();
            _validationActions[Util.GetPropertyName(() => notice.Stamp)] = ValidateStamp;
        }

        private string ValidateStamp(Notice notice)
        {
            return notice.Stamp != new DateTime() ? null : "Поле Дата отправления должно быть заполнено";
        }
    }
}
