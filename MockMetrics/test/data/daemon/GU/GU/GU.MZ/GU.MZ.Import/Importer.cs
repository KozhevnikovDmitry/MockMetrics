using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using Common.DA.DAException;
using Common.DA.Interface;
using Common.Types.Exceptions;
using Excel;

using System.Linq;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;
using GU.MZ.DataModel.Requisites;
using License = GU.MZ.DataModel.Licensing.License;

namespace GU.MZ.Import
{
    /// <summary>
    /// Класс, занимающийся имортом данных по лицензированию
    /// </summary>
    public class Importer
    {
        private readonly IDomainDataMapper<LicenseHolder> _holderMapper;
        private readonly IDomainDataMapper<LicenseDossier> _dossierMapper;
        private readonly IDomainDataMapper<License> _licenseMapper;
        private readonly IDomainDataMapper<LicenseObject> _licObjectMapper;
        private readonly IDictionaryManager _dictionaryManager;
        private readonly Synchronizer _synchronizer;
        private readonly Protocoller _protocoller;

        public event Action<int> OnProgress;

        public bool CancellationRequested { get; set; }

        public Exception Exception { get; private set; }

        private void RaiseOnProgress(int total, int current)
        {
            if (OnProgress != null)
            {
                OnProgress(Convert.ToInt32(current * 100.0 / total));
            }
        }
        
        public Importer(IDomainDataMapper<LicenseHolder> holderMapper,
                        IDomainDataMapper<LicenseDossier> dossierMapper, 
                        IDomainDataMapper<License> licenseMapper, 
                        IDomainDataMapper<LicenseObject> licObjectMapper, 
                        IDictionaryManager dictionaryManager,
                        Synchronizer synchronizer, 
                        Protocoller protocoller)
        {
            _holderMapper = holderMapper;
            _dossierMapper = dossierMapper;
            _licenseMapper = licenseMapper;
            _licObjectMapper = licObjectMapper;
            _dictionaryManager = dictionaryManager;
            _synchronizer = synchronizer;
            _protocoller = protocoller;
            CancellationRequested = false;
        }

        public void Import(IDomainDbManager db, string fileName, string logName)
        {
            _protocoller.Path = logName;
            _protocoller.Drop();
            Exception = null;
            CancellationRequested = false;
            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                var excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                DataSet data = excelReader.AsDataSet();
                excelReader.Close();

                if (data.Tables.Count == 0)
                {
                    Exception = new ImportException("Неверный формат файла реестра");
                    throw Exception;
                }

                ProceedTable(data.Tables[0], db);
                _protocoller.Save();
            }

        }

        private void ProceedTable(DataTable table, IDomainDbManager db)
        {
            db.BeginDomainTransaction();
            try
            {
                var rows = table.Rows;
                var licRows = table.AsEnumerable().Where(t => t.ItemArray.Any() && t[0].ToString().StartsWith("ЛО-")).ToArray();
                if (!licRows.Any())
                {
                    throw new ImportException("Неверный формат файла реестра");
                }
                for (var i = 0; i < licRows.Count(); i++)
                {
                    CancelCheck(db);

                    var holder = SaveLicenseHolder(licRows[i], db);
                    var dossier = SaveDossier(licRows[i], holder, db);
                    var license = SaveLicense(licRows[i], dossier, holder, db);

                    var lower = rows.IndexOf(licRows[i]);
                    var upper = i + 1 == licRows.Count() ? rows.Count : rows.IndexOf(licRows[i + 1]);

                    for (int j = lower; j < upper; j++)
                    {
                        CancelCheck(db);

                        RaiseOnProgress(rows.Count, j);

                        SaveLicenseObject(rows[j], license, db);
                    }
                }
                db.CommitDomainTransaction();
            }
            catch (CancelImportException)
            {
                _protocoller.Drop();
            }
            catch (TransactionControlException ex)
            {
                _protocoller.Drop();
                Exception = ex;
                throw;
            }
            catch (BLLException ex)
            {
                _protocoller.Drop();
                Exception = ex;
                throw;
            }
            catch (Exception ex)
            {
                _protocoller.Drop();
                Exception = ex;
                db.RollbackDomainTransaction();
                throw new ImportException("Ошибка импорта данных", ex);
            }
        }

