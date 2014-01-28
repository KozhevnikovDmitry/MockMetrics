using System;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using Common.UI.ViewModel.AvalonInteraction;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using GU.MZ.BL.Reporting;
using GU.MZ.UI.View.ReportDilaogView;
using GU.MZ.UI.ViewModel.ReportDialogViewModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.TaskViewModel
{
    public class TaskReportsVm : NotificationObject, IAvalonDockCaller
    {
        private readonly IDialogUiFactory _uiFactory;
        private readonly IReportFacade _reportFacade;
        private readonly NewFullActivityReportVm _fullActivityReportVm;

        public bool IsDesign { get; set; }

        public TaskReportsVm(IDialogUiFactory uiFactory, 
            IReportFacade reportFacade,
            NewFullActivityReportVm fullActivityReportVm)
        {
            _uiFactory = uiFactory;
            _reportFacade = reportFacade;
            _fullActivityReportVm = fullActivityReportVm;
            AvalonInteractor = new MiddleAvalonDockInteractor(this);
            ShowLicenceByActivityReportCommand = new DelegateCommand(ShowLicenceByActivityReport);
            ShowStatementByServiceElcReportCommand = new DelegateCommand(ShowStatementByServiceElcReport);
            ShowFullActivityDataReportCommand = new DelegateCommand(ShowFullActivityDataReport);
        }

        #region Binding Commands

        /// <summary>
        /// Команда открытия отчёта "Лицензии в разрезе видов деятельности"
        /// </summary>
        public DelegateCommand ShowLicenceByActivityReportCommand { get; private set; }

        /// <summary>
        /// Команда открытия отчёта "Заявления в электронном виде в разрезе услуг"
        /// </summary>
        public DelegateCommand ShowStatementByServiceElcReportCommand { get; private set; }

        /// <summary>
        /// Команда открытия отчёта "Полный отчёт по лицензированию вида деятельности"
        /// </summary>
        public DelegateCommand ShowFullActivityDataReportCommand { get; private set; }

        /// <summary>
        /// Открывает окно с отчётом "Полный отчёт по лицензированию вида деятельности"
        /// </summary>
        private void ShowFullActivityDataReport()
        {
            try
            {
                if (_uiFactory.ShowDialogView(new NewFullActivityReportView(), _fullActivityReportVm, "Интервал дат для отчёта"))
                {

                    var date1 = _fullActivityReportVm.Date1.HasValue ? _fullActivityReportVm.Date1.Value : DateTime.MinValue;
                    var date2 = _fullActivityReportVm.Date2.HasValue ? _fullActivityReportVm.Date2.Value : DateTime.MaxValue;

                    var report = _reportFacade.FullActivityDataReport.Initialize(_fullActivityReportVm.LicensedActivity, date1, date2);

                    AvalonInteractor.RaiseOpenReportDockable("Полный отчёт по лицензированию вида деятельности", report, IsDesign);
                }
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
        /// Открывает отчёт "Лицензии в разрезе видов деятельности"
        /// </summary>
        private void ShowStatementByServiceElcReport()
        {
            try
            {
                var report = _reportFacade.StatementByServiceReport.Initialize(true);
                AvalonInteractor.RaiseOpenReportDockable("Отчёт по заявлениям в электронной форме в разрезе услуг", report, IsDesign);
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
        /// Открывает отчёт "Реестр лицензий"
        /// </summary>
        private void ShowLicenceByActivityReport()
        {
            try
            {
                var report = _reportFacade.LicenseByActivityReport;
                AvalonInteractor.RaiseOpenReportDockable("Отчёт по выданым лицензиям в разрезе видов деятельности", report, IsDesign);
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

        public IAvalonDockInteractor AvalonInteractor { get; private set; }
    }
}