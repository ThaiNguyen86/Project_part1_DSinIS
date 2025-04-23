using OracleUserManagementApp.Services;
using System;
using System.Windows.Forms;

namespace OracleUserManagementApp.Forms
{
    public partial class ViewPrivilegesForm : Form
    {
        private OracleService _oracleService;

        public ViewPrivilegesForm()
        {
            InitializeComponent();
            _oracleService = new OracleService();
            LoadGrantees();
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

        private void cmbGrantee_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGrantee.SelectedItem != null)
            {
                lstPrivileges.Items.Clear();
                var privileges = _oracleService.GetPrivileges(cmbGrantee.SelectedItem.ToString());
                foreach (var priv in privileges)
                {
                    string privilegeInfo = $"{priv.PrivilegeName} on {priv.ObjectName}";
                    if (!string.IsNullOrEmpty(priv.ColumnName))
                    {
                        privilegeInfo += $" ({priv.ColumnName})";
                    }
                    if (priv.WithGrantOption)
                    {
                        privilegeInfo += " WITH GRANT OPTION";
                    }
                    lstPrivileges.Items.Add(privilegeInfo);
                }
            }
        }
    }
}