        #region Parse Holder

        private LicenseHolder SaveLicenseHolder(DataRow licRow, IDomainDbManager db)
        {
            try
            {
                var inn = licRow[10].ToString().Trim();
                var ogrn = licRow[9].ToString().Trim();

                LicenseHolder holder =
                    db.GetDomainTable<LicenseHolder>()
                       .Where(t => t.Inn == inn && t.Ogrn == ogrn)
                       .ToList()
                       .Select(t => db.RetrieveDomainObject<LicenseHolder>(t.Id))
                       .SingleOrDefault();

                if (holder == null)
                {
                    holder = LicenseHolder.CreateInstance();
                    holder.Inn = inn;
                    holder.Ogrn = ogrn;
                }

                var requisites = HolderRequisites.CreateInstance();
                requisites.LicenseHolderId = holder.Id;

                DateTime createDate;
                if (DateTime.TryParse(licRow[1].ToString().Trim(), out createDate))
                {
                    requisites.CreateDate = createDate;
                }
                else
                {
                    requisites.CreateDate = DateTime.Today;
                    requisites.Note += string.Format("[[{0}][{1}]]", "Дата регистрации", licRow[1].ToString().Trim());
                }

                FillRequisites(requisites, licRow[2].ToString().Trim(), db);

                requisites = _synchronizer.SyncRequisites(requisites, db);

                requisites.Address = _synchronizer.SyncHolderAddress(requisites, GetAddress(licRow, 5, 4, 7, 6, 8), db);

                holder.RequisitesList.Add(requisites);

                _protocoller.Protocol(holder);
                return _holderMapper.Save(holder, db);
            }
            catch (BLLException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ImportException("Ошибка импорта данных лицензиата", ex) { Row = licRow };
            }
        }

        private Address GetAddress(DataRow licRow, int regionInd, int zipInd, int areaInd, int cityInd, int streetInd)
        {
            try
            {
                var address = Address.CreateInstance();
                address.Zip = licRow[zipInd].ToString().Trim();
                address.CountryRegion = licRow[regionInd].ToString().Trim();

                var cityStr = licRow[cityInd].ToString().Trim().Split(',');
                var areaStr = licRow[areaInd].ToString().Trim().Split(',');
                var streetStr = licRow[streetInd].ToString().Trim().Split(',');

                if (cityStr.Count() > 1)
                {
                    address.City = cityStr.Last().Trim();
                    address.Note += licRow[cityInd].ToString();
                }
                else
                {
                    address.City = cityStr.Single().Trim();
                }

                if (areaStr.Count() > 1)
                {
                    address.Area = areaStr.First().Trim();
                    address.Note += licRow[areaInd].ToString();
                }
                else
                {
                    address.Area = areaStr.Single().Trim();
                }

                if (streetStr.Count() > 2)
                {
                    address.House = streetStr.Last().Trim();
                    address.Street = streetStr[streetStr.Count() - 2].Trim();
                    address.Note += licRow[streetInd].ToString();

                }
                else
                {
                    address.House = streetStr.Last().Trim();
                    address.Street = streetStr.First().Trim();
                    if (address.House.Length > 10)
                    {
                        address.Note += licRow[streetInd].ToString();
                    }
                }

                if (address.House.Length > 10)
                {
                    address.House = string.Empty;
                }

                address.Note = address.Note.Trim();
                return address;
            }
            catch (Exception ex)
            {
                throw new ImportException("Ошибка импорта данных адреса", ex) { Row = licRow };
            }
        }

