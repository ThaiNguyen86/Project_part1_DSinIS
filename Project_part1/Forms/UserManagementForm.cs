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
            SetupDataGridView();
            LoadUsersAndRoles();
        }

        private void SetupDataGridView()
        {
            // Clear any existing columns
            dgvUsersRoles.Columns.Clear();

            // Add columns to the DataGridView
            dgvUsersRoles.Columns.Add("Name", "Name");
            dgvUsersRoles.Columns.Add("Type", "Type");
            dgvUsersRoles.Columns.Add("Status", "Status");

            // Optional: Set column widths or other properties
            dgvUsersRoles.Columns["Name"].Width = 150;
            dgvUsersRoles.Columns["Type"].Width = 100;
            dgvUsersRoles.Columns["Status"].Width = 100;

            // Enable sorting
            foreach (DataGridViewColumn column in dgvUsersRoles.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }

            // Optional: Add alternating row colors for better readability
            dgvUsersRoles.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
        }

        private void LoadUsersAndRoles()
        {
            // Clear existing rows
            dgvUsersRoles.Rows.Clear();

            var userRoles = _oracleService.GetUsersAndRoles();
            var checkConnection = _oracleService.TestConnection();
            if (userRoles.Count == 0)
            {
                if (checkConnection)
                {
                    MessageBox.Show("No users or roles found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No users or roles found. Check database connection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                foreach (var item in userRoles)
                {
                    dgvUsersRoles.Rows.Add(
                        item.Name,
                        item.Type,
                        item.Type == "USER" ? item.Status : "N/A"
                    );
                }
            }

            // Refresh the DataGridView
            dgvUsersRoles.Refresh();
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
                // Use OracleService instead of direct connection
                _oracleService.CreateUser(new Models.UserRoleModel
                {
                    Name = username,
                    Password = password,
                    Type = "USER"
                });
                MessageBox.Show($"User {username} created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsersAndRoles();
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
                _oracleService.CreateRole(roleName);
                MessageBox.Show($"Role {roleName} created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsersAndRoles();
            }
            catch (OracleException ex)
            {
                MessageBox.Show($"Error creating role: {ex.Message}\nError Code: {ex.Number}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsersRoles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user or role to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected row
            DataGridViewRow selectedRow = dgvUsersRoles.SelectedRows[0];
            string name = selectedRow.Cells["Name"].Value.ToString();
            string type = selectedRow.Cells["Type"].Value.ToString();

            if (MessageBox.Show($"Are you sure you want to delete {type.ToLower()} {name}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                if (type == "USER")
                {
                    _oracleService.DeleteUser(name);
                }
                else // type == "ROLE"
                {
                    _oracleService.DeleteRole(name);
                }
                MessageBox.Show($"{type} {name} deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsersAndRoles();
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