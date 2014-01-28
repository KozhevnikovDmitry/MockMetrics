using System;
using System.Linq;

using Common.BL.Validation;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel.ValidationViewModel;

using GU.BL.Policy;
using GU.DataModel;

using Microsoft.Practices.Prism.Commands;

namespace GU.UI.ViewModel.ContentViewModel
{
    public class ContentNodeVM : DomainValidateableVM<ContentNode>
    {
        protected readonly ContentNodeVmBuilder _contentNodeVmBuilder;

        public ContentNodeVM(ContentNode entity, IDomainValidator<ContentNode> domainValidator, ContentNodeVmBuilder contentNodeVmBuilder, bool isValidateable = true)
            : base(entity, domainValidator, isValidateable)
        {
            _contentNodeVmBuilder = contentNodeVmBuilder;
            this.AddCopyCommand = new DelegateCommand(this.AddCopy, () => this.CanAddCopy);
            this.DeleteCommand = new DelegateCommand(this.Delete, () => this.CanDelete);
        }

        public virtual ComplexContentNodeVM ParentContentNodeVm { get; set; }

        public bool AllowSinglePropertyValidate
        {
            get
            {
                return this._domainValidator.AllowSinglePropertyValidate;
            }
            set
            {
                this._domainValidator.AllowSinglePropertyValidate = value;
            }
        }

        #region Binding Properties

        public int DeepLevel
        {
            get
            {
                if (ParentContentNodeVm == null)
                {
                    return 0;
                }

                int level = ParentContentNodeVm.DeepLevel + 1;

                if (this is ComplexChoiseContentNodeVM)
                {
                    level++;
                }

                return level;
            }
        }

        public virtual string Name
        {
            get
            {
                return string.Format("{0} {1}", Entity.SpecNode.Name, Entity.NodeNum);
            }
        }

        public virtual int? SortOrder
        {
            get
            {
                return this.Entity.SpecNode.Order;
            }

        }

        public virtual SpecNodeType? SpecNodeType
        {
            get
            {
                return this.Entity.SpecNode.SpecNodeType;
            }
        }

        #endregion

        #region Binding Commands

        public DelegateCommand AddCopyCommand { get; private set; }

        private void AddCopy()
        {
            try
            {
                var contentPolicy = new ContentPolicy();
                var copyNode = contentPolicy.AddChildNode(Entity.ParentContentNode, Entity.SpecNode);
                var copyNodeVm = _contentNodeVmBuilder.For(copyNode).Build(IsEditable);
                copyNodeVm.ParentContentNodeVm = this.ParentContentNodeVm;

                var lastSimilarNode = this.ParentContentNodeVm
                                          .ContentNodeVmList
                                          .Where(t => t.Entity.SpecNode == this.Entity.SpecNode)
                                          .OrderBy(t => t.SortOrder)
                                          .Last();

                var lastSimilarIndex = this.ParentContentNodeVm
                                           .ContentNodeVmList
                                           .IndexOf(lastSimilarNode);

                this.ParentContentNodeVm.ContentNodeVmList.Insert(lastSimilarIndex + 1, copyNodeVm);

                ParentContentNodeVm.ContentNodeVmList.ToList().ForEach(t => t.RaiseOnAddDeletePropertiesChanged());
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        public virtual bool CanAddCopy
        {
            get
            {
                try
                {
                    return IsEditable
                           &&
                           this.Entity.ParentContentNode != null
                           &&
                           (this.Entity.SpecNode.MaxOccurs
                           >
                           this.Entity.ContainedNodeList.Count(t => t.SpecNodeId == this.Entity.SpecNodeId));
                }
                catch (GUException ex)
                {
                    NoticeUser.ShowError(ex);
                    return false;
                }
                catch (Exception ex)
                {
                    NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
                    return false;
                }
            }
        }

        public DelegateCommand DeleteCommand { get; private set; }

        private void Delete()
        {
            try
            {
                this.CheckForCreatePotentional();
                var contentPolicy = new ContentPolicy();
                contentPolicy.RemoveNode(Entity);
                this.ParentContentNodeVm.ContentNodeVmList.Remove(this);
                ParentContentNodeVm.ContentNodeVmList.ToList().ForEach(t => t.RaiseOnAddDeletePropertiesChanged());
            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        private void CheckForCreatePotentional()
        {
            if (this.Entity.ContainedNodeList.Count(t => t.SpecNodeId == this.Entity.SpecNodeId) == 1)
            {
                var index = this.ParentContentNodeVm.ContentNodeVmList.IndexOf(this);
                var newPotentionalNodeVm = new PotentionalContentNodeVM(this.Entity.SpecNode, ParentContentNodeVm, _contentNodeVmBuilder) { IsEditable = IsEditable };
                ParentContentNodeVm.ContentNodeVmList.Insert(index, newPotentionalNodeVm);
            }
        }

        public virtual bool CanDelete
        {
            get
            {
                try
                {
                    return IsEditable
                           &&
                           this.Entity.ParentContentNode != null
                           &&
                           CanDeleteSpecific()
                           &&
                           (this.Entity.SpecNode.MinOccurs
                           <
                           this.Entity.ContainedNodeList.Count(t => t.SpecNodeId == this.Entity.SpecNodeId));
                }
                catch (GUException ex)
                {
                    NoticeUser.ShowError(ex);
                    return false;
                }
                catch (Exception ex)
                {
                    NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
                    return false;
                }
            }
        }

        protected virtual bool CanDeleteSpecific()
        {
            return true;
        }

        private void RaiseOnAddDeletePropertiesChanged()
        {
            AddCopyCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(() => CanAddCopy);
            RaisePropertyChanged(() => CanDelete);
            this.RaisePropertyChanged(() => Name);
        }

        #endregion
    }
}