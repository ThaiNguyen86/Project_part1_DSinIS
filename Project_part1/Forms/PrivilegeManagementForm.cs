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
            // Re-attach the event handler for Grantee changes
            cmbGrantee.SelectedIndexChanged += new EventHandler(cmbGrantee_SelectedIndexChanged);
        }
        // Extracts the raw name from a ComboBox item (removes the type in parentheses)
        private string ExtractRawName(string displayText)
        {
            if (string.IsNullOrEmpty(displayText)) return displayText;
            int index = displayText.IndexOf(" (");
            return index >= 0 ? displayText.Substring(0, index) : displayText;
        }
        private void LoadGrantees()
        {
            cmbGrantee.Items.Clear();
            var userRoles = _oracleService.GetUsersAndRoles();
            foreach (var item in userRoles)
            {
                // Display the grantee with its type in parentheses, e.g., "TEST_USER1 (USER)"
                string displayText = $"{item.Name} ({item.Type})";
                cmbGrantee.Items.Add(displayText);
            }
        }

        private void LoadObjects()
        {
            // Preserve the current selection
            string selectedObject = ExtractRawName(cmbObject.SelectedItem?.ToString());

            cmbObject.Items.Clear();
            var objects = _oracleService.GetDatabaseObjectsWithType();
            foreach (var obj in objects)
            {
                // Display the object with its type in parentheses, e.g., "EMPLOYEES (TABLE)"
                string displayText = $"{obj.Name} ({obj.Type})";
                cmbObject.Items.Add(displayText);
            }
            cmbObject.Items.Add("N/A");

            // Restore the previous selection if it still exists
            if (!string.IsNullOrEmpty(selectedObject))
            {
                var matchingItem = cmbObject.Items.Cast<string>().FirstOrDefault(item => ExtractRawName(item) == selectedObject);
                if (matchingItem != null)
                {
                    cmbObject.SelectedItem = matchingItem;
                }
            }
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
            "DBA"
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

        private void cmbGrantee_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Reset other controls when Grantee changes, but keep Grantee selection
            cmbObject.SelectedItem = null;
            cmbPrivilege.SelectedItem = null;
            cmbColumn.Items.Clear();
            cmbColumn.SelectedItem = null;
            cmbColumn.Text = ""; // Explicitly clear the Text property
            chkWithGrantOption.Checked = false;
            cmbRole.SelectedItem = null;
            cmbObject.Enabled = true;
            cmbColumn.Enabled = true;
            chkWithGrantOption.Enabled = true;
        }

        private void cmbPrivilege_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPrivilege.SelectedItem == null) return;

            string privilege = cmbPrivilege.SelectedItem.ToString();
            bool isObjectPrivilege = new[] { "SELECT", "INSERT", "UPDATE", "DELETE", "EXECUTE" }
                .Contains(privilege, StringComparer.OrdinalIgnoreCase);

            if (isObjectPrivilege)
            {
                // Enable controls for object privileges and preserve current object selection
                cmbObject.Enabled = true;
                cmbColumn.Enabled = true;
                chkWithGrantOption.Enabled = true;

                // Reload columns if an object is selected
                if (cmbObject.SelectedItem != null && cmbObject.SelectedItem.ToString() != "N/A")
                {
                    string objectName = ExtractRawName(cmbObject.SelectedItem.ToString());
                    var columns = _oracleService.GetColumns(objectName);
                    if (columns.Count == 0)
                    {
                        // Object might have been dropped or is inaccessible; refresh the object list
                        MessageBox.Show($"The object '{objectName}' is no longer accessible. Refreshing the object list.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LoadObjects();
                        cmbObject.SelectedItem = "N/A"; // Reset the selection
                        cmbColumn.Items.Clear();
                        cmbColumn.SelectedItem = null;
                        cmbColumn.Text = "";
                        return;
                    }

                    // Populate columns if available
                    cmbColumn.Items.Clear();
                    cmbColumn.SelectedItem = null;
                    cmbColumn.Text = "";
                    foreach (var col in columns)
                    {
                        cmbColumn.Items.Add(col);
                    }
                }
            }
            else
            {
                // Disable controls and set to "N/A" for system privileges/roles
                cmbObject.Enabled = false;
                cmbColumn.Enabled = false;
                chkWithGrantOption.Enabled = false;
                chkWithGrantOption.Checked = false;
                cmbObject.SelectedItem = "N/A";
                cmbColumn.Items.Clear();
                cmbColumn.SelectedItem = null;
                cmbColumn.Text = "";
            }
        }

        private void cmbObject_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Reset column selection when object changes
            cmbColumn.Items.Clear();
            cmbColumn.SelectedItem = null;
            cmbColumn.Text = "";

            if (cmbObject.SelectedItem != null && cmbObject.SelectedItem.ToString() != "N/A")
            {
                string objectName = ExtractRawName(cmbObject.SelectedItem.ToString());
                var columns = _oracleService.GetColumns(objectName);
                if (columns.Count == 0)
                {
                    // Object might have been dropped or is inaccessible; refresh the object list
                    MessageBox.Show($"The object '{objectName}' is no longer accessible. Refreshing the object list.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LoadObjects();
                    cmbObject.SelectedItem = "N/A";
                    return;
                }

                // Populate columns if available
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
                string grantee = ExtractRawName(cmbGrantee.SelectedItem.ToString());
                string objectName = ExtractRawName(cmbObject.SelectedItem?.ToString());
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
                    if (!string.IsNullOrEmpty(columnName) && privilege != "SELECT" && privilege != "UPDATE")
                    {
                        MessageBox.Show("Column-level privileges are only supported for SELECT and UPDATE.");
                        return;
                    }
                }
                else
                {
                    // System privileges/roles don't need object or column
                    objectName = null;
                    columnName = null;
                    withGrantOption = false; // System privileges don't support WITH GRANT OPTION
                }

                _oracleService.GrantPrivilege(grantee, privilege, objectName, columnName, withGrantOption);
                MessageBox.Show($"Privilege {privilege} granted to {grantee} successfully!");

                // Refresh the object list if a view might have been created (SELECT with column)
                if (privilege == "SELECT" && !string.IsNullOrEmpty(columnName))
                {
                    LoadObjects();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error granting privilege: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                string grantee = ExtractRawName(cmbGrantee.SelectedItem.ToString());
                string objectName = ExtractRawName(cmbObject.SelectedItem?.ToString());
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
                    if (!string.IsNullOrEmpty(columnName) && privilege != "SELECT" && privilege != "UPDATE")
                    {
                        MessageBox.Show("Column-level privileges are only supported for SELECT and UPDATE.");
                        return;
                    }
                    // Warn user if revoking UPDATE with a column selected
                    if (privilege == "UPDATE" && !string.IsNullOrEmpty(columnName))
                    {
                        DialogResult result = MessageBox.Show(
                            "Revoking the UPDATE privilege will apply to the entire table, not just the selected column. Continue?",
                            "Warning",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);
                        if (result == DialogResult.No)
                        {
                            return;
                        }
                    }
                    // Optional: Warn user if revoking SELECT with a column and selecting a view
                    if (privilege == "SELECT" && !string.IsNullOrEmpty(columnName) && objectName.EndsWith($"_VIEW_{columnName}", StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show("You have selected a view. The revocation will apply to the base table associated with this view.");
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

                // Refresh the object list if a view might have been dropped (SELECT with column)
                if (privilege == "SELECT" && !string.IsNullOrEmpty(columnName))
                {
                    LoadObjects();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error revoking privilege: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                string user = ExtractRawName(cmbGrantee.SelectedItem.ToString());
                _oracleService.GrantRoleToUser(role, user);
                MessageBox.Show($"Role {role} granted to {user} successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error granting role: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRevokeRole_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbRole.SelectedItem == null || cmbGrantee.SelectedItem == null)
                {
                    MessageBox.Show("Please select a role and a grantee.");
                    return;
                }

                string role = cmbRole.SelectedItem.ToString();
                string user = ExtractRawName(cmbGrantee.SelectedItem.ToString());
                _oracleService.RevokeRoleFromUser(role, user);
                MessageBox.Show($"Role {role} revoked from {user} successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error revoking role: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}