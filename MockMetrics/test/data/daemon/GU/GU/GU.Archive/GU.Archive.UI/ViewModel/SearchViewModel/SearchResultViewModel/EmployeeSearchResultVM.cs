using System;
using System.Linq;
using Common.Types.Exceptions;
using Common.UI.ViewModel.SearchViewModel;
using GU.Archive.BL;
using GU.Archive.DataModel;
using GU.BL;
using GU.DataModel;

namespace GU.Archive.UI.ViewModel.SearchViewModel.SearchResultViewModel
{
    public class EmployeeSearchResultVM : AbstractSearchResultVM<Employee>
    {
        public EmployeeSearchResultVM(Employee entity)
            : base(entity)
        {

        }

        protected override void Initialize()
        {
            base.Initialize();
            try
            {
                PersonDataString = string.Format("{0} {1} {2}", Result.Surname, Result.Name, Result.Patronymic);
                var agency = GuFacade.GetDictionaryManager()
                                     .GetDictionary<Agency>()
                                     .First(t => t.Id == Result.AgencyId).Name;
                WorkDataString = string.Format("{0}, {1}", Result.Position, agency);
                ContactsDataString = string.Format("тел.: {0}, email: {1}", Result.Phone, Result.Email);
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

        public string PersonDataString { get; set;}

        public string WorkDataString { get; set; }

        public string ContactsDataString { get; set; }

        #endregion

        #region Binding Commands

        #endregion

    }
}
