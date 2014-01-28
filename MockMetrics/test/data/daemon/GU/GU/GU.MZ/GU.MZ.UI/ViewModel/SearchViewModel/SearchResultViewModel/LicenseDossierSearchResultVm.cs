using Common.BL.DictionaryManagement;
using Common.UI.ViewModel.SearchViewModel;
using GU.MZ.DataModel.Dossier;
using GU.MZ.UI.ViewModel.LicenseDossierViewModel;

namespace GU.MZ.UI.ViewModel.SearchViewModel.SearchResultViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения результата поиска Лицензиатов
    /// </summary>
    public class LicenseDossierSearchResultVm : AbstractSearchResultVM<LicenseDossier>
    {
        private readonly LicenseDossierInfoVm _dossierInfoVm;
        private readonly IDictionaryManager _dictionaryManager;

        public LicenseDossierSearchResultVm(LicenseDossier entity, LicenseDossierInfoVm dossierInfoVm)
            : base(entity, null)
        {
            _dossierInfoVm = dossierInfoVm;
            Initialize();
        }

        protected override void Initialize()
        {
            base.Initialize();
            _dossierInfoVm.Initialize(Result);
        }

        #region Binding Properties

        /// <summary>
        /// Строка номером дела и видом деятельности
        /// </summary>
        public string DossierCommonString { get { return _dossierInfoVm.DossierCommonString; } }

        /// <summary>
        /// Строка с датой заведения
        /// </summary>
        public string CreateDateString { get { return _dossierInfoVm.CreateDateString; } }

        /// <summary>
        /// Строка с информацией о лицензиате
        /// </summary>
        public string HolderDataString { get { return _dossierInfoVm.HolderDataString; } }

        #endregion
    }
}
