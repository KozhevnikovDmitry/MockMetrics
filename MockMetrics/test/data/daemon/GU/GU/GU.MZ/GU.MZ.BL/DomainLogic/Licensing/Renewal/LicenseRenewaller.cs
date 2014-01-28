using System;
using System.Collections.Generic;
using System.Linq;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using Common.Types.Exceptions;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.LicenseChange;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.DomainLogic.Licensing.Renewal
{
    public class LicenseRenewaller : TransactionWrapper, ILicenseRenewaller
    {
        private readonly IParser _parser;
        private readonly IEnumerable<IRenewalScenario> _scenarios;
        private readonly IDomainDataMapper<ChangeLicenseSession> _sessionMapper;
        private readonly IDomainDataMapper<License> _licenseMapper;
        private readonly IDomainDataMapper<DossierFile> _fileMapper;
        private readonly Func<IDomainDbManager> _getDb;
        private readonly List<ChangeLicenseSession> _sessions;

        public LicenseRenewaller(IParser parser,
                                 IEnumerable<IRenewalScenario> scenarios,
                                 IDomainDataMapper<ChangeLicenseSession> sessionMapper,
                                 IDomainDataMapper<License> licenseMapper,
                                 IDomainDataMapper<DossierFile> fileMapper,
                                 Func<IDomainDbManager> getDb)
        {
            _parser = parser;
            _scenarios = scenarios;
            _sessionMapper = sessionMapper;
            _licenseMapper = licenseMapper;
            _fileMapper = fileMapper;
            _getDb = getDb;
            _sessions = new List<ChangeLicenseSession>();
        }

        public License RenewalLicense(DossierFile dossierFile)
        {
            if (_sessions.Any())
            {
                throw new BLLException("Остались несохранённые сеансы изменения лицензии");
            }

            var renewalScenaries = _parser.ParseRenewalData(dossierFile.Task);
            var renewaled = dossierFile.Clone();
            foreach (var renewalScenario in renewalScenaries)
            {
                var scenario = _scenarios.SingleOrDefault(t => t.RenewalType == renewalScenario.Value);

                if (scenario == null)
                {
                    throw new NoRenewalScenarioException(dossierFile.Task);
                }

                var session = scenario.Renewal(renewaled, renewalScenario.Key);
                _sessions.Add(session);
            }

            renewaled.CopyTo(dossierFile);

            return dossierFile.License;
        }

        public DossierFile SaveChanges(DossierFile dossierFile)
        {
            DossierFile result = null;
            using (var dbManager = _getDb())
            {

                Transaction(dbManager, db =>
                {
                    foreach (var licenseChangeSession in _sessions)
                    {
                        _sessionMapper.Save(licenseChangeSession, db);
                    }

                    _sessions.Clear();

                    dossierFile.License = _licenseMapper.Retrieve(dossierFile.LicenseId, db);
                    result = _fileMapper.Save(dossierFile, db);
                });
            }

            return result;
        }

        public bool IsRenewalled(DossierFile dossierFile)
        {
            using (var dbManager = _getDb())
            {
                return dbManager.GetDomainTable<ChangeLicenseSession>().Any(t => t.DossierFileId == dossierFile.Id);
            }
        }

        public List<string> RenewalScenaries(DossierFile dossierFile)
        {
            var renewalScenaries = _parser.ParseRenewalData(dossierFile.Task);
            return renewalScenaries.Select(t => t.Key.SpecNode.Name).ToList();
        }
    }
}