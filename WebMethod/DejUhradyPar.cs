using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using System.Text;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Exceptions;

namespace PoplWS.WebMethod
{
  public class DejUhradyPar
  {
    #region DejUhrady
    /// <summary>
    /// pro zapnuti odberu uplateb je potreba rucni zapis do tabulek P_GEO_ODBER, P_EXTAPP.
    /// Tabulka P_GEO_ODBERPL obsahuje data cekajici na predani
    /// 
    /// select, insert, update, delete on  P_GEO_ODBERPL
    /// select, insert, update, delete on  P_GEO_PAR
    /// 
    /// Pokud je volano s poslednim cislem davky, jsou vytovrena nova data za nove platby.
    /// Pokud je volano s mensim cislem davky, nez je posledni cislo davky, tak jsou znovu zaslana puvodni
    /// data, plus jsou pridany nove platby.
    /// Je udrzovan seznam pouze naposled predanych plateb, proto lze takto ziskat pouze posledni predane platby,
    /// ktere se nepodarilo zpracovat na klientovi. Nelze ziskat stav minus jedna.
    /// 
    /// je provadene kontrola na povolene poplatky - jsou vraceny pouze platby na povolene poplatky 
    /// 
    /// prvni volani ma cislo davky -1  => jsou predany vsechny dosud nepradane platby
    /// </summary>
    /// <param name="session"></param>
    /// <param name="EXT_APP_KOD"></param>
    /// <param name="Davka"></param>
    /// <returns></returns>
    public UHRADY_NEW_RESP DejUhrady(Session session, int EXT_APP_KOD, string Davka, int NevracetPlatbyEA)
    {
      Session sesna = session;

      UHRADY_NEW_RESP plResp = new UHRADY_NEW_RESP();
      plResp.status = Status.NOTEXISTS;
      plResp.DAVKA = Davka;
      KONTROLA_POPLATKU kp = null;
      int EAbit;
      int oldDavka;
      int callDavka; 

      object obj = null;

      #region kontrola vsupnich udaju a nastaveni odberu

        try
      {

          if (EXT_APP_KOD == null)
        { throw new Exception("kód externí aplikace není zadán"); }

        //zda je pristup na poplatek
        kp = new KONTROLA_POPLATKU(sesna, EXT_APP_KOD);
        if (!kp.EAexist())
        { throw new Exception("chybný kód externí aplikace"); }

        obj = sesna.ExecuteScalar("select value from P_EXTAPP where ID = " + EXT_APP_KOD.ToString());
        if (obj != null)
          EAbit = Convert.ToInt32(obj);
        else
        { throw new Exception("chybný kód externí aplikace"); }

        if ( !Int32.TryParse(Davka, out callDavka) )  //naplni callDavka
         { throw new Exception("chybné číslo dávky"); }

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
      #endregion kontrola vstupnich udaju a nastaveni odberu


      obj = sesna.ExecuteScalar("select MAX(DAVKA) poslDavka from P_GEO_ODBERPL where EA = " + EXT_APP_KOD.ToString());
      if (obj != null)
        oldDavka = Convert.ToInt32(obj);
      else
        oldDavka = -1;

      if (callDavka == -1)
        oldDavka = -1;

      VytvorDavku zpracujDavku = new VytvorDavku(NovaData); 
      int newDavka = -1;
      try
      {
          sesna.ExplicitBeginTransaction();
          zpracujDavku(oldDavka, callDavka, EXT_APP_KOD, EAbit, sesna, out newDavka);
          sesna.ExplicitCommitTransaction();

          if (newDavka > callDavka)
          {
            //vyberu a poslu data 
            StringBuilder cmd = new StringBuilder();
            cmd.Append("select SUM(KC) PLATBA_PLKC, PLATBA_ID, PLATBA_VS, PLATBA_PLDATE, PLATBA_NAUCETDNE, ");
            cmd.Append(" PLATBA_SS, PLATBA_BANKSPOJ, PLATBA_BANKU, PLATBA_PLATCE, PLATBA_DOKLAD, PLATBA_POKLDOK, ");
            cmd.Append(" PLATBA_POZNAMKA ");
            cmd.Append("from P_PLATBA, P_GEO_ODBERPL ");
            cmd.Append(" where EA = " + EXT_APP_KOD.ToString());
            cmd.Append("   and DAVKA > " + callDavka.ToString());
            cmd.Append("   and PLATBA_ID = PLID   ");
            if ( NevracetPlatbyEA == 0 )  //nechci platby vlozene EA
              cmd.Append("   and PLATBA_EA <> " + EXT_APP_KOD.ToString());
            cmd.Append("   and exists (select 1 from P_PRPL, P_EXTAPP_POPL eap where PRID = PRPL_ID ");
            cmd.Append("               and eap.ID = " + EXT_APP_KOD.ToString());
            cmd.Append("               and eap.POPLATEK = PRPL_POPLATEK) ");
            cmd.Append("  group by PLATBA_ID, PLATBA_VS, PLATBA_PLDATE, PLATBA_NAUCETDNE, ");
            cmd.Append("      PLATBA_SS, PLATBA_BANKSPOJ, PLATBA_BANKU, PLATBA_PLATCE, PLATBA_DOKLAD, PLATBA_POKLDOK, ");
            cmd.Append("      PLATBA_POZNAMKA");
            cmd.Append(" having SUM(KC) <> 0");
            cmd.Append("    order by PLATBA_PLDATE");  //na prani Zdenala 2.2.18  

            SelectedData resultSet = sesna.ExecuteQueryWithMetadata(cmd.ToString());

            try
            {
                foreach (var row in resultSet.ResultSet[1].Rows)
                {
                    PLATBA platba = new PLATBA();
                    row.copyToObject<PLATBA>(resultSet, platba);
                    plResp.UHRADY.Add(platba);
                }

            }
            catch (Exception exc)
            {
                sesna.ExplicitRollbackTransaction();
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
                
            }
          }

          plResp.result = Result.OK;
          plResp.DAVKA = newDavka.ToString();

          if (plResp.UHRADY.Count() > 0)
            plResp.status = Status.EXISTS;
          else
            plResp.status = Status.NOTEXISTS;

          return plResp;

      }  //try
      catch (SqlExecutionErrorException exc)  
      {
        sesna.ExplicitRollbackTransaction();
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
      }

 

    }
    #endregion  //end DejUhrady

    private delegate void VytvorDavku(int oldDavka, int callDavka, int EA, int EAbit, Session sesna, out int newDavka);
    
    private void NovaData(int oldDavka, int callDavka, int EA, int EAbit, Session sesna, out int newDavka)
    {

      DBVal dbv = new DBVal(sesna);
      IDBValFactory idbv = dbv.CreateDBV();

      string nastavenyBit = String.Empty;
      if (dbv.CreateDBV().GetType() == typeof(MSSQL))
          nastavenyBit = "ZPRACOVAT & " + EAbit.ToString();
      if (dbv.CreateDBV() is ORACLE)
          nastavenyBit = "bitand(ZPRACOVAT," + EAbit.ToString() + ')'; 
      
      if (idbv.Database == typeof(ORACLE))
      {
          sesna.ExecuteNonQuery("select * from P_GEO_PAR where " + nastavenyBit + " = " + EAbit.ToString()
                                           + " for update nowait");
          //0.22
          sesna.ExecuteNonQuery("select * from P_GEO_ODBERPL where EA = " + EA.ToString()
                                           + " for update nowait");
      }
      if (idbv.Database == typeof(MSSQL))
      {
          sesna.ExecuteNonQuery("select * from P_GEO_PAR (UPDLOCK) where " + nastavenyBit + " = " + EAbit.ToString());
          sesna.ExecuteNonQuery("select * from P_GEO_ODBERPL (UPDLOCK) where EA = " + EA.ToString());
      }
      sesna.ExecuteNonQuery("delete from P_GEO_PAR where ZPRACOVAT = 0"); //0.21 - 23.2.2018

      DateTime rDate = DateTime.Now.AddMonths(-18);  
      DateTime sDate = DateTime.Now.AddMonths(-2);  //0.22
      if (idbv.Database == typeof(ORACLE))
      {
          sesna.ExecuteNonQuery("delete from P_GEO_PAR where ENTRYDATE < TO_DATE('" + rDate.ToString("dd.MM.yyyy") + "', 'dd.mm.yyyy')");
          sesna.ExecuteNonQuery("delete from P_GEO_ODBERPL where ENTRYDATE < TO_DATE('" + sDate.ToString("dd.MM.yyyy") + "', 'dd.mm.yyyy')");  //0.22
      }
      if (idbv.Database == typeof(MSSQL))
      {
          sesna.ExecuteNonQuery("delete from P_GEO_PAR where ENTRYDATE < '" + rDate.ToString("yyyyMMdd") + "'");
          sesna.ExecuteNonQuery("delete from P_GEO_ODBERPL where ENTRYDATE < '" + sDate.ToString("yyyyMMdd") + "'"); //0.22
      }

      sesna.ExecuteNonQuery(String.Format("delete from P_GEO_ODBERPL where EA = {0} and DAVKA <= {1}", EA, callDavka));

      //vlozim nova data 
      StringBuilder cmd = new StringBuilder();
      cmd.Clear();
      cmd.Append("insert into P_GEO_ODBERPL (DAVKA, EA, PRID, PLID, KC) ");
      cmd.Append("     select MAX(ID) DAVKA, " + EA.ToString() + ", PRID, PLID, ");
      cmd.Append("        sum(case TYP when 'P' then KC else KC * -1 end) KC ");
      cmd.Append("  from P_GEO_PAR a");
      cmd.Append("       where (PRID > 0 and PLID > 0) ");     //kladna platba k predpisu
      cmd.Append("         and " + nastavenyBit + " = " + EAbit.ToString());
      cmd.Append("         and ID > " + oldDavka.ToString());
      cmd.Append("   group by PRID, PLID ");
      sesna.ExecuteNonQuery(cmd.ToString());

      //zjistim posledni cislo davky
      object obj = sesna.ExecuteScalar("select MAX(DAVKA) from P_GEO_ODBERPL where EA = " + EA.ToString());
      if (obj != null)
        newDavka = Convert.ToInt32(obj);
      else
        newDavka = callDavka;

  
      //odnastaveni EAbitu 
      string nastavitBit = String.Empty;
      if (dbv.CreateDBV().GetType() == typeof(MSSQL))
       nastavitBit = String.Format(@" (ZPRACOVAT | {0} ) - {0}", EAbit);
      if (dbv.CreateDBV() is ORACLE)                 
        nastavitBit = String.Format(@"ZPRACOVAT - bitand(ZPRACOVAT, {0})", EAbit);
      sesna.ExecuteNonQuery("update P_GEO_PAR set ZPRACOVAT = " + nastavitBit
                           + " where ID <= " + newDavka.ToString() );

    }

    private void StaraData(int oldDavka, int callDavka, int EA, int EAbit, Session sesna, out int newDavka)
    {
      newDavka = 0;
      return;

      DBVal dbv = new DBVal(sesna);
      string nastavenyBit = String.Empty;
      if (dbv.CreateDBV().GetType() == typeof(MSSQL))
        nastavenyBit = "ZPRACOVAT & " + EAbit.ToString();
      if (dbv.CreateDBV() is ORACLE)
        nastavenyBit = "bitand(ZPRACOVAT," + EAbit.ToString() + ")";

      //vlozim nova data od minuleho pozadavku
      StringBuilder cmd = new StringBuilder();
      cmd.Clear();
      cmd.Append("insert into P_GEO_ODBERPL (DAVKA, EA, PRID, PLID, KC) ");
      cmd.Append("     select MAX(ID) DAVKA, " + EA.ToString() + ", PRID, PLID, ");
      cmd.Append("        sum(case TYP when 'P' then KC else KC * -1 end) KC ");
      cmd.Append("  from P_GEO_PAR a");
      cmd.Append("       where (PRID > 0 and PLID > 0) ");     //kladna platba k predpisu
      cmd.Append("         and " + nastavenyBit + " = " + EAbit.ToString());
      cmd.Append("         and ID > " + oldDavka.ToString());
      cmd.Append("   group by PRID, PLID ");
      sesna.ExecuteNonQuery(cmd.ToString());

      //zjistim posledni cislo davky
      object obj = sesna.ExecuteScalar("select MAX(DAVKA)  from P_GEO_ODBERPL where EA = " + EA.ToString());
      if (obj != null)
        newDavka = Convert.ToInt32(obj);
      else
        newDavka = oldDavka;

      if (callDavka == -1)  //prvni zadost o data
        sesna.ExecuteNonQuery("update P_GEO_ODBERPL set DAVKA = " + newDavka.ToString()
                            + "where EA = " + EA.ToString());
      else
        sesna.ExecuteNonQuery("update P_GEO_ODBERPL set DAVKA = " + newDavka.ToString()
                            + "where EA = " + EA.ToString()
                            + "  and DAVKA > " + callDavka.ToString()
                            + "  and DAVKA < " + newDavka.ToString());
      
      //odnastaveni EAbitu 
      string nastavitBit = String.Empty;
      if (dbv.CreateDBV().GetType() == typeof(MSSQL))
        nastavitBit = String.Format(@" (ZPRACOVAT | {0} ) - {0}", EAbit);
      if (dbv.CreateDBV() is ORACLE)
        nastavitBit = String.Format(@"ZPRACOVAT - bitand(ZPRACOVAT, {0})", EAbit);
      sesna.ExecuteNonQuery("update P_GEO_PAR set ZPRACOVAT = " + nastavitBit
                           + " where ID <= " + newDavka.ToString());

    }

  }
}