using System;
using System.ComponentModel;
using PostGrad.Core.DomainModel.Notifying;

namespace PostGrad.Core.DomainModel.MzOrder
{
    public enum StandartOrderType
    {
        [Description("Приказ о возврате заявления")]
        ReturnTask = 1,
        [Description("Приказ о принятии к рассмотрению")]
        AcceptTask = 2,
        [Description("Приказ о предоставлении лицензии")]
        GrantLicense = 3,
        [Description("Приказ о переоформлении лицензии")]
        RenewalLicense = 4,
        [Description("Приказ об отказе в предоставлении лицензии")]
        NotGrantLicense = 5,
        [Description("Приказ об отказе в переоформлении лицензии")]
        NotRenewalLicense = 6,
        [Description("Приказ о прекращении действия лицензии")]
        StopLicense = 7
    }

    public static class StandartOrderTypeExtensions
    {
        public static NoticeType ToNoticeType(this StandartOrderType orderType)
        {
            switch (orderType)
            {
                case StandartOrderType.AcceptTask:
                {
                    return NoticeType.AcceptDocuments;
                }
                case StandartOrderType.ReturnTask:
                {
                    return NoticeType.RejectDocuments;
                }
                case StandartOrderType.GrantLicense:
                {
                    return NoticeType.ServiceGrant;
                }
                case StandartOrderType.RenewalLicense:
                {
                    return NoticeType.ServiceGrant;
                }
                case StandartOrderType.NotGrantLicense:
                {
                    return NoticeType.ServiseReject;
                }
                case StandartOrderType.NotRenewalLicense:
                {
                    return NoticeType.ServiseReject;
                }
                case StandartOrderType.StopLicense:
                {
                    return NoticeType.ServiceGrant;
                }
            }

            throw new NotSupportedException(string.Format("Тип перечисления не поддерживается {0}", orderType));
        }
    }
}
