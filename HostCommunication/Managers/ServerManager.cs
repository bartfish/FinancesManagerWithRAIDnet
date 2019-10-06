using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace HostCommunication.Managers
{
    public static class ServerManager
    {
        public static SqlConnection EstablishBackupServerConn()
        {
            return new SqlConnection("Server=" + ConfigurationManager.AppSettings["serverMasterDb"] + ";Integrated security=SSPI;database=master");
        }
    }
}
