using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using HostCommunication.HostModels;

namespace HostCommunication.Managers
{
    public static class DbManager
    {
        private static List<string> _listOfDbs = new List<string>();
        private static List<string> _listOfServers = new List<string>();

        private static void InitializeLists()
        {
           // _listOfDbs.Add(ConfigurationManager.AppSettings["DbMasterDatabase"]);
            _listOfDbs.Add(ConfigurationManager.AppSettings["D0_Database"]);
            _listOfDbs.Add(ConfigurationManager.AppSettings["D1_Database"]);

            _listOfServers.Add(ConfigurationManager.AppSettings["DbServer_One"]);
            _listOfServers.Add(ConfigurationManager.AppSettings["DbServer_Two"]);         
        }


        public static void InitializeBackup()
        {
            var listOfDbDescriptions = CheckDatabasesExistence();
            foreach (var dbDescription in listOfDbDescriptions)
            {
                RunSqlOnDatabases(dbDescription, ConfigurationManager.AppSettings["sqlCreateBackupDb"]);
            }
        }

        private static string loadPreparedSqlQueryForDbCreation(string sqlDirectoryToModify, string dbName)
        {
            string script = File.ReadAllText(sqlDirectoryToModify);
            script = script.Replace("fmWebApp", dbName);

            // set database sizes based on the number of datbases to create

            return script;
        }

        private static void RunSqlOnDatabases(DbDescription dbDescription, string sqlFileDirectory)
        {

            string script = loadPreparedSqlQueryForDbCreation(sqlFileDirectory, dbDescription.Name);

            using (var conn = ServerManager.EstablishBackupServerConnWithCredentials(dbDescription.Server))
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

        private static List<DbDescription> CheckDatabasesExistence()
        {
            bool dbExists = false;
            int dbId = 0;

            InitializeLists();
            List<DbDescription> dbDescriptions = new List<DbDescription>();
            try
            {
                foreach (var currentServer in _listOfServers)
                {
                    // establish connection with the first server and check if specific databases are in there
                    using (var conn = ServerManager.EstablishBackupServerConn(currentServer))
                    {
                        conn.Open();

                        foreach (var db in _listOfDbs)
                        {
                            // check if on this server, there exist all databases
                            string sqlDbIsOnTheServer = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", db);
                            using (SqlCommand sqlCmd = new SqlCommand(sqlDbIsOnTheServer, conn))
                            {
                                var foundRow = sqlCmd.ExecuteScalar();

                                if (foundRow != null)
                                    int.TryParse(foundRow.ToString(), out dbId);

                                dbExists = dbId != 0;

                                dbDescriptions.Add(new DbDescription()
                                {
                                    Name = db,
                                    Server = currentServer,
                                    Exists = dbExists,
                                    IsActive = dbExists
                                });
                            }
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return dbDescriptions;
        }       
    }
}

