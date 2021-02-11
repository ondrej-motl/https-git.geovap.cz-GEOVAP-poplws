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
  public class SeznamPsu
  {
    public DEJPSY_RESP DejPsiSeznam(Session sesna, int EXT_APP_KOD, int? PES_ZNAMKA, string PES_JMENO,
                               int? PES_PLEMENO_KOD, string POPLATNIK_PRIJMENI, string POPLATNIK_FIRMA,
                               string POPLATNIK_ULICE, int? POPLATNIK_CP)
      {
          GET_PES_PARAMS inParams = new GET_PES_PARAMS { PES_ZNAMKA = PES_ZNAMKA,
                                                         PES_JMENO = PES_JMENO,
                                                         PES_PLEMENO_KOD = PES_PLEMENO_KOD,
                                                         POPLATNIK_PRIJMENI = POPLATNIK_PRIJMENI,
                                                         POPLATNIK_FIRMA = POPLATNIK_FIRMA,
                                                         POPLATNIK_ULICE = POPLATNIK_ULICE,
                                                         POPLATNIK_CP = POPLATNIK_CP};
          return DejPsiSeznam(sesna, EXT_APP_KOD, inParams);
      }

    public DEJPSY_RESP DejPsiSeznam(Session sesna, int EXT_APP_KOD, GET_PES_PARAMS inParams)
    {
      DEJPSY_RESP Resp = new DEJPSY_RESP();

      sesna.DropIdentityMap();

      try
      {
        #region kontrola vyberovych parametru
          if (((inParams.PES_ZNAMKA == null)
                 && string.IsNullOrWhiteSpace(inParams.PES_JMENO)
                 && (inParams.PES_PLEMENO_KOD == null)
                 && string.IsNullOrWhiteSpace(inParams.POPLATNIK_PRIJMENI)
                 && string.IsNullOrWhiteSpace(inParams.POPLATNIK_FIRMA)
                 && string.IsNullOrWhiteSpace(inParams.POPLATNIK_ULICE)
                 && (inParams.POPLATNIK_CP == null)
                 && string.IsNullOrWhiteSpace(inParams.PES_TETOVANI)
                 && string.IsNullOrWhiteSpace(inParams.PES_CIP)
                 && string.IsNullOrWhiteSpace(inParams.PES_ZNAMKA_CIP)
                )
           )
          {
            { throw new Exception("výběrová podmínka není zadána"); }
          }


        #endregion kontrola vyberovych parametru

        #region kontrola prava na poplatek
        if (EXT_APP_KOD == null)
        { throw new Exception("kód externí aplikace není zadán"); }
        P_EXTAPP EA = sesna.GetObjectByKey<P_EXTAPP>(EXT_APP_KOD);
        if (EA == null)
        { throw new Exception("chybný kód externí aplikace"); }


        int popl = Convert.ToInt16(sesna.ExecuteScalar(string.Format("SELECT KONF_POPLATEK FROM PES_KONFIG")));

        KONTROLA_POPLATKU kp = new KONTROLA_POPLATKU(sesna, EXT_APP_KOD);
        if (!kp.EAexist())
        { throw new Exception("chybný kód externí aplikace"); }
        if (!kp.existPravoNadPoplatkem(popl))
        { 
            throw new Exception("Ext. aplikace nemá oprávnění k typu pohledávky.");

        }
        #endregion kontrola prava na poplatek

        #region kontrola prava nad predpisy, platce,
        PravoNadPoplatkem pnp = null;
        try
        {
          pnp = new PravoNadPoplatkem(sesna);
        }
        catch (Exception)
        {
          throw new Exception ("kontrola přístp. práv uživatele nad daty Příjmové agendy skončila chybou");
        }

        
        if  ( ! pnp.PravoExist(popl, PravoNadPoplatkem.PrtabTable.PRPL, PravoNadPoplatkem.SQLPerm.SELECT))
            throw new Exception("PoplWS - nedostatečná oprávnění pro čtení předpisů.");
        if (!pnp.PravoExist(popl, PravoNadPoplatkem.PrtabTable.RGP, PravoNadPoplatkem.SQLPerm.SELECT))
            throw new Exception("PoplWS - nedostatečná oprávnění pro čtení plátců.");

        #endregion kontrola prava nad predpisy

        #region SQL select
        DBVal dbv = new DBVal(sesna);
         IDBValFactory idbv = dbv.CreateDBV();

         DateTime rDate = DateTime.Now;

        StringBuilder cmd = new StringBuilder();
        cmd.Append("select ");
        cmd.Append("  (select sum(PRPL_PREDPIS + PRPL_SANKCE - PRPL_SPAROVANO) from P_PRPL b ");
        cmd.Append("          where b.PRPL_ICO = a.RGP_ICO " );
        cmd.Append("            and b.PRPL_POPLATEK = a.RGP_POPLATEK " );
        cmd.Append("            and b.PRPL_PREDPIS + b.PRPL_SANKCE > 0  " );
        cmd.Append("            and b.PRPL_SPLATNO < " + idbv.getParamText("DNESEK") ); 
        cmd.Append("            and b.PRPL_RECORD in (' ','P')) DLUH, " );
        cmd.Append(" PES_MAJITEL_ICO, PES_ID, PES_CIS_PRIZNANI, PES_JMENO, PES_POPIS,");
        cmd.Append(" PES_ZNAMKA, PES_CIP, PES_NAROZEN, PES_DRZEN_OD, PES_POPL_OD, PES_POPL_DO,");
        cmd.Append(" PES_SLEVA_OD, PES_SLEVA_DO, PES_ZMENA_OD, PES_OCKOVAN, PES_UCEL, PES_SAZPOPL_SAZBA,");
        cmd.Append(" PES_POHLAVI, PES_OSVOBOZEN, PES_DRZEN_DO, PES_MAJITEL_PERIODA, PES_POZNAMKA, PES_TET,");
        cmd.Append(" PES_CIP2, PES_ZNAMKA_VYD, PES_BARVA, PES_SLEVAC_OD, PES_SLEVAC_DO, PES_SLEVAC_KC,");
        cmd.Append(" PES_UQ_ID,");
        cmd.Append(" C_PES_PLEMENO_NAZEV, C_PES_BARVA_NAZEV, ");
        cmd.Append(" ADR_ICO, ADR_TYP, ADR_NAZEV1, ADR_NAZEV2, ADR_ICZUJ, ADR_ICZUJ_NAZEV,");
        cmd.Append(" ADR_KODCOB, ADR_KODCOB_NAZEV, ADR_KODUL, ADR_KODUL_NAZEV, ADR_CIS_DOMU, ADR_CIS_OR,");
        cmd.Append(" ADR_ZNAK_CO, ADR_PSC, ADR_KNAZEV1, ADR_KNAZEV2, ADR_KICZUJ, ADR_KICZUJ_NAZEV,");
        cmd.Append(" ADR_KKODCOB, ADR_KKODCOB_NAZEV, ADR_KKODUL, ADR_KKODUL_NAZEV, ADR_KCIS_DOMU, ADR_KCIS_OR,");
        cmd.Append(" ADR_KZNAK_CO, ADR_KPSC, ADR_SIPO, ADR_ZMENAROB,");
        cmd.Append(" ADR_JMENO, ADR_KJMENO, ADR_PRIJMENI, ADR_KPRIJMENI, ADR_TITUL_PRED, ADR_KTITUL_PRED,");
        cmd.Append(" ADR_TITUL_ZA, ADR_KTITUL_ZA, ADR_ADRPOPL, ADR_ICO_OPROS, ADR_OPROS_ROZH, ADR_DATNAR,");
        cmd.Append(" ADR_ZMENAROB_ZPRAC, ADR_POSTA, ADR_kPOSTA, ADR_TELEFON, ADR_EMAIL, ADR_PLATCEDPH,");
        cmd.Append(" ADR_DIC, ADR_ISIR, ADR_IDDS, ADR_PAS, ADR_STAT, ADR_ID,");
        cmd.Append(" ADR_DADRESA, ADR_EA");
        cmd.Append("  from P_RGP a, P_ADRESA_ROBRHS, PES_MAJITEL, PES_PES ");
        cmd.Append("     LEFT OUTER JOIN C_PES_PLEMENO on (PES_PLEMENO = C_PES_PLEMENO_KOD) ");
        cmd.Append("     LEFT OUTER JOIN C_PES_BARVA on (PES_BARVA = C_PES_BARVA_KOD) ");
        cmd.Append(" where RGP_ICO = PES_MAJITEL_ICO ");
        cmd.Append("   and PES_MAJITEL_ICO = ADR_ICO  ");
        cmd.Append("   and MAJITEL_AKTIVNI = 'A' ");
        cmd.Append("   and PES_MAJITEL_ICO = MAJITEL_ICO ");
        cmd.Append("   and PES_MAJITEL_PERIODA = MAJITEL_PERIODA ");


          cmd.Append("   and PES_POPL_OD <= " + idbv.getParamText("DNESEK"));
          cmd.Append("   and (PES_POPL_DO is null or PES_POPL_DO >= " + idbv.getParamText("DNESEK") + ")");
          cmd.Append("   and RGP_POPLATEK = " + idbv.getParamText("POPL") );
#endregion SQL select

          List<string> pNames = new List<string>();
          pNames.Add("DNESEK");
          pNames.Add("POPL");

          List<object> pValues = new List<object>();
          pValues.Add(rDate.Date);  //DNESEK
          pValues.Add(popl);        //POPL

 #region odstraneni wildcarts [%, _]
          if (!string.IsNullOrWhiteSpace(inParams.PES_JMENO))
          {
              inParams.PES_JMENO = inParams.PES_JMENO.Replace("%", string.Empty);
              inParams.PES_JMENO = inParams.PES_JMENO.Replace("_", string.Empty);
          }
          if (!string.IsNullOrWhiteSpace(inParams.POPLATNIK_PRIJMENI))
          {
              inParams.POPLATNIK_PRIJMENI = inParams.POPLATNIK_PRIJMENI.Replace("%", string.Empty);
              inParams.POPLATNIK_PRIJMENI = inParams.POPLATNIK_PRIJMENI.Replace("_", string.Empty);
          }
          if (!string.IsNullOrWhiteSpace(inParams.POPLATNIK_FIRMA))
          {
              inParams.POPLATNIK_FIRMA = inParams.POPLATNIK_FIRMA.Replace("%", string.Empty);
              inParams.POPLATNIK_FIRMA = inParams.POPLATNIK_FIRMA.Replace("_", string.Empty);
          }
          if (!string.IsNullOrWhiteSpace(inParams.POPLATNIK_ULICE))
          {
              inParams.POPLATNIK_ULICE = inParams.POPLATNIK_ULICE.Replace("%", string.Empty);
              inParams.POPLATNIK_ULICE = inParams.POPLATNIK_ULICE.Replace("_", string.Empty);
          }
 #endregion odstraneni wildcarts

#region where podminky
          if (inParams.PES_ZNAMKA != null)
          {
              cmd.Append(" and PES_ZNAMKA = " + inParams.PES_ZNAMKA.ToString());
          }
          if (!string.IsNullOrWhiteSpace(inParams.PES_JMENO))
          {
              if (inParams.PES_JMENO.Any(c => char.IsLower(c)))  
            {
                if (inParams.PES_JMENO.Length >= 5)
                    cmd.Append(" and PES_JMENO like '" + inParams.PES_JMENO + "%'");
              else
                    cmd.Append(" and PES_JMENO = '" + inParams.PES_JMENO + "'");
            }
            else
            {
                if (inParams.PES_JMENO.Length >= 5)
                    cmd.Append(" and UPPER(PES_JMENO) like '" + inParams.PES_JMENO + "%'");
              else
                    cmd.Append(" and UPPER(PES_JMENO) = '" + inParams.PES_JMENO + "'");
            }
          }

          if (inParams.PES_PLEMENO_KOD != null)
          {
              cmd.Append(" and PES_PLEMENO = " + inParams.PES_PLEMENO_KOD.ToString());
          }

          if (!string.IsNullOrWhiteSpace(inParams.POPLATNIK_PRIJMENI))
          {
              if (inParams.POPLATNIK_PRIJMENI.Any(c => char.IsLower(c)))  
            {
                if (inParams.POPLATNIK_PRIJMENI.Length >= 5)
                    cmd.Append(" and ADR_PRIJMENI like '" + inParams.POPLATNIK_PRIJMENI + "%'");
              else
                    cmd.Append(" and ADR_PRIJMENI = '" + inParams.POPLATNIK_PRIJMENI + "'");
            }
            else
            {
                if (inParams.POPLATNIK_PRIJMENI.Length >= 5)
                  cmd.Append(" and UPPER(ADR_PRIJMENI) like '" + inParams.POPLATNIK_PRIJMENI + "%'");
              else
                    cmd.Append(" and UPPER(ADR_PRIJMENI) = '" + inParams.POPLATNIK_PRIJMENI + "'");
            }
          }
          if (!string.IsNullOrWhiteSpace(inParams.POPLATNIK_FIRMA))
          {
              if (inParams.POPLATNIK_FIRMA.Any(c => char.IsLower(c)))
            {
                if (inParams.POPLATNIK_FIRMA.Length >= 5)
                    cmd.Append(" and ADR_NAZEV1 like '" + inParams.POPLATNIK_FIRMA + "%'");
              else
                    cmd.Append(" and ADR_NAZEV1 = '" + inParams.POPLATNIK_FIRMA + "'");
            }
            else
            {
                if (inParams.POPLATNIK_FIRMA.Length >= 5)
                    cmd.Append(" and UPPER(ADR_NAZEV1) like '" + inParams.POPLATNIK_FIRMA + "%'");
              else
                    cmd.Append(" and UPPER(ADR_NAZEV1) = '" + inParams.POPLATNIK_FIRMA + "'");
            }
          }

          if (!string.IsNullOrWhiteSpace(inParams.POPLATNIK_ULICE))
          {
              if (inParams.POPLATNIK_ULICE.Any(c => char.IsLower(c)))  
            {
                if (inParams.POPLATNIK_ULICE.Length >= 5)
                    cmd.Append(" and ADR_KODUL_NAZEV like '" + inParams.POPLATNIK_ULICE + "%'");
              else
                    cmd.Append(" and ADR_KODUL_NAZEV = '" + inParams.POPLATNIK_ULICE + "'");
            }
            else
            {
                if (inParams.POPLATNIK_ULICE.Length >= 5)
                    cmd.Append(" and UPPER(ADR_KODUL_NAZEV) like '" + inParams.POPLATNIK_ULICE + "%'");
              else
                    cmd.Append(" and UPPER(ADR_KODUL_NAZEV) = '" + inParams.POPLATNIK_ULICE + "'");
            }
          }

          if (inParams.POPLATNIK_CP != null)
          {
              cmd.Append(" and ADR_CIS_DOMU = " + inParams.POPLATNIK_CP.ToString());
          }

          if (!string.IsNullOrWhiteSpace(inParams.PES_ZNAMKA_CIP))
          {
               cmd.Append(" and PES_CIP = '" + inParams.PES_ZNAMKA_CIP + "'");
          }

          if (!string.IsNullOrWhiteSpace(inParams.PES_TETOVANI))
          {
              cmd.Append(" and PES_TET = '" + inParams.PES_TETOVANI + "'");
          }

          if (!string.IsNullOrWhiteSpace(inParams.PES_CIP))
          {
              cmd.Append(" and PES_CIP2 = '" + inParams.PES_CIP + "'");
          }

#endregion SQL podminky

        SelectedData resultSet = sesna.ExecuteQueryWithMetadata(cmd.ToString(), pNames.ToArray(), pValues.ToArray());


        string ico = string.Empty;
        PES pes;
        PLATCE_PSA psiPlatce = null;
        if (resultSet.ResultSet[1].Rows.Count() > 0)
          Resp.PLATCI = new List<PLATCE_PSA>();

        foreach (var row in resultSet.ResultSet[1].Rows) 
            {
              if (ico != row.ValByName(resultSet, "ADR_ICO").ToString())
                  {
                    psiPlatce = new PLATCE_PSA();
                    psiPlatce.OSOBA = new OSOBA();
                    row.copyToObject<OSOBA>(resultSet, psiPlatce.OSOBA);
                    row.copyToObject<KONTAKTNI_ADRESA>(resultSet, psiPlatce.OSOBA.KONTAKTNI_ADRESA);
                    object dluh = row.ValByName(resultSet, "DLUH") ?? (object)0;
                    psiPlatce.DLUH = Convert.ToDecimal(dluh) != 0;
                    Resp.PLATCI.Add(psiPlatce);

                    ico = row.ValByName(resultSet, "ADR_ICO").ToString();
                    psiPlatce.PSI = new List<PES>();
                  }
         
               pes = new PES();
               row.copyToObject<PES>(resultSet, pes);
          
               pes.PLEMENO = (string)row.ValByName(resultSet, "C_PES_PLEMENO_NAZEV");
               pes.BARVA = (string)row.ValByName(resultSet, "C_PES_BARVA_NAZEV");
               psiPlatce.PSI.Add(pes);              
            }

        Resp.result = Result.OK;
        if ((psiPlatce != null) && (psiPlatce.PSI.Count() > 0))
          Resp.status = Status.EXISTS;
        else
          Resp.status = Status.NOTEXISTS;

        return Resp;
      }
      catch (Exception exc)
      {
        Resp.result = Result.ERROR;
        Resp.status = Status.ERROR;  //0.27
        


        if (exc.InnerException == null)
        {
          Resp.ERRORMESS = exc.Message;
        }
        else
        {
          Resp.ERRORMESS = exc.InnerException.Message;
        }
        /*
          throw new Exception(String.Format("chyba \n {0}", exc.InnerException.Message));
          */
        return Resp;
      }
    }
  }
}