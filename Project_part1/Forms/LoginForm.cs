using Oracle.ManagedDataAccess.Client;
using System;
using System.Windows.Forms;

namespace OracleUserManagementApp.Forms
{
    public partial class LoginForm : Form
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public bool IsSysDba { get; private set; }
        public string ConnectionType { get; private set; } // Service hoặc SID
        public string ServiceOrSidValue { get; private set; }
        public string Hostname { get; private set; } // New property for hostname
        public string Port { get; private set; }     // New property for port

        public LoginForm()
        {
            InitializeComponent();
            rbService.Checked = true; // Mặc định chọn Service
            UpdateInputState();
        }

        private void rbService_CheckedChanged(object sender, EventArgs e)
        {
            UpdateInputState();
        }

        private void rbSid_CheckedChanged(object sender, EventArgs e)
        {
            UpdateInputState();
        }

        private void UpdateInputState()
        {
            txtService.Enabled = rbService.Checked;
            txtSid.Enabled = rbSid.Checked;
            if (rbService.Checked)
            {
                txtService.Focus();
                txtSid.Text = string.Empty; // Xóa nội dung txtSid khi chọn Service
            }
            else
            {
                txtSid.Focus();
                txtService.Text = string.Empty; // Xóa nội dung txtService khi chọn SID
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter username and password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtHostname.Text))
            {
                MessageBox.Show("Please enter hostname.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPort.Text) || !int.TryParse(txtPort.Text.Trim(), out int port) || port <= 0)
            {
                MessageBox.Show("Please enter a valid port number.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string serviceOrSidValue = rbService.Checked ? txtService.Text.Trim() : txtSid.Text.Trim();
            if (string.IsNullOrWhiteSpace(serviceOrSidValue))
            {
                MessageBox.Show($"Please enter {(rbService.Checked ? "Service" : "SID")} value.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Set properties
            Username = txtUsername.Text.Trim();
            Password = txtPassword.Text.Trim();
            IsSysDba = true; // Note: Consider adding a checkbox for IsSysDba in the UI
            ConnectionType = rbService.Checked ? "SERVICE" : "SID";
            ServiceOrSidValue = serviceOrSidValue;
            Hostname = txtHostname.Text.Trim();
            Port = txtPort.Text.Trim();

            // Test connection
            if (TestConnection(Username, Password, IsSysDba, ConnectionType, ServiceOrSidValue, Hostname, Port))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Invalid username, password, or connection details. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool TestConnection(string username, string password, bool isSysDba, string connectionType, string serviceOrSidValue, string hostname, string port)
        {
            try
            {
                string connectDataKey = connectionType == "SERVICE" ? "SERVICE_NAME" : "SID";
                string connectionString = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={hostname})(PORT={port}))(CONNECT_DATA=({connectDataKey}={serviceOrSidValue})));User Id={username};Password={password};";
                if (isSysDba)
                    connectionString += "DBA Privilege=SYSDBA;";

                using (var conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show($"Oracle Error: {ex.Message}\nError Code: {ex.Number}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}