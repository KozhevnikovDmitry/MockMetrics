using System.ComponentModel;

namespace GU.MZ.DataModel.Notifying
{
    /// <summary>
    /// Перечисление типов уведомлений
    /// </summary>
    public enum NoticeType
    {
        [Description("Уведомление о приёме документов")] 
        AcceptDocuments = 1,

        [Description("Уведомление о возрате документов")] 
        RejectDocuments = 2,

        [Description("Уведомление о проведении документарной проверки")] 
        DocumentExpertise = 3,

        [Description("Уведомление о проведении выездной проверки")] 
        PlaceInspection = 4,

        [Description("Уведомление об отказе в предоставлении услуги")] 
        ServiseReject = 5,

        [Description("Уведомление о предоставлении услуги")] 
        ServiceGrant = 6,

        [Description("Уведомление о необходимости устранения нарушений")] 
        ViolationResolve = 7
    }
}
