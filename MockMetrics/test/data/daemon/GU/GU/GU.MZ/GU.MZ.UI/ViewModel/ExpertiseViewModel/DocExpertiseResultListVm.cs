using System;
using System.Collections.Specialized;
using System.Linq;
using BLToolkit.EditableObjects;
using Common.Types.Exceptions;
using Common.UI;
using GU.MZ.BL.DomainLogic.Supervision;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.DataModel.Inspect;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.ExpertiseViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения списка результатов проверки
    /// </summary>
    public class DocExpertiseResultListVm : SmartListVm<DocumentExpertiseResult>
    {
        public ScenarioStep ScenarioStep { get; set; }

        /// <summary>
        /// Супервайзер тома
        /// </summary>
        private SupervisionFacade _superviser;

        public DocExpertiseResultListVm(SupervisionFacade superviser)
        {
            _superviser = superviser;
        }

        public override void Initialize(EditableList<DocumentExpertiseResult> items)
        {
            base.Initialize(items);
            foreach (var listItemVm in ListItemVMs.Cast<DocExpertiseResultItemListVm>())
            {
                listItemVm.ExpertedDocumentList = _superviser.GetAvailableDocs();
            }
        }

        /// <summary>
        /// Обрабатывает событие изменения состава коллекции ViewModel'ов для отображения элементов списка.
        /// </summary>
        /// <param name="sender">Коллекция ViewModel'ов для отображения элементов списка</param>
        /// <param name="e">Аругменты события</param>
        protected override void OnListItemVMsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.OnListItemVMsChanged(sender, e);
            if (_superviser != null)
            {
                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems.Cast<DocExpertiseResultItemListVm>())
                    {
                        item.ExpertedDocumentList = _superviser.GetAvailableDocs();
                    }
                }
            }
        }

        /// <summary>
        /// Добавляет новый результат проверки в список.
        /// </summary>
        protected override void AddItem()
        {
            try
            {
                _superviser.AddExpertiseResult(_superviser.GetAvailableDocs().First(), ScenarioStep);
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка.", ex));
            }
        }
    }
}
