using System;
using Common.BL.DictionaryManagement;
using Common.Types;
using Common.Types.Exceptions;
using Common.UI.ViewModel.SearchViewModel;
using GU.MZ.DataModel.Licensing;

namespace GU.MZ.UI.ViewModel.SearchViewModel.SearchResultViewModel
{
    /// <summary>
    /// Класс ViewModel для представления реузльтата поиска лицензий.
    /// </summary>
    public class LicenseSearchResultVm : AbstractSearchResultVM<License>
    {
        private readonly IDictionaryManager _dictionaryManager;

        /// <summary>
        /// Класс ViewModel для представления реузльтата поиска лицензий.
        /// </summary>
        /// <param name="entity">Отображаемая лицензия</param>
        public LicenseSearchResultVm(License entity, IDictionaryManager dictionaryManager)
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
                LicenseDataString = string.Format(
                    "№ {0}; Бланк {1}; выдана {2}",
                    Result.RegNumber,
                    Result.BlankNumber,
                    Result.GrantDate.Value.ToLongDateString());

                LicensedActivityString =
                    _dictionaryManager.GetDictionaryItem<LicensedActivity>(Result.LicensedActivityId).
                        Name;

                LicenseHolderString = Result.ActualRequisites.FullName;

                LicenseStatusString = Result.CurrentStatus.GetDescription();
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
        /// Строка с информацией о лицензии
        /// </summary>
        public string LicenseDataString { get; private set; }

        /// <summary>
        /// Строка с информацией о деятельности
        /// </summary>
        public string LicensedActivityString { get; private set; }

        /// <summary>
        /// Строка с информацией о лицензиате
        /// </summary>
        public string LicenseHolderString { get; private set; }

        /// <summary>
        /// Строка с информацией о статусе лицензии
        /// </summary>
        public string LicenseStatusString { get; private set; }

        #endregion
    }
}
