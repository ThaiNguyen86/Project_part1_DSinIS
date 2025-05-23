﻿namespace OracleUserManagementApp.Forms
{
    partial class LoginForm
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
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.rbService = new System.Windows.Forms.RadioButton();
            this.rbSid = new System.Windows.Forms.RadioButton();
            this.lblServiceInput = new System.Windows.Forms.Label();
            this.txtService = new System.Windows.Forms.TextBox();
            this.lblSidInput = new System.Windows.Forms.Label();
            this.txtSid = new System.Windows.Forms.TextBox();
            this.lblHostname = new System.Windows.Forms.Label(); // New
            this.txtHostname = new System.Windows.Forms.TextBox(); // New
            this.lblPort = new System.Windows.Forms.Label(); // New
            this.txtPort = new System.Windows.Forms.TextBox(); // New
            this.SuspendLayout();
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(30, 30);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Username:";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(120, 27);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(200, 20);
            this.txtUsername.TabIndex = 1;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(30, 60);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(120, 57);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(200, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblHostname
            // 
            this.lblHostname.AutoSize = true;
            this.lblHostname.Location = new System.Drawing.Point(30, 90);
            this.lblHostname.Name = "lblHostname";
            this.lblHostname.Size = new System.Drawing.Size(58, 13);
            this.lblHostname.TabIndex = 4;
            this.lblHostname.Text = "Hostname:";
            // 
            // txtHostname
            // 
            this.txtHostname.Location = new System.Drawing.Point(120, 87);
            this.txtHostname.Name = "txtHostname";
            this.txtHostname.Size = new System.Drawing.Size(200, 20);
            this.txtHostname.TabIndex = 5;
            this.txtHostname.Text = "localhost"; // Default value
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(30, 120);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(33, 13);
            this.lblPort.TabIndex = 6;
            this.lblPort.Text = "Port:";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(120, 117);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(200, 20);
            this.txtPort.TabIndex = 7;
            this.txtPort.Text = "1521"; // Default value
            // 
            // rbService
            // 
            this.rbService.AutoSize = true;
            this.rbService.Checked = true;
            this.rbService.Location = new System.Drawing.Point(120, 150);
            this.rbService.Name = "rbService";
            this.rbService.Size = new System.Drawing.Size(61, 17);
            this.rbService.TabIndex = 8;
            this.rbService.TabStop = true;
            this.rbService.Text = "Service";
            this.rbService.CheckedChanged += new System.EventHandler(this.rbService_CheckedChanged);
            // 
            // rbSid
            // 
            this.rbSid.AutoSize = true;
            this.rbSid.Location = new System.Drawing.Point(200, 150);
            this.rbSid.Name = "rbSid";
            this.rbSid.Size = new System.Drawing.Size(44, 17);
            this.rbSid.TabIndex = 9;
            this.rbSid.Text = "SID";
            this.rbSid.CheckedChanged += new System.EventHandler(this.rbSid_CheckedChanged);
            // 
            // lblServiceInput
            // 
            this.lblServiceInput.AutoSize = true;
            this.lblServiceInput.Location = new System.Drawing.Point(30, 180);
            this.lblServiceInput.Name = "lblServiceInput";
            this.lblServiceInput.Size = new System.Drawing.Size(50, 13);
            this.lblServiceInput.TabIndex = 10;
            this.lblServiceInput.Text = "Service:";
            // 
            // txtService
            // 
            this.txtService.Location = new System.Drawing.Point(120, 177);
            this.txtService.Name = "txtService";
            this.txtService.Size = new System.Drawing.Size(200, 20);
            this.txtService.TabIndex = 11;
            // 
            // lblSidInput
            // 
            this.lblSidInput.AutoSize = true;
            this.lblSidInput.Location = new System.Drawing.Point(30, 210);
            this.lblSidInput.Name = "lblSidInput";
            this.lblSidInput.Size = new System.Drawing.Size(33, 13);
            this.lblSidInput.TabIndex = 12;
            this.lblSidInput.Text = "SID:";
            // 
            // txtSid
            // 
            this.txtSid.Location = new System.Drawing.Point(120, 207);
            this.txtSid.Name = "txtSid";
            this.txtSid.Size = new System.Drawing.Size(200, 20);
            this.txtSid.TabIndex = 13;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(120, 240);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 14;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(245, 240);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 280); // Increased height
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtSid);
            this.Controls.Add(this.lblSidInput);
            this.Controls.Add(this.txtService);
            this.Controls.Add(this.lblServiceInput);
            this.Controls.Add(this.rbSid);
            this.Controls.Add(this.rbService);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.txtHostname);
            this.Controls.Add(this.lblHostname);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblUsername);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rbService;
        private System.Windows.Forms.RadioButton rbSid;
        private System.Windows.Forms.Label lblServiceInput;
        private System.Windows.Forms.TextBox txtService;
        private System.Windows.Forms.Label lblSidInput;
        private System.Windows.Forms.TextBox txtSid;
        private System.Windows.Forms.Label lblHostname; // New
        private System.Windows.Forms.TextBox txtHostname; // New
        private System.Windows.Forms.Label lblPort; // New
        private System.Windows.Forms.TextBox txtPort; // New
    }
}