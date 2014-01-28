using System.Linq;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA;
using Common.DA.Interface;

using GU.MZ.DataModel;
using GU.MZ.DataModel.Person;

namespace GU.MZ.BL.DataMapping
{
    /// <summary>
    /// Класс маппер сущностей состояние эксперта - юридическое лицо
    /// </summary>
    public class JuridicalExpertStateDataMapper : AbstractDataMapper<IExpertState>, IExpertStateDataMapper
    {
        /// <summary>
        /// Класс маппер сущностей состояние эксперта - юридическое лицо
        /// </summary>
        /// <param name="domainContext">Доменный контекст</param>
        public JuridicalExpertStateDataMapper(IDomainContext domainContext)
            : base(domainContext)
        {

        }


        public virtual ExpertStateType ExpertStateType
        {
            get
            {
                return ExpertStateType.Juridical;
            }
        } 

        #region Overrides of AbstractDataMapper<JuridicalExpertState>

        protected override IExpertState RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var juridicalExpertState = dbManager.RetrieveDomainObject<JuridicalExpertState>(id);

            juridicalExpertState.Address = dbManager.RetrieveDomainObject<Address>(juridicalExpertState.AddressId);

            return juridicalExpertState;
        }

        protected override IExpertState SaveOperation(IExpertState obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = (obj as JuridicalExpertState).Clone();
            
            dbManager.SaveDomainObject(tmp.Address);

            if (dbManager.GetDomainTable<JuridicalExpertState>().Any(t => t.Id == obj.Id))
            {
                tmp.PersistentState = PersistentState.Old;
            }

            tmp.AddressId = tmp.Address.Id;

            dbManager.SaveDomainObject(tmp);

            return tmp;
        }

        protected override void FillAssociationsOperation(IExpertState obj, IDomainDbManager dbManager)
        {
            
        }

        #endregion
    }
}
