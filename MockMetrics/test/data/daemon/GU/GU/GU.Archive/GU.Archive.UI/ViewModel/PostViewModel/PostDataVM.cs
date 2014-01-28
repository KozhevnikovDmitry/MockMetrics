using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel;
using Common.UI.ViewModel.ValidationViewModel;

using GU.Archive.BL;
using GU.Archive.DataModel;

using Microsoft.Practices.Prism.Commands;

namespace GU.Archive.UI.ViewModel.PostViewModel
{
    public class PostDataVM : ValidateableVM
    {
        public Post Entity { get; protected set; }

        public PostDataVM(Post post, bool isValidateable = true)
            : base(isValidateable)
        {
            AllowValidate = false;
            Entity = post;
            PostTypeList = ArchiveFacade.GetDictionaryManager().GetEnumDictionary<PostType>();
            DeliveryTypeList = ArchiveFacade.GetDictionaryManager().GetEnumDictionary<DeliveryType>();
            AuthorList = ArchiveFacade.GetDictionaryManager().GetDictionary<Author>();
            RequestTypeList = ArchiveFacade.GetDictionaryManager().GetEnumDictionary<RequestType>();
            
            BrowseOrganizationCommand = new DelegateCommand(this.BrowseOrganization);
        }

        #region Binding Properties

        /// <summary>
        /// Регистрационный номер
        /// </summary>
        public string RegistrationNum
        {
            get
            {
                return Entity.RegistrationNum;
            }
            set
            {
                if (Entity.RegistrationNum != value)
                {
                    Entity.RegistrationNum = value;
                    RaisePropertyChanged(() => RegistrationNum);
                }
            }
        }

        public Dictionary<int, string> PostTypeList { get; private set; }

        /// <summary>
        /// Тип корреспонденции
        /// </summary>
        public int PostType
        {
            get
            {
                return (int)Entity.PostType;
            }
            set
            {
                if ((int)Entity.PostType != value)
                {
                    Entity.PostType = (PostType)value;
                    RaisePropertyChanged(() => PostType);
                }
            }
        }

        public Dictionary<int, string> DeliveryTypeList { get; private set; }

        /// <summary>
        /// Способ доставки корреспонденции
        /// </summary>
        public int DeliveryType
        {
            get
            {
                return (int)Entity.DeliveryType;
            }
            set
            {
                if ((int)Entity.DeliveryType != value)
                {
                    Entity.DeliveryType = (DeliveryType)value;
                    RaisePropertyChanged(() => DeliveryType);
                }
            }
        }

        /// <summary>
        /// Наименование организации, из которой приплыла Корреспонденция
        /// </summary>
        public string OrganizationName
        {
            get
            {
                return Entity.Organization != null ? Entity.Organization.ShortName : string.Empty;
            }
        }

        public List<Author> AuthorList { get; set; }

        /// <summary>
        /// автор
        /// </summary>
        public int AuthorId
        {
            get
            {
                return Entity.AuthorId;
            }
            set
            {
                if (Entity.AuthorId != value)
                {
                    Entity.AuthorId = value;
                    RaisePropertyChanged(() => AuthorId);
                }
            }
        }

        public Dictionary<int, string> RequestTypeList { get; set; }

        /// <summary>
        /// тип запроса
        /// </summary>
        public int RequestType
        {
            get
            {
                return (int)Entity.RequestType;
            }
            set
            {
                if ((int)Entity.RequestType != value)
                {
                    Entity.RequestType = (RequestType)value;
                    RaisePropertyChanged(() => RequestType);
                }
            }
        }

        public DateTime Stamp
        {
            get
            {
                return Entity.Stamp;
            }
            set
            {
                if (Entity.Stamp != value)
                {
                    Entity.Stamp = value;
                    RaisePropertyChanged(() => Stamp);
                }
            }
        }

        public string Note
        {
            get
            {
                return Entity.Note;
            }
            set
            {
                if (Entity.Note != value)
                {
                    Entity.Note = value;
                    RaisePropertyChanged(() => Note);
                }
            }
        }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Команда поиска организаций
        /// </summary>
        public DelegateCommand BrowseOrganizationCommand { get; private set; }

        /// <summary>
        /// Открывает окно поиска организаций
        /// </summary>
        private void BrowseOrganization()
        {
            try
            {
                var orgSearchVm = UIFacade.GetSearchVM<Organization>();
                orgSearchVm.IsSearchOpenned = true;
                orgSearchVm.AvalonInteractor.OpenEditableDockable += (sender, args) => { };
                if (UIFacade.ShowSearchDialogView(orgSearchVm, "Поиск организаций", ResizeMode.CanResizeWithGrip, SizeToContent.Manual, new Size(800, 600)))
                {
                    this.Entity.Organization = orgSearchVm.Result;
                    this.Entity.OrganizationId = orgSearchVm.Result.Id;
                    this.RaisePropertyChanged(() => OrganizationName);
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        #endregion

    }
}
