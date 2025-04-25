using Oracle.ManagedDataAccess.Client;
using OracleUserManagementApp.Services;
using System;
using System.Windows.Forms;

namespace OracleUserManagementApp.Forms
{
    public partial class UserRoleManagementForm : Form
    {
        private readonly OracleService _oracleService;

        public UserRoleManagementForm()
        {
            InitializeComponent();
            _oracleService = new OracleService();
            LoadUsersAndRoles();
        }

        private void LoadUsersAndRoles()
        {
            lstUsersRoles.Items.Clear();
            var userRoles = _oracleService.GetUsersAndRoles();
            var checkConnection = _oracleService.TestConnection();
            if (userRoles.Count == 0)
            {
                if (checkConnection)
                {
                    lstUsersRoles.Items.Add("No users or roles found.");
                }
                else
                {
                    lstUsersRoles.Items.Add("No users or roles found. Check database connection.");
                }
            }
            else
            {
                foreach (var item in userRoles)
                {
                    lstUsersRoles.Items.Add($"{item.Name} ({item.Type})");
                }
            }
        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            string username = Prompt.ShowDialog("Enter username:", "Create User");
            string password = Prompt.ShowDialog("Enter password:", "Create User");
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username and password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var conn = Utils.ConnectionHelper.GetConnection())
                {
                    conn.Open();
                    string sql = $"CREATE USER {username} IDENTIFIED BY {password}";
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show($"User {username} created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsersAndRoles();
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show($"Error creating user: {ex.Message}\nError Code: {ex.Number}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCreateRole_Click(object sender, EventArgs e)
        {
            string roleName = Prompt.ShowDialog("Enter role name:", "Create Role");
            if (string.IsNullOrWhiteSpace(roleName))
            {
                MessageBox.Show("Role name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var conn = Utils.ConnectionHelper.GetConnection())
                {
                    conn.Open();
                    string sql = $"CREATE ROLE {roleName}";
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show($"Role {roleName} created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsersAndRoles();
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show($"Error creating role: {ex.Message}\nError Code: {ex.Number}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstUsersRoles.SelectedItem == null)
            {
                MessageBox.Show("Please select a user or role to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedItem = lstUsersRoles.SelectedItem.ToString();
            string name = selectedItem.Substring(0, selectedItem.IndexOf('(')).Trim();
            string type = selectedItem.Contains("(USER)") ? "USER" : "ROLE";

            if (MessageBox.Show($"Are you sure you want to delete {type.ToLower()} {name}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                using (var conn = Utils.ConnectionHelper.GetConnection())
                {
                    conn.Open();
                    string sql = type == "USER" ? $"DROP USER {name} CASCADE" : $"DROP ROLE {name}";
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show($"{type} {name} deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsersAndRoles();
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show($"Error deleting {type.ToLower()}: {ex.Message}\nError Code: {ex.Number}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // Lớp hỗ trợ để hiển thị hộp thoại nhập liệu
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 200 };
            Button confirmation = new Button() { Text = "OK", Left = 150, Width = 100, Top = 80, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}