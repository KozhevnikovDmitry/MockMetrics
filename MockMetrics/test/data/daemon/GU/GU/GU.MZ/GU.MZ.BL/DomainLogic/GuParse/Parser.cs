using System;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.EditableObjects;
using GU.BL.Import;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.AcceptTask;
using GU.MZ.BL.DomainLogic.Licensing.Renewal;
using GU.MZ.BL.DomainLogic.MzTask;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.DomainLogic.GuParse
{
    /// <summary>
    /// Базовый парсер данных заявок
    /// </summary>
    public abstract class  Parser : IParserImpl
    {
        protected readonly IContentImporter ContentImporter;
        protected readonly IContentMapper ContentMapper;

        public Parser(IContentImporter contentImporter, IContentMapper contentMapper)
        {
            ContentImporter = contentImporter;
            ContentMapper = contentMapper;
        }

        #region IParser Implementation

        public LicenseHolder ParseHolder(Task task)
        {
            var holderNode = GetHolderNode(task);
            var addressNode = ContentImporter.GetChildContentNodeEndsWith(holderNode, "Address");
            var holder = ContentMapper.MapHolder(holderNode);
            var requisites = ContentMapper.MapRequisites(holderNode);
            holder.RequisitesList = new EditableList<HolderRequisites> { requisites };
            requisites.LicenseHolder = holder;
            requisites.Address = ContentMapper.MapAddress(addressNode);
            return holder;
        }

        public HolderInfo ParseHolderInfo(Task task)
        {
            var holderNode = GetHolderNode(task);
            return ContentMapper.MapHolderInfo(holderNode);
        }

        public License ParseLicense(Task task)
        {
            throw new System.NotImplementedException();
        }

        public LicenseInfo ParseLicenseInfo(Task task)
        {
            var licenseNode = GetLicenseNode(task);
            return ContentMapper.MapLicenseInfo(licenseNode);
        }

        public EditableList<LicenseObject> ParseLicenseObjects(Task task)
        {
            var licObjectNodes = GetLincenseObjectNodes(task);
            return ParseLicenseObjects(licObjectNodes);
        }

        public Dictionary<ContentNode, RenewalType> ParseRenewalData(Task task)
        {
            var result = new Dictionary<ContentNode, RenewalType>();
            var renewalScenarioNodes = GetRenewalTypeNodes(task);
            foreach (var renewalScenarioNode in renewalScenarioNodes)
            {
                var enumValues = Enum.GetValues(typeof(RenewalType)).OfType<RenewalType>();
                if (enumValues.Any(t => t.ToString() == renewalScenarioNode.SpecNode.Tag))
                {
                    result[renewalScenarioNode] = enumValues.Single(t => t.ToString() == renewalScenarioNode.SpecNode.Tag);
                }
                else
                {
                    result[renewalScenarioNode] = RenewalType.None;
                }
            }

            return result;
        }

        public EditableList<LicenseObject> ParseRenewalLicenseObjects(ContentNode renewalNode)
        {
            var licObjectNodes = ContentImporter.GetContentNodes(renewalNode, "WorksInfo");
            return ParseLicenseObjects(licObjectNodes);
        }

        #endregion

        #region Service Methods

        protected EditableList<LicenseObject> ParseLicenseObjects(IEnumerable<ContentNode> licObjectNodes)
        {
            var result = new EditableList<LicenseObject>();
            foreach (var licObjectNode in licObjectNodes)
            {
                result.Add(ParseLicenseObject(licObjectNode));
            }

            return result;
        }

        public virtual LicenseObject ParseLicenseObject(ContentNode licObjNode)
        {
            var licenseObject = LicenseObject.CreateInstance();
            var addressNode = ContentImporter.GetContentNode(licObjNode, "ActivityAddress");
            licenseObject.Address = ContentMapper.MapAddress(addressNode);
            CompleteLicenseObject(licObjNode, licenseObject);
            return licenseObject;
        }

        #endregion

        #region Abstracts To Implement

        protected abstract IEnumerable<ContentNode> GetRenewalTypeNodes(Task task);

        public abstract LicenseServiceType LicenseServiceType { get; }

        public abstract ContentNode GetHolderNode(Task task);

        public abstract ContentNode GetLicenseNode(Task task);

        public abstract IEnumerable<ContentNode> GetLincenseObjectNodes(Task task);

        public abstract void CompleteLicenseObject(ContentNode licObjNode, LicenseObject licenseObject);

        #endregion
    }
}