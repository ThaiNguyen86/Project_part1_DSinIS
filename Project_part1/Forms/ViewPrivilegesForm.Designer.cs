namespace OracleUserManagementApp.Forms
{
    partial class ViewPrivilegesForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cmbGrantee = new System.Windows.Forms.ComboBox();
            this.lstPrivileges = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbGrantee
            // 
            this.cmbGrantee.FormattingEnabled = true;
            this.cmbGrantee.Location = new System.Drawing.Point(150, 50);
            this.cmbGrantee.Name = "cmbGrantee";
            this.cmbGrantee.Size = new System.Drawing.Size(200, 21);
            this.cmbGrantee.TabIndex = 0;
            this.cmbGrantee.SelectedIndexChanged += new System.EventHandler(this.cmbGrantee_SelectedIndexChanged);
            // 
            // lstPrivileges
            // 
            this.lstPrivileges.FormattingEnabled = true;
            this.lstPrivileges.Location = new System.Drawing.Point(50, 100);
            this.lstPrivileges.Name = "lstPrivileges";
            this.lstPrivileges.Size = new System.Drawing.Size(300, 200);
            this.lstPrivileges.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Grantee:";
            // 
            // ViewPrivilegesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 350);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstPrivileges);
            this.Controls.Add(this.cmbGrantee);
            this.Name = "ViewPrivilegesForm";
            this.Text = "View Privileges";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ComboBox cmbGrantee;
        private System.Windows.Forms.ListBox lstPrivileges;
        private System.Windows.Forms.Label label1;
    }
}