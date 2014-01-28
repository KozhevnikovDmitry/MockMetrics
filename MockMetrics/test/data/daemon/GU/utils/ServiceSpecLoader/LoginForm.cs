using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Devart.Data.Oracle;
using System.Data.Common;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

namespace ServiceSpecLoader
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public bool Login()
        {
            while (ShowDialog() == DialogResult.OK)
            {
                DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
                builder.Add("Server", textServer.Text);
                builder.Add("Port", 1521);
                builder.Add("Sid", textSid.Text);
                builder.Add("User Id", textLogin.Text);
                builder.Add("Password", textPassword.Text);
                builder.Add("Direct", "true");
                
                var p = new DevartDataProvider();
                DbManager.AddDataProvider("Devart", p);
                DbManager.AddConnectionString("Devart", builder.ConnectionString);


                try
                {
                    using (DbManager db = new DbManager())
                    {
                        var res = db.
                            SetCommand("ALTER SESSION SET CURRENT_SCHEMA = :scheme").
                            Parameter("scheme", textScheme.Text);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return false;
        }

        private void textLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnOk.PerformClick();
        }

    }
}
