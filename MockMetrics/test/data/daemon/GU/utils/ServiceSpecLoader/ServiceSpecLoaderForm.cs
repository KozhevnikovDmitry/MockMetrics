using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Devart.Data.Oracle;
using BLToolkit.Data;
using BLToolkit.DataAccess;
using DevExpress.XtraEditors.Controls;

namespace ServiceSpecLoader
{
    public partial class ServiceSpecLoaderForm : Form
    {
        private bool _isLogged = false;
        private Dictionary<int, KeyValuePair<Service, DataSet>> _data;

        #region Init&Login

        public ServiceSpecLoaderForm()
        {
            _data = new Dictionary<int, KeyValuePair<Service, DataSet>>();
            _isLogged = Login();
            InitializeComponent();
        }

        private bool Login()
        {
            var loginForm = new LoginForm();
            return loginForm.Login();
        }

        #endregion

        #region Events

        private void ServiceSpecLoaderForm_Shown(object sender, EventArgs e)
        {
            if (!_isLogged)
            {
                Close();
                return;
            }
            AgencyAccessor aa = AgencyAccessor.CreateInstance();
            foreach (var agency in aa.SelectAll())
            {
                cbAgency.Properties.Items.Add(agency);
            }
        }

        private void cbAgency_EditValueChanged(object sender, EventArgs e)
        {
            ServiceGroupAccessor sga = ServiceGroupAccessor.CreateInstance();
            cbServiceGroup.Properties.Items.Clear();
            foreach (CheckedListBoxItem item in cbAgency.Properties.Items)
            {
                if (item.CheckState == CheckState.Checked)
                {
                    Agency a = item.Value as Agency;
                    foreach (var sg in sga.SelectByAgency(a.AgencyId))
                    {
                        sg.Agency = a;
                        cbServiceGroup.Properties.Items.Add(sg);
                    }
                }
            }
        }

        private void cbServiceGroup_EditValueChanged(object sender, EventArgs e)
        {
            ServiceAccessor sa = ServiceAccessor.CreateInstance();
            lbService.Items.Clear();
            foreach (CheckedListBoxItem item in cbServiceGroup.Properties.Items)
            {
                if (item.CheckState == CheckState.Checked)
                {
                    var sg = item.Value as ServiceGroup;
                    foreach (var s in sa.SelectByServiceGroup(sg.ServiceGroupId))
                    {
                        s.ServiceGroup = sg;
                        lbService.Items.Add(s);
                    }
                }
            }
            lbService.CheckAll();
        }

        private void lbService_DoubleClick(object sender, EventArgs e)
        {
            if (lbService.CheckedItemsCount != lbService.ItemCount)
                lbService.CheckAll();
            else lbService.UnCheckAll();
        }

        private void cbDataService_EditValueChanged(object sender, EventArgs e)
        {
            var serviceId = (cbDataService.SelectedItem as Service).ServiceId;
            var ds = _data[serviceId].Value;
            grdDoc.DataSource = ds.Tables["mig_doc_spec"];
            grdDocSect.DataSource = ds.Tables["mig_doc_sect_spec"];
            grdAttr.DataSource = ds.Tables["mig_attr_spec"];
            grdRequest.DataSource = ds.Tables["mig_request_spec"];
            grdRequestAttr.DataSource = ds.Tables["mig_request_attr_spec"];
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportDataToFiles();
        }

        private void btnRetrieveData_Click(object sender, EventArgs e)
        {
            var serviceList = new List<Service>();
            foreach (CheckedListBoxItem item in lbService.CheckedItems)
            {
                serviceList.Add(item.Value as Service);
            }
            RetrieveDataFromDB(serviceList);

            cbDataService.Properties.Items.Clear();
            foreach (var s in serviceList)
                cbDataService.Properties.Items.Add(s);
            if (cbDataService.Properties.Items.Count > 0)
                cbDataService.SelectedIndex = 0;
        }

        private void textExportPath_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            folderBrowserDialog.SelectedPath = textExportPath.Text;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                textExportPath.Text = folderBrowserDialog.SelectedPath;
        }

        #endregion


        #region Import&Export

        private void RetrieveDataFromDB(List<Service> serviceList)
        {
            MigDataAccessor ma = DataAccessor.CreateInstance<MigDataAccessor>();

            _data.Clear();
            foreach (var service in serviceList)
            {
                DataSet ds = new DataSet();
                ma.SelectDoc(service.ServiceId, ds);
                ma.SelectDocSect(service.ServiceId, ds);
                ma.SelectAttr(service.ServiceId, ds);
                ma.SelectRequest(service.ServiceId, ds);
                ma.SelectRequestAttr(service.ServiceId, ds);
                _data[service.ServiceId] = new KeyValuePair<Service, DataSet>(service, ds);
            }
        }

        private void ExportDataToFiles()
        {
            try
            {
                foreach (var d in _data.Values)
                {
                    Service s = d.Key;
                    string agencyName = String.Format("{0}. {1}", s.ServiceGroup.Agency.AgencyId, s.ServiceGroup.Agency.AgencyName);
                    string serviceGroupName = String.Format("{0}. {1}", s.ServiceGroup.ServiceGroupId, s.ServiceGroup.ServiceGroupName);
                    string serviceName = String.Format("{0}. {1}", s.ServiceId, s.ServiceName);
                    
                    string dirPath = Path.Combine(textExportPath.Text, agencyName, serviceGroupName);
                    string fileName = serviceName + ".sql";

                    SQLScriptHelper.ExportDatasetToSqlScript(
                        d.Value, s.ServiceId, 
                        new String[] { "mig_request_attr_spec", "mig_request_spec", "mig_attr_spec", "mig_doc_sect_spec", "mig_doc_spec" }, 
                        true, false, 
                        String.Format("{0}\n  {1}\n    {2}", agencyName, serviceGroupName, serviceName), 
                        Path.Combine(dirPath, fileName), true, false);

                    /*CSVHelper.ExportDatatableToCSV(d.Value.Tables["mig_doc_spec"], Path.Combine(dirPath, "doc.csv"), true, false);
                    CSVHelper.ExportDatatableToCSV(d.Value.Tables["mig_doc_sect_spec"], Path.Combine(dirPath, "doc_sect.csv"), true, false);
                    CSVHelper.ExportDatatableToCSV(d.Value.Tables["mig_attr_spec"], Path.Combine(dirPath, "attr.csv"), true, false);
                    CSVHelper.ExportDatatableToCSV(d.Value.Tables["mig_request_spec"], Path.Combine(dirPath, "request.csv"), true, false);
                    CSVHelper.ExportDatatableToCSV(d.Value.Tables["mig_request_attr_spec"], Path.Combine(dirPath, "request_attr.csv"), true, false);
                    */
                }
                MessageBox.Show("Данные успешно выгружены");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка выгрузки данных: " + ex.Message);
            }
        }

        #endregion

    }

}