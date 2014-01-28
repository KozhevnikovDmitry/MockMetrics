using System;
using Common.BL.DataMapping;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.AvalonInteraction;
using Common.UI.ViewModel.AvalonInteraction.Interface;
using Common.UI.ViewModel.Event;
using GU.MZ.DataModel.Licensing;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.LicenseViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения списка лицензий.
    /// </summary>
    public class LicenseListVm : SmartListVm<License>, IAvalonDockCaller
    {
        private readonly IDomainDataMapper<License> _licenseMapper;

        /// <summary>
        /// Класс ViewModel для отображения списка лицензий.
        /// </summary>
        public LicenseListVm(IDomainDataMapper<License> licenseMapper)
        {
            _licenseMapper = licenseMapper;
            AvalonInteractor = new MiddleAvalonDockInteractor(this);
        }

        protected override void SetListOptions()
        {
            base.SetListOptions();
            CanAddItems = false;
        }

        protected override void OnOpenItemRequested(object sender, ChooseItemRequestedEventArgs e)
        {
            try
            {
                var license = _licenseMapper.Retrieve(e.Result.GetKeyValue());
                AvalonInteractor.RaiseOpenEditableDockable(e.Result.ToString(), typeof(License), license);
                
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


        #region IAvalonDockCaller

        /// <summary>
        /// Объект для взаимодействия с AvalonDockVM
        /// </summary>
        public IAvalonDockInteractor AvalonInteractor { get; private set; }

        #endregion
    }
}
