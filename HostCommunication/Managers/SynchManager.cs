using FMDataModel.DataModels;
using HostCommunication.HostModels;
using System;
using System.Collections.Generic;
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
    }
}
