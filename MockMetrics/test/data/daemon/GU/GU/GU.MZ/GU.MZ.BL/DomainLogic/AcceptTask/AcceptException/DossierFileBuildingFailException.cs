using Common.Types.Exceptions;

namespace GU.MZ.BL.DomainLogic.AcceptTask.AcceptException
{
    /// <summary>
    /// Класс исключение для ошибки "Создание тома лицензионного дела завершилось с ошибкой".
    /// </summary>
    public class DossierFileBuildingFailException : GUException
    {
        public DossierFileBuildingFailException()
            : base("Создание тома лицензионного дела завершилось с ошибкой")
        {

        }
    }
}