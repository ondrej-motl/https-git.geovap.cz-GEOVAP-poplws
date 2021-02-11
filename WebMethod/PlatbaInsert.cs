using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace PoplWS.WebMethod
{
  public class PlatbaInsert
  {
    public UHRADY_RESP InsertUhraduPredpisu(Session session, int EXT_APP_KOD, int PREDPIS_ID, decimal KC, DateTime DATUM_UHRADY, string ZAPLATIL, string DOKLAD, int SS)
    {
      Session sesna = session;
      UHRADY_RESP plResp = new UHRADY_RESP();
      plResp.status = Status.NOTEXISTS;
      P_PRPL prpl;

      #region kontrola vsupnich udaju
      try
      {

        if (EXT_APP_KOD == null)
        { throw new Exception("kód externí aplikace není zadán"); }

        if (PREDPIS_ID == null)
        { throw new Exception("PREDPIS_ID není zadán"); }

        prpl = sesna.FindObject<P_PRPL>(CriteriaOperator.Parse("PRPL_ID = ? and PRPL_PREDPIS + PRPL_SANKCE > 0", PREDPIS_ID));
        if (prpl == null)
        {
          plResp.status = Status.NOTEXISTS;
          plResp.result = Result.ERROR;
          { throw new Exception("neexistující předpis"); }
        }

        if (KC <= 0)
          { throw new Exception("nelze vložit zápornou platbu"); }

        P_NASTAVENI nast = sesna.GetObjectByKey<P_NASTAVENI>("OTEVR_OBDOBI");
        string obd = "01." + nast.HODNOTA;
        DateTime otevrObd = DateTime.ParseExact(obd, "d.M.yyyy", null);
        if (DATUM_UHRADY < otevrObd)
          { throw new Exception("datum úhrady není v otevřeném období"); }


        //zda je pristup na poplatek
        KONTROLA_POPLATKU kp = new KONTROLA_POPLATKU(sesna, EXT_APP_KOD);
        if (!kp.EAexist())
        { throw new Exception("chybný kód externí aplikace"); }
        if (!kp.existPravoNadPoplatkem(prpl.CompoundKey.PRPL_POPLATEK))
        { throw new Exception("k pohledávce neexistuje oprávnění"); }

        #region kontrola prava nad poplatkem
        PravoNadPoplatkem pnp = null;
        try
        {
          pnp = new PravoNadPoplatkem(sesna);
        }
        catch (Exception)
        {
          throw new Exception("kontrola přístp. práv uživatele nad daty Příjmové agendy skončila chybou");
        }

        //pro platby je povoleno vkladani
        if (!pnp.PravoExist((int)prpl.CompoundKey.PRPL_POPLATEK, PravoNadPoplatkem.PrtabTable.PLATBA, PravoNadPoplatkem.SQLPerm.INSERT))
          throw new Exception("PoplWS - nedostatečná oprávnění pro vkládání plateb.");

        #endregion kontrola prava nad poplatkem

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

      decimal prNesparovano = (prpl.PRPL_PREDPIS + prpl.PRPL_SANKCE) - prpl.PRPL_SPAROVANO;
      try
      {
        using (UnitOfWork uow = new UnitOfWork(sesna.DataLayer))
          {
             DBUtil dbu = new DBUtil(sesna);
         
             P_PLATBA plIns = new P_PLATBA(uow);
             plIns.PLATBA_ID = dbu.LIZNI_SEQ("PLATBA_ID");
             plIns.PLATBA_VS = prpl.PRPL_VS;
             plIns.PLATBA_PLDATE = DATUM_UHRADY;
             plIns.PLATBA_NAUCETDNE = plIns.PLATBA_PLDATE;
             plIns.PLATBA_PLKC = KC;
             plIns.PLATBA_PLATCE = ZAPLATIL.SubstrLeft(30);
             if ( ! string.IsNullOrWhiteSpace(DOKLAD))
                plIns.PLATBA_DOKLAD = DOKLAD.SubstrLeft(30);
             plIns.PLATBA_UCETMESIC = plIns.PLATBA_PLDATE.Month;
             plIns.PLATBA_UCETROK = plIns.PLATBA_PLDATE.Year;
             plIns.PLATBA_RECORD = " ";
             plIns.PLATBA_SS = SS; 
             DBValue dbv = DBValue.Instance(sesna);
             plIns.LOGIN = dbv.DBUserName;
             plIns.LASTUPDATE = dbv.DBSysDateTime;
             plIns.ENTRYLOGIN = plIns.LOGIN;
             plIns.ENTRYDATE = plIns.LASTUPDATE;
             plIns.PLATBA_EA = EXT_APP_KOD;
             plIns.PLATBA_INTOZN = "0";  

             decimal prKc = prpl.PRPL_PREDPIS + prpl.PRPL_SANKCE;
             if ((prKc > 0) && (KC > 0) && (prNesparovano > 0))   //kladna platba s kladnym predpisem
             {
                 P_PAROVANI parovaniIns = new P_PAROVANI(uow);
                 parovaniIns.CompoundKey1.PAR_PRPL_ID = prpl.PRPL_ID;
                 parovaniIns.CompoundKey1.PAR_PLATBA_ID = plIns.PLATBA_ID;
                 
                 if (prNesparovano == KC)
                   parovaniIns.PAR_SPARKC = KC;
                 if (Math.Abs(prNesparovano) < Math.Abs(KC))
                   parovaniIns.PAR_SPARKC = prNesparovano;
                 if (Math.Abs(prNesparovano) > Math.Abs(KC))
                   parovaniIns.PAR_SPARKC = KC;

                 parovaniIns.PAR_VYTVRUCNE = "NE";
                 parovaniIns.PAR_DATE = dbv.DBSysDateTime.ToString("yyMMddHHmmss");

                 uow.ExecuteNonQuery(String.Format(
                          "insert into P_PR_AKT (PRA_PRPL_ID, PRA_PR, PRA_TYP) values ({0}, {1}, '{2}')",
                                  prpl.PRPL_ID, plIns.PLATBA_ID, "+"));

             }
             uow.CommitChanges();

             plResp.result = Result.OK;
             plResp.status = Status.INSERTED;

             return plResp;
          }  //uow
        } //try

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
    }

  }
}