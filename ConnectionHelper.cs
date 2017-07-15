using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

namespace TrScanCode.Utils
{
    internal class ConnectionHelper
    {
        internal static SqlConnection getContextConnection()
        {
            SqlConnection returnValue = null;
            try
            {
                returnValue = new SqlConnection("Context connection = true");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnValue;
        }

        internal static SqlConnection getOpenContextConnection()
        {
            SqlConnection returnValue = getContextConnection();
            returnValue.Open();
            return returnValue;
        }

        internal static void sendMessage(string msg,params string[] param)
        {
            SqlContext.Pipe.Send(String.Format(msg, param));
        }
    }
}
