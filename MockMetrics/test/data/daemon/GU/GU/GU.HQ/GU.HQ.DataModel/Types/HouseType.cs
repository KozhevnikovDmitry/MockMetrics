
namespace GU.HQ.DataModel.Types
{
    /// <summary>
    /// тип жилья: многоквартирный/частный
    /// </summary>
    public enum HouseTypePrivate 
    {
        /// <summary>
        /// Частный дом
        /// </summary>
        HousePrivate = 1,

        /// <summary>
        /// Многоквартирный дом
        /// </summary>
        Condominium = 2
    }


    /// <summary>
    /// Тип жилья: по комфортабельности
    /// </summary>
    public enum HouseTypeComfort
    {
        /// <summary>
        /// благоустроенный дом
        /// </summary>
        Comfortable  = 1,

        /// <summary>
        /// не благоустроенный
        /// </summary>
        NotComfortable = 2
    }
}
