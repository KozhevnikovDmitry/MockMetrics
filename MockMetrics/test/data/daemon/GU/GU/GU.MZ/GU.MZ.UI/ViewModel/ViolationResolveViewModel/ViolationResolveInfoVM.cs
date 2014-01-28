using System;
using System.Collections.Generic;
using GU.MZ.DataModel.Violation;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.ViolationResolveViewModel
{
    /// <summary>
    /// Класс ViewMolel для отображения данных сущности Информация об устранении нарушений
    /// </summary>
    public class ViolationResolveInfoVm : SmartValidateableVm<ViolationResolveInfo>
    {
        public override void Initialize(ViolationResolveInfo entity)
        {
            base.Initialize(entity);
            ViolationResolvedList = new Dictionary<bool, string>();
            ViolationResolvedList[true] = "Устранены";
            ViolationResolvedList[false] = "Не устранены";
        }

        #region Binding Properties

        /// <summary>
        /// Флаг успешного устранения нарушений
        /// </summary>
        public bool IsResolved
        {
            get
            {
                return Entity.IsResolved;
            }
            set
            {
                if (Entity.IsResolved != value)
                {
                    Entity.IsResolved = value;

                    if (IsResolved)
                    {
                        ReturnStamp = null;
                    }
                    else
                    {
                        ResolveStamp = null;
                    }

                    RaisePropertyChanged(() => IsResolved);
                }
            }
        }

        /// <summary>
        /// Словарь вариантов успешности устранения нарушений
        /// </summary>
        public Dictionary<bool, string> ViolationResolvedList { get; private set; }

        /// <summary>
        /// Дата устранения нарушений
        /// </summary>
        public DateTime? ResolveStamp
        {
            get
            {
                return Entity.ResolveStamp;
            }
            set
            {
                if (Entity.ResolveStamp != value)
                {
                    Entity.ResolveStamp = value;
                    RaisePropertyChanged(() => ResolveStamp);
                }
            }
        }

        /// <summary>
        /// Дата возврата документов    
        /// </summary>
        public DateTime? ReturnStamp
        {
            get
            {
                return Entity.ReturnStamp;
            }
            set
            {
                if (Entity.ReturnStamp != value)
                {
                    Entity.ReturnStamp = value;
                    RaisePropertyChanged(() => ReturnStamp);
                }
            }
        }

        /// <summary>
        /// Примечание
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
            RaisePropertyChanged(() => ReturnStamp);
            RaisePropertyChanged(() => ResolveStamp);
            RaisePropertyChanged(() => Note);
        }
    }
}
