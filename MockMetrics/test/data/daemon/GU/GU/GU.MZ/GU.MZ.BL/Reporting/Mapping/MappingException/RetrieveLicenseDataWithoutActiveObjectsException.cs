using Common.Types.Exceptions;

namespace GU.MZ.BL.Reporting.Mapping.MappingException
{
    /// <summary>
    /// Класс исключений для обработки ошибок "Попытка получения данных лицензии без единого активного объекта с номенклатурой"
    /// </summary>
    public class RetrieveLicenseDataWithoutActiveObjectsException : BLLException
    {
        public RetrieveLicenseDataWithoutActiveObjectsException()
            : base("Попытка получения данных лицензии без единого активного объекта с номенклатурой")
        {
            
        }
    }
}