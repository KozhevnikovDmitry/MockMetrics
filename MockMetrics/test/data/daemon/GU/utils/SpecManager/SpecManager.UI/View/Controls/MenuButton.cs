using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace SpecManager.UI.View.Controls
{
    /// <summary>
    /// Кастомный контрол - кнопка с выпадающим меню.
    /// </summary>
    public class MenuButton : ToggleButton
    {
        static MenuButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuButton), new FrameworkPropertyMetadata(typeof(MenuButton)));

            ContextMenuProperty.OverrideMetadata(typeof(MenuButton),
                new FrameworkPropertyMetadata(null,
                    (d, e) =>
                    {
                        if (e.NewValue == null)
                            return;

                        var self = (MenuButton)d;
                        var contextMenu = (ContextMenu)e.NewValue;

                        self.SetBinding(MenuButton.IsCheckedProperty,
                            new Binding("ContextMenu.IsOpen") { RelativeSource = RelativeSource.Self, Mode = BindingMode.TwoWay });

                        contextMenu.PlacementTarget = ContextMenuService.GetPlacementTarget(self);
                        contextMenu.Placement = ContextMenuService.GetPlacement(self);
                    }));
        }

        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            if (this.ContextMenu != null)
                this.ContextMenu.IsOpen = false;
            e.Handled = true;
        }
    }
}
