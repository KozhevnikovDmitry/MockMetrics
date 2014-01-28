using System;
using Common.BL.DictionaryManagement;
using Common.Types.Exceptions;
using Common.UI.ViewModel.SearchViewModel;
using GU.MZ.DataModel.Person;

namespace GU.MZ.UI.ViewModel.SearchViewModel.SearchResultViewModel
{
    /// <summary>
    /// Класс ViewModel для представления реузльтата поиска экспертов.
    /// </summary>
    public class ExpertSearchResultVm : AbstractSearchResultVM<Expert>
    {
        private readonly IDictionaryManager _dictionaryManager;

        /// <summary>
        /// Класс ViewModel для представления реузльтата поиска экспертов.
        /// </summary>
        /// <param name="entity">Отображаемый сотрудник</param>
        public ExpertSearchResultVm(Expert entity, IDictionaryManager dictionaryManager)
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
            try
            {
                PersonDataString = Result.ExpertState.GetName();
                WorkDataString = Result.ExpertState.GetWorkdata();
                AccreditateDataString = string.Format(
                    "Свидетельство № {0} до {1}",
                    Result.AccreditateDocumentNumber,
                    Result.AccreditationDueDate.Value.ToLongDateString());
                ActivityDataString =
                    _dictionaryManager.GetDictionaryItem<AccreditateActivity>(
                        Result.AccreditateActivityid).Name;
            }
            catch (BLLException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new VMException(ex);
            }
        }

        #region Binding Properties

        /// <summary>
        /// Строка с персональными данными сотрудника
        /// </summary>
        public string PersonDataString { get; set; }

        /// <summary>
        /// Строка с информацией о служебном положении сотрудника
        /// </summary>
        public string WorkDataString { get; set; }

        /// <summary>
        /// Строка с контактными данными сотрудника
        /// </summary>
        public string AccreditateDataString { get; set; }

        /// <summary>
        /// Строка с инвормацией о штатной принадлежности сотрудника.
        /// </summary>
        public string ActivityDataString { get; set; }

        #endregion
    }
}
