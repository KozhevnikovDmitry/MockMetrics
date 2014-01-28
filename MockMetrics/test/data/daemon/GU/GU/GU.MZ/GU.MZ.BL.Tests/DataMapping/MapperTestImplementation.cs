using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;

using Moq;

namespace GU.MZ.BL.Test.DataMappingTest
{
    /// <summary>
    /// Стаб маппера для тестирования абстрактного класса AbstractDataMapper
    /// </summary>
    public class MapperTestImplementation<T> : AbstractDataMapper<T> where T : class, IPersistentObject
    {
        public MapperTestImplementation(IDomainContext domainContext)
            : base(domainContext)
        {
        }

        protected override T RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            Result = new Mock<T>();
            return Result.Object;
        }

        protected override T SaveOperation(T obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            Result = new Mock<T>();
            return Result.Object;
        }

        protected override void FillAssociationsOperation(T obj, IDomainDbManager dbManager)
        {
            
        }

        /// <summary>
        /// Мок объекта-агрегата, который маппится в маппере
        /// Нужен для проверки вызова метода AcceptChanges()
        /// </summary>
        public Mock<T> Result { get; private set; }
    }
}
