using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common.BL.DictionaryManagement;
using Common.DA.Interface;

using GU.MZ.BL.Reporting.Data;
using GU.MZ.BL.Reporting.Mapping.MappingException;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.Reporting.Mapping
{
    /// <summary>
    /// Класс отчёт "Бланк лицензии"
    /// </summary>
    public class LicenseBlankReport : BaseReport
    {
        private readonly Func<IDomainDbManager> _getDb;

        /// <summary>
        /// Менеджер кэша справочников
        /// </summary>
        private readonly IDictionaryManager _dictionaryManager;

        private License _license;

        /// <summary>
        /// Класс отчёт "Бланк лицензии"
        /// </summary>
        /// <param name="_getDb"></param>
        /// <param name="dictionaryManager">Менеджер кэша справочников</param>
        public LicenseBlankReport(Func<IDomainDbManager> _getDb,
                                  IDictionaryManager dictionaryManager)
        {
            ViewPath = "Reporting/View/GU.MZ/LicenseBlank.mrt";
            this._getDb = _getDb;
            _dictionaryManager = dictionaryManager;
        }

        public void SetLicense(License license)
        {
            _license = CloneLicense(license);
        }

        /// <summary>
        /// Возвращает "глубокий" клон лицензии
        /// </summary>
        /// <param name="license">Исходная лицензия</param>
        /// <returns>Клон</returns>
        /// <remarks>
        /// Клонируем лицензию для того, чтобы изменения не отразились на исходной лицензии.
        /// </remarks>
        private License CloneLicense(License license)
        {
            var clone = license.Clone();

            for (int i = 0; i < clone.LicenseObjectList.Count; i++)
            {
                clone.LicenseObjectList[i] = clone.LicenseObjectList[i].Clone();
                for (int j = 0; j < clone.LicenseObjectList[i].ObjectSubactivityList.Count; j++)
                {
                    clone.LicenseObjectList[i].ObjectSubactivityList[j] =
                        clone.LicenseObjectList[i].ObjectSubactivityList[j].Clone();
                }
            }

            return clone;
        }

        /// <summary>
        /// Возвращает данные для отчёта "Бланк лицензии".
        /// </summary>
        /// <returns>Объект с информацией для отчёта "Бланк лицензии"</returns>
        public override object RetrieveData()
        {
            using (var dbManager = _getDb())
            {
                var result = new LicenseBlankReportData();
                RetrieveLicenseData(result);
                RetrieveBlankData(result);
                RetrieveLicenseObjectData(result, dbManager);
                return result;
            }
        }

        #region Retrieve license data

        private void RetrieveLicenseData(LicenseBlankReportData reportData)
        {
            reportData.RegNumber = _license.RegNumber;

            FillLicenseGrantDateData(reportData);
            FillGrantOrderStampData(reportData);
            FillLicenseDueDateData(reportData);

            var activity = _dictionaryManager.GetDictionaryItem<LicensedActivity>(_license.LicensedActivityId);
            reportData.LicensedActivity = activity.BlankName;
            reportData.AdditionalActivityInfo = activity.AdditionalInfo;
            reportData.GrantOrderRegNumber = _license.GrantOrderRegNumber;
        }

        private void FillLicenseGrantDateData(LicenseBlankReportData reportData)
        {
            if (_license.GrantDate.HasValue)
            {
                reportData.GrantDateDay = _license.GrantDate.Value.Day.ToString();
                reportData.GrantDateMonthYear = string.Format(
                    "{0} {1}",
                    MonthDataHelper.GetMonth(_license.GrantDate.Value.Month),
                    _license.GrantDate.Value.Year.ToString());
            }
            else
            {
                reportData.GrantDateDay = "Ошибка";
                reportData.GrantDateMonthYear = "Ошибка";
            }
        }

        private void FillGrantOrderStampData(LicenseBlankReportData reportData)
        {
            if (_license.GrantOrderStamp.HasValue)
            {
                reportData.GrantOrderStampDay = _license.GrantOrderStamp.Value.Day.ToString();
                reportData.GrantOrderStampMonthYear = string.Format(
                    "{0} {1}",
                    MonthDataHelper.GetMonth(_license.GrantOrderStamp.Value.Month),
                    _license.GrantOrderStamp.Value.Year.ToString());
            }
            else
            {
                reportData.GrantOrderStampDay = "Ошибка";
                reportData.GrantOrderStampMonthYear = "Ошибка";
            }
        }

        private void FillLicenseDueDateData(LicenseBlankReportData reportData)
        {
            if (_license.DueDate.HasValue)
            {
                reportData.DueDateDay = _license.DueDate.Value.Day.ToString();
                reportData.DueDateMonthYear = string.Format(
                    "{0} {1}",
                    MonthDataHelper.GetMonth(_license.DueDate.Value.Month),
                    _license.DueDate.Value.Year.ToString());
            }
            else
            {
                reportData.DueDateDay = string.Empty;
                reportData.DueDateMonthYear = string.Empty;
            }
        }
        
        private void RetrieveBlankData(LicenseBlankReportData reportData)
        {
            reportData.BlankNumber = _license.BlankNumber;
            reportData.LicensiarHeadName = _license.ActualRequisites.HeadName;
            reportData.LicensiarHeadPosition = _license.ActualRequisites.HeadPositionName;
            reportData.LicenseHolderAddress = _license.ActualRequisites.Address.ToLongString();
            reportData.LicenseHolderShortName = _license.ActualRequisites.ShortName;
            reportData.LicenseHolderFullName = _license.ActualRequisites.FullName;
            reportData.LicenseHolderFirmName = _license.ActualRequisites.FirmName;
            reportData.Ogrn = _license.LicenseHolder.Ogrn;
            reportData.Inn = _license.LicenseHolder.Inn;
        }
        
        #endregion

        #region Retrieve annex data

        /// <summary>
        /// Добавляет в набор данные для отчёта данные по приложениям лицензии
        /// </summary>
        /// <param name="reportData">Данные для отчёта</param>
        /// <param name="dbManager">Менеджер базы данных</param>
        /// <exception cref="RetrieveLicenseDataWithoutActiveObjectsException">Попытка получения данных лицензии без единого активного объекта с номенклатурой</exception>
        private void RetrieveLicenseObjectData(LicenseBlankReportData reportData, IDomainDbManager dbManager)
        {
            reportData.LicenseAnnexBlankList = new List<LicenseAnnexBlankReportData>();

            var actualObjects = _license.LicenseObjectList.Where(t => t.LicenseObjectStatusId == 2).ToList();

            if (!actualObjects.Any())
            {
                throw new RetrieveLicenseDataWithoutActiveObjectsException();
            }

            FillSubactivitiesFromDictionary(actualObjects);
            FillBackTextWithAddresses(reportData, actualObjects);
            FillFrontTextWithActivities(reportData, actualObjects);

            reportData.AnnexCount = "0";
            reportData.AnnexBlankCount = "0";

            if (reportData.FrontText.Count() > 300 || reportData.BackBottomText.Count() > 300)
            {
                FillDataForLongLicense(reportData, actualObjects);
            }
        }

        /// <summary>
        /// Для объектов с номенклатурой заполняет ассоциацию Лицензируемая поддеятельность - LicensedSubactivity
        /// </summary>
        /// <param name="licenseObjects">Список объектов с номенклатурой</param>
        private void FillSubactivitiesFromDictionary(List<LicenseObject> licenseObjects)
        {
            licenseObjects.ForEach(
                l =>
                l.ObjectSubactivityList.ForEach(
                    t =>
                    t.LicensedSubactivity =
                    _dictionaryManager.GetDictionaryItem<LicensedSubactivity>(t.LicensedSubactivityId)));
        }

        /// <summary>
        /// Заполняеи текст на оборотной стороне лицензии 
        /// </summary>
        /// <param name="reportData">Данные для отчёта</param>
        /// <param name="actualObjects">Актуальные объекты с номенклатурой</param>
        private void FillBackTextWithAddresses(LicenseBlankReportData reportData, List<LicenseObject> actualObjects)
        {
            var addrBuilder = new StringBuilder();
            foreach (var licenseObject in actualObjects)
            {
                addrBuilder.Append(actualObjects.IndexOf(licenseObject) + 1)
                           .Append(". ")
                           .Append(licenseObject.Address.ToLongString())
                           .Append(";\n");
            }
            reportData.BackBottomText = addrBuilder.ToString();
            reportData.BackBottomText = reportData.BackBottomText.Substring(0, reportData.BackBottomText.Length - 2) + ".";
        }

        /// <summary>
        /// Заполняет текст на лицевой стороне лицензии 
        /// </summary>
        /// <param name="reportData">Данные для отчёта</param>
        /// <param name="actualObjects">Актуальные объекты с номенклатурой</param>
        private void FillFrontTextWithActivities(LicenseBlankReportData reportData, List<LicenseObject> actualObjects)
        {
            var actBuilder = new StringBuilder();

            foreach (var licenseObject in actualObjects)
            {
                foreach (
                    var objectSubactivityGroup in
                        licenseObject.ObjectSubactivityList.GroupBy(t => t.LicensedSubactivity.SubactivityGroupId))
                {
                    var serviceGroup =
                        _dictionaryManager.GetDictionaryItem<LicensedSubactivity>(
                            objectSubactivityGroup.First().LicensedSubactivityId).SubactivityGroup;

                    actBuilder.Append(string.IsNullOrEmpty(serviceGroup.BlankName) ? serviceGroup.Name : serviceGroup.BlankName).Append(": ");

                    foreach (var objectSubactivity in objectSubactivityGroup)
                    {
                        var subactivity =
                            _dictionaryManager.GetDictionaryItem<LicensedSubactivity>(
                                objectSubactivity.LicensedSubactivityId);

                        actBuilder.Append(string.IsNullOrEmpty(subactivity.BlankName) ? subactivity.Name : subactivity.BlankName).Append(", ");
                    }
                }
            }


            reportData.FrontText = actBuilder.ToString();
            reportData.FrontText = reportData.FrontText.Substring(0, reportData.FrontText.Length - 2) + ".";

            reportData.BackTopText = "Адреса осуществления лицензируемого вида деятельности:";
        }

        /// <summary>
        /// Заполняет данные для длинной лицензии с приложениями
        /// </summary>
        /// <param name="reportData">Данные для отчёта</param>
        /// <param name="actualObjects">Актуальные объекты с номенклатурой</param>
        private void FillDataForLongLicense(LicenseBlankReportData reportData, List<LicenseObject> actualObjects)
        {
            reportData.FrontText = "Согласно приложению(ям)";
            reportData.BackTopText = "Адреса осуществления лицензируемого вида деятельности, согласно приложению(ям)";
            reportData.BackBottomText = string.Empty;
            reportData.AnnexCount = actualObjects.Count.ToString();
            reportData.AnnexBlankCount = "0";

            foreach (var licenseObject in actualObjects)
            {
                var annexData = RetrieveAnnexData(licenseObject, actualObjects.IndexOf(licenseObject) + 1);
                foreach (var licenseAnnexBlankReportData in annexData)
                {
                    reportData.LicenseAnnexBlankList.Add(licenseAnnexBlankReportData);
                }
            }

            reportData.AnnexBlankCount = reportData.LicenseAnnexBlankList.Count().ToString();
        }

        /// <summary>
        /// Возврвщает данные для отчёта по одному приложению лицензии
        /// </summary>
        /// <param name="licenseObject">Объект с номенклатурой</param>
        /// <returns>Данные для отчёта по одному приложению лицензии</returns>
        private List<LicenseAnnexBlankReportData> RetrieveAnnexData(LicenseObject licenseObject, int annexNumber)
        {
            var activityList = GetSubactivityList(licenseObject);

            var result =
                activityList.Select(
                    activity =>
                    new LicenseAnnexBlankReportData { SubactivityList = activity, AnnexNumber = annexNumber.ToString() })
                            .ToList();

            if (result.Count > 1)
            {
                for (int i = 1; i < result.Count; i++)
                {
                    result[i].AnnexNumber += " продолжение";
                }
            }

            FillSharedAnnexInformation(licenseObject, result);

            return result;
        }

        /// <summary>
        /// Возврвщает список поддеятельностей на объекте с номенклатурой в разбивке на листы приложения.
        /// </summary>
        /// <param name="licenseObject">Объект с номенклатурой</param>
        /// <returns>Список поддеятельностей на объекте с номенклатурой</returns>
        private List<string> GetSubactivityList(LicenseObject licenseObject)
        {
            var result = new List<string>();

            var actBuilder = new StringBuilder();

            foreach (var objectSubactivityGroup in licenseObject.ObjectSubactivityList.GroupBy(t => t.LicensedSubactivity.SubactivityGroupId))
            {
                var serviceGroup =
                        _dictionaryManager.GetDictionaryItem<LicensedSubactivity>(
                            objectSubactivityGroup.First().LicensedSubactivityId).SubactivityGroup;

                actBuilder.Append(string.IsNullOrEmpty(serviceGroup.BlankName) ? serviceGroup.Name : serviceGroup.BlankName).Append(": ");

                foreach (var objectSubactivity in objectSubactivityGroup)
                {
                    var subactivity =
                             _dictionaryManager.GetDictionaryItem<LicensedSubactivity>(
                                 objectSubactivity.LicensedSubactivityId);

                    actBuilder.Append(string.IsNullOrEmpty(subactivity.BlankName) ? subactivity.Name : subactivity.BlankName).Append(", ");

                    if (actBuilder.ToString().Length > 350)
                    {
                        result.Add(actBuilder.ToString());
                        actBuilder = new StringBuilder();
                    }
                }
            }

            if (actBuilder.ToString().Count() != 0)
            {
                result.Add(actBuilder.ToString());
            }

            result[result.Count - 1] = result.Last().Substring(0, result.Last().Length - 2) + ".";

            return result;
        }

        /// <summary>
        /// Для списка LicenseAnnexBlankReportData добавляет общую информацию для всех листов приложения лицензии
        /// </summary>
        /// <param name="licenseObject">Объект с номенклатурой</param>
        /// <param name="reportData">Данные для всех листов приложения</param>
        private void FillSharedAnnexInformation(LicenseObject licenseObject, List<LicenseAnnexBlankReportData> reportData)
        {
            foreach (var licenseAnnexBlankReportData in reportData)
            {
                FillAnnexGrantDateData(licenseAnnexBlankReportData, licenseObject);
                licenseAnnexBlankReportData.AddressList = licenseObject.Address.ToLongString();
            }
        }

        /// <summary>
        /// Заполняет данные о дате предоставления приложения
        /// </summary>
        /// <param name="reportData">Данные для отчёта</param>
        /// <param name="licenseObject">Объект с номенклатурой</param>
        private void FillAnnexGrantDateData(LicenseAnnexBlankReportData reportData, LicenseObject licenseObject)
        {
            if (licenseObject.GrantOrderStamp.HasValue)
            {
                reportData.AnnexGrantDateDay = licenseObject.GrantOrderStamp.Value.Day.ToString();
                reportData.AnnexGrantDateMonthYear = string.Format(
                    "{0} {1}",
                    MonthDataHelper.GetMonth(licenseObject.GrantOrderStamp.Value.Month),
                    licenseObject.GrantOrderStamp.Value.Year.ToString());
            }
            else
            {
                reportData.AnnexGrantDateDay = "Ошибка";
                reportData.AnnexGrantDateMonthYear = "Ошибка";
            }
        }

        #endregion
    }
}
