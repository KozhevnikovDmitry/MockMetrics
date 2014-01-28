using System.Collections.Generic;
using GU.HQ.BL;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.UI.ViewModel.ClaimViewModel
{
    public class ClaimShortVM 
    {
        private Claim _claim;
        public Dictionary<int, string> ListClaimStatus { get; private set; }

        public ClaimShortVM(Claim claim)
        {
            _claim = claim;
            ListClaimStatus = HqFacade.GetDictionaryManager().GetEnumDictionary<ClaimStatusType>();
        }

        #region Binding Properties

        public string ClaimDate
        {
            get { return _claim.ClaimDate == null ? "" :  _claim.ClaimDate.Value.ToString("dd.MM.yyyy HH:mm:ss"); }
        }

        public string ClaimStatus
        {
            get { return ListClaimStatus[(int)_claim.CurrentStatusTypeId]; }
        }

        public string ClaimDeclarerFio
        {
            get { return _claim.Declarer.Sname + " " + _claim.Declarer.Name + " " + _claim.Declarer.Patronymic; }
        }

        #endregion Binding Properties
    }
}
