using OracleUserManagementApp.Models;
using OracleUserManagementApp.Services;
using System;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OracleUserManagementApp.Forms
{
    public partial class UserRoleManagementForm : Form
    {
        private OracleService _oracleService;

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
            foreach (var item in userRoles)
            {
                lstUsersRoles.Items.Add($"{item.Name} ({item.Type})");
            }
        }

        private void btnCreateUser_Click(object sender, EventArgs e)
        {
            try
            {
                var user = new UserRoleModel
                {
                    Name = txtName.Text,
                    Password = txtPassword.Text,
                    Type = "USER"
                };
                _oracleService.CreateUser(user);
                MessageBox.Show("User created successfully!");
                LoadUsersAndRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnCreateRole_Click(object sender, EventArgs e)
        {
            try
            {
                _oracleService.CreateRole(txtName.Text);
                MessageBox.Show("Role created successfully!");
                LoadUsersAndRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstUsersRoles.SelectedItem != null)
            {
                try
                {
                    string[] parts = lstUsersRoles.SelectedItem.ToString().Split(' ');
                    string name = parts[0];
                    string type = parts[1].Trim('(', ')');
                    if (type == "USER")
                    {
                        _oracleService.DeleteUser(name);
                        MessageBox.Show("User deleted successfully!");
                    }
                    else
                    {
                        _oracleService.DeleteRole(name);
                        MessageBox.Show("Role deleted successfully!");
                    }
                    LoadUsersAndRoles();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
    }
}