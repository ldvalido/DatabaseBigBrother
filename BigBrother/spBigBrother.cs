using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Xml;
using System.Text.RegularExpressions;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure(Name="spBigBrother")]
    public static void spBigBrother(SqlXml eventData,out int result)
    {
        result = default(int);
        if (eventData == null)
            return;

        // Replace with your own code
        string sqlSTR = String.Empty;
        XmlDocument _xml = new XmlDocument();
        _xml.LoadXml(eventData.Value);

        XmlNode _node = _xml.SelectSingleNode("//EVENT_INSTANCE/TSQLCommand/CommandText");
        if (_node != null)
        {
            string cmdText = _node.InnerText;
            cmdText = sqlNormalize(cmdText);
            SqlContext.Pipe.Send("Normalizacion :" +  cmdText);
            if (cmdText.Contains("*") || cmdText.Contains("CURSOR"))
            {
                SqlContext.Pipe.Send("The use of * or Cursors is not allowed in the inner of Store Procedure");
                SqlContext.Pipe.Send("Please complete all fields that you need");
                result = -1;
            }
            else
            {
                //sqlSTR = "INSERT INTO Audits VALUES ('" + SqlContext.TriggerContext.EventData.Value + "')";
                //SqlConnection conn = new SqlConnection("Context connection = true");
                //conn.Open();
                //SqlCommand com = new SqlCommand();
                //com.Connection = conn;
                //com.CommandText = sqlSTR;
                //com.ExecuteNonQuery();
            }
        }
        else
        {
            SqlContext.Pipe.Send("Query Text not found");
        }
        if (result == default(int))
        {
            SqlContext.Pipe.Send("Store Procedure approbed");
        }
        //TODO: Clean the query
    }
    
    static string sqlNormalize(string normalQuery) 
    {
        string returnValue = normalQuery;
        //Find /* secuence and remove
        string regex = "^(?<BlockComment>/\\*.*\\*/)|(?<LinealComment>--.*)$";
        System.Text.RegularExpressions.RegexOptions options = ((System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace | System.Text.RegularExpressions.RegexOptions.Multiline)
                    | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(regex, options);

        Match mat = reg.Match(normalQuery);

        //returnValue = Regex.Replace(normalQuery, regex,"$´$'",options);
        returnValue = returnValue.Replace(Environment.NewLine, " ");
        returnValue = returnValue.Replace("\t", " ");
        returnValue = reg.Replace(returnValue, "$´$'");
        
        return returnValue;
    }
}
