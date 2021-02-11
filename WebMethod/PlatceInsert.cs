using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace PoplWS.WebMethod
{
    public class PlatceInsert
    {
        /// <summary>
        /// metodu lze spoustet s parametrem sesna typu Session nebo UnitOfWork
        /// pokud je typu Session a metoda neni spustena uvnitr transakce, je transakce nastartovana a zkomitovana uvnitr metody
        /// pokud je typu Session a metoda je spustena uvnitr transakce, commit neni provaden
        /// pokud je typu UnitOfWork, commit neni provaden
        /// </summary>
        /// <param name="sesna"></param>
        /// <param name="EXT_APP_KOD"></param>
        /// <param name="platce"></param>
        /// <param name="rgp"></param>
        /// <returns></returns>
        public PLATCE_RESP InsertPlatce(Session sesna, int EXT_APP_KOD, PLATCE platce, P_RGP rgp = null)
        {
             return InsertPlatce(sesna, EXT_APP_KOD, platce, rgp, false);
        }

        public PLATCE_RESP InsertPlatce(Session sesna, int EXT_APP_KOD, PLATCE platce, P_RGP rgp, bool nastavitKCdleDPH)
        {

            PLATCE_RESP Resp = new PLATCE_RESP();
            CriteriaOperator criteria;
            platce.RGP_EA = EXT_APP_KOD;

            Type sesnaType = null;
            Type unitOfWorkType = null;

            bool commituj = false;
            if (sesna is DevExpress.Xpo.Session)   
            {
                commituj = (sesna == null) ? true : (!sesna.InTransaction);
                sesnaType = typeof(DevExpress.Xpo.Session);
            }
            if (sesna is DevExpress.Xpo.UnitOfWork)
            {
                commituj = false;
                unitOfWorkType = typeof(DevExpress.Xpo.UnitOfWork);
            }

           


            try
            {


                #region kontrola v pripade transakce - nejak musim vratit vysledek, k tomu se pouzije objekt rgp
                if (((sesna.InTransaction) && (rgp == null))
                    || ((sesna is DevExpress.Xpo.UnitOfWork) && (rgp == null)))
                {
                    string thisNamespace = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace;
                    string thisMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    throw new Exception(string.Format("Proměnná \"rgp\" ve volání metody {0}.{1} není inicializována.", thisNamespace, thisMethodName));
                }
                #endregion kontrola v pripade transakce

                #region kontrola ext.aplikace
                if (EXT_APP_KOD == null)
                { throw new Exception("kód externí aplikace není zadán"); }
                P_EXTAPP EA = sesna.GetObjectByKey<P_EXTAPP>(EXT_APP_KOD);
                if (EA == null)
                { throw new Exception("chybný kód externí aplikace"); }
                
                if ((int)platce.RGP_POPLATEK == 0)
                { throw new Exception("kód poplatku není zadán"); }

                KONTROLA_POPLATKU kp = new KONTROLA_POPLATKU(sesna, EXT_APP_KOD);
                if (!kp.EAexist())
                { throw new Exception("chybný kód externí aplikace"); }
                if (!kp.existPravoNadPoplatkem(platce.RGP_POPLATEK))
                { 
                    throw new Exception("Ext. aplikace nemá oprávnění k typu pohledávky.");
                 }
                #endregion kontrola ext.apliace

                #region kontrola prava nad poplatkem
                PravoNadPoplatkem pnp = null;
                try
                {
                    pnp = new PravoNadPoplatkem(sesna);
                }
                catch (Exception e)
                {
                    throw new Exception("kontrola přístup. práv uživatele nad daty Příjmové agendy skončila chybou" + "\n" + e.Message);
                }

                //pro platce je povoleno vkladani
                if (!pnp.PravoExist((int)platce.RGP_POPLATEK, PravoNadPoplatkem.PrtabTable.RGP, PravoNadPoplatkem.SQLPerm.INSERT))
                    throw new Exception("PoplWS - nedostatečná oprávnění");

                #endregion kontrola prava nad poplatkem

                #region test, zda adresa existuje v P_ADRESA_ROBRHS, dohledani ADR_ICO
                criteria = CriteriaOperator.Parse("ADR_ID = ?", platce.OSOBA_ID);
                XPCollection<P_ADRESA_ROBRHS> adr = new XPCollection<P_ADRESA_ROBRHS>(sesna, criteria);

                if (adr.Count != 1)
                {
                    Resp.result = Result.ERROR;
                    Resp.ERRORMESS = String.Format(@"osoba ID={0} neexistuje", platce.OSOBA_ID);
                    return Resp;
                }
                platce.RGP_ICO = adr[0].ADR_ICO;
                #endregion test, zda adresa existuje

                #region DOPLKOD - nastaveni pokud je null, TRIM, odstraneni nasobnych mezer
                if (platce.RGP_DOPLKOD == null)
                    platce.RGP_DOPLKOD = " ";
                else
                    platce.RGP_DOPLKOD = platce.RGP_DOPLKOD.Trim();

                if (platce.RGP_DOPLKOD == String.Empty)
                    platce.RGP_DOPLKOD = " ";
                while (platce.RGP_DOPLKOD.IndexOf("  ") >= 0)
                {
                    platce.RGP_DOPLKOD = platce.RGP_DOPLKOD.Replace("  ", " ");
                }
                #endregion DOPLKODU - nastaveni, pokud je null, TRIM, odstraneni nasobnych mezer

                #region test zda neexists v P_RGP
                DBUtil dbu = new DBUtil(sesna);
                C_EVPOPL evp = dbu.GetEvpopl(platce.RGP_POPLATEK, platce.RGP_PER);
                criteria = CriteriaOperator.Parse("CompoundKey1.RGP_POPLATEK = ?" +
                                                                 " and CompoundKey1.RGP_ICO = ?" +
                                                                 " and CompoundKey1.RGP_DOPLKOD = ?",
                                                       platce.RGP_POPLATEK, platce.RGP_ICO, platce.RGP_DOPLKOD);
                XPCollection<P_RGP> plceExist = new XPCollection<P_RGP>(sesna, criteria);

                foreach (P_RGP item in plceExist)
                {
                    if (item.CompoundKey1.RGP_PER == platce.RGP_PER)
                    {
                        Resp.result = Result.OK;
                        Resp.status = Status.EXISTS;
                        Utils.copy.CopyDlePersistentAttr<PLATCE_RESP>(plceExist[0], Resp);
                        Resp.RGP_POPLATEK = plceExist[0].CompoundKey1.RGP_POPLATEK;
                        Resp.RGP_PER = plceExist[0].CompoundKey1.RGP_PER;
                        Resp.RGP_DOPLKOD = plceExist[0].CompoundKey1.RGP_DOPLKOD;
                        Resp.OSOBA_ID = platce.OSOBA_ID;

                        Resp.RGP_ID = plceExist[0].RGP_ID;
                        Resp.VS = DejVS(sesna, evp, plceExist[0].RGP_PORVS);  //0.35
                        return Resp;
                    }
                }

                if (plceExist.Count > 0)
                {
                    platce.RGP_PORVS = plceExist[0].RGP_PORVS;
                    Resp.VS = DejVS(sesna, evp, plceExist[0].RGP_PORVS);  //0.35
                }

                #endregion test zda exists v P_RGP

                nastavitKCdleDPH = nastavitKCdleDPH && platce.RADKY_DPH.Count > 0;
                if (((int)platce.RGP_KCROK == 0) && nastavitKCdleDPH && (platce.RADKY_DPH.Count > 0))
                    platce.RGP_KCROK = 1;

                #region vypocet splatky
                short pocSplatek = 0; short periodicita = 0;
                decimal kcSplatka = 0, poslSplatka = 0;
                dbu.GetRgpSplatky(platce.RGP_KCROK, platce.RGP_PER, ref kcSplatka, ref poslSplatka, ref pocSplatek, ref periodicita);
                if (periodicita != 365)
                {
                    if ((int)platce.RGP_KCROK == 0)
                        throw new Exception(String.Format(@"částka není zadána (KC_ZAROK)"));

                    if (poslSplatka == 0)
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

                    platce.RGP_KCSPLATKA = sumDphRowsSplatka;
                    platce.RGP_POSLSPLATKA = sumDphRowsPoslSplatka;

                    diff = 0;
                    if (!nastavitKCdleDPH)  //0.19 - do 0.19 nebyla podminka
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

                    //0.9 kontrola splatky a posledni splatky  
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

                #region perioda existuje pro poplatek

                if (evp == null)
                {
                    Resp.result = Result.ERROR;

                    Resp.ERRORMESS = "nepovolená kombinace poplatek / perioda";
                    return Resp;
                }
                #endregion perioda existuje pro poplatek

                #region pouziti vkladaneho VS
                if (!string.IsNullOrEmpty(platce.VS))
                {
                    int porvs;
                    if (!korektniVS(sesna, platce, evp, out porvs))
                    {
                        Resp.result = Result.ERROR;  //0.16
                        Resp.ERRORMESS = string.Format("chybný VS {0}", platce.VS);
                        return Resp;
                    }
                    platce.RGP_PORVS = porvs;
                }
                #endregion pouziti vkladaneho VS

                {
                    if ((sesna.GetType().BaseType == sesnaType) && (!sesna.InTransaction))
                        sesna.BeginTransaction();
                    try
                    {
                        XPCollection<P_RGP> plceInserted;
                        if (rgp == null)
                            rgp = new P_RGP(sesna);
                        Utils.copy.CopyDlePersistentAttr<P_RGP>(platce, rgp);

                        rgp.CompoundKey1.RGP_POPLATEK = platce.RGP_POPLATEK;
                        rgp.CompoundKey1.RGP_PER = platce.RGP_PER;
                        rgp.CompoundKey1.RGP_DOPLKOD = platce.RGP_DOPLKOD;
                        rgp.CompoundKey1.RGP_ICO = platce.RGP_ICO;
                        rgp.Save();


                        //povkladam rozpis dane
                        foreach (DPHRozpis item in dphSplatka)
                        {
                            P_RGP_DPHSPL rd = new P_RGP_DPHSPL(sesna);
                            Utils.copy.CopyDlePersistentAttr<P_RGP_DPHSPL>(item, rd);
                            rgp.P_RGP_DPHSPL.Add(rd);
                            rd.Save();
                        }
                        foreach (DPHRozpis item in dphPoslSplatka)
                        {
                            P_RGP_DPHPSPL rd = new P_RGP_DPHPSPL(sesna);
                            DPHRozpisTmp tmp = new DPHRozpisTmp();
                            Utils.copy.CopyDlePersistentAttr<DPHRozpisTmp>(item, tmp);
                            Utils.copy.CopyDlePersistentAttr<P_RGP_DPHPSPL>(tmp, rd);
                            rgp.P_RGP_DPHPSPL.Add(rd);
                            rd.Save();
                        }

                        if (commituj && (sesna.InTransaction)) sesna.CommitTransaction();

                        Utils.copy.CopyDlePersistentAttr<PLATCE_RESP>(rgp, Resp);

                        Resp.result = Result.OK;
                        Resp.status = Status.INSERTED;
                        Resp.RGP_POPLATEK = rgp.CompoundKey1.RGP_POPLATEK;
                        Resp.RGP_PER = rgp.CompoundKey1.RGP_PER;
                        Resp.RGP_DOPLKOD = rgp.CompoundKey1.RGP_DOPLKOD;
                        Resp.OSOBA_ID = platce.OSOBA_ID;
                        Resp.RGP_ID = rgp.RGP_ID;
                        if (Resp.VS == null)
                           Resp.VS = DejVS(sesna, evp, plceExist[0].RGP_PORVS);  //0.35

                        switch (rgp.RGP_EXPUCTO)
                        {
                            case "ANO":
                                Resp.EXPORTOVAT_DO_FINANCI = SouhlasAnoNe.ANO;
                                break;
                            default:
                                Resp.EXPORTOVAT_DO_FINANCI = SouhlasAnoNe.NE;
                                break;
                        }

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

                }  //using (MyUnitOfWork

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

                Util.Util.WriteLog(Resp.ERRORMESS + exc.StackTrace); 

                return Resp;
                /*
                  throw new Exception(String.Format("chyba \n {0}", exc.InnerException.Message));
                  */
            }


        }

        /*======================================================================*/
        private string DejVS(Session sesna, C_EVPOPL evp, int porvs)
        {
            decimal popl = evp.CompoundKey1.EVPOPL_KOD.NAZPOPL_POPLATEK;
            string formatVS = evp.CompoundKey1.EVPOPL_KOD.NAZPOPL_FORMATVS.ToLower();

            if (formatVS.ToLower().Contains('r'))
               return null;

            DBUtil dbu = new DBUtil(sesna);
            return dbu.GetVS(popl, porvs);
        }


    

        /*======================================================================*/
        private bool korektniVS(Session sesna, PLATCE platce, C_EVPOPL evp, out int porvs)
        {
            //navrat = false i tehdy, pokud je dany VS jiz pouzivan
            decimal popl = evp.CompoundKey1.EVPOPL_KOD.NAZPOPL_POPLATEK;
            string formatVS = evp.CompoundKey1.EVPOPL_KOD.NAZPOPL_FORMATVS.ToLower();
            string VS = platce.VS;
            int _popl = 0;

            string c = string.Empty;
            string k = string.Empty;
            string _vs = string.Empty;
            string spor = string.Empty;

            porvs = 0;
            if (VS.Length != formatVS.Length)
                return false;

            for (int i = 0; i < formatVS.Length; i++)
            {
                switch (formatVS[i])
                {
                    case 'c':
                        c += VS[i];
                        break;
                    case 'k':
                        k += VS[i];
                        break;
                }

            }

            int.TryParse(k, out _popl);
            int.TryParse(c, out porvs);
            if ((_popl == 0) || (porvs == 0))
                return false;

            if (_popl != (int)popl)
                return false;

            DBUtil dbu = new DBUtil(sesna);
            _vs = dbu.GetVS(_popl, porvs);

            if (_vs != VS)
                return false;


            int por = porvs;
            XPQuery<P_RGP> selRgp = sesna.Query<P_RGP>();

            var list = (from a in selRgp
                        where a.CompoundKey1.RGP_POPLATEK == popl
                           && a.RGP_PORVS == por
                        select a);
            foreach (var item in list)
            {
                if ((item.CompoundKey1.RGP_ICO != platce.RGP_ICO) ||
                    (item.CompoundKey1.RGP_PER == platce.RGP_PER) ||
                    (item.CompoundKey1.RGP_DOPLKOD != platce.RGP_DOPLKOD)
                   )
                    return false;
            }

            return true;
        }


    }

}