        private void FillRequisites(HolderRequisites requisites, string reqStr, IDomainDbManager db)
        {
            try
            {
                requisites.JurRequisites = JurRequisites.CreateInstance();
                var shNmStart = reqStr.IndexOf("Краткое название:", StringComparison.Ordinal) + "Краткое название:".Length;
                var fullNmStart = reqStr.IndexOf("Полное название:", StringComparison.Ordinal);

                requisites.JurRequisites.ShortName = requisites.JurRequisites.FirmName = reqStr.Substring(shNmStart, fullNmStart - shNmStart).Trim();

                fullNmStart += "Полное название:".Length;

                var lgFrmStart = reqStr.IndexOf("Организационно-правовая форма:", StringComparison.Ordinal);
                requisites.JurRequisites.FullName = reqStr.Substring(fullNmStart, lgFrmStart - fullNmStart).Trim();

                lgFrmStart += "Организационно-правовая форма:".Length;
                var lgFrmStr = "не указано";
                if (lgFrmStart < reqStr.Length)
                {
                    lgFrmStr = reqStr.Substring(lgFrmStart).Trim();
                }

                requisites.JurRequisites.LegalFormId = GetLegalForm(lgFrmStr, db);
                requisites.JurRequisites.HeadName = string.Empty;
                requisites.JurRequisites.HeadPositionName = string.Empty;
            }
            catch (BLLException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ImportException("Ошибка импорта реквизитов лицензиата", ex) { ParseString = reqStr };
            }
        }

        private int GetLegalForm(string legalFormName, IDomainDbManager db)
        {
            try
            {
                legalFormName = legalFormName.Trim().ToUpper();
                var legalForm =
                    db.GetDomainTable<LegalForm>().SingleOrDefault(t => t.Name == legalFormName);

                if (legalForm == null)
                {
                    legalForm = LegalForm.CreateInstance();
                    legalForm.Name = legalFormName;

                    db.SaveDomainObject(legalForm);
                }

                return legalForm.Id;
            }
            catch (Exception ex)
            {
                throw new ImportException("Ошибка получения ОПФ лицензиата", ex) { ParseString = legalFormName };
            }
        }

        #endregion


        #region Parse Dossier

        private LicenseDossier SaveDossier(DataRow licRow, LicenseHolder holder, IDomainDbManager db)
        {
            try
            {
                var regNumber = FullTrim(licRow[0].ToString());
                var dossier = db.GetDomainTable<LicenseDossier>()
                                 .Where(t => t.RegNumber == regNumber)
                                 .ToList()
                                 .Select(t => db.RetrieveDomainObject<LicenseDossier>(t.Id))
                                 .SingleOrDefault();
                if (dossier != null)
                {
                    if (dossier.LicenseHolderId != holder.Id)
                    {
                        throw new Exception(string.Format("Для дела id=[{0}] Ожидалось HolderId=[{1}], а получение [{2}]", dossier.Id, holder.Id, dossier.LicenseHolderId));
                    }

                    return dossier;
                }

                dossier = LicenseDossier.CreateInstance();
                dossier.IsActive = true;
                dossier.LicensedActivityId = GetLicenseActivityId(licRow);
                dossier.RegNumber = regNumber;
                dossier.CreateDate = holder.ActualRequisites.CreateDate;
                dossier.LicenseHolderId = holder.Id;
                dossier.LicenseHolder = holder;

                _protocoller.Protocol(dossier);
                return _dossierMapper.Save(dossier, db);
            }
            catch (BLLException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ImportException("Ошибка импорта данных лицензионного дела", ex) { Row = licRow }; 
            }

        }

        private int GetLicenseActivityId(DataRow licRow)
        {
            try
            {
                var activitiesStr = licRow[17].ToString();
                var activityStr = activitiesStr.Split(':').First().Trim().ToUpper();
                var licActivity =
                    _dictionaryManager
                        .GetDictionary<LicensedActivity>()
                        .SingleOrDefault(t => t.Name.Trim().ToUpper() == activityStr);

                if (licActivity == null)
                {
                    throw new Exception("Не найдено лицензируемой деятельности с именем: " + activityStr);
                }

                return licActivity.Id;
            }
            catch (Exception ex)
            {
                throw new ImportException("Ошибка получения лицензируемой деятельности", ex) { Row = licRow }; 
            }
        }

