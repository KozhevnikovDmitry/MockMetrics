using System.Collections.Generic;

using Common.UI.ViewModel.Interfaces;
using Common.UI.ViewModel.WorkspaceViewModel;

using GU.MZ.CERT.UI.View;
using GU.MZ.CERT.UI.ViewModel;
using GU.MZ.CERT.View;
using GU.UI.View;
using GU.UI.ViewModel;

using Microsoft.Practices.Prism.ViewModel;

namespace GU.MZ.CERT.ViewModel
{
    public class MainOptionsVM : NotificationObject
    {
        public MainOptionsVM()
        {
            this.OptionsVMs = new List<IWorkspaceVM> 
            { 
                new ToolVM { View = new CommonOptionsView { DataContext = new CommonOptionsVM() }, DisplayName = "Общие настройки" }, 
                new ToolVM { View = new CertOptionsView { DataContext = new CertOptionsVM() }, DisplayName = "Настройки модуля Аттестации" }, 
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
