using GU.MZ.DataModel.Licensing;

namespace GU.MZ.UI.ViewModel.DossierFileViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения элемента списка лицензий
    /// </summary>
    public class LicenseListItemVm
    {
        /// <summary>
        /// Лицензия
        /// </summary>
        private readonly License _license;

        /// <summary>
        /// Класс ViewModel для отображения элемента списка лицензий
        /// </summary>
        /// <param name="license">Лицензия</param>
        public LicenseListItemVm(License license)
        {
            _license = license;
            LicenseString = string.Format(
                "Лицензия №{0} от {1}", _license.RegNumber, _license.GrantDate.Value.ToLongDateString());
        }

        #region Binding Properties

        /// <summary>
        /// Строка с информацией о лицензии
        /// </summary>
        public string LicenseString { get; private set; }

        #endregion
    }
}
