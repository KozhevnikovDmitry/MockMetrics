using System;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Common.DA;
using Common.DA.Interface;


namespace GU.HQ.DataModel
{
    [TableName("gu_hq.claim_status")]
    public abstract class ClaimStatusHist : IdentityDomainObject<ClaimStatusHist>, IPersistentObject
    {
        [PrimaryKey, Identity]
        [SequenceName("gu_hq.claim_status_seq")]
        [MapField("claim_status_id")]
        public abstract override int Id { get; set; }

        [MapField("claim_id")]
        public abstract int ClaimId { get; set; }

        [MapField("claim_status_type_id")]
        public abstract int ClaimStatusTypeId { get; set; }

        [MapField("date")]
        public abstract DateTime Date { get; set; }

        [MapField("u_user_id")]
        public abstract int UUserId { get; set; }
    }
}