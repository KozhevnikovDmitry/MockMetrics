using System.ComponentModel;

namespace PostGrad.Core.BL
{
    /// <summary>
    /// Перечисление типов источников реквизитов лицензиата в ходе привязки
    /// </summary>
    public enum RequisitesOrigin
    {
        /// <summary>
        /// Из реестра лицензиатов
        /// </summary>
        [Description("Лицензиат")]
        FromRegistr,

        /// <summary>
        /// Из данных заявления
        /// </summary>
        [Description("Заявитель")]
        FromTask
    }
}