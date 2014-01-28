using System;
using System.Collections.Generic;
using Common.BL.DictionaryManagement;
using GU.MZ.DataModel.Licensing;

using Microsoft.Practices.Prism.ViewModel;

using System.Linq;

namespace GU.MZ.UI.ViewModel.ReportDialogViewModel
{
    /// <summary>
    /// Класс ViewModel для задания данных по отчёту "Полный отчёт по лицензированию по виду деятельности"
    /// </summary>
    public class NewFullActivityReportVm : NotificationObject
    {
        /// <summary>
        /// Класс ViewModel для задания данных по отчёту "Полный отчёт по лицензированию по виду деятельности"
        /// </summary>
        public NewFullActivityReportVm(IDictionaryManager dictionaryManager)
        {
            LicensedActivityList = dictionaryManager.GetDictionary<LicensedActivity>();

            // TODO : это нормально, что всегда по одной деятельности делается отчёт?
            LicensedActivity = LicensedActivityList.First();
            Date1 = null;
            Date2 = DateTime.Now;
        }

        #region Binding Properties

        /// <summary>
        /// Лицензируемая деятельность
        /// </summary>
        private LicensedActivity _licensedActivity;

        /// <summary>
        /// Лицензируемая деятельность
        /// </summary>
        public LicensedActivity LicensedActivity
        {
            get
            {
                return _licensedActivity;
            }
            set
            {
                if (_licensedActivity != value)
                {
                    _licensedActivity = value;
                    RaisePropertyChanged(() => LicensedActivity);
                }
            }
        }

        /// <summary>
        /// Список видов лицензируемой деятельности
        /// </summary>
        public List<LicensedActivity> LicensedActivityList { get; private set; }

        private DateTime? _date1;

        public DateTime? Date1
        {
            get
            {
                return _date1;
            }
            set
            {
                if (_date1 != value)
                {
                    _date1 = value;
                    RaisePropertyChanged(() => Date1);
                }
            }
        }

        private DateTime? _date2;

        public DateTime? Date2
        {
            get
            {
                return _date2;
            }
            set
            {
                if (_date2 != value)
                {
                    _date2 = value;
                    RaisePropertyChanged(() => Date2);
                }
            }
        }

        #endregion
    }
}
