using FMDataModel.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HostCommunication.Managers
{
    public static class SynchManager
    {
        private static List<string> dbTables = new List<string>()
        {
            "fm_Users", "fm_Incomes", "fm_Outcomes"
        };

        public static void FetchAllFromMasterDb()
        {
            using (var model = new fmDbDataModel())
            {
                foreach (var table in dbTables)
                {
                    var tableFromModel = Set(GetType(table);
                }
            }
        }

        public static void FetchNewestFromMasterDb()
        {

        }
    }
}
