using System.Collections.Generic;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.Types.Exceptions;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.AcceptTask;
using GU.MZ.BL.DomainLogic.Licensing.Renewal;
using GU.MZ.BL.DomainLogic.MzTask;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.DomainLogic.GuParse
{
    /// <summary>
    /// Фасад подсистемы парса данных заявки
    /// Фасад выбирает реализацию парсера согласно типу услуги
    /// </summary>
    public class ParserFacade : IParser
    {
        private readonly IEnumerable<IParserImpl> _parserImpls;

        public ParserFacade(IEnumerable<IParserImpl> parserImpls)
        {
            _parserImpls = parserImpls;
        }

        private IParserImpl GetImplementation(Task task)
        {
            var mzTask = MzTaskContext.Current.MzTask(task);
            return GetImplementation(mzTask.LicenseServiceType);
        }

        private IParserImpl GetImplementation(Content content)
        {
            var tag = content.Spec.Uri;
            if (tag.StartsWith("mz/lic/services/drug/"))
            {
                return GetImplementation(LicenseServiceType.Drug);
            }
            if (tag.StartsWith("mz/lic/services/med/"))
            {
                return GetImplementation(LicenseServiceType.Med);
            }
            if (tag.StartsWith("mz/lic/services/farm/"))
            {
                return GetImplementation(LicenseServiceType.Farm);
            }

            throw new BLLException(string.Format("Неизвествнй Uri услуги: {0}", tag));
        }

        private IParserImpl GetImplementation(LicenseServiceType licenseServiceType)
        {
            var parsers = _parserImpls.Where(t => t.LicenseServiceType == licenseServiceType);

            if (!parsers.Any())
            {
                throw new NoParserForLicenseTypeException(licenseServiceType);
            }

            if (parsers.Count() > 1)
            {
                throw new MultipleParserForLicenseTypeException(licenseServiceType);
            }

            return parsers.Single();
        }

        public LicenseHolder ParseHolder(Task task)
        {
            return GetImplementation(task).ParseHolder(task);
        }

        public HolderInfo ParseHolderInfo(Task task)
        {
            return GetImplementation(task).ParseHolderInfo(task);
        }

        public License ParseLicense(Task task)
        {
            return GetImplementation(task).ParseLicense(task);
        }

        public LicenseInfo ParseLicenseInfo(Task task)
        {
            return GetImplementation(task).ParseLicenseInfo(task);
        }

        public EditableList<LicenseObject> ParseLicenseObjects(Task task)
        {
            return GetImplementation(task).ParseLicenseObjects(task);
        }

        public Dictionary<ContentNode, RenewalType> ParseRenewalData(Task task)
        {
            return GetImplementation(task).ParseRenewalData(task);
        }

        public EditableList<LicenseObject> ParseRenewalLicenseObjects(ContentNode renewalNode)
        {
            return GetImplementation(renewalNode.Content).ParseRenewalLicenseObjects(renewalNode);
        }
    }
}
