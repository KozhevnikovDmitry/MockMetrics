using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace GU.Enisey
{
    [RunInstaller(true)]
    public partial class EniseyWindowsServiceInstaller : System.Configuration.Install.Installer
    {
        public EniseyWindowsServiceInstaller()
        {
            InitializeComponent();

            var processInstaller = new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();

            //set the privileges
            processInstaller.Account = ServiceAccount.LocalSystem;

            //must be the same as what was set in Program's constructor
            serviceInstaller.ServiceName = Program.WINDOWS_SERVICE_NAME;
            serviceInstaller.DisplayName = Program.WINDOWS_SERVICE_NAME;
            serviceInstaller.Description = "GU.Enisey windows service";
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.AfterInstall += serviceInstaller_AfterInstall;

            this.Installers.Add(processInstaller);
            this.Installers.Add(serviceInstaller);
        }

        void serviceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            var sc = new ServiceController(Program.WINDOWS_SERVICE_NAME);
            sc.Start();
        }
    }
}
