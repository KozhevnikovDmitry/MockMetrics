using System.Collections.Generic;
using System.Linq;
using GU.BL;
using GU.DataModel;
using GU.HQ.BL;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.UI.ViewModel.ClaimViewModel
{
    public class ClaimSummaryShortVM
    {
        private Claim _claim;
        private Dictionary<int, string> _claimStatusList { get; set; }
        private List<QueueBaseRegType> _queueBaseRegTypeList { get; set; }
        private List<Queue> _queueList { get; set; }
        private List<Agency> _agencyList { get; set; }

        public ClaimSummaryShortVM(Claim claim)
        {
            _claim = claim;
            _queueBaseRegTypeList = HqFacade.GetDictionaryManager().GetDictionary<QueueBaseRegType>();
            _queueList = HqFacade.GetDictionaryManager().GetDictionary<Queue>();
            _claimStatusList = HqFacade.GetDictionaryManager().GetEnumDictionary<ClaimStatusType>();
            _agencyList = GuFacade.GetDictionaryManager().GetDictionary<Agency>();
        }


        #region Binding Properties
        
        /// <summary>
        /// идентификатор заявления в системе
        /// </summary>
        public int Id { get { return _claim.Id; } }

        /// <summary>
        /// Дата подачи заявления
        /// </summary>
        public string ClaimDate{ get { return _claim.ClaimDate == null ? "" :  _claim.ClaimDate.Value.ToString("dd.MM.yyyy HH:mm:ss"); }
        }

        /// <summary>
        /// Статус заявления
        /// </summary>
        public string ClaimStatus { get { return _claimStatusList[(int)_claim.CurrentStatusTypeId]; } }

        /// <summary>
        /// номер в очереди
        /// </summary>
        public string QueueNum { get { return _claim.QueueClaim == null ? "-" : _queueList.Find(t => t.Id == _claim.QueueClaim.QueueId).QueueTypeId == (int)QueueType.QueueSimple ? _claim.QueueClaim.QueueNum.ToString() : "-"; } }

        /// <summary>
        /// номер в очереди внеочередников
        /// </summary>
        public string QueuePrivNum { get { return _claim.QueueClaim == null ? "-" : _queueList.Find(t => t.Id == _claim.QueueClaim.QueueId).QueueTypeId == (int)QueueType.QueuePriv ? _claim.QueueClaim.QueueNum.ToString() : "-"; } }

        /// <summary>
        /// Номера пунктов подпунктов оснований постановки на учет указанные заявителем
        /// </summary>
        public string DocNums
        {
            get
            {
                var str = "";
                foreach (var bri in _claim.DeclarerBaseReg.BaseRegItems)
                    str = str + _queueBaseRegTypeList.Single(x => x.Id == bri.QueueBaseRegTypeId).DocNum.ToString() + ", ";
                
                return str.Length > 2 ? str.Substring(0, str.Length - 2) : str;
            }
        }

        /// <summary>
        /// Наименование учереждения
        /// </summary>
        public string AgencyName { get { return _agencyList.Find(t => t.Id == _claim.AgencyId).LongName; } }

        #endregion Binding Properties
    }
}
