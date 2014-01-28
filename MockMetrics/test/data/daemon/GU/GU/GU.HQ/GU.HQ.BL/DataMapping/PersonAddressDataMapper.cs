using System;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.HQ.DataModel;


namespace GU.HQ.BL.DataMapping
{
    public class PersonAddressDataMapper : AbstractDataMapper<PersonAddress>
    {
        private readonly IDomainDataMapper<Address> _addressDataMapper;

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="domainContext"></param>
        /// <param name="addressDataMapper"> </param>
        public PersonAddressDataMapper(IDomainContext domainContext, IDomainDataMapper<Address> addressDataMapper)
            : base(domainContext)
        {
            _addressDataMapper = addressDataMapper;
        }

        /// <summary>
        /// получить объект Адресс персоны по ID
        /// </summary>
        /// <param name="id">Идентификатор объекта</param>
        /// <param name="dbManager">объект для работы с БД</param>
        /// <returns>объект Адресс персоны</returns>
        protected override PersonAddress RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var obj = dbManager.RetrieveDomainObject<PersonAddress>(id);

            obj.Address = _addressDataMapper.Retrieve(obj.AddressId);

            return obj;
        }

        /// <summary>
        /// Сохранить объект
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dbManager"></param>
        /// <param name="forceSave"></param>
        /// <returns></returns>
        protected override PersonAddress SaveOperation(PersonAddress obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var paClone = obj.Clone();

            paClone.Address = _addressDataMapper.Save(paClone.Address, dbManager);

            paClone.AddressId = paClone.Address.Id;

            dbManager.SaveDomainObject(paClone);

            paClone.AcceptChanges();
            return paClone;
        }

        protected override void FillAssociationsOperation(PersonAddress obj, IDomainDbManager dbManager)
        {
            throw new NotImplementedException();
        }
    }
}
