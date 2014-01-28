using System;
using System.Collections.Generic;
using System.Linq;

using Common.BL.Validation;
using Common.Types.Exceptions;
using Common.UI;

using GU.BL.Policy;
using GU.DataModel;

namespace GU.UI.ViewModel.ContentViewModel
{
    public class ComplexChoiseContentNodeVM : ContentNodeVM
    {

        public ComplexChoiseContentNodeVM(ContentNode entity, IDomainValidator<ContentNode> domainValidator, ContentNodeVmBuilder contentNodeVmBuilder, bool isValidateable = true)
            : base(entity, domainValidator, contentNodeVmBuilder, isValidateable)
        {
            this._choiseSpecNodeId = this.Entity.ChildContentNodes.Single().SpecNode.Id;
        }

        public override ComplexContentNodeVM ParentContentNodeVm
        {
            get
            {
                return this._parentContentNodeVm;
            }
            set
            {
                this._parentContentNodeVm = value;
                ChoiseContentNodeVm.ParentContentNodeVm = value;
            }
        }

        private int _choiseSpecNodeId;

        public int ChoiseSpecNodeId
        {
            get
            {
                return this._choiseSpecNodeId;
            }
            set
            {
                if (this._choiseSpecNodeId != value)
                {
                    this._choiseSpecNodeId = value;
                    this.ChangeContent();
                    this.RaisePropertyChanged(() => this.ChoiseSpecNodeId);
                }
            }
        }

        private void ChangeContent()
        {
            try
            {
                var specNode = this.Entity.SpecNode
                                   .ChildSpecNodes
                                   .Single(t => t.Id == this.ChoiseSpecNodeId);

                var contentPolicy = new ContentPolicy();
                contentPolicy.SwitchChoice(this.Entity, specNode);
                var vmBuilder = _contentNodeVmBuilder.For(this.Entity.ChildContentNodes.Single());
                this.ChoiseContentNodeVm = vmBuilder.Build(IsEditable);
                this.ChoiseContentNodeVm.ParentContentNodeVm = ParentContentNodeVm;

            }
            catch (GUException ex)
            {
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                NoticeUser.ShowError(new GUException("Непредвиденная ошибка", ex));
            }
        }

        public List<SpecNode> ChoiseSpecNodeList
        {
            get
            {
                return this.Entity.SpecNode.ChildSpecNodes;
            }
        }

        private ContentNodeVM _choiseContentNodeVm;

        private ComplexContentNodeVM _parentContentNodeVm;

        public ContentNodeVM ChoiseContentNodeVm
        {
            get
            {
                return this._choiseContentNodeVm;
            }
            set
            {
                if (this._choiseContentNodeVm != value)
                {
                    this._choiseContentNodeVm = value;
                    this.RaisePropertyChanged(() => this.ChoiseContentNodeVm);
                }
            }
        }

        public override void RaiseValidatingPropertyChanged()
        {
            ChoiseContentNodeVm.RaiseValidatingPropertyChanged();
        }
    }
}