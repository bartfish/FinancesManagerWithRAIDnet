using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace HostCommunication.Managers
{
    public static class DbManager
    {

        public static void InitializeBackup()
        {
            var x = CheckDatabaseExistence();
        }

        private static void CreateBackupFromSql()
        {

        }

        private static bool CheckDatabaseExistence()
        {
            string databaseName = "fmWebApp";
            bool dbExists = false;
            try
            {
                SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS_2;Integrated security=SSPI;database=master");
                string sqlCreateDBQuery = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", databaseName);

                using (conn)
                {
                    using (SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, conn))
                    {
                        conn.Open();

                        object resultObj = sqlCmd.ExecuteScalar();

                        int databaseID = 0;

                        if (resultObj != null)
                        {
                            int.TryParse(resultObj.ToString(), out databaseID);
                        }

                        conn.Close();

                        dbExists = (databaseID > 0);
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
