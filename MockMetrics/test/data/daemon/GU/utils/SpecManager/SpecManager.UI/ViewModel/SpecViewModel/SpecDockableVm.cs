using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;

using SpecManager.BL.Interface;
using SpecManager.BL.Model;
using SpecManager.BL.SpecSource;
using SpecManager.UI.View;
using SpecManager.UI.View.SpecSourceView;
using SpecManager.UI.ViewModel.SpecSourceViewModel;

namespace SpecManager.UI.ViewModel.SpecViewModel
{
    public class SpecDockableVm : NotificationObject
    {
        private Spec _spec;

        private readonly SpecTreeClipboard _specTreeClipboard;

        private ISpecSource _specSource;

        private ISpecSource _preservedSpecSource;

        private readonly ISpecViewFactory _specViewFactory;

        public SpecDockableVm(Spec spec, SpecTreeClipboard specTreeClipboard, ISpecSource specSource, ISpecViewFactory specViewFactory)
        {
            this.CurrentNodeView = null;
            this.CanAddNodeAfter = false;
            this.CanAddChildNode = false;
            this.CanShiftUpNode = false;
            this.CanShiftDownNode = false;
            this.CanRemoveNode = false;

            this._spec = spec;
            this._specTreeClipboard = specTreeClipboard;
            _specSource = specSource;
            _specViewFactory = specViewFactory;
            this.SelectTreeNodeCommand = new DelegateCommand<object>(this.SelectTreeNode);
            this.AddNodeAfterCommand = new DelegateCommand(this.AddNodeAfter, () => this.CanAddNodeAfter);
            this.AddSimpleChildNodeCommand = new DelegateCommand(this.AddSimpleChildNode, () => this.CanAddChildNode);
            this.AddComplexChildNodeCommand = new DelegateCommand(this.AddComplexChildNode, () => this.CanAddChildNode);
            this.AddComplexChoiseChildNodeCommand = new DelegateCommand(this.AddComplexChoiseChildNode, () => this.CanAddChildNode);
            this.AddRefSpecChildNodeCommand = new DelegateCommand(this.AddRefSpecChildNode, () => this.CanAddChildNode);
            this.CopySpecNodeCommand = new DelegateCommand<ISpecTreeNodeVm>(this.CopySpecNode);
            this.CutSpecNodeCommand = new DelegateCommand<ISpecTreeNodeVm>(this.CutSpecNode);
            this.PasteSpecNodeCommand = new DelegateCommand<ISpecTreeNodeVm>(this.PasteSpecNode);
            this.ShiftUpNodeCommand = new DelegateCommand(this.ShiftUpNode, () => this.CanShiftUpNode);
            this.ShiftDownNodeCommand = new DelegateCommand(this.ShiftDownNode, () => this.CanShiftDownNode);
            this.RemoveNodeCommand = new DelegateCommand(this.RemoveNode, () => this.CanRemoveNode);
            this.TreeNodeRootVm = new ObservableCollection<ISpecTreeNodeVm> { new SpecTreeNodeVm(this._spec) };
            TreeNodeRootVm.CollectionChanged += this.NodeCollectionChanged;
            SelectTreeNode(null);
        }

        public void Save()
        {
            try
            {
                var warning = _specSource.PreSave(_spec);

                if (warning.HasWarnings)
                {
                    var warningVm = new PreSaveWarningsVm(warning);
                    var warningView = new PreSaveWarningsView { DataContext = warningVm };
                    var dialogVm = new DialogVm { View = warningView, DisplayName = warningVm.Title };
                    var dialogView = new DialogView { DataContext = dialogVm, Owner = Application.Current.MainWindow };
                    dialogView.ShowDialog();
                    if (!dialogVm.IsOkResult)
                    {
                        return;
                    }
                }

                _spec = _specSource.Save(_spec);
                foreach (var specTreeNodeVm in TreeNodeRootVm)
                {
                    specTreeNodeVm.UnsubscribeCollectionChanged();
                }

                this.TreeNodeRootVm = new ObservableCollection<ISpecTreeNodeVm> { new SpecTreeNodeVm(this._spec) };
                TreeNodeRootVm.CollectionChanged += this.NodeCollectionChanged;
                this.RaisePropertyChanged(() => SourceName);
                this.OnNameChanged();
                _preservedSpecSource = null;
            }
            catch (GUException ex)
            {
                RestorePreservedSource();
                NoticeUser.ShowError(ex);
            }
            catch (Exception ex)
            {
                RestorePreservedSource();
                NoticeUser.ShowError(new Exception("Непредвиденная ошибка", ex));
            }
        }

        private void RestorePreservedSource()
        {
            if (_preservedSpecSource != null)
            {
                _specSource = _preservedSpecSource;
                _preservedSpecSource = null;
            }
        }

