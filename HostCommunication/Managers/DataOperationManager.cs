using HostCommunication.HostModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using System.Web;

namespace HostCommunication.Managers
{
    public static class DataOperationManager
    {
        private static int errorCounter = 0; // if the value is the same as the number of databases return null to the user and to not allow for the operation to continue
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
                SwitchToOtherPart();
                // call the method once again
                return calledMethod.DynamicInvoke(paramsSent);
            }
            else if (methodReturnStatus == MethodReturnStatus.Error)
            {
                // if error occured
                SwitchToMirror();
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

        public static void SwitchToMirror()
        {
            // go to next mirror on the other server and return it
            
        }

        public static void SwitchToOtherPart()
        {
            // go to other part of the databases on the same server

        }

        public static string UpdateConnString(DbDescription newDbDescription)
        {
            string initialStr = $"data source={newDbDescription.Server};initial catalog={newDbDescription.Name};user id=sa;password=br123;MultipleActiveResultSets=True;App=EntityFramework";
            HttpContext.Current.Session["dbConnectionString"] = initialStr;
            return initialStr;
        }
        
    }
}
