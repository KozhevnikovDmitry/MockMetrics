using Common.UI.View;
using Common.UI.ViewModel;

namespace Common.UI
{
    /// <summary>
    /// Класс отображающий сплэш заставку
    /// </summary>
    public class SplashService
    {
        /// <summary>
        /// Сплэш окно
        /// </summary>
        private SplashWindow _splashView;

        /// <summary>
        /// Отображает сплэш с именем прилоежния
        /// </summary>
        /// <param name="applicationName">Имя приложения</param>
        public void Show(string applicationName)
        {
            if(_splashView != null)
            {
                Close();
            }

            var splashVm = new SplashVM { ApplicationName = applicationName };
            _splashView = new SplashWindow { DataContext = splashVm };
            _splashView.Show();
        }

        /// <summary>
        /// Закрывает сплэш
        /// </summary>
        public void Close()
        {
            _splashView.Close();
        }
    }
}
