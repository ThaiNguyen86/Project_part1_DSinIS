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
            this.dgvPrivileges = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrivileges)).BeginInit();
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
            // dgvPrivileges
            // 
            this.dgvPrivileges.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPrivileges.Location = new System.Drawing.Point(50, 100);
            this.dgvPrivileges.Name = "dgvPrivileges";
            this.dgvPrivileges.Size = new System.Drawing.Size(500, 200);
            this.dgvPrivileges.TabIndex = 1;
            this.dgvPrivileges.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPrivileges.AllowUserToAddRows = false;
            this.dgvPrivileges.AllowUserToDeleteRows = false;
            this.dgvPrivileges.ReadOnly = true;
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
            this.ClientSize = new System.Drawing.Size(600, 350);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvPrivileges);
            this.Controls.Add(this.cmbGrantee);
            this.Name = "ViewPrivilegesForm";
            this.Text = "View Privileges";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrivileges)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }


        private System.Windows.Forms.ComboBox cmbGrantee;
        private System.Windows.Forms.DataGridView dgvPrivileges;
        private System.Windows.Forms.Label label1;
    }
}