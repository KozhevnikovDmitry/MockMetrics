using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Types.Exceptions;
using Common.UI.ViewModel.SearchViewModel;
using GU.Archive.BL;
using GU.Archive.DataModel;

namespace GU.Archive.UI.ViewModel.SearchViewModel.SearchResultViewModel
{
    public class PostSearchResultVM : AbstractSearchResultVM<Post>
    {
        public PostSearchResultVM(Post entity)
            : base(entity)
        {
        }

        protected override void Initialize()
        {
            base.Initialize();
            try
            {
                var dm = ArchiveFacade.GetDictionaryManager();
                RegistrationNum = Result.RegistrationNum;
                PostType = dm.GetEnumDictionary<PostType>()[(int) Result.PostType];
                DeliveryType = dm.GetEnumDictionary<DeliveryType>()[(int)Result.DeliveryType];
                Stamp = Result.Stamp.ToShortDateString();
                if (Result.OrganizationId != null)
                    Organization = dm.GetDictionaryItem<Organization>(Result.OrganizationId).ShortName;
                Author = dm.GetDictionaryItem<Author>(Result.AuthorId).Name;
                RegInfo = string.Format("№{0} от {1}, {2}", RegistrationNum, Stamp, PostType);
            }
            catch (Exception ex)
            {
                throw new VMException(ex);
            }
        }

        public string RegistrationNum { get; set; }

        public string PostType { get; set; }

        public string DeliveryType { get; set; }

        public string Stamp { get; set; }

        public string Organization { get; set; }

        public string Author { get; set; }

        public string RegInfo { get; set; }
    }
}
