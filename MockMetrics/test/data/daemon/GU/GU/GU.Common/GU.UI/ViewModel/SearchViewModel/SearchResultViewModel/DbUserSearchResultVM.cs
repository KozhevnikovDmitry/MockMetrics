using System.Linq;

using Common.UI.ViewModel.SearchViewModel;

using GU.BL;
using GU.DataModel;

namespace GU.UI.ViewModel.SearchViewModel.SearchResultViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения данных сущности Пользователь в элемента списка поиска
    /// </summary>
    public class DbUserSearchResultVM : AbstractSearchResultVM<DbUser>
    {
        /// <summary>
        /// Класс ViewModel для отображения данных сущности DbUser в элемента списка поиска
        /// </summary>
        /// <param name="entity">Объект Пользователь</param>
        public DbUserSearchResultVM(DbUser entity)
            : base(entity)
        {
        }

        /// <summary>
        /// Инициализирует поля привязки VM'а
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            Login = Result.Name;
            this.Fio = Result.UserText;
            Status = GuFacade.GetDictionaryManager()
                             .GetEnumDictionary<DbUserStateType>()
                             .Single(t => t.Key == (int)Result.State)
                             .Value;

            var agency = Result.AgencyId.HasValue
                             ? GuFacade.GetDictionaryManager().GetDictionaryItem<Agency>(Result.AgencyId).Name
                             : "Вне ведомств";

            this.WorkDataString = string.Format("Должность: {0} \n Ведомство: {1}", Result.AppointText, agency);
        }

        #region Binding Properties

        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; private set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public string Fio { get; private set; }

        /// <summary>
        /// Строка с рабочими данными
        /// </summary>
        public string WorkDataString { get; private set; }

        /// <summary>
        /// Статус пользователя
        /// </summary>
        public string Status { get; private set; }


        #endregion
    }
}
