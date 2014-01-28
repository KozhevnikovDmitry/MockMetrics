using System;
using BLToolkit.EditableObjects;
using Common.DA;
using GU.DataModel;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.BL.Search.SearchDomain
{
    // часть велосипеда, без этого пидали не нажимаются 
    public class SearchClaim : Claim
    {
        public override PersistentState PersistentState { get; set; }
        public override int Id { get; set; }
        public override ClaimStatusType CurrentStatusTypeId { get; set; }
        public override int DeclarerId { get; set; }
        public override Person Declarer { get; set; }
        public override int AgencyId { get; set; }
        public override Agency Agency { get; set; }
        public override string AreaFileNum { get; set; }
        public override DateTime? ClaimDate { get; set; }
        public override string Note { get; set; }
        public override int TaskId { get; set; }
        public override DeclarerBaseReg DeclarerBaseReg { get; set; }
        public override EditableList<DeclarerRelative> Relatives { get; set; }
        public override EditableList<ClaimCategory> ClaimCategories { get; set; }
        public override ClaimQueueReg QueueReg { get; set; }
        public override EditableList<Notice> Notices { get; set; }
        public override EditableList<QueuePriv> QueuePrivList { get; set; }
        public override ClaimQueueDeReg QueueDeReg { get; set; }
        public override HouseProvided HouseProvided { get; set; }
        public override QueueClaim QueueClaim { get; set; }
        public override EditableList<ClaimStatusHist> ClaimStatusHist { get; set; }
    }
}
