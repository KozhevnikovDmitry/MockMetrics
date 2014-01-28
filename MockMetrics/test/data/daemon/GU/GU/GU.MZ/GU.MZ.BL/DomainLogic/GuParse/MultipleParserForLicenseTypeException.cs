using Common.Types;
using Common.Types.Exceptions;
using GU.MZ.BL.DomainLogic.MzTask;

namespace GU.MZ.BL.DomainLogic.GuParse
{
    public class MultipleParserForLicenseTypeException : BLLException
    {
        public MultipleParserForLicenseTypeException(LicenseServiceType licenseServiceType)
            : base((string)string.Format("Найдено несколько механизмов импорта заявки для лицензии типа [{0}]", licenseServiceType.GetDescription()))
        {

        }
    }
}