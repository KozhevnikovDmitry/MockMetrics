using System;
using System.Collections.Generic;
using System.Windows;
using Common.BL.DataMapping;
using Common.BL.DictionaryManagement;
using Common.Types.Exceptions;
using Common.UI;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.Licensing;
using GU.MZ.UI.ViewModel.SmartViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.MZ.UI.ViewModel.LicenseViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения данных лицензии.
    /// </summary>
    public class LicenseDataVm : SmartValidateableVm<License>
    {
        private readonly ISearchDialogFactory _uiFactory;
        private readonly IDictionaryManager _dictionaryManager;
        private readonly IDomainDataMapper<LicenseDossier> _dossierMapper;

        /// <summary>
        /// Класс ViewModel для отображения данных лицензии.
        /// </summary>
        public LicenseDataVm(ISearchDialogFactory uiFactory,
                             IDictionaryManager dictionaryManager,
                             IDomainDataMapper<LicenseDossier> dossierMapper)
        {
            _uiFactory = uiFactory;
            _dictionaryManager = dictionaryManager;
            _dossierMapper = dossierMapper;
            LicensedActivityList = _dictionaryManager.GetDictionary<LicensedActivity>();
            SearchLicenseDossierCommand = new DelegateCommand(SearchLicenseDossier, CanSearchLicenseDossier);
        }

        #region Binding Properties

        #region License Fields

        /// <summary>
        /// Регистрационный номер лицензии
        /// </summary>
        public string RegNumber
        {
            get
            {
                return Entity.RegNumber;
            }
            set
            {
                if (Entity.RegNumber != value)
                {
                    Entity.RegNumber = value;
                    RaisePropertyChanged(() => RegNumber);
                }
            }
        }

        /// <summary>
        /// Дата выдачи лицензии
        /// </summary>
        public DateTime? GrantDate
        {
            get
            {
                return Entity.GrantDate;
            }
            set
            {
                if (Entity.GrantDate != value)
                {
                    Entity.GrantDate = value;
                    RaisePropertyChanged(() => GrantDate);
                }
            }
        }

        /// <summary>
        /// Дата окончания срока действия лицензии
        /// </summary>
        public DateTime? DueDate
        {
            get
            {
                return Entity.DueDate;
            }
            set
            {
                if (Entity.DueDate != value)
                {
                    Entity.DueDate = value;
                    RaisePropertyChanged(() => DueDate);
                }
            }
        }

        /// <summary>
        /// Номер бланка лицензии
        /// </summary>
        public string BlankNumber
        {
            get
            {
                return Entity.BlankNumber;
            }
            set
            {
                if (Entity.BlankNumber != value)
                {
                    Entity.BlankNumber = value;
                    RaisePropertyChanged(() => BlankNumber);
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
        /// Имя главы лицензирующей организации
        /// </summary>
        public string LicensiarHeadName
        {
            get
            {
                return Entity.LicensiarHeadName;
            }
            set
            {
                if (Entity.LicensiarHeadName != value)
                {
                    Entity.LicensiarHeadName = value;
                    RaisePropertyChanged(() => LicensiarHeadName);
                }
            }
        }

        /// <summary>
        /// Должность главы лицензирующей организации
        /// </summary>
        public string LicensiarHeadPosition
        {
            get
            {
                return Entity.LicensiarHeadPosition;
            }
            set
            {
                if (Entity.LicensiarHeadPosition != value)
                {
                    Entity.LicensiarHeadPosition = value;
                    RaisePropertyChanged(() => LicensiarHeadPosition);
                }
            }
        }

        /// <summary>
        /// Id лицензируемой деятельности
        /// </summary>
        public int LicensedActivityId
        {
            get
            {
                return Entity.LicensedActivityId;
            }
            set
            {
                if (Entity.LicensedActivityId != value)
                {
                    Entity.LicensedActivityId = value;
                    RaisePropertyChanged(() => LicensedActivityId);
                }
            }
        }

        /// <summary>
        /// Список видов лицензируемой деятельности
        /// </summary>
        public List<LicensedActivity> LicensedActivityList { get; private set; }

        #endregion


        #region Dossier Fields

        /// <summary>
        /// Регистрационный номер дела
        /// </summary>
        public string DossierRegNumber
        {
            get
            {
                return Entity.LicenseDossier != null ? Entity.LicenseDossier.RegNumber : string.Empty;
            }
        }    

        /// <summary>
        /// ИНН лицензиата
        /// </summary>
        public string Inn
        {
            get
            {
                return Entity.LicenseHolder != null ? Entity.LicenseHolder.Inn : string.Empty;
            }
        }

        /// <summary>
        /// ОГРН лицензиата
        /// </summary>
        public string Ogrn
        {
            get
            {
                return Entity.LicenseHolder != null ? Entity.LicenseHolder.Ogrn : string.Empty;
            }
        }

        /// <summary>
        /// Полное имя лицензиата
        /// </summary>
        public string HolderFullName
        {
            get
            {
                return Entity.ActualRequisites != null ? Entity.ActualRequisites.FullName : string.Empty;
            }
        }
        
        #endregion

        #endregion

        #region Binding Commands

        /// <summary>
        /// Команда поиска лицензионного дела
        /// </summary>
        public DelegateCommand SearchLicenseDossierCommand { get; private set; }

        /// <summary>
        /// Открывает окно поиска лицензионного дела
        /// </summary>
        private void SearchLicenseDossier()
        {
            try
            {
                var dossierSearchVm = _uiFactory.GetSearchVm<LicenseDossier>();
                dossierSearchVm.IsSearchOpenned = true;
                dossierSearchVm.AvalonInteractor.OpenEditableDockable += (sender, args) => { };
                if (_uiFactory.ShowSearchDialogView(dossierSearchVm, "Поиск лицензионного дела", ResizeMode.CanResizeWithGrip, SizeToContent.Manual, new Size(800, 600)))
                {
                    Entity.LicenseDossier = _dossierMapper.Retrieve(dossierSearchVm.Result.Id);
                    Entity.LicenseDossierId = Entity.LicenseDossier.Id;

                    var requisites = Entity.LicenseDossier.LicenseHolder.ActualRequisites;
                    var licenseRequisites = requisites.ToLicenseRequisites();
                    Entity.LicenseRequisitesList.Add(licenseRequisites);
                    
                    RaisePropertyChanged(() => Ogrn);
                    RaisePropertyChanged(() => Inn);
                    RaisePropertyChanged(() => HolderFullName);
                    RaisePropertyChanged(() => DossierRegNumber);
                    SearchLicenseDossierCommand.RaiseCanExecuteChanged();
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

        private bool CanSearchLicenseDossier()
        {
            try
            {
                return Entity.LicenseDossier == null;
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }

            return false;
        }
        
        #endregion

        public override void RaiseAllPropertyChanged()
        {
            base.RaiseAllPropertyChanged();
            RaisePropertyChanged(() => DueDate);
            RaisePropertyChanged(() => DossierRegNumber);
            RaisePropertyChanged(() => Inn);
            RaisePropertyChanged(() => Ogrn);
            RaisePropertyChanged(() => HolderFullName);
        }
    }
}
