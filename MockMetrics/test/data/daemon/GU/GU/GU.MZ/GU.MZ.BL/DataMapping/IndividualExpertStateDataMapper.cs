
using System.Linq;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA;
using Common.DA.Interface;

using GU.MZ.DataModel.Person;

namespace GU.MZ.BL.DataMapping
{
    /// <summary>
    /// Класс маппер экспертов физических лиц
    /// </summary>
    public class IndividualExpertStateDataMapper : AbstractDataMapper<IExpertState>, IExpertStateDataMapper
    {
        public IndividualExpertStateDataMapper(IDomainContext domainContext)
            : base(domainContext)
        {
        }

        public virtual ExpertStateType ExpertStateType
        {
            get
            {
                return  ExpertStateType.Individual;
            }
        } 

        #region Overrides of AbstractDataMapper<IExpertState>

        protected override IExpertState RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            return dbManager.RetrieveDomainObject<IndividualExpertState>(id);
        }

        protected override IExpertState SaveOperation(IExpertState obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = (obj as IndividualExpertState).Clone();

            if (dbManager.GetDomainTable<IndividualExpertState>().Any(t => t.Id == obj.Id))
            {
                tmp.PersistentState = PersistentState.Old;
            }

            dbManager.SaveDomainObject(tmp);

            return tmp;
        }

        protected override void FillAssociationsOperation(IExpertState obj, IDomainDbManager dbManager)
        {
            
        }

        #endregion
    }
}
