using System.ComponentModel;

namespace GU.MZ.DataModel.Dossier
{
    /// <summary>
    /// Перечисление статусов тома лицензионного дела
    /// </summary>
    [DefaultValue(DossierFileStatus.Unbounded)]
    public enum DossierFileStatus
    {
        /// <summary>
        /// Том не привязан к лицензионному делу и лицензиату
        /// </summary>
        Unbounded = 1,

        /// <summary>
        /// Том находится на стадии ведения
        /// </summary>
        Active = 2,

        /// <summary>
        /// Ведение тома завершено
        /// </summary>
        Closed = 3
    }
}
