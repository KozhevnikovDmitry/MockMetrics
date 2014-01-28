using System.Windows.Interactivity;
using System.Windows;

namespace Common.UI.ViewModel.Attachables.Properties
{
    public class StylizedTriggers
    {
        #region Fields (public)

        public static readonly DependencyProperty EventTriggersProperty = DependencyProperty.RegisterAttached(
            @"EventTriggers",
            typeof(StylizedTriggerCollection),
            typeof(StylizedTriggers),
            new FrameworkPropertyMetadata(null, OnPropertyChanged));

        #endregion

        #region Static Methods (public)

        public static StylizedTriggerCollection GetEventTriggers(DependencyObject uie)
        {
            return (StylizedTriggerCollection)uie.GetValue(EventTriggersProperty);
        }

        public static void SetEventTriggers(DependencyObject uie, StylizedTriggerCollection value)
        {
            uie.SetValue(EventTriggersProperty, value);
        }

        #endregion

        #region Static Methods (private)

        private static void OnPropertyChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs e)
        {
            var uie = dpo as UIElement;

            if (uie == null)
            {
                return;
            }

            System.Windows.Interactivity.TriggerCollection itemTriggers = Interaction.GetTriggers(uie);

            var newTriggers = e.NewValue as StylizedTriggerCollection;
            var oldTriggers = e.OldValue as StylizedTriggerCollection;

            if (newTriggers == oldTriggers)
            {
                return;
            }

            if (oldTriggers != null)
            {
                foreach (var trigger in oldTriggers)
                {
                    int index = itemTriggers.IndexOf(trigger);

                    if (index >= 0)
                    {
                        itemTriggers.RemoveAt(index);
                    }
                }
            }

            if (newTriggers != null)
            {
                foreach (var trigger in newTriggers)
                {
                    int index = itemTriggers.IndexOf(trigger);

                    if (index < 0)
                    {
                        var trg = (System.Windows.Interactivity.EventTrigger)trigger.Clone();
                        itemTriggers.Add(trg);
                    }
                }
            } 
        }

        #endregion
    }
}