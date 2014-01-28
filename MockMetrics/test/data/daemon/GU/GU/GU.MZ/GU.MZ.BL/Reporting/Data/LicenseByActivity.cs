using System.Collections.Generic;

namespace GU.MZ.BL.Reporting.Data
{
    /// <summary>
    /// Класс, представляющий данные для отчёта "Отчёт по выданым лицензиям в разрезе видов деятельности".
    /// </summary>
    public class LicenseByActivity
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Детализация отчёта.
        /// </summary>
        public List<LicenseByActivityStat> LicenseByActivityStatList { get; set; }

        /// <summary>
        /// Класс с данными по отчёту "Отчёт по выданым лицензиям в разрезе видов деятельности".
        /// </summary>
        public class LicenseByActivityStat
        {
            /// <summary>
            /// Наименование вида деятельности.
            /// </summary>
            public string ActivityName { get; set; }

            /// <summary>
            /// Количество лицензий.
            /// </summary>
            public int Count { get; set; }
        }
    }
}
