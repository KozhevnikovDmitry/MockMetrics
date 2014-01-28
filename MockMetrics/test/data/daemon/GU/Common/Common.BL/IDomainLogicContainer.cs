using Autofac;

using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using Common.BL.DomainContext;
using Common.BL.ReportMapping;
using Common.BL.Validation;
using Common.DA.Interface;
using Common.Types.Exceptions;

namespace Common.BL
{
    /// <summary>
    /// ��������� ������� ����������� ������ ������
    /// </summary>
    public interface IDomainLogicContainer
    {
        /// <summary>
        /// IoC ��������� �������� �� �����������
        /// </summary>
        IContainer IocContainer { get; }

        /// <summary>
        /// ���������� ��������� ������������� �������-���������� ������   
        /// </summary>
        /// <typeparam name="T">�����</typeparam>
        /// <returns>��������� �������-���������� ������������� ������</returns>
        T ResolveDomainDependent<T>(params object[] parameters) where T : IDomainDependent;

        IDomainDataMapper<T> ResolveDataMapper<T>() where T : IPersistentObject;

        IDomainValidator<T> ResolveValidator<T>() where T : IPersistentObject;
    }
}