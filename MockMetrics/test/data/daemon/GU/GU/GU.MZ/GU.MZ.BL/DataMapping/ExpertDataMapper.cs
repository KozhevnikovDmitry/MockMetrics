using System.Collections.Generic;
using System.Linq;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;

using GU.MZ.DataModel.Person;

namespace GU.MZ.BL.DataMapping
{
    /// <summary>
    /// Класс маппер сущностей Эксперт
    /// </summary>
    public class ExpertDataMapper : AbstractDataMapper<Expert>
    {
        private readonly IEnumerable<IExpertStateDataMapper> _expertStateDataMappers;

        /// <summary>
        /// Класс маппер сущностей Эксперт
        /// </summary>
        /// <param name="domainContext">Доменный контекст</param>
        public ExpertDataMapper(IDomainContext domainContext, IEnumerable<IExpertStateDataMapper> expertStateDataMappers)
            : base(domainContext)
        {
            _expertStateDataMappers = expertStateDataMappers;
        }

        #region Overrides of AbstractDataMapper<Expert>

        protected override Expert RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var expert = dbManager.RetrieveDomainObject<Expert>(id);

            expert.ExpertState = _expertStateDataMappers.Single(t => t.ExpertStateType == expert.ExpertStateType)
                                                        .Retrieve(id, dbManager);

            return expert;
        }

        protected override Expert SaveOperation(Expert obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();

            dbManager.SaveDomainObject(tmp);

            tmp.ExpertState.Id = tmp.Id;

            tmp.ExpertState = _expertStateDataMappers.Single(t => t.ExpertStateType == tmp.ExpertStateType)
                                                     .Save(tmp.ExpertState, dbManager);

            return tmp;
        }

        protected override void FillAssociationsOperation(Expert obj, IDomainDbManager dbManager)
        {
            obj.ExpertState = _expertStateDataMappers.Single(t => t.ExpertStateType == obj.ExpertStateType)
                                                      .Retrieve(obj.Id, dbManager);
        }

        #endregion
    }
}
