using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;

using GU.MZ.DataModel;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Requisites;

namespace GU.MZ.BL.DataMapping
{
    /// <summary>
    /// Маппер сущностей Резвизиты лицензиата
    /// </summary>
    public class HolderRequisitesDataMapper : AbstractDataMapper<HolderRequisites>
    {
        /// <summary>
        /// Маппер сущностей Резвизиты лицензиата
        /// </summary>
        /// <param name="domainContext">Доменный контекст</param>
        public HolderRequisitesDataMapper(IDomainContext domainContext)
            : base(domainContext)
        {
        }

        protected override HolderRequisites RetrieveOperation(object id, IDomainDbManager dbManager)
        {

            var requisites = dbManager.RetrieveDomainObject<HolderRequisites>(id);

            if (requisites.IndRequisitesId.HasValue)
            {
                requisites.IndRequisites = dbManager.RetrieveDomainObject<IndRequisites>(requisites.IndRequisitesId);
            }
            else
            {
                requisites.JurRequisites = dbManager.RetrieveDomainObject<JurRequisites>(requisites.JurRequisitesId);
            }

            requisites.Address = dbManager.RetrieveDomainObject<Address>(requisites.AddressId);

            return requisites;
        }

        protected override HolderRequisites SaveOperation(HolderRequisites obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();

            if (tmp.IndRequisites != null)
            {
                dbManager.SaveDomainObject(tmp.IndRequisites);
                tmp.IndRequisitesId = tmp.IndRequisites.Id;
            }
            else
            {
                dbManager.SaveDomainObject(tmp.JurRequisites);
                tmp.JurRequisitesId = tmp.JurRequisites.Id;
            }

            dbManager.SaveDomainObject(tmp.Address);
            tmp.AddressId = tmp.Address.Id;

            dbManager.SaveDomainObject(tmp);

            return tmp;
        }

        protected override void FillAssociationsOperation(HolderRequisites obj, IDomainDbManager dbManager)
        {
            if (obj.JurRequisites != null)
            {
                dbManager.SaveDomainObject(obj.JurRequisites);
                obj.JurRequisitesId = obj.JurRequisites.Id;
            }
            else
            {
                dbManager.SaveDomainObject(obj.IndRequisites);
                obj.IndRequisitesId = obj.IndRequisites.Id;
            }
        }
    }
}
