using System.Linq;
using BLToolkit.EditableObjects;
using Common.BL.DataMapping;
using Common.BL.DomainContext;
using Common.DA.Interface;

using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Inspect;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.DataModel.Notifying;
using GU.MZ.DataModel.Violation;

namespace GU.MZ.BL.DataMapping
{
    /// <summary>
    /// Класс маппер сущностей Этап ведения конкретного тома лицензионного дела
    /// </summary>
    public class FileScenarioStepDataMapper : AbstractDataMapper<DossierFileScenarioStep>
    {
        /// <summary>
        /// Маппер документарных проверок
        /// </summary>
        private readonly IDomainDataMapper<DocumentExpertise> _documentExpertiseMapper;

        /// <summary>
        /// Маппер выездных проверок 
        /// </summary>
        private readonly IDomainDataMapper<Inspection> _inspectionMapper;
        private readonly IDomainDataMapper<StandartOrder> _standartOrderMapper;
        private readonly IDomainDataMapper<ExpertiseOrder> _expertiseOrderMapper;
        private readonly IDomainDataMapper<InspectionOrder> _inspectionOrderMapper;

        /// <summary>
        /// Класс маппер сущностей Этап ведения конкретного тома лицензионного дела
        /// </summary>
        /// <param name="domainContext">Доменный контекст</param>
        /// <param name="documentExpertiseMapper">Маппер документарных проверок</param>
        /// <param name="inspectionMapper"></param>
        /// <param name="expertiseOrderMapper"></param>
        public FileScenarioStepDataMapper(IDomainContext domainContext, 
                                          IDomainDataMapper<DocumentExpertise> documentExpertiseMapper,
                                          IDomainDataMapper<Inspection> inspectionMapper,
                                          IDomainDataMapper<StandartOrder> standartOrderMapper,
                                          IDomainDataMapper<ExpertiseOrder> expertiseOrderMapper,
                                          IDomainDataMapper<InspectionOrder> inspectionOrderMapper)
            : base(domainContext)
        {
            _documentExpertiseMapper = documentExpertiseMapper;
            _inspectionMapper = inspectionMapper;
            _standartOrderMapper = standartOrderMapper;
            _expertiseOrderMapper = expertiseOrderMapper;
            _inspectionOrderMapper = inspectionOrderMapper;
        }

        protected override DossierFileScenarioStep RetrieveOperation(object id, IDomainDbManager dbManager)
        {
            var dossierFileScenarioStep = dbManager.RetrieveDomainObject<DossierFileScenarioStep>(id);

            if (dbManager.GetDomainTable<ViolationNotice>().Any(t => t.Id == dossierFileScenarioStep.Id))
            {
                dossierFileScenarioStep.ViolationNotice = dbManager.RetrieveDomainObject<ViolationNotice>(id);
            }

            if (dbManager.GetDomainTable<Notice>().Any(t => t.Id == dossierFileScenarioStep.Id))
            {
                dossierFileScenarioStep.Notice = dbManager.RetrieveDomainObject<Notice>(id);
            }

            if (dbManager.GetDomainTable<ExpertiseOrder>().Any(t => t.Id == dossierFileScenarioStep.Id))
            {
                dossierFileScenarioStep.ExpertiseOrder = _expertiseOrderMapper.Retrieve(id, dbManager);
                dossierFileScenarioStep.ExpertiseOrder.FileScenarioStep = dossierFileScenarioStep;
            }

            if (dbManager.GetDomainTable<InspectionOrder>().Any(t => t.Id == dossierFileScenarioStep.Id))
            {
                dossierFileScenarioStep.InspectionOrder = _inspectionOrderMapper.Retrieve(id, dbManager);
                dossierFileScenarioStep.InspectionOrder.FileScenarioStep = dossierFileScenarioStep;
            }

            if (dbManager.GetDomainTable<DocumentExpertise>().Any(t => t.Id == dossierFileScenarioStep.Id))
            {
                dossierFileScenarioStep.DocumentExpertise = _documentExpertiseMapper.Retrieve(id, dbManager);
            }

            if (dbManager.GetDomainTable<Inspection>().Any(t => t.Id == dossierFileScenarioStep.Id))
            {
                dossierFileScenarioStep.Inspection = _inspectionMapper.Retrieve(id, dbManager);
            }

            if (dbManager.GetDomainTable<ViolationResolveInfo>().Any(t => t.Id == dossierFileScenarioStep.Id))
            {
                dossierFileScenarioStep.ViolationResolveInfo = dbManager.RetrieveDomainObject<ViolationResolveInfo>(id);
            }

            dossierFileScenarioStep.StandartOrderList =
                new EditableList<StandartOrder>(
                    dbManager.GetDomainTable<StandartOrder>()
                             .Where(t => t.FileScenarioStepId == dossierFileScenarioStep.Id)
                             .Select(t => t.Id)
                             .ToList()
                             .Select(t => _standartOrderMapper.Retrieve(t, dbManager))
                             .ToList());

            foreach (var standartOrder in dossierFileScenarioStep.StandartOrderList)
            {
                standartOrder.FileScenarioStep = dossierFileScenarioStep;
            }

            return dossierFileScenarioStep;
        }