        #endregion


        #region Parse License

        private License SaveLicense(DataRow licRow, LicenseDossier dossier, LicenseHolder holder, IDomainDbManager db)
        {
            try
            {
                var regNumber = FullTrim(licRow[18].ToString());

                var license = db.GetDomainTable<License>()
                                 .Where(t => t.RegNumber == regNumber)
                                 .ToList()
                                 .Select(t => db.RetrieveDomainObject<License>(t.Id))
                                 .SingleOrDefault();

                if (license == null)
                {
                    license = License.CreateInstance();
                    license.RegNumber = regNumber;
                    license.LicensedActivityId = dossier.LicensedActivityId;
                    license.LicenseDossierId = dossier.Id;
                    license.LicensiarHeadName = licRow[3].ToString().Trim();

                    if (string.IsNullOrEmpty(licRow[20].ToString()))
                    {
                        license.GrantDate = null;
                    }
                    else
                    {
                        DateTime grantDate;
                        if (DateTime.TryParse(licRow[20].ToString().Trim(), out grantDate))
                        {
                            license.GrantDate = grantDate;
                        }
                        else
                        {
                            license.Note += string.Format("[[{0}][{1}]]", "Дата начала действия лицензии", licRow[20]);
                        }
                    }

                    if (licRow[21].ToString().Trim().ToUpper() == "БЕССРОЧНО"
                        ||
                        string.IsNullOrEmpty(licRow[21].ToString().Trim()))
                    {
                        license.DueDate = null;
                    }
                    else
                    {
                        DateTime dueDate;
                        if (DateTime.TryParse(licRow[21].ToString().Trim(), out dueDate))
                        {
                            license.DueDate = dueDate;
                        }
                        else
                        {
                            license.Note += string.Format("[[{0}][{1}]]", "Дата окончания действия лицензии", licRow[21]);
                        }
                    }

                    string orderStr = licRow[19].ToString().Trim();
                    if (!string.IsNullOrEmpty(orderStr))
                    {
                        license.GrantOrderStamp = Convert.ToDateTime(orderStr.Substring(0, 10));
                        license.GrantOrderRegNumber = orderStr.Split(':').Last();
                    }

                    GetBlankData(licRow, license);
                }

                var licenceRequisites = holder.ActualRequisites.ToLicenseRequisites();
                licenceRequisites.LicenseId = license.Id;
                var address = licenceRequisites.Address;
                licenceRequisites = _synchronizer.SyncRequisites(licenceRequisites, db);
                licenceRequisites.Address = _synchronizer.SyncLicenseAddress(licenceRequisites, address, db);
                license.LicenseRequisitesList.Add(licenceRequisites);
                SetupLicenseStatusData(licRow, license);

                license.Note = license.Note.Trim();
                _protocoller.Protocol(license);
                return _licenseMapper.Save(license, db);
            }
            catch (BLLException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ImportException("Ошибка импорта данных лицензии", ex) { Row = licRow };
            }

        }

