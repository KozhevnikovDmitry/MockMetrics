using System;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel;
using GU.BL;
using GU.HQ.BL;
using GU.HQ.BL.Reporting.Data;
using GU.HQ.DataModel;
using GU.UI.View.ReportDialogView;
using GU.UI.ViewModel.ReportDialogViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.HQ.UI.ViewModel
{
    public class HqRepositoryVM : BaseAvalonDockVM
    {
        public HqRepositoryVM()
            : base(new SingletonDockableUiFactory())
        {
            ShowClaimListCommand = new DelegateCommand(ShowClaimList);
            ShowClaimCommand  = new DelegateCommand(ShowClaim);
            ShowClaimRegistrReportCommand = new DelegateCommand(this.ShowClaimRegistrReport);
            IsDebug = true;
        }

        #region Binding Properties

        public DelegateCommand ShowClaimListCommand { get; private set; }
        public DelegateCommand ShowClaimCommand { get; private set; }
        public DelegateCommand ShowClaimRegistrReportCommand { get; private set; }

        /// <summary>
        /// Флажок указывающий на дебажность запущенной сборки.
        /// </summary>
        public bool IsDebug { get; set; }

        /// <summary>
        /// Отобразить список заявлений
        /// </summary>
        private void ShowClaimList()
        {
            try
            {
                AvalonInteractor.RaiseOpenSearchDockable("Заявки", typeof(Claim));
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        
        /// <summary>
        /// Отобразить заявление пустое
        /// </summary>
        private void ShowClaim()
        {
            try
            {
                AvalonInteractor.RaiseOpenEditableDockable("Заявка", typeof(Claim), Claim.CreateInstance());
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        private void ShowClaimRegistrReport()
        {
            try
            {
                var vm = new GetReportDateIntervalVM();
                if (UIFacade.ShowDialogView(new GetReportDateIntervalView(), vm, "Интервал дат для отчёта"))
                {
                    var date1 = vm.Date1.HasValue ? vm.Date1.Value : DateTime.MinValue;
                    var date2 = vm.Date2.HasValue ? vm.Date2.Value : DateTime.MaxValue;
                    var report = HqFacade.GetClaimRegistrReport(GuFacade.GetDbUser().UserText, date1, date2);

                    var reportView = UIFacade.GetReportPresenter(report, IsDebug);
                    AddDockable("Список граждан, нуждающихся в жилых помещениях по договору социального найма", reportView);
                }
              //  var reportView = UIFacade.GetReportPresenter<ClaimRegistr>("Reporting/View/GU.HQ/ClaimRegistrReport.mrt", "data", IsDebug, GuFacade.GetDbUser().UserText);
              //  AddDockable("Список граждан, нуждающихся в жилых помещениях по договору социального найма", reportView);
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }
        


        #endregion
    }
}
