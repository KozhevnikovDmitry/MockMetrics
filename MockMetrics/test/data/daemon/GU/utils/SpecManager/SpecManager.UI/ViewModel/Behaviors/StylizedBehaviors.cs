using System.Windows.Interactivity;
using System.Windows;

namespace SpecManager.UI.ViewModel.Behaviors
{
    public class StylizedBehaviors
    {
        #region Fields (public)

        public static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached(
            @"Behaviors",
            typeof(StylizedBehaviorCollection),
            typeof(StylizedBehaviors),
            new FrameworkPropertyMetadata(null, OnPropertyChanged));

        #endregion

        #region Static Methods (public)

        public static StylizedBehaviorCollection GetBehaviors(DependencyObject uie)
        {
            return (StylizedBehaviorCollection)uie.GetValue(BehaviorsProperty);
        }

        public static void SetBehaviors(DependencyObject uie, StylizedBehaviorCollection value)
        {
            uie.SetValue(BehaviorsProperty, value);
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

            BehaviorCollection itemBehaviors = Interaction.GetBehaviors(uie);

            var newBehaviors = e.NewValue as StylizedBehaviorCollection;
            var oldBehaviors = e.OldValue as StylizedBehaviorCollection;

            if (newBehaviors == oldBehaviors)
            {
                return;
            }

            if (oldBehaviors != null)
            {
                foreach (var behavior in oldBehaviors)
                {
                    int index = itemBehaviors.IndexOf(behavior);

                    if (index >= 0)
                    {
                        itemBehaviors.RemoveAt(index);
                    }
                }
            }

            if (newBehaviors != null)
            {
                foreach (var behavior in newBehaviors)
                {
                    int index = itemBehaviors.IndexOf(behavior);

                    if (index < 0)
                    {
                       var bhv = (Behavior)behavior.Clone();
                       itemBehaviors.Add(bhv);
                    }
                }
            }
        }

        #endregion
    }    
}
