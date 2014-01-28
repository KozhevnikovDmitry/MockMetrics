using System;
using System.Linq;
using System.Collections.Generic;
using Common.BL.Validation;
using Common.UI.ViewModel.ListViewModel;
using GU.HQ.BL;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.UI.ViewModel.DeclarerViewModel
{
    public class DeclarerRelativesItemVM : AbstractListItemVM<DeclarerRelative>
    {
        public Dictionary<int, string> SexList { get; private set; }
        public List<RelativeType> RelativeTypeList { get; private set; }

        public DeclarerRelativesItemVM(DeclarerRelative entity, IDomainValidator<DeclarerRelative> domainValidator, bool isValidateable)
            : base(entity, domainValidator, isValidateable)
        {
            SexList = HqFacade.GetDictionaryManager().GetEnumDictionary<Sex>();
            RelativeTypeList = HqFacade.GetDictionaryManager().GetDictionary<RelativeType>();
        }

        protected override void Initialize()
        {
            if (Entity.Person == null)
                Entity.Person = Person.CreateInstance();
        }


        #region Binding Properties

        /// <summary>
        /// Заголовок родственника
        /// </summary>
        public string RelativeInfo
        {
            get
            {
                return String.Format("{0} {1} {2} {3}", Entity.Person.Sname, Entity.Person.Name, Entity.Person.Patronymic, Entity.RelativeTypeId == 0 ? "": RelativeTypeList.Single(t => t.Id == Entity.RelativeTypeId).ToString());
            }
        }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string Sname
        {
            get { return Entity.Person == null ?  "" : Entity.Person.Sname; }
            set
            {
                if (!string.IsNullOrEmpty(value) && Entity.Person.Sname != value)
                {
                    Entity.Person.Sname = value;
                    RaisePropertyChanged(() => RelativeInfo);
                }
            }
        }


        /// <summary>
        /// Имя
        /// </summary>
        public string Name
        {
            get { return Entity.Person == null ? "" : Entity.Person.Name; }
            set
            {
                if (!string.IsNullOrEmpty(value) && Entity.Person.Name != value)
                {
                    Entity.Person.Name = value;
                    RaisePropertyChanged(() => RelativeInfo);
                }
            }
        }

        /// <summary>
        /// Отчество
        /// </summary>
        public string Patronymic
        {
            get { return Entity.Person == null ? "" : Entity.Person.Patronymic; }
            set
            {
                if (!string.IsNullOrEmpty(value) && Entity.Person.Patronymic != value)
                {
                    Entity.Person.Patronymic = value;
                    RaisePropertyChanged(() => RelativeInfo);
                }
            }
        }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime BirthDate
        {
            get
            {
                return Entity.Person != null && Entity.Person.BirthDate != DateTime.MinValue // проверка нужна чтоб защититься от пустого (только созданного) Person
                           ? Entity.Person.BirthDate
                           : DateTime.Now;
            }
            set
            {
                    Entity.Person.BirthDate = value;
            }
        }

        /// <summary>
        /// Родственное отношение
        /// </summary>
        public int RelativeTypeId
        {
            get { return Entity == null ? 0 : Entity.RelativeTypeId; }
            set
            {
                if (Entity.RelativeTypeId != value)
                {
                    Entity.RelativeTypeId = value;
                    RaisePropertyChanged(() => RelativeInfo);
                }
            }
        }

        /// <summary>
        /// Пол
        /// </summary>
        public int SexId
        {
            get
            {
                return Entity.Person != null && Entity.Person.Sex != 0 // проверка нужна чтоб защититься от пустого (только созданного) Person
                           ? (int) Entity.Person.Sex
                           : (int) Sex.Male;
            }
            set
            {
                if ((int)Entity.Person.Sex != value)
                    Entity.Person.Sex = (Sex)value;
            }
        }

        #endregion Binding Properties
    }
}
