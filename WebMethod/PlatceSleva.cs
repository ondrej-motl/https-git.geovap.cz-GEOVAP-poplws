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
 public class SlevaPlatceKO
 {
     /// <summary>
     /// popis vazby je v PoplWS- Výměna dat za komOdpad.*
     /// metoda je urcena pro data představující slevy. K exportu je urcena  je určena metoda DejPlatceZaKO. 
     /// Vyexportovanou davku je potreba do 30 dnu naimportovat, jinak je původni export odstranen a je potreba
     /// export provést znovu.
     /// 
     /// uziv. musi mit select, insert, update, delete on P_ODPADY_EULEVY
     ///
     /// </summary>
     /// <param name="sesna"></param>
     /// <param name="EXT_APP_KOD"></param>
     /// <param name="inParams"></param>
     /// pokud je inParams.VS ="*" jsou vráceni všichni poplatníci
     /// <returns></returns>
     public POPLATNIK_SLEVA_RESP PlatceSlevaKO(Session sesna, int EXT_APP_KOD, POPLATNIK_SLEVA inParams)
     {
         POPLATNIK_SLEVA_RESP Resp = new POPLATNIK_SLEVA_RESP();
         
         Resp.status = Status.ERROR;
         Resp.result = Result.ERROR;

         sesna.DropIdentityMap();

         string poplKO = string.Empty;
         decimal popl;
      try
      {

         
          #region kontrola vyberovych parametru

          if (string.IsNullOrWhiteSpace(inParams.PERIODA))
              throw new Exception("Výběrová podmínka PERIODA není zadána.");
          if (string.IsNullOrWhiteSpace(inParams.DAVKA_ID))
              throw new Exception("Výběrová podmínka DAVKA_ID není zadána.");
          if (inParams.POPLATEK == null)
              throw new Exception("Výběrová podmínka POPLATEK není zadán.");

        #endregion kontrola vyberovych parametru


        #region kontrola prava na poplatek
        if (EXT_APP_KOD == null)
        { throw new Exception("kód externí aplikace není zadán"); }

        KONTROLA_POPLATKU kp = new KONTROLA_POPLATKU(sesna, EXT_APP_KOD);
        if (!kp.EAexist())
        { throw new Exception("chybný kód externí aplikace"); }
        
        popl = decimal.ToInt16(inParams.POPLATEK);
        if (!kp.existPravoNadPoplatkem(popl))
        { //throw new Exception("k typu pohledávky neexistuje oprávnění"); 
            throw new Exception("Ext. aplikace nemá oprávnění k typu pohledávky.");
        }
        #endregion kontrola prava na poplatek

        #region kontrola prava na vkladani predpisu
        PravoNadPoplatkem pnp = null;
        try
        {
            pnp = new PravoNadPoplatkem(sesna);
        }
        catch (Exception)
        {
            throw new Exception("kontrola přístup. práv uživatele nad daty Příjmové agendy skončila chybou");
        }

        //pro platce je povoleno prohlizeni
        if (!pnp.PravoExist(Decimal.ToInt16(popl), PravoNadPoplatkem.PrtabTable.PRPL, PravoNadPoplatkem.SQLPerm.INSERT))
            throw new Exception("PoplWS - nedostatečná oprávnění pro vkládání předpisů.");

        #endregion kontrola prava na vkladani predpisu

        DBUtil dbu = new DBUtil(sesna);
        bool jeToPoplKO = dbu.IsPoplKO(inParams.POPLATEK, out poplKO);
        #region pouze poplatek za KO
        if (! jeToPoplKO) 
            throw new Exception(string.Format("Kontrola přístup. práv skončila chybou - ({0}) nejedná se o poplatek za kom. odpad.", inParams.POPLATEK));
        #endregion pouze poplatek za KO

        #region perioda opdovida spravnemu poplatku
        string poplDlePer = dbu.getPoplzPerKod(inParams.PERIODA, poplKO);
        if (poplDlePer != popl.ToString() )
            throw new Exception(string.Format("Kontrola přístup. práv skončila chybou. \n Perioda \"{0}\" neodpovídá poplatku \"{1}\".", inParams.PERIODA, popl));

        #endregion perioda opdovida spravnemu poplatku

        #region davka je max. 30 dni od exportu
        P_ODPADY_EULEVY ulevy = sesna.FindObject<P_ODPADY_EULEVY>(CriteriaOperator.Parse("DAVKA = ? and ZPRAC = 'E' ", inParams.DAVKA_ID));
        if (ulevy == null)
            throw new Exception(string.Format("Exportovaná dávka \"{0}\" není platnou dávkou  pro import.", inParams.DAVKA_ID));

        int stariDat = int.MinValue;
        int.TryParse(System.Web.Configuration.WebConfigurationManager.AppSettings["KOExpDatExpiraceDni"], out stariDat); //pri neuspechu je stariDat=0

        DBValue dbv = DBValue.Instance(sesna);
        if (ulevy.ENTRYDATE <= dbv.DBSysDate.AddDays(stariDat * -1))
            throw new Exception(string.Format("Importovaná data jsou starší než povolených {0} dní od data exportu.", stariDat));
        #endregion davka je max. 30 dni od exportu


        if (inParams.SLEVA != null && inParams.SLEVA.Count() > 0)
        {
            if (!UtilDataKO.smazStarouDavkuSlev(sesna, EXT_APP_KOD, inParams.DAVKA_ID, inParams.PERIODA, ref Resp))
                return Resp;

            Resp.result = Result.OK;
            Resp.status = Status.EXISTS;
            UtilDataKO.ulozSlevyDoDB(sesna, EXT_APP_KOD, inParams, ref Resp); 
        }
        else
        {
            Resp.result = Result.ERROR;
            Resp.status = Status.NOTEXISTS;
            Resp.ERRORMESS = "Pro import slev nebyla zaslána žádná sleva.";
        }

        return Resp;
      }
      catch (Exception exc)
      {
          Resp.result = Result.ERROR;
          Resp.status = Status.ERROR;

          if (exc.InnerException == null)
          {
              Resp.ERRORMESS = exc.Message;
          }
          else
          {
              Resp.ERRORMESS = exc.InnerException.Message;
          }
          //
          //  throw new Exception(String.Format("chyba \n {0}", exc.InnerException.Message));
          //
          return Resp;
      }
     }
 }
}