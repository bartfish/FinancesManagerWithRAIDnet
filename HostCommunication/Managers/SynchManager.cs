using FMDataModel.DataModels;
using HostCommunication.HostModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace HostCommunication.Managers
{
    public static class SynchManager
    {
        private static string[] _dbTables { get; set; }

        public static void UpdateMirrorDb(DbDescription dbDescription)
        {

        }

        public static void FetchAllFromMasterDb()
        {
            _dbTables = DbManager.FetchAllTableNames();
            using (var model = new fmDbDataModel())
            {
                foreach (var table in _dbTables)
                {
                    
                }
            }
        }

        public static void FetchNewestFromMasterDb()
        {

        }

        public static void CreateDbMirror(DbDescription dbToBeCreated, DbDescription workingMirror)
        {
            DbManager.RunSqlAgainstDatabase(dbToBeCreated, ConfigurationManager.AppSettings["sqlCreateBackupDb"]);
            List<DependentQuery> listOfQueries = DbManager.BuildInsertsFrom(workingMirror, dbToBeCreated);
            DbManager.RunDQueriesAcrossDb(listOfQueries);
        }
    }
}
