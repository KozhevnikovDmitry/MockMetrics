namespace Common.BL.DomainContext
{
    /// <summary>
    /// ��������� ��� �������-��������� �������
    /// </summary>
    public interface IDomainDependent
    {
        /// <summary>
        /// ������������� ������������� ��������� ���������.
        /// </summary>
        /// <param name="assemblyName">��� ������ � �������-���������� ��������</param>
        void SetDomainKey(string assemblyName);

        /// <summary>
        /// ������������� ������������� ��������� ���������.
        /// </summary>
        string DomainKey { set; }
    }
}