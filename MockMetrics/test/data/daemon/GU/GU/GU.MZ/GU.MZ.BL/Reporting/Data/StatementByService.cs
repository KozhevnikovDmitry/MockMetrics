using System.Collections.Generic;

namespace GU.MZ.BL.Reporting.Data
{
    /// <summary>
    /// Класс, представляющий данные для отчёта "Отчёт по заявлениям в электронной форме в разрезе услуг".
    /// </summary>
    public class StatementByService
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Детализация отчёта.
        /// </summary>
        public List<StatementByServiceStat> StatementByServiceStatList { get; set; }

        /// <summary>
        /// Класс с данными по отчёту "Отчёт по заявлениям в электронной форме в разрезе услуг".
        /// </summary>
        public class StatementByServiceStat
        {
            /// <summary>
            /// Имя услуги.
            /// </summary>
            public string ServiceName { get; set; }

            /// <summary>
            /// Количество заявлений.
            /// </summary>
            public int Count { get; set; }

            /// <summary>
            /// Имя группы услуг
            /// </summary>
            public string ServiceGroupName { get; set; }
        }
    }
}
