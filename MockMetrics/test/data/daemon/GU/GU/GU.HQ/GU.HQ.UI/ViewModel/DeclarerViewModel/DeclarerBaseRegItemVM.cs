using System.Collections.Generic;
using Common.BL.Validation;
using Common.UI.ViewModel.ListViewModel;
using GU.HQ.BL;
using GU.HQ.DataModel;
using GU.HQ.DataModel.Types;

namespace GU.HQ.UI.ViewModel.DeclarerViewModel
{
    public class DeclarerBaseRegItemVM : AbstractListItemVM<DeclarerBaseRegItem>
    {
        public DeclarerBaseRegItemVM(DeclarerBaseRegItem entity, IDomainValidator<DeclarerBaseRegItem> domainValidator, bool isValidateable) 
            : base(entity, domainValidator, isValidateable)
        {
            DeclarerBaseRegList = HqFacade.GetDictionaryManager().GetDictionary<QueueBaseRegType>();
        }

        protected override void Initialize()
        {
        }

        /// <summary>
        /// Список оснований
        /// </summary>
        public List<QueueBaseRegType> DeclarerBaseRegList { get; private set; }

        /// <summary>
        /// ID основания указанного заявителем
        /// </summary>
        public int QueueBaseRegTypeId
        {
            get { return Entity == null ? 0 : Entity.QueueBaseRegTypeId; }
            set { Entity.QueueBaseRegTypeId = value; }
        }
    }
}
