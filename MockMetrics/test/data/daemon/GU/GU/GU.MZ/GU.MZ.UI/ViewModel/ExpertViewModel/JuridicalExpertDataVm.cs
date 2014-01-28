using System;
using System.Collections.Generic;
using Common.BL.DictionaryManagement;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Holder;
using GU.MZ.DataModel.Person;
using GU.MZ.UI.View;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.ExpertViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения данных эксперта юридического лица
    /// </summary>
    public class JuridicalExpertDataVm : SmartValidateableVm<JuridicalExpertState>
    {
        private readonly IDialogUiFactory _uiFactory;

        private readonly IDictionaryManager _dictionaryManager;
        private readonly ISmartValidateableVm<Address> _addressVm;

        /// <summary>
        /// Класс ViewModel для отображения данных эксперта юридического лица
        /// </summary>
        public JuridicalExpertDataVm(IDialogUiFactory uiFactory,
                                     IDictionaryManager dictionaryManager,
                                     ISmartValidateableVm<Address> addressVm)
        {
            _uiFactory = uiFactory;
            _dictionaryManager = dictionaryManager;
            _addressVm = addressVm;
        }

        public override void Initialize(JuridicalExpertState entity)
        {
            base.Initialize(entity);
            LegalFormList = _dictionaryManager.GetDictionary<LegalForm>();
            EditAddressCommand = new DelegateCommand(EditAddress);
        }

        #region Binding Properties

        /// <summary>
        /// Полнове наименование лицензиата
        /// </summary>
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

        /// <summary>
        /// Короткое наименование лицензиата
        /// </summary>
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

        /// <summary>
        /// Фирменное наименование лицензиата
        /// </summary>
        public string FirmName
        {
            get
            {
                return Entity.FirmName;
            }
            set
            {
                if (Entity.FirmName != value)
                {
                    Entity.FirmName = value;
                    RaisePropertyChanged(() => FirmName);
                }
            }
        }

        /// <summary>
        /// ИНН лицензиата
        /// </summary>
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

        /// <summary>
        /// ОГРН лицензиата
        /// </summary>
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

        /// <summary>
        /// ФИО главы организации лицензиата
        /// </summary>
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

        /// <summary>
        /// Должность главы организации лицензиата
        /// </summary>
        public string HeadPositionName
        {
            get
            {
                return Entity.HeadPositionName;
            }
            set
            {
                if (Entity.HeadPositionName != value)
                {
                    Entity.HeadPositionName = value;
                    RaisePropertyChanged(() => HeadPositionName);
                }
            }
        }

        /// <summary>
        /// Список организационно-правовых форм
        /// </summary>
        public List<LegalForm> LegalFormList { get; private set; }

        /// <summary>
        /// Id организационно-правовой формы лицензиата
        /// </summary>
        public int LegalFormId
        {
            get
            {
                return Entity.LegalFormId;
            }
            set
            {
                if (Entity.LegalFormId != value)
                {
                    Entity.LegalFormId = value;
                    RaisePropertyChanged(() => LegalFormId);
                }
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
        /// Команда открытия окна просмотра юридического адреса эксперта
        /// </summary>
        public DelegateCommand EditAddressCommand { get; private set; }

        /// <summary>
        /// Открывает окно просмотра юридического адреса эксперта
        /// </summary>
        private void EditAddress()
        {
            try
            {
                var address = Entity.Address != null ? Entity.Address.Clone() : Address.CreateInstance();
                _addressVm.Initialize(address);
                if (_uiFactory.ShowValidateableDialogView(new AddressView(), _addressVm, "Адрес эксперта"))
                {
                    if (_addressVm.Entity.IsDirty)
                    {
                        Address = _addressVm.Entity;
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

        public override void RaiseAllPropertyChanged()
        {
            base.RaiseAllPropertyChanged();
            RaisePropertyChanged(() => Address);
        }
    }
}
