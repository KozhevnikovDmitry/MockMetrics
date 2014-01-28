using System;
using System.Collections.Generic;
using System.Linq;
using Common.BL.Validation;
using Common.UI.ViewModel.ListViewModel;
using GU.HQ.BL;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.UI.ViewModel.PersonViewModel
{
    public class PersonDocItemVM : AbstractListItemVM<PersonDoc>
    {
        private PersonDoc _personDoc;
        
        public PersonDocItemVM(PersonDoc entity, IDomainValidator<PersonDoc> domainValidator, bool isValidateable) 
            : base(entity, domainValidator, isValidateable)
        {
            _personDoc = entity;
            PersonDocumentTypeList = HqFacade.GetDictionaryManager().GetDictionary<PersonDocumentType>();
        }

        /// <summary>
        /// просто надо
        /// </summary>
        protected override void Initialize()
        {

        }

        #region Binding Properties

        /// <summary>
        /// Список оснований регистрации
        /// </summary>
        public List<PersonDocumentType> PersonDocumentTypeList { get; private set; }

        /// <summary>
        /// Документ заявителя
        /// </summary>
        private PersonDoc PersonDoc 
        { 
            get { return _personDoc ?? (_personDoc = PersonDoc.CreateInstance()); }
        }

        /// <summary>
        /// Id тип документа
        /// </summary>
        public int DocumentTypeId
        {
            get { return _personDoc.DocumentTypeId; }
            set
            {
                if (PersonDoc.DocumentTypeId != value)
                    PersonDoc.DocumentTypeId = value;

                RaisePropertyChanged(() => DocumentTypeId);
            } 
        }

        /// <summary>
        /// серия документа
        /// </summary>
        public string Seria
        {
            get { return _personDoc.Seria; }
            set
            {
                if (!PersonDoc.Seria.Equals(value))
                    PersonDoc.Seria = value;
            }
        }

        /// <summary>
        /// номер документа
        /// </summary>
        public string Num
        {
            get { return _personDoc.Num; }
            set
            {
                if (!PersonDoc.Num.Equals(value))
                    PersonDoc.Num = value;

                RaisePropertyChanged(() => Num);
            } 
        }

        /// <summary>
        /// Дата выдачи документа
        /// </summary>
        public DateTime DateDoc
        {
            get { return _personDoc.DateDoc == DateTime.MinValue ? DateTime.Now.Date : _personDoc.DateDoc; }
            set 
            {
                if (PersonDoc.DateDoc != value)
                    PersonDoc.DateDoc = value;
            }
        }

        /// <summary>
        /// организация которая выдала документ
        /// </summary>
        public string Org
        {
            get { return _personDoc.Org; }
            set
            {
                if (!PersonDoc.Org.Equals(value))
                    PersonDoc.Org = value;
            }
        }

        /// <summary>
        /// Код подразделения
        /// </summary>
        public string OrgCode
        {
            get { return _personDoc.OrgCode; }
            set
            {
                if (!PersonDoc.OrgCode.Equals(value))
                    PersonDoc.OrgCode = value;
            }
        }

        /// <summary>
        /// комментарий
        /// </summary>
        public string Note
        {
            get { return _personDoc.Note; }
            set
            {
                if (!PersonDoc.Note.Equals(value))
                    PersonDoc.Note = value;
            }
        }

        #region Common 

        /// <summary>
        /// Развернутый свернутый элемент
        /// </summary>
        public bool IsExpander
        {
            get { return _personDoc == null; }
        }

        public string DocNameShort
        {
            get { return _personDoc == null ? "" : _personDoc.DocumentTypeId == 0 ? "" : PersonDocumentTypeList.Single(x => x.Id == _personDoc.DocumentTypeId).ToString() + " № " + _personDoc.Num.ToString(); }
        }

        #endregion Common

        #endregion Binding Properties
    }
}
