using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace PoplWS.WebMethod
{
    public class SeznamPoplatku
    {
      Session sesna;
      DBValue dbValue;
      int EXT_APP_KOD;
      public SeznamPoplatku(string login, string password, int EXT_APP_KOD)
      {
            XPOConnector xpc = new XPOConnector(login, password);
            sesna = xpc.GetSession();
            dbValue = DBValue.Instance(sesna);
            this.EXT_APP_KOD = EXT_APP_KOD;

            login = dbValue.DBUserName;
      }

      public POPLATKY_RESP dejSeznam()
        {

          POPLATKY_RESP poplResp = new POPLATKY_RESP();
          KONTROLA_POPLATKU kp = new KONTROLA_POPLATKU(sesna, EXT_APP_KOD);
          try
          {
            if (!kp.EAexist())
            { throw new Exception("chybný kód externí aplikace / neexistuji poplatky s povoleným přístupem"); }
          }
          catch (Exception exc)
          {
            poplResp.result = Result.ERROR;

            if (exc.InnerException == null)
            {
              poplResp.ERRORMESS = exc.Message;
            }
            else
            {
              poplResp.ERRORMESS = exc.InnerException.Message;
            }
            return poplResp;
            /*
              throw new Exception(String.Format("chyba \n {0}", exc.InnerException.Message));
              */
          }

            XPCollection<C_NAZPOPL> xpoPoplatky = new XPCollection<C_NAZPOPL>(sesna);

            foreach (var item in xpoPoplatky)
	        {
            if (!kp.existPravoNadPoplatkem(item.NAZPOPL_POPLATEK ))
            { continue; }

     		        POPLATEK popl = new POPLATEK();
                popl.KOD = (int)item.NAZPOPL_POPLATEK;
                popl.NAZEV = item.C_NAZPOPL_NAZEV;
                popl.PERIODA = new List<PERIODA>();

                foreach (var itemPer in item.C_EVPOPL)
	            {
                    if ((itemPer.EVPOPL_FROMDATE <= dbValue.DBSysDate) &&
                        (itemPer.EVPOPL_TODATE >= dbValue.DBSysDate))
                    {
                        PERIODA perioda = new PERIODA();
                        perioda.KOD = itemPer.CompoundKey1.EVPOPL_PER.PERIODA_PERIODA;
                        perioda.NAZEV = itemPer.CompoundKey1.EVPOPL_PER.PERIODA_NAZEV;
                        perioda.PERIODICITA = (int)itemPer.CompoundKey1.EVPOPL_PER.PERIODA_POC;
                        popl.PERIODA.Add(perioda);
                    }
	            }

                poplResp.poplatky.Add(popl);
	        }

            poplResp.result = Result.OK;
            poplResp.status = Status.EXISTS;
            return poplResp;

        }
    }

    public class POPLATKY_RESP 
    {
      /// <summary>
      ///  pokud akce probehla bez chyb = OK
      /// </summary>
      [System.Xml.Serialization.XmlAttribute("result")]
      public Result result { get; set; }

      /// <summary>
      /// zda zaznam existoval, nebo byl vlozen
      /// </summary>
      [System.Xml.Serialization.XmlAttribute("status")]
      public Status status { get; set; }

      public string ERRORMESS { get; set; }

      public List<POPLATEK> poplatky;

      public POPLATKY_RESP()
      {
        poplatky = new List<POPLATEK>();
      }
    }
   
    public class POPLATEK
    {
        public int KOD { get; set; }
        public string NAZEV { get; set; }
        public List<PERIODA> PERIODA;
    }

    public class PERIODA
    {
        public string KOD { get; set; }
        public string NAZEV { get; set; }
        public int PERIODICITA { get; set; }
    }

}