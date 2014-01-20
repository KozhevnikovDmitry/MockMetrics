namespace PostGrad.Core.BL
{
    /// <summary>
    /// Класс, представляющий краткую информацию о заявителе
    /// </summary>
    public class HolderInfo
    {
        /// <summary>
        /// Полное наименование заявителя
        /// </summary>
        public virtual string FullName { get; set; }

        /// <summary>
        /// ИНН заявителя
        /// </summary>
        public virtual string Inn { get; set; }

        /// <summary>
        /// ОГРН заявителя
        /// </summary>
        public virtual string Ogrn { get; set; }
    }
}