        public void SetSource(ISpecSource specSource)
        {
            _preservedSpecSource = _specSource;
            _specSource = specSource;
        }

        private ISpecTreeNodeVm SelectedTreeNodeVm { get; set; }

        public string Name
        {
            get
            {
                return _spec.Name;
            }
        }

        public string SourceName
        {
            get
            {
                return _specSource.Name;
            }
        }

        #region Events

        public event Action<string> NameChanged;

        public void OnNameChanged()
        {
            Action<string> handler = this.NameChanged;
            if (handler != null)
            {
                string name = string.Format("{0} [{1}]", this._spec.Uri, SourceName.Split('=').First());
                handler(name);
            }
        }

        public event Action<string> HintChanged;

        public void OnHintChanged()
        {
            Action<string> handler = this.HintChanged;
            if (handler != null)
            {
                handler(Name);
            }
        }

        #endregion

        #region Event Handling

        void NodeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.RaisePropertyChanged(() => TreeNodeRootVm);
        }

        #endregion

        #region Binding Properties

        private ObservableCollection<ISpecTreeNodeVm> _treeNodeRootVm;

        public ObservableCollection<ISpecTreeNodeVm> TreeNodeRootVm
        {
            get
            {
                return this._treeNodeRootVm;
            }
            set
            {
                if (this._treeNodeRootVm != value)
                {
                    this._treeNodeRootVm = value;
                    this.RaisePropertyChanged(() => this.TreeNodeRootVm);
                }
            }
        }

        private UserControl _currentNodeView;

        public UserControl CurrentNodeView
        {
            get
            {
                return this._currentNodeView;
            }
            set
            {
                if (!Equals(this._currentNodeView, value))
                {
                    this._currentNodeView = value;
                    this.RaisePropertyChanged(() => this.CurrentNodeView);
                }
            }
        }

        #endregion

        #region Binding Commands

        #region SelectTreeNode

        public DelegateCommand<object> SelectTreeNodeCommand { get; private set; }

