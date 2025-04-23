namespace OracleUserManagementApp.Forms
{
    partial class UserRoleManagementForm
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
            this.lstUsersRoles = new System.Windows.Forms.ListBox();
            this.btnCreateUser = new System.Windows.Forms.Button();
            this.btnCreateRole = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstUsersRoles
            // 
            this.lstUsersRoles.FormattingEnabled = true;
            this.lstUsersRoles.Location = new System.Drawing.Point(30, 30);
            this.lstUsersRoles.Name = "lstUsersRoles";
            this.lstUsersRoles.Size = new System.Drawing.Size(300, 200);
            this.lstUsersRoles.TabIndex = 0;
            // 
            // btnCreateUser
            // 
            this.btnCreateUser.Location = new System.Drawing.Point(30, 250);
            this.btnCreateUser.Name = "btnCreateUser";
            this.btnCreateUser.Size = new System.Drawing.Size(90, 30);
            this.btnCreateUser.TabIndex = 1;
            this.btnCreateUser.Text = "Create User";
            this.btnCreateUser.UseVisualStyleBackColor = true;
            this.btnCreateUser.Click += new System.EventHandler(this.btnCreateUser_Click);
            // 
            // btnCreateRole
            // 
            this.btnCreateRole.Location = new System.Drawing.Point(130, 250);
            this.btnCreateRole.Name = "btnCreateRole";
            this.btnCreateRole.Size = new System.Drawing.Size(90, 30);
            this.btnCreateRole.TabIndex = 2;
            this.btnCreateRole.Text = "Create Role";
            this.btnCreateRole.UseVisualStyleBackColor = true;
            this.btnCreateRole.Click += new System.EventHandler(this.btnCreateRole_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(230, 250);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 30);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // UserRoleManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 300);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnCreateRole);
            this.Controls.Add(this.btnCreateUser);
            this.Controls.Add(this.lstUsersRoles);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "UserRoleManagementForm";
            this.Text = "Manage Users and Roles";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ListBox lstUsersRoles;
        private System.Windows.Forms.Button btnCreateUser;
        private System.Windows.Forms.Button btnCreateRole;
        private System.Windows.Forms.Button btnDelete;
    }
}