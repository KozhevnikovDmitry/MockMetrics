using System;
using System.Linq;
using Common.BL.DictionaryManagement;
using GU.BL.Import;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.AcceptTask;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Requisites;

namespace GU.MZ.BL.DomainLogic.GuParse
{
    public class ContentMapper : IContentMapper
    {
        private readonly IContentImporter _contentImporter;
        private readonly IDictionaryManager _dictionaryManager;

        public ContentMapper(IContentImporter contentImporter, IDictionaryManager dictionaryManager)
        {
            _contentImporter = contentImporter;
            _dictionaryManager = dictionaryManager;
        }

        public LicenseHolder MapHolder(ContentNode holderNode)
        {
            var holder = LicenseHolder.CreateInstance();

            string inn = string.Empty;
            string ogrn = string.Empty;
            MapInnAndOgrn(holderNode, out inn, out ogrn);
            holder.Inn = inn;
            holder.Ogrn = ogrn;
            return holder;
        }

        private void MapInnAndOgrn(ContentNode holderNode, out string inn, out string ogrn)
        {
            var innNode = _contentImporter.GetContentNode(holderNode, "INN");
            var ogrnNode = _contentImporter.GetContentNode(holderNode, "OGRN");

            if (innNode.SpecNode.SpecNodeType != SpecNodeType.Simple)
            {
                innNode = _contentImporter.GetChildContentNodeStartsWith(innNode, "INN");
            }

            if (ogrnNode.SpecNode.SpecNodeType != SpecNodeType.Simple)
            {
                ogrnNode = _contentImporter.GetChildContentNodeStartsWith(ogrnNode, "OGRN");
            }

            inn = innNode.StrValue;
            ogrn = ogrnNode.StrValue;
        }

        public HolderRequisites MapRequisites(ContentNode holderNode)
        {
            var requisites = HolderRequisites.CreateInstance();
            if (holderNode.SpecNode.Tag.EndsWith("OrgInfo"))
            {
                var jurRequisites = JurRequisites.CreateInstance();

                jurRequisites.FullName = _contentImporter.GetNodeStrValue(holderNode, "FullName");
                jurRequisites.ShortName = _contentImporter.GetNodeStrValue(holderNode, "ShortName");
                jurRequisites.FirmName = _contentImporter.GetNodeStrValue(holderNode, "FirmName");
                jurRequisites.LegalFormId = GetLegalFormId(holderNode);

                var chiefNode = _contentImporter.GetContentNode(holderNode, "Director");
                jurRequisites.HeadPositionName = _contentImporter.GetNodeStrValue(chiefNode, "Position");
                var chiefLastName = _contentImporter.GetNodeStrValue(chiefNode, "ChiefLastName");
                var chiefFirstName = _contentImporter.GetNodeStrValue(chiefNode, "ChiefFirstName");
                var chiefMiddleName = _contentImporter.GetNodeStrValue(chiefNode, "ChiefMiddleName");
                jurRequisites.HeadName = string.Format("{0} {1}.{2}.", chiefLastName, chiefFirstName.First(),
                    chiefMiddleName.First());

                requisites.JurRequisites = jurRequisites;
            }
            else
            {
                var indRequisites = IndRequisites.CreateInstance();
                indRequisites.Surname = _contentImporter.GetNodeStrValue(holderNode, "FamilyName");
                indRequisites.Name = _contentImporter.GetNodeStrValue(holderNode, "FirstName");
                indRequisites.Patronymic = _contentImporter.GetNodeStrValue(holderNode, "Patronymic");

                var docuemntNode = _contentImporter.GetContentNode(holderNode, "Document");
                indRequisites.Serial = _contentImporter.GetNodeStrValue(docuemntNode, "Series");
                indRequisites.Number = _contentImporter.GetNodeStrValue(docuemntNode, "Number");
                var stamp = _contentImporter.GetNodeDateValue(docuemntNode, "IssueDate");
                indRequisites.Stamp = stamp.HasValue ? stamp.Value : new DateTime();
                indRequisites.Organization = _contentImporter.GetNodeStrValue(docuemntNode, "Issuer");
                requisites.IndRequisites = indRequisites;
            }

            return requisites;
        }

