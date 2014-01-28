using System;
using Common.DA;
using GU.DataModel;

namespace GU.BL.Search.SearchDomain
{
    public class SearchTask : Task
    {
        public int? FakeId { get; set; }

        public int? FakeServiceId { get; set; }

        //TODO: реализовать поиск по agency
        public int? FakeAgencyId { get; set; }

        public override DateTime? CreateDate { get; set; }

        public override DateTime? DueDate { get; set; }

        public override DateTime? CloseDate { get; set; }

        public override int? ContentId { get; set; }

        public override Content Content { get; set; }

        public int? FakeCurrentState { get; set; }

        public override string CustomerFio { get; set; }

        public override string CustomerPhone { get; set; }

        public override string CustomerEmail { get; set; }

        #region Task stub implementation

        public override int Id
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override int ServiceId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override int AgencyId
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override TaskStatusType CurrentState
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override BLToolkit.EditableObjects.EditableList<TaskStatus> StatusList
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override string Note
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override string AuthCode
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override PersistentState PersistentState { get; set; }

        #endregion        
    }
}
