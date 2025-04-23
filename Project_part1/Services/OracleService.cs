using Oracle.ManagedDataAccess.Client;
using OracleUserManagementApp.Models;
using OracleUserManagementApp.Utils;
using System;
using System.Collections.Generic;
using System.Data;

namespace OracleUserManagementApp.Services
{
    public class OracleService
    {
        public void CreateUser(UserRoleModel user)
        {
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = $"CREATE USER {user.Name} IDENTIFIED BY {user.Password}";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUser(string username)
        {
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = $"DROP USER {username} CASCADE";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CreateRole(string roleName)
        {
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = $"CREATE ROLE {roleName}";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteRole(string roleName)
        {
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = $"DROP ROLE {roleName}";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<UserRoleModel> GetUsersAndRoles()
        {
            var userRoles = new List<UserRoleModel>();
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();
                // Lấy users
                string sqlUsers = "SELECT username, account_status FROM dba_users";
                using (var cmd = new OracleCommand(sqlUsers, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userRoles.Add(new UserRoleModel
                            {
                                Name = reader["username"].ToString(),
                                Type = "USER",
                                Status = reader["account_status"].ToString()
                            });
                        }
                    }
                }
                // Lấy roles
                string sqlRoles = "SELECT role FROM dba_roles";
                using (var cmd = new OracleCommand(sqlRoles, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userRoles.Add(new UserRoleModel
                            {
                                Name = reader["role"].ToString(),
                                Type = "ROLE"
                            });
                        }
                    }
                }
            }
            return userRoles;
        }

        public void GrantPrivilege(string grantee, string privilege, string objectName, string columnName, bool withGrantOption)
        {
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();
                string grantOption = withGrantOption ? " WITH GRANT OPTION" : "";
                string sql = "";
                if (!string.IsNullOrEmpty(columnName))
                {
                    sql = $"GRANT {privilege} ({columnName}) ON {objectName} TO {grantee}{grantOption}";
                }
                else
                {
                    sql = $"GRANT {privilege} ON {objectName} TO {grantee}{grantOption}";
                }
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void GrantRoleToUser(string role, string user)
        {
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = $"GRANT {role} TO {user}";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RevokePrivilege(string grantee, string privilege, string objectName, string columnName)
        {
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = "";
                if (!string.IsNullOrEmpty(columnName))
                {
                    sql = $"REVOKE {privilege} ({columnName}) ON {objectName} FROM {grantee}";
                }
                else
                {
                    sql = $"REVOKE {privilege} ON {objectName} FROM {grantee}";
                }
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<PrivilegeModel> GetPrivileges(string grantee)
        {
            var privileges = new List<PrivilegeModel>();
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                    SELECT grantee, table_name, privilege, column_name, grantable
                    FROM dba_tab_privs
                    WHERE grantee = :grantee
                    UNION
                    SELECT grantee, table_name, privilege, column_name, grantable
                    FROM dba_col_privs
                    WHERE grantee = :grantee";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("grantee", grantee));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            privileges.Add(new PrivilegeModel
                            {
                                Grantee = reader["grantee"].ToString(),
                                ObjectName = reader["table_name"].ToString(),
                                PrivilegeName = reader["privilege"].ToString(),
                                ColumnName = reader["column_name"].ToString(),
                                WithGrantOption = reader["grantable"].ToString() == "YES"
                            });
                        }
                    }
                }
            }
            return privileges;
        }

        public List<string> GetDatabaseObjects()
        {
            var objects = new List<string>();
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = "SELECT object_name FROM dba_objects WHERE object_type IN ('TABLE', 'VIEW', 'PROCEDURE', 'FUNCTION')";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            objects.Add(reader["object_name"].ToString());
                        }
                    }
                }
            }
            return objects;
        }

        public List<string> GetColumns(string tableName)
        {
            var columns = new List<string>();
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = $"SELECT column_name FROM dba_tab_columns WHERE table_name = :tableName";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("tableName", tableName));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            columns.Add(reader["column_name"].ToString());
                        }
                    }
                }
            }
            return columns;
        }
        public bool TestConnection()
        {
            try
            {
                using (var conn = ConnectionHelper.GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show($"Connection failed: {ex.Message}");
                return false;
            }
        }
    }
}
