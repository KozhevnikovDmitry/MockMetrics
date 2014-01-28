using GU.BL.Import;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.MzTask;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.DomainLogic.GuParse
{
    /// <summary>
    /// Парсер данных заявок по мед. деятельности
    /// </summary>
    public class MedParser : BasicParser
    {
        public MedParser(IContentImporter contentImporter, IContentMapper contentMapper) : base(contentImporter, contentMapper)
        {
        }

        public override LicenseServiceType LicenseServiceType
        {
            get { return LicenseServiceType.Med; }
        }

        public override void CompleteLicenseObject(ContentNode licObjNode, LicenseObject licenseObject)
        {
            licenseObject.Note = ContentImporter.GetNodeStrValue(licObjNode, "Works");
        }
    }
}