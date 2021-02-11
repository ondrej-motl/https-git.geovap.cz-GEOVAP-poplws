using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace PoplWS.WebMethod

{
    public class PlatceUpdate
    {
        /// <summary>
        /// lze použít pro update rozpisu DPH, výše splátky, poslední splátky, celkové částky
        /// jine hodnoty touto metodou nejsou měněny
        /// </summary>
        /// <param name="sesna"></param>
        /// <param name="EXT_APP_KOD"></param>
        /// <param name="platce"></param>
        /// <returns></returns>
        public PLATCE_RESP UpdatePlatce(Session sesna, int EXT_APP_KOD, PLATCE platce)
        {
            return UpdatePlatce(sesna, EXT_APP_KOD, platce, false);
        }

        /// <summary>
        /// updatuje rozpis DPH, výši splátky, celkovou částku
        /// </summary>
        public PLATCE_RESP UpdatePlatce(Session sesna, int EXT_APP_KOD, PLATCE platce, bool nastavitKCdleDPH)
        {
            PLATCE_RESP Resp = new PLATCE_RESP();
            CriteriaOperator criteria;
            platce.RGP_EA = EXT_APP_KOD;

            Type sesnaType = null;
            Type unitOfWorkType = null;

            if (platce.RGP_ID <= 0)
                throw new Exception("ID Plátce není zadáno.");

            try
            {
                bool commituj = false;
                if (sesna is DevExpress.Xpo.Session)   //UnitOfWork dedi z Session a proto je tez typu Session, test (sesna is DevExpress.Xpo.Session) je tak vzdy true
                {
                    commituj = (sesna == null) ? true : (!sesna.InTransaction);
                    sesnaType = typeof(DevExpress.Xpo.Session);
                }
                if (sesna is DevExpress.Xpo.UnitOfWork)
                {
                    commituj = false;
                    unitOfWorkType = typeof(DevExpress.Xpo.UnitOfWork);
                }

                #region test zda exists v P_RGP
                if (platce.RGP_ID > 0)
                    criteria = CriteriaOperator.Parse(String.Format(@"RGP_ID = {0}", platce.RGP_ID));
                else
                    criteria = CriteriaOperator.Parse("CompoundKey1.RGP_POPLATEK = ?" +
                                                                     " and CompoundKey1.RGP_PER = ?" +
                                                                     " and CompoundKey1.RGP_ICO = ?" +
                                                                     " and CompoundKey1.RGP_DOPLKOD = ?",
                                                           platce.RGP_POPLATEK, platce.RGP_PER, platce.RGP_ICO, platce.RGP_DOPLKOD);
                XPCollection<P_RGP> plceExist = new XPCollection<P_RGP>(sesna, criteria);
                if (plceExist.Count == 0)
                {
                    Resp.result = Result.ERROR;
                    Resp.status = Status.NOTEXISTS;

                    //rgp = null;
                    return Resp;
                }
                #endregion test zda exists v P_RGP

                //odmazu stavajici rozpisDPH
                for (int i = plceExist[0].P_RGP_DPHSPL.Count - 1; i >= 0; i--)
                {  plceExist[0].P_RGP_DPHSPL[i].Delete(); }
                for (int i = plceExist[0].P_RGP_DPHPSPL.Count - 1; i >= 0; i--)
                { plceExist[0].P_RGP_DPHPSPL[i].Delete(); }

                P_RGP rgpOld = plceExist[0];
                PLATCE platceOld = new PLATCE();

                Utils.copy.CopyDlePersistentAttr<PLATCE>(platce, platceOld);  
                Utils.copy.CopyDlePersistentAttr<PLATCE>(rgpOld, platce);

                platce.RGP_POPLATEK = rgpOld.CompoundKey1.RGP_POPLATEK;
                platce.RGP_PER = rgpOld.CompoundKey1.RGP_PER;
                platce.RGP_ICO = rgpOld.CompoundKey1.RGP_ICO;
                platce.RGP_DOPLKOD = rgpOld.CompoundKey1.RGP_DOPLKOD;
                platce.EXPORTOVAT_DO_FINANCI = (SouhlasAnoNe)Enum. Parse(typeof(SouhlasAnoNe), rgpOld.RGP_EXPUCTO);
                platce.RGP_FROMDATE = platceOld.RGP_FROMDATE;
                platce.RGP_TODATE = platceOld.RGP_TODATE;
                rgpOld.RGP_EA = EXT_APP_KOD;  //0.39

                nastavitKCdleDPH = nastavitKCdleDPH && platce.RADKY_DPH.Count > 0;
                if (( (int)platce.RGP_KCROK == 0) && nastavitKCdleDPH && (platce.RADKY_DPH.Count > 0))
                    platce.RGP_KCROK = 1;

                #region vypocet splatky
                if (platceOld.RGP_KCROK == 0)
                    platce.RGP_KCROK = rgpOld.RGP_KCROK;
                else
                    platce.RGP_KCROK = platceOld.RGP_KCROK;

                //0.12 umoznuji updatovat na nulu
                //      if ((platceOld.RGP_KCROK == 0)) { throw new Exception("částka není zadána"); }

                short pocSplatek = 0; short periodicita = 0;
                decimal kcSplatka = 0, poslSplatka = 0;
                DBUtil dbu = new DBUtil(sesna);
                dbu.GetRgpSplatky(platce.RGP_KCROK, platce.RGP_PER, ref kcSplatka, ref poslSplatka, ref pocSplatek, ref periodicita);
                if (poslSplatka == 0)
                {
                   // throw new Exception(String.Format(@"Nelze určit výši splátky, perioda ""{0}"" neexistuje.", platce.RGP_PER));
                    throw new Exception(String.Format(@"Nelze určit výši splátky pro roční výši předpisu {0} Kč", platce.RGP_KCROK));
                }
                platce.RGP_KCSPLATKA = kcSplatka;
                platce.RGP_POSLSPLATKA = poslSplatka;

                #endregion vypocet splatky

                #region vypocet splatek DPH, zaokrouhleni DPH
                List<DPHRozpis> dphSplatka = new List<DPHRozpis>();
                List<DPHRozpis> dphPoslSplatka = new List<DPHRozpis>();
                if (platce.RADKY_DPH.Count > 0)
                {
                    //rozpocitam radky DPH na splatky a posledni splatku
                    DPHRozpis dpr;
                    foreach (RADEK_DPH item in platce.RADKY_DPH)
                    {
                        if (item.ZAKLAD == 0)
                        { continue; }
                        dpr = new DPHRozpis();
                        Utils.copy.CopyDlePersistentAttr(item, dpr);
                        dpr.ZAKLAD = item.ZAKLAD / pocSplatek;
                        dpr.DAN = item.ZAKLAD * ((item.SAZBA ?? 0) / 100);
                        dpr.KC = dpr.ZAKLAD + dpr.DAN;
                        dphSplatka.Add(dpr);
                        
                        dpr = new DPHRozpis();
                        Utils.copy.CopyDlePersistentAttr(item, dpr);
                        decimal splatka = item.ZAKLAD / pocSplatek;
                        dpr.ZAKLAD = item.ZAKLAD - (splatka * (pocSplatek - 1));
                        dpr.DAN = item.ZAKLAD * ((item.SAZBA ?? 0) / 100);
                        dpr.KC = dpr.ZAKLAD + dpr.DAN;
                        dphPoslSplatka.Add(dpr);
                    }
                    //pripadne zaokrouhli radky dane - dolu
                    DPHZaokrouhleni dphZaokr = new DPHZaokrouhleni();
                    dphZaokr.DPHZaokrouhli(dphSplatka, platce.RGP_POPLATEK, sesna);
                    dphZaokr.DPHZaokrouhli(dphPoslSplatka, platce.RGP_POPLATEK, sesna);

                    //sectu vsechny radky DPH a vytvorim do posledni splatky radek dorovnani
                    decimal sumDphRowsSplatka = 0, sumDphRowsPoslSplatka = 0, diff = 0;
                    foreach (DPHRozpis item in dphSplatka)
                    { sumDphRowsSplatka += item.KC + item.ZAOKROUHLENI; }
                    foreach (DPHRozpis item in dphPoslSplatka)
                    { sumDphRowsPoslSplatka += item.KC + item.ZAOKROUHLENI; }

                    //0.19
                    sumDphRowsSplatka = Utils.Mathm.ZaokrouhliDouble(sumDphRowsSplatka, 2);
                    sumDphRowsPoslSplatka = Utils.Mathm.ZaokrouhliDouble(sumDphRowsPoslSplatka, 2);

                    //0.20 vlivem zaokrouhleni napr. u 12 splatek se muze lisit
                    platce.RGP_KCSPLATKA = sumDphRowsSplatka;
                    platce.RGP_POSLSPLATKA = sumDphRowsPoslSplatka;

                    //pokud je treba, vytvorim dorovnavaci nedanovy radek, aby celkova castka rozpisu DPH 
                    //souhlasila s RGP_KCROK 
                    if (!nastavitKCdleDPH)  
                      diff = platce.RGP_KCROK - (sumDphRowsSplatka * (pocSplatek - 1)) - sumDphRowsPoslSplatka;
                    diff = Utils.Mathm.ZaokrouhliDouble(diff, 2);
                    if (diff != 0)
                    {
                        dpr = new DPHRozpis();
                        dpr.KAT = 'R';
                        dpr.ZAKLAD = diff;
                        dpr.KC = diff;
                        dpr.SAZBA = null;
                        dpr.POZNAMKA = "dorovnání";
                        dphPoslSplatka.Add(dpr);
                        sumDphRowsPoslSplatka += diff;
                        platce.RGP_POSLSPLATKA = sumDphRowsPoslSplatka; //0.20
                    }

                    #region kontrola splatky, posledni splatky
                    if (nastavitKCdleDPH)
                    {
                        platce.RGP_KCSPLATKA = sumDphRowsSplatka;
                        platce.RGP_POSLSPLATKA = sumDphRowsPoslSplatka;
                        platce.RGP_KCROK = (sumDphRowsSplatka * (pocSplatek - 1)) + sumDphRowsPoslSplatka;
                    }
                    else
                    {
                        if (platce.RGP_KCROK != (sumDphRowsSplatka * (pocSplatek - 1)) + sumDphRowsPoslSplatka)
                        {
                            throw new Exception(string.Format("Nesouhlasí celková výše Kč ({0}) s rozpisem DPH ({1}).",
                                                             platce.RGP_KCROK, (sumDphRowsSplatka * (pocSplatek - 1)) + sumDphRowsPoslSplatka));
                        }

                        if (sumDphRowsSplatka != platce.RGP_KCSPLATKA)
                        {
                            throw new Exception(string.Format("Nesouhlasí výše splátky ({0}) s rozpisem DPH splátky ({1}).",
                                                             platce.RGP_KCSPLATKA, sumDphRowsSplatka));
                        }

                        if (sumDphRowsPoslSplatka != platce.RGP_POSLSPLATKA)
                        {
                            throw new Exception(string.Format("Nesouhlasí výše poslední splátky ({0}) s rozpisem DPH poslední splátky ({1}).",
                                                             platce.RGP_POSLSPLATKA, sumDphRowsPoslSplatka));
                        }
                    }
                    #endregion kontrola splatky, posledni splatky

                }
                #endregion vypocet splatek DPH, zaokrouhledni DPH

                //zkopiruji 
                Utils.copy.CopyDlePersistentAttr<P_RGP>(platce, rgpOld);

                rgpOld.Save();

                if ((sesna.GetType().BaseType == sesnaType) && (!sesna.InTransaction))
                    sesna.BeginTransaction();

                try
                {

                    //povkladam rozpis dane
                    #region vlozeni rozpisu DPH
                    foreach (DPHRozpis item in dphSplatka)
                    {
                        P_RGP_DPHSPL rd = new P_RGP_DPHSPL(sesna);
                        Utils.copy.CopyDlePersistentAttr<P_RGP_DPHSPL>(item, rd);
                        rgpOld.P_RGP_DPHSPL.Add(rd);
                        rd.Save();
                    }
                    foreach (DPHRozpis item in dphPoslSplatka)
                    {
                        P_RGP_DPHPSPL rd = new P_RGP_DPHPSPL(sesna);
                        DPHRozpisTmp tmp = new DPHRozpisTmp();
                        Utils.copy.CopyDlePersistentAttr<DPHRozpisTmp>(item, tmp);
                        Utils.copy.CopyDlePersistentAttr<P_RGP_DPHPSPL>(tmp, rd);
                        rgpOld.P_RGP_DPHPSPL.Add(rd);
                        rd.Save();
                    }
                    #endregion vlozeni rozpisu DPH

                    if (commituj && (sesna.InTransaction)) sesna.CommitTransaction();

                    Utils.copy.CopyDlePersistentAttr<PLATCE_RESP>(rgpOld, Resp);

                    Resp.result = Result.OK;
                    Resp.status = Status.EXISTS;
                    Resp.RGP_POPLATEK = rgpOld.CompoundKey1.RGP_POPLATEK;
                    Resp.RGP_PER = rgpOld.CompoundKey1.RGP_PER;
                    Resp.RGP_DOPLKOD = rgpOld.CompoundKey1.RGP_DOPLKOD;
                    Resp.OSOBA_ID = 0;
                    Resp.RGP_ID = rgpOld.RGP_ID;
                    Resp.EXPORTOVAT_DO_FINANCI = (SouhlasAnoNe)Enum.Parse(typeof(SouhlasAnoNe), rgpOld.RGP_EXPUCTO);  //convert string to Enum;

                    return Resp;
                }

                catch (Exception exc)
                {
                    if (sesna.InTransaction)
                        sesna.RollbackTransaction();

                    Resp.result = Result.ERROR;

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
                return Resp;
                /*
                  throw new Exception(String.Format("chyba \n {0}", exc.InnerException.Message));
                  */
            }

        }
    }
}