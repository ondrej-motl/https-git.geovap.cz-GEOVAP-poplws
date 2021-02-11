using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml.Serialization;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Data.Filtering;
using System.Text;

namespace PoplWS.WebMethod
{
 public class NajdiPlatceKO
 {
     /// </summary>
     /// <param name="sesna"></param>
     /// <param name="EXT_APP_KOD"></param>
     /// <param name="inParams"></param>
     /// pokud neni inParams.VS uvedeny (string.IsNullOrEmpty()) jsou vráceni všichni poplatníci
     /// <returns></returns>
     public PLATCI_RESP DejPlatceKO(Session sesna, int EXT_APP_KOD, GET_PLATCE_PARAMS inParams)
     {
         PLATCI_RESP Resp = new PLATCI_RESP();
         
         Resp.status = Status.ERROR;
         Resp.result = Result.ERROR;

         sesna.DropIdentityMap();

         int popl;
         StringBuilder cmd = new StringBuilder();

         string VSlog = null;

      try
      {

         
          #region kontrola vyberovych parametru
          if (string.IsNullOrWhiteSpace(inParams.PERIODA))
              throw new Exception("Výběrová podmínka PERIODA není zadána.");

          popl = decimal.ToInt16(inParams.POPLATEK);

        #endregion kontrola vyberovych parametru


        #region kontrola prava aplikace na poplatek
        if (EXT_APP_KOD == null)
        { throw new Exception("kód externí aplikace není zadán"); }

        KONTROLA_POPLATKU kp = new KONTROLA_POPLATKU(sesna, EXT_APP_KOD);
        if (!kp.EAexist())
        { throw new Exception("chybný kód externí aplikace"); }
        if (!kp.existPravoNadPoplatkem(popl))
        { 
            throw new Exception("Ext. aplikace nemá oprávnění k typu pohledávky.");
        }
        #endregion kontrola prava aplikace na poplatek

        #region kontrola prava na cteni platce
        PravoNadPoplatkem pnp = null;
        try
        {
            pnp = new PravoNadPoplatkem(sesna);
        }
        catch (Exception)
        {
            throw new Exception("kontrola přístup. práv uživatele nad daty Příjmové agendy skončila chybou");
        }

        if (!pnp.PravoExist(popl, PravoNadPoplatkem.PrtabTable.RGP, PravoNadPoplatkem.SQLPerm.SELECT))
            throw new Exception("PoplWS - nedostatečná oprávnění pro čtení plátce.");

        #endregion kontrola prava na cteni platce


        DBUtil dbu = new DBUtil(sesna);
        #region perioda s rocni periodicitou
        C_EVPOPL evp = dbu.GetEvpopl(popl, inParams.PERIODA);
        if (evp.CompoundKey1.EVPOPL_PER.PERIODA_POC != 1)
            throw new Exception(string.Format("Kontrola přístup. práv skončila chybou. \n Perioda \"{0}\" musí mít periodicitu = \"1\".", inParams.PERIODA));
        #endregion perioda s rocni periodicitou

        string poplKO;
        bool jeToPoplKO = dbu.IsPoplKO(inParams.POPLATEK, out poplKO);

        #region pouze poplatek za KO
        if (!jeToPoplKO) 
            throw new Exception(string.Format("Kontrola přístup. práv skončila chybou - ({0}) nejedná se o poplatek za kom. odpad.", inParams.POPLATEK));

        #endregion pouze poplatek za KO

        #region perioda opdovida spravnemu poplatku
        string poplDlePer = dbu.getPoplzPerKod(inParams.PERIODA, poplKO);
        if (poplDlePer != popl.ToString())
            throw new Exception(string.Format("Kontrola přístup. práv skončila chybou. \n Perioda \"{0}\" neodpovídá poplatku \"{1}\".", inParams.PERIODA, popl));

        #endregion perioda opdovida spravnemu poplatku


#if ! DEBUG  //region pouze pro periodicitu s jednou periodou za rok

        #region pouze pro periodicitu s jednou periodou za rok
          C_PERIODA per = sesna.FindObject<C_PERIODA>(CriteriaOperator.Parse("PERIODA_PERIODA = ?", inParams.PERIODA));
          try 
	        {	        
               if ((per != null) && (per.PERIODA_POC != 1))
		          throw new Exception(string.Format("Data za kom. odpad lze získat pouze za periodu s roční periodicitou."));
	        }
	        catch (Exception e)
	        {
		        throw new Exception(string.Format("Kontrola periodicity. \n {0}", e.Message));
            }

        #endregion pouze pro periodicitu s jednou periodou za rok
#endif //DEBUG

        PLATCE2 platce = null;


        bool dataSmazana = false;
        string message;
        UtilDataKO.smazStarouDavku(sesna, EXT_APP_KOD, inParams.PERIODA, out dataSmazana, ref Resp);
        if (!dataSmazana)
           return Resp;
            

        DBVal dbv = new DBVal(sesna);
        IDBValFactory idbv = dbv.CreateDBV();

        cmd.Append("select distinct bl.LIKEVS_VS POPL_VS, b.ADR_NAZEV1 POPL_NAZEV, al.LIKEVS_VS ZAST_VS, a.ADR_NAZEV1 ZAST_NAZEV,");
        cmd.Append("  a.ADR_DATNAR ZAST_DATNAR, b.ADR_DATNAR POPL_DATNAR, bl.LIKEVS_POHL, a.ADR_ICZUJ_NAZEV,");
        if (idbv.Database == typeof(MSSQL))   
        {
             cmd.Append("     dbo.P_ADRESA_ULICE(a.ADR_KODCOB_NAZEV, a.ADR_KODUL_NAZEV, a.ADR_CIS_DOMU, a.ADR_CIS_OR, a.ADR_ZNAK_CO) ZAST_ADR,");
             cmd.Append("     dbo.P_ADRESA_ULICE(b.ADR_KODCOB_NAZEV, b.ADR_KODUL_NAZEV, b.ADR_CIS_DOMU, b.ADR_CIS_OR, b.ADR_ZNAK_CO) POPL_ADR,");
        }
        else 
        {
             cmd.Append("     P_ADRESA_ULICE(a.ADR_KODCOB_NAZEV, a.ADR_KODUL_NAZEV, a.ADR_CIS_DOMU, a.ADR_CIS_OR, a.ADR_ZNAK_CO) ZAST_ADR,");
             cmd.Append("     P_ADRESA_ULICE(b.ADR_KODCOB_NAZEV, b.ADR_KODUL_NAZEV, b.ADR_CIS_DOMU, b.ADR_CIS_OR, b.ADR_ZNAK_CO) POPL_ADR,");
        }
        cmd.Append(" RGP_KCROK POPL_KC, RGP_POPLATEK, RGP_PER ");
        cmd.Append("from P_ADRESA_ROBRHS a, P_LIKEVS al, P_ODPADY_MAJITEL, P_ODPADY_BYDLIC,");
        cmd.Append("  P_ADRESA_ROBRHS b, P_LIKEVS bl, P_RGP");
        cmd.Append(" where a.ADR_ICO = al.LIKEVS_ICO");
        //cmd.Append("	  and al.LIKEVS_POPLATEK = (select KONF_POPLATEK from P_ODPADY_KONFIG)");
        cmd.Append("	  and al.LIKEVS_POPLATEK = " + popl.ToString());
        cmd.Append("	  and al.LIKEVS_ICO = MAJITEL_ICO");
        cmd.Append("	  and MAJITEL_OBJEKT_BYT = BYDLICI_OBJEKT_BYT");
        cmd.Append("	  and BYDLICI_OBJEKT_BYT > -1");
        cmd.Append("	  and BYDLICI_DATUM_OD < " + idbv.getParamText("KONEC_ROKU"));
        cmd.Append("      and (BYDLICI_DATUM_DO >= " + idbv.getParamText("ZACATEK_ROKU")+ " or BYDLICI_DATUM_DO is null)");
        cmd.Append("	  and BYDLICI_OSVOBOZEN = 'N' ");
        cmd.Append("	  and b.ADR_ICO = bl.LIKEVS_ICO");
        //cmd.Append("	  and bl.LIKEVS_POPLATEK = (select KONF_POPLATEK from P_ODPADY_KONFIG)");
        cmd.Append("	  and bl.LIKEVS_POPLATEK = " + popl.ToString());
        cmd.Append("	  and bl.LIKEVS_POPLATEK = RGP_POPLATEK");
        cmd.Append("	  and bl.LIKEVS_PORVS = RGP_PORVS");
        cmd.Append("	  and RGP_KCROK > 0");
        cmd.Append("	  and bl.LIKEVS_ICO = BYDLICI_ICO");
        cmd.Append("	  and RGP_PER = MAJITEL_PERIODA");
        cmd.Append("	  and BYDLICI_PERIODA = MAJITEL_PERIODA");
        if (!string.IsNullOrEmpty(inParams.VS) && (inParams.VS != "*"))
            cmd.Append("	  and bl.LIKEVS_VS = '" + inParams.VS + "'");
        cmd.Append("      and BYDLICI_PERIODA = '" + inParams.PERIODA + "'");
        cmd.Append("UNION ");
        cmd.Append("select distinct al.LIKEVS_VS POPL_VS, a.ADR_NAZEV1 POPL_NAZEV, al.LIKEVS_VS ZAST_VS, a.ADR_NAZEV1 ZAST_NAZEV,");
        cmd.Append("  a.ADR_DATNAR ZAST_DATNAR, a.ADR_DATNAR POPL_DATNAR, al.LIKEVS_POHL, a.ADR_ICZUJ_NAZEV,");
        if (idbv.Database == typeof(MSSQL))   
        {
             cmd.Append("     dbo.P_ADRESA_ULICE(a.ADR_KODCOB_NAZEV, a.ADR_KODUL_NAZEV, a.ADR_CIS_DOMU, a.ADR_CIS_OR, a.ADR_ZNAK_CO) ZAST_ADR,");
             cmd.Append("     dbo.P_ADRESA_ULICE(a.ADR_KODCOB_NAZEV, a.ADR_KODUL_NAZEV, a.ADR_CIS_DOMU, a.ADR_CIS_OR, a.ADR_ZNAK_CO) POPL_ADR,");
        }
        else 
        {
             cmd.Append("     P_ADRESA_ULICE(a.ADR_KODCOB_NAZEV, a.ADR_KODUL_NAZEV, a.ADR_CIS_DOMU, a.ADR_CIS_OR, a.ADR_ZNAK_CO) ZAST_ADR,");
             cmd.Append("     P_ADRESA_ULICE(a.ADR_KODCOB_NAZEV, a.ADR_KODUL_NAZEV, a.ADR_CIS_DOMU, a.ADR_CIS_OR, a.ADR_ZNAK_CO) POPL_ADR,");
        }
        cmd.Append(" RGP_KCROK POPL_KC, RGP_POPLATEK, RGP_PER ");
        cmd.Append("from P_ADRESA_ROBRHS a, P_LIKEVS al, P_ODPADY_BYDLIC, P_RGP");
        cmd.Append("   where a.ADR_ICO = al.LIKEVS_ICO");
        //cmd.Append("	  and al.LIKEVS_POPLATEK = (select KONF_POPLATEK from P_ODPADY_KONFIG)");
        cmd.Append("	  and al.LIKEVS_POPLATEK = " + popl.ToString());
        cmd.Append("	  and al.LIKEVS_ICO = BYDLICI_ICO");
        cmd.Append("	  and BYDLICI_OBJEKT_BYT = -1 ");
        cmd.Append("	  and BYDLICI_DATUM_OD < " + idbv.getParamText("KONEC_ROKU"));
        cmd.Append("      and (BYDLICI_DATUM_DO >= " + idbv.getParamText("ZACATEK_ROKU")+ " or BYDLICI_DATUM_DO is null)");
        cmd.Append("	  and BYDLICI_OSVOBOZEN = 'N' ");
        cmd.Append("      and BYDLICI_PERIODA = '" + inParams.PERIODA + "'");
        cmd.Append("	  and al.LIKEVS_POPLATEK = RGP_POPLATEK");
        cmd.Append("	  and al.LIKEVS_PORVS = RGP_PORVS");
        cmd.Append("	  and RGP_PER = BYDLICI_PERIODA");
        if (!string.IsNullOrEmpty(inParams.VS) && (inParams.VS != "*"))
            cmd.Append("	  and al.LIKEVS_VS = '" + inParams.VS + "'");
        cmd.Append("	  and RGP_KCROK > 0");
        cmd.Append(" order by 3");

                  List<string> paramNames = new List<string>();
                  paramNames.Add("ZACATEK_ROKU");
                  paramNames.Add("KONEC_ROKU");

                  int rok = Convert.ToInt16(dbv.DBSysDate.ToString("yyyy"));
                  DateTime zacRoku = new DateTime(rok, 1, 31).Date;
                  DateTime konRoku = new DateTime(rok, 12, 31).Date;
                  List<object> paramValues = new List<object>();
                  paramValues.Add(zacRoku);     //ZACATEK_ROKU
                  paramValues.Add(konRoku);   //KONEC_ROKU

          SelectedData resultSet = sesna.ExecuteQueryWithMetadata(cmd.ToString(), paramNames.ToArray(), paramValues.ToArray());

          Resp.RGP_POPLATEK = inParams.POPLATEK;
          Resp.RGP_PER = inParams.PERIODA;
          int stariDat = int.MinValue;
          int.TryParse(System.Web.Configuration.WebConfigurationManager.AppSettings["KOExpDatExpiraceDni"], out stariDat);  
          Resp.PLATNOST_DNI = stariDat;
          foreach (var row in resultSet.ResultSet[1].Rows) 
          {
            platce = new PLATCE2();
            platce.POPLATNIK = new POPLATNIK();
            platce.ZASTUPCE = new ZASTUPCE();

            VSlog = (string)row.ValByName(resultSet, "POPL_VS");
            if (row.ValByName(resultSet, "POPL_KC") != null)
              platce.POPLATNIK.RGP_KCROK = Convert.ToDecimal(row.ValByName(resultSet, "POPL_KC"));
            if (row.ValByName(resultSet, "POPL_DATNAR") != null)
              platce.POPLATNIK.ROK_NAROZENI = Convert.ToInt16(((DateTime)row.ValByName(resultSet, "POPL_DATNAR")).ToString("yyyy")); 
            platce.POPLATNIK.VS = (string)row.ValByName(resultSet, "POPL_VS");
            platce.POPLATNIK.NAZEV = (string)row.ValByName(resultSet, "POPL_NAZEV");
            platce.POPLATNIK.ADRESA = (string)row.ValByName(resultSet, "POPL_ADR");
            if (!string.IsNullOrEmpty((string)row.ValByName(resultSet, "ADR_ICZUJ_NAZEV")))
                platce.POPLATNIK.ADRESA = platce.POPLATNIK.ADRESA + ", " + (string)row.ValByName(resultSet, "ADR_ICZUJ_NAZEV");
            platce.POPLATNIK.LIKEVS_DLUH = Convert.ToInt16(row.ValByName(resultSet, "LIKEVS_POHL")) > 0 ? 1 : 0;

            if (row.ValByName(resultSet, "ZAST_DATNAR") != null) 
                platce.ZASTUPCE.ROK_NAROZENI = Convert.ToInt16(((DateTime)row.ValByName(resultSet, "ZAST_DATNAR")).ToString("yyyy"));
            platce.ZASTUPCE.VS = (string)row.ValByName(resultSet, "ZAST_VS");
            platce.ZASTUPCE.NAZEV = (string)row.ValByName(resultSet, "ZAST_NAZEV");
            platce.ZASTUPCE.ADRESA = (string)row.ValByName(resultSet, "ZAST_ADR");
            platce.POPLATNIK.PERIODA = (string)row.ValByName(resultSet, "RGP_PER");
            platce.ZASTUPCE.PERIODA = platce.POPLATNIK.PERIODA;
      
            Resp.PLATCI.Add(platce);  
          }

          

          Resp.result = Result.OK;
          if ((platce != null) && (Resp.PLATCI.Count() > 0))
          {
              Resp.status = Status.EXISTS;
              UtilDataKO.ulozDavkuDoDB(sesna, EXT_APP_KOD, ref Resp); 
          }
          else
              Resp.status = Status.NOTEXISTS;

          return Resp;
      }
      catch (Exception exc)
      {
          Resp.result = Result.ERROR;
          Resp.status = Status.ERROR;

          if (exc.InnerException == null)
          {
              Resp.ERRORMESS = VSlog + "\n" + exc.Message;
          }
          else
          {
              Resp.ERRORMESS = VSlog + "\n" + exc.InnerException.Message;
          }
          /*
            throw new Exception(String.Format("chyba \n {0}", exc.InnerException.Message));
            */
#if DEBUG 
          if (cmd.Capacity > 0)
          {

              Resp.ERRORMESS = Resp.ERRORMESS + "\n" + cmd.ToString();
              //throw new Exception("SQL uloženo do Clipboardu. " + cmd.ToString());
          }
#endif
          return Resp;
      }

     }
 }
}