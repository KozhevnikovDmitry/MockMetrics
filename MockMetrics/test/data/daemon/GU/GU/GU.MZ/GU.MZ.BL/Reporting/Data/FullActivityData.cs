using System.Collections.Generic;

namespace GU.MZ.BL.Reporting.Data
{
    /// <summary>
    /// Класс, представляющий данные для отчёта "Полный отчёт по лицензированию по виду деятельности"
    /// </summary>
    public class FullActivityData
    {
        public List<FullActivityDataDetail> Details { get; set; } 
    }

    /// <summary>
    /// Класс, представляющий одну строку для отчёта "Полный отчёт по лицензированию по виду деятельности"
    /// </summary>
    public class FullActivityDataDetail
    {
        #region DossierFile data

        /// <summary>
        /// Регистрационный номер записи
        /// </summary>
        public string RegNumber { get; set; }

        /// <summary>
        /// Дата приёма документов
        /// </summary>
        public string Stamp { get; set; }

        /// <summary>
        /// ФИО, должность отвественного исполнителя
        /// </summary>
        public string ResponsibleEmployeeData { get; set; }

        /// <summary>
        /// Примечение
        /// </summary>
        public string Note { get; set; }

        #endregion

        #region Task data

        /// <summary>
        /// Заявленная административная процедура - гос услуга
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// Основание для переоформления услуги
        /// </summary>
        public string RenewalReason { get; set; }

        /// <summary>
        /// Установленный срок предоставления услуги
        /// </summary>
        public string ServicePeriod { get; set; }

        /// <summary>
        /// ФИО, должность лица принявшего заявление
        /// </summary>
        public string AccepterEmployeeData { get; set; }

        /// <summary>
        /// Предельная дата предоставления услуги
        /// </summary>
        public string TaskDueDate { get; set; }
        
        #endregion

        #region ServiceResult data

        /// <summary>
        /// Результат предоставления услуги 
        /// </summary>
        public string ServiceResult { get; set; }

        /// <summary>
        /// Дата приказа о предостлавлении или отказе в предоставлении услуги
        /// </summary>
        public string GrantResultOrderStamp { get; set; }

        /// <summary>
        /// Номер приказа о предостлавлении или отказе в предоставлении услуги
        /// </summary>
        public string GrantResultOrderRegNumber { get; set; }

        /// <summary>
        /// Дата предоставления результата и примечание способе доставки (почтой, лично)
        /// </summary>
        public string DoneData { get; set; }

        /// <summary>
        /// Фактический срок предоставления услуги в днях
        /// </summary>
        public int TaskWorkDays { get; set; }

        #endregion
        
        #region LicenseDossier data

        /// <summary>
        /// Номер лицензионного дела
        /// </summary>
        public string LicenseDossierRegNumber { get; set; }
        
        /// <summary>
        /// Организационно-правовая форма
        /// </summary>
        public string LegalForm { get; set; }

        /// <summary>
        /// Полное наименивание заявителя
        /// </summary>
        public string HolderFullName { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public string Inn { get; set; }
        
        #endregion

        #region Violation data

        /// <summary>
        /// Дата вручения уведомления об устранении нарушений
        /// </summary>
        public string ViolationNoticeStamp { get; set; }

        /// <summary>
        /// Дата устранения нарушения
        /// </summary>
        public string ViolationResolveStamp { get; set; }

        /// <summary>
        /// Реквизиты приказа о рассмотрении или возврате документов
        /// </summary>
        public string AcceptRejectOrderRequisites { get; set; }

        /// <summary>
        /// Дата возврата документов и заявления
        /// </summary>
        public string ReturnDocumentsStamp { get; set; }
        
        #endregion

        #region Document expertise data

        /// <summary>
        /// Реквизиты приказа о проведении документарной проверки
        /// </summary>
        public string DocumentExpertiseOrderRequisites { get; set; }

        /// <summary>
        /// Сроки проведения документарной проверки
        /// </summary>
        public string DocumentExpertisePeriod { get; set; }

        /// <summary>
        /// Дата акта документарной проверки
        /// </summary>
        public string DocumentExpertiseActStamp { get; set; }
        
        #endregion

        #region Inspection data

        /// <summary>
        /// Реквизиты приказа о проведении выездной проверки
        /// </summary>
        public string InspectionOrderRequisites { get; set; }

        /// <summary>
        /// Сроки проведения выездной проверки
        /// </summary>
        public string InspectionPeriod { get; set; }

        /// <summary>
        /// Дата акта выездной проверки
        /// </summary>
        public string InspectionActStamp { get; set; }

        /// <summary>
        /// Информация о привлечённыз экспертах
        /// </summary>
        public string InvolvedExpertsData { get; set; }
        
        #endregion
    }
}
