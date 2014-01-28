using System.Windows.Interactivity;

using AvalonDock;
using AvalonDock.Layout;

using Common.Types.Exceptions;
using Common.UI.ViewModel.Interfaces;

namespace Common.UI.ViewModel.Attachables.Behaviors
{
    public class DockingManagerClosingBehavior : Behavior<DockingManager>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.DocumentClosing += this.OnDocumentClosing;
            AssociatedObject.DocumentClosed += this.OnDocumentClosed;
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.DocumentClosing -= this.OnDocumentClosing;
            this.AssociatedObject.DocumentClosed -= this.OnDocumentClosed;
        }

        private IAvalonDockVM DataContext
        {
            get
            {
                if (AssociatedObject.DataContext is IAvalonDockVM)
                {
                    return AssociatedObject.DataContext as IAvalonDockVM;
                }

                throw new VMException("ViewModel для DockingManager не указан или имеет неверный тип");
            }
        }

        private IDockableVM GetDocumentContext(LayoutDocument document)
        {
            if (document.Content is IDockableVM)
            {
                return document.Content as IDockableVM;
            }

            throw new VMException("ViewModel для LayoutDocument не указан или имеет неверный тип");
        }

        void OnDocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            var dockableVm = GetDocumentContext(e.Document);
            DataContext.Workspaces.Remove(dockableVm);
        }

        void OnDocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            GetDocumentContext(e.Document).RaiseOnClosing(e);
        }
    }
}
