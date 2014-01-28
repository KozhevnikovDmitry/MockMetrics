using System.Linq;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;
using GU.HQ.DataModel;

namespace GU.HQ.BL.DataMapping
{
    public class AddressDataMapper : AbstractDataMapper<Address>
    {
        public AddressDataMapper(IDomainContext domainContext) 
            : base(domainContext)
        {
        }

        /// <summary>
        /// Получить объект по Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dbManager"></param>
        /// <returns></returns>
        protected override Address RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var obj = dbManager.RetrieveDomainObject<Address>(id);

            // описание адреса
            if ((from ad in dbManager.GetDomainTable<AddressDesc>()
                 where ad.Id == obj.Id
                 select ad.Id).Any())
                obj.AddressDesc = dbManager.RetrieveDomainObject<AddressDesc>(obj.Id);

            return obj;
        }

        /// <summary>
        /// Сохранить объект по Id
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dbManager"></param>
        /// <param name="forceSave"></param>
        /// <returns></returns>
        protected override Address SaveOperation(Address obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var addressClone = obj.Clone();

            dbManager.BeginDomainTransaction();

            // сохраняем информацию о адресе 
            dbManager.SaveDomainObject(addressClone); 

            // сохраняем информацию о расшифровке адреса
            if (addressClone.AddressDesc != null)
            {
                addressClone.AddressDesc.Id = addressClone.Id;
                dbManager.SaveDomainObject(addressClone.AddressDesc);
            }

            addressClone.AcceptChanges();
            dbManager.CommitDomainTransaction();
            return addressClone;
        }

        protected override void FillAssociationsOperation(Address obj, IDomainDbManager dbManager)
        {
            obj.AddressDesc = dbManager.RetrieveDomainObject<AddressDesc>(obj.Id);
        }
    }
}
