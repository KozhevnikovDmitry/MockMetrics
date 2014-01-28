using Common.UI.WeakEvent;
using Common.UI.WeakEvent.EventSubscriber;
using GU.HQ.DataModel;

namespace GU.HQ.UI.ViewModel.EventSubscriber
{
    public class ClaimEventSubscriber : BaseDomainObjectEventSubscriber<Claim>
    {
        private readonly IDomainObjectEventSubscriber<Person> _declarer;

        public ClaimEventSubscriber(IDomainObjectEventSubscriber<Person> declarer)
        {
            _declarer = declarer;
        }

        public override void PropertyChangedSubscribe(Claim sourceObject, System.Windows.IWeakEventListener listener)
        {
            
            base.PropertyChangedSubscribe(sourceObject, listener);

            if (sourceObject.Relatives != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.Relatives, listener);

                foreach (var relative in sourceObject.Relatives)
                {
                    PropertyChangedWeakEventManager.AddListener(relative, listener);
                    PropertyChangedWeakEventManager.AddListener(relative.Person, listener);
                }
            }

            if (sourceObject.Notices != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.Notices, listener);

                foreach (var notice in sourceObject.Notices)
                {
                    PropertyChangedWeakEventManager.AddListener(notice, listener);
                }
            }

            if (sourceObject.ClaimCategories != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.ClaimCategories, listener);

                foreach (var claimCategory in sourceObject.ClaimCategories)
                {
                    PropertyChangedWeakEventManager.AddListener(claimCategory, listener);
                }
            }

            if (sourceObject.DeclarerBaseReg != null && sourceObject.DeclarerBaseReg.BaseRegItems != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.DeclarerBaseReg.BaseRegItems, listener);

                foreach (var baseRegItems in sourceObject.DeclarerBaseReg.BaseRegItems)
                {
                    PropertyChangedWeakEventManager.AddListener(baseRegItems, listener);
                }
            }

            _declarer.PropertyChangedSubscribe(sourceObject.Declarer, listener);

            if (sourceObject.QueuePrivList != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.QueuePrivList, listener);

                foreach (var qp in sourceObject.QueuePrivList)
                {
                    PropertyChangedWeakEventManager.AddListener(qp, listener);
                }
            }

            if (sourceObject.HouseProvided != null)
            {
                PropertyChangedWeakEventManager.AddListener(sourceObject.HouseProvided, listener);
                if (sourceObject.HouseProvided.Address != null)
                {
                    PropertyChangedWeakEventManager.AddListener(sourceObject.HouseProvided.Address, listener);
                    if(sourceObject.HouseProvided.Address.AddressDesc != null)
                        PropertyChangedWeakEventManager.AddListener(sourceObject.HouseProvided.Address.AddressDesc, listener);
                }
            }
        }
    }
}
