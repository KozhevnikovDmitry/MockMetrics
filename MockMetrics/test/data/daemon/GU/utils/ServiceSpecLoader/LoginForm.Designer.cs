namespace ServiceSpecLoader
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOk = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.textLogin = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.textPassword = new DevExpress.XtraEditors.TextEdit();
            this.btnExit = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.textServer = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.textSid = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.textScheme = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.textLogin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textServer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textSid.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textScheme.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(197, 9);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "ОК";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(30, 13);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Логин";
            // 
            // textLogin
            // 
            this.textLogin.Location = new System.Drawing.Point(81, 12);
            this.textLogin.Name = "textLogin";
            this.textLogin.Size = new System.Drawing.Size(100, 20);
            this.textLogin.TabIndex = 0;
            this.textLogin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textLogin_KeyDown);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 41);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(37, 13);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Пароль";
            // 
            // textPassword
            // 
            this.textPassword.Location = new System.Drawing.Point(81, 38);
            this.textPassword.Name = "textPassword";
            this.textPassword.Properties.UseSystemPasswordChar = true;
            this.textPassword.Size = new System.Drawing.Size(100, 20);
            this.textPassword.TabIndex = 1;
            this.textPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textLogin_KeyDown);
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(197, 38);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Выход";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(12, 67);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(37, 13);
            this.labelControl3.TabIndex = 1;
            this.labelControl3.Text = "Сервер";
            // 
            // textServer
            // 
            this.textServer.EditValue = "10.1.1.36";
            this.textServer.Location = new System.Drawing.Point(81, 64);
            this.textServer.Name = "textServer";
            this.textServer.Size = new System.Drawing.Size(100, 20);
            this.textServer.TabIndex = 2;
            this.textServer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textLogin_KeyDown);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(12, 93);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(20, 13);
            this.labelControl4.TabIndex = 1;
            this.labelControl4.Text = "Сид";
            // 
            // textSid
            // 
            this.textSid.EditValue = "bp";
            this.textSid.Location = new System.Drawing.Point(81, 90);
            this.textSid.Name = "textSid";
            this.textSid.Size = new System.Drawing.Size(100, 20);
            this.textSid.TabIndex = 3;
            this.textSid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textLogin_KeyDown);
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(12, 119);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(31, 13);
            this.labelControl5.TabIndex = 1;
            this.labelControl5.Text = "Схема";
            // 
            // textScheme
            // 
            this.textScheme.EditValue = "gu";
            this.textScheme.Location = new System.Drawing.Point(81, 116);
            this.textScheme.Name = "textScheme";
            this.textScheme.Size = new System.Drawing.Size(100, 20);
            this.textScheme.TabIndex = 4;
            this.textScheme.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textLogin_KeyDown);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 146);
            this.Controls.Add(this.textPassword);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.textScheme);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.textSid);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.textServer);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.textLogin);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Подключение к БД";
            ((System.ComponentModel.ISupportInitialize)(this.textLogin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textServer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textSid.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textScheme.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnOk;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit textLogin;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit textPassword;
        private DevExpress.XtraEditors.SimpleButton btnExit;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit textServer;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit textSid;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit textScheme;
    }
}