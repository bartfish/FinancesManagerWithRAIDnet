using HostCommunication.HostModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Net.Http;
using System.Reflection;
using System.Text;

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
            //Current.Session["dbConnection"] = initialStr;
            ////Fetch Connection String from Web.config 
            //// ConnectionStrings[0] == Fetch Connection String Format
            //var DBCS = ConfigurationManager.ConnectionStrings[0];

            ////Convert Readonly to Writable (In Connection String Provider is readonly so we need to change it as Writable)  
            ////Dont forgot to Declare BelowNameSpace  
            ////using System.Configuration;  
            ////using System.Reflection;  
            //var writable = typeof(ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            //writable.SetValue(DBCS, false);

            ////Replace Connecion string  
            //DBCS.ConnectionString = initialStr;

            // add a reference to System.Configuration
            //var entityCnxStringBuilder = new EntityConnectionStringBuilder
            //    (System.Configuration.ConfigurationManager
            //        .ConnectionStrings["fmDbDataModel"].ConnectionString);

            //// init the sqlbuilder with the full EF connectionstring cargo
            //var sqlCnxStringBuilder = new SqlConnectionStringBuilder
            //    (entityCnxStringBuilder.ProviderConnectionString);

            //// now flip the properties that were changed
            //context.Database.Connection.ConnectionString
            //    = initialStr;
            //Uri UriAssemblyFolder = new Uri(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase));
            //string appPath = UriAssemblyFolder.LocalPath;
            //ExeConfigurationFileMap map = new ExeConfigurationFileMap { ExeConfigFilename = "FMDataModel" };
            //Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            //var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            //connectionStringsSection.ConnectionStrings["fmDbDataModel"].ConnectionString = initialStr;
            //config.Save();
            //ConfigurationManager.RefreshSection("connectionStrings");
            return initialStr;
        }
        
    }
}
