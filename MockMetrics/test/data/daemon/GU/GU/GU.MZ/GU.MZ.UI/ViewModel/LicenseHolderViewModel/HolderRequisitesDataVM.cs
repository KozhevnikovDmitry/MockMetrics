using System;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Requisites;
using GU.MZ.UI.View;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.LicenseHolderViewModel
{
    /// <summary>
    /// Класс ViewModel для окна редактирования данных реквизитов лицензиата
    /// </summary>
    public class HolderRequisitesDataVm : SmartValidateableVm<HolderRequisites>
    {
        private readonly IDialogUiFactory _uiFactory;
        private readonly ISmartValidateableVm<Address> _addressVm;

        /// <summary>
        /// Класс ViewModel для окна редактирования данных Лицензиата
        /// </summary>
        public HolderRequisitesDataVm(IDialogUiFactory uiFactory, 
                                      ISmartValidateableVm<Address> addressVm, 
                                      ISmartValidateableVm<JurRequisites> jurRequisitesDataVm, 
                                      ISmartValidateableVm<IndRequisites> indRequisitesDataVm)
        {
            _uiFactory = uiFactory;
            _addressVm = addressVm;
            _jurRequisitesDataVm = jurRequisitesDataVm;
            _indRequisitesDataVm = indRequisitesDataVm;
            AddChild(_jurRequisitesDataVm);
            AddChild(_indRequisitesDataVm);
        }

        public override void Initialize(HolderRequisites entity)
        {
            base.Initialize(entity);
            ShowAddressCommand = new DelegateCommand(ShowAddress);

            if (Entity.JurRequisites != null)
            {
                _jurRequisitesDataVm.Initialize(Entity.JurRequisites);
            }
            else
            {
                _indRequisitesDataVm.Initialize(Entity.IndRequisites);
            }
        }

        #region Binding Properties

        private readonly ISmartValidateableVm<IndRequisites> _indRequisitesDataVm;

        public ISmartValidateableVm<IndRequisites> IndRequisitesDataVm
        {
            get
            {
                return _indRequisitesDataVm.Entity != null ? _indRequisitesDataVm : null;
            }
        }

        private readonly ISmartValidateableVm<JurRequisites> _jurRequisitesDataVm;

        public ISmartValidateableVm<JurRequisites> JurRequisitesDataVm
        {
            get
            {
                return _jurRequisitesDataVm.Entity != null? _jurRequisitesDataVm : null;
            }
             
        }
        
        /// <summary>
        /// Юридический адрес организации
        /// </summary>
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

        #endregion

        #region Binding Commands

        /// <summary>
        /// Команда открытия окна просмотра юридического адреса лицензиата
        /// </summary>
        public DelegateCommand ShowAddressCommand { get; private set; }

        /// <summary>
        /// Открывает окно просмотра юридического адреса лицензиата
        /// </summary>
        private void ShowAddress()
        {
            try
            {
                var address = Entity.Address != null ? Entity.Address.Clone() : Address.CreateInstance();
                _addressVm.Initialize(address);
                if (_uiFactory.ShowDialogView(new AddressView(), _addressVm, "Адрес лицензиата"))
                {
                    if (!_addressVm.Entity.IsDirty)
                    {
                         _addressVm.Entity.CopyTo(Entity.Address);
                    }
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
