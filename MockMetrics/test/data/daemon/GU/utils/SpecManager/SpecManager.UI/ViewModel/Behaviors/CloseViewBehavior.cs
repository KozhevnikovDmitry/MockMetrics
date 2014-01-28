using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

using AvalonDock.Layout;

namespace SpecManager.UI.ViewModel.Behaviors
{
    public class CloseViewBehavior
    {
        // Регистрируем вложенное свойство
        public static readonly DependencyProperty CloseViewProperty =
            DependencyProperty.RegisterAttached("CloseView", typeof(bool), typeof(CloseViewBehavior),
            new PropertyMetadata(false, OnCloseViewChanged));

        public static bool GetCloseView(UserControl obj)
        {
            return (bool)obj.GetValue(CloseViewProperty);
        }

        public static void SetCloseView(UserControl obj, bool value)
        {
            obj.SetValue(CloseViewProperty, value);
        }

        private static void OnCloseViewChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs args)
        {
            bool close = (bool)args.NewValue;
            Window win = dpo as Window;
            if (win == null)
                win = FindParentWindow(dpo);

            if (win != null)
            {
                if (close)
                {
                    try
                    {
                        win.Close();
                    }
                    catch (InvalidOperationException) //Костыль на случай попытки закрытия уже закрываемого в данный момент View
                    {

                    }
                }
            }
            else
            {
                LayoutDocument doc = FindParentDocument(dpo);
                if (doc != null)
                {
                    if (close)
                    {
                        doc.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает родительское окно для элемента.
        /// </summary>
        /// <param name="child">Зависимый элемент</param>
        /// <returns>Родительское окно</returns>
        public static Window FindParentWindow(DependencyObject child)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            if (parent == null)
                return null;

            Window parentWindow = parent as Window;
            if (parentWindow != null)
            {
                return parentWindow;
            }
            else
            {
                return FindParentWindow(parent);
            }
        }

        /// <summary>
        /// Возвращает родительский <c>LayoutDocument</c> AvalonDock'a для элемента.
        /// </summary>
        /// <param name="child">Зависимый элемент</param>
        /// <returns>Родительский <c>LayoutDocument</c></returns>        
        public static LayoutDocument FindParentDocument(DependencyObject child)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            if (parent == null)
                return null;

            LayoutDocument parentDocument = parent as LayoutDocument;
            if (parentDocument != null)
            {
                return parentDocument;
            }
            else
            {
                return FindParentDocument(parent);
            }
        }
    }
}
