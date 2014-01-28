using System;

using Common.BL.Validation;
using Common.Types.Exceptions;
using Common.UI;

using GU.BL.Policy;
using GU.DataModel;

using Microsoft.Practices.Prism.Commands;

namespace GU.UI.ViewModel.ContentViewModel
{
    public class PotentionalContentNodeVM : ContentNodeVM
    {
        private readonly SpecNode _specNode;

        private readonly ComplexContentNodeVM _parentContentNodeVm;
        
        public PotentionalContentNodeVM(SpecNode specNode, ComplexContentNodeVM parentContentNodeVm, ContentNodeVmBuilder contentNodeVmBuilder)
            : base(null, new StubDomainValidatior<ContentNode>(),contentNodeVmBuilder, false)
        {
            this._specNode = specNode;
            this._parentContentNodeVm = parentContentNodeVm;
            AddContentCommand = new DelegateCommand(AddContent, () => IsEditable);
        }

        #region Binding Properties
        
        public override string Name
        {
            get
            {
                return this._specNode.Name;
            }
        }

        public override int? SortOrder
        {
            get
            {
                return this._specNode.Order;
            }
        }

        public override SpecNodeType? SpecNodeType
        {
            get
            {
                return null;
            }
        }

        #endregion

        #region Binding Commands

        public DelegateCommand AddContentCommand { get; private set; }

        private void AddContent()
        {
            try
            {
                var contentPolicy = new ContentPolicy();
                var childNode = contentPolicy.AddChildNode(_parentContentNodeVm.Entity, _specNode);
                var nodeVm = _contentNodeVmBuilder.For(childNode).Build(IsEditable);
                nodeVm.ParentContentNodeVm = this._parentContentNodeVm;
                var index = this._parentContentNodeVm.ContentNodeVmList.IndexOf(this);
                this._parentContentNodeVm.ContentNodeVmList.Insert(index, nodeVm);
                this.AddContentCommand.RaiseCanExecuteChanged();
                this._parentContentNodeVm.ContentNodeVmList.Remove(this);
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

        public override bool CanAddCopy
        {
            get
            {
                return false;
            }
        }

        public override bool CanDelete
        {
            get
            {
                return false;
            }
        }

        #endregion

    }
}