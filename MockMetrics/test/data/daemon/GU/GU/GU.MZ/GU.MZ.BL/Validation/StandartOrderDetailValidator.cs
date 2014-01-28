using System;
using Common.BL.Validation;
using Common.Types;
using GU.MZ.DataModel.MzOrder;

namespace GU.MZ.BL.Validation
{
    public class StandartOrderDetailValidator : AbstractDomainValidator<StandartOrderDetail>
    {
        public StandartOrderDetailValidator()
        {
            var orderDetail = StandartOrderDetail.CreateInstance();
            _validationActions[Util.GetPropertyName(() => orderDetail.SubjectId)] = ValidateSubjectId;
            _validationActions[Util.GetPropertyName(() => orderDetail.SubjectStamp)] = ValidateSubjectStamp;
            _validationActions[Util.GetPropertyName(() => orderDetail.Address)] = ValidateAddress;
            _validationActions[Util.GetPropertyName(() => orderDetail.FullName)] = ValidateFullName;
            _validationActions[Util.GetPropertyName(() => orderDetail.ShortName)] = ValidateShortName;
            _validationActions[Util.GetPropertyName(() => orderDetail.FirmName)] = ValidateFirmName;
            _validationActions[Util.GetPropertyName(() => orderDetail.Inn)] = ValidateInn;
            _validationActions[Util.GetPropertyName(() => orderDetail.Ogrn)] = ValidateOgrn;
        }

        private string ValidateOgrn(StandartOrderDetail orderDetail)
        {
            return string.IsNullOrEmpty(orderDetail.Ogrn) ? "Поле ОГРН организации должно быть заполнено" : null;
        }

        private string ValidateInn(StandartOrderDetail orderDetail)
        {
            return string.IsNullOrEmpty(orderDetail.Inn) ? "Поле ИНН организации должно быть заполнено" : null;
        }

        private string ValidateFirmName(StandartOrderDetail orderDetail)
        {
            return string.IsNullOrEmpty(orderDetail.FirmName) ? "Поле фирменное название организации должно быть заполнено" : null;
        }

        private string ValidateShortName(StandartOrderDetail orderDetail)
        {
            return string.IsNullOrEmpty(orderDetail.ShortName) ? "Поле краткое название организации должно быть заполнено" : null;
        }

        private string ValidateFullName(StandartOrderDetail orderDetail)
        {
            return string.IsNullOrEmpty(orderDetail.FullName) ? "Поле полное название организации должно быть заполнено" : null;
        }

        private string ValidateAddress(StandartOrderDetail orderDetail)
        {
            return string.IsNullOrEmpty(orderDetail.Address) ? "Поле адрес организации должно быть заполнено" : null;
        }

        private string ValidateSubjectStamp(StandartOrderDetail orderDetail)
        {
            return orderDetail.SubjectStamp == new DateTime() ? "Поле дата должно быть заполнено" : null;
        }

        private string ValidateSubjectId(StandartOrderDetail orderDetail)
        {
           return string.IsNullOrEmpty(orderDetail.SubjectId) ? "Поле регистрационный номер должно быть заполнено" : null;
        }
    }
}