        private void SetupLicenseStatusData(DataRow licRow, License license)
        {
            try
            {
                license.CurrentStatus = LicenseStatusType.Project;

                var expired = licRow[21].ToString().Trim();
                var renewal = licRow[22].ToString().Trim();
                var voided = licRow[25].ToString().Trim();
                var stop = licRow[26].ToString().Trim();
                var suspend = licRow[23].ToString().Trim();
                var restart = licRow[24].ToString().Trim();

                if (!string.IsNullOrEmpty(expired))
                    license.Note += string.Format("[Дата окончания действия лицензии:[{0}];]", expired);
                if (!string.IsNullOrEmpty(renewal))
                    license.Note += string.Format("[Сведения о переоформлении документа, подтверждающего наличие лицензии:[{0}];]", renewal);
                if (!string.IsNullOrEmpty(voided))
                    license.Note += string.Format("[Сведения об аннулировании лицензии:[{0}];]", voided);
                if (!string.IsNullOrEmpty(stop))
                    license.Note += string.Format("[Сведения о прекращении действия лицензии:[{0}];]", stop);
                if (!string.IsNullOrEmpty(suspend))
                    license.Note += string.Format("[Сведения о приостановлении действия лицензии:[{0}];]", suspend);
                if (!string.IsNullOrEmpty(restart))
                    license.Note += string.Format("[Сведения о возобновлении действия лицензии:[{0}];]", restart);
            }
            catch (Exception ex)
            {
                throw new ImportException("Ошибка получения статуса лицензии", ex) { Row = licRow }; 
            }

        }

        private void GetBlankData(DataRow licRow, License license)
        {
            try
            {
                var blankStr = licRow[27].ToString().Trim();
                if (!string.IsNullOrEmpty(blankStr))
                {
                    var blankStart = blankStr.IndexOf("Бланк:", StringComparison.Ordinal) + "Бланк:".Length;
                    var stampStrat = blankStr.IndexOf("Дата выдачи:", StringComparison.Ordinal);
                    if (blankStart != -1 && stampStrat != -1)
                    {
                        license.BlankNumber = blankStr.Substring(blankStart, stampStrat - blankStart).Trim();
                    }

                    license.Note += string.Format("[Сведения о документе, подтверждающем наличие лицензии:[{0}];]",
                        blankStr);
                }
            }
            catch (Exception ex)
            {
                throw new ImportException("Ошибка получения данных бланка лицензии", ex) { Row = licRow }; 
            }

        }

        #endregion


        #region Parse LicenseObject

        private void SaveLicenseObject(DataRow licObjRow, License license, IDomainDbManager db)
        {
            try
            {
                var licObject = LicenseObject.CreateInstance();
                licObject.LicenseId = license.Id;
                licObject.Name = string.Empty;
                licObject.Note = licObjRow[17].ToString().Trim();

                if (string.IsNullOrEmpty(licObject.Note))
                {
                    return;
                }

                var statuses = _dictionaryManager.GetDictionary<LicenseObjectStatus>();

                if (license.GrantOrderStamp.HasValue)
                {
                    licObject.LicenseObjectStatusId = statuses.Single(t => t.Name == "Проект").Id;
                }
                else
                {
                    licObject.LicenseObjectStatusId = statuses.Single(t => t.Name == "Актуальный").Id;
                }

                licObject.Address = GetAddress(licObjRow, 13, 12, 15, 14, 16);
                licObject = _synchronizer.SyncLicenseObject(licObject, db);

                var syncAddress = _synchronizer.SyncLicenseObjectAddress(licObject, GetAddress(licObjRow, 13, 12, 15, 14, 16), db);

                if ((licObject.Id != 0 && syncAddress.Id == 0) || (licObject.Id == 0 && syncAddress.Id != 0))
                {
                    throw new ImportException("Ошибка синхронизации данных объекта лицензии и адреса") { Row = licObjRow };
                }

                licObject.Address = syncAddress;
                licObject.AddressId = syncAddress.Id;

                _protocoller.Protocol(licObject);
                _licObjectMapper.Save(licObject, db, true);
            }
            catch (BLLException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ImportException("Ошибка импорта данных объекта лицензии", ex) { Row = licObjRow };
            }
        }

        #endregion


        #region Util

        private void CancelCheck(IDomainDbManager db)
        {
            if (CancellationRequested)
            {
                db.RollbackDomainTransaction();
                throw new CancelImportException();
            }
        }

        private string FullTrim(string str)
        {
            return Regex.Replace(str, "[^а-яА-яa-zA-Z0-9_.-]+", "", RegexOptions.Compiled);
        }

        #endregion

        internal class CancelImportException : Exception
        {
        }
    }
}
