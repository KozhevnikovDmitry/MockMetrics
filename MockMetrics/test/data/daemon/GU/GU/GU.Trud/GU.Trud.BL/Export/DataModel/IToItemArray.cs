namespace GU.Trud.BL.Export.DataModel
{
    /// <summary>
    /// Интерфейс для классов хранящих запись экспортных данных.
    /// </summary>
    internal interface IToItemArray
    {
        /// <summary>
        /// Возвращает массив значений записи.
        /// </summary>
        /// <returns>Mассив значений записи</returns>
        object[] ToItemArray();

        /// <summary>
        /// Возвращает массив имён полей запиcи.
        /// </summary>
        /// <returns>Массив имён полей запиcи</returns>
        string[] ToItemNameArray();
    }
}
