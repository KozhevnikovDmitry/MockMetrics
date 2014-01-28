using Common.DA.ProviderConfiguration;

namespace Common.DA.Interface
{
    /// <summary>
    /// ��������� ������, ������������� �� �������� ����������� � ���� ������
    /// </summary>
    public interface IConfigurationTester
    {
        /// <summary>
        /// ��������� ����������� ���������� � �� �� �������� ������������ 
        /// </summary>
        /// <param name="config">������������ �����������</param>
        /// <returns>���� ����������� �����������</returns>
        bool TestConfiguration(IProviderConfiguration config);

        /// <summary>
        /// ��������� ����������� ���������� � �� �� �������� ������������ 
        /// </summary>
        /// <param name="config">������������ �����������</param>
        /// <returns>���������� � �������� �����������</returns>
        ConfigurationTestResult FullTestConfiguration(IProviderConfiguration config);
    }
}