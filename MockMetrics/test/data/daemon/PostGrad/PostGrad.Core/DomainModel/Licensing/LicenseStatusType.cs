using System.ComponentModel;

namespace PostGrad.Core.DomainModel.Licensing
{
    /// <summary>
    /// Типы статусов лицензии
    /// </summary>
    public enum LicenseStatusType
    {
        [Description("Проект")]
        Project = 1,
        [Description("Действует")]
        Active = 2,
        [Description("Переоформлена")]
        Renewal = 3,
        [Description("Приостановлена")]
        Suspend = 4,
        [Description("Возобновлена")]
        Restart = 5,
        [Description("Срок действия истёк")]
        Expired = 6,
        [Description("Аннулирована")]
        Void = 7,
        [Description("Прекращена")]
        Stop = 8
    }
}