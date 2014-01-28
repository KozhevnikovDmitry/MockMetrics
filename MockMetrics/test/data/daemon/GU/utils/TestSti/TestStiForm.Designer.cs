namespace TestSti
{
    partial class TestStiForm
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
            this.btnShow = new System.Windows.Forms.Button();
            this.btnShowDesigner = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point(89, 54);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(98, 23);
            this.btnShow.TabIndex = 0;
            this.btnShow.Text = "Show";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // btnShowDesigner
            // 
            this.btnShowDesigner.Location = new System.Drawing.Point(89, 83);
            this.btnShowDesigner.Name = "btnShowDesigner";
            this.btnShowDesigner.Size = new System.Drawing.Size(98, 23);
            this.btnShowDesigner.TabIndex = 1;
            this.btnShowDesigner.Text = "Show designer";
            this.btnShowDesigner.UseVisualStyleBackColor = true;
            this.btnShowDesigner.Click += new System.EventHandler(this.btnShowDesigner_Click);
            // 
            // TestStiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 201);
            this.Controls.Add(this.btnShowDesigner);
            this.Controls.Add(this.btnShow);
            this.Name = "TestStiForm";
            this.Text = "Form";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Button btnShowDesigner;
    }
}

