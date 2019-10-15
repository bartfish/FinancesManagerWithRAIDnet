using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;

namespace HostCommunication.Managers
{
    public static class DbManager
    {
        public static void InitializeBackup()
        {
            
            if (!CheckDatabaseExistence())
            {
                RunSqlOnDb(ConfigurationManager.AppSettings["sqlCreateBackupDb"]);
            }
        }

        private static void RunSqlOnDb(string sqlDirectory)
        {
            string script = File.ReadAllText(sqlDirectory);
            using (var conn = ServerManager.EstablishBackupServerConn())
            {
                try
                {
                    IEnumerable<string> splitScript = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

                    conn.Open();
                    foreach (string splitted in splitScript)
                    {
                        if (!string.IsNullOrEmpty(splitted.Trim()))
                        {
                            using (var command = new SqlCommand(splitted, conn))
                            {
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    conn.Close();

                }
                catch (Exception)
                {

                    throw;
                }

            }
        }

        private static bool CheckDatabaseExistence()
        {
            bool dbExists = false;
            int dbId = 0; 
            try
            {
                
                string sqlCreateDBQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", ConfigurationManager.AppSettings["DbBackupName"]);

                using (var conn = ServerManager.EstablishBackupServerConn())
                {
                    using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, conn))
                    {
                        conn.Open();

                        var foundRow = sqlCmd.ExecuteScalar();

                        if (foundRow != null)
                        {
                            int.TryParse(foundRow.ToString(), out dbId);
                        }

                        conn.Close();

                        dbExists = dbId != 0;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return dbExists;
        }       
    }
}

