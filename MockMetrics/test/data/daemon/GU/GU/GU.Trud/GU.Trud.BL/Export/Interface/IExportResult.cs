using Common.Types.Exceptions;

namespace GU.Trud.BL.Export.Interface
{
    /// <summary>
    /// Интерфейс результатов поиска
    /// </summary>
    public interface IExportResult
    {
        /// <summary>
        /// Возвращает флаг ошибочного завершения работы.
        /// </summary>
        bool IsFailed { get;  }

        /// <summary>
        /// Объект исключение с информацией об ошибке.
        /// </summary>
        GUException Exception { get;  }
    }
}
