using System;
using System.Linq;
using System.Windows;
using Common.Types.Exceptions;
using Common.UI;
using Common.UI.ViewModel;
using Common.UI.ViewModel.WorkspaceViewModel;
using GU.Building.DataModel;
using GU.Building.UI.View;
using GU.BL;
using GU.Building.BL;
using GU.DataModel;
using GU.UI.View.TaskView;
using GU.UI.ViewModel.TaskViewModel;
using Microsoft.Practices.Prism.Commands;

namespace GU.Building.UI.ViewModel
{
    public class TaskModuleVM : BaseTaskManagementVM
    {
        public TaskModuleVM()
            : base(new SingletonDockableUiFactory())
        {
            
        }
    }
}

