namespace GU.MZ.BL.DomainLogic.AcceptTask
{
    /// <summary>
    /// Класс, представляющий краткую информацию о заявителе
    /// </summary>
    public class HolderInfo
    {
        /// <summary>
        /// Полное наименование заявителя
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// ИНН заявителя
        /// </summary>
        public string Inn { get; set; }

        /// <summary>
        /// ОГРН заявителя
        /// </summary>
        public string Ogrn { get; set; }
    }
}