using System;

using Common.BL.Validation;
using Common.UI.ViewModel.ListViewModel;

using GU.HQ.DataModel;

namespace GU.HQ.UI.ViewModel.ClaimViewModel
{
    public class NoticeItemVM : AbstractListItemVM<Notice>
    {
        /// <summary>
        /// Класс Уведомление
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="domainValidator"></param>
        /// <param name="isValidateable"></param>
        public NoticeItemVM(Notice entity, IDomainValidator<Notice> domainValidator, bool isValidateable) 
            : base(entity, domainValidator, isValidateable)
        {
            Expanded = string.IsNullOrEmpty(Entity.DocumentNumber);
        }

        protected override void Initialize()
        {  
        }

        /// <summary>
        /// Заголовок уведомления
        /// </summary>
        public string NoticeInfo
        {
            get
            {
                return Entity != null ? String.Format("№ {0}  от {1:dd.MM.yyyy}", Entity.DocumentNumber, (Entity.DocumentDate != DateTime.MinValue ? Entity.DocumentDate : DateTime.Now)  ) : "";
            }
        }

        /// <summary>
        /// Свернутым или развернутым отображать уведомление
        /// </summary>
        public bool Expanded { get; set; }

        /// <summary>
        /// Дата уведомления
        /// </summary>
        public DateTime DocumentDate
        {
            get 
            {  
                return Entity == null || Entity.DocumentDate == DateTime.MinValue ? DateTime.Now : Entity.DocumentDate; 
            }
            set
            {
                Entity.DocumentDate = value;
                RaisePropertyChanged(() => NoticeInfo);
            }
        }

        /// <summary>
        /// Номер уведомления
        /// </summary>
        public string DocumentNumber
        {
            get
            {
                return Entity == null ? "" : Entity.DocumentNumber;
            }
            set 
            { 
                Entity.DocumentNumber = value;
                RaisePropertyChanged(() => NoticeInfo);
            }
        }
    }
}
