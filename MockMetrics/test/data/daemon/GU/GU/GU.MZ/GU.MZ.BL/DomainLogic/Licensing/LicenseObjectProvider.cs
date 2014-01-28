using System.Collections.Generic;
using System.Linq;
using BLToolkit.EditableObjects;
using GU.DataModel;
using GU.MZ.BL.DomainLogic.GuParse;
using GU.MZ.BL.DomainLogic.MzTask;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.BL.DomainLogic.Licensing
{
    public interface ILicenseObjectProvider
    {
        EditableList<LicenseObject> GetNewLicenseObjects(Task task);
    }
    
    public class LicenseObjectProvider : ILicenseObjectProvider
    {
        private readonly IEnumerable<IParserImpl> _parsers;

        public LicenseObjectProvider(IEnumerable<IParserImpl> parsers)
        {
            _parsers = parsers;
        }

        public EditableList<LicenseObject> GetNewLicenseObjects(Task task)
        {
            var mzTask = MzTaskContext.Current.MzTask(task);
            var parserImpl = _parsers.Single(t => t.LicenseServiceType == mzTask.LicenseServiceType);
            return parserImpl.ParseLicenseObjects(task);
        }
    }
}