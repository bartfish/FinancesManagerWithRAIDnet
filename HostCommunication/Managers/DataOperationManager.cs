using HostCommunication.HostModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace HostCommunication.Managers
{
    public static class DataOperationManager
    {
        private static int numOfUpdatedDatabases = 0;
        private static DbDescription currentlyConnectedDb;
        private static List<DbDescription> _currentListOfDbs = new List<DbDescription>();

        /// <summary>
        /// Method responsible for veryfying the results assigned by the programmer
        /// </summary>
        /// <param name="calledMethod"></param>
        /// <param name="paramsSent"></param>
        /// <param name="methodReturnStatus"></param>
        public static object VerifyResult(Delegate calledMethod, object[] paramsSent, MethodReturnStatus methodReturnStatus)
        {
            string currentDbConnection = ConfigurationManager.ConnectionStrings["fmDbDataModel"].ConnectionString;
            if (methodReturnStatus == MethodReturnStatus.Value)
            {
                // if everything went well 
                    // return to the method and continue its execution
            }
            else if (methodReturnStatus == MethodReturnStatus.Null)
            {
                // if error occured
                // switch webconfig to the mirror db from other server
                // call the method once again
                return calledMethod.DynamicInvoke(paramsSent);
            }
            else if (methodReturnStatus == MethodReturnStatus.Error)
            {
                // if error occured
                // switch webconfig to the mirror db from other server

                // run the method once again
                return calledMethod.DynamicInvoke(paramsSent);
            }
            return calledMethod.DynamicInvoke(paramsSent);

        }

        public static void InitializeDbsData(List<DbDescription> dbs)
        {
            _currentListOfDbs = dbs;
        }

        public static DbDescription SwitchToMirror(DbDescription currDb)
        {
            // go to next mirror on the other server and return it
            return new DbDescription();
        }

        public static DbDescription SwitchToOtherPart(DbDescription currDb)
        {
            // go to other part of the databases on the same server
            return new DbDescription();
        }

        public static string UpdateConnString(DbDescription newDbDescription)
        {
            return "";
        }



    }
}