        private void SelectTreeNode(object obj)
        {
            try
            {
                if (obj is ISpecTreeNodeVm)
                {
                    var selectedTreeNodeVm = obj as ISpecTreeNodeVm;
                    
                    SelectedTreeNodeVm = selectedTreeNodeVm;
                    this.SetCurrentNodeView(selectedTreeNodeVm);
                    this.SetNodeCommandCanExecute(selectedTreeNodeVm);
                }
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

        private void SetCurrentNodeView(ISpecTreeNodeVm specTreeNodeVm)
        {
            var specVm = _specViewFactory.GetSpecVm(specTreeNodeVm.Item);
            specVm.NameChanged += s =>
                {
                    specTreeNodeVm.RaiseSpecNodeDataChanged();
                    OnNameChanged();
                    OnHintChanged();
                };

            specVm.MinOccursChanged += x => specTreeNodeVm.RaiseSpecNodeDataChanged();
            specVm.MaxOccursChanged += x => specTreeNodeVm.RaiseSpecNodeDataChanged();
            specVm.SpecNodeTypeChanged += x => specTreeNodeVm.RaiseSpecNodeDataChanged();
            specVm.AttrDataTypeChanged += x => specTreeNodeVm.RaiseSpecNodeDataChanged();

            var specView = _specViewFactory.GetSpecView(specTreeNodeVm.Item);
            specView.DataContext = specVm;
            CurrentNodeView = specView;
        }

        private void SetNodeCommandCanExecute(ISpecTreeNodeVm specTreeNodeVm)
        {
            if (specTreeNodeVm.SpecNodeType.HasValue)
            {
                if (specTreeNodeVm.SpecNodeType.Value == SpecNodeType.Complex
                    || specTreeNodeVm.SpecNodeType.Value == SpecNodeType.ComplexChoice)
                {
                    CanAddChildNode = true;
                }
                else
                {
                    CanAddChildNode = false;
                }
                CanAddNodeAfter = true;
                CanRemoveNode = true;
                CanShiftDownNode = true;
                CanShiftUpNode = true;
            }
            else
            {
                CanAddChildNode = true;
                CanAddNodeAfter = false;
                CanRemoveNode = false;
                CanShiftDownNode = false;
                CanShiftUpNode = false;
            }
        }

        #endregion

        #region CopyPaste

        public DelegateCommand<ISpecTreeNodeVm> CopySpecNodeCommand { get; private set; }

        private void CopySpecNode(ISpecTreeNodeVm specTreeNodeVm)
        {
            try
            {
                this._specTreeClipboard.Copy(specTreeNodeVm);
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

        public DelegateCommand<ISpecTreeNodeVm> CutSpecNodeCommand { get; private set; }

        private void CutSpecNode(ISpecTreeNodeVm specTreeNodeVm)
        {
            try
            {
                this._specTreeClipboard.Cut(specTreeNodeVm);
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

        public DelegateCommand<ISpecTreeNodeVm> PasteSpecNodeCommand { get; private set; }

        private void PasteSpecNode(ISpecTreeNodeVm specTreeNodeVm)
        {
            try
            {
                this._specTreeClipboard.Paste(specTreeNodeVm);
                this.RaisePropertyChanged(() => this.TreeNodeRootVm);
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

        #endregion

        #region AddAfter

        public DelegateCommand AddNodeAfterCommand { get; private set; }

        private void AddNodeAfter()
        {
            try
            {
                if (SelectedTreeNodeVm is SpecNodeTreeNodeVm)
                {
                    (SelectedTreeNodeVm as SpecNodeTreeNodeVm).AddAfter();
                }
                else
                {
                    throw new ArgumentException("specTreeNodeVm");
                }
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

        private bool _canAddNodeAfter;

        public bool CanAddNodeAfter
        {
            get
            {
                return this._canAddNodeAfter;
            }
            set
            {
                if (this._canAddNodeAfter != value)
                {
                    this._canAddNodeAfter = value;
                    this.AddNodeAfterCommand.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion

        #region AddChild

        public DelegateCommand AddSimpleChildNodeCommand { get; private set; }

        private void AddSimpleChildNode()
        {
            try
            {
                SelectedTreeNodeVm.AddChild(SpecNodeType.Simple);
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

        public DelegateCommand AddComplexChildNodeCommand { get; private set; }

        private void AddComplexChildNode()
        {
            try
            {
                SelectedTreeNodeVm.AddChild(SpecNodeType.Complex);
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

        public DelegateCommand AddComplexChoiseChildNodeCommand { get; private set; }

        private void AddComplexChoiseChildNode()
        {
            try
            {
                SelectedTreeNodeVm.AddChild(SpecNodeType.ComplexChoice);
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

        public DelegateCommand AddRefSpecChildNodeCommand { get; private set; }

        private void AddRefSpecChildNode()
        {
            try
            {
                SelectedTreeNodeVm.AddChild(SpecNodeType.RefSpec);
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

        private bool _canAddChildNode;

        public bool CanAddChildNode
        {
            get
            {
                return this._canAddChildNode;
            }
            set
            {
                if (this._canAddChildNode != value)
                {
                    this._canAddChildNode = value;
                    this.AddSimpleChildNodeCommand.RaiseCanExecuteChanged();
                    this.AddComplexChildNodeCommand.RaiseCanExecuteChanged();
                    this.AddComplexChoiseChildNodeCommand.RaiseCanExecuteChanged();
                    this.AddRefSpecChildNodeCommand.RaiseCanExecuteChanged();
                    this.RaisePropertyChanged(() => CanAddChildNode);
                }
            }
        }

        #endregion

        #region Shift

        public DelegateCommand ShiftUpNodeCommand { get; private set; }

        private void ShiftUpNode()
        {
            try
            {
                if (SelectedTreeNodeVm is SpecNodeTreeNodeVm)
                {
                    (SelectedTreeNodeVm as SpecNodeTreeNodeVm).ShiftUp();
                }
                else
                {
                    throw new ArgumentException("specTreeNodeVm");
                }
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

        private bool _canShiftUpNode;

        public bool CanShiftUpNode
        {
            get
            {
                return this._canShiftUpNode;
            }
            set
            {
                if (this._canShiftUpNode != value)
                {
                    this._canShiftUpNode = value;
                    this.ShiftUpNodeCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public DelegateCommand ShiftDownNodeCommand { get; private set; }

        private void ShiftDownNode()
        {
            try
            {
                if (SelectedTreeNodeVm is SpecNodeTreeNodeVm)
                {
                    (SelectedTreeNodeVm as SpecNodeTreeNodeVm).ShiftDown();
                }
                else
                {
                    throw new ArgumentException("specTreeNodeVm");
                }
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

        private bool _canShiftDownNode;

        public bool CanShiftDownNode
        {
            get
            {
                return this._canShiftDownNode;
            }
            set
            {
                if (this._canShiftDownNode != value)
                {
                    this._canShiftDownNode = value;
                    this.ShiftDownNodeCommand.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion

        #region Remove

        public DelegateCommand RemoveNodeCommand { get; private set; }

        private void RemoveNode()
        {
            try
            {
                if (SelectedTreeNodeVm is SpecNodeTreeNodeVm)
                {
                    (SelectedTreeNodeVm as SpecNodeTreeNodeVm).Remove();
                }
                else
                {
                    throw new ArgumentException("specTreeNodeVm");
                }
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

        private bool _canRemoveNode;

        public bool CanRemoveNode
        {
            get
            {
                return this._canRemoveNode;
            }
            set
            {
                if (this._canRemoveNode != value)
                {
                    this._canRemoveNode = value;
                    this.RemoveNodeCommand.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion

        #endregion
    }
}
