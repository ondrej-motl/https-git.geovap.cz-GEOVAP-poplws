using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace PoplWS.WebMethod
{
  public class PredpisStorno
  {
    private Session sesna;
    STORNO_RESP Resp;

    public STORNO_RESP StornoPredpis(Session session, int EXT_APP_KOD, int PREDPIS_ID, string POZNAMKA)
     {
       sesna = session;
       Resp = new STORNO_RESP();
       Resp.STORNO_ID = -1;

       CriteriaOperator criteria;
       sesna.DropIdentityMap();

 #region test vyplneni vstupnich hodnot, kontroly
       try
       {
           P_PRPL predp = null;
         ValidateInputParam(EXT_APP_KOD, PREDPIS_ID, ref predp);

         if (predp == null)
         {
           Resp.result = Result.ERROR;
           Resp.status = Status.NOTEXISTS;
           Resp.ERRORMESS = "Stornovaný předpis neexistuje.";
           return Resp;
         }


         string[] sRecord = new string[]{" ", "", "P"};
         if (( ! sRecord.Contains(predp.PRPL_RECORD)) )
         {
           Resp.result = Result.ERROR;
           Resp.status = Status.EXISTS;
           Resp.ERRORMESS = "Předpis neumožňuje jeho stornování.";
           return Resp;
         }

         if (predp.PRPL_PR > 0)
         {  
             Resp.result = Result.ERROR;
             Resp.status = Status.EXISTS;
             Resp.ERRORMESS = "Předpis je vymáhán, nelze jej stornovat.";
             return Resp;
         }

  #endregion test  vyplneni vstupnich hodnot

  #region vzdy odparuji vse od predpisu
         XPCollection<P_PAROVANI> odparujPredpisy = new XPCollection<P_PAROVANI>(sesna,
                               CriteriaOperator.Parse("((CompoundKey1.PAR_PRPL_ID = ?) or (CompoundKey1.PAR_PLATBA_ID = ?) )",
                               PREDPIS_ID, PREDPIS_ID * -1));
         
         decimal dluh = (predp.PRPL_PREDPIS + predp.PRPL_SANKCE) - (predp.PRPL_SPAROVANO * (Math.Abs(predp.PRPL_PREDPIS + predp.PRPL_SANKCE) / (predp.PRPL_PREDPIS + predp.PRPL_SANKCE))); 
         decimal odparKc = Math.Abs(dluh);
         if (predp.PRPL_SANKCEDO == DateTime.MinValue) 
         {
             foreach (var item in odparujPredpisy)
             {
                 odparKc += item.PAR_SPARKC; 
             }
         }

         sesna.Delete(odparujPredpisy);
         sesna.Save(odparujPredpisy);  

  #endregion vzdy odparuji vse od predpisu

        DBValue dbv = DBValue.Instance(this.sesna);

        //otevrene obdobi
            P_NASTAVENI nast = sesna.GetObjectByKey<P_NASTAVENI>("OTEVR_OBDOBI");
            DateTime otevrObd = DateTime.ParseExact("01." + nast.HODNOTA, "d.M.yyyy", null);
            bool predpUzavren = predp.PRPL_OBDOBIROK > -1;
            predpUzavren = predpUzavren && ((predp.PRPL_OBDOBIROK < otevrObd.Year)
                           || (((predp.PRPL_OBDOBIROK == otevrObd.Year) && (predp.PRPL_OBDOBIMES < otevrObd.Month ))));
            bool uzaverkyNedelaji = (dbv.DBSysDate - otevrObd).TotalDays > (365 + 60);
                
        DBUtil dbu = new DBUtil(sesna);
        P_PRPL prIns = null;
        if ((predp.PRPL_EXPORTOVANO == "ANO") || predpUzavren || uzaverkyNedelaji)
        {
            using (MyUnitOfWork uow = new MyUnitOfWork(sesna.DataLayer))
            {
                P_PRPL prOld = uow.FindObject<P_PRPL>(CriteriaOperator.Parse("PRPL_ID = ?", PREDPIS_ID));

                prIns = new P_PRPL(uow);
                prIns.PRPL_ID = dbu.LIZNI_SEQ("PRPL_ID");
                prIns.CompoundKey.PRPL_POPLATEK = prOld.CompoundKey.PRPL_POPLATEK;
                prIns.CompoundKey.PRPL_PER = prOld.CompoundKey.PRPL_PER;
                prIns.CompoundKey.PRPL_ICO = prOld.CompoundKey.PRPL_ICO;
                prIns.CompoundKey.PRPL_DOPLKOD = prOld.CompoundKey.PRPL_DOPLKOD;
                prIns.PRPL_RECORD = prOld.PRPL_RECORD;
                prIns.PRPL_ROK = dbv.DBSysDate.Year;
                prIns.PRPL_PORPER = prOld.PRPL_PORPER;
                prIns.PRPL_VS = prOld.PRPL_VS;
                prIns.PRPL_EA = EXT_APP_KOD;
                prIns.PRPL_PREDPIS = prOld.PRPL_PREDPIS * -1;
                prIns.PRPL_SANKCE = prOld.PRPL_SANKCE * -1;
                prIns.PRPL_EXPFIN = prOld.PRPL_EXPFIN;
                prIns.PRPL_VYSTUP = prOld.PRPL_VYSTUP;
                prIns.PRPL_TISK = prOld.PRPL_TISK;
                prIns.PRPL_VYSTAVENO = dbv.DBSysDate;
                if (prOld.PRPL_SPLATNO > dbv.DBSysDate)
                    prIns.PRPL_SPLATNO = prOld.PRPL_SPLATNO;
                else
                    prIns.PRPL_SPLATNO = dbv.DBSysDate;
                prIns.PRPL_EXPFIN = prOld.PRPL_EXPFIN;
                prIns.PRPL_EXPORTOVANO = "NE";
                prIns.PRPL_UCETMESIC = prIns.PRPL_VYSTAVENO.Month;
                prIns.PRPL_UCETROK = prIns.PRPL_VYSTAVENO.Year;
                prIns.PRPL_EXPSIPO = prOld.PRPL_EXPSIPO;
                prIns.PRPL_POZNAMKA = POZNAMKA.SubstrLeft(110);
                prIns.PRPL_SS = prOld.PRPL_SS;
                P_C_TYPDANE typdane = uow.GetObjectByKey<P_C_TYPDANE>(prOld.PRPL_TYPDANE.TYPDANE_KOD);
                if (typdane != null)
                { prIns.PRPL_TYPDANE = typdane; }
                else
                {
                    typdane = uow.GetObjectByKey<P_C_TYPDANE>((short)5);
                    prIns.PRPL_TYPDANE = typdane;
                }
                if ((prOld.P_PRPL_DPH != null) && (prOld.P_PRPL_DPH.Count > 0))
                    prIns.PRPL_UZP = dbv.DBSysDate;

                prIns.PRPL_EA = EXT_APP_KOD;
                prIns.PRPL_EXTDOKLAD = prOld.PRPL_EXTDOKLAD;

                if ((prOld.P_PRPL_DPH != null) && (prOld.P_PRPL_DPH.Count > 0))
                {
                    P_PRPL_DPH dph;
                    foreach (var item in prOld.P_PRPL_DPH)
                    {
                        dph = new P_PRPL_DPH(uow);
                        dph.DPH_KAT = item.DPH_KAT;
                        dph.DPH_SAZBA = item.DPH_SAZBA;
                        dph.DPH_KC = item.DPH_KC * -1;
                        dph.DPH_POZNAMKA = item.DPH_POZNAMKA;
                        dph.DPH_ZAKLAD = item.DPH_ZAKLAD * -1;
                        dph.DPH_ZAOKR = item.DPH_ZAOKR * -1;
                        dph.DPH_DAN = item.DPH_DAN * -1;
                        prIns.P_PRPL_DPH.Add(dph);
                    }
                }

                uow.CommitTransaction();

                P_PAROVANI par = new P_PAROVANI(sesna);
                decimal priparujKc = 0;
                if (Math.Abs(prIns.PRPL_PREDPIS + prIns.PRPL_SANKCE) <= Math.Abs(odparKc))
                    priparujKc = Math.Abs(prIns.PRPL_PREDPIS + prIns.PRPL_SANKCE);
                if (Math.Abs(prIns.PRPL_PREDPIS + prIns.PRPL_SANKCE) > Math.Abs(odparKc))
                    priparujKc = Math.Abs(odparKc);

                if (priparujKc != 0)
                {
                    if ((prIns.PRPL_PREDPIS + prIns.PRPL_SANKCE) < 0)
                    {
                        par.CompoundKey1.PAR_PRPL_ID = prIns.PRPL_ID;
                        par.CompoundKey1.PAR_PLATBA_ID = prOld.PRPL_ID * -1;
                    }
                    if ((prIns.PRPL_PREDPIS + prIns.PRPL_SANKCE) > 0)
                    {
                        par.CompoundKey1.PAR_PRPL_ID = prOld.PRPL_ID;
                        par.CompoundKey1.PAR_PLATBA_ID = prIns.PRPL_ID * -1;
                    }

                    par.PAR_SPARKC = priparujKc;
                    par.PAR_VYTVRUCNE = "ANO";
                    par.PAR_DATE = dbv.DBSysDateTime.ToString("yyMMddHHmmss");

                    par.Save();
                }
            }  //end using
        }

        if (prIns != null)
            Resp.STORNO_ID = prIns.PRPL_ID;
        else
        {
            predp.PRPL_RECORD = "S";
            predp.PRPL_EA = EXT_APP_KOD;
            predp.LASTUPDATE = dbv.DBSysDateTime;
            predp.LOGIN = String.Format("PoplWS({0})", EXT_APP_KOD);
            predp.Save();
            Resp.STORNO_ID = predp.PRPL_ID;
        }
        Resp.result = Result.OK;
        Resp.status = Status.INSERTED;

        try  //
        {
            Util.Util.WriteLog(string.Format("pozadavek na export do ucta: EXPFIN={0}, PREDPIS_ID={1}, {2} Kč", prIns.PRPL_EXPFIN, prIns.PRPL_ID, prIns.PRPL_PREDPIS + prIns.PRPL_SANKCE));
            if (prIns.PRPL_EXPFIN == "ANO")
                dbu.PredpisExport((int)prIns.CompoundKey.PRPL_POPLATEK, prIns.CompoundKey.PRPL_PER,
                                  (int)prIns.PRPL_UCETMESIC, (int)prIns.PRPL_UCETROK,
                                  prIns.PRPL_ID);

        }
        catch (Exception exc)
        {
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
          return Resp;
          /*
            throw new Exception(String.Format("chyba \n {0}", exc.InnerException.Message));
            */
        }

    }

    /*======================================================================*/                                             
     private void ValidateInputParam(int EXT_APP_KOD, int PREDPIS_ID, ref P_PRPL prpl)
     {
       if (EXT_APP_KOD == null)
       { throw new Exception("kód externí aplikace není zadán"); }
       P_EXTAPP EA = sesna.GetObjectByKey<P_EXTAPP>(EXT_APP_KOD);

       if (EA == null)
       { throw new Exception("chybný kód externí aplikace"); }

       if (PREDPIS_ID == 0)
       { throw new Exception("údaj PREDPIS_ID není zadán"); }

       prpl = sesna.FindObject<P_PRPL>(CriteriaOperator.Parse("PRPL_ID = ?", PREDPIS_ID));
       if (prpl == null)
       {
         this.Resp.status = Status.NOTEXISTS;
         this.Resp.result = Result.ERROR;
         { throw new Exception("neexistující předpis"); }
       }

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
         throw new Exception("kontrola přístup. práv uživatele nad daty Příjmové agendy skončila chybou");
       }
       //pro predpisy je povoleno vkladani
       if (!pnp.PravoExist((int)prpl.CompoundKey.PRPL_POPLATEK, PravoNadPoplatkem.PrtabTable.PRPL, PravoNadPoplatkem.SQLPerm.INSERT))
         throw new Exception("PoplWS - nedostatečná oprávnění pro vkládání předpisů.");
       #endregion kontrola prava nad poplatkem

     }
    
  }
}