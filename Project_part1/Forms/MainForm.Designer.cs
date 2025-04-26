namespace OracleUserManagementApp.Forms
{
    partial class MainForm
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
            this.btnManageUsers = new System.Windows.Forms.Button();
            this.btnManagePrivileges = new System.Windows.Forms.Button();
            this.btnViewPrivileges = new System.Windows.Forms.Button();
            this.btnBackToLogin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnManageUsers
            // 
            this.btnManageUsers.Location = new System.Drawing.Point(117, 112);
            this.btnManageUsers.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.btnManageUsers.Name = "btnManageUsers";
            this.btnManageUsers.Size = new System.Drawing.Size(467, 89);
            this.btnManageUsers.TabIndex = 0;
            this.btnManageUsers.Text = "Manage Users and Roles";
            this.btnManageUsers.UseVisualStyleBackColor = true;
            this.btnManageUsers.Click += new System.EventHandler(this.btnManageUsers_Click);
            // 
            // btnManagePrivileges
            // 
            this.btnManagePrivileges.Location = new System.Drawing.Point(117, 223);
            this.btnManagePrivileges.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.btnManagePrivileges.Name = "btnManagePrivileges";
            this.btnManagePrivileges.Size = new System.Drawing.Size(467, 89);
            this.btnManagePrivileges.TabIndex = 1;
            this.btnManagePrivileges.Text = "Manage Privileges";
            this.btnManagePrivileges.UseVisualStyleBackColor = true;
            this.btnManagePrivileges.Click += new System.EventHandler(this.btnManagePrivileges_Click);
            // 
            // btnViewPrivileges
            // 
            this.btnViewPrivileges.Location = new System.Drawing.Point(117, 335);
            this.btnViewPrivileges.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.btnViewPrivileges.Name = "btnViewPrivileges";
            this.btnViewPrivileges.Size = new System.Drawing.Size(467, 89);
            this.btnViewPrivileges.TabIndex = 2;
            this.btnViewPrivileges.Text = "View Privileges";
            this.btnViewPrivileges.UseVisualStyleBackColor = true;
            this.btnViewPrivileges.Click += new System.EventHandler(this.btnViewPrivileges_Click);
            // 
            // btnBackToLogin
            // 
            this.btnBackToLogin.Location = new System.Drawing.Point(117, 446);
            this.btnBackToLogin.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.btnBackToLogin.Name = "btnBackToLogin";
            this.btnBackToLogin.Size = new System.Drawing.Size(467, 89);
            this.btnBackToLogin.TabIndex = 3;
            this.btnBackToLogin.Text = "Back to Login";
            this.btnBackToLogin.UseVisualStyleBackColor = true;
            this.btnBackToLogin.Click += new System.EventHandler(this.btnBackToLogin_Click);
            //
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 669);
            this.Controls.Add(this.btnBackToLogin);
            this.Controls.Add(this.btnViewPrivileges);
            this.Controls.Add(this.btnManagePrivileges);
            this.Controls.Add(this.btnManageUsers);
            this.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Name = "MainForm";
            this.Text = "Oracle DB Admin";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnManageUsers;
        private System.Windows.Forms.Button btnManagePrivileges;
        private System.Windows.Forms.Button btnViewPrivileges;
        private System.Windows.Forms.Button btnBackToLogin;
    }
}