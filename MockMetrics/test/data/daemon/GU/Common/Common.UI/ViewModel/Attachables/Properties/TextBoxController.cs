using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Common.UI.ViewModel.Attachables.Properties
{
    public static class TextBoxAttach
    {
        public static readonly DependencyProperty ControllerProperty = DependencyProperty.RegisterAttached(
            "Controller", typeof(ITextBoxController), typeof(TextBoxAttach),
            new FrameworkPropertyMetadata(null, OnControllerChanged));

        public static readonly DependencyProperty KeyProperty = DependencyProperty.RegisterAttached(
            "Key", typeof(string), typeof(TextBoxAttach),
            new FrameworkPropertyMetadata(null, OnKeyChanged));

        public static void SetController(UIElement element, ITextBoxController value)
        {
            element.SetValue(ControllerProperty, value);
        }

        public static ITextBoxController GetController(UIElement element)
        {
            return (ITextBoxController)element.GetValue(ControllerProperty);
        }

        public static void SetKey(UIElement element, string value)
        {
            element.SetValue(KeyProperty, value);
        }

        public static string GetKey(UIElement element)
        {
            return (string)element.GetValue(KeyProperty);
        }

        private static readonly Dictionary<DependencyObject, ITextBoxController> viewVMs = new Dictionary<DependencyObject, ITextBoxController>();

        private static readonly Dictionary<DependencyObject, Dictionary<string, Control>> registry = new Dictionary<DependencyObject, Dictionary<string, Control>>();

        private static void OnControllerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as Control;
            if (!(element is TextBox) && !(element is PasswordBox))
                throw new ArgumentNullException("TextBoxAttach: DependencyObject host is not a TextBox or PasswordBox");

            var dob = FindParentUserControl(d);

            var oldController = e.OldValue as ITextBoxController;
            if (oldController != null)
            {
                viewVMs[dob] = null;
                oldController.SelectText -= SelectText;
            }

            var newController = e.NewValue as ITextBoxController;
            if (newController != null && viewVMs[dob] != newController)
            {
                viewVMs[dob] = newController;
                newController.SelectText += SelectText;
            }
        }

        private static void OnKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as Control;
            
            if (!(element is TextBox) && !(element is PasswordBox))
                throw new ArgumentNullException("TextBoxAttach: DependencyObject host is not a TextBox or PasswordBox");

            var dob = FindParentUserControl(d);

            if (!viewVMs.Keys.Contains(dob))
            {
                viewVMs[dob] = null;
            }

            if (!registry.Keys.Contains(dob))
            {
                registry[dob] = new Dictionary<string, Control>();
            }

            var oldKey = e.OldValue as string;
            if (!string.IsNullOrEmpty(oldKey))
            {
                registry[dob].Remove(oldKey);
            }

            var newKey = e.NewValue as string;
            if (!string.IsNullOrEmpty(newKey))
            {
                registry[dob].Add(newKey, element);
            }
        }

        private static void SelectText(ITextBoxController sender, string key)
        {
            Dictionary<string, Control> elements;
            Control element;
            var dob = (from v in viewVMs
                       where v.Value == sender
                       select v.Key).First();
            registry.TryGetValue(dob, out elements);

            if (elements == null)
            {
                throw new KeyNotFoundException("TextBoxAttach: Controller not found");
            }
            
            elements.TryGetValue(key, out element);
            if (element == null)
            {
                throw new KeyNotFoundException("TextBoxAttach: Element not found");
            }

            element.Focus();
            if (element is TextBox)
            {
                (element as TextBox).SelectAll();
            }
            else
            {
                (element as PasswordBox).SelectAll();
            }
        }

        public static UserControl FindParentUserControl(DependencyObject child)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            if (parent == null)
            {
                parent = (child as FrameworkElement).Parent;
                if (parent == null)
                    return null;
            }

            UserControl parentUserControl = parent as UserControl;
            if (parentUserControl != null)
            {
                return parentUserControl;
            }
            else
            {
                return FindParentUserControl(parent);
            }
        }
    }

    public interface ITextBoxController
    {
        event SelectTextEventHandler SelectText;
    }

    public delegate void SelectTextEventHandler(ITextBoxController sender, string key);
}
