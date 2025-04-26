namespace OracleUserManagementApp.Forms
{
    partial class PrivilegeManagementForm
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
            this.cmbObject = new System.Windows.Forms.ComboBox();
            this.cmbPrivilege = new System.Windows.Forms.ComboBox();
            this.cmbColumn = new System.Windows.Forms.ComboBox();
            this.chkWithGrantOption = new System.Windows.Forms.CheckBox();
            this.btnGrant = new System.Windows.Forms.Button();
            this.btnRevoke = new System.Windows.Forms.Button();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.btnGrantRole = new System.Windows.Forms.Button();
            this.btnRevokeRole = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbGrantee
            // 
            this.cmbGrantee.FormattingEnabled = true;
            this.cmbGrantee.Location = new System.Drawing.Point(180, 40); // Adjusted position
            this.cmbGrantee.Name = "cmbGrantee";
            this.cmbGrantee.Size = new System.Drawing.Size(300, 28); // Increased size
            this.cmbGrantee.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 40); // Adjusted position
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Grantee:";
            // 
            // cmbObject
            // 
            this.cmbObject.FormattingEnabled = true;
            this.cmbObject.Location = new System.Drawing.Point(180, 80); // Adjusted position
            this.cmbObject.Name = "cmbObject";
            this.cmbObject.Size = new System.Drawing.Size(300, 28); // Increased size
            this.cmbObject.TabIndex = 1;
            this.cmbObject.SelectedIndexChanged += new System.EventHandler(this.cmbObject_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 80); // Adjusted position
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Object:";
            // 
            // cmbPrivilege
            // 
            this.cmbPrivilege.FormattingEnabled = true;
            this.cmbPrivilege.Items.AddRange(new object[] {
        "SELECT",
        "INSERT",
        "UPDATE",
        "DELETE",
        "EXECUTE",
        "CREATE SESSION",
        "CREATE TABLE",
        "CREATE USER",
        "CREATE ROLE",
        "CREATE VIEW",
        "CREATE PROCEDURE",
        "UNLIMITED TABLESPACE",
        "DBA"
    });
            this.cmbPrivilege.Location = new System.Drawing.Point(180, 120); // Adjusted position
            this.cmbPrivilege.Name = "cmbPrivilege";
            this.cmbPrivilege.Size = new System.Drawing.Size(300, 28); // Increased size
            this.cmbPrivilege.TabIndex = 2;
            this.cmbPrivilege.SelectedIndexChanged += new System.EventHandler(this.cmbPrivilege_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 120); // Adjusted position
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Privilege:";
            // 
            // cmbColumn
            // 
            this.cmbColumn.FormattingEnabled = true;
            this.cmbColumn.Location = new System.Drawing.Point(180, 160); // Adjusted position
            this.cmbColumn.Name = "cmbColumn";
            this.cmbColumn.Size = new System.Drawing.Size(300, 28); // Increased size
            this.cmbColumn.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(40, 160); // Adjusted position
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Column:";
            // 
            // chkWithGrantOption
            // 
            this.chkWithGrantOption.AutoSize = true;
            this.chkWithGrantOption.Location = new System.Drawing.Point(180, 200); // Adjusted position
            this.chkWithGrantOption.Name = "chkWithGrantOption";
            this.chkWithGrantOption.Size = new System.Drawing.Size(114, 17);
            this.chkWithGrantOption.TabIndex = 4;
            this.chkWithGrantOption.Text = "With Grant Option";
            this.chkWithGrantOption.UseVisualStyleBackColor = true;
            // 
            // btnGrant
            // 
            this.btnGrant.Location = new System.Drawing.Point(180, 230); // Adjusted position
            this.btnGrant.Name = "btnGrant";
            this.btnGrant.Size = new System.Drawing.Size(130, 35); // Increased size
            this.btnGrant.TabIndex = 5;
            this.btnGrant.Text = "Grant Privilege";
            this.btnGrant.UseVisualStyleBackColor = true;
            this.btnGrant.Click += new System.EventHandler(this.btnGrant_Click);
            // 
            // btnRevoke
            // 
            this.btnRevoke.Location = new System.Drawing.Point(350, 230); // Adjusted position
            this.btnRevoke.Name = "btnRevoke";
            this.btnRevoke.Size = new System.Drawing.Size(130, 35); // Increased size
            this.btnRevoke.TabIndex = 6;
            this.btnRevoke.Text = "Revoke Privilege";
            this.btnRevoke.UseVisualStyleBackColor = true;
            this.btnRevoke.Click += new System.EventHandler(this.btnRevoke_Click);
            // 
            // cmbRole
            // 
            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Location = new System.Drawing.Point(180, 280); // Adjusted position
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(300, 28); // Increased size
            this.cmbRole.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(40, 280); // Adjusted position
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Role:";
            // 
            // btnGrantRole
            // 
            this.btnGrantRole.Location = new System.Drawing.Point(180, 320); // Adjusted position
            this.btnGrantRole.Name = "btnGrantRole";
            this.btnGrantRole.Size = new System.Drawing.Size(130, 35); // Increased size
            this.btnGrantRole.TabIndex = 8;
            this.btnGrantRole.Text = "Grant Role";
            this.btnGrantRole.UseVisualStyleBackColor = true;
            this.btnGrantRole.Click += new System.EventHandler(this.btnGrantRole_Click);
            // 
            // btnRevokeRole
            // 
            this.btnRevokeRole.Location = new System.Drawing.Point(350, 320); // Adjusted position
            this.btnRevokeRole.Name = "btnRevokeRole";
            this.btnRevokeRole.Size = new System.Drawing.Size(130, 35); // Increased size
            this.btnRevokeRole.TabIndex = 14;
            this.btnRevokeRole.Text = "Revoke Role";
            this.btnRevokeRole.UseVisualStyleBackColor = true;
            this.btnRevokeRole.Click += new System.EventHandler(this.btnRevokeRole_Click);
            // 
            // PrivilegeManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F); // Adjusted for better scaling
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 400); // Increased form size
            this.Controls.Add(this.btnRevokeRole);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGrantRole);
            this.Controls.Add(this.cmbRole);
            this.Controls.Add(this.btnRevoke);
            this.Controls.Add(this.btnGrant);
            this.Controls.Add(this.chkWithGrantOption);
            this.Controls.Add(this.cmbColumn);
            this.Controls.Add(this.cmbPrivilege);
            this.Controls.Add(this.cmbObject);
            this.Controls.Add(this.cmbGrantee);
            this.Name = "PrivilegeManagementForm";
            this.Text = "Manage Privileges";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle; // Prevent resizing
            this.MaximizeBox = false; // Disable maximize
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen; // Center the form
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ComboBox cmbGrantee;
        private System.Windows.Forms.ComboBox cmbObject;
        private System.Windows.Forms.ComboBox cmbPrivilege;
        private System.Windows.Forms.ComboBox cmbColumn;
        private System.Windows.Forms.CheckBox chkWithGrantOption;
        private System.Windows.Forms.Button btnGrant;
        private System.Windows.Forms.Button btnRevoke;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.Button btnGrantRole;
        private System.Windows.Forms.Button btnRevokeRole; 
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}