namespace OracleUserManagementApp.Models
{
    public class PrivilegeModel
    {
        public string Grantee { get; set; }
        public string ObjectName { get; set; }
        public string PrivilegeName { get; set; }
        public string ColumnName { get; set; }
        public bool WithGrantOption { get; set; }
        public string Type { get; set; } 
    }
}