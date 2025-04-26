using Oracle.ManagedDataAccess.Client;
using OracleUserManagementApp.Models;
using OracleUserManagementApp.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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

        // Renames an existing user by dropping and recreating it with preserved roles and privileges
        public void RenameUser(string oldUsername, string newUsername, string password)
        {
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();

                List<PrivilegeModel> privileges = GetPrivileges(oldUsername);

                string dropSql = $"DROP USER {oldUsername} CASCADE";
                using (var dropCmd = new OracleCommand(dropSql, conn))
                {
                    dropCmd.ExecuteNonQuery();
                }

                string createSql = $"CREATE USER {newUsername} IDENTIFIED BY {password}";
                using (var createCmd = new OracleCommand(createSql, conn))
                {
                    createCmd.ExecuteNonQuery();
                }

                foreach (var privilege in privileges)
                {
                    try
                    {
                        if (privilege.Type == "Role")
                        {
                            GrantRoleToUser(privilege.PrivilegeName, newUsername);
                        }
                        else if (privilege.Type == "System Privilege")
                        {
                            GrantPrivilege(newUsername, privilege.PrivilegeName, null, null, false);
                        }
                        else if (privilege.Type == "Object Privilege")
                        {
                            GrantPrivilege(newUsername, privilege.PrivilegeName, privilege.ObjectName, null, privilege.WithGrantOption);
                        }
                        else if (privilege.Type == "Column Privilege")
                        {
                            GrantPrivilege(newUsername, privilege.PrivilegeName, privilege.ObjectName, privilege.ColumnName, privilege.WithGrantOption);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error restoring privilege {privilege.PrivilegeName} for user {newUsername}: {ex.Message}");
                    }
                }
            }
        }

        // Renames an existing role by dropping and recreating it with preserved privileges and grants
        public void RenameRole(string oldRoleName, string newRoleName)
        {
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();

                List<PrivilegeModel> privileges = GetPrivileges(oldRoleName);
                List<string> grantees = new List<string>();
                string granteesSql = @"
                SELECT grantee
                FROM dba_role_privs
                WHERE granted_role = :roleName";
                using (var granteesCmd = new OracleCommand(granteesSql, conn))
                {
                    granteesCmd.Parameters.Add(new OracleParameter("roleName", oldRoleName));
                    using (var reader = granteesCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            grantees.Add(reader["grantee"].ToString());
                        }
                    }
                }

                string dropSql = $"DROP ROLE {oldRoleName}";
                using (var dropCmd = new OracleCommand(dropSql, conn))
                {
                    dropCmd.ExecuteNonQuery();
                }

                string createSql = $"CREATE ROLE {newRoleName}";
                using (var createCmd = new OracleCommand(createSql, conn))
                {
                    createCmd.ExecuteNonQuery();
                }

                foreach (var privilege in privileges)
                {
                    try
                    {
                        if (privilege.Type == "System Privilege")
                        {
                            GrantPrivilege(newRoleName, privilege.PrivilegeName, null, null, false);
                        }
                        else if (privilege.Type == "Object Privilege")
                        {
                            GrantPrivilege(newRoleName, privilege.PrivilegeName, privilege.ObjectName, null, privilege.WithGrantOption);
                        }
                        else if (privilege.Type == "Column Privilege")
                        {
                            GrantPrivilege(newRoleName, privilege.PrivilegeName, privilege.ObjectName, privilege.ColumnName, privilege.WithGrantOption);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error restoring privilege {privilege.PrivilegeName} for role {newRoleName}: {ex.Message}");
                    }
                }

                foreach (var grantee in grantees)
                {
                    try
                    {
                        GrantRoleToUser(newRoleName, grantee);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error granting role {newRoleName} to {grantee}: {ex.Message}");
                    }
                }
            }
        }
        // Changes the password for an existing user
        public void ChangeUserPassword(string username, string newPassword)
        {
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();
                // SQL command to alter the user's password
                string sql = $"ALTER USER {username} IDENTIFIED BY {newPassword}";
                try
                {
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (OracleException ex)
                {
                    throw new Exception($"Error changing password for user {username}: {ex.Message} (Error Code: {ex.Number})", ex);
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
                // Get non-system users
                string sqlUsers = @"
                    SELECT username, account_status 
                    FROM dba_users 
                    WHERE oracle_maintained = 'N' 
                    AND username NOT IN ('SYS', 'SYSTEM', 'DBSNMP', 'OUTLN', 'APPQOSSYS')";
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
                // Get non-system roles
                string sqlRoles = @"
                    SELECT role 
                    FROM dba_roles 
                    WHERE oracle_maintained = 'N' 
                    AND role NOT IN ('DBA', 'CONNECT', 'RESOURCE', 'SYSDBA', 'SYSOPER')";
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

        public List<string> GetRoles()
        {
            var roles = new List<string>();
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
                    SELECT role 
                    FROM dba_roles 
                    WHERE oracle_maintained = 'N' 
                    AND role NOT IN ('DBA', 'CONNECT', 'RESOURCE', 'SYSDBA', 'SYSOPER')";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(reader["role"].ToString());
                        }
                    }
                }
            }
            return roles;
        }

        private string GetSchemaForObject(string objectName)
        {
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
            SELECT owner
            FROM dba_objects
            WHERE object_name = :objectName
            AND object_type IN ('TABLE', 'VIEW', 'PROCEDURE', 'FUNCTION')
            AND owner NOT IN ('SYS', 'SYSTEM', 'DBSNMP', 'OUTLN', 'APPQOSSYS')
            AND oracle_maintained = 'N'";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("objectName", objectName));
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader["owner"].ToString();
                        }
                    }
                }
            }
            throw new ArgumentException($"Object {objectName} not found or is not accessible.");
        }
        public void GrantPrivilege(string grantee, string privilege, string objectName, string columnName, bool withGrantOption)
        {
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql;

                // Check if it's a system privilege or role
                bool isSystemPrivilegeOrRole = new[] {
            "CREATE SESSION", "CREATE TABLE", "CREATE USER", "CREATE ROLE",
            "CREATE VIEW", "CREATE PROCEDURE", "UNLIMITED TABLESPACE",
            "DBA"
        }.Contains(privilege, StringComparer.OrdinalIgnoreCase);

                if (isSystemPrivilegeOrRole)
                {
                    // Grant system privilege or role
                    sql = $"GRANT {privilege} TO {grantee}";
                }
                else
                {
                    // Grant object privilege
                    if (string.IsNullOrEmpty(objectName))
                    {
                        throw new ArgumentException("Object name is required for object privileges.");
                    }

                    // Get the schema (owner) for the object
                    string schema = GetSchemaForObject(objectName);
                    string qualifiedObjectName = $"{schema}.{objectName.Trim()}";

                    // Validate objectName (basic check for valid identifier)
                    if (!System.Text.RegularExpressions.Regex.IsMatch(qualifiedObjectName, @"^[A-Za-z0-9_\.]+$"))
                    {
                        throw new ArgumentException($"Invalid object name: {qualifiedObjectName}");
                    }

                    string grantOption = withGrantOption ? " WITH GRANT OPTION" : "";
                    if (!string.IsNullOrEmpty(columnName))
                    {
                        // Column-level privilege for SELECT or UPDATE
                        if (privilege != "SELECT" && privilege != "UPDATE")
                        {
                            throw new ArgumentException($"Column-level privileges are only supported for SELECT and UPDATE, not {privilege}.");
                        }

                        if (privilege == "SELECT")
                        {
                            // For SELECT, create a view with the specified column
                            string viewName = $"{objectName}_VIEW_{columnName}";
                            string createViewSql = $"CREATE OR REPLACE VIEW {schema}.{viewName} AS SELECT {columnName} FROM {qualifiedObjectName}";
                            System.Diagnostics.Debug.WriteLine($"Creating view: {createViewSql}");
                            try
                            {
                                using (var cmd = new OracleCommand(createViewSql, conn))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            catch (OracleException ex)
                            {
                                throw new Exception($"Oracle error creating view {schema}.{viewName}: {ex.Message} (Error Code: {ex.Number})", ex);
                            }

                            // Grant SELECT on the view
                            sql = $"GRANT SELECT ON {schema}.{viewName} TO {grantee}{grantOption}";
                        }
                        else
                        {
                            // For UPDATE, use column-level grant
                            sql = $"GRANT {privilege} ({columnName}) ON {qualifiedObjectName} TO {grantee}{grantOption}";
                        }
                    }
                    else
                    {
                        // Table-level privilege
                        sql = $"GRANT {privilege} ON {qualifiedObjectName} TO {grantee}{grantOption}";
                    }
                }

                // Log the SQL for debugging
                System.Diagnostics.Debug.WriteLine($"GrantPrivilege SQL: {sql}");

                try
                {
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (OracleException ex)
                {
                    throw new Exception($"Oracle error granting privilege: {ex.Message} (Error Code: {ex.Number})", ex);
                }
            }
        }

        public void RevokePrivilege(string grantee, string privilege, string objectName, string columnName)
        {
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql;

                // Check if it's a system privilege or role
                bool isSystemPrivilegeOrRole = new[] {
            "CREATE SESSION", "CREATE TABLE", "CREATE USER", "CREATE ROLE",
            "CREATE VIEW", "CREATE PROCEDURE", "UNLIMITED TABLESPACE",
            "DBA"
        }.Contains(privilege, StringComparer.OrdinalIgnoreCase);

                if (isSystemPrivilegeOrRole)
                {
                    // Revoke system privilege or role
                    sql = $"REVOKE {privilege} FROM {grantee}";
                }
                else
                {
                    // Revoke object privilege
                    if (string.IsNullOrEmpty(objectName))
                    {
                        throw new ArgumentException("Object name is required for object privileges.");
                    }

                    // Get the schema (owner) for the object
                    string schema = GetSchemaForObject(objectName);
                    string qualifiedObjectName = $"{schema}.{objectName.Trim()}";

                    // Validate objectName
                    if (!System.Text.RegularExpressions.Regex.IsMatch(qualifiedObjectName, @"^[A-Za-z0-9_\.]+$"))
                    {
                        throw new ArgumentException($"Invalid object name: {qualifiedObjectName}");
                    }

                    if (!string.IsNullOrEmpty(columnName))
                    {
                        // Column-level privilege
                        if (privilege != "SELECT" && privilege != "UPDATE")
                        {
                            throw new ArgumentException($"Column-level privileges are only supported for SELECT and UPDATE, not {privilege}.");
                        }

                        if (privilege == "SELECT")
                        {
                            // Check if the objectName is a view created by GrantPrivilege (e.g., ends with _VIEW_{columnName})
                            string baseObjectName = objectName;
                            string targetColumnName = columnName;
                            if (objectName.EndsWith($"_VIEW_{columnName}", StringComparison.OrdinalIgnoreCase))
                            {
                                // Extract the base table name by removing "_VIEW_{columnName}"
                                string suffix = $"_VIEW_{columnName}";
                                baseObjectName = objectName.Substring(0, objectName.Length - suffix.Length);
                                // Recompute the schema for the base object
                                schema = GetSchemaForObject(baseObjectName);
                            }

                            // Construct the correct view name using the base object name
                            string viewName = $"{baseObjectName}_VIEW_{columnName}";
                            string qualifiedViewName = $"{schema}.{viewName}";

                            // Check if the view exists
                            bool viewExists = false;
                            string checkViewSql = @"
                        SELECT COUNT(*) 
                        FROM dba_objects 
                        WHERE object_name = :viewName 
                        AND owner = :schema 
                        AND object_type = 'VIEW'";
                            using (var checkCmd = new OracleCommand(checkViewSql, conn))
                            {
                                checkCmd.Parameters.Add(new OracleParameter("viewName", viewName));
                                checkCmd.Parameters.Add(new OracleParameter("schema", schema));
                                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                                viewExists = count > 0;
                            }

                            if (viewExists)
                            {
                                // Revoke SELECT on the view
                                sql = $"REVOKE SELECT ON {qualifiedViewName} FROM {grantee}";
                                System.Diagnostics.Debug.WriteLine($"Revoking SELECT privilege on view: {qualifiedViewName}");

                                try
                                {
                                    using (var cmd = new OracleCommand(sql, conn))
                                    {
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                catch (OracleException ex)
                                {
                                    throw new Exception($"Oracle error revoking privilege on view {qualifiedViewName}: {ex.Message} (Error Code: {ex.Number})", ex);
                                }

                                // Drop the view after revoking the privilege
                                try
                                {
                                    string dropViewSql = $"DROP VIEW {qualifiedViewName}";
                                    using (var dropCmd = new OracleCommand(dropViewSql, conn))
                                    {
                                        dropCmd.ExecuteNonQuery();
                                        System.Diagnostics.Debug.WriteLine($"Dropped view: {qualifiedViewName}");
                                    }
                                }
                                catch (OracleException ex)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Error dropping view {qualifiedViewName}: {ex.Message} (Error Code: {ex.Number})");
                                }
                            }
                            else
                            {
                                // View does not exist; log a warning and skip revocation
                                System.Diagnostics.Debug.WriteLine($"Warning: View {qualifiedViewName} does not exist. Skipping revocation of SELECT privilege.");
                                return; // Exit the method since there's nothing to revoke
                            }

                            // Exit after handling SELECT privilege to avoid executing the outer REVOKE block
                            return;
                        }
                        else // privilege == "UPDATE"
                        {
                            // For UPDATE, revoke at table level (Oracle limitation: ORA-01750)
                            sql = $"REVOKE {privilege} ON {qualifiedObjectName} FROM {grantee}";
                            System.Diagnostics.Debug.WriteLine("Warning: Revoking UPDATE privilege at table level, as column-level revocation is not supported by Oracle. This will revoke UPDATE on all columns.");
                        }
                    }
                    else
                    {
                        // Table-level privilege
                        sql = $"REVOKE {privilege} ON {qualifiedObjectName} FROM {grantee}";
                    }
                }

                // Log the SQL for debugging
                System.Diagnostics.Debug.WriteLine($"RevokePrivilege SQL: {sql}");

                try
                {
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (OracleException ex)
                {
                    throw new Exception($"Oracle error revoking privilege: {ex.Message} (Error Code: {ex.Number})", ex);
                }
            }
        }

        public void GrantRoleToUser(string role, string user)
        {
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = $"GRANT {role} TO {user}";
                try
                {
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (OracleException ex)
                {
                    throw new Exception($"Oracle error granting role: {ex.Message} (Error Code: {ex.Number})", ex);
                }
            }
        }

        public void RevokeRoleFromUser(string role, string user)
        {
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = $"REVOKE {role} FROM {user}";
                try
                {
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (OracleException ex)
                {
                    throw new Exception($"Oracle error revoking role: {ex.Message} (Error Code: {ex.Number})", ex);
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
            SELECT grantee, table_name, privilege, NULL AS column_name, grantable, 'Object Privilege' AS privilege_type
            FROM dba_tab_privs
            WHERE grantee = :grantee
            UNION
            SELECT grantee, table_name, privilege, column_name, grantable, 'Column Privilege' AS privilege_type
            FROM dba_col_privs
            WHERE grantee = :grantee
            UNION
            SELECT grantee, NULL AS table_name, privilege, NULL AS column_name, 'NO' AS grantable, 'System Privilege' AS privilege_type
            FROM dba_sys_privs
            WHERE grantee = :grantee
            UNION
            SELECT grantee, NULL AS table_name, granted_role AS privilege, NULL AS column_name, 'NO' AS grantable, 'Role' AS privilege_type
            FROM dba_role_privs
            WHERE grantee = :grantee";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("grantee", grantee.ToUpper()));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            privileges.Add(new PrivilegeModel
                            {
                                Grantee = reader["grantee"]?.ToString(),
                                ObjectName = reader["table_name"]?.ToString(),
                                PrivilegeName = reader["privilege"]?.ToString(),
                                ColumnName = reader["column_name"]?.ToString(),
                                WithGrantOption = reader["grantable"]?.ToString() == "YES",
                                Type = reader["privilege_type"]?.ToString()
                            });
                        }
                    }
                }
            }
            return privileges;
        }
        public List<(string Name, string Type)> GetDatabaseObjectsWithType()
        {
            var objects = new List<(string Name, string Type)>();
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();
                string sql = @"
            SELECT object_name, object_type
            FROM dba_objects 
            WHERE object_type IN ('TABLE', 'VIEW', 'PROCEDURE', 'FUNCTION')
            AND owner NOT IN ('SYS', 'SYSTEM', 'DBSNMP', 'OUTLN', 'APPQOSSYS')
            AND oracle_maintained = 'N'";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string objName = reader["object_name"].ToString().Trim();
                            string objType = reader["object_type"].ToString().Trim();
                            if (!string.IsNullOrEmpty(objName))
                            {
                                objects.Add((objName, objType));
                            }
                        }
                    }
                }
            }
            return objects;
        }

        public List<string> GetDatabaseObjects()
        {
            return GetDatabaseObjectsWithType().Select(obj => obj.Name).ToList();
        }

        public List<string> GetColumns(string tableName)
        {
            var columns = new List<string>();
            using (var conn = ConnectionHelper.GetConnection())
            {
                conn.Open();
                // Handle views created for column-level SELECT (e.g., EMPLOYEES_VIEW_DEPTNO)
                string baseTableName = tableName;
                if (tableName.Contains("_VIEW_"))
                {
                    // Extract the base table name (e.g., "EMPLOYEES" from "EMPLOYEES_VIEW_DEPTNO")
                    int viewIndex = tableName.IndexOf("_VIEW_");
                    if (viewIndex >= 0)
                    {
                        baseTableName = tableName.Substring(0, viewIndex);
                    }
                }

                // Get the schema (owner) for the table
                string owner;
                try
                {
                    owner = GetSchemaForObject(baseTableName);
                }
                catch (ArgumentException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to get schema for table {baseTableName}: {ex.Message}");
                    return columns; // Return empty list if object is not found
                }

                string sql = @"
            SELECT column_name 
            FROM dba_tab_columns 
            WHERE table_name = :tableName
            AND owner = :owner";
                using (var cmd = new OracleCommand(sql, conn))
                {
                    cmd.Parameters.Add(new OracleParameter("tableName", baseTableName));
                    cmd.Parameters.Add(new OracleParameter("owner", owner));
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
            catch (OracleException ex)
            {
                System.Windows.Forms.MessageBox.Show($"Connection failed: {ex.Message} (Error Code: {ex.Number})");
                return false;
            }
        }
    }
}