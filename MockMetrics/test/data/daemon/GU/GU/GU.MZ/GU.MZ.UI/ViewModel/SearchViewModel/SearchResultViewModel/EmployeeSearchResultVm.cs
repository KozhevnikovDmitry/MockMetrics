using System;

using Common.Types.Exceptions;
using Common.UI.ViewModel.SearchViewModel;

using GU.MZ.DataModel.Person;

namespace GU.MZ.UI.ViewModel.SearchViewModel.SearchResultViewModel
{
    /// <summary>
    /// Класс ViewModel для представления реузльтата поиска сотрудников.
    /// </summary>
    public class EmployeeSearchResultVm : AbstractSearchResultVM<Employee>
    {
        /// <summary>
        /// Класс ViewModel для представления реузльтата поиска сотрудников.
        /// </summary>
        /// <param name="entity">Отображаемый сотрудник</param>
        public EmployeeSearchResultVm(Employee entity)
            : base(entity, null)
        {

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
                PersonDataString = Result.Name;
                WorkDataString = Result.Position;
                ContactsDataString = string.Format("тел.: {0}, email: {1}", Result.Phone, Result.Email);
                StuffDataString = Result.IsStaff ? "Штатный сотрудник" : "Внештатный сотрудник";
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
        public string PersonDataString { get; set;}

        /// <summary>
        /// Строка с информацией о служебном положении сотрудника
        /// </summary>
        public string WorkDataString { get; set; }

        /// <summary>
        /// Строка с контактными данными сотрудника
        /// </summary>
        public string ContactsDataString { get; set; }

        /// <summary>
        /// Строка с инвормацией о штатной принадлежности сотрудника.
        /// </summary>
        public string StuffDataString { get; set; }

        #endregion
    }
}