        private int GetLegalFormId(ContentNode orgInfo)
        {
            string legalFormName = _contentImporter.GetNodeStrValue(orgInfo, "LawForm");
            return GetLegalFormId(legalFormName);
        }

        private int GetLegalFormId(string legalFormName)
        {
            var legalForm =
                _dictionaryManager.GetDictionary<LegalForm>().SingleOrDefault(
                    l => l.Name.Trim().ToUpper() == legalFormName.Trim().ToUpper());

            return legalForm != null ? legalForm.Id : _dictionaryManager.GetDictionary<LegalForm>().First().Id;
        }

        public Address MapAddress(ContentNode addressNode)
        {
            var address = Address.CreateInstance();
            address.Zip = _contentImporter.GetNodeStrValue(addressNode, "PostalCode");
            address.CountryRegion = _contentImporter.GetNodeStrValue(addressNode, "Region");
            address.Area = _contentImporter.GetNodeStrValue(addressNode, "District");
            address.City = _contentImporter.GetNodeStrValue(addressNode, "CityLocality");
            address.Street = _contentImporter.GetNodeStrValue(addressNode, "StreetText");
            address.House = _contentImporter.GetNodeStrValue(addressNode, "ApartmentBlock");
            address.Flat = _contentImporter.GetNodeStrValue(addressNode, "Apartment");
            address.Build = _contentImporter.GetNodeStrValue(addressNode, "Building");
            return address;
        }

        public HolderInfo MapHolderInfo(ContentNode holderNode)
        {
            var holderInfo = new HolderInfo();

            if (_contentImporter.HasContentNode(holderNode, "FullName"))
            {
                holderInfo.FullName = _contentImporter.GetNodeStrValue(holderNode, "FullName");
            }
            else
            {
                holderInfo.FullName = string.Format("{0} {1}.{2}.", _contentImporter.GetNodeStrValue(holderNode, "FamilyName"),
                    _contentImporter.GetNodeStrValue(holderNode, "FirstName").FirstOrDefault(), 
                    _contentImporter.GetNodeStrValue(holderNode, "Patronymic").FirstOrDefault());
            }

            string inn = string.Empty;
            string ogrn = string.Empty;
            MapInnAndOgrn(holderNode, out inn, out ogrn);
            holderInfo.Inn = inn;
            holderInfo.Ogrn = ogrn;
            return holderInfo;
        }

        public LicenseInfo MapLicenseInfo(ContentNode licenseNode)
        {
            return new LicenseInfo
            {
                RegNumber = _contentImporter.GetNodeStrValue(licenseNode, "LicenseNumber"),
                GrantDate = _contentImporter.GetNodeDateValue(licenseNode, "LicenseDate").Value,
                Licensiar = _contentImporter.GetNodeStrValue(licenseNode, "LicenseOrg")
            };
        }

        public HolderRequisites MapJurRenewalRequisites(ContentNode renewalNode)
        {
            var results = HolderRequisites.CreateInstance();
            results.JurRequisites = JurRequisites.CreateInstance();
            results.JurRequisites.FullName = _contentImporter.GetNodeStrValue(renewalNode, "FullName");
            results.JurRequisites.LegalFormId = GetLegalFormId(_contentImporter.GetNodeStrValue(renewalNode, "LawForm"));
            results.JurRequisites.ShortName = _contentImporter.GetContentNode(renewalNode, "ShortName") != null ? _contentImporter.GetNodeStrValue(renewalNode, "ShortName") : null;
            results.JurRequisites.FirmName = _contentImporter.GetContentNode(renewalNode, "FirmName") != null ? _contentImporter.GetNodeStrValue(renewalNode, "FirmName") : null;
            return results;
        }

        public HolderRequisites MapIndRenewalRequisites(ContentNode renewalNode)
        {
            var results = HolderRequisites.CreateInstance();
            results.IndRequisites = IndRequisites.CreateInstance();
            results.IndRequisites.Surname = _contentImporter.GetNodeStrValue(renewalNode, "FamilyName");
            results.IndRequisites.Name = _contentImporter.GetNodeStrValue(renewalNode, "FirstName");
            results.IndRequisites.Patronymic = _contentImporter.GetContentNode(renewalNode, "Patronymic") != null ? _contentImporter.GetNodeStrValue(renewalNode, "Patronymic") : null;
            return results;
        }
    }
}