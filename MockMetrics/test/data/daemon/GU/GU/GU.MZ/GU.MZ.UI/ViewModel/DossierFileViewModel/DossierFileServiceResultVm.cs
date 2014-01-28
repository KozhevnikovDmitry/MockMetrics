using System;
using System.Collections.Generic;
using System.Linq;
using Common.BL.DictionaryManagement;
using GU.MZ.DataModel.Dossier;
using GU.MZ.DataModel.FileScenario;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.DossierFileViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения данных сущности Результат предоставления услуги в томе
    /// </summary>
    public class DossierFileServiceResultVm : SmartValidateableVm<DossierFileServiceResult>
    {
        private readonly IDictionaryManager _dictionaryManager;

        /// <summary>
        /// Класс ViewModel для отображения данных сущности Результат предоставления услуги в томе
        /// </summary>
        public DossierFileServiceResultVm(IDictionaryManager dictionaryManager)
        {
            _dictionaryManager = dictionaryManager;
        }

        public override void Initialize(DossierFileServiceResult entity)
        {
            base.Initialize(entity);
            ServiceResultString = _dictionaryManager.GetDictionaryItem<ServiceResult>(entity.ServiceResultId).Name;
            ResultGrantList = new List<string> { "Вручено лично", "Направлено по почте" };
            GrantWay = ResultGrantList.First();

        }

        #region Binding Properties

        private string _serviceResultString;

        /// <summary>
        /// Строка с ожидаемым результатом
        /// </summary>
        public string ServiceResultString
        {
            get { return _serviceResultString; }
            private set
            {
                if (value != _serviceResultString)
                {
                    _serviceResultString = value;
                    RaisePropertyChanged(() => ServiceResultString);
                }
            }
        }

        /// <summary>
        /// Строка с процедурой предоставления результата
        /// </summary>
        private string _grantResultProcedureString;

        /// <summary>
        /// Возвращает или устанавливает строку с процедурой предоставления результата
        /// </summary>
        public string GrantResultProcedureString
        {
            get
            {
                return _grantResultProcedureString;
            }
            set
            {
                if (_grantResultProcedureString != value)
                {
                    _grantResultProcedureString = value;
                    RaisePropertyChanged(() => GrantResultProcedureString);
                }
            }
        }

        /// <summary>
        /// Информация о способе предоставления результата 
        /// </summary>
        public string GrantWay
        {
            get
            {
                return Entity.GrantWay;
            }
            set
            {
                if (Entity.GrantWay != value)
                {
                    Entity.GrantWay = value;
                    RaisePropertyChanged(() => GrantWay);
                }
            }
        }

        /// <summary>
        /// Словарь вариантов предоставленности результата тома
        /// </summary>
        public List<string> ResultGrantList { get; private set; }

        /// <summary>
        /// Дата предоставления результата
        /// </summary>
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

        /// <summary>
        /// Примечение к результату
        /// </summary>
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
        
        public override void RaiseAllPropertyChanged()
        {
            base.RaiseAllPropertyChanged();
            RaisePropertyChanged(() => ServiceResultString);
            RaisePropertyChanged(() => GrantResultProcedureString);
            RaisePropertyChanged(() => GrantWay);
            RaisePropertyChanged(() => Stamp);
            RaisePropertyChanged(() => Note);
        }
    }
}
