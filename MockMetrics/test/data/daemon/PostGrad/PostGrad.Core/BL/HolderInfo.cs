namespace PostGrad.Core.BL
{
    /// <summary>
    /// �����, �������������� ������� ���������� � ���������
    /// </summary>
    public class HolderInfo
    {
        /// <summary>
        /// ������ ������������ ���������
        /// </summary>
        public virtual string FullName { get; set; }

        /// <summary>
        /// ��� ���������
        /// </summary>
        public virtual string Inn { get; set; }

        /// <summary>
        /// ���� ���������
        /// </summary>
        public virtual string Ogrn { get; set; }
    }
}