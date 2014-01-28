using System.Collections.Specialized;
using System.Windows;
using Common.UI.WeakEvent.EventSubscriber;

using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Inspect;
using GU.MZ.DataModel.MzOrder;

namespace GU.MZ.UI.ViewModel.EventSubscriber
{
    /// <summary>
    /// Класс отвественный подписку на событие <c>PropertyChanged</c> этапа ведения тома лицензионного дела.
    /// </summary>
    public class FileScenarioStepEventSubscriber : BaseDomainObjectEventSubscriber<DossierFileScenarioStep>
    {
        private readonly IDomainObjectEventSubscriber<InspectionOrder> _inspectionOrderSubscriber;
        private readonly IDomainObjectEventSubscriber<ExpertiseOrder> _expertiseOrderSubscriber;
        private readonly IDomainObjectEventSubscriber<DocumentExpertise> _documentExpertiseSubscriber;
        private readonly IDomainObjectEventSubscriber<Inspection> _inspectionSubscriber;
        private readonly IDomainObjectEventSubscriber<StandartOrder> _standartOrderSubscriber;

        public FileScenarioStepEventSubscriber(IDomainObjectEventSubscriber<InspectionOrder> inspectionOrderSubscriber,
                                               IDomainObjectEventSubscriber<ExpertiseOrder> expertiseOrderSubscriber,
                                               IDomainObjectEventSubscriber<DocumentExpertise> documentExpertiseSubscriber,
                                               IDomainObjectEventSubscriber<Inspection> inspectionSubscriber,
                                               IDomainObjectEventSubscriber<StandartOrder> standartOrderSubscriber)
        {
            _inspectionOrderSubscriber = inspectionOrderSubscriber;
            _expertiseOrderSubscriber = expertiseOrderSubscriber;
            _documentExpertiseSubscriber = documentExpertiseSubscriber;
            _inspectionSubscriber = inspectionSubscriber;
            _standartOrderSubscriber = standartOrderSubscriber;
        }

        public override void PropertyChangedSubscribe(DossierFileScenarioStep sourceObject, IWeakEventListener listener)
        {
            base.PropertyChangedSubscribe(sourceObject, listener);

            if (sourceObject.DocumentExpertise != null)
            {
                _documentExpertiseSubscriber.PropertyChangedSubscribe(sourceObject.DocumentExpertise, listener);
            }

            if (sourceObject.Inspection != null)
            {
                _inspectionSubscriber.PropertyChangedSubscribe(sourceObject.Inspection, listener);
            }

            if (sourceObject.InspectionOrder != null)
            {
                _inspectionOrderSubscriber.PropertyChangedSubscribe(sourceObject.InspectionOrder, listener);
            }

            if (sourceObject.ExpertiseOrder != null)
            {
                _expertiseOrderSubscriber.PropertyChangedSubscribe(sourceObject.ExpertiseOrder, listener);
            }

            if (sourceObject.StandartOrderList != null)
            {
                CollectionChangedEventManager.AddListener(sourceObject.StandartOrderList, listener);
                foreach (var order in sourceObject.StandartOrderList)
                {
                    _standartOrderSubscriber.PropertyChangedSubscribe(order, listener);
                }
            }
        }
    }
}
