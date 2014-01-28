using Common.BL.DictionaryManagement;
using GU.MZ.DataModel.Licensing;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.LicenseViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения данных объекта с номенклатурой в списке
    /// </summary>
    public class LicenseObjectItemVm : SmartListItemVm<LicenseObject>
    {
        private readonly IDictionaryManager _dictionaryManager;

        public LicenseObjectItemVm(IDictionaryManager dictionaryManager)
        {
            _dictionaryManager = dictionaryManager;
        }

        #region Binding Properties

        /// <summary>
        /// Наименование объекта с номенклатурой
        /// </summary>
        public string Name { get { return Item.Name; } }

        /// <summary>
        /// Реквизиты решения о предоставления лицензии 
        /// </summary>
        public string Order { get { return string.Format("Приказ №{0} от {1}", Item.GrantOrderRegNumber, Item.GrantOrderStamp.HasValue ? Item.GrantOrderStamp.Value.ToShortDateString() : "Не указано"); } }

        /// <summary>
        /// Адрес объекта с номенклатурой
        /// </summary>
        public string Address { get { return Item.Address.ToString(); } }

        /// <summary>
        /// Статус объекта с номенклатурой
        /// </summary>
        public string Status { get { return _dictionaryManager.GetDictionaryItem<LicenseObjectStatus>(Item.LicenseObjectStatusId).Name; } }

        #endregion

        public override void RaiseAllPropertyChanged()
        {
            base.RaiseAllPropertyChanged();
            RaisePropertyChanged(() => Order);
            RaisePropertyChanged(() => Status);
        }
    }
}
