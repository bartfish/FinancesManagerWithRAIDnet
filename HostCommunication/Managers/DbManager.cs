using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using HostCommunication.HostModels;
using System.Data;

namespace HostCommunication.Managers
{
    public static class DbManager
    {
        private static List<string> _listOfDbs = new List<string>();
        private static List<string> _listOfServers = new List<string>();
        private static List<DbDescription> _listOfDbDescriptions = new List<DbDescription>();

        private static void InitializeLists()
        {
            _listOfDbs.Add(ConfigurationManager.AppSettings["D0_Database"]);
            _listOfDbs.Add(ConfigurationManager.AppSettings["D1_Database"]);
            _listOfDbs.Add(ConfigurationManager.AppSettings["D2_Database"]);
            _listOfDbs.Add(ConfigurationManager.AppSettings["D3_Database"]);

            _listOfServers.Add(ConfigurationManager.AppSettings["DbServer_One"]);
            _listOfServers.Add(ConfigurationManager.AppSettings["DbServer_Two"]);         
        }


        public static void PrepareRAID()
        {
            _listOfDbDescriptions = CheckDatabasesExistence();
            foreach (var dbDescription in _listOfDbDescriptions)
            {
                if (!dbDescription.Exists)
                {
                    RunSqlOnDatabases(dbDescription, ConfigurationManager.AppSettings["sqlCreateBackupDb"]);
                }
            }

            spreadData();

        }

        private static void spreadData()
        {
            // fetch all data from each table from master database
            string[] tables = fetchAllTables();
            List<DependentQuery> dpQueries = new List<DependentQuery>();
            foreach (var table in tables)
            {
                string sqlGetAllDataFromTable = string.Format("SELECT * FROM {0}.dbo.{1}", ConfigurationManager.AppSettings["DbMasterDatabase"], table);
                using (var conn = ServerManager.EstablishBackupServerConnWithCredentials(ConfigurationManager.AppSettings["DbServer_One"]))
                {
                    SqlCommand cmd = new SqlCommand(sqlGetAllDataFromTable, conn);
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    da.Fill(dataTable);

                    // initialize columns into insert statement
                    // build insert sql query for each table (2 inserts per table, each containing half of the data)
                    string insertQuery = "INSERT INTO dbName.dbo." + table + "(";
                    foreach (var column in dataTable.Columns)
                    {
                        insertQuery += column + ",";
                    }
                    insertQuery = insertQuery.Remove(insertQuery.Length - 1);
                    insertQuery += ") Values(";
                    
                    int dbParity = 0;
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        string inQueryWithVals = insertQuery;
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            var data = dataRow[column.ToString()];
                            if (data.ToString() == string.Empty)
                            {
                                inQueryWithVals += "null,";
                            }
                            else
                            {
                                if
                                (column.DataType == typeof(string) || column.DataType == typeof(DateTime))
                                {
                                    inQueryWithVals += "'" + dataRow[column.ToString()].ToString() + "',";
                                }
                                else if (column.DataType == typeof(Byte[]))
                                {
                                    byte[] xy = (Byte[])dataRow[column.ToString()];
                                    string result = System.Text.Encoding.UTF8.GetString(xy);
                                    inQueryWithVals += "'" + (byte[])dataRow[column.ToString()] + "',";

                                }
                                else
                                {
                                    inQueryWithVals += dataRow[column.ToString()] + ",";
                                }
                            }
                        }
                        inQueryWithVals = inQueryWithVals.Remove(inQueryWithVals.Length - 1);
                        inQueryWithVals += ")";

                        DependentQuery dpQuery = new DependentQuery();
                        List<DbDescription> dbsToAssign = null;
                        // assign to which database on which server the specific built in the next step sql query is going to be run
                        if (dbParity % 2 == 0)
                        {
                            dbsToAssign = _listOfDbDescriptions
                                .FindAll(d => d.Server == ConfigurationManager.AppSettings["DbServer_One"]);  
                        }
                        else
                        {
                            dbsToAssign = _listOfDbDescriptions
                                .FindAll(d => d.Server == ConfigurationManager.AppSettings["DbServer_Two"]);
                        }

                        foreach (var dbToAssign in dbsToAssign)
                        {
                            dpQuery.DatabaseDescription = dbToAssign;
                            dpQuery.Query = inQueryWithVals;
                            dpQuery = dpQuery.UpdateQueryParams();
                            dpQueries.Add(dpQuery);
                        }
                        dbParity++;
                    }

                    var x = dpQueries;
                    conn.Close();
                    da.Dispose();
                }


            }

            // run sql queries on each database
            //foreach(var currQuery in dpQueries)
            //{
            //    using (var conn = ServerManager.EstablishBackupServerConnWithCredentials(currQuery.DatabaseDescription.Server))
            //    {
            //        try
            //        {
            //            conn.Open();
            //            using (var command = new SqlCommand(currQuery.Query, conn))
            //            {
            //                command.ExecuteNonQuery();
            //            }
            //            conn.Close();

            //        }
            //        catch (Exception)
            //        {

            //            throw;
            //        }

            //    }
            //}
            // run all from the first server

            // run all from the second server 

        }

        private static string[] fetchAllTables()
        {
            List<string> values = new List<string>();
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key.StartsWith("Table"))
                {
                    string value = ConfigurationManager.AppSettings[key];
                    values.Add(value);
                }
            }
            return values.ToArray();
        }


        private static string loadPreparedSqlQueryForDbCreation(string sqlDirectoryToModify, string dbName)
        {
            string script = File.ReadAllText(sqlDirectoryToModify);
            script = script.Replace("fmWebApp", dbName);
            
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
                int counter = 0, serverNum = 0;
                foreach (var currentServer in _listOfServers)
                {
                    // establish connection with the first server and check if specific databases are in there
                    using (var conn = ServerManager.EstablishBackupServerConn(currentServer))
                    {
                        conn.Open();

                        for(int i = counter; i < _listOfDbs.Count; i++)
                        {
                            if (serverNum == 0)
                            {
                                if (counter >= 2)
                                {
                                    serverNum++;
                                    break;
                                }
                            }
                            
                            // check if on this server, there exist all databases
                            string sqlDbIsOnTheServer = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", _listOfDbs[i]);
                            using (SqlCommand sqlCmd = new SqlCommand(sqlDbIsOnTheServer, conn))
                            {
                                var foundRow = sqlCmd.ExecuteScalar();

                                if (foundRow != null)
                                    int.TryParse(foundRow.ToString(), out dbId);

                                dbExists = dbId != 0;

                                dbDescriptions.Add(new DbDescription()
                                {
                                    Name = _listOfDbs[i],
                                    Server = currentServer,
                                    Exists = dbExists,
                                    IsActive = dbExists
                                });
                            }
                            counter++;
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return dbDescriptions;
        }       
    }
}

