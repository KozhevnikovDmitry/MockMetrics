using System;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.AvalonInteraction;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using GU.MZ.BL.Reporting.Mapping;
using GU.MZ.DataModel.Notifying;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.ViolationResolveViewModel
{
    public class ViolationNoticeVm : SmartValidateableVm<ViolationNotice>, IAvalonDockCaller
    {
        private readonly ViolationNoticeReport _noticeReport;

        public ViolationNoticeVm(ViolationNoticeReport noticeReport)
        {
            _noticeReport = noticeReport;
            PrintViolationNoticeCommand = new DelegateCommand(PrintViolationNotice, CanPrintViolationNotice);
            AvalonInteractor = new MiddleAvalonDockInteractor(this);
        }

        #region Binding Properties

        public int TaskRegNumber
        {
            get
            {
                return Entity.TaskRegNumber;
            }
            set
            {
                if (Entity.TaskRegNumber != value)
                {
                    Entity.TaskRegNumber = value;
                    RaisePropertyChanged(() => TaskRegNumber);
                }
            }
        }

        public DateTime TaskStamp
        {
            get
            {
                return Entity.TaskStamp;
            }
            set
            {
                if (Entity.TaskStamp != value)
                {
                    Entity.TaskStamp = value;
                    RaisePropertyChanged(() => TaskStamp);
                }
            }
        }

        public string EmployeeName
        {
            get
            {
                return Entity.EmployeeName;
            }
            set
            {
                if (Entity.EmployeeName != value)
                {
                    Entity.EmployeeName = value;
                    RaisePropertyChanged(() => EmployeeName);
                }
            }
        }
        
        public string EmployeePosition
        {
            get
            {
                return Entity.EmployeePosition;
            }
            set
            {
                if (Entity.EmployeePosition != value)
                {
                    Entity.EmployeePosition = value;
                    RaisePropertyChanged(() => EmployeePosition);
                }
            }
        }

        public string LicenseHolder
        {
            get
            {
                return Entity.LicenseHolder;
            }
            set
            {
                if (Entity.LicenseHolder != value)
                {
                    Entity.LicenseHolder = value;
                    RaisePropertyChanged(() => LicenseHolder);
                }
            }
        }

        public string Address
        {
            get
            {
                return Entity.Address;
            }
            set
            {
                if (Entity.Address != value)
                {
                    Entity.Address = value;
                    RaisePropertyChanged(() => Address);
                }
            }
        }

        public string LicenseActivity
        {
            get
            {
                return Entity.LicensedActivity;
            }
            set
            {
                if (Entity.LicensedActivity != value)
                {
                    Entity.LicensedActivity = value;
                    RaisePropertyChanged(() => LicenseActivity);
                }
            }
        }

        public string Violations
        {
            get
            {
                return Entity.Violations;
            }
            set
            {
                if (Entity.Violations != value)
                {
                    Entity.Violations = value;
                    RaisePropertyChanged(() => Violations);
                }
            }
        }

        #endregion

        #region Binding Commands

        public DelegateCommand PrintViolationNoticeCommand { get; set; }

        private void PrintViolationNotice()
        {
            try
            {
                _noticeReport.ViolationNoticeId = Entity.Id;

                bool isDesigner = false;
#if DEBUG
                isDesigner = true;
#endif
                AvalonInteractor.RaiseOpenReportDockable(Entity.ToString(), _noticeReport, isDesigner);
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка", ex));
            }
        }

        private bool CanPrintViolationNotice()
        {
            return Entity.Id != 0;
        }

        #endregion

        #region IAvalonDockCaller

        /// <summary>
        /// Объект для взаимодействия с AvalonDockVM
        /// </summary>
        public IAvalonDockInteractor AvalonInteractor { get; private set; }

        #endregion

        public override void RaiseAllPropertyChanged()
        {
            base.RaiseAllPropertyChanged();
            RaisePropertyChanged(() => Violations);
        }
    }
}
