using System.Collections.Generic;
using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.WorkspaceViewModel;
using Microsoft.Practices.Prism.ViewModel;
using GU.ZLP.View;
using GU.UI.View;
using GU.UI.ViewModel;

namespace GU.ZLP.ViewModel
{
    public class MainOptionsVM : NotificationObject
    {
        public MainOptionsVM()
        {
            OptionsVMs = new List<IWorkspaceVM> 
            { 
                new ToolVM { View = new CommonOptionsView() { DataContext = new CommonOptionsVM() }, DisplayName = "Общие настройки" }, 
                new ToolVM { View = new GUOptionsView() { DataContext = new GUOptionsVM() }, DisplayName = "Настройки модуля Работы с заявлениями"  }
            };
        }

        #region Binding Properties

        private List<IWorkspaceVM> _optionsVMs;

        public List<IWorkspaceVM> OptionsVMs
        {
            get
            {
                return _optionsVMs;
            }
            set
            {
                if (_optionsVMs != value)
                {
                    _optionsVMs = value;
                    RaisePropertyChanged(() => OptionsVMs);
                }
            }
        }

        #endregion

        #region Binding Commands

        #endregion

    }
}
