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
            SetupDataGridView();
            LoadGrantees();
        }

        private void SetupDataGridView()
        {
            // Clear any existing columns
            dgvPrivileges.Columns.Clear();

            // Add columns to the DataGridView
            dgvPrivileges.Columns.Add("Type", "Type");
            dgvPrivileges.Columns.Add("Privilege", "Privilege");
            dgvPrivileges.Columns.Add("Object", "Object");
            dgvPrivileges.Columns.Add("Column", "Column");
            dgvPrivileges.Columns.Add("WithGrantOption", "With Grant Option");

            // Optional: Set column widths or other properties
            dgvPrivileges.Columns["Type"].Width = 100;
            dgvPrivileges.Columns["Privilege"].Width = 100;
            dgvPrivileges.Columns["Object"].Width = 150;
            dgvPrivileges.Columns["Column"].Width = 100;
            dgvPrivileges.Columns["WithGrantOption"].Width = 100;

            // Enable sorting
            foreach (DataGridViewColumn column in dgvPrivileges.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }

            // Optional: Add alternating row colors for better readability
            dgvPrivileges.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
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
                // Clear existing rows
                dgvPrivileges.Rows.Clear();

                // Get privileges for the selected grantee
                var privileges = _oracleService.GetPrivileges(cmbGrantee.SelectedItem.ToString());

                // Populate the DataGridView
                foreach (var priv in privileges)
                {
                    dgvPrivileges.Rows.Add(
                        priv.Type,
                        priv.PrivilegeName,
                        priv.ObjectName ?? "N/A",
                        priv.ColumnName ?? "N/A",
                        priv.WithGrantOption ? "Yes" : "No"
                    );
                }

                // Refresh the DataGridView
                dgvPrivileges.Refresh();
            }
        }
    }
}