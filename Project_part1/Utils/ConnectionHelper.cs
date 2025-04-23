using System;
using System.Configuration;
using Oracle.ManagedDataAccess.Client;

namespace OracleUserManagementApp.Utils
{
    public static class ConnectionHelper
    {
        public static OracleConnection GetConnection()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["OracleConnection"]?.ConnectionString;
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new ConfigurationErrorsException("Connection string 'OracleConnection' not found in App.config.");
                }
                return new OracleConnection(connectionString);
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new Exception("Failed to load connection string from App.config.", ex);
            }
        }
    }
}