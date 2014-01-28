namespace ServiceSpecLoader
{
    partial class ServiceSpecLoaderForm
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
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnRetrieveData = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lbService = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.cbServiceGroup = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.cbAgency = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.cbDataService = new DevExpress.XtraEditors.ComboBoxEdit();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.tabpageDoc = new DevExpress.XtraTab.XtraTabPage();
            this.grdDoc = new System.Windows.Forms.DataGridView();
            this.tabpageAttr = new DevExpress.XtraTab.XtraTabPage();
            this.grdAttr = new System.Windows.Forms.DataGridView();
            this.tabpageRequest = new DevExpress.XtraTab.XtraTabPage();
            this.grdRequest = new System.Windows.Forms.DataGridView();
            this.tabpageRequestAttr = new DevExpress.XtraTab.XtraTabPage();
            this.grdRequestAttr = new System.Windows.Forms.DataGridView();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            this.textExportPath = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tabpageDocSect = new DevExpress.XtraTab.XtraTabPage();
            this.grdDocSect = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lbService)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbServiceGroup.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbAgency.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbDataService.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.tabpageDoc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDoc)).BeginInit();
            this.tabpageAttr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAttr)).BeginInit();
            this.tabpageRequest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRequest)).BeginInit();
            this.tabpageRequestAttr.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdRequestAttr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textExportPath.Properties)).BeginInit();
            this.tabpageDocSect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDocSect)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.labelControl3);
            this.splitContainerControl1.Panel1.Controls.Add(this.btnRetrieveData);
            this.splitContainerControl1.Panel1.Controls.Add(this.labelControl2);
            this.splitContainerControl1.Panel1.Controls.Add(this.labelControl1);
            this.splitContainerControl1.Panel1.Controls.Add(this.lbService);
            this.splitContainerControl1.Panel1.Controls.Add(this.cbServiceGroup);
            this.splitContainerControl1.Panel1.Controls.Add(this.cbAgency);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.groupControl1);
            this.splitContainerControl1.Panel2.Controls.Add(this.btnExport);
            this.splitContainerControl1.Panel2.Controls.Add(this.textExportPath);
            this.splitContainerControl1.Panel2.Controls.Add(this.labelControl4);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(837, 445);
            this.splitContainerControl1.SplitterPosition = 249;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(12, 102);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(35, 13);
            this.labelControl3.TabIndex = 0;
            this.labelControl3.Text = "Услуга";
            // 
            // btnRetrieveData
            // 
            this.btnRetrieveData.Location = new System.Drawing.Point(16, 410);
            this.btnRetrieveData.Name = "btnRetrieveData";
            this.btnRetrieveData.Size = new System.Drawing.Size(115, 23);
            this.btnRetrieveData.TabIndex = 2;
            this.btnRetrieveData.Text = "Получить данные";
            this.btnRetrieveData.Click += new System.EventHandler(this.btnRetrieveData_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 57);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(67, 13);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "Группа услуг";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(54, 13);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Ведомство";
            // 
            // lbService
            // 
            this.lbService.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbService.Location = new System.Drawing.Point(12, 121);
            this.lbService.Name = "lbService";
            this.lbService.Size = new System.Drawing.Size(228, 267);
            this.lbService.TabIndex = 0;
            this.lbService.DoubleClick += new System.EventHandler(this.lbService_DoubleClick);
            // 
            // cbServiceGroup
            // 
            this.cbServiceGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbServiceGroup.Location = new System.Drawing.Point(12, 76);
            this.cbServiceGroup.Name = "cbServiceGroup";
            this.cbServiceGroup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbServiceGroup.Size = new System.Drawing.Size(228, 20);
            this.cbServiceGroup.TabIndex = 0;
            this.cbServiceGroup.EditValueChanged += new System.EventHandler(this.cbServiceGroup_EditValueChanged);
            // 
            // cbAgency
            // 
            this.cbAgency.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAgency.Location = new System.Drawing.Point(12, 31);
            this.cbAgency.Name = "cbAgency";
            this.cbAgency.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbAgency.Size = new System.Drawing.Size(228, 20);
            this.cbAgency.TabIndex = 0;
            this.cbAgency.EditValueChanged += new System.EventHandler(this.cbAgency_EditValueChanged);
            // 
            // groupControl1
            // 
            this.groupControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.cbDataService);
            this.groupControl1.Controls.Add(this.xtraTabControl1);
            this.groupControl1.Location = new System.Drawing.Point(14, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(557, 376);
            this.groupControl1.TabIndex = 3;
            this.groupControl1.Text = "Данные";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(17, 27);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(35, 13);
            this.labelControl5.TabIndex = 0;
            this.labelControl5.Text = "Услуга";
            // 
            // cbDataService
            // 
            this.cbDataService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDataService.Location = new System.Drawing.Point(58, 24);
            this.cbDataService.Name = "cbDataService";
            this.cbDataService.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbDataService.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbDataService.Size = new System.Drawing.Size(494, 20);
            this.cbDataService.TabIndex = 2;
            this.cbDataService.EditValueChanged += new System.EventHandler(this.cbDataService_EditValueChanged);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.xtraTabControl1.Location = new System.Drawing.Point(5, 50);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.tabpageDoc;
            this.xtraTabControl1.Size = new System.Drawing.Size(552, 321);
            this.xtraTabControl1.TabIndex = 1;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabpageDoc,
            this.tabpageDocSect,
            this.tabpageAttr,
            this.tabpageRequest,
            this.tabpageRequestAttr});
            // 
            // tabpageDoc
            // 
            this.tabpageDoc.Controls.Add(this.grdDoc);
            this.tabpageDoc.Name = "tabpageDoc";
            this.tabpageDoc.Size = new System.Drawing.Size(546, 293);
            this.tabpageDoc.Text = "doc";
            // 
            // grdDoc
            // 
            this.grdDoc.AllowUserToAddRows = false;
            this.grdDoc.AllowUserToDeleteRows = false;
            this.grdDoc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdDoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDoc.Location = new System.Drawing.Point(0, 0);
            this.grdDoc.Name = "grdDoc";
            this.grdDoc.ReadOnly = true;
            this.grdDoc.Size = new System.Drawing.Size(546, 293);
            this.grdDoc.TabIndex = 0;
            // 
            // tabpageAttr
            // 
            this.tabpageAttr.Controls.Add(this.grdAttr);
            this.tabpageAttr.Name = "tabpageAttr";
            this.tabpageAttr.Size = new System.Drawing.Size(546, 293);
            this.tabpageAttr.Text = "attr";
            // 
            // grdAttr
            // 
            this.grdAttr.AllowUserToAddRows = false;
            this.grdAttr.AllowUserToDeleteRows = false;
            this.grdAttr.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdAttr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdAttr.Location = new System.Drawing.Point(0, 0);
            this.grdAttr.Name = "grdAttr";
            this.grdAttr.ReadOnly = true;
            this.grdAttr.Size = new System.Drawing.Size(546, 293);
            this.grdAttr.TabIndex = 1;
            // 
            // tabpageRequest
            // 
            this.tabpageRequest.Controls.Add(this.grdRequest);
            this.tabpageRequest.Name = "tabpageRequest";
            this.tabpageRequest.Size = new System.Drawing.Size(546, 293);
            this.tabpageRequest.Text = "request";
            // 
            // grdRequest
            // 
            this.grdRequest.AllowUserToAddRows = false;
            this.grdRequest.AllowUserToDeleteRows = false;
            this.grdRequest.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdRequest.Location = new System.Drawing.Point(0, 0);
            this.grdRequest.Name = "grdRequest";
            this.grdRequest.ReadOnly = true;
            this.grdRequest.Size = new System.Drawing.Size(546, 293);
            this.grdRequest.TabIndex = 1;
            // 
            // tabpageRequestAttr
            // 
            this.tabpageRequestAttr.Controls.Add(this.grdRequestAttr);
            this.tabpageRequestAttr.Name = "tabpageRequestAttr";
            this.tabpageRequestAttr.Size = new System.Drawing.Size(546, 293);
            this.tabpageRequestAttr.Text = "request_attr";
            // 
            // grdRequestAttr
            // 
            this.grdRequestAttr.AllowUserToAddRows = false;
            this.grdRequestAttr.AllowUserToDeleteRows = false;
            this.grdRequestAttr.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdRequestAttr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdRequestAttr.Location = new System.Drawing.Point(0, 0);
            this.grdRequestAttr.Name = "grdRequestAttr";
            this.grdRequestAttr.ReadOnly = true;
            this.grdRequestAttr.Size = new System.Drawing.Size(546, 293);
            this.grdRequestAttr.TabIndex = 1;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(496, 411);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "Экспорт";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // textExportPath
            // 
            this.textExportPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textExportPath.EditValue = "d:\\account\\projects\\gu\\db\\gu\\data\\service_spec\\";
            this.textExportPath.Location = new System.Drawing.Point(14, 413);
            this.textExportPath.Name = "textExportPath";
            this.textExportPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.textExportPath.Size = new System.Drawing.Size(471, 20);
            this.textExportPath.TabIndex = 0;
            this.textExportPath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.textExportPath_ButtonClick);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(14, 394);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(99, 13);
            this.labelControl4.TabIndex = 0;
            this.labelControl4.Text = "Хранилище данных";
            // 
            // tabpageDocSect
            // 
            this.tabpageDocSect.Controls.Add(this.grdDocSect);
            this.tabpageDocSect.Name = "tabpageDocSect";
            this.tabpageDocSect.Size = new System.Drawing.Size(546, 293);
            this.tabpageDocSect.Text = "doc_sect";
            // 
            // grdDocSect
            // 
            this.grdDocSect.AllowUserToAddRows = false;
            this.grdDocSect.AllowUserToDeleteRows = false;
            this.grdDocSect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdDocSect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDocSect.Location = new System.Drawing.Point(0, 0);
            this.grdDocSect.Name = "grdDocSect";
            this.grdDocSect.ReadOnly = true;
            this.grdDocSect.Size = new System.Drawing.Size(546, 293);
            this.grdDocSect.TabIndex = 1;
            // 
            // ServiceSpecLoaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(837, 445);
            this.Controls.Add(this.splitContainerControl1);
            this.Name = "ServiceSpecLoaderForm";
            this.Text = "ГУ: Спецификации услуг";
            this.Shown += new System.EventHandler(this.ServiceSpecLoaderForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lbService)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbServiceGroup.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbAgency.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbDataService.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.tabpageDoc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDoc)).EndInit();
            this.tabpageAttr.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAttr)).EndInit();
            this.tabpageRequest.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdRequest)).EndInit();
            this.tabpageRequestAttr.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdRequestAttr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textExportPath.Properties)).EndInit();
            this.tabpageDocSect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDocSect)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.CheckedComboBoxEdit cbAgency;
        private DevExpress.XtraEditors.CheckedComboBoxEdit cbServiceGroup;
        private DevExpress.XtraEditors.CheckedListBoxControl lbService;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ButtonEdit textExportPath;
        private DevExpress.XtraEditors.SimpleButton btnExport;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage tabpageDoc;
        private System.Windows.Forms.DataGridView grdDoc;
        private DevExpress.XtraTab.XtraTabPage tabpageAttr;
        private System.Windows.Forms.DataGridView grdAttr;
        private DevExpress.XtraTab.XtraTabPage tabpageRequest;
        private System.Windows.Forms.DataGridView grdRequest;
        private DevExpress.XtraTab.XtraTabPage tabpageRequestAttr;
        private System.Windows.Forms.DataGridView grdRequestAttr;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.ComboBoxEdit cbDataService;
        private DevExpress.XtraEditors.SimpleButton btnRetrieveData;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private DevExpress.XtraTab.XtraTabPage tabpageDocSect;
        private System.Windows.Forms.DataGridView grdDocSect;
    }
}

