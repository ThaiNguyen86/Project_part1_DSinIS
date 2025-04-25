using OracleUserManagementApp.Services;
using System;
using System.Windows.Forms;

namespace OracleUserManagementApp.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            var form = new UserRoleManagementForm();
            form.ShowDialog();
        }

        private void btnManagePrivileges_Click(object sender, EventArgs e)
        {
            var form = new PrivilegeManagementForm();
            form.ShowDialog();
        }

        private void btnViewPrivileges_Click(object sender, EventArgs e)
        {
            var form = new ViewPrivilegesForm();
            form.ShowDialog();
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            var service = new OracleService();
            if (service.TestConnection())
            {
                MessageBox.Show("Connection successful!");
            }
        }

        private void btnBackToLogin_Click(object sender, EventArgs e)
        {
            // Close MainForm to return to LoginForm (handled by Program.cs)
            this.Close();
        }
    }
}