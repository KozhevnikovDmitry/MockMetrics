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
    /// »нтерфейс классов контейнеров бизнес логики
    /// </summary>
    public interface IDomainLogicContainer
    {
        /// <summary>
        /// IoC контейнер хран€щий всЄ зависимости
        /// </summary>
        IContainer IocContainer { get; }

        /// <summary>
        /// ¬озвращает экземпл€р произвольного доменно-зависимого класса   
        /// </summary>
        /// <typeparam name="T"> ласс</typeparam>
        /// <returns>Ёкземпл€р доменно-зависимого произвольного класса</returns>
        T ResolveDomainDependent<T>(params object[] parameters) where T : IDomainDependent;

        IDomainDataMapper<T> ResolveDataMapper<T>() where T : IPersistentObject;

        IDomainValidator<T> ResolveValidator<T>() where T : IPersistentObject;
    }
}