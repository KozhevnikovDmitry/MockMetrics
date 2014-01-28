using System;
using System.ComponentModel;
using GU.HQ.DataModel.Person;

namespace GU.HQ.UI.ViewModel.Person
{
    public class PersonDocVM
    {
        private PersonDoc _personDoc;

        public PersonDocVM(PersonDoc personDoc)
        {
            _personDoc = personDoc;
        }


        public object DocumentType
        {
            get { return _personDoc.DocumentType; }
        }

        public object Seria
        {
            get { return _personDoc.Seria; }
        }

        public object Num
        {
            get { return _personDoc.Num; }
        }

        public DateTime DateDoc
        {
            get { return _personDoc.DateDoc; }
        }

        public object Org
        {
            get { return _personDoc.Org; }
        }

        public object OrgCode
        {
            get { return _personDoc.OrgCode; }
        }

        public object Note
        {
            get { return _personDoc.Note; }
        }
    }
}
