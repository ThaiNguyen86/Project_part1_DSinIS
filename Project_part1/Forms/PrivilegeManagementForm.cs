using OracleUserManagementApp.Services;
using System;
using System.Data.Common;
using System.Windows.Forms;

namespace OracleUserManagementApp.Forms
{
    public partial class PrivilegeManagementForm : Form
    {
        private OracleService _oracleService;

        public PrivilegeManagementForm()
        {
            InitializeComponent();
            _oracleService = new OracleService();
            LoadGrantees();
            LoadObjects();
        }

        private void LoadGrantees()
        {
            cmbGrantee.Items.Clear();
            var userRoles = _oracleService.GetUsersAndRoles();
            foreach (var item in userRoles)
            {
                cmbGrantee.Items.Add(item.Name);
            }
        }

        private void LoadObjects()
        {
            cmbObject.Items.Clear();
            var objects = _oracleService.GetDatabaseObjects();
            foreach (var obj in objects)
            {
                cmbObject.Items.Add(obj);
            }
        }

        private void cmbObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbColumn.Items.Clear();
            if (cmbObject.SelectedItem != null)
            {
                var columns = _oracleService.GetColumns(cmbObject.SelectedItem.ToString());
                foreach (var col in columns)
                {
                    cmbColumn.Items.Add(col);
                }
            }
        }

        private void btnGrant_Click(object sender, EventArgs e)
        {
            try
            {
                string privilege = cmbPrivilege.SelectedItem.ToString();
                string grantee = cmbGrantee.SelectedItem.ToString();
                string objectName = cmbObject.SelectedItem.ToString();
                string columnName = cmbColumn.SelectedItem?.ToString();
                bool withGrantOption = chkWithGrantOption.Checked;

                if ((privilege == "SELECT" || privilege == "UPDATE") && string.IsNullOrEmpty(columnName))
                {
                    MessageBox.Show("Please select a column for SELECT or UPDATE privilege.");
                    return;
                }

                _oracleService.GrantPrivilege(grantee, privilege, objectName, columnName, withGrantOption);
                MessageBox.Show("Privilege granted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnRevoke_Click(object sender, EventArgs e)
        {
            try
            {
                string privilege = cmbPrivilege.SelectedItem.ToString();
                string grantee = cmbGrantee.SelectedItem.ToString();
                string objectName = cmbObject.SelectedItem.ToString();
                string columnName = cmbColumn.SelectedItem?.ToString();

                _oracleService.RevokePrivilege(grantee, privilege, objectName, columnName);
                MessageBox.Show("Privilege revoked successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnGrantRole_Click(object sender, EventArgs e)
        {
            try
            {
                string role = txtRole.Text;
                string user = cmbGrantee.SelectedItem.ToString();
                _oracleService.GrantRoleToUser(role, user);
                MessageBox.Show("Role granted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}