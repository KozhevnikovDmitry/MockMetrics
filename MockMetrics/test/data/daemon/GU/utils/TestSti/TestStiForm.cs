using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Report;

namespace TestSti
{
    public partial class TestStiForm : Form
    {
        public TestStiForm()
        {
            InitializeComponent();
        }

        private StiReport PrepareReport()
        {
            var serviceGroup1 = new ServiceGroup { Name = "Group1" };
            var serviceGroup2 = new ServiceGroup { Name = "Group2" };
            var service11 = new Service { Name = "Service1", ServiceGroup = serviceGroup1 };
            var service12 = new Service { Name = "Service2", ServiceGroup = serviceGroup1 };
            var service21 = new Service { Name = "Service1", ServiceGroup = serviceGroup2 };

            var tasks = new List<Task> {
                new Task { Number = "1", Service = service11},
                new Task { Number = "2", Service = service11},
                new Task { Number = "3", Service = service12},
                new Task { Number = "4", Service = service21}
            };

            var report = new StiReport();
            report.Load("Report.mrt");
            report.RegBusinessObject("test", "Task", tasks);

            return report;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            var report = PrepareReport();
            report.Show();
        }

        private void btnShowDesigner_Click(object sender, EventArgs e)
        {
            var report = PrepareReport();
            report.Design();
        }
    }
}
