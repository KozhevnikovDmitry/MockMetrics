using System;
using System.Linq;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel;
using Common.UI.ViewModel.Event;
using Common.UI.Views;
using GU.Archive.BL;
using GU.Archive.DataModel;
using GU.Archive.UI.View.EmployeeView;
using GU.Archive.UI.View.OrganizationView;
using GU.Archive.UI.View.PostView;
using GU.Archive.UI.ViewModel.EmployeeViewModel;
using GU.Archive.UI.ViewModel;
using GU.Archive.UI.ViewModel.PostViewModel;
using GU.BL;
using GU.BL.Policy;
using GU.DataModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.Archive.UI.ViewModel
{
    public class ArchiveRepositoryVM : BaseAvalonDockVM
    {
        public ArchiveRepositoryVM()
            : base(new SingletonDockableUiFactory())
        {
            ShowEmployeesCommand = new DelegateCommand(ShowEmployees, IsAdministrator);
            CreateNewEmployeeCommand = new DelegateCommand(CreateNewEmployee, IsAdministrator);

            ShowOrganizationCommand = new DelegateCommand(ShowOrganization, IsAdministrator);
            CreateNewOrganizationCommand = new DelegateCommand(CreateNewOrganization, IsAdministrator);

            CreateNewPostCommand = new DelegateCommand(CreateNewPost);
            ShowPostsCommand=new DelegateCommand(ShowPosts);

            JumpToTaskCommand = new DelegateCommand(JumpToTask, () => IsJumpToTaskAvailable);
        }

        #region Binding Properties

        #endregion

        #region Binding Commands

        #region Post

        public DelegateCommand CreateNewPostCommand { get; protected set; }

        public DelegateCommand ShowPostsCommand { get; protected set; }

        private void CreateNewPost()
        {
            try
            {
                this.AvalonInteractor.RaiseOpenEditableDockable("Новая корреспонденция", typeof(Post), Post.CreateInstance());
            }
            catch (DALException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        private void ShowPosts()
        {
            try
            {
                this.AvalonInteractor.RaiseOpenSearchDockable("Корреспонденция", typeof(Post));
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

        #region Employee

        public DelegateCommand ShowEmployeesCommand { get; protected set; }

        public DelegateCommand CreateNewEmployeeCommand { get; protected set; }

        private void CreateNewEmployee()
        {
            try
            {
                this.AvalonInteractor.RaiseOpenEditableDockable("Новый сотрудник", typeof(Employee), Employee.CreateInstance());
            }
            catch (DALException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        private void ShowEmployees()
        {
            try
            {
                this.AvalonInteractor.RaiseOpenSearchDockable("Сотрудники", typeof(Employee));
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

        #region Organization

        public DelegateCommand ShowOrganizationCommand { get; protected set; }

        public DelegateCommand CreateNewOrganizationCommand { get; protected set; }

        private void CreateNewOrganization()
        {
            try
            {
                this.AvalonInteractor.RaiseOpenEditableDockable("Новая организация", typeof(Organization), Organization.CreateInstance());
            }
            catch (DALException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        private void ShowOrganization()
        {
            try
            {
                this.AvalonInteractor.RaiseOpenSearchDockable("Организации", typeof(Organization));
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

        public DelegateCommand JumpToTaskCommand { get; protected set; }

        private void JumpToTask()
        {
            try
            {
                var postVm = ActiveWorkspaceVM as EditableVM<Post>;
                if (postVm != null && postVm.Entity.Task != null)
                {
                    AvalonInteractor.RaiseOpenEditableDockable(postVm.Entity.Task.ToString(), typeof(Task), postVm.Entity.Task, true);
                }  
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(ex);
            }
        }

        public bool IsJumpToTaskAvailable
        {
            get
            {
                var postVm = ActiveWorkspaceVM as EditableVM<Post>;
                if (postVm != null && postVm.Entity.Task != null)
                {
                    return true;
                }

                return false;
            }
        }

        #endregion

        protected override void NotifyActiveDocumentChanged()
        {
            RaisePropertyChanged(() => IsJumpToTaskAvailable);
            JumpToTaskCommand.RaiseCanExecuteChanged();

            base.NotifyActiveDocumentChanged();
        }

        private bool IsAdministrator()
        {
            return GuFacade.GetDbUser().HasRole(RoleConstants.GU_USER_ADMIN);
        }
    }
}
