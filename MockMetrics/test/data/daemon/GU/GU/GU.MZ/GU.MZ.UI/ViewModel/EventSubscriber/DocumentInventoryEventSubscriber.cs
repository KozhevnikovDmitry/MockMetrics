using System.Windows;
using Common.UI.WeakEvent;
using Common.UI.WeakEvent.EventSubscriber;
using GU.MZ.DataModel.Dossier;

namespace GU.MZ.UI.ViewModel.EventSubscriber
{
    public class DocumentInventoryEventSubscriber : BaseDomainObjectEventSubscriber<DocumentInventory>
    {
        public override void PropertyChangedSubscribe(DocumentInventory sourceObject, IWeakEventListener listener)
        {
            base.PropertyChangedSubscribe(sourceObject, listener);

            if (sourceObject.ProvidedDocumentList != null)
            {
                CollectionChangedWeakEventManager.AddListener(sourceObject.ProvidedDocumentList, listener);
                foreach (var document in sourceObject.ProvidedDocumentList)
                {
                    PropertyChangedWeakEventManager.AddListener(document, listener);
                }
            }
        }
    }
}
