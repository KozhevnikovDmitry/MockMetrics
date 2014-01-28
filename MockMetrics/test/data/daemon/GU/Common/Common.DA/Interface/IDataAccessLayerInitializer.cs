using System.Collections.Generic;
using System.Reflection;

using Common.DA.ProviderConfiguration;

namespace Common.DA.Interface
{
    /// <summary>
    /// ��������� ������, ������������� �� ������������� ���� ������
    /// </summary>
    public interface IDataAccessLayerInitializer : IConfigurationTester
    {

        /// <summary>
        /// ���������� �������� ���� ������, �� ������� ������� �����������.
        /// </summary>
        /// <returns>�������� ��</returns>
        IDomainDbManager GetDbManager(Assembly blAssembly);

        /// <summary>
        /// �������������� ���� ������� � ������.
        /// </summary>
        /// <param name="config">������������ �����������</param>
        /// <param name="dataModelAssembly">������ � ���� ��������</param>
        /// <param name="domainObjectInitilizer">������������� �������� ��������</param>
        void Initialize(IProviderConfiguration config, 
                        Assembly dataModelAssembly, 
                        IDomainObjectInitializer domainObjectInitilizer);

        void Initialize(IProviderConfiguration config,
            Dictionary<Assembly,IDomainObjectInitializer> initDictionary);
    }
}