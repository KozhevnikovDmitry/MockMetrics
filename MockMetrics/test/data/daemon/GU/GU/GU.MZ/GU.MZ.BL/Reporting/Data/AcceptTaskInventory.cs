using System;
using System.Collections.Generic;

namespace GU.MZ.BL.Reporting.Data
{
    /// <summary>
    /// Класс, представляющий данные для отчёта "Опись предоставленных документов"
    /// </summary>
    public class AcceptTaskInventory
    {
        /// <summary>
        /// Регистрационный номер описи
        /// </summary>
        public string InventoryRegNumber { get; set; }

        /// <summary>
        /// Наименование подателя документов
        /// </summary>
        public string HolderName { get; set; }

        /// <summary>
        /// Наименование лицензируемого вида деятельности
        /// </summary>
        public string LicensedActivityName { get; set; }

        /// <summary>
        /// Должность составителя описи
        /// </summary>
        public string AuthorPosition { get; set; }

        /// <summary>
        /// Фамилия и инициалы составителя описи
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Дата составления описи
        /// </summary>
        public DateTime InventoryStamp { get; set; }

        /// <summary>
        /// Список документов, предоставленных подателем заявки
        /// </summary>
        public List<InventDocument> InventDocumentList { get; set; }

        /// <summary>
        /// Документ в описи
        /// </summary>
        public class InventDocument
        {
            /// <summary>
            /// Наименование документа
            /// </summary>
            public string DocumentName { get; set; }

            /// <summary>
            /// Количество предоставленных документов 
            /// </summary>
            public int Count { get; set; }
        }
    }
}
