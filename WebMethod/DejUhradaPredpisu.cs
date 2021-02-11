using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;


namespace PoplWS.WebMethod
{
  public class DejUhradaPredpisu
  {
    /// <summary>
    /// seznam plateb k predpisu
    /// predpis KC - je ponizeny o pripadny zaporny predpis či zápornou platbu. Jsou tedy vráceny i předpisy s nulovou částkou - 
    ///    jde vlastně o stornovaný předpis.
    /// KC platby = PLATBA_SPAROVANO - pouze priparovana cast platby k predpisu
    /// </summary>
    /// <param name="session"></param>
    /// <param name="PREDPIS_ID"></param>
    /// <param name="EXT_APP_KOD"></param>
    /// <returns></returns>
    internal UHRADA_PREDPISU_RESP dej_PlatbyKPredpisu(Session session, int EXT_APP_KOD, int PREDPIS_ID)
    {
      Session sesna = session;
      UHRADA_PREDPISU_RESP plResp = new UHRADA_PREDPISU_RESP();
      plResp.status = Status.NOTEXISTS;

#region kontrola vsupnich udaju
      try
      {

        if (EXT_APP_KOD == null)
        { throw new Exception("kód externí aplikace není zadán"); }

        P_PRPL pr = sesna.GetObjectByKey<P_PRPL>(PREDPIS_ID);
        if (pr == null)
        {
          plResp.status = Status.NOTEXISTS;
        }

        decimal uhrazenoKc = 0;
        decimal prKc = 0;
        Util.Util.DejKCReduk(ref prKc, ref uhrazenoKc, pr.USER_PREDPIS, pr.PRPL_SPAROVANO, pr.PRPL_SPAROVANO_MINUSEM);
        plResp.PREDPIS_KC = prKc;
        plResp.PREDPIS_KC_UHRAZENO = uhrazenoKc;

        if (plResp.PREDPIS_KC < 0)
        { throw new Exception("nejedná se o předpis pohledávky"); }

        KONTROLA_POPLATKU kp = new KONTROLA_POPLATKU(sesna, EXT_APP_KOD);
        if (!kp.EAexist()) 
                 { throw new Exception("chybný kód externí aplikace"); }
        if (!kp.existPravoNadPoplatkem(pr.CompoundKey.PRPL_POPLATEK))
                  { throw new Exception("k pohledávce neexistuje oprávnění"); }


        #region kontrola prava nad predpisy
        PravoNadPoplatkem pnp = null;
        try
        {
          pnp = new PravoNadPoplatkem(sesna);
        }
        catch (Exception)
        {
          throw new Exception("kontrola přístp. práv uživatele nad daty Příjmové agendy skončila chybou");
        }

        
        if (!pnp.PravoExist((int)pr.CompoundKey.PRPL_POPLATEK, PravoNadPoplatkem.PrtabTable.PLATBA, PravoNadPoplatkem.SQLPerm.SELECT))
            throw new Exception("PoplWS - nedostatečná oprávnění pro čtení úhrad předpisů.");

        #endregion kontrola prava nad predpisy

      }
      catch (Exception exc)
      {
        plResp.result = Result.ERROR;
        plResp.PREDPIS_KC = 0;

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
#endregion kontrola vsupnich udaju

      XPCollection<P_PAROVANI> par = new XPCollection<P_PAROVANI>(sesna, CriteriaOperator.Parse("CompoundKey1.PAR_PRPL_ID = ? and CompoundKey1.PAR_PLATBA_ID > 0", PREDPIS_ID));  //32006

      foreach (var parovani in par)
      {
        P_PLATBA pPlatba = sesna.GetObjectByKey<P_PLATBA>(parovani.CompoundKey1.PAR_PLATBA_ID);
        PLATBA platba = new PLATBA();
        Utils.copy.CopyDlePersistentAttr<PLATBA>(pPlatba, platba);
        platba.PLATBA_PLKC = parovani.PAR_SPARKC;
        plResp.UHRADY.Add(platba);
      }

      plResp.result = Result.OK;
      plResp.status = Status.EXISTS;
      return plResp;
    }
  }
}