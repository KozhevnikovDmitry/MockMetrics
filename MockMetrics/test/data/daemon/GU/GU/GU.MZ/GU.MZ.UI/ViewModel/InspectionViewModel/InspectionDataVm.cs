using System;
using System.Collections.Generic;
using GU.MZ.DataModel.Inspect;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.InspectionViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения данных выездной проверки
    /// </summary>
    public class InspectionDataVm : SmartValidateableVm<Inspection>
    {
        public override void Initialize(Inspection entity)
        {
            base.Initialize(entity);
            InspectionPassList = new Dictionary<bool, string>();
            InspectionPassList[true] = "Успешно пройдена";
            InspectionPassList[false] = "Выявлены нарушения";
        }

        #region Binding Properties

        /// <summary>
        /// Примечание выездной проверки
        /// </summary>
        public string InspectionNote
        {
            get
            {
                return Entity.InspectionNote;
            }
            set
            {
                if (Entity.InspectionNote != value)
                {
                    Entity.InspectionNote = value;
                    RaisePropertyChanged(() => InspectionNote);
                }
            }
        }
        
        /// <summary>
        /// Дата акта проверки
        /// </summary>
        public DateTime ActStamp
        {
            get
            {
                return Entity.ActStamp;
            }
            set
            {
                if (Entity.ActStamp != value)
                {
                    Entity.ActStamp = value;
                    RaisePropertyChanged(() => ActStamp);
                }
            }
        }

        /// <summary>
        /// Дата начала проверки
        /// </summary>
        public DateTime StartStamp
        {
            get
            {
                return Entity.StartStamp;
            }
            set
            {
                if (Entity.StartStamp != value)
                {
                    Entity.StartStamp = value;
                    RaisePropertyChanged(() => StartStamp);
                }
            }
        }

        /// <summary>
        /// Дата окончания проверки
        /// </summary>
        public DateTime EndStamp
        {
            get
            {
                return Entity.EndStamp;
            }
            set
            {
                if (Entity.EndStamp != value)
                {
                    Entity.EndStamp = value;
                    RaisePropertyChanged(() => EndStamp);
                }
            }
        }

        /// <summary>
        /// Флаг успешности прохождения проверки
        /// </summary>
        public bool IsPassed
        {
            get
            {
                return Entity.IsPassed;
            }
            set
            {
                if (Entity.IsPassed != value)
                {
                    Entity.IsPassed = value;
                    RaisePropertyChanged(() => IsPassed);
                }
            }
        }

        /// <summary>
        /// Словарь вариантов успешности прохождения проверки
        /// </summary>
        public Dictionary<bool, string> InspectionPassList { get; private set; }

        #endregion

        public override void RaiseAllPropertyChanged()
        {
            base.RaiseAllPropertyChanged();
            RaisePropertyChanged(() => IsPassed);
        }
    }
}
