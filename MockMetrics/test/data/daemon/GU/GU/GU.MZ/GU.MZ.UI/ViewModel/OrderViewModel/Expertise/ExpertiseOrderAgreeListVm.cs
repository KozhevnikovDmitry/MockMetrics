using System;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using GU.MZ.DataModel.MzOrder;
using GU.MZ.UI.View.OrderView;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.OrderViewModel.Expertise
{
    public class ExpertiseOrderAgreeListVm : SmartListVm<ExpertiseOrderAgree>
    {
        private readonly IDialogUiFactory _uiFactory;
        private readonly AddOrderAgreeVm _addOrderAgreeVm;

        public ExpertiseOrderAgreeListVm(IDialogUiFactory uiFactory, AddOrderAgreeVm addOrderAgreeVm)
        {
            _uiFactory = uiFactory;
            _addOrderAgreeVm = addOrderAgreeVm;
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

                var addView = new AddOrderAgreeView { DataContext = _addOrderAgreeVm };
                if (_uiFactory.ShowDialogView(addView, _addOrderAgreeVm, "Добавить согласовавшего"))
                {
                    var agree = ExpertiseOrderAgree.CreateInstance();
                    agree.EmployeeName = _addOrderAgreeVm.Employee.Name;
                    agree.EmployeePosition = _addOrderAgreeVm.Employee.Position;
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
