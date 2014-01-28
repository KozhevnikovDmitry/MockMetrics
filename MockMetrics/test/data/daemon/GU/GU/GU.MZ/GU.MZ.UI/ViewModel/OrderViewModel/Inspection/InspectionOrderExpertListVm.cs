using System;
using System.Linq;
using Common.BL.DictionaryManagement;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.DataModel.Person;
using GU.MZ.UI.View.OrderView.Inspection;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.OrderViewModel.Inspection
{
    public class InspectionOrderExpertListVm : SmartListVm<InspectionOrderExpert>
    {
        private readonly IDialogUiFactory _uiFactory;
        private readonly AddInspectionOrderExpertVm _addInspectionOrderExpertVm;
        private readonly IDictionaryManager _dictionaryManager;

        public InspectionOrderExpertListVm(IDictionaryManager dictionaryManager, IDialogUiFactory uiFactory, AddInspectionOrderExpertVm addInspectionOrderExpertVm)
        {
            _dictionaryManager = dictionaryManager;
            _uiFactory = uiFactory;
            _addInspectionOrderExpertVm = addInspectionOrderExpertVm;
        }

        protected override void SetListOptions()
        {
            base.SetListOptions();
            CanAddItems = true;
            CanRemoveItems = true;
        }

        protected override void AddItem()
        {
            try
            {
                if (!_dictionaryManager.GetDynamicDictionary<Expert>().Any())
                {
                    NoticeUser.ShowInformation("Доступных экспертов не найдено.");
                    return;
                }


                var addView = new AddInspectionOrderExpertView { DataContext = _addInspectionOrderExpertVm };
                if (_uiFactory.ShowDialogView(addView, _addInspectionOrderExpertVm, "Добавить эксперта"))
                {
                    var agree = InspectionOrderAgree.CreateInstance();
                    agree.EmployeeName = _addInspectionOrderExpertVm.Expert.ExpertState.GetName();
                    agree.EmployeePosition = _addInspectionOrderExpertVm.Expert.ExpertState.GetWorkdata();
                   Items.Add(agree);
                }

                
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
