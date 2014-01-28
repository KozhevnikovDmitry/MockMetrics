using System.Collections.Generic;
using Common.BL.Validation;
using Common.UI.ViewModel.ListViewModel;
using GU.DataModel;
using GU.HQ.BL;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.UI.ViewModel.ClaimViewModel
{
    public class ClaimStatusHistItemVM :  AbstractListItemVM<ClaimStatusHist>
    {
        public ClaimStatusHistItemVM(ClaimStatusHist entity, IDomainValidator<ClaimStatusHist> domainValidator, bool isValidateable) 
            : base(entity, domainValidator, isValidateable)
        {
            ClaimStatusList = HqFacade.GetDictionaryManager().GetEnumDictionary<ClaimStatusType>();
            DbUserList = HqFacade.GetDictionaryManager().GetDictionary<DbUser>();
        }

        protected override void Initialize()
        {
        }

        #region Common

        /// <summary>
        /// Список пользователей
        /// </summary>
        private List<DbUser> DbUserList { get; set; }

        /// <summary>
        /// Список статусов
        /// </summary>
        private Dictionary<int, string> ClaimStatusList;

        #endregion Common

        #region Binding Properties

        /// <summary>
        /// Статус заявления
        /// </summary>
        public string Status { get { return ClaimStatusList[Entity.ClaimStatusTypeId]; } }

        /// <summary>
        /// Время установки
        /// </summary>
        public string DateSetStatus
        {
            get { return Entity.Date.ToString("dd.MM.yyyy HH:mm:ss"); }
        }

        /// <summary>
        /// Пользователь кто установил статус
        /// </summary>
        public string UserName
        {
            get { return DbUserList.Find(t => t.Id == Entity.UUserId).UserText; }
        }

        #endregion Binding Properties
    }
}