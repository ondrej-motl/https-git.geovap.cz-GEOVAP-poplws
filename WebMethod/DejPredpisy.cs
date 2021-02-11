using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace PoplWS.WebMethod
{
    public class SeznamPredpisu 
    {
        /// <summary>
        /// vraci seznam kladných nenulových předpisů dle poplatku a osoby_id
        /// částka predpisu je částka snížená o přípárování zápornou částí (CASTKA = PREDPIS + SANKCE - SPAROVANO_MINUSEM
        ///  => seznam neobsahuje předpisy, které byly celé uhrazeny jen zápornymi předpisy
        /// </summary>
        /// <param name="session"></param>
        /// <param name="EXT_APP_KOD"></param>
        /// <param name="predpis"></param>
        /// <returns></returns>
        internal PREDPISY_RESP DejPredpisy(Session session, int EXT_APP_KOD, int osobaId, int poplKod)
        {
            Session sesna = session;
            PREDPISY_RESP resp = new PREDPISY_RESP();
            resp.status = Status.NOTEXISTS;

            CriteriaOperator criteria;
            sesna.DropIdentityMap();

            #region kontrola vstupnich udaju
            try
            {

                if (EXT_APP_KOD == null)
                { throw new Exception("kód externí aplikace není zadán"); }

                KONTROLA_POPLATKU kp = new KONTROLA_POPLATKU(sesna, EXT_APP_KOD);
                if (!kp.EAexist())
                { throw new Exception("Chybný kód externí aplikace."); }
                if (!kp.existPravoNadPoplatkem(poplKod))
                { throw new Exception("K pohledávce neexistuje oprávnění."); }

                if (osobaId <= 0)
                    throw new Exception("Osoba musí být zadána.");

                #region kontrola prava na cteni predpisu
                PravoNadPoplatkem pnp = null;
                try
                {
                    pnp = new PravoNadPoplatkem(sesna);
                }
                catch (Exception)
                {
                    throw new Exception("kontrola přístp. práv uživatele nad daty Příjmové agendy skončila chybou");
                }

                if (!pnp.PravoExist(poplKod, PravoNadPoplatkem.PrtabTable.PRPL, PravoNadPoplatkem.SQLPerm.SELECT))
                    throw new Exception("PoplWS - nedostatečná oprávnění pro čtení předpisů.");

                #endregion kontrola prava nad predpisy

            }
            catch (Exception exc)
            {
                resp.result = Result.ERROR;

                if (exc.InnerException == null)
                {
                    resp.ERRORMESS = exc.Message;
                }
                else
                {
                    resp.ERRORMESS = exc.InnerException.Message;
                }
                return resp;
            }
            #endregion kontrola vstupnich udaju

            try
            {
            criteria = CriteriaOperator.Parse("ADR_ID = ?", osobaId);
            XPCollection<P_ADRESA_ROBRHS> osoba = new XPCollection<P_ADRESA_ROBRHS>(sesna, criteria);
            string adrIco;
            if (osoba.Count == 1)
            {
                adrIco = osoba.First().ADR_ICO;
            }
            else
            {
                resp.result = Result.OK;
                resp.status = Status.NOTEXISTS;
                resp.ERRORMESS = string.Format("Osoba {0} neexistuje", osobaId);
                return resp;
            }

            int roku;
            if ( ! int.TryParse(System.Configuration.ConfigurationManager.AppSettings["DejPredpisy_HistRoku"], out roku))
                roku = 1;
            DateTime predpisyOdRoku = new DateTime(DateTime.Now.AddYears(-1 * roku).Year, 1, 1);
            criteria = CriteriaOperator.Parse("CompoundKey.PRPL_ICO = ? and CompoundKey.PRPL_POPLATEK = ? " + 
                                       " and PRPL_RECORD in (' ', 'P') and  " +
                                       " (PRPL_VYSTAVENO > ? or PRPL_PREDPIS + PRPL_SANKCE - PRPL_SPAROVANO > 0)", 
                                       adrIco, poplKod, predpisyOdRoku);
            XPCollection<P_PRPL> prpls = new XPCollection<P_PRPL>(sesna, criteria);
            foreach (var item in prpls)
            {

                if (item.USER_PREDPIS - item.PRPL_SPAROVANO_MINUSEM <= 0)
                    continue;

                if (item.PRPL_PREDPIS < 0)
                    continue;

                C_NAZPOPL nazpopl = sesna.GetObjectByKey<C_NAZPOPL>((decimal)poplKod);
                if (nazpopl != null)
                {
                    resp.BANKOVNI_SPOJENI.SMER_KOD_BANKY = nazpopl.NAZPOPL_PRIJBANKA;
                    resp.BANKOVNI_SPOJENI.CISLO_UCTU = nazpopl.NAZPOPL_PRIJUCET;
                    resp.BANKOVNI_SPOJENI.IBAN = nazpopl.NAZPOPL_IBAN;
                }
                
                //zkopiruji 
                PREDPISBaseUhr predpis = new PREDPISBaseUhr();
                Utils.copy.CopyDlePersistentAttr<PREDPISBaseUhr>(item, predpis);
                predpis.PRPL_TYPDANE = item.PRPL_TYPDANE.TYPDANE_KOD;
                predpis.TYP_NAZEV = item.PRPL_TYPDANE.TYPDANE_NAZEV;
                predpis.POPLATEK_KOD = (int)nazpopl.NAZPOPL_POPLATEK;
                predpis.POPLATEK_NAZEV = nazpopl.C_NAZPOPL_NAZEV;
                predpis.PRPL_VS = item.PRPL_VS;

                decimal uhrazenoKc = 0, prKc = 0;
                Util.Util.DejKCReduk(ref prKc, ref uhrazenoKc, item.USER_PREDPIS, item.PRPL_SPAROVANO, item.PRPL_SPAROVANO_MINUSEM);
                predpis.PRPL_PREDPIS = prKc;
                predpis.KC_UHRAZENO = uhrazenoKc;

                foreach (var itemDPH in item.P_PRPL_DPH)
                {
                    RADEK_DPH dph = new RADEK_DPH();
                    DPHRozpisPredp tmp = new DPHRozpisPredp();
                    Utils.copy.CopyDlePersistentAttr<DPHRozpisPredp>(itemDPH, tmp);
                    Utils.copy.CopyDlePersistentAttr<RADEK_DPH>(tmp, dph);

                    predpis.RADKY_DPH.Add(dph);
                }
                resp.PREDPISY.Add(predpis);

            }


            resp.result = Result.OK;
            if (resp.PREDPISY.Count > 0)
                resp.status = Status.EXISTS;

            return resp;

            }
            catch (Exception exc)
            {
                resp.result = Result.ERROR;
                resp.status = Status.ERROR;

                resp.ERRORMESS = "Chyba při získávání seznamu předpisů.";
                Util.Util.WriteLog(exc.Message + "\n\n" + exc.InnerException);
                return resp;
            }

        }
    }
}