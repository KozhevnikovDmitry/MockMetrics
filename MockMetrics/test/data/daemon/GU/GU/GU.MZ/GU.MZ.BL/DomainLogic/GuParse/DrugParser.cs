using System.Collections.Generic;
using GU.BL.Import;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.MzTask;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.DomainLogic.GuParse
{
    /// <summary>
    /// Парсер данных заявок по нарк. деятельности
    /// </summary>
    public class DrugParser : Parser
    {
        public DrugParser(IContentImporter contentImporter, IContentMapper contentMapper)
            : base(contentImporter, contentMapper)
        {
        }

        protected override IEnumerable<ContentNode> GetRenewalTypeNodes(Task task)
        {
            var baseNode = ContentImporter.GetContentNode(task.RootContentNode, "Base");
            if (baseNode == null)
            {
                throw new NoRenewalScenarioException(task);
            }

            return baseNode.ChildContentNodes;
        }

        public override LicenseServiceType LicenseServiceType
        {
            get { return LicenseServiceType.Drug; }
        }

        public override ContentNode GetHolderNode(Task task)
        {
            return ContentImporter.GetContentNode(task.RootContentNode, "OrgInfo");
        }

        public override ContentNode GetLicenseNode(Task task)
        {
            return ContentImporter.GetContentNode(task.RootContentNode, "LicenseInfo");
        }

        public override IEnumerable<ContentNode> GetLincenseObjectNodes(Task task)
        {
            var orgInfo = GetHolderNode(task);
            return ContentImporter.GetContentNodes(orgInfo, "WorksInfo");
        }

        public override LicenseObject ParseLicenseObject(ContentNode licObjNode)
        {
            var licenseObject = LicenseObject.CreateInstance();
            var addressNode = ContentImporter.GetContentNode(licObjNode, "Address");
            licenseObject.Address = ContentMapper.MapAddress(addressNode);
            CompleteLicenseObject(licObjNode, licenseObject);
            return licenseObject;
        }

        public override void CompleteLicenseObject(ContentNode licObjNode, LicenseObject licenseObject)
        {
            var worksNodes = ContentImporter.GetContentNodes(licObjNode, "Works");
            foreach (var contentNode in worksNodes)
            {
                licenseObject.Note += contentNode.StrValue + ", ";
            }

            if (!string.IsNullOrEmpty(licenseObject.Note) && licenseObject.Note.Length > 2)
            {
                licenseObject.Note = licenseObject.Note.Substring(0, licenseObject.Note.Length - 2);
            }

            licenseObject.Address.Note += ContentImporter.GetNodeStrValue(licObjNode, "ApartmentDescription");
        }
    }
}