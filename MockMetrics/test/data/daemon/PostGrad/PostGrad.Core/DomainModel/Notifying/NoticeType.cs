using System.ComponentModel;

namespace PostGrad.Core.DomainModel.Notifying
{
    /// <summary>
    /// Перечисление типов уведомлений
    /// </summary>
    public enum NoticeType
    {
        [Description("о приёме документов")] 
        AcceptDocuments = 1,

        [Description("о возрате документов")] 
        RejectDocuments = 2,

        [Description("о проведении документарной проверки")] 
        DocumentExpertise = 3,

        [Description("о проведении выездной проверки")] 
        PlaceInspection = 4,

        [Description("об отказе в предоставлении услуги")] 
        ServiseReject = 5,

        [Description("о предоставлении услуги")] 
        ServiceGrant = 6,

        [Description("о необходимости устранения нарушений")] 
        ViolationResolve = 7
    }
}
