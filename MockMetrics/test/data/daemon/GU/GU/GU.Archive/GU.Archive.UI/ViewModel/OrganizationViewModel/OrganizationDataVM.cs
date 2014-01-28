using System;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel;
using Common.UI.ViewModel.ValidationViewModel;

using GU.Archive.DataModel;
using GU.Archive.UI.View;
using Microsoft.Practices.Prism.Commands;

namespace GU.Archive.UI.ViewModel.OrganizationViewModel
{
    public class OrganizationDataVM : ValidateableVM
    {
        public Organization Entity { get; protected set; }

        public bool IsEditable { get; set; }

        public OrganizationDataVM(Organization organization, bool isValidateable = true)
            : base(isValidateable)
        {
            IsEditable = true;
            AllowValidate = false;
            Entity = organization;
            OpenAddressCommand = new DelegateCommand(OpenAddress);
        }

        public void RaiseIsEditableChanged()
        {
            RaisePropertyChanged(() => IsEditable);
        }
        
        #region Binding Properties

        public string Inn
        {
            get
            {
                return Entity.Inn;
            }
            set
            {
                if (Entity.Inn != value)
                {
                    Entity.Inn = value;
                    RaisePropertyChanged(() => Inn);
                }
            }
        }

        public string Ogrn
        {
            get
            {
                return Entity.Ogrn;
            }
            set
            {
                if (Entity.Ogrn != value)
                {
                    Entity.Ogrn = value;
                    RaisePropertyChanged(() => Ogrn);
                }
            }
        }

        public Address Address
        {
            get
            {
                return Entity.Address;
            }
            set
            {
                if (Entity.Address != value)
                {
                    Entity.Address = value;
                    RaisePropertyChanged(() => Address);
                }
            }
        }

        public string HeadName
        {
            get
            {
                return Entity.HeadName;
            }
            set
            {
                if (Entity.HeadName != value)
                {
                    Entity.HeadName = value;
                    RaisePropertyChanged(() => HeadName);
                }
            }
        }

        public string FullName
        {
            get
            {
                return Entity.FullName;
            }
            set
            {
                if (Entity.FullName != value)
                {
                    Entity.FullName = value;
                    RaisePropertyChanged(() => FullName);
                }
            }
        }

        public string ShortName
        {
            get
            {
                return Entity.ShortName;
            }
            set
            {
                if (Entity.ShortName != value)
                {
                    Entity.ShortName = value;
                    RaisePropertyChanged(() => ShortName);
                }
            }
        }

        public string Phone
        {
            get
            {
                return Entity.Phone;
            }
            set
            {
                if (Entity.Phone != value)
                {
                    Entity.Phone = value;
                    RaisePropertyChanged(() => Phone);
                }
            }
        }

        public string Fax
        {
            get
            {
                return Entity.Fax;
            }
            set
            {
                if (Entity.Fax != value)
                {
                    Entity.ShortName = value;
                    RaisePropertyChanged(() => Fax);
                }
            }
        }

        public string Email
        {
            get
            {
                return Entity.Email;
            }
            set
            {
                if (Entity.Email != value)
                {
                    Entity.Email = value;
                    RaisePropertyChanged(() => Email);
                }
            }
        }
        
        #endregion

        #region Binding Commands

        public DelegateCommand OpenInnDocumentCommand { get; protected set; }

        public DelegateCommand OpenOgrnDocumentCommand { get; protected set; }

        public DelegateCommand OpenAddressCommand { get; protected set; }

        private void OpenAddress()
        {
            try
            {
                AddressVM addressVM = 
                    new AddressVM(Address == null ? Address.CreateInstance() : Address.Clone()) { IsEditable = this.IsEditable };
                if (IsEditable)
                {
                    if (UIFacade.ShowValidateableDialogView(new AddressView(), addressVM, "Адрес"))
                    {
                        Address = addressVM.Address;
                        Entity.AddressId = addressVM.Address.Id;
                        RaisePropertyChanged(() => Address);
                    }
                }
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new VMException("Непредвиденная ошибка", ex));
            }
        }

        #endregion
    }
}
