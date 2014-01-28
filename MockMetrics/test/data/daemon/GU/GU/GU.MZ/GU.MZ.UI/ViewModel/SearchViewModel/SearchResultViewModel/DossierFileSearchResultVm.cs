using System;
using System.Linq;
using Common.BL.DictionaryManagement;
using Common.UI.ViewModel.SearchViewModel;
using GU.BL.Extensions;
using GU.DataModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Licensing;
using GU.UI.Extension;

namespace GU.MZ.UI.ViewModel.SearchViewModel.SearchResultViewModel
{
    /// <summary>
    /// Класс ViewModel для представления реузльтата поиска сотрудников.
    /// </summary>
    public class DossierFileSearchResultVm : AbstractSearchResultVM<DossierFile>
    {
        private readonly IDictionaryManager _dictionaryManager;

        /// <summary>
        /// Класс ViewModel для представления реузльтата поиска томов лицензионного дела.
        /// </summary>
        /// <param name="entity">Отображаемый том лицензионного дела</param>
        public DossierFileSearchResultVm(DossierFile entity, IDictionaryManager dictionaryManager)
            : base(entity, null)
        {
            _dictionaryManager = dictionaryManager;
            Initialize();
        }

        /// <summary>
        /// Инициализация полей привязки.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            DossierFileType = Result.LicenseDossierId == null ? "Прототип тома" : "Том дела";

            DossierFileString = string.Format("Рег. номер №{0} от {1}. Текущий этап ведения: {2}", 
                                              Result.RegNumber, 
                                              Result.CreateDate.ToLongDateString(),
                                              Result.Scenario.ScenarioStepList.Single(t => t.Id == Result.CurrentScenarioStepId));

            TaskDataString = string.Format("ID={0} от {1} на услугу {2}",
                                            Result.Task.Id,
                                            Result.CreateDate.ToLongDateString(),
                                            _dictionaryManager.GetDictionaryItem<Service>(Result.Service.Id).Name);

            LicenseDossierString = Result.CurrentStatus == DossierFileStatus.Unbounded ? 
                                       "Том ещё не привязан к лицензионному делу"
                                       : string.Format("№{0} по виду деятельности {1}",
                                                       Result.LicenseDossier.RegNumber,
                                                       _dictionaryManager.GetDictionaryItem<LicensedActivity>(Result.LicenseDossier.LicensedActivityId).Name);

            if (Result.CurrentStatus == DossierFileStatus.Unbounded)
            {
                LicenseHolderString = "Том ещё не привязан к лицензионному делу";
            }
            else
            {
                var actualRequisites =
                    Result.LicenseDossier.LicenseHolder.RequisitesList.OrderByDescending(t => t.CreateDate).First();
                LicenseHolderString = actualRequisites.FullName;
            }

            var status = Result.Task.StatusList.OrderByDescending(t => t.Stamp).FirstOrDefault();
            ShortStatusString = _dictionaryManager.GetEnumDictionary<TaskStatusType>()[(int)Result.TaskState];
            if (status != null)
            {
                StatusDataString = string.Format("{0} {1} {2}", ShortStatusString, status.Stamp.ToLongDateString(), status.Stamp.ToLongTimeString());
            }
            else
            {
                StatusDataString = ShortStatusString;
            }
            IconPath = Result.TaskState.GetIconPath();

            // TODO : переделать проставление флажков относительно текущего этапа ведения.
            IsDeadlineSoon = Result.Task.DueDate.Value <= DateTime.Today.AddWorkingDays(1);
            IsDeadlineFailed = Result.Task.DueDate.Value < DateTime.Today;
        }

        #region Binding Properties

        /// <summary>
        /// Строка с информацией о типе тома
        /// </summary>
        public string DossierFileType { get; private set; }

        /// <summary>
        /// Строка с информацией по том
        /// </summary>
        public string DossierFileString { get; private set; }

        /// <summary>
        /// Строка с информацией о заявке
        /// </summary>
        public string TaskDataString { get; private set; }

        /// <summary>
        /// Строка с информацией о лицензионном деле
        /// </summary>
        public string LicenseDossierString { get; private set; }

        /// <summary>
        /// Строка с информацией о лицензиате
        /// </summary>
        public string LicenseHolderString { get; private set; }

        /// <summary>
        /// Строка с информацией о статусе заявки
        /// </summary>
        public string StatusDataString { get; private set; }

        /// <summary>
        /// Строка с краткой информацией о статусе заявки
        /// </summary>
        public string ShortStatusString { get; private set; }

        /// <summary>
        /// Путь до иконки статуса
        /// </summary>
        public string IconPath { get; private set; }

        /// <summary>
        /// Флажок, указывающий на приближение срока завершения текущего этапа
        /// </summary>
        public bool IsDeadlineSoon { get; private set; }

        /// <summary>
        /// Флажок, указывающий на просрочку срока завершения этапа
        /// </summary>
        public bool IsDeadlineFailed { get; private set; }

        #endregion
    }
}
