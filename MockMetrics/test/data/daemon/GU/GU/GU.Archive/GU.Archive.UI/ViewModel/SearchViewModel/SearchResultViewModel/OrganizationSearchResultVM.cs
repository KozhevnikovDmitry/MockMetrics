using System;
using Common.Types.Exceptions;
using Common.UI.ViewModel.SearchViewModel;
using GU.Archive.DataModel;

namespace GU.Archive.UI.ViewModel.SearchViewModel.SearchResultViewModel
{
    public class OrganizationSearchResultVM : AbstractSearchResultVM<Organization>
    {
        public OrganizationSearchResultVM(Organization entity)
            : base(entity)
        {

        }

        protected override void Initialize()
        {
            base.Initialize();
            try
            {
                InnOgrnString = string.Format("{0} / {1}", Result.Inn, Result.Ogrn);
                OrganizationNameString = Result.ShortName;
                HeadDataString = Result.HeadName;
                AddressString = Result.Address.ToString();
            }
            catch (Exception ex)
            {
                throw new VMException(ex);
            }
        }

        #region Binding Properties

        public string InnOgrnString { get; set; }

        public string OrganizationNameString { get; set; }

        public string HeadDataString { get; set; }

        public string AddressString { get; set; }

        #endregion

    }
}
