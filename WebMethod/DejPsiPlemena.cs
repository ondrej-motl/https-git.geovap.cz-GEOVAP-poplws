using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Xpo.DB;

namespace PoplWS.WebMethod
{
  public class DejPsiPlemena
  {
    public PESPLEMENA_RESP DejPsiSeznamPlemen(Session sesna, int EXT_APP_KOD)
    {
        PESPLEMENA_RESP Resp = new PESPLEMENA_RESP();

        sesna.DropIdentityMap();

        try
        {

          if (EXT_APP_KOD == null)
          { throw new Exception("kód externí aplikace není zadán"); }
          P_EXTAPP EA = sesna.GetObjectByKey<P_EXTAPP>(EXT_APP_KOD);
          if (EA == null)
          { throw new Exception("chybný kód externí aplikace"); }


          int popl = Convert.ToInt16(sesna.ExecuteScalar(string.Format("SELECT KONF_POPLATEK FROM PES_KONFIG")));   

          KONTROLA_POPLATKU kp = new KONTROLA_POPLATKU(sesna, EXT_APP_KOD);
          if (!kp.EAexist())
          { throw new Exception("chybný kód externí aplikace"); }
          if (!kp.existPravoNadPoplatkem(popl))
          { 
              throw new Exception("Ext. aplikace nemá oprávnění k typu pohledávky.");
          }


          XPCollection<C_PES_PLEMENO> plemena = new XPCollection<C_PES_PLEMENO>(sesna, CriteriaOperator.Parse("1 = 1"), new SortProperty("C_PES_PLEMENO_NAZEV", SortingDirection.Ascending));
          if (plemena != null)
            Resp.PLEMENA = new List<PLEMENO>();
          foreach (C_PES_PLEMENO item in plemena)
          {
            PLEMENO pl = new PLEMENO();
            pl.KOD = item.C_PES_PLEMENO_KOD;
            pl.NAZEV = item.C_PES_PLEMENO_NAZEV;
            Resp.PLEMENA.Add(pl);
          }

          Resp.result = Result.OK;
          Resp.status = Status.EXISTS;
          return Resp;
        }
        catch (Exception exc)
        {
          Resp.result = Result.ERROR;


          if (exc.InnerException == null)
          {
            Resp.ERRORMESS = exc.Message;
          }
          else
          {
            Resp.ERRORMESS = exc.InnerException.Message;
          }
          /*
            throw new Exception(String.Format("chyba \n {0}", exc.InnerException.Message));
            */
          return Resp;
        }

    }
  }

}