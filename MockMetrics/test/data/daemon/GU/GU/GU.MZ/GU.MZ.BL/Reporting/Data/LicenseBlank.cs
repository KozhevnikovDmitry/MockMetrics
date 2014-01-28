using System.Collections.Generic;

namespace GU.MZ.BL.Reporting.Data
{
    /// <summary>
    /// Класс, представляющий данные для отчёта "Бланк лицензии"
    /// </summary>
    public class LicenseBlankReportData
    {
        /// <summary>
        /// Класс, представляющий данные для отчёта "Бланк лицензии"
        /// </summary>
        public LicenseBlankReportData()
        {
            BackBottomText = string.Empty;
            FrontText = string.Empty;
        }

        #region License data

        /// <summary>
        /// Номер лицензии
        /// </summary>
        public string RegNumber { get; set; }

        /// <summary>
        /// День даты выдачи лицензии
        /// </summary>
        public string GrantDateDay { get; set; }

        /// <summary>
        /// Месяц и год даты выдачи лицензии
        /// </summary>
        public string GrantDateMonthYear { get; set; }

        /// <summary>
        /// Вид лицензируемой деятельности
        /// </summary>
        public string LicensedActivity { get; set; }

        /// <summary>
        /// Дополнительная информация по виду лицензируемой деятельности
        /// </summary>
        public string AdditionalActivityInfo { get; set; }

        /// <summary>
        /// День даты окончания срока действия лицензии
        /// </summary>
        public string DueDateDay { get; set; }

        /// <summary>
        /// Месяц и год даты окончания срока действия лицензии
        /// </summary>
        public string DueDateMonthYear { get; set; }



        /// <summary>
        /// День даты принятия решения о предоставлении лицензии
        /// </summary>
        public string GrantOrderStampDay { get; set; }

        /// <summary>
        /// Месяц и год даты принятия решения о предоставлении лицензии
        /// </summary>
        public string GrantOrderStampMonthYear { get; set; }

        /// <summary>
        /// Регистрационный номер решения о предоставлении лицензии
        /// </summary>
        public string GrantOrderRegNumber { get; set; }

        /// <summary>
        /// Верхний текст на лицевой стороне лицензии
        /// </summary>
        public string FrontText { get; set; }

        /// <summary>
        /// Нижний текст на оборотной стороне лицензии сверху
        /// </summary>
        public string BackTopText { get; set; }

        /// <summary>
        /// Нижний текст на оборотной стороне лицензии снизу
        /// </summary>
        public string BackBottomText { get; set; }
        
        #endregion

        #region Blank data

        /// <summary>
        /// Номер бланка лицензии
        /// </summary>
        public string BlankNumber { get; set; }

        /// <summary>
        /// Наименование должности главы лицензирующей организации
        /// </summary>
        public string LicensiarHeadPosition { get; set; }
        
        /// <summary>
        /// ФИО главы лицензирующей организации
        /// </summary>
        public string LicensiarHeadName { get; set; }
        
        #endregion

        #region License Holder data

        /// <summary>
        /// Полное наименование лицензиата
        /// </summary>
        public string LicenseHolderFullName { get; set; }

        /// <summary>
        /// Краткое наименование лицензиата
        /// </summary>
        public string LicenseHolderShortName { get; set; }

        /// <summary>
        /// Фирменное наименование лицензиата
        /// </summary>
        public string LicenseHolderFirmName { get; set; }

        /// <summary>
        /// ОГРН лицензиата
        /// </summary>
        public string Ogrn { get; set; }

        /// <summary>
        /// ИНН лицензиата
        /// </summary>
        public string Inn { get; set; }

        /// <summary>
        /// Адрес лицензиата
        /// </summary>
        public string LicenseHolderAddress { get; set; }
        
        #endregion

        #region LicenseAnnex's data

        /// <summary>
        /// Количество приложений лицензии
        /// </summary>
        public string AnnexCount { get; set; }

        /// <summary>
        /// Количество листов приложений
        /// </summary>
        public string AnnexBlankCount { get; set; }

        /// <summary>
        /// Список приложений лицензии
        /// </summary>
        public List<LicenseAnnexBlankReportData> LicenseAnnexBlankList { get; set; }

        #endregion
    }

    /// <summary>
    /// Класс, представляющий данные для отчёта "Бланк лицензии" по одному приложению лицензии
    /// </summary>
    public class LicenseAnnexBlankReportData
    {
        /// <summary>
        /// Порядковый номер приложения
        /// </summary>
        public string AnnexNumber { get; set; }
        
        /// <summary>
        /// День даты создания приложения
        /// </summary>
        public string AnnexGrantDateDay { get; set; }

        /// <summary>
        /// Месяц и год даты создания приложения
        /// </summary>
        public string AnnexGrantDateMonthYear { get; set; }

        /// <summary>
        /// Список адресов осуществления деятельности
        /// </summary>
        public string AddressList { get; set; }

        /// <summary>
        /// Список видов деятельности
        /// </summary>
        public string SubactivityList { get; set; }
    }
}
