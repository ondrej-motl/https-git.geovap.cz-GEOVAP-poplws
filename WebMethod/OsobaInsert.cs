using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace PoplWS.WebMethod
{
    public class OsobaInsert
    {
        public OSOBA_RESP InsertOsobu(Session session, int EXT_APP_KOD, OSOBA osoba)
        {
            Session sesna = session;
            OSOBA_RESP osobaResp = new OSOBA_RESP();
            osoba.ADR_EA = EXT_APP_KOD;

            try
            {

#region test  vyplneni vstupnich hodnot

                if (osoba.RC_IC != null)
                   osoba.RC_IC = osoba.RC_IC.Trim();
                if (osoba.JMENO != null)
                  osoba.JMENO = osoba.JMENO.Trim();
                if (osoba.PRIJMENI != null)
                  osoba.PRIJMENI = osoba.PRIJMENI.Trim();

                if (string.IsNullOrEmpty(osoba.RC_IC))  // na string.IsNullOrWhiteSpace(osoba.RC_IC) testovat nemusim, 
                                                        //protoze je vyse Trimovan 
                {
                    P_NASTAVENI nast = sesna.GetObjectByKey<P_NASTAVENI>("ICOP");
                    if ((nast == null) || ((nast != null) && (nast.HODNOTA == "1")))
                    {
                        osobaResp.result = Result.ERROR;
                        osobaResp.ERRORMESS = "IČ/RČ musí­ být vyplněno";
                        return osobaResp;
                    }
                }

              if (EXT_APP_KOD == null)
              { throw new Exception("kód externí aplikace není zadán"); }
              P_EXTAPP EA = sesna.GetObjectByKey<P_EXTAPP>(EXT_APP_KOD);
              if (EA == null)
              { throw new Exception("chybný kód externí aplikace"); }

              if (((osoba.TYP == TypOsoby.F) || (osoba.TYP == TypOsoby.C))  
                  && (string.IsNullOrEmpty(osoba.RC_IC))
                  && (string.IsNullOrEmpty(osoba.JMENO) || string.IsNullOrEmpty(osoba.PRIJMENI) || 
                             (!osoba.DATUM_NAROZENI.HasValue))
                  )
              { throw new Exception("u fyzické osoby musí být vyplněno jméno, příjmení a datum narození"); }


              if (!string.IsNullOrEmpty(osoba.STAT))
              {
                  P_STAT stat = sesna.FindObject<P_STAT>(CriteriaOperator.Parse("Upper(STAT_NAZEV) = ?", osoba.STAT.ToUpper()));
                  if (stat == null)
                      stat = sesna.FindObject<P_STAT>(CriteriaOperator.Parse("STAT_KOD = ?", osoba.STAT.ToUpper()));

                  if (stat == null)
                      throw new Exception(string.Format("stát \"{0}\" není uveden v číselníku států", osoba.STAT));
                  else
                      osoba.STAT = stat.STAT_KOD;
              }
              else
              {
                  osoba.STAT = null;  
              }


              if (osoba.ZAHRANICNI_ADRESA != null)
              {
                  if ((osoba.TYP != TypOsoby.C) && (osoba.TYP != TypOsoby.P))
                    throw new Exception(string.Format("Zahraniční adresu lze uvádět pouze pro cizince a právnické osoby."));
                  if (String.IsNullOrEmpty(osoba.ZAHRANICNI_ADRESA.STAT))
                    throw new Exception(string.Format("U zahraniční adresy musí být uveden stát."));

                  if (   !String.IsNullOrEmpty(osoba.OBEC_NAZEV) || !String.IsNullOrEmpty(osoba.ULICE_NAZEV)
                       || (osoba.CIS_DOMU != null) || (osoba.CIS_OR != null)
                       || !String.IsNullOrEmpty(osoba.PSC) || !String.IsNullOrEmpty(osoba.STAT)
                       )
                    throw new Exception(string.Format("U zahraniční adresy nelze současně uvádět trvalou adresu."));

                  P_STAT stat = sesna.FindObject<P_STAT>(CriteriaOperator.Parse("Upper(STAT_NAZEV) = ?", osoba.ZAHRANICNI_ADRESA.STAT.ToUpper()));
                  if (stat == null)
                      stat = sesna.FindObject<P_STAT>(CriteriaOperator.Parse("STAT_KOD = ?", osoba.ZAHRANICNI_ADRESA.STAT.ToUpper()));
                  if (stat == null)
                      throw new Exception(string.Format("Stát \"{0}\" není uveden v číselníku států.", osoba.ZAHRANICNI_ADRESA.STAT));
              }


#endregion test  vyplneni vstupnich hodnot

              using (UnitOfWork uow = new UnitOfWork(sesna.DataLayer))
              {
                //dotaz, zda jiz neexistuje
                  if (!string.IsNullOrEmpty(osoba.RC_IC))
                  {
                      CriteriaOperator criteria = CriteriaOperator.Parse("(ADR_SICO = ? or ADR_ICO = ?)", osoba.RC_IC, osoba.RC_IC);
                      XPCollection<P_ADRESA_ROBRHS> adrExist = new XPCollection<P_ADRESA_ROBRHS>(sesna, criteria);
                      if (adrExist.Count > 0)
                      {
                          #region adresa existuje - muze existovat vice osob se stejnym ADR_SICO
                          #region adresa je vybirana v poradi 
                                                        
                          P_ADRESA_ROBRHS osobaExist = null;
                          int isico = -1;
                          int iico = -1;
                          for (int i = 1; i <= 4; i++)
                          {
                              if (i == 1)
                                  adrExist.Filter = CriteriaOperator.Parse("ADR_EA = ? and ADR_ADRPOPL > -1", EXT_APP_KOD);
                              if (i == 2)
                                  adrExist.Filter = CriteriaOperator.Parse("ADR_ADRPOPL > -1");
                              if (i == 3)
                                  adrExist.Filter = CriteriaOperator.Parse("ADR_EA = ?", EXT_APP_KOD);
                              if (i == 4)
                                  adrExist.Filter = null;
                              if (adrExist.Count > 0) 
                              {   if (adrExist.First(P_ADRESA_ROBRHS => P_ADRESA_ROBRHS.ADR_SICO == osoba.RC_IC).ADR_SICO != null)
                                      isico = 0;
                                  if (adrExist.First(P_ADRESA_ROBRHS => P_ADRESA_ROBRHS.ADR_ICO == osoba.RC_IC).ADR_ICO != null)
                                      iico = 0;
                              }
                                                     
                              if (isico > -1 || iico > -1)
                                  break;
                          }
                          #endregion adresa je vybrana v poradi

                          if (isico > -1 || iico > -1)
                          {
                              osobaExist = isico >= 0 ? adrExist[isico] : adrExist[iico];

                              if ((adrExist[adrExist.IndexOf(osobaExist)].ADR_SICO == null) && (isico == -1) && (iico >= 0))
                              {  
                                  adrExist[iico].ADR_SICO = osoba.RC_IC;
                                  adrExist[iico].Save();
                              }

                              osobaResp.result = Result.OK;
                              osobaResp.status = Status.EXISTS;
                              
                              Utils.copy.CopyDlePersistentAttr<OSOBA_RESP>(osobaExist, osobaResp);
                              Utils.copy.CopyDlePersistentAttr<KONTAKTNI_ADRESA>(osobaExist, osobaResp.KONTAKTNI_ADRESA);
                              osobaResp.RC_IC = osobaExist.ADR_SICO;  //0.8
                              #endregion adresa existuje
                              return osobaResp;
                          }
                      }
                  }

                P_ADRESA_ROBRHS adr = new P_ADRESA_ROBRHS(uow);

                Utils.copy.CopyDlePersistentAttr<P_ADRESA_ROBRHS>(osoba, adr);
                Utils.copy.CopyDlePersistentAttr<P_ADRESA_ROBRHS>(osoba.KONTAKTNI_ADRESA, adr);
                if (osoba.ZAHRANICNI_ADRESA != null)
                   Utils.copy.CopyDlePersistentAttr<P_ADRESA_ROBRHS>(osoba.ZAHRANICNI_ADRESA, adr);

                try
                {
                  uow.CommitChanges();
                  CriteriaOperator criteria = CriteriaOperator.Parse("ADR_ICO = ?", adr.ADR_ICO);
                  XPCollection<P_ADRESA_ROBRHS> adrInserted = new XPCollection<P_ADRESA_ROBRHS>(sesna, criteria);

                  Utils.copy.CopyDlePersistentAttr<OSOBA_RESP>(adrInserted[0], osobaResp);
                  Utils.copy.CopyDlePersistentAttr<KONTAKTNI_ADRESA>(adrInserted[0], osobaResp.KONTAKTNI_ADRESA);

                  osobaResp.result = Result.OK;
                  osobaResp.status = Status.INSERTED;
                  osobaResp.ADR_ID = adrInserted[0].ADR_ID;
                  osobaResp.RC_IC = adrInserted[0].ADR_SICO;  //0.8
                  return osobaResp;
                }
                catch (Exception exc)
                {
                  uow.RollbackTransaction();
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
                  /*
                   throw new Exception(String.Format("chyba \n {0}", exc.InnerException.Message));
                   */
                }
              }  //uow
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

              Util.Util.WriteLog(osobaResp.ERRORMESS + "\n" + exc.StackTrace);

              return osobaResp;
              /*
                throw new Exception(String.Format("chyba \n {0}", exc.InnerException.Message));
                */
            }

        }
    }
}