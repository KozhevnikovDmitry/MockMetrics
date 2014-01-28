using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using GU.Enisey.BL;

namespace GU.Enisey
{
    partial class EniseyWindowsService : ServiceBase
    {
        public EniseyWindowsService()
        {
            InitializeComponent();
            this.ServiceName = Program.WINDOWS_SERVICE_NAME;
        }

        protected override void OnStart(string[] args)
        {
            Task.Factory.StartNew(StartService);
        }

        private void StartService()
        {
            try
            {
                EniseyFacade.Initialize();
                EniseyFacade.InitializeServices();
            }
            catch (Exception ex)
            {
                LogMessage(string.Format("Failed to start service.\n{0}", ex), EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }

        public void LogMessage(string log, EventLogEntryType eventType)
        {
            try
            {
                string appName = Program.WINDOWS_SERVICE_NAME;
                if (!EventLog.SourceExists(appName))
                    EventLog.CreateEventSource(appName, appName);
                using(var eventLog = new EventLog())
                {
                    eventLog.Source = appName;
                    eventLog.WriteEntry(log, eventType);
                }
            }
            catch
            {
            }
        }
    }
}
