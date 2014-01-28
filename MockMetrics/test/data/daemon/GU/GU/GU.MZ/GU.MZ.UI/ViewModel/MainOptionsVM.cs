using System.Collections.Generic;
using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.WorkspaceViewModel;
using GU.MZ.UI.View;
using GU.UI.View;
using GU.UI.ViewModel;
using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.UI.ViewModel
{
    public class MainOptionsVm : NotificationObject
    {
        public MainOptionsVm()
        {
            OptionsVMs = new List<IWorkspaceVM> 
            { 
                new ToolVM { View = new CommonOptionsView { DataContext = new CommonOptionsVm() }, DisplayName = "Общие настройки" }, 
                new ToolVM { View = new GumzOptionsView { DataContext = new GumzOptionsVm() }, DisplayName = "Настройки модуля Лицензирование" }, 
                new ToolVM { View = new GUOptionsView { DataContext = new GUOptionsVM() }, DisplayName = "Настройки модуля Работы с заявлениями"  }
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
