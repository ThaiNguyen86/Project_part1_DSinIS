using OracleUserManagementApp.Services;
using System;
using System.Linq;
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
            LoadPrivileges();
            LoadRoles();
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
            // Add "N/A" for system privileges/roles
            cmbObject.Items.Add("N/A");
        }

        private void LoadPrivileges()
        {
            cmbPrivilege.Items.Clear();
            // Object privileges
            cmbPrivilege.Items.AddRange(new object[]
            {
                "SELECT",
                "INSERT",
                "UPDATE",
                "DELETE",
                "EXECUTE"
            });
            // System privileges and roles
            cmbPrivilege.Items.AddRange(new object[]
            {
                "CREATE SESSION",
                "CREATE TABLE",
                "CREATE USER",
                "CREATE ROLE",
                "CREATE VIEW",
                "CREATE PROCEDURE",
                "UNLIMITED TABLESPACE",
                "DBA",
                "SYSDBA",
                "SYSOPER"
            });
            cmbPrivilege.SelectedIndexChanged += new EventHandler(cmbPrivilege_SelectedIndexChanged);
        }

        private void LoadRoles()
        {
            cmbRole.Items.Clear();
            var roles = _oracleService.GetRoles();
            foreach (var role in roles)
            {
                cmbRole.Items.Add(role);
            }
        }

        private void cmbPrivilege_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPrivilege.SelectedItem == null) return;

            string privilege = cmbPrivilege.SelectedItem.ToString();
            bool isObjectPrivilege = new[] { "SELECT", "INSERT", "UPDATE", "DELETE", "EXECUTE" }
                .Contains(privilege, StringComparer.OrdinalIgnoreCase);

            if (isObjectPrivilege)
            {
                cmbObject.Enabled = true;
                cmbColumn.Enabled = true;
                chkWithGrantOption.Enabled = true;
                cmbObject.SelectedItem = null; // Allow object selection
            }
            else
            {
                cmbObject.Enabled = false;
                cmbColumn.Enabled = false;
                chkWithGrantOption.Enabled = false;
                chkWithGrantOption.Checked = false;
                cmbObject.SelectedItem = "N/A"; // Set to "N/A" for system privileges/roles
                cmbColumn.Items.Clear();
            }
        }

        private void cmbObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbColumn.Items.Clear();
            if (cmbObject.SelectedItem != null && cmbObject.SelectedItem.ToString() != "N/A")
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
                if (cmbGrantee.SelectedItem == null || cmbPrivilege.SelectedItem == null)
                {
                    MessageBox.Show("Please select a grantee and privilege.");
                    return;
                }

                string privilege = cmbPrivilege.SelectedItem.ToString();
                string grantee = cmbGrantee.SelectedItem.ToString();
                string objectName = cmbObject.SelectedItem?.ToString();
                string columnName = cmbColumn.SelectedItem?.ToString();
                bool withGrantOption = chkWithGrantOption.Checked;

                // Check if it's an object privilege
                bool isObjectPrivilege = new[] { "SELECT", "INSERT", "UPDATE", "DELETE", "EXECUTE" }
                    .Contains(privilege, StringComparer.OrdinalIgnoreCase);

                if (isObjectPrivilege)
                {
                    if (string.IsNullOrEmpty(objectName) || objectName == "N/A")
                    {
                        MessageBox.Show("Please select a valid database object for this privilege.");
                        return;
                    }
                    if ((privilege == "SELECT" || privilege == "UPDATE") && string.IsNullOrEmpty(columnName))
                    {
                        MessageBox.Show("Please select a column for SELECT or UPDATE privilege.");
                        return;
                    }
                }
                else
                {
                    // System privileges/roles don't need object or column
                    objectName = null;
                    columnName = null;
                    withGrantOption = false; // SYSDBA, SYSOPER, DBA don't support WITH GRANT OPTION
                }

                _oracleService.GrantPrivilege(grantee, privilege, objectName, columnName, withGrantOption);
                MessageBox.Show($"Privilege {privilege} granted to {grantee} successfully!");
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
                if (cmbGrantee.SelectedItem == null || cmbPrivilege.SelectedItem == null)
                {
                    MessageBox.Show("Please select a grantee and privilege.");
                    return;
                }

                string privilege = cmbPrivilege.SelectedItem.ToString();
                string grantee = cmbGrantee.SelectedItem.ToString();
                string objectName = cmbObject.SelectedItem?.ToString();
                string columnName = cmbColumn.SelectedItem?.ToString();

                // Check if it's an object privilege
                bool isObjectPrivilege = new[] { "SELECT", "INSERT", "UPDATE", "DELETE", "EXECUTE" }
                    .Contains(privilege, StringComparer.OrdinalIgnoreCase);

                if (isObjectPrivilege)
                {
                    if (string.IsNullOrEmpty(objectName) || objectName == "N/A")
                    {
                        MessageBox.Show("Please select a valid database object for this privilege.");
                        return;
                    }
                }
                else
                {
                    // System privileges/roles don't need object or column
                    objectName = null;
                    columnName = null;
                }

                _oracleService.RevokePrivilege(grantee, privilege, objectName, columnName);
                MessageBox.Show($"Privilege {privilege} revoked from {grantee} successfully!");
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
                if (cmbRole.SelectedItem == null || cmbGrantee.SelectedItem == null)
                {
                    MessageBox.Show("Please select a role and a grantee.");
                    return;
                }

                string role = cmbRole.SelectedItem.ToString();
                string user = cmbGrantee.SelectedItem.ToString();
                _oracleService.GrantRoleToUser(role, user);
                MessageBox.Show($"Role {role} granted to {user} successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}