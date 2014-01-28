using System;
using System.Collections.Generic;
using Common.BL.Validation;
using Common.UI.ViewModel.ValidationViewModel;
using GU.HQ.BL;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;


namespace GU.HQ.UI.ViewModel.PersonViewModel
{
    public class PersonDisabilityVM : DomainValidateableVM<PersonDisability>
    {
        public PersonDisabilityVM(PersonDisability entity, IDomainValidator<PersonDisability> validator, bool isValidateable = true)
            : base(entity, validator, isValidateable)
        {
            DisabilityTypeList = HqFacade.GetDictionaryManager().GetDictionary<DisabilityType>();
        }
        


        #region BindingProperties

        #region Disability

        /// <summary>
        /// Справочник группы инвалидности
        /// </summary>
        public List<DisabilityType> DisabilityTypeList { get; private set; }
      
        /// <summary>
        /// группа льготы
        /// </summary>
        public int DisabilityTypeId
        {
            get { return  Entity == null ? 0 : Entity.DisabilityTypeId; }
            set { Entity.DisabilityTypeId = value; }
        }

        /// <summary>
        /// Бессрочная инвалидность
        /// </summary>
        public bool DisabilityUnlim
        {
            get
            {
                return (Entity != null && Entity.DisabilityDate != null && DateTime.Compare(Entity.DisabilityDate.Value, Convert.ToDateTime("31.12.2999")) == 0);
            }
            set 
            {
                Entity.DisabilityDate = value ? Convert.ToDateTime("31.12.2999") : (DateTime?)null;
                RaisePropertyChanged(() => DisabilityUnlim);
                RaisePropertyChanged(() => DisabilityNoUnlim);
            }
        }

        /// <summary>
        /// Ивалидность не бессрочная
        /// </summary>
        public bool DisabilityNoUnlim { get { return !DisabilityUnlim; } }

        /// <summary>
        /// Дата инвалидности
        /// </summary>
        public DateTime? DisabilityDate
        {
            get { return Entity == null ? null : Entity.DisabilityDate; }
            set { Entity.DisabilityDate = value; }
        }

        #endregion Desability

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Note
        {
            get { return Entity == null ? "" : Entity.Note; }
            set
            {
                if (!String.IsNullOrEmpty(value) && !Entity.Note.Equals(value))
                    Entity.Note = value;
            }
        }

        #region IsPoor

        /// <summary>
        /// Малоимущий
        /// </summary>
        public bool IsPoor
        {
            get { return Entity != null && Entity.IsPoor != 0; }
            set
            {
                Entity.IsPoor = value ? 1 : 0;
                RaisePropertyChanged(() => IsPoor);
            }
        }

        /// <summary>
        /// Дата документа о признании малоимущим
        /// </summary>
        public DateTime? IsPoorDocDate 
        {
            get { return Entity == null ? null : Entity.IsPoorDocDate; } 
            set
            {
                Entity.IsPoorDocDate = value;
            }
        }

        /// <summary>
        /// номер документа о признании малоимущим
        /// </summary>
        public string IsPoorDocNum
        {
            get { return Entity == null ? "" : Entity.IsPoorDocNum; }
            set 
            {
                if (!string.IsNullOrEmpty(value) && !Entity.IsPoorDocNum.Equals(value))
                    Entity.IsPoorDocNum = value;
            }
        }

        #endregion IsPoor

        /// <summary>
        /// Номер документа соцзащиты
        /// </summary>
        public string UsznDocNum
        {
            get { return Entity == null ? null : Entity.UsznDocNum; }
            set 
            {
                if (!String.IsNullOrEmpty(value) && !Entity.UsznDocNum.Equals(value))
                    Entity.UsznDocNum = value;
            }
        }

        /// <summary>
        /// Дата документа соцзащиты
        /// </summary>
        public DateTime? UsznDocDate
        {
            get { return Entity == null ? null : Entity.UsznDocDate; }
            set { Entity.UsznDocDate = value; }
        }

        #endregion BindingProperties
    }
}
