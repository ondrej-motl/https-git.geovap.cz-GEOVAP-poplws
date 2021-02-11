using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace PoplWS.WebMethod
{
  public class DejOsobu
  {
      public DEJOSOBU_RESP NajdiOsobu(Session sesna, int EXT_APP_KOD, string RC_IC, string JMENO, string PRIJMENI, DateTime? DATUM_NAROZENI, string NAZEV)
      {
          DEJOSOBU_RESP osobaResp = new DEJOSOBU_RESP();
          try
          {
              if (! string.IsNullOrWhiteSpace(RC_IC))
                  return Najdi(sesna, EXT_APP_KOD, RC_IC);

              if (!string.IsNullOrWhiteSpace(JMENO) && !string.IsNullOrWhiteSpace(PRIJMENI) && (DATUM_NAROZENI.HasValue))
                  return Najdi(sesna, EXT_APP_KOD, JMENO, PRIJMENI, DATUM_NAROZENI.Value, "");

              if (!string.IsNullOrWhiteSpace(NAZEV))
                  return Najdi(sesna, EXT_APP_KOD, "", "", new DateTime(2000, 1, 1), NAZEV);


              osobaResp.ERRORMESS = "Chybné vstupní parametry. \n Povolené kombinace: " +
                                    " \n - RC_IC" +
                                    " \n - JMENO, PRIJMENI, DATUM_NAROZENI" +
                                    " \n - NAZEV";
              osobaResp.result = Result.ERROR;
              osobaResp.status = Status.ERROR;
              return osobaResp;
          }
          catch (Exception exc)
          {
              osobaResp.result = Result.ERROR;
              osobaResp.status = Status.ERROR;

              if (exc.InnerException == null)
              {
                  osobaResp.ERRORMESS = exc.Message;
              }
              else
              {
                  osobaResp.ERRORMESS = exc.InnerException.Message;
              }
              return osobaResp;
          }
      }

      public DEJOSOBU_RESP Najdi(Session sesna, int EXT_APP_KOD, string JMENO, string PRIJMENI, DateTime DATUM_NAROZENI, string NAZEV)
    {
      DEJOSOBU_RESP osobaResp = new DEJOSOBU_RESP();
      try
      {
        CriteriaOperator criteria;
        if (NAZEV.Length > 0)
             criteria = CriteriaOperator.Parse("Contains(UPPER(ADR_NAZEV1), ?) and ADR_TYP = ?",
                                                        NAZEV.ToUpper(), "P");  
          else
             criteria = CriteriaOperator.Parse("UPPER(ADR_JMENO) = ? and UPPER(ADR_PRIJMENI) = ? and ADR_DATNAR = ? ", JMENO.ToUpper(), PRIJMENI.ToUpper(), DATUM_NAROZENI); 
        XPCollection<P_ADRESA_ROBRHS> adrExist = new XPCollection<P_ADRESA_ROBRHS>(sesna, criteria);

        if (adrExist.Count > 100)
        {
            osobaResp.result = Result.ERROR;
            osobaResp.status = Status.TOOMANY;
            osobaResp.ERRORMESS = "Příliš mnoho hodnot, je potřeba zpřesnit výběrové kritérium.";
            return osobaResp;
        }

        if (adrExist.Count > 0)
        {
          osobaResp.result = Result.OK;
          osobaResp.status = Status.EXISTS;
          int returnRC;
          if ( ! int.TryParse(System.Web.Configuration.WebConfigurationManager.AppSettings["ReturnRC"], out returnRC));
              returnRC = 0;

          foreach (var item in adrExist)
          {
            //zkopiruji adresu a kontaktni adresu
            OSOBA osoba = new OSOBA();
            Utils.copy.CopyDlePersistentAttr<OSOBA>(item, osoba);
            Utils.copy.CopyDlePersistentAttr<KONTAKTNI_ADRESA>(item, osoba.KONTAKTNI_ADRESA);
            osoba.RC_IC = item.ADR_SICO;   //0.8
              if ((osoba.TYP == TypOsoby.C) || (osoba.TYP == TypOsoby.F))
                  if (returnRC == 0)
                       osoba.RC_IC = null;
            osobaResp.OSOBY.Add(osoba);
          }
        }
        else
           osobaResp.status = Status.NOTEXISTS;

        return osobaResp;
      }
      catch (Exception exc)
      {
        osobaResp.result = Result.ERROR;

        if (exc.InnerException == null)
        {
          osobaResp.ERRORMESS = exc.Message;
        }
        else
        {
          osobaResp.ERRORMESS = exc.InnerException.Message;
        }
        return osobaResp;
      }

    }

   public DEJOSOBU_RESP Najdi(Session session, int EXT_APP_KOD, string RC_IC)
    {
      Session sesna = session;
      DEJOSOBU_RESP osobaResp = new DEJOSOBU_RESP();

      try
      {
          CriteriaOperator criteria = CriteriaOperator.Parse("ADR_SICO = ?", RC_IC);
        XPCollection<P_ADRESA_ROBRHS> adrExist = new XPCollection<P_ADRESA_ROBRHS>(sesna, criteria);
        if (adrExist.Count > 0)
        {
          osobaResp.result = Result.OK;
          osobaResp.status = Status.EXISTS;
          foreach (var item in adrExist)
          {
            //zkopiruji adresu a kontaktni adresu
            OSOBA osoba = new OSOBA();
            Utils.copy.CopyDlePersistentAttr<OSOBA>(item, osoba);
            Utils.copy.CopyDlePersistentAttr<KONTAKTNI_ADRESA>(item, osoba.KONTAKTNI_ADRESA);
            osoba.RC_IC = item.ADR_SICO;   //0.8
            osobaResp.OSOBY.Add(osoba);
          }
        }
        else
          osobaResp.status = Status.NOTEXISTS;
        return osobaResp;
      }
      catch (Exception exc)
      {
        osobaResp.result = Result.ERROR;

        if (exc.InnerException == null)
        {
          osobaResp.ERRORMESS = exc.Message;
        }
        else
        {
          osobaResp.ERRORMESS = exc.InnerException.Message;
        }
        return osobaResp;
      }

    }

  }
}