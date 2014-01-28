using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;

using GU.MZ.DataModel.Notifying;

namespace GU.MZ.BL.DataMapping
{
    /// <summary>
    /// Класс маппер сущностей Уведомление заявителю
    /// </summary>
    public class NoticeDataMapper : AbstractDataMapper<Notice>
    {
        /// <summary>
        /// Класс маппер сущностей Уведомление заявителю
        /// </summary>
        /// <param name="domainContext">Доменный контекст</param>
        public NoticeDataMapper(IDomainContext domainContext)
            : base(domainContext)
        {
        }

        protected override Notice RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            return dbManager.RetrieveDomainObject<Notice>(id);
        }

        protected override Notice SaveOperation(Notice obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();
            
            dbManager.SaveDomainObject(tmp);

            return tmp;
        }

        protected override void FillAssociationsOperation(Notice obj, IDomainDbManager dbManager)
        {
            
        }
    }
}
