using System;
using System.Linq;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel;
using Common.UI.ViewModel.Event;
using Common.UI.Views;
using GU.Building.BL;
using GU.Building.DataModel;
using GU.Building.UI.ViewModel;
using GU.DataModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.Building.UI.ViewModel
{
    public class BuildingModuleVM : BaseAvalonDockVM
    {
        public BuildingModuleVM()
            : base(new SingletonDockableUiFactory())
        {
            
        }
    }
}
