using Microsoft.Practices.Prism.ViewModel;

namespace Common.UI.ViewModel
{
    /// <summary>
    /// Класс VM для переключателя модулей приложения
    /// </summary>
    public class ModuleSelectorVM : NotificationObject
    {
        #region Binding Properties

        /// <summary>
        /// Путь к иконке переключателя
        /// </summary>
        public string IconPath { get; set; }

        /// <summary>
        /// Отображаемое имя переключателя
        /// </summary>
        public string DisplayName { get; set; }

        #endregion
    }
}
