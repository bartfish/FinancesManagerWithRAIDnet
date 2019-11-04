using HostCommunication.HostModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HostCommunication.Managers
{
    public static class DataOperationManager
    {
        public static int numOfUpdatedDatabases = 0;

        public static void VerifyResult(Delegate yourMethod, object[] paramsSent, MethodReturnStatus methodReturnStatus)
        {

            if (methodReturnStatus == MethodReturnStatus.Value)
            {
                // if everything went well 
                    // return to the method and continue its execution
            }
            else if (methodReturnStatus == MethodReturnStatus.Null)
            {
                // if error occured
                // switch webconfig to the mirror db from other server
                // call the method 
                yourMethod.DynamicInvoke(paramsSent);


            }
            else if (methodReturnStatus == MethodReturnStatus.Error)
            {
                // if error occured
                // switch webconfig to the mirror db from other server
            }

        }

    }
}
