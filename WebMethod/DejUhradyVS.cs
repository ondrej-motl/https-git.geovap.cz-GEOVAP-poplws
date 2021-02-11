using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Xpo.DB;

namespace PoplWS.WebMethod
{
  public class DejUhradyVS 
  {
    /// <summary>
    /// uhrady za VS
    /// </summary>
    /// <param name="session"></param>
    /// <param name="EXT_APP_KOD"></param>
    /// <param name="VS"></param>
    /// <returns></returns>
    public UHRADY_RESP DejUhrady(Session session, int EXT_APP_KOD, string VS)
    {
      Session sesna = session;
      UHRADY_RESP plResp = new UHRADY_RESP();
      plResp.status = Status.NOTEXISTS;

   #region kontrola vsupnich udaju
      try
      {

        if (EXT_APP_KOD == null)
        { throw new Exception("kód externí aplikace není zadán"); }

        if (string.IsNullOrEmpty(VS))
        { throw new Exception("VS není zadán"); }

        P_LIKEVS likevs = sesna.FindObject<P_LIKEVS>(CriteriaOperator.Parse("LIKEVS_VS = ?", VS));
        if (likevs == null)
        {
          plResp.status = Status.NOTEXISTS;
          plResp.result = Result.OK;
          return plResp;
        }


        //zda je pristup na poplatek
        KONTROLA_POPLATKU kp = new KONTROLA_POPLATKU(sesna, EXT_APP_KOD);
        if (!kp.EAexist())
        { throw new Exception("chybný kód externí aplikace"); }
        if (!kp.existPravoNadPoplatkem(likevs.CompoundKey1.LIKEVS_POPLATEK))
        { throw new Exception("k pohledávce neexistuje oprávnění"); }


      }
      catch (Exception exc)
      {
        plResp.result = Result.ERROR;

        if (exc.InnerException == null)
        {
          plResp.ERRORMESS = exc.Message;
        }
        else
        {
          plResp.ERRORMESS = exc.InnerException.Message;
        }
        return plResp;
        /*
          throw new Exception(String.Format("chyba \n {0}", exc.InnerException.Message));
          */
      }
      #endregion kontrola vstupnich udaju

      //setridim vzestupne dle PLATBA_ID
      XPCollection<P_PLATBA> platby = new XPCollection<P_PLATBA>(sesna, CriteriaOperator.Parse("PLATBA_VS = ?", VS), new SortProperty("PLATBA_ID", SortingDirection.Ascending));
      foreach (var item in platby) 
      {
        PLATBA pl = new PLATBA();
        Utils.copy.CopyDlePersistentAttr<PLATBA>(item, pl);
        plResp.UHRADY.Add(pl);
      }

      plResp.result = Result.OK;
      if (platby.Count > 0) //0.27
          plResp.status = Status.EXISTS;  

      return plResp;
    }
  }
}