        protected override DossierFileScenarioStep SaveOperation(
            DossierFileScenarioStep obj, IDomainDbManager dbManager, bool forceSave = false)
        {
            var tmp = obj.Clone();

            dbManager.SaveDomainObject(tmp);

            DeleteChilds(obj, dbManager);
            
            if (tmp.ViolationNotice != null)
            {
                tmp.ViolationNotice.Id = tmp.Id;
                dbManager.SaveDomainObject(tmp.ViolationNotice);
                tmp.ViolationNotice.Id = tmp.Id;
            }

            if (tmp.Notice != null)
            {
                tmp.Notice.Id = tmp.Id;
                dbManager.SaveDomainObject(tmp.Notice);
                tmp.Notice.Id = tmp.Id;
            }

            if (tmp.ViolationResolveInfo != null)
            {
                tmp.ViolationResolveInfo.Id = tmp.Id;
                dbManager.SaveDomainObject(tmp.ViolationResolveInfo);
                tmp.ViolationResolveInfo.Id = tmp.Id;
            }

            if (tmp.ExpertiseOrder != null)
            {
                tmp.ExpertiseOrder.Id = tmp.Id;
                tmp.ExpertiseOrder = _expertiseOrderMapper.Save(tmp.ExpertiseOrder, dbManager);
                tmp.ExpertiseOrder.FileScenarioStep = tmp;
                tmp.ExpertiseOrder.Id = tmp.Id;
            }

            if (tmp.InspectionOrder != null)
            {
                tmp.InspectionOrder.Id = tmp.Id;
                tmp.InspectionOrder = _inspectionOrderMapper.Save(tmp.InspectionOrder, dbManager);
                tmp.InspectionOrder.FileScenarioStep = tmp;
                tmp.InspectionOrder.Id = tmp.Id;
            }

            if (tmp.DocumentExpertise != null)
            {
                tmp.DocumentExpertise.Id = tmp.Id;
                tmp.DocumentExpertise = _documentExpertiseMapper.Save(tmp.DocumentExpertise, dbManager);
            }

            if (tmp.Inspection != null)
            {
                tmp.Inspection.Id = tmp.Id;
                tmp.Inspection = _inspectionMapper.Save(tmp.Inspection, dbManager);
            }

            if (tmp.StandartOrderList.DelItems != null)
            {
                foreach (var delItem in tmp.StandartOrderList.DelItems.Cast<StandartOrder>())
                {
                   _standartOrderMapper.Delete(delItem);
                }
            }

            for (int i = 0; i < tmp.StandartOrderList.Count; i++)
            {
                tmp.StandartOrderList[i].FileScenarioStepId = tmp.Id;
                tmp.StandartOrderList[i] = _standartOrderMapper.Save(tmp.StandartOrderList[i], dbManager);
                tmp.StandartOrderList[i].FileScenarioStep = tmp;
            }

            return tmp;
        }

        private void DeleteChilds(DossierFileScenarioStep step, IDomainDbManager dbManager)
        {
            if (step.DeleteNotices != null)
            {
                foreach (var notice in step.DeleteNotices)
                {
                    notice.MarkDeleted();
                    dbManager.SaveDomainObject(notice);
                }
            }

            if (step.DeleteViolationNotices != null)
            {
                foreach (var notice in step.DeleteViolationNotices)
                {
                    notice.MarkDeleted();
                    dbManager.SaveDomainObject(notice);
                }
            }

            if (step.DeleteViolationResolveInfos != null)
            {
                foreach (var violationResolveInfo in step.DeleteViolationResolveInfos)
                {
                    violationResolveInfo.MarkDeleted();
                    dbManager.SaveDomainObject(violationResolveInfo);
                }
            }

            if (step.DeleteExpertises != null)
            {
                foreach (var documentExpertise in step.DeleteExpertises)
                {
                    _documentExpertiseMapper.Delete(documentExpertise);
                }
            }

            if (step.DeleteInspections != null)
            {
                foreach (var inspection in step.DeleteInspections)
                {
                    _inspectionMapper.Delete(inspection);
                }
            }

            if (step.DeleteInspectionOrders != null)
            {
                foreach (var inspectionOrder in step.DeleteInspectionOrders)
                {
                    _inspectionOrderMapper.Delete(inspectionOrder);
                }
            }

            if (step.DeleteExpertiseOrders != null)
            {
                foreach (var expertiseOrder in step.DeleteExpertiseOrders)
                {
                    _expertiseOrderMapper.Delete(expertiseOrder);
                }
            }
        }

        protected override void FillAssociationsOperation(DossierFileScenarioStep obj, IDomainDbManager dbManager)
        {
            
        }
    }
}
