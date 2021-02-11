using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Configuration;

namespace PoplWS.Util
{
    internal class ConfigValue
    {
        private static System.Collections.Specialized.NameValueCollection ReturnRCs;

        static ConfigValue()
        {
            ReturnRCs = new System.Collections.Specialized.NameValueCollection();
            string[] hodnoty = WebConfigurationManager.AppSettings.AllKeys
                                      .Where(key => key.StartsWith("ReturnRC"))
                                      .Select(key => WebConfigurationManager.AppSettings[key])
                                      .ToArray();
            foreach (string item in hodnoty)
            {
                string s = item.Replace("ReturnRC", "");
                string[] kv = s.Split('=');
                if (kv.Length == 2)
                   ReturnRCs.Add(kv[0], kv[1]);
            }
                                      
        }

        static bool ReturnRC(string poplatek)
        {
            return (ReturnRCs.Get(poplatek) == "1") == null ? true : ReturnRCs.Get(poplatek) == "1";
        }

    }

    internal class Util
    {
        internal static void DejKCReduk(ref decimal predpisOut, ref decimal uhrazenoOut, decimal predpis, decimal sparovano, decimal sparovano_minusem)
        {
            if (sparovano_minusem != 0)
            {
                decimal snizeniKc = sparovano_minusem * (Math.Abs((int)predpis) / (int)predpis);
                decimal prKcReduk = predpis - snizeniKc;
                predpisOut = Utils.Mathm.ZaokrouhliDouble(prKcReduk, 2);
                uhrazenoOut = (sparovano * (Math.Abs((int)predpis) / (int)predpis)) - snizeniKc;
                uhrazenoOut = Utils.Mathm.ZaokrouhliDouble(uhrazenoOut, 2);
            }
            else
            {
                predpisOut = Utils.Mathm.ZaokrouhliDouble(predpis, 2);
                uhrazenoOut = (sparovano * (Math.Abs((int)predpis) / (int)predpis));
                uhrazenoOut = Utils.Mathm.ZaokrouhliDouble(uhrazenoOut, 2);
            }
        }

        internal static string GetLogFileName()
        {
            return SpyExtensionAttribute.DataDir() + "/logPoplWS.txt";
        }
        internal static void WriteLog(string message)
        {
            int logMode = 0;
            int.TryParse(System.Web.Configuration.WebConfigurationManager.AppSettings["LogMode"], out logMode);
            if (logMode < 3)
                return;

            string logMessage =  string.Format(Environment.NewLine + "{0} - {1} \n", DateTime.Now, message) ;
            System.IO.File.AppendAllText(GetLogFileName(), logMessage); 
        }

    }
}