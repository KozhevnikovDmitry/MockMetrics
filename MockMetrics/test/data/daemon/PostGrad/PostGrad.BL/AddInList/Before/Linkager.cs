using System;
using System.Collections.Generic;
using System.Linq;
using PostGrad.Core.BL;
using PostGrad.Core.DA;
using PostGrad.Core.DomainModel.Dossier;
using PostGrad.Core.DomainModel.FileScenario;
using PostGrad.Core.DomainModel.Holder;

namespace PostGrad.BL.AddInList.Before
{
    public class Linkager : ILinkager 
    {
        private readonly Func<IDomainDbManager> _dbFactory;
        private readonly ILicenseHolderRepository _holderRepository;
        private readonly ILicenseDossierRepository _dossierRepository;
        private readonly ITaskParser _taskTaskParser;

        public Linkager(Func<IDomainDbManager> dbFactory,
                        ILicenseHolderRepository holderRepository,
                        ILicenseDossierRepository dossierRepository,
                        ITaskParser taskTaskParser)
        {
            _dbFactory = dbFactory;
            _holderRepository = holderRepository;
            _dossierRepository = dossierRepository;
            _taskTaskParser = taskTaskParser;
        }

        public IDossierFileLinkWrapper Linkage(DossierFile dossierFile)
        {
            using (var db = _dbFactory())
            {
                var wrapper = new DossierFileLinkWrapper(dossierFile);
                LinkageHolder(wrapper, db);
                LinkageRequisites(wrapper);
                LinkageDossier(wrapper, db);
                LinkageLicense(wrapper);
                CheckHolderData(wrapper);
                SetupDossierFile(wrapper, db);

                return wrapper;
            }
        }

        private void LinkageHolder(IDossierFileLinkWrapper fileLink, IDomainDbManager dbManager)
        {
            var holderInfo = _taskTaskParser.ParseHolderInfo(fileLink.DossierFile.Task);

            if (_holderRepository.HolderExists(holderInfo.Inn, holderInfo.Ogrn, dbManager))
            {
                fileLink.LicenseHolder = _holderRepository.GetLicenseHolder(holderInfo.Inn, holderInfo.Ogrn, dbManager);
            }
            else
            {
                if (fileLink.DossierFile.ScenarioType == ScenarioType.Light)
                {
                    throw new NoExistingHolderForLightSceanrioException();
                }

                fileLink.LicenseHolder = _taskTaskParser.ParseHolder(fileLink.DossierFile.Task);
            }
        }

        private void LinkageRequisites(IDossierFileLinkWrapper fileLink)
        {
            fileLink.AvailableRequisites = new Dictionary<RequisitesOrigin, HolderRequisites>();
            if (fileLink.LicenseHolder.Id == 0)
            {
                fileLink.AvailableRequisites[RequisitesOrigin.FromTask] =
                    fileLink.LicenseHolder.RequisitesList.OrderByDescending(t => t.CreateDate).First();
            }
            else
            {
                if (fileLink.DossierFile.ScenarioType == ScenarioType.Full)
                {
                    fileLink.AvailableRequisites[RequisitesOrigin.FromTask]
                        = _taskTaskParser.ParseHolder(fileLink.DossierFile.Task).RequisitesList.Single();
                }

                fileLink.AvailableRequisites[RequisitesOrigin.FromRegistr] =
                    fileLink.LicenseHolder.RequisitesList.OrderByDescending(t => t.CreateDate).First();
            }

            fileLink.SelectedRequisites = fileLink.AvailableRequisites.First().Value;
        }

        private void LinkageDossier(IDossierFileLinkWrapper fileLink, IDomainDbManager dbManager)
        {
            if (_dossierRepository.DossierExists(fileLink.DossierFile.LicensedActivity.Id,
                                            fileLink.LicenseHolder.Id, dbManager))
            {
                fileLink.LicenseDossier = _dossierRepository.GetLicenseDossier(fileLink.DossierFile.LicensedActivity.Id,
                                                                  fileLink.LicenseHolder.Id, dbManager);
            }
            else
            {
                if (fileLink.DossierFile.ScenarioType == ScenarioType.Light)
                {
                    throw new NoExistingDossierForLightSceanrioException();
                }

                fileLink.LicenseDossier = LicenseDossier.CreateInstance(fileLink.DossierFile.LicensedActivity, fileLink.LicenseHolder);
            }
        }

        private void LinkageLicense(IDossierFileLinkWrapper fileLink)
        {
            if (fileLink.DossierFile.IsNewLicense)
            {
                return;
            }

            var licenseInfo = _taskTaskParser.ParseLicenseInfo(fileLink.DossierFile.Task);
            var licenses =
                fileLink.LicenseDossier.LicenseList.Where(
                    t => t.RegNumber.Trim().ToUpper() == licenseInfo.RegNumber.Trim().ToUpper()
                      && t.GrantDate == licenseInfo.GrantDate).ToList();

            if (licenses.Count > 1)
            {
                throw new TooMoreLinkagingLicesensesException(fileLink.LicenseDossier, licenseInfo.RegNumber, licenseInfo.GrantDate);
            }

            fileLink.License = licenses.SingleOrDefault();
        }

        private void CheckHolderData(IDossierFileLinkWrapper fileLink)
        {
            if (fileLink.LicenseHolder == null)
            {
                fileLink.IsHolderDataDoubtfull = false;
                return;
            }

            var holderInfо = _taskTaskParser.ParseHolderInfo(fileLink.DossierFile.Task);

            fileLink.IsHolderDataDoubtfull = holderInfо.Inn != fileLink.LicenseHolder.Inn
                                             || holderInfо.Ogrn != fileLink.LicenseHolder.Ogrn;
        }

        private void SetupDossierFile(IDossierFileLinkWrapper fileLink, IDomainDbManager dbManager)
        {
            fileLink.DossierFile.RegNumber = _dossierRepository.GetNextFileRegNumber(fileLink.LicenseDossier, dbManager);
            fileLink.DossierFile.CurrentStatus = DossierFileStatus.Active;
        }
    }
}
