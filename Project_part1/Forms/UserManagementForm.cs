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
            // Prompt for username and password in a single dialog
            var (username, password) = Prompt.ShowDualInputDialog("Username:", "Password:", "Create User");
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username and password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Use OracleService to create the user
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
        // Event handler for the Modify button
        private void btnModify_Click(object sender, EventArgs e)
        {
            // Check if a row is selected
            if (dgvUsersRoles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user or role to modify.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected row
            DataGridViewRow selectedRow = dgvUsersRoles.SelectedRows[0];
            string name = selectedRow.Cells["Name"].Value.ToString();
            string type = selectedRow.Cells["Type"].Value.ToString();

            try
            {
                if (type == "USER")
                {
                    // For users, prompt for new username and password
                    var (newUsername, newPassword) = Prompt.ShowDualInputDialog("New Username:", "New Password:", $"Modify User {name}");
                    if (string.IsNullOrWhiteSpace(newUsername) || string.IsNullOrWhiteSpace(newPassword))
                    {
                        MessageBox.Show("New username and password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Confirm the modification
                    if (MessageBox.Show($"Are you sure you want to modify user {name} to {newUsername} with a new password?", "Confirm Modify", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;

                    // If the username has changed, rename the user; otherwise, just change the password
                    if (newUsername != name)
                    {
                        _oracleService.RenameUser(name, newUsername, newPassword);
                        MessageBox.Show($"User {name} modified to {newUsername} successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        _oracleService.ChangeUserPassword(name, newPassword);
                        MessageBox.Show($"Password for user {name} changed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    LoadUsersAndRoles(); // Refresh the grid
                }
                else // type == "ROLE"
                {
                    // For roles, prompt for a new role name
                    string newRoleName = Prompt.ShowDialog("Enter new role name:", $"Modify Role {name}");
                    if (string.IsNullOrWhiteSpace(newRoleName))
                    {
                        MessageBox.Show("New role name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Confirm the modification
                    if (MessageBox.Show($"Are you sure you want to rename role {name} to {newRoleName}?", "Confirm Modify", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;

                    // Rename the role
                    _oracleService.RenameRole(name, newRoleName);
                    MessageBox.Show($"Role {name} renamed to {newRoleName} successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsersAndRoles(); // Refresh the grid
                }
            }
            catch (OracleException ex)
            {
                MessageBox.Show($"Error modifying {type.ToLower()}: {ex.Message}\nError Code: {ex.Number}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    public static class Prompt
    {
        // Displays a dialog to prompt the user for a single input
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 300,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label()
            {
                Left = 50,
                Top = 20,
                Text = text,
                Width = 200,
                AutoSize = false,
                MaximumSize = new System.Drawing.Size(200, 0),
                Height = 40
            };
            TextBox textBox = new TextBox() { Left = 50, Top = 60, Width = 200 };
            Button confirmation = new Button() { Text = "OK", Left = 150, Width = 100, Top = 90, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        // Displays a dialog to prompt the user for two inputs (e.g., username and password)
        public static (string input1, string input2) ShowDualInputDialog(string label1, string label2, string caption)
        {
            Form prompt = new Form()
            {
                Width = 350,
                Height = 220,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label labelInput1 = new Label()
            {
                Left = 50,
                Top = 20,
                Text = label1,
                Width = 200,
                AutoSize = false,
                MaximumSize = new System.Drawing.Size(200, 0),
                Height = 20
            };
            TextBox textBox1 = new TextBox() { Left = 50, Top = 40, Width = 250 };
            Label labelInput2 = new Label()
            {
                Left = 50,
                Top = 70,
                Text = label2,
                Width = 200,
                AutoSize = false,
                MaximumSize = new System.Drawing.Size(200, 0),
                Height = 20
            };
            TextBox textBox2 = new TextBox() { Left = 50, Top = 90, Width = 250, UseSystemPasswordChar = true }; // Password field
            Button confirmation = new Button() { Text = "OK", Left = 150, Width = 100, Top = 130, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(labelInput1);
            prompt.Controls.Add(textBox1);
            prompt.Controls.Add(labelInput2);
            prompt.Controls.Add(textBox2);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? (textBox1.Text, textBox2.Text) : (string.Empty, string.Empty);
        }
    }
}