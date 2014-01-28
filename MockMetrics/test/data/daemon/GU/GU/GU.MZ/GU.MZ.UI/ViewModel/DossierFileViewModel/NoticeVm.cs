using System;
using Common.Types;
using GU.MZ.DataModel.Notifying;
using GU.MZ.UI.ViewModel.SmartViewModel;

namespace GU.MZ.UI.ViewModel.DossierFileViewModel
{
    /// <summary>
    /// Класс ViewModel для отображения данных сущности уведомление
    /// </summary>
    public class NoticeVm : SmartValidateableVm<Notice>
    {
        public override void Initialize(Notice entity)
        {
            base.Initialize(entity);
            NoticeInfo = Entity.NoticeType.GetDescription();
            RaisePropertyChanged(() => NoticeInfo);
        }

        #region Binding Properties

        /// <summary>
        /// Дата отправления уведомления
        /// </summary>
        public DateTime Stamp
        {
            get
            {
                return Entity.Stamp;
            }
            set
            {
                if (Entity.Stamp != value)
                {
                    Entity.Stamp = value;
                    RaisePropertyChanged(() => Stamp);
                }
            }
        }

        /// <summary>
        /// Email
        /// </summary>
        public string Email
        {
            get
            {
                return Entity.Email;
            }
            set
            {
                if (Entity.Email != value)
                {
                    Entity.Email = value;
                    RaisePropertyChanged(() => Email);
                }
            }
        }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address
        {
            get
            {
                return Entity.Address;
            }
            set
            {
                if (Entity.Address != value)
                {
                    Entity.Address = value;
                    RaisePropertyChanged(() => Address);
                }
            }
        }

        /// <summary>
        /// Тело уведомления
        /// </summary>
        public string Body
        {
            get
            {
                return Entity.Body;
            }
            set
            {
                if (Entity.Body != value)
                {
                    Entity.Body = value;
                    RaisePropertyChanged(() => Body);
                }
            }
        }

        /// <summary>
        /// Реквизиты почтового отправления
        /// </summary>
        public string PostRequisites
        {
            get
            {
                return Entity.PostRequisites;
            }
            set
            {
                if (Entity.PostRequisites != value)
                {
                    Entity.PostRequisites = value;
                    RaisePropertyChanged(() => PostRequisites);
                }
            }
        }


        public string NoticeInfo { get; set; }

        #endregion

        public override void RaiseAllPropertyChanged()
        {
            base.RaiseAllPropertyChanged();
            RaisePropertyChanged(() => PostRequisites);
            RaisePropertyChanged(() => Body);
            RaisePropertyChanged(() => Address);
            RaisePropertyChanged(() => Email);
        }
    }
}
