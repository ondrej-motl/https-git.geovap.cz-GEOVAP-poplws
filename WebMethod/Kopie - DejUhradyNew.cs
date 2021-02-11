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
  public class DejUhradyNew
  {
    #region DejUhrady
    /// <summary>
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
    public UHRADY_NEW_RESP DejUhrady(Session session, int EXT_APP_KOD, string Davka)
    {
      /*
        Guid g = Guid.NewGuid(); 
        string davka = g.ToString();
 
            Guid.NewGuid().ToString() => 36 characters (Hyphenated)
            outputs: 12345678-1234-1234-1234-123456789abc

            Guid.NewGuid().ToString("D") => 36 characters (Hyphenated, same as ToString())
            outputs: 12345678-1234-1234-1234-123456789abc

            Guid.NewGuid().ToString("N") => 32 characters (Digits only)
            outputs: 12345678123412341234123456789abc
      */
      Session sesna = session;

      UHRADY_NEW_RESP plResp = new UHRADY_NEW_RESP();
      plResp.status = Status.NOTEXISTS;
      plResp.DAVKA = Davka;
      KONTROLA_POPLATKU kp = null;
      int EAbit;
      int oldDavka;
      int callDavka; 

      object obj = null;

      #region kontrola vsupnich udaju
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
          EAbit = (int)obj;
        else
        { throw new Exception("chybný kód externí aplikace"); }


        if ( !Int32.TryParse(Davka, out callDavka) )  //nanplni callDavka
         { throw new Exception("chybné číslo dávky"); }

      }
      catch (Exception exc)
      {
        plResp.result = Result.ERROR;

        if (exc.InnerException == null)
        {
          plResp.ERRORMES = exc.Message;
        }
        else
        {
          plResp.ERRORMES = exc.InnerException.Message;
        }
        return plResp;
        /*
          throw new Exception(String.Format("chyba \n {0}", exc.InnerException.Message));
         */
      }
      #endregion kontrola vstupnich udaju

      //   platby, uvazuji pouze platby plusove i minusove, 
      //   zaporne predpisy neuvazuji, predpisy obsluhuje ext. aplikace a proto by o nich mela vedet


      obj = sesna.ExecuteScalar("select MAX(DAVKA) poslDavka from P_GEO_ODBERPL where EA = " + EXT_APP_KOD.ToString());
      if (obj != null)
        oldDavka = (int)obj;
      else
        oldDavka = -1;

      VytvorDavku zpracujDavku;
      if ((oldDavka == callDavka)) // || (callDavka == -1))
        zpracujDavku = new VytvorDavku(NovaData);
      else
        zpracujDavku = new VytvorDavku(StaraData);

      int newDavka = -1;
      try
      {
          sesna.ExplicitBeginTransaction();
          zpracujDavku(oldDavka, callDavka, EXT_APP_KOD, EAbit, sesna, out newDavka);
          sesna.ExplicitCommitTransaction();

/*          string predatDavku = string.Empty;
          if (zpracujDavku == NovaData)
          {
            if (newDavka > callDavka)
              predatDavku = " DAVKA > " + callDavka.ToString();
            else
              predatDavku = "1 = 2";
          }

          if (zpracujDavku == StaraData)
          {
            predatDavku = " DAVKA > " + callDavka.ToString();
          }
        */
          if (callDavka == -1)
            oldDavka = -1;

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
            cmd.Append("   and exists (select 1 from P_PRPL, P_EXTAPP_POPL eap where PRID = PRPL_ID ");
            cmd.Append("               and eap.ID = " + EXT_APP_KOD.ToString());
            cmd.Append("               and eap.POPLATEK = PRPL_POPLATEK) ");
            cmd.Append("  group by PLATBA_ID, PLATBA_VS, PLATBA_PLDATE, PLATBA_NAUCETDNE, ");
            cmd.Append("      PLATBA_SS, PLATBA_BANKSPOJ, PLATBA_BANKU, PLATBA_PLATCE, PLATBA_DOKLAD, PLATBA_POKLDOK, ");
            cmd.Append("      PLATBA_POZNAMKA");

            SelectedData resultSet = sesna.ExecuteQueryWithMetadata(cmd.ToString());

            foreach (var row in resultSet.ResultSet[1].Rows) //.ResultSet[1] obsahuje vlastni data
            {
              PLATBA platba = new PLATBA();
              resultSet.ResultSet[1].Rows[0].copyToObject<PLATBA>(resultSet, platba);
              plResp.UHRADY.Add(platba);
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
      catch (SqlExecutionErrorException exc)  //XPO exception
      // catch (Exception)
      {
        sesna.ExplicitRollbackTransaction();
        plResp.result = Result.ERROR;
        if (exc.InnerException == null)
        {
          plResp.ERRORMES = exc.Message;
        }
        else
        {
          plResp.ERRORMES = exc.InnerException.Message;
        }

        return plResp;
      }

 /*
   #region nahraj novou davku 
     start transaction 
        --vycistim tabulku 
         delete from P_GEO_PAR where ZPRACOVAT = EAbit
      
      
        --odmazu stara data. Jsou uspesne predana, protoze je volano s poslednim cislem davky
         delete from P_GEO_ODBERPL where EA = 
                           and DAVKA < oldDavka 

         --vlozim nova data
         insert into P_GEO_ODBERPL (DAVKA, EA, PRID, PLID, KC)
           select MAX(ID) DAVKA, EA, PRID, PLID, sum(KC) from P_GEO_PAR a
             where (PRID > 0 and PLID > 0)      --kladna platba k predpisu
               and ZPRACOVAT ...
               and ID > oldDavka  
         group by PRID, PLID
     
        select MAX(ID) newDavka from P_GEO_ODBERPL where EA = 
      
        --odnastaveni EAbitu 
        update P_GEO_PAR set ZPRACOVAT = 
           where ID <= newDavka
    commit transaction      
    
 
    #end region nahraj novou davku 

    #region dej znovu data
     start transaction 
       
         --vlozim nova data od minuleho pozadavku
         insert into P_GEO_ODBERPL (DAVKA, EA, PRID, PLID, KC)
           select MAX(ID) DAVKA, EA, PRID, PLID, sum(KC) from P_GEO_PAR a
             where (PRID > 0 and PLID > 0)      --kladna platba k predpisu
               and ZPRACOVAT ...
               and ID > poslDavka  
         group by PRID, PLID
     
        select MAX(ID) newDavka from P_GEO_ODBERPL where EA = 

        if newDavka = poslDavka  
             then   nejsou nove platby a proto koncim
   
        --data z posledniho neuspesneho dotazu pridam do noveho dotazu 
        update P_GEO_ODBERPL set DAVKA = newDavka where DAVKA = poslDavka     
      
        --odnastaveni EAbitu 
        update P_GEO_PAR set ZPRACOVAT = 
           where ID <= newDavka
  
     commit transaction      

    #endregion dej znovu data

      
      --vyberu a poslu data 
         select SUM(KC) KC, PLATBA_ID, PLATBA_VS, ...  from P_PLATBA, P_GEO_ODBERPL
           where EA = 
             and PLATBA_ID = PLID   
             and exists (select 1 from P_PRPL, P_EXTAPP_POPL eap where PRID = PRPL_ID
                           and eap.ID = EA ..
                           and eap.POPLATEK = PRPL_POPLATEK) 
      
      
   
         //MSSQL where ZPRACOVAT | 2 - 2 = 0   //je nastaven jen druhy bit 
                               ZPRACOVAT = 2   //je nastaven jen druhy bit 
         //MSSQ  where PRPL_PR & 8 = 8 //je nastaven 4. bit
         //MSSQ  nastaveni 3. bitu    set PRPL_PR = (PRPL_PR | 4)  
         //MSSQL odnastaveni 4 bitu   set PRPL_PR = ( PRPL_PR  | 8 ) - 8 
         //ORACLE where bitand(PRPL_PR, 8) = 8 //je nastaven 4. bit
         //ORACLE where PRPL_PR - bitand(PRPL_PR, 4) = 0 //je nastaven jen treti bit 
                                             PRPL_PR = 4 //je nastaven jen treti bit 
         //ORACLE bitove or    PRPL_PR - bitand(PRPL_PR, 4)
         //ORACLE nastaveni 3.bitu    set PRPL_PR = PRPL_PR - bitand(PRPL_PR, 4) + 4
         //ORACLE odnastaveni 4 bitu  set PRPL_PR = PRPL_PR - bitand(PRPL_PR, 8) 
        */


    }
    #endregion  //end DejUhrady

    private delegate void VytvorDavku(int oldDavka, int callDavka, int EA, int EAbit, Session sesna, out int newDavka);
    
    private void NovaData(int oldDavka, int callDavka, int EA, int EAbit, Session sesna, out int newDavka)
    {
      //vycistim tabulku 
      sesna.ExecuteNonQuery("delete from P_GEO_PAR where ZPRACOVAT = " + EAbit.ToString());

      //odmazu stara data. Jsou uspesne predana, protoze je volano s poslednim cislem davky
      sesna.ExecuteNonQuery(String.Format("delete from P_GEO_ODBERPL where EA = {0} and DAVKA < {1}",
                                            EA, oldDavka));

      DBVal dbv = new DBVal(sesna);
      string nastavenyBit = String.Empty;
      if (dbv.CreateDBV().GetType() == typeof(MSSQL))
           nastavenyBit = "ZPRACOVAT & " + EAbit.ToString();    //PRPL_PR & 8 = 8 //je nastaven 4. bit
      if (dbv.CreateDBV() is ORACLE)                 
           nastavenyBit = "bitand(ZPRACOVAT," + EAbit.ToString(); //bitand(PRPL_PR, 8) = 8 //je nastaven 4. bit

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
        newDavka = (int)obj;
      else
        newDavka = oldDavka;

  /*    sesna.ExecuteNonQuery("update P_GEO_ODBERPL set DAVKA = " + newDavka.ToString()
                           + " where DAVKA > " + oldDavka.ToString()
                           + "  and DAVKA < " + newDavka.ToString());
  */

      //odnastaveni EAbitu 
      string nastavitBit = String.Empty;
      if (dbv.CreateDBV().GetType() == typeof(MSSQL))
        nastavitBit = String.Format(@" (ZPRACOVAT | {0} ) - {0}", EAbit);   //odnastaveni 4 bitu   set PRPL_PR = ( PRPL_PR  | 8 ) - 8 
      if (dbv.CreateDBV() is ORACLE)                 
        nastavitBit = String.Format(@"ZPRACOVAT - bitand(ZPRACOVAT, {0})", EAbit);     //odnastaveni 4 bitu  set PRPL_PR = PRPL_PR - bitand(PRPL_PR, 8) 
      sesna.ExecuteNonQuery("update P_GEO_PAR set ZPRACOVAT = " + nastavitBit
                           + " where ID <= " + newDavka.ToString() );

    }

    private void StaraData(int oldDavka, int callDavka, int EA, int EAbit, Session sesna, out int newDavka)
    {

      DBVal dbv = new DBVal(sesna);
      string nastavenyBit = String.Empty;
      if (dbv.CreateDBV().GetType() == typeof(MSSQL))
        nastavenyBit = "ZPRACOVAT & " + EAbit.ToString();    //PRPL_PR & 8 = 8 //je nastaven 4. bit
      if (dbv.CreateDBV() is ORACLE)
        nastavenyBit = "bitand(ZPRACOVAT," + EAbit.ToString(); //bitand(PRPL_PR, 8) = 8 //je nastaven 4. bit

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
        newDavka = (int)obj;
      else
        newDavka = oldDavka;

        //data z posledniho neuspesneho dotazu pridam do noveho dotazu 
      //sesna.ExecuteNonQuery(String.Format("update P_GEO_ODBERPL set DAVKA = {0} where DAVKA = {1} and EA = {2}", newDavka, oldDavka, EA));
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
        nastavitBit = String.Format(@" (ZPRACOVAT | {0} ) - {0}", EAbit);   //odnastaveni 4 bitu   set PRPL_PR = ( PRPL_PR  | 8 ) - 8 
      if (dbv.CreateDBV() is ORACLE)
        nastavitBit = String.Format(@"ZPRACOVAT - bitand(ZPRACOVAT, {0})", EAbit);     //odnastaveni 4 bitu  set PRPL_PR = PRPL_PR - bitand(PRPL_PR, 8) 
      sesna.ExecuteNonQuery("update P_GEO_PAR set ZPRACOVAT = " + nastavitBit
                           + " where ID <= " + newDavka.ToString());

    }

  }
}