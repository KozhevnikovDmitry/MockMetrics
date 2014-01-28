using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.Interfaces;
using GU.Archive.BL;
using GU.Archive.DataModel;
using GU.BL;
using GU.DataModel;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.Archive.UI.ViewModel
{
    public class RegistrateTaskVM : NotificationObject, IConfirmableVM
    {
        public RegistrateTaskVM(Task task)
        {
            _task = task;
            AuthorList = ArchiveFacade.GetDictionaryManager().GetDictionary<Author>();
            RequestTypeList = ArchiveFacade.GetDictionaryManager().GetEnumDictionary<RequestType>();
            BrowseOrganizationCommand = new DelegateCommand(this.BrowseOrganization);
        }

        private Task _task;

        public Post Post { get; private set; }

        private int _organizationId;

        #region Binding Properties

        /// <summary>
        /// Регистрационный номер
        /// </summary>
        private string _registrationNum;

        /// <summary>
        /// Регистрационный номер
        /// </summary>
        public string RegistrationNum
        {
            get
            {
                return _registrationNum;
            }
            set
            {
                if (_registrationNum != value)
                {
                    _registrationNum = value;
                    RaisePropertyChanged(() => RegistrationNum);
                }
            }
        }

        /// <summary>
        /// Наименование организации, из которой приплыла Корреспонденция
        /// </summary>
        private string _organizationName;

        /// <summary>
        /// Возвращает или устанавливает, наименование организации, из которой приплыла Корреспонденция
        /// </summary>
        public string OrganizationName
        {
            get
            {
                return _organizationName;
            }
            set
            {
                if (_organizationName != value)
                {
                    _organizationName = value;
                    RaisePropertyChanged(() => OrganizationName);
                }
            }
        }

        public List<Author> AuthorList { get; set; }

        /// <summary>
        /// автор
        /// </summary>
        private int _authorId;

        public int AuthorId
        {
            get
            {
                return _authorId;
            }
            set
            {
                if (_authorId != value)
                {
                    _authorId = value;
                    RaisePropertyChanged(() => AuthorId);
                }
            }
        }

        public Dictionary<int, string> RequestTypeList { get; set; }

        /// <summary>
        /// тип запроса
        /// </summary>
        private int _requestType;

        public int RequestType
        {
            get
            {
                return _requestType;
            }
            set
            {
                if (_requestType != value)
                {
                    _requestType = value;
                    RaisePropertyChanged(() => RequestType);
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
                    var organization = orgSearchVm.Result;
                    this._organizationId = organization.Id;
                    OrganizationName = organization.ShortName;
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

        #region Implementation of IConfirmableVM

        public void Confirm()
        {
            try
            {
                if (string.IsNullOrEmpty(RegistrationNum) || AuthorId == 0 || RequestType == 0)
                {
                    NoticeUser.ShowWarning("Необходимо заполнить все поля");
                    return;
                }

                // change Task state
                var taskPolicy = GuFacade.GetTaskPolicy();
                taskPolicy.SetStatus(TaskStatusType.Accepted, "Регистрация в канцелярии", _task);

                // create Post
                var post = Post.CreateInstance();
                post.RegistrationNum = RegistrationNum;
                if (_organizationId != 0)
                {
                    post.OrganizationId = _organizationId;
                    post.Organization =
                        ArchiveFacade.GetDictionaryManager().GetDictionaryItem<Organization>(_organizationId);
                }
                post.AuthorId = AuthorId;
                post.RequestType = (RequestType) RequestType;
                post.PostType = PostType.Incoming;
                post.DeliveryType = DeliveryType.GU;
                post.Stamp = DateTime.Now;
                post.TaskId = _task.Id;
                post.Task = _task;
                // TODO: все что выше нужно куда-то вынести

                post = ArchiveFacade.GetDataMapper<Post>().Save(post);

                Post = post;
                IsConfirmed = true;
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(ex);
            }
        }

        public void ResetAfterFail()
        {
            _task.RejectChanges();
        }

        public bool IsConfirmed { get; private set; }

        #endregion
    }
}
