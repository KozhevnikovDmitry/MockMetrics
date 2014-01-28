using Common.BL.DataMapping;
using Common.UI.ViewModel.AvalonInteraction;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using Common.UI.ViewModel.Event;
using GU.MZ.DataModel.Dossier;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.DossierFileViewModel
{
    /// <summary>
    /// Класс ViewMoidel для отображения списка томов лицензионного дела
    /// </summary>
    public class DossierFileListVm : SmartListVm<DossierFile>, IAvalonDockCaller
    {
        private readonly IDomainDataMapper<DossierFile> _fileMapper;

        /// <summary>
        /// Класс ViewMoidel для отображени списка томов лицензионного дела
        /// </summary>
        public DossierFileListVm(IDomainDataMapper<DossierFile> fileMapper)
        {
            _fileMapper = fileMapper;
            AvalonInteractor = new MiddleAvalonDockInteractor(this);
        }

        #region AbstractListVM

        /// <summary>
        /// Устанавливает кастомные настройки списка.
        /// </summary>
        protected override void SetListOptions()
        {
            base.SetListOptions();
            CanAddItems = false;
        }

        /// <summary>
        /// Обрабатывает запрос на открытие элемента списка.
        /// </summary>
        /// <param name="sender">Элемент списка</param>
        /// <param name="e">Аругменты события</param>
        protected override void OnOpenItemRequested(object sender, ChooseItemRequestedEventArgs e)
        {
            var dossierFile = _fileMapper.Retrieve(e.Result.GetKeyValue());
            AvalonInteractor.RaiseOpenEditableDockable(e.Result.ToString(), typeof(DossierFile), dossierFile);
        }

        #endregion

        #region IAvalonDockCaller

        /// <summary>
        /// Объект для взаимодействия с AvalonDockVM
        /// </summary>
        public IAvalonDockInteractor AvalonInteractor { get; private set; }

        #endregion
    }
}
