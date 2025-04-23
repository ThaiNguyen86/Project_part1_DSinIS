
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
            this.txtRole = new System.Windows.Forms.TextBox();
            this.btnGrantRole = new System.Windows.Forms.Button();
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
            this.cmbGrantee.Location = new System.Drawing.Point(150, 50);
            this.cmbGrantee.Name = "cmbGrantee";
            this.cmbGrantee.Size = new System.Drawing.Size(200, 21);
            this.cmbGrantee.TabIndex = 0;
            // 
            // cmbObject
            // 
            this.cmbObject.FormattingEnabled = true;
            this.cmbObject.Location = new System.Drawing.Point(150, 80);
            this.cmbObject.Name = "cmbObject";
            this.cmbObject.Size = new System.Drawing.Size(200, 21);
            this.cmbObject.TabIndex = 1;
            this.cmbObject.SelectedIndexChanged += new System.EventHandler(this.cmbObject_SelectedIndexChanged);
            // 
            // cmbPrivilege
            // 
            this.cmbPrivilege.FormattingEnabled = true;
            this.cmbPrivilege.Items.AddRange(new object[] {
            "SELECT",
            "INSERT",
            "UPDATE",
            "DELETE",
            "EXECUTE"});
            this.cmbPrivilege.Location = new System.Drawing.Point(150, 110);
            this.cmbPrivilege.Name = "cmbPrivilege";
            this.cmbPrivilege.Size = new System.Drawing.Size(200, 21);
            this.cmbPrivilege.TabIndex = 2;
            // 
            // cmbColumn
            // 
            this.cmbColumn.FormattingEnabled = true;
            this.cmbColumn.Location = new System.Drawing.Point(150, 140);
            this.cmbColumn.Name = "cmbColumn";
            this.cmbColumn.Size = new System.Drawing.Size(200, 21);
            this.cmbColumn.TabIndex = 3;
            // 
            // chkWithGrantOption
            // 
            this.chkWithGrantOption.AutoSize = true;
            this.chkWithGrantOption.Location = new System.Drawing.Point(150, 170);
            this.chkWithGrantOption.Name = "chkWithGrantOption";
            this.chkWithGrantOption.Size = new System.Drawing.Size(114, 17);
            this.chkWithGrantOption.TabIndex = 4;
            this.chkWithGrantOption.Text = "With Grant Option";
            this.chkWithGrantOption.UseVisualStyleBackColor = true;
            // 
            // btnGrant
            // 
            this.btnGrant.Location = new System.Drawing.Point(150, 200);
            this.btnGrant.Name = "btnGrant";
            this.btnGrant.Size = new System.Drawing.Size(100, 30);
            this.btnGrant.TabIndex = 5;
            this.btnGrant.Text = "Grant Privilege";
            this.btnGrant.UseVisualStyleBackColor = true;
            this.btnGrant.Click += new System.EventHandler(this.btnGrant_Click);
            // 
            // btnRevoke
            // 
            this.btnRevoke.Location = new System.Drawing.Point(260, 200);
            this.btnRevoke.Name = "btnRevoke";
            this.btnRevoke.Size = new System.Drawing.Size(100, 30);
            this.btnRevoke.TabIndex = 6;
            this.btnRevoke.Text = "Revoke Privilege";
            this.btnRevoke.UseVisualStyleBackColor = true;
            this.btnRevoke.Click += new System.EventHandler(this.btnRevoke_Click);
            // 
            // txtRole
            // 
            this.txtRole.Location = new System.Drawing.Point(150, 240);
            this.txtRole.Name = "txtRole";
            this.txtRole.Size = new System.Drawing.Size(200, 20);
            this.txtRole.TabIndex = 7;
            // 
            // btnGrantRole
            // 
            this.btnGrantRole.Location = new System.Drawing.Point(150, 270);
            this.btnGrantRole.Name = "btnGrantRole";
            this.btnGrantRole.Size = new System.Drawing.Size(100, 30);
            this.btnGrantRole.TabIndex = 8;
            this.btnGrantRole.Text = "Grant Role";
            this.btnGrantRole.UseVisualStyleBackColor = true;
            this.btnGrantRole.Click += new System.EventHandler(this.btnGrantRole_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Grantee:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Object:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Privilege:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 140);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Column:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(50, 240);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Role:";
            // 
            // PrivilegeManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 350);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGrantRole);
            this.Controls.Add(this.txtRole);
            this.Controls.Add(this.btnRevoke);
            this.Controls.Add(this.btnGrant);
            this.Controls.Add(this.chkWithGrantOption);
            this.Controls.Add(this.cmbColumn);
            this.Controls.Add(this.cmbPrivilege);
            this.Controls.Add(this.cmbObject);
            this.Controls.Add(this.cmbGrantee);
            this.Name = "PrivilegeManagementForm";
            this.Text = "Manage Privileges";
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
        private System.Windows.Forms.TextBox txtRole;
        private System.Windows.Forms.Button btnGrantRole;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}