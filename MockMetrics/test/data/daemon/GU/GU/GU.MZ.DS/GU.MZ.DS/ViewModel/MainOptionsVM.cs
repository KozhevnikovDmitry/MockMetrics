using System.Collections.Generic;

using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.WorkspaceViewModel;

using GU.MZ.DS.UI.View;
using GU.MZ.DS.UI.ViewModel;
using GU.MZ.DS.View;
using GU.UI.View;
using GU.UI.ViewModel;

using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.DS.ViewModel
{
    public class MainOptionsVM : NotificationObject
    {
        public MainOptionsVM()
        {
            this.OptionsVMs = new List<IWorkspaceVM> 
            { 
                new ToolVM { View = new CommonOptionsView { DataContext = new CommonOptionsVM() }, DisplayName = "Общие настройки" }, 
                new ToolVM { View = new DsOptionsView { DataContext = new DsOptionsVM() }, DisplayName = "Настройки модуля Лекарственное обеспечение" }, 
                new ToolVM { View = new GUOptionsView { DataContext = new GUOptionsVM() }, DisplayName = "Настройки модуля Работы с заявлениями"  }
            };
        }

        #region Binding Properties

        private List<IWorkspaceVM> _optionsVMs;

        public List<IWorkspaceVM> OptionsVMs
        {
            get
            {
                return this._optionsVMs;
            }
            set
            {
                if (this._optionsVMs != value)
                {
                    this._optionsVMs = value;
                    this.RaisePropertyChanged(() => this.OptionsVMs);
                }
            }
        }

        #endregion

        #region Binding Commands

        #endregion

    }
}
