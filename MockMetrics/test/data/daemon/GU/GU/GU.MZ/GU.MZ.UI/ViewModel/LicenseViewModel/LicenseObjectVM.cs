using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Common.BL.DictionaryManagement;
using Common.Types;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.Interface;
using GU.MZ.DataModel;
using GU.MZ.DataModel.Licensing;
using GU.MZ.UI.View;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel.LicenseViewModel
{
    /// <summary>
    /// Класс ViewModel отображения данных объекта с номенклатурой
    /// </summary>
    public class LicenseObjectVm : SmartValidateableVm<LicenseObject>
    {
        private readonly IDialogUiFactory _uiFactory;
        private readonly IDictionaryManager _dictionaryManager;
        private readonly ISmartValidateableVm<Address> _addressVm;

        /// <summary>
        /// Класс ViewModel отображения данных объекта с номенклатурой.
        /// </summary>
        public LicenseObjectVm(IDialogUiFactory uiFactory,
                               IDictionaryManager dictionaryManager,
                               ISmartValidateableVm<Address> addressVm)
        {
            _uiFactory = uiFactory;
            _dictionaryManager = dictionaryManager;
            _addressVm = addressVm;
        }

        public override void Initialize(LicenseObject entity)
        {
            base.Initialize(entity);
            EditAddressCommand = new DelegateCommand(EditAddress);
            LicenseObjectStatusList = _dictionaryManager.GetDictionary<LicenseObjectStatus>();
            SetActivityDictionary();
        }

        /// <summary>
        /// Заполняет список поддеятельностей
        /// </summary>
        private void SetActivityDictionary()
        {
            _objectSubactivityList = new List<VisSubAct>();

            _dictionaryManager.GetDictionary<LicensedSubactivity>()
                      .Where(t => t.LicensedActivityId == Entity.License.LicensedActivityId)
                      .ToList()
                      .ForEach(t => _objectSubactivityList.Add(new VisSubAct {LicensedSubactivity = t, IsChecked = false }));

            Entity.ObjectSubactivityList.ForEach(
                t => _objectSubactivityList.Single(s => s.LicensedSubactivity.Id == t.LicensedSubactivityId).IsChecked = true);

            _objectSubactivityList.ForEach(t => t.PropertyChanged += OnObjectSubactivityChecked);

            ObjectSubactivityList = new ListCollectionView(_objectSubactivityList);
            ObjectSubactivityList.GroupDescriptions.Add(
                new PropertyGroupDescription(
                    Util.GetPropertyName(() => new VisSubAct().GroupName)));
        }

        /// <summary>
        /// Обрабатывает событие выбора или удаления поддеятельности
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void OnObjectSubactivityChecked(object s, PropertyChangedEventArgs e)
        {
            var visSubAct = s as VisSubAct;
            if (visSubAct.IsChecked)
            {
                var objSubactivity = ObjectSubactivity.CreateInstance();
                objSubactivity.LicensedSubactivityId = visSubAct.LicensedSubactivity.Id;
                objSubactivity.LicensedSubactivity = visSubAct.LicensedSubactivity;
                Entity.ObjectSubactivityList.Add(objSubactivity);
            }
            else
            {
                Entity.ObjectSubactivityList.RemoveAll(t => t.LicensedSubactivityId == visSubAct.LicensedSubactivity.Id);
            }
        }

        #region Binding Properties

        /// <summary>
        /// Наименование объекта с номенклатурой
        /// </summary>
        public string Name
        {
            get
            {
                return Entity.Name;
            }
            set
            {
                if (Entity.Name != value)
                {
                    Entity.Name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        /// <summary>
        /// Номер решения о предоставлении лицензии
        /// </summary>
        public string GrantOrderRegNumber
        {
            get
            {
                return Entity.GrantOrderRegNumber;
            }
            set
            {
                if (Entity.GrantOrderRegNumber != value)
                {
                    Entity.GrantOrderRegNumber = value;
                    RaisePropertyChanged(() => GrantOrderRegNumber);
                }
            }
        }

        /// <summary>
        /// Дата решения о предоставлении лицензии
        /// </summary>
        public DateTime? GrantOrderStamp
        {
            get
            {
                return Entity.GrantOrderStamp;
            }
            set
            {
                if (Entity.GrantOrderStamp != value)
                {
                    Entity.GrantOrderStamp = value;
                    RaisePropertyChanged(() => GrantOrderStamp);
                }
            }
        }

        /// <summary>
        /// Id статуса объекта с номенклатурой
        /// </summary>
        public int LicenseObjectStatusId
        {
            get
            {
                return Entity.LicenseObjectStatusId;
            }
            set
            {
                if (Entity.LicenseObjectStatusId != value)
                {
                    Entity.LicenseObjectStatusId = value;
                    RaisePropertyChanged(() => LicenseObjectStatusId);
                }
            }
        }

        /// <summary>
        /// Список статусов объектов с номенклатурой
        /// </summary>
        public List<LicenseObjectStatus> LicenseObjectStatusList { get; private set; }
        
        /// <summary>
        /// Адрес объекта с номенклатурой
        /// </summary>
        public string Address
        {
            get
            {
                return Entity.Address.ToString();
            }
            set
            {

            }
        }

        /// <summary>
        /// Список поддеятельностей
        /// </summary>
        private List<VisSubAct> _objectSubactivityList;

        public ICollectionView ObjectSubactivityList { get; set; }

        #endregion

        #region Binding Commands

        /// <summary>
        /// Команда редактирования адреса объекта с номенклатурой
        /// </summary>
        public DelegateCommand EditAddressCommand { get; private set; }

        /// <summary>
        /// Открывает диалог редактирования адреса
        /// </summary>
        private void EditAddress()
        {
            try
            {
                _addressVm.Initialize(Entity.Address.Clone());
                if (_uiFactory.ShowValidateableDialogView(new AddressView(), _addressVm, "Адрес объекта с номенклатурой"))
                {
                    Entity.Address = _addressVm.Entity;
                    RaisePropertyChanged(() => Address);
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

        public class VisSubAct : NotificationObject
        {
            private bool _isChecked;

            public bool IsChecked
            {
                get
                {
                    return _isChecked;
                }
                set
                {
                    if (_isChecked != value)
                    {
                        _isChecked = value;
                        RaisePropertyChanged(() => IsChecked);
                    }
                }
            }
            
            public LicensedSubactivity LicensedSubactivity { get; set; }

            public string GroupName
            {
                get
                {
                    return LicensedSubactivity.SubactivityGroup.Name;
                }
            }
        }
    }
}
