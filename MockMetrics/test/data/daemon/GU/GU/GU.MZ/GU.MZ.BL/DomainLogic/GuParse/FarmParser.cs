using GU.BL.Import;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.MzTask;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.DomainLogic.GuParse
{
    /// <summary>
    /// Парсер данных заявок по фарм. деятельности
    /// </summary>
    public class FarmParser : BasicParser
    {
        public FarmParser(IContentImporter contentImporter, IContentMapper contentMapper)
            : base(contentImporter, contentMapper)
        {
        }

        public override LicenseServiceType LicenseServiceType
        {
            get { return LicenseServiceType.Farm; }
        }

        public override void CompleteLicenseObject(ContentNode licObjNode, LicenseObject licenseObject)
        {
            var worksNode = ContentImporter.GetContentNode(licObjNode, "Works");
            foreach (var workNode in worksNode.ChildContentNodes)
            {
                if (workNode.BoolValue.HasValue && workNode.BoolValue.Value)
                {
                    licenseObject.Note += workNode.SpecNode.Name + ", ";
                }
            }

            if (!string.IsNullOrEmpty(licenseObject.Note) && licenseObject.Note.Length > 2)
            {
                licenseObject.Note = licenseObject.Note.Substring(0, licenseObject.Note.Length - 2);
            }

            licenseObject.Name = ContentImporter.GetNodeStrValue(licObjNode, "Subject");

        }
    }
}