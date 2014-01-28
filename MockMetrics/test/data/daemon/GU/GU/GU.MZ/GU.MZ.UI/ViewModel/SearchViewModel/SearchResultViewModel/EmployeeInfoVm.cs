using Common.BL.DataMapping;
using GU.MZ.DataModel.Person;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.SearchViewModel.SearchResultViewModel
{
    public class EmployeeInfoVm : EntityInfoVm<Employee>
    {
        public EmployeeInfoVm(IDomainDataMapper<Employee> entityMapper) : base(entityMapper)
        {
        }

        #region Binding Properties

        /// <summary>
        /// Строка с персональными данными сотрудника
        /// </summary>
        public string PersonDataString
        {
            get { return Entity.Name; }
        }

        /// <summary>
        /// Строка с информацией о служебном положении сотрудника
        /// </summary>
        public string WorkDataString
        {
            get { return Entity.Position; }
        }

        /// <summary>
        /// Строка с контактными данными сотрудника
        /// </summary>
        public string ContactsDataString
        {
            get { return string.Format("тел.: {0}, email: {1}", Entity.Phone, Entity.Email); }
        }

        /// <summary>
        /// Строка с инвормацией о штатной принадлежности сотрудника.
        /// </summary>
        public string StuffDataString
        {
            get { return Entity.IsStaff ? "Штатный сотрудник" : "Внештатный сотрудник"; }
        }

        #endregion
    }
}