using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Common.BL.Validation;
using Common.DA;
using Common.UI.ViewModel.ValidationViewModel;
using GU.BL;
using GU.BL.Policy;
using GU.DataModel;

namespace GU.UI.ViewModel.UserViewModel
{
    public class UserDataVM : DomainValidateableVM<DbUser>
    {
        public UserDataVM(DbUser entity,
                          IDomainValidator<DbUser> domainValidator,
                          bool isValidateable = true)
            : base(entity, domainValidator, isValidateable)
        {
            var userAgency = GuFacade.GetDbUser().Agency;
            var agencyList = AgencyPolicy.GetActualAgencies(userAgency).OrderBy(t => t.Name).ToList();
            // TODO: группировка несколько хитрее, потом как-нибудь
            AgencyList = new ListCollectionView(agencyList);
        }

        #region Binding Properties

        public bool IsOldUser
        {
            get
            {
                return Entity.PersistentState == PersistentState.Old;
            }
        }

        /// <summary>
        /// login
        /// </summary>
        public string Name
        {
            get
            {
                return Entity.Name;
            }
            set
            {
                if (Entity.Name != value)
                {
                    Entity.Name = value.ToUpper();
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        /// <summary>
        /// ФИО
        /// </summary>
        public string UserText
        {
            get
            {
                return Entity.UserText;
            }
            set
            {
                if (Entity.UserText != value)
                {
                    Entity.UserText = value;
                    RaisePropertyChanged(() => UserText);
                }
            }
        }

        /// <summary>
        /// AppointText
        /// </summary>
        public string AppointText
        {
            get
            {
                return Entity.AppointText;
            }
            set
            {
                if (Entity.AppointText != value)
                {
                    Entity.AppointText = value;
                    RaisePropertyChanged(() => AppointText);
                }
            }
        }

        public int? AgencyId
        {
            get
            {
                return Entity.AgencyId;
            }
            set
            {
                if (Entity.AgencyId != value)
                {
                    Entity.AgencyId = value;
                    RaisePropertyChanged(() => AgencyId);
                }
            }
        }

        public ListCollectionView AgencyList { get; set; }

        #endregion

        #region Binding Commands

        #endregion

    }
}
