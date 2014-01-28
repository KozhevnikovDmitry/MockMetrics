using System;
using Common.UI.ViewModel.Interfaces;
using PostSharp.Aspects;

namespace Common.UI.Aspect
{
    [Serializable]
    public sealed class NotifyPropertyChangedAttribute : LocationInterceptionAspect
    {
        public override void OnSetValue(LocationInterceptionArgs args)
        {
            if (args.GetCurrentValue() == null || !args.GetCurrentValue().Equals(args.Value))
            {
                base.OnSetValue(args);
                var raisePropertyChanged = args.Instance as IRaisePropertyChanged;
                if (raisePropertyChanged != null)
                    raisePropertyChanged.RaiseNotifyPropertyChanged(args.LocationName);
            }
        }
    }
}
