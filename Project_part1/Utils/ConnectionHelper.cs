using Oracle.ManagedDataAccess.Client;

namespace OracleUserManagementApp.Utils
{
    public static class ConnectionHelper
    {
        private static string _username;
        private static string _password;
        private static bool _isSysDba;
        private static string _connectionType; // SERVICE hoặc SID
        private static string _serviceOrSidValue;

        public static void SetCredentials(string username, string password, bool isSysDba, string connectionType, string serviceOrSidValue)
        {
            _username = username;
            _password = password;
            _isSysDba = isSysDba;
            _connectionType = connectionType;
            _serviceOrSidValue = serviceOrSidValue;
        }

        public static OracleConnection GetConnection()
        {
            string connectDataKey = _connectionType == "SERVICE" ? "SERVICE_NAME" : "SID";
            string connectionString = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=({connectDataKey}={_serviceOrSidValue})));User Id={_username};Password={_password};";
            if (_isSysDba)
                connectionString += "DBA Privilege=SYSDBA;";
            return new OracleConnection(connectionString);
        }
    }
}