using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.Import
{
    public class Protocoller
    {
        public string Path { get; set; }

        private XDocument _log;

        public Protocoller(string path)
        {
            Path = path;
            Drop();
        }

        public void Protocol(LicenseHolder holder)
        {
            var xHolder = new XElement("Holder",
                new XAttribute("Id", holder.Id),
                new XAttribute("Sync", holder.Id != 0),
                new XElement("Inn", holder.Inn),
                new XElement("Ogrn", holder.Ogrn)
            );

            foreach (var holderRequisitese in holder.RequisitesList)
            {
                xHolder.Add(Protocol(holderRequisitese));
            }

            _log.Root.Add(xHolder);
        }

        private XElement Protocol(HolderRequisites requisites)
        {
            var xreq = new XElement("HolderRequisites",
                new XAttribute("Id", requisites.Id),
                new XAttribute("Sync", requisites.Id != 0),
                new XElement("LicenseHolderId", requisites.LicenseHolderId),
                new XElement("FullName", requisites.JurRequisites.FullName),
                new XElement("FirmName", requisites.JurRequisites.FirmName),
                new XElement("ShortName", requisites.JurRequisites.ShortName),
                new XElement("LegalFormId", requisites.JurRequisites.LegalFormId),
                new XElement("HeadName", requisites.JurRequisites.HeadName),
                new XElement("HeadPositionName", requisites.JurRequisites.HeadPositionName),
                new XElement("CreateDate", requisites.CreateDate),
                new XElement("Note", requisites.Note),
                Protocol(requisites.Address)
            );

            return xreq;
        }

        private XElement Protocol(Address address)
        {
            if (address == null)
            {
                address = Address.CreateInstance();
            }

            var xAddress = new XElement("Address",
                new XAttribute("Id", address.Id),
                new XAttribute("Sync", address.Id != 0),
                new XElement("Country", address.Country),
                new XElement("CountryRegion", address.CountryRegion),
                new XElement("Area", address.Area),
                new XElement("City", address.City),
                new XElement("Street", address.Street),
                new XElement("House", address.House),
                new XElement("Build", address.Build),
                new XElement("Flat", address.Flat),
                new XElement("Note", address.Note)
            );

            return xAddress;
        }

        public void Protocol(LicenseDossier dossier)
        {
            var xDossier = new XElement("LicenseDossier",
                new XAttribute("Id", dossier.Id),
                new XAttribute("Sync", dossier.Id != 0),
                new XElement("RegNumber", dossier.RegNumber),
                new XElement("CreateDate", dossier.CreateDate),
                new XElement("LicenseHolderId", dossier.LicenseHolderId),
                new XElement("LicensedActivityId", dossier.LicensedActivityId),
                new XElement("IsActive", dossier.IsActive)
            );

            _log.Root.Add(xDossier);
        }

        public void Protocol(License license)
        {
            var xLicense = new XElement("License",
               new XAttribute("Id", license.Id),
               new XAttribute("Sync", license.Id != 0),
               new XElement("RegNumber", license.RegNumber),
               new XElement("GrantDate", license.GrantDate),
               new XElement("GrantOrderRegNumber", license.GrantOrderRegNumber),
               new XElement("GrantOrderStamp", license.GrantOrderStamp),
               new XElement("BlankNumber", license.BlankNumber),
               new XElement("DueDate", license.DueDate),
               new XElement("LicenseDossierId", license.LicenseDossierId),
               new XElement("CurrentStatus", license.CurrentStatus),
               new XElement("LicensedActivityId", license.LicensedActivityId),
               new XElement("LicensiarHeadName", license.LicensiarHeadName),
               new XElement("LicensiarHeadPosition", license.LicensiarHeadPosition),
               new XElement("Note", license.Note)
           );

            foreach (var licenseStatus in license.LicenseStatusList)
            {
                xLicense.Add(Protocol(licenseStatus));
            }

            foreach (var requisites in license.LicenseRequisitesList)
            {
                xLicense.Add(Protocol(requisites));
            }

            _log.Root.Add(xLicense);
        }

        private XElement Protocol(LicenseStatus licenseStatus)
        {
            var xreq = new XElement("LicenseStatus",
                new XAttribute("Id", licenseStatus.Id),
                new XAttribute("Sync", licenseStatus.Id != 0),
                new XElement("LicenseId", licenseStatus.LicenseId),
                new XElement("Stamp", licenseStatus.Stamp),
                new XElement("Note", licenseStatus.Note)
            );

            return xreq;
        }

        private XElement Protocol(LicenseRequisites requisites)
        {
            var xreq = new XElement("LicenseRequisites",
                new XAttribute("Id", requisites.Id),
                new XAttribute("Sync", requisites.Id != 0),
                new XElement("LicenseId", requisites.LicenseId),
                new XElement("FullName", requisites.JurRequisites.FullName),
                new XElement("FirmName", requisites.JurRequisites.FirmName),
                new XElement("ShortName", requisites.JurRequisites.ShortName),
                new XElement("LegalFormId", requisites.JurRequisites.LegalFormId),
                new XElement("HeadName", requisites.JurRequisites.HeadName),
                new XElement("HeadPositionName", requisites.JurRequisites.HeadPositionName),
                new XElement("CreateDate", requisites.CreateDate),
                new XElement("Note", requisites.Note),
                Protocol(requisites.Address)
            );

            return xreq;
        }

        public void Protocol(LicenseObject licenseObject)
        {
            try
            {
                var xLicObj = new XElement("LicenseObject",
                new XAttribute("Id", licenseObject.Id),
                new XAttribute("Sync", licenseObject.Id != 0),
                new XElement("LicenseId", licenseObject.LicenseId),
                new XElement("GrantOrderRegNumber", licenseObject.GrantOrderRegNumber),
                new XElement("GrantOrderStamp", licenseObject.GrantOrderStamp),
                new XElement("LicenseObjectStatusId", licenseObject.LicenseObjectStatusId),
                new XElement("Name", licenseObject.Name),
                new XElement("Note", licenseObject.Note),
                Protocol(licenseObject.Address));
                
                _log.Root.Add(xLicObj);

            }
            catch (Exception)
            {
                _log.Root.Name = "Root";
                throw;
            }

        }

        public XDocument Save()
        {
            var holders = _log.Root.Descendants("Holder");
            var holderRequisites = _log.Root.Descendants("HolderRequisites");
            var holderRequisitesAddresses = holderRequisites.SelectMany(t => t.Descendants("Address"));
            var dossiers = _log.Root.Descendants("LicenseDossier");
            var licenses = _log.Root.Descendants("License");
            var licenseRequisites = _log.Root.Descendants("LicenseRequisites");
            var licenseRequisitesAddresses = licenseRequisites.SelectMany(t => t.Descendants("Address")); ;
            var licenseObjects = _log.Root.Descendants("LicenseObject");
            var licenseObjectAddresses = licenseObjects.SelectMany(t => t.Descendants("Address")); ;


            var totals = new XElement("Totals",
                new XElement("Holders", new XAttribute("new", NotSyncCount(holders)), new XAttribute("sync", SyncCount(holders))),
                new XElement("HolderRequisites", new XAttribute("new", NotSyncCount(holderRequisites)), new XAttribute("sync", SyncCount(holderRequisites))),
                new XElement("HolderRequisitesAddresses", new XAttribute("new", NotSyncCount(holderRequisitesAddresses)), new XAttribute("sync", SyncCount(holderRequisitesAddresses))),
                new XElement("Dossiers", new XAttribute("new", NotSyncCount(dossiers)), new XAttribute("sync", SyncCount(dossiers))),
                new XElement("Licenses", new XAttribute("new", NotSyncCount(licenses)), new XAttribute("sync", SyncCount(licenses))),
                new XElement("LicenseRequisites", new XAttribute("new", NotSyncCount(licenseRequisites)), new XAttribute("sync", SyncCount(licenseRequisites))),
                new XElement("LicenseRequisitesAddresses", new XAttribute("new", NotSyncCount(licenseRequisitesAddresses)), new XAttribute("sync", SyncCount(licenseRequisitesAddresses))),
                new XElement("LicenseObjects", new XAttribute("new", NotSyncCount(licenseObjects)), new XAttribute("sync", SyncCount(licenseObjects))),
                new XElement("LicenseObjectAddresses", new XAttribute("new", NotSyncCount(licenseObjectAddresses)), new XAttribute("sync", SyncCount(licenseObjectAddresses))));

            _log.Root.Add(totals);
            _log.Save(Path);
            return _log;
        }

        public void Drop()
        {
            _log = new XDocument();
            _log.Add(new XElement("Root"));
        }

        private int SyncCount(IEnumerable<XElement> query)
        {
            return query.Count(t => t.Attribute("Sync") != null && t.Attribute("Sync").Value == "true");
        }

        private int NotSyncCount(IEnumerable<XElement> query)
        {
            return query.Count(t => t.Attribute("Sync") != null && t.Attribute("Sync").Value == "false");
        }
    }
}
