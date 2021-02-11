using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;


namespace PoplWS.WebMethod
{
  public class PredpisInsert
  {
    public PREDPIS_RESP InsertPredpis(Session session, int EXT_APP_KOD, PREDPIS predpis)
      {
        return InsertPredpis(session, EXT_APP_KOD, predpis, false);
      }

    public PREDPIS_RESP InsertPredpis(Session session, int EXT_APP_KOD, PREDPIS predpis, bool nastavitKCdleDPH)
      {
        Session sesna = session;
        PREDPIS_RESP Resp = new PREDPIS_RESP();
        CriteriaOperator criteria;
        //Session sesna = predpis.sesna;

        sesna.DropIdentityMap();

        predpis.PRPL_ID = 0;
        predpis.PRPL_EXTVS = null;
        predpis.PRPL_EA = EXT_APP_KOD;

        try
        {
#region test  vyplneni vstupnich hodnot

          if (EXT_APP_KOD == null)
          { throw new Exception("kód externí aplikace není zadán"); }
          P_EXTAPP EA = sesna.GetObjectByKey<P_EXTAPP>(EXT_APP_KOD);
          if (EA == null)
          { throw new Exception("chybný kód externí aplikace"); }



          if (predpis.PLATCE_ID == 0)
          { throw new Exception("údaj PLATCE_ID není zadán"); }

          if (predpis.PRPL_PREDPIS == 0)
          { throw new Exception("údaj KC není zadán"); }

          if (predpis.PRPL_VYSTAVENO == DateTime.MinValue)
          { throw new Exception("údaj VYSTAVENO_DNE není zadán"); }

          if ((predpis.RADKY_DPH.Count > 0) && (predpis.PRPL_UZP == null))
          { throw new Exception("údaj DATUM_UZP není zadán"); }

          if (predpis.RADKY_DPH.Count > 0)
          { 
            P_NASTAVENI nast = sesna.GetObjectByKey<P_NASTAVENI>("OTEVR_UZP");
            string s = nast.HODNOTA.Substring(1, nast.HODNOTA.IndexOf('|') - 1);
            DateTime otevrUZP = DateTime.ParseExact(s, "d.M.yyyy", null);
            otevrUZP = new DateTime(otevrUZP.Year, otevrUZP.Month, 1);
            if (predpis.PRPL_UZP < otevrUZP)
            { throw new Exception("DATUM_UZP není v otevřeném období UZP"); }
          }

          //otevrene obdobi
          {
            P_NASTAVENI nast = sesna.GetObjectByKey<P_NASTAVENI>("OTEVR_OBDOBI");
            string obd = "01." + nast.HODNOTA;
            DateTime otevrObd = DateTime.ParseExact(obd, "d.M.yyyy", null);
            if (predpis.PRPL_VYSTAVENO < otevrObd)
              { throw new Exception("datum vystavení není v otevřeném období"); }
          }     

          //v minulosti
          {
            DBValue dbv = DBValue.Instance(sesna);
            if (predpis.PRPL_VYSTAVENO < dbv.DBSysDate)
              throw new Exception("datum vystavení nemůže být v minulosti"); 
          }

#endregion test  vyplneni vstupnich hodnot

        #region test, zda existuje v P_RGP, dohledani zbytku klice
        XPQuery<P_RGP> Qplce = sesna.Query<P_RGP>();
        XPCollection<P_RGP> plces = new XPCollection<P_RGP>(sesna, false);  //loadingEnabled = false;
        P_RGP[] rgps = Qplce.Where(a => a.RGP_ID == predpis.PLATCE_ID).ToArray<P_RGP>();
        plces.AddRange(rgps);

        if (plces.Count == 0)
        { throw new Exception(String.Format(@"poplatník s ID {0} neexistuje", predpis.PLATCE_ID)); }

        #endregion test, zda existuje v P_RGP, dohledani zbytku klice


        KONTROLA_POPLATKU kp = new KONTROLA_POPLATKU(sesna, EXT_APP_KOD);
        if (!kp.EAexist())
        { throw new Exception("chybný kód externí aplikace"); }
        if (!kp.existPravoNadPoplatkem(plces[0].CompoundKey1.RGP_POPLATEK))
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
        //pro predpisy je povoleno vkladani
        if (!pnp.PravoExist((int)plces[0].CompoundKey1.RGP_POPLATEK, PravoNadPoplatkem.PrtabTable.PRPL, PravoNadPoplatkem.SQLPerm.INSERT))
          throw new Exception("PoplWS - nedostatečná oprávnění");
    #endregion kontrola prava nad poplatkem

        P_RGP plce = plces[0];
        predpis.PRPL_PORPER = predpis.PRPL_PORPER == 0 ? 1 : predpis.PRPL_PORPER;

              DBUtil dbu = new DBUtil(sesna);
              C_EVPOPL evp = dbu.GetEvpopl(plce.CompoundKey1.RGP_POPLATEK, plce.CompoundKey1.RGP_PER);
              if (evp == null)
                throw new Exception("nepovolená kombinace poplatek / perioda");

              //datum splatnosti
              if (predpis.PRPL_SPLATNO == DateTime.MinValue)
              {
                predpis.PRPL_SPLATNO = dbu.GetDatumSplatnosti(predpis.PRPL_VYSTAVENO, evp, (short)predpis.PRPL_PORPER);
              }

              if ((predpis.PRPL_VYSTAVENO > predpis.PRPL_SPLATNO) && (evp.EVPOPL_SPLT_VYST != "A"))
                throw new Exception("datum splatnosti nemůže být starší než datum vystavení");

              //pocet period
              if (evp.CompoundKey1.EVPOPL_PER.PERIODA_POC < predpis.PRPL_PORPER)
                throw new Exception(String.Format("nepovolené pořadí periody \"{0}\", max. pořadí pro periodu \"{1}\" = {2}", predpis.PRPL_PORPER, plce.CompoundKey1.RGP_PER, evp.CompoundKey1.EVPOPL_PER.PERIODA_POC));
          
        using (MyUnitOfWork uow = new MyUnitOfWork(sesna.DataLayer))
        {
            P_PRPL prIns = new P_PRPL(uow);
            Utils.copy.CopyDlePersistentAttr<P_PRPL>(predpis, prIns);

            prIns.PRPL_ID = dbu.LIZNI_SEQ("PRPL_ID");
            prIns.CompoundKey.PRPL_POPLATEK = plce.CompoundKey1.RGP_POPLATEK;
            prIns.CompoundKey.PRPL_PER = plce.CompoundKey1.RGP_PER;
            prIns.CompoundKey.PRPL_ICO = plce.CompoundKey1.RGP_ICO;
            prIns.CompoundKey.PRPL_DOPLKOD = plce.CompoundKey1.RGP_DOPLKOD;
            prIns.PRPL_ROK = predpis.PRPL_ROK == 0 ? predpis.PRPL_VYSTAVENO.Year : predpis.PRPL_ROK;
            prIns.PRPL_PORPER = predpis.PRPL_PORPER == 0 ? 1 : predpis.PRPL_PORPER;
            prIns.PRPL_SS = predpis.PRPL_SS ?? evp.EVPOPL_SS;

            if (predpis.EXPORTOVAT_DO_FINANCI == null)
              prIns.PRPL_EXPFIN = plce.RGP_EXPUCTO;
            else
              prIns.PRPL_EXPFIN = predpis.EXPORTOVAT_DO_FINANCI.ToString();
            P_C_TYPDANE typdane = uow.GetObjectByKey<P_C_TYPDANE>(predpis.PRPL_TYPDANE);
            if (typdane != null)
            { prIns.PRPL_TYPDANE = typdane; }
            else
            {
              typdane = uow.GetObjectByKey<P_C_TYPDANE>((short)5);
              prIns.PRPL_TYPDANE = typdane;
            }

            prIns.PRPL_RECORD = dbu.GetRecordFromTypDane(prIns.PRPL_TYPDANE.TYPDANE_KOD);

            if (prIns.PRPL_RECORD == "-1")
              { throw new Exception(String.Format(@"chybný typ daně {0}", prIns.PRPL_TYPDANE)); }

            if (prIns.PRPL_RECORD == "P")
            {
              prIns.PRPL_SANKCE = prIns.PRPL_PREDPIS;
              prIns.PRPL_PREDPIS = 0;
            };

            prIns.PRPL_VS = plce.VS;

            prIns.PRPL_PORSANKCE = 0m;
            prIns.PRPL_PROCSANKCE = plce.RGP_PROCSANKCE;
            prIns.PRPL_VYSTUP = plce.RGP_VYSTUP;
            prIns.PRPL_TISK = "NE";
            prIns.PRPL_EXPORTOVANO = "NE";
            prIns.PRPL_UCETMESIC = predpis.PRPL_VYSTAVENO.Month;
            prIns.PRPL_UCETROK = predpis.PRPL_VYSTAVENO.Year;
            prIns.PRPL_OBDOBIMES = -1;
            prIns.PRPL_OBDOBIROK = -1;
            prIns.PRPL_TYPSANKCE = plce.RGP_TYPSANKCE;
            prIns.PRPL_PEVNACASTKA = plce.RGP_PEVNACASTKA;
            prIns.PRPL_NASOBEK = plce.RGP_NASOBEK;
            prIns.PRPL_PERNAS = plce.RGP_PERNAS;
            prIns.PRPL_STAVSANKCE = " ";
            prIns.PRPL_SPAROVANO = 0m;
            prIns.PRPL_EXPSIPO = "NE";
            prIns.PRPL_VYMAHAT = predpis.PRPL_VYMAHAT == null ? "N" : predpis.PRPL_VYMAHAT;
            prIns.PRPL_VYMAHANO = "N";
            prIns.PRPL_PR = 0;
            DBValue dbv = DBValue.Instance(sesna);
            prIns.LOGIN = dbv.DBUserName;
            prIns.LASTUPDATE = dbv.DBSysDateTime;
            prIns.ENTRYLOGIN = prIns.ENTRYLOGIN;
            prIns.ENTRYDATE = prIns.LASTUPDATE;
            if (! prIns.PRPL_OLDVS.HasValue)
               prIns.PRPL_OLDVS = plce.RGP_OLDVS;  

#region DPH - zkontrolovani celkove castky, zaokrouhleni
            decimal SumaRadkuDPH = predpis.PRPL_PREDPIS;
            List<DPHRozpis> radkyDph = new List<DPHRozpis>();
            if (predpis.RADKY_DPH.Count > 0)
            {
              DPHRozpis dpr;
              foreach (RADEK_DPH item in predpis.RADKY_DPH)
              {
                if (item.ZAKLAD == 0)
                { continue; }
                dpr = new DPHRozpis();
                Utils.copy.CopyDlePersistentAttr(item, dpr);
                dpr.DAN = item.ZAKLAD * ((item.SAZBA ?? 0) / 100);
                dpr.KC = dpr.ZAKLAD + dpr.DAN;
                radkyDph.Add(dpr);
              }

              DPHZaokrouhleni dphZaokr = new DPHZaokrouhleni();
              dphZaokr.DPHZaokrouhli(radkyDph, prIns.CompoundKey.PRPL_POPLATEK, sesna);

              SumaRadkuDPH = 0;
              foreach (var item in radkyDph)
              {
                SumaRadkuDPH += item.KC + item.ZAOKROUHLENI;
              }

              if ( nastavitKCdleDPH )  
                 predpis.PRPL_PREDPIS = SumaRadkuDPH;
              else
              {
                  if (SumaRadkuDPH != predpis.PRPL_PREDPIS)
                  {
                      throw new Exception(String.Format("nesouhlasí částka předpisu {0} \n s rozpisem DPH {1}",
                                                    predpis.PRPL_PREDPIS, SumaRadkuDPH));
                  }
              }
           }
#endregion DPH - zkontrolovani celkove castky, zaokrouhleni

            //povkladam radkyDph
            foreach (var item in radkyDph)
            {
              P_PRPL_DPH dphr = new P_PRPL_DPH(uow);
              DPHRozpisPredp tmp = new DPHRozpisPredp();
              Utils.copy.CopyDlePersistentAttr<DPHRozpisPredp>(item, tmp);
              Utils.copy.CopyDlePersistentAttr<P_PRPL_DPH>(tmp, dphr);
              prIns.P_PRPL_DPH.Add(dphr);
            }

            uow.CommitTransaction();

            Utils.copy.CopyDlePersistentAttr<PREDPIS_RESP>(predpis, Resp);
            Resp.result = Result.OK;
            Resp.status = Status.INSERTED;
            Utils.copy.CopyDlePersistentAttr<PREDPIS_RESP>(prIns, Resp);
            Resp.PRPL_TYPDANE = prIns.PRPL_TYPDANE.TYPDANE_KOD;
            switch (prIns.PRPL_EXPFIN)
            {
               case "ANO": 
                          Resp.EXPORTOVAT_DO_FINANCI = SouhlasAnoNe.ANO;
                          break; 
                  default:
                          Resp.EXPORTOVAT_DO_FINANCI = SouhlasAnoNe.NE;
                          break; 
            }

            foreach (var item in prIns.P_PRPL_DPH)
            {
              RADEK_DPH respDphRow = new RADEK_DPH();
              DPHRozpisPredp tmp = new DPHRozpisPredp();
              Utils.copy.CopyDlePersistentAttr<DPHRozpisPredp>(item, tmp);
              Utils.copy.CopyDlePersistentAttr<RADEK_DPH>(tmp, respDphRow);
              Resp.RADKY_DPH.Add(respDphRow);
            }

            try
            {
                if (prIns.PRPL_EXPFIN == "ANO")
                    dbu.PredpisExport((int)prIns.CompoundKey.PRPL_POPLATEK, prIns.CompoundKey.PRPL_PER,
                                      (int)Resp.PRPL_UCETMESIC, (int)Resp.PRPL_UCETROK,
                                      Resp.PRPL_ID);
            }
            catch (Exception exc)
            {                
            }

            return Resp;
          } //uow
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

          Util.Util.WriteLog(Resp.ERRORMESS + "\n" + exc.StackTrace); 

          return Resp;
          /*
            throw new Exception(String.Format("chyba \n {0}", exc.InnerException.Message));
            */
        }
      }

  }
}