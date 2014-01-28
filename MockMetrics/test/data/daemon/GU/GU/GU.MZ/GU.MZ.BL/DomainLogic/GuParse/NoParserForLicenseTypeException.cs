using Common.Types;
using Common.Types.Exceptions;
using GU.MZ.BL.DomainLogic.MzTask;

namespace GU.MZ.BL.DomainLogic.GuParse
{
    public class NoParserForLicenseTypeException : BLLException
    {
        public NoParserForLicenseTypeException(LicenseServiceType licenseServiceType)
            : base(string.Format("Не найден механизм импорта заявки для лицензии типа [{0}]", licenseServiceType.GetDescription()))
        {

        }
    }
}