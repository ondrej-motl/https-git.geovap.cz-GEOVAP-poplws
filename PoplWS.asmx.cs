
#define logujSoap
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;


using System.Xml;   
using System.IO;    
using System.Text;  

namespace PoplWS
{
  public static class WsVersion
  {
    public const string WSVerze = "1.0.0.1";
    public const string WSAssembly = "1.0.0.39";

      /* 
        
            - pri insertu do P_PRPL vyplnovano i LASTUPDATE a LOGIN
       0.38 - 4.12.19 - vlozena uhrada ma PLATBA_EXPORTOVANO = "NE", PLATBA_EXPFIN = 'NE'
         - 12.11.19 - doplnen export storna predpisu ihned po vystaveni       
       0.37 1.10.19 - doplnena zahranicni adresa
       0.36 vynechana 
       0.35 1.8.19 - pri zakladani platce je plnen i VS, pokud format VS neobsahuje rok 
       0.34 13.6.19 - export dat z KO - po exportu dat je potřeba do (web.config parametr KOExpDatExpiraceDni) 30 dnů naimportovat 
                data s úlevami, jinak je potřeba provést nový export.
               .Kazdym novym exportem jsou mazana stara data pro periodu a ext. agendu
                  predchozi davka tak nelze pouzít pro import slev     
                Nelze importovat VS poplatniku, které nebyly v exportu.
               .udaj DLUH nabyva pouze hodnot 0/1  [0 - nema dluh/1 - ma dluh]
               .postacuje pravo "platce videt", musi se jednat o poplatek za KO - nutno mit pravo select nad P_ODPADY_KONFIG
               .novy Web.config  - nova konfiguracni hodnota "KOExpDatExpiraceDni" jak stara data od exportu lze importovat.
        0.33 - typ databaze nacitan z DataLayer, puvodne bylo nacitano z ConnectionString
        0.32 - DevExpress verze 18.2 
           a - XPOConnector.GetSession ve webovem volani nahrazeno za GetSessionMultiThread 
        0.31 - DBValue jiz neni jako singleton
        0.30 -  nova metoda dejPlatce - pozadavek Vity, 6.2.19 nainstalovano v Melniku
        0.29 - oprava vkladani pravnicke osoby - byl chybne vyzadovan datum narozeni
        0.28 - při chybném založení přístupových práv agendou Prijmy končila kontrola na poplatek chybou - pokud existovala větev "neurčeno"  
        0.27 nasazena 10.10.2018 ve verejnem testovacim prostredi
               20.11.2018 nainstalovano v Litomyšli, 23.11.2018 nainstalovano v Melniku
             - 20.11.2018 - u vkladaneho predpisu je odrezavana casova slozka u VYSTAVENO, SPLATNO (class PredpisBase) 
             - s verzi je vhodne distribuovat i novy Web.config - doplnene hodnoty
             - ctenim hodnoty ReturnRC z Web.config je rizeno, zda bude vráceno rč, či nikoli. Defaultni chovani je ReturnRC = 0.
               .lze modifikovat i pro ReturnRCxxx - viz Util.cs
             - DejUhradaPredpisu - do UHRADA_PREDPISU_RESP pridano pole PREDPIS_KC_UHRAZENO 
             - DejPredpisy - pole PLATCE_ID neni plneno, bude proto mít vždy hodntu nula 
                jsou poskytovany predpisy s datem vystavení mladším než je počet roků v hodnotě parametru z Web.config - DejPredpisy_Roku.
                Defaultni hodnota  DejPredpisy_Roku = 5.
                priklad: DejPredpisy_Roku = 7 => PRPL_VYSTAVENO >= '1.1.now-7let'
             - AppSettings.LogMode > 2 zapina logovani chyb do souboru
                                   == 2 nepouzito  
                                   == 1 logovani SOAP zprav zapnuto
                                   == 0 logovani SOAP zprav vypnuto  
       * v MSSQL je pri GetObjectByKey volán select nad "XPObjectType" - dle DevExpress týmu nelze tento dotaz potlačit
        0.26 - 5.9.18 - devexpress verze 18.1
        0.25 - 20.4.18 - nova metoda DejPsiSeznam2 umožňující vyhledávat psy dle čipu, čipové známky, tetování
                       - logovani soap komunikace na zaklade parametru LogMode ["0"/"1"] ve Web.Config.
                         logovani je provadeno do adresare .LogSoap    
        0.24 - 2.2.18 - DejUhrady - vystup je triden dle data platby - prani Zdenala
        0.23 - 26.2.18 - P_PLATBA - PLATBA_SS zmena typu z int na decimal
        0.22 - 20.2.18 - oprava DejPsiSeznam
        0.21 - 23.2.2018 - data v P_GEO_PAR odmazavany az po 18 mesicich
               31.1.2018 - P_PLATBA.PLATBA_POPLATEK - nastaveni default hodnoty na -1. Chybně se nastavovalo na nulu.
                0.20 - DPH - pri vkladani a update platce je updatovana SPLATKA, POSLSPLATKA 
                0.19 - DPH - doplneni metody InsertPlatce o dorovnavaci radek DPH
                           . zaokrouhlovani splatky a posledni splatky DPH na 2 des. mista
                0.18 - 6.10.17 - ukladani sazby rozpisu DPH u platce, u predpisu bylo OK
                0.17 - updatePlatce updatuje i RGP_FROMDATE, RGP_TODATE
                0.16 - změna kontroly RGP_KCROK v PlatceInsert  
                     - doplnen Resp.result = Result.ERROR pri kontrole pouziti ext. VS - insert platce  
                     - export predpisu do ucta je provaden dle nastaveni loginu - lze jej tedy exportovat ihned po jeho vlozeni (provadi dbu.PredpisExport)   
                0.14 - 7.4.17 - nova metoda DejOsobu  
                     - 14.3.17 - pri zakladani predpisu je OLDVS (pokud neni vyplnen) prebiran od platce
                0.13 - doplnena metoda pro stornovani predpisu    
                0.12 - criteriaOperator predelany z String.Format na positional parameters   
                0.11 - predelani vypoctu v DBUtil.GetRgpSplatky
                0.9 - nova metoda UpdatePlatce - zatim nezverejnena do web. sluzeb
                - nove pretizeni metody InsertPredpis(Session session, int EXT_APP_KOD, PREDPIS predpis, bool nastavitPredpisKCdleDPH)
                    pridan parametr nastavitKCdleDPH, pokud je true, je zpetne nastaven predpis.KC dle jednotlivych radku DPH
                - pretizeni metody InsertPlatce - pridan parametr nastavitKCdleDPH, pokud je true, je zpetne nastavena KCSPLATKA a POSLSPLATKA dle rozpisu DPH
                - do insertPlatce pridana kontrola splátky a výše splátky dle rozpisu DPH 
                0.8 - pro ORACLE je potreba v connectStringu nastavit XPOProvider na XpoProvider=PoplOracle ... - viz class PoplOracleConnectionProvider
                    - ADR_ICO je nepovinne => nutno Prijmy min. verze 3.118
                    - metoda InsertPlatce bezi transakcne, transakce může být řízena vne procedury InsertPlatce
                        bod pomoci  using (PoplWS.MyUnitOfWork uow = new PoplWS.MyUnitOfWork(sesna.DataLayer))
                                    { .... uow.CommitChanges();
                        nebo pomoci sesna.BeginTransaction();
                                        ....  sesna.CommitTransaction();
                        pokud neni transakce obslouzena ani jednim ze dvou zpusobu, je nastartovana a comitovana
                        uvnitr metody InsertPlatce.
                        .DropIdentityMap nahrazeno pomoci reload objektu C_EVPOPL - aby nedochazelo k ukoncovani zvenku rizene transakce
                0.7 - chybna kontrola - pravnicka osoba jiz nemusi mit datum narozeni
                    - oprava vkladani fyzicke osoby se zadanym jmenem a prijmenim  
                0.6 - 14.8.2015 - P_DATMODEL 3.110
                                    pokud pri zakladani platce nebyl uveden export do financi byl nastaven na NE.
                                    Nyni se cte dle nastaveni v C_EVPOPL
                0.5 - pri vyhledani psu dle ulice nemusi byt zadano cislo popisne
            1.0.0.4 - vyhledávání psů je možné i case insensitive. 
                        Pokud podmínka obsahuje pouze velká písmena, je hledání case insensitive
            1.0.0.3 - nova metoda DejPsiSeznam 
                        Metoda kontroluje opravněni pro login na úrovni poplatku dle nastavení přístupu v agendě Příjmy.
                        Je pozadovano alespon prohlizeni predpisu a platcu.
                  */

  }
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://www.geovap.cz/PoplWS/", Name = "PoplWs ", Description = "Verze webové služby: " + WsVersion.WSAssembly)]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class PoplWS : System.Web.Services.WebService
    {


#if logujSoap
        [SpyExtension("DejTypyPohledavek")] 
#endif
      [WebMethod]
      public WebMethod.POPLATKY_RESP DejTypyPohledavek(string USER_NAME, string PASSWORD, int EXT_APP_KOD)
        {

         try
         {
            WebMethod.SeznamPoplatku seznam = new WebMethod.SeznamPoplatku(USER_NAME, PASSWORD, EXT_APP_KOD);
            return seznam.dejSeznam();
          }
          catch (Exception e)
          {
              return new WebMethod.POPLATKY_RESP { ERRORMESS = e.Message };
          }
        }


#if logujSoap
        [SpyExtension("VlozOsobu")]
#endif
       [WebMethod]
        public OSOBA_RESP VlozOsobu(string USER_NAME, string PASSWORD, int EXT_APP_KOD, OSOBA osoba)
        {
            Session sesna = null;
            try
            {
                try
                {
                  XPOConnector xpc = new XPOConnector(USER_NAME, PASSWORD);
                  sesna = xpc.GetSessionMultiThread();
                  WebMethod.OsobaInsert insOsoba = new WebMethod.OsobaInsert();
                  return insOsoba.InsertOsobu(sesna, EXT_APP_KOD, osoba);
                }
                catch (Exception exc)
                {
                    Util.Util.WriteLog("wmError-" + exc.Message + "\n" + exc.StackTrace);
                    return null;
                }
            }
            finally
            {
                sesna.Dispose();
            }
        }

#if logujSoap
        [SpyExtension("VlozPlatce")]
#endif
      [WebMethod]
     public PLATCE_RESP VlozPlatce(string USER_NAME, string PASSWORD, int EXT_APP_KOD, PLATCE platce)
        {
            Session sesna = null;
            try
            {
                try
                {
                    XPOConnector xpc = new XPOConnector(USER_NAME, PASSWORD);
                    sesna = xpc.GetSessionMultiThread();
                    WebMethod.PlatceInsert insPlatce = new WebMethod.PlatceInsert();
                    return insPlatce.InsertPlatce(sesna, EXT_APP_KOD, platce);
                }
	            catch (Exception exc)
	            {
                    Util.Util.WriteLog("wmError-" + exc.Message + "\n" + exc.StackTrace);
                    return null;
	            }
            }
            finally
            {
                sesna.Dispose();
            }
        }
    

#if logujSoap
        [SpyExtension("VlozPredpis")]
#endif
       [WebMethod]
        public PREDPIS_RESP VlozPredpis(string USER_NAME, string PASSWORD, int EXT_APP_KOD, PREDPIS predpis)
        {
            Session sesna = null;
            try
            {
                try
                {
                  XPOConnector xpc = new XPOConnector(USER_NAME, PASSWORD);
                  sesna = xpc.GetSessionMultiThread();
                  WebMethod.PredpisInsert insPredpis = new WebMethod.PredpisInsert();
                  return insPredpis.InsertPredpis(sesna, EXT_APP_KOD, predpis);
                }
                catch (Exception exc)
                {
                    Util.Util.WriteLog("wmError-" + exc.Message + "\n" + exc.StackTrace);
                    return null;
                }
            }
            finally
            {
                sesna.Dispose();
            }
        }


#if logujSoap
        [SpyExtension("DejUhradyPredpisu")]
#endif
       [WebMethod]
        public UHRADA_PREDPISU_RESP DejUhradyPredpisu(string USER_NAME, string PASSWORD, int EXT_APP_KOD, int PREDPIS_ID)
        {
          XPOConnector xpc = new XPOConnector(USER_NAME, PASSWORD);
          Session sesna = xpc.GetSessionMultiThread();
          WebMethod.DejUhradaPredpisu getPlatby = new WebMethod.DejUhradaPredpisu();
          return getPlatby.dej_PlatbyKPredpisu(sesna, EXT_APP_KOD, PREDPIS_ID);
          sesna.Dispose();
        }

#if logujSoap
        [SpyExtension("DejOsobuDleDatNar")]
#endif
        [WebMethod]
        public DEJOSOBU_RESP DejOsobuDleDatNar(string USER_NAME, string PASSWORD, int EXT_APP_KOD, string JMENO, string PRIJMENI, DateTime DATUM_NAROZENI)
        {
          XPOConnector xpc = new XPOConnector(USER_NAME, PASSWORD);
          Session sesna = xpc.GetSessionMultiThread();
          WebMethod.DejOsobu findOsobu = new WebMethod.DejOsobu();
          return findOsobu.Najdi(sesna, EXT_APP_KOD, JMENO, PRIJMENI, DATUM_NAROZENI, "");
        }

#if logujSoap
        [SpyExtension("DejOsobuDleRCIC")]
#endif
        [WebMethod]
        public DEJOSOBU_RESP DejOsobuDleRCIC(string USER_NAME, string PASSWORD, int EXT_APP_KOD, string RC_IC)
        {
          XPOConnector xpc = new XPOConnector(USER_NAME, PASSWORD);
          Session sesna = xpc.GetSessionMultiThread();
          WebMethod.DejOsobu findOsobu = new WebMethod.DejOsobu();
          return findOsobu.Najdi(sesna, EXT_APP_KOD, RC_IC);
        }

#if logujSoap
        [SpyExtension("DejOsobu")]
#endif
        [WebMethod]
        public DEJOSOBU_RESP DejOsobu(string USER_NAME, string PASSWORD, int EXT_APP_KOD, string RC_IC, string JMENO, string PRIJMENI, DateTime DATUM_NAROZENI, string NAZEV)
        {
            XPOConnector xpc = new XPOConnector(USER_NAME, PASSWORD);
            Session sesna = xpc.GetSessionMultiThread();
            WebMethod.DejOsobu findOsobu = new WebMethod.DejOsobu();
            return findOsobu.NajdiOsobu(sesna, EXT_APP_KOD, RC_IC, JMENO, PRIJMENI, DATUM_NAROZENI, NAZEV);
        }

#if logujSoap
        [SpyExtension("DejUhradyVS")]
#endif
        [WebMethod]
        public UHRADY_RESP DejUhradyVS(string USER_NAME, string PASSWORD, int EXT_APP_KOD, string VS)
        {
          XPOConnector xpc = new XPOConnector(USER_NAME, PASSWORD);
          Session sesna = xpc.GetSessionMultiThread();
          WebMethod.DejUhradyVS getPlatby = new WebMethod.DejUhradyVS();
          return getPlatby.DejUhrady(sesna, EXT_APP_KOD, VS);
        }

#if logujSoap
        [SpyExtension("DejUhrady")]
#endif
        [WebMethod]
        public UHRADY_NEW_RESP DejUhrady(string USER_NAME, string PASSWORD, int EXT_APP_KOD, string DAVKA, int PLATBY_EA)
        {
          XPOConnector xpc = new XPOConnector(USER_NAME, PASSWORD);
          Session sesna = xpc.GetSessionMultiThread();
          WebMethod.DejUhradyPar getPlatby = new WebMethod.DejUhradyPar();
          int NevracetPlatbyEA = PLATBY_EA;
          if (EXT_APP_KOD == PLATBY_EA)
              NevracetPlatbyEA = PLATBY_EA;
            else
              NevracetPlatbyEA = 0;  
          return getPlatby.DejUhrady(sesna, EXT_APP_KOD, DAVKA, NevracetPlatbyEA);
        }


#if logujSoap
        [SpyExtension("DejPlatce")]
#endif
        [WebMethod]
        public PLATCE_RESP DejPlatce(string USER_NAME, string PASSWORD, int EXT_APP_KOD, GET_PLATCE_PARAMS INPUT_PARAMS)
        {
            XPOConnector xpc = new XPOConnector(USER_NAME, PASSWORD);
            Session sesna = xpc.GetSessionMultiThread();
            WebMethod.NajdiPlatce platce = new WebMethod.NajdiPlatce();
            return platce.DejPlatce(sesna, EXT_APP_KOD, INPUT_PARAMS);
        }


#if logujSoap
        [SpyExtension("DejPredpisy")]
#endif
        [WebMethod]
        public PREDPISY_RESP DejPredpisy(string USER_NAME, string PASSWORD, int EXT_APP_KOD, int OSOBA_ID, int POPLATEK)
        {
            XPOConnector xpc = new XPOConnector(USER_NAME, PASSWORD);
            Session sesna = xpc.GetSessionMultiThread();
            WebMethod.SeznamPredpisu seznamPredpisu = new WebMethod.SeznamPredpisu();
            return seznamPredpisu.DejPredpisy(sesna, EXT_APP_KOD, OSOBA_ID, POPLATEK);
        }


#if logujSoap
        [SpyExtension("VlozUhraduPredpisu")]
#endif
        [WebMethod]
        public UHRADY_RESP VlozUhraduPredpisu(string USER_NAME, string PASSWORD, int EXT_APP_KOD, int PREDPIS_ID, decimal KC, DateTime DATUM_UHRADY, string ZAPLATIL, string DOKLAD, int SS)
        {
          XPOConnector xpc = new XPOConnector(USER_NAME, PASSWORD);
          Session sesna = xpc.GetSessionMultiThread();
          WebMethod.PlatbaInsert insPlatbu = new WebMethod.PlatbaInsert();
          return insPlatbu.InsertUhraduPredpisu(sesna, EXT_APP_KOD, PREDPIS_ID, KC, DATUM_UHRADY, ZAPLATIL, DOKLAD, SS );
        }

        //[WebMethod]
        public STORNO_RESP ZrusPredpis(string USER_NAME, string PASSWORD, int EXT_APP_KOD, int PREDPIS_ID, string poznamka)
        {
          XPOConnector xpc = new XPOConnector(USER_NAME, PASSWORD);
          Session sesna = xpc.GetSessionMultiThread();
          WebMethod.PredpisStorno stornoPr = new WebMethod.PredpisStorno();
          return stornoPr.StornoPredpis(sesna, EXT_APP_KOD, PREDPIS_ID, "storno předpisu pomoci WS");
        }   


      //PSI
#if logujSoap
        [SpyExtension("DejPsiPlemena")]
#endif
        [WebMethod]
        public PESPLEMENA_RESP DejPsiPlemena(string USER_NAME, string PASSWORD, int EXT_APP_KOD)
        {
          XPOConnector xpc = new XPOConnector(USER_NAME, PASSWORD);
          Session sesna = xpc.GetSessionMultiThread();
          WebMethod.DejPsiPlemena plemena = new WebMethod.DejPsiPlemena();
          return plemena.DejPsiSeznamPlemen(sesna, EXT_APP_KOD);
        }

#if logujSoap
        [SpyExtension("DejPsiSeznam")]
#endif
        [WebMethod]
        public DEJPSY_RESP DejPsiSeznam(string USER_NAME, string PASSWORD, int EXT_APP_KOD, int? PES_ZNAMKA, string PES_JMENO,
                                   int? PES_PLEMENO_KOD, string POPLATNIK_PRIJMENI, string POPLATNIKA_FIRMA,
                                   string POPLATNIK_ULICE, int? POPLATNIK_CP)
        {
          XPOConnector xpc = new XPOConnector(USER_NAME, PASSWORD);
          Session sesna = xpc.GetSessionMultiThread();
          WebMethod.SeznamPsu seznamPsu = new WebMethod.SeznamPsu();
          return seznamPsu.DejPsiSeznam(sesna, EXT_APP_KOD, PES_ZNAMKA, PES_JMENO,
                                          PES_PLEMENO_KOD, POPLATNIK_PRIJMENI, POPLATNIKA_FIRMA,
                                          POPLATNIK_ULICE, POPLATNIK_CP );
        }

#if logujSoap
        [SpyExtension("DejPsiSeznam2")]
#endif
        [WebMethod]
        public DEJPSY_RESP DejPsiSeznam2(string USER_NAME, string PASSWORD, int EXT_APP_KOD, GET_PES_PARAMS INPUT_PARAMS)
        {
            XPOConnector xpc = new XPOConnector(USER_NAME, PASSWORD);
            Session sesna = xpc.GetSessionMultiThread();
            WebMethod.SeznamPsu seznamPsu = new WebMethod.SeznamPsu();
            return seznamPsu.DejPsiSeznam(sesna, EXT_APP_KOD, INPUT_PARAMS);
        }

#if logujSoap
        [SpyExtension("VlozStornoPredpisu")]
#endif
        [WebMethod]
        public STORNO_RESP VlozStornoPredpisu(string USER_NAME, string PASSWORD, int EXT_APP_KOD, int PREDPIS_ID, string POZNAMKA)
        {
            XPOConnector xpc = new XPOConnector(USER_NAME, PASSWORD);
            Session sesna = xpc.GetSessionMultiThread();
            WebMethod.PredpisStorno storno = new WebMethod.PredpisStorno();
            return storno.StornoPredpis(sesna, EXT_APP_KOD, PREDPIS_ID, POZNAMKA);
        }


#if logujSoap
        [SpyExtension("DejStaty")]
#endif
        [WebMethod]
        public STATY_RESP DejStaty(string USER_NAME, string PASSWORD, int EXT_APP_KOD)
        {
          XPOConnector xpc = new XPOConnector(USER_NAME, PASSWORD);
          Session sesna = xpc.GetSessionMultiThread();
          WebMethod.DejStaty staty = new WebMethod.DejStaty();
          return staty.DejSeznamStaty(sesna, EXT_APP_KOD);
        }

#if logujSoap
        [SpyExtension("DejPlatceKO")]
#endif
        [WebMethod]
        public PLATCI_RESP DejPlatceKO(string USER_NAME, string PASSWORD, int EXT_APP_KOD, GET_PLATCE_PARAMS INPUT_PARAMS)
        {
            XPOConnector xpc = new XPOConnector(USER_NAME, PASSWORD);
            Session sesna = xpc.GetSessionMultiThread();
            WebMethod.NajdiPlatceKO platce = new WebMethod.NajdiPlatceKO();
            return platce.DejPlatceKO(sesna, EXT_APP_KOD, INPUT_PARAMS);
        }

#if logujSoap
        [SpyExtension("PlatceSlevaKO")]
#endif
        [WebMethod]
        public POPLATNIK_SLEVA_RESP PlatceSlevaKO(string USER_NAME, string PASSWORD, int EXT_APP_KOD, POPLATNIK_SLEVA INPUT_PARAMS)
        {
            XPOConnector xpc = new XPOConnector(USER_NAME, PASSWORD);
            Session sesna = xpc.GetSessionMultiThread();
            WebMethod.SlevaPlatceKO slevy = new WebMethod.SlevaPlatceKO();
            return slevy.PlatceSlevaKO(sesna, EXT_APP_KOD, INPUT_PARAMS);
        }

    
    }
}