using System;
using System.Collections.Generic;
using GU.MZ.DataModel.Inspect;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.ExpertiseViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения данных сущности документарная проверка
    /// </summary>
    public class DocumentExpertiseVm : SmartValidateableVm<DocumentExpertise>
    {
        public override void Initialize(DocumentExpertise entity)
        {
            base.Initialize(entity);
            ExpertisePassList = new Dictionary<bool, string>();
            ExpertisePassList[true] = "Успешно пройдена";
            ExpertisePassList[false] = "Выявлены нарушения";
        }

        #region Binding Properties

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
        public Dictionary<bool, string> ExpertisePassList { get; private set; }

        #endregion

        public override void RaiseAllPropertyChanged()
        {
            base.RaiseAllPropertyChanged();
            RaisePropertyChanged(() => IsPassed);
        }
    }
}
