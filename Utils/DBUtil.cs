using System;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using System.Linq;
using System.Collections.Generic;  //List

using DevExpress.Xpo;

namespace PoplWS
{
    public class DBUtil
    {
      Session sesna;
      public DBUtil(Session session)
      {
        sesna = session;
      }


        public int LIZNI_SEQ(string sequence)
        {
          DBVal dbv = new DBVal(sesna);
          IDBValFactory idbv = dbv.CreateDBV();
          int ID = 0;
          
          if (idbv.Database == typeof(MSSQL))
          {
            if (sequence == "PRPL_ID") sequence = "P_SEQUENCE_PRPL";
            if (sequence == "PLATBA_ID") sequence = "P_SEQUENCE_PLATBA";
            DevExpress.Xpo.DB.SelectedData selData = sesna.ExecuteSproc("LIZNI_SEQ", new OperandValue(sequence));
            ID = Convert.ToInt32(selData.ResultSet[0].Rows[0].Values[0]);
          }

          if (idbv.GetType() == typeof(ORACLE))
          {
            if (sequence == "PRPL_ID") sequence = "P_SEQ_PRPL_ID";
            if (sequence == "PLATBA_ID") sequence = "P_SEQ_PLATBA_ID";
            object obj = sesna.ExecuteScalar("select " + sequence + ".Nextval ID from dual");
            ID = Convert.ToInt32(obj);
          }

          if (ID == 0)
          { throw new Exception(String.Format("chyba při čtení hodnoty sequence {0}", sequence)); }

          return ID;
        }

        internal bool IsPoplKO(decimal popl, out string poplKO)
        {
            poplKO = "-1";
            object obj = sesna.ExecuteScalar("select KONF_POPLATEK from P_ODPADY_KONFIG");
            if (obj != null)
            {
                poplKO = obj.ToString();
                return Convert.ToInt32(obj) == popl;
            }
            else
                return false;
        }
        
        public void GetRgpSplatky(decimal kcrok, string perioda, ref decimal kcsplatka, ref decimal poslsplatka, ref short pocSplatek, ref short periodicita)
        {
            XPQuery<C_PERIODA> periody = sesna.Query<C_PERIODA>();

            try
            {
                var list = (from a in periody
                            where a.PERIODA_PERIODA == perioda
                            select a).First();

                int pocPer = 0;
                kcsplatka = 0;
                if (list != null)
                {
                    pocPer = (int)list.PERIODA_POC;
                    periodicita = (short)list.PERIODA_POC;
                    kcsplatka = (int)(kcrok / pocPer);
                    if (kcsplatka < 1)   //pokud by 1 Kc chteli platit na více splátek
                        poslsplatka = kcrok;
                    else
                        poslsplatka = kcrok - (kcsplatka * (pocPer - 1));

                    if (pocPer == 365)
                    {
                        pocSplatek = 1;
                        kcsplatka = kcrok;
                        poslsplatka = kcrok;
                    }
                    else
                    { pocSplatek = (short)pocPer; }
                }
                else
                {
                    throw new Exception(string.Format("Perioda '{0}' neexistuje.", perioda));
                }
            }
            catch
            {
                throw new Exception(string.Format("Perioda '{0}' neexistuje", perioda));
            }


        }

        public void GetEvpoplData(decimal popl, string per, 
                                        ref string expfin,
                                        ref decimal procsankce,
                                        ref string typsankce,
                                        ref decimal pevnacastka,
                                        ref decimal nasobek, 
                                        ref string pernas,
                                        ref string vystup,
                                        ref int porvs)
        {
            XPQuery<C_EVPOPL> evpopls = sesna.Query<C_EVPOPL>();

            CriteriaOperator criteria = CriteriaOperator.Parse("CompoundKey1.EVPOPL_KOD.NAZPOPL_POPLATEK = ? " +
                                                "and CompoundKey1.EVPOPL_PER.PERIODA_PERIODA = ?",  
                                                  popl, per);
            C_EVPOPL ep = sesna.FindObject<C_EVPOPL>(criteria); //pokud je v cache s temito criterii, vraci objekt z cache
            ep.Reload();  

            var list = from a in evpopls
                       where (a.EVPOPL_FROMDATE <= DateTime.Now   //LINQ prevede DateTime.Now pro MSSQL na getdate()
                              && (a.EVPOPL_TODATE >= DateTime.Now || a.EVPOPL_TODATE != null))  
                           && a.CompoundKey1.EVPOPL_KOD.NAZPOPL_POPLATEK == popl
                           && a.CompoundKey1.EVPOPL_PER.PERIODA_PERIODA == per
                       select a;

            expfin = string.Empty;
            if (list.Count() > 0)
            {
                //prvni radek 
                expfin = ((C_EVPOPL)(list.AsEnumerable().ElementAt(0))).EVPOPL_EXPFIN;
                procsankce = ((C_EVPOPL)(list.AsEnumerable().ElementAt(0))).EVPOPL_PROCSANKCE;
                procsankce = ((C_EVPOPL)(list.AsEnumerable().ElementAt(0))).EVPOPL_PROCSANKCE;
                typsankce = ((C_EVPOPL)(list.AsEnumerable().ElementAt(0))).EVPOPL_TYPSANKCE;
                pevnacastka = ((C_EVPOPL)(list.AsEnumerable().ElementAt(0))).EVPOPL_PEVNACASTKA;
                nasobek = ((C_EVPOPL)(list.AsEnumerable().ElementAt(0))).EVPOPL_NASOBEK;
                pernas = ((C_EVPOPL)(list.AsEnumerable().ElementAt(0))).EVPOPL_PERNAS;
                vystup = ((C_EVPOPL)(list.AsEnumerable().ElementAt(0))).EVPOPL_FORMTISK;
                porvs = Convert.ToInt32( ((C_EVPOPL)(list.AsEnumerable().ElementAt(0))).CompoundKey1.EVPOPL_KOD.NAZPOPL_PORVS );
            }

        }

        internal C_EVPOPL GetEvpopl(decimal poplatek, string perioda)
        {
          C_NAZPOPL nazp = sesna.GetObjectByKey<C_NAZPOPL>(poplatek);
          C_PERIODA per = sesna.GetObjectByKey<C_PERIODA>(perioda);
          C_EVPOPL.CompoundKey1Struct key = new C_EVPOPL.CompoundKey1Struct();
          key.EVPOPL_KOD = nazp;
          key.EVPOPL_PER = per;
          return sesna.GetObjectByKey<C_EVPOPL>(key);
        }

        internal DateTime GetDatumSplatnosti(DateTime vystaveno, C_EVPOPL evp, short porPer)
        {
          DateTime obdMez = vystaveno;
          
          int pocDni = (int)evp.EVPOPL_POCDNU;
          C_PERIODA per = null;
          DBValue dbv = null;

          switch (evp.EVPOPL_TERMPLAC.TERMPLAC_TERMPLAC)
          	{
            case "Z":  //zaèátek období
            case "K":  //zacatek nebo konec období
                       per = sesna.GetObjectByKey<C_PERIODA>(evp.CompoundKey1.EVPOPL_PER.PERIODA_NAZEV);
                       C_KALENDAR.CompoundKey1Struct key = new C_KALENDAR.CompoundKey1Struct();
                       key.KALENDAR_PERIODA.PERIODA_PERIODA = evp.CompoundKey1.EVPOPL_PER.PERIODA_NAZEV;
                       key.KALENDAR_PORPER = porPer;
                       obdMez = (sesna.GetObjectByKey<C_KALENDAR>(key)).KALENDAR_FROMDATE;
                    break;
            case "P":  //po vystavení poplatku
                      dbv = DBValue.Instance(sesna); //new DBValue(sesna);
                      obdMez = dbv.DBSysDate; 
                    break;
            case "R":  //zaèátek akt. roku
                      dbv = DBValue.Instance(sesna); //new DBValue(sesna);
                      obdMez = new DateTime(dbv.DBSysDate.Year, 1, 1); 
                    break;
            case "E":  //konec akt. roku
                      dbv = DBValue.Instance(sesna); //new DBValue(sesna);
                      obdMez = new DateTime(dbv.DBSysDate.Year, 12, 31); 
                    break;
	          }

          return obdMez.AddDays(pocDni);
        }

      internal string GetRecordFromTypDane(short typDane)
      {
        P_C_TYPDANE _typDane = sesna.GetObjectByKey<P_C_TYPDANE>(typDane);
        if (_typDane == null)
        { return "-1"; }
        else 
          { return _typDane.TYPDANE_REC; }
      }

      internal string GetVS(decimal poplatek, decimal porvs)
      {
        C_NAZPOPL nazp = sesna.GetObjectByKey<C_NAZPOPL>(poplatek);

        string formatVS = nazp.NAZPOPL_FORMATVS;
        string k = string.Empty;
        string r = string.Empty;
        string c = string.Empty;
        string prefix = string.Empty;

        string popl = ((int)poplatek).ToString();
        string rok = DateTime.Now.ToString("yyyy");
        string por = ((int)porvs).ToString();
        foreach (var item in formatVS)
        {
          switch (item.ToString().ToLower()[0])
          {
            case 'k':
              k += 'k';
              break;
            case 'r':
              r += 'r';
              break;
            case 'c':
              c += 'c';
              break;
            default:
              prefix += item.ToString().ToLower();
              break;
          }
        }

        string vs = prefix;

        if (k.Length > 0) 
           vs += popl.PadLeft(k.Length, '0');
        vs += rok.Substring(rok.Length - r.Length, r.Length);
        vs += por.PadLeft(c.Length, '0');

        return vs;
      }

      internal bool PredpisExport(int popl, string per, int ucetMes, int ucetRok, int idPredpisu)
      {
          try
          {
              P_NASTAVENI nast = sesna.GetObjectByKey<P_NASTAVENI>("VAZBA_UCTO_TYP");
              bool procDB = false;
              if (nast != null)
                 procDB = (Convert.ToInt16(nast.HODNOTA) == 1);

              if (! procDB)
                  return false;

              DBValue dbv = DBValue.Instance(sesna);
                string user = dbv.DBUserName.ToUpper();

                P_POPL_USERS pUser = sesna.FindObject<P_POPL_USERS>(
                                          CriteriaOperator.Parse("CompoundKey1.LOGIN = ? " +
                                                                 "and CompoundKey1.HODNOTA_NAZEV = ? " +
                                                                 "and CompoundKey1.SEKCE is null",
                                                      dbv.DBUserName.ToUpper(), "Expfin_predp_ihned"));
#region logovani
                      if  (pUser != null)
                          Util.Util.WriteLog(string.Format("nastaveni pro export do ucta: LOGIN={0}, Expfin_predp_ihned={1}, " +
                                                            "popl={2}, per={3}, ucetMes={4}, ucetRok={5}, predpisID={6}", 
                                                            dbv.DBUserName.ToUpper(), pUser.HODNOTA,
                                                            popl, per, ucetMes, ucetRok, idPredpisu));
                      else
                          Util.Util.WriteLog(string.Format("nastaveni pro export do ucta: LOGIN={0}, nebylo dohledano", dbv.DBUserName.ToUpper()));
#endregion logovani

              if ((pUser != null) && (Convert.ToInt32(pUser.HODNOTA) > 0))
              {
                  DevExpress.Xpo.DB.SelectedData selData = sesna.ExecuteSproc("GEO_EXP_PRPL_OX",
                                            new OperandValue(popl),
                                            new OperandValue(per),
                                            new OperandValue(ucetMes),
                                            new OperandValue(ucetRok),
                                            new OperandValue(idPredpisu));
                  return true;
              }
              else
                  return false;

          }
    	    catch (Exception exc)
	      {
            return false;
            throw new Exception("Export předpisu do účetnictví neproběhl z důvodu chyby. \n\n" + exc.Message);
                 
	      }                                 
      }

      internal string getPoplzPerKod(string periodaKod, string poplKO)
      {
          P_ODPADY_KONFIG konf = sesna.FindObject<P_ODPADY_KONFIG>(CriteriaOperator.Parse("KONF_POPLATEK = ?", poplKO));
          string[] stringSeparator = new string[] {"~~"};
          string[] perPopl = konf.KONF_PERPOPL.Split(stringSeparator, StringSplitOptions.RemoveEmptyEntries);
          Dictionary<string, string> param = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
          foreach (var item in perPopl)
          {
              if (item.Length > 0)
              {
                  string[] val = item.Split('~');
                  param.Add(val[0], val[1]);  
              }
          }
          string popl;
          param.TryGetValue(periodaKod, out popl);
          return popl;
      }
    }

    /// <summary>
    /// pristup prava prihlaseneho uzivatele nad poplatkem
    /// </summary>
    public class PravoNadPoplatkem
    {
      private string _user;
      private XPCollection<P_PRISTUP_TABLE> prtab;
      internal enum SQLPerm { SELECT, INSERT, UPDATE, DELETE };
      internal enum PrtabTable { RGP, PRPL, PLATBA }
      internal PravoNadPoplatkem(Session sesna)
      {
        DBValue dbv = DBValue.Instance(sesna);
        _user = dbv.DBUserName.ToUpper();
                prtab = new XPCollection<P_PRISTUP_TABLE>(
                                    sesna,
                                    CriteriaOperator.Parse("pk.PRTAB_LOGIN = ?", _user));
      }

      private bool _pravoExist(P_PRISTUP_TABLE item, PrtabTable table, SQLPerm pravo)
      {
        switch (pravo)
        {
          case SQLPerm.SELECT:
            switch (table)
            {
              case PrtabTable.RGP:
                return item.PRTAB_RGP != "N";
                break;
              case PrtabTable.PRPL:
                return item.PRTAB_PRPL != "N";
                break;
              case PrtabTable.PLATBA:
                return item.PRTAB_PLATBA != "N";
                break;
              default:
                return false;
                break;
            }
            break;
          case SQLPerm.INSERT:
            switch (table)
            {
              case PrtabTable.RGP:
                return item.PRTAB_RGP == "A";
                break;
              case PrtabTable.PRPL:
                return item.PRTAB_PRPL == "A";
                break;
              case PrtabTable.PLATBA:
                return item.PRTAB_PLATBA == "A";
                break;
              default:
                return false;
                break;
            }
            break;
          
          case SQLPerm.UPDATE:
            return false;
            break;
                 
          case SQLPerm.DELETE:
            return false;
            break;

          default:
            return false;
            break;
        }
      }


      internal bool PravoExist(int poplatek, PrtabTable table, SQLPerm pravo)
      {
          if (prtab == null)
          {
              Util.Util.WriteLog(string.Format("PravoExist return false: prtab == null, poplatek: {0}, table: {1}, pravo: {2}, user: {3}",
                                                   poplatek, table, pravo, _user));
              return false;
          }

        List<int> poplList = new List<int>();
        bool result = false;

        foreach (P_PRISTUP_TABLE item in prtab)
        {
          if (item.pk.PRTAB_POPLATEK == "*")
          {
              return _pravoExist(item, table, pravo);
          }

          string[] prtabPopl = item.pk.PRTAB_POPLATEK.Split(',');
          string pops;
          foreach (string ps in prtabPopl)
          {
            pops = ps.Trim();
            if (pops.IndexOf('-') > 0)
            {
              int minP = Convert.ToInt32(pops.Substring(0, pops.IndexOf('-')));
              int maxP = Convert.ToInt32(pops.Substring(pops.IndexOf('-') + 1, pops.Length - pops.IndexOf('-') - 1));
              for (int i = minP; i <= maxP; i++)
              {
                poplList.Add(i);
              }
            }
            else
            {
                int popl = -1;
                if (int.TryParse(pops, out popl))  //0.28  (int)pops
                    poplList.Add(popl);
                else
                    throw new Exception(string.Format("Chybné přidělení práva pro poplatek, či skupinu \"{0}\". ", pops));
            }
          }


          if (poplList.IndexOf(poplatek) >= 0)
              result = (result || _pravoExist(item, table, pravo));
        }

         if (! result)
             Util.Util.WriteLog(string.Format("PravoExist return false: poplatek: {0}, table: {1}, pravo: {2}, poplList: {3}, user: {4}",
                       poplatek, table, pravo, String.Join(",", poplList), _user));

if (System.Diagnostics.Debugger.IsAttached)
{ 
                 Util.Util.WriteLog(string.Format("PravoExist return {5}: poplatek: {0}, table: {1}, pravo: {2}, poplList: {3}, user: {4}",
                       poplatek, table, pravo, String.Join(",", poplList), _user, result));
}

        return result;
      }

    }

    /// <summary>
    /// ulozi exportovana / importovana data
    /// </summary>
    public class UtilDataKO
    {
        internal static void ulozDavkuDoDB(Session sesna, int EXT_APP_KOD, ref PLATCI_RESP resp)
        {
            try
            {
                DBUtil dbu = new DBUtil(sesna);
                MyUnitOfWork uow = new MyUnitOfWork(sesna.DataLayer);
                foreach (PLATCE2 item in resp.PLATCI)
                {

                    P_ODPADY_EULEVY pul = new P_ODPADY_EULEVY(uow);   
                    pul.EUL_ID = dbu.LIZNI_SEQ("P_ODPADY_EULEVY_ID");
                    pul.DAVKA = resp.DAVKA_ID;
                    pul.EA = EXT_APP_KOD;
                    pul.POPL = resp.RGP_POPLATEK;
                    pul.PER = resp.RGP_PER;
                    pul.KC_ZAPER = item.POPLATNIK.RGP_KCROK;
                    pul.VS = item.POPLATNIK.VS;
                    pul.ZPRAC = "E";
                    uow.CommitTransaction();
                    uow.Dispose();
                    uow = new MyUnitOfWork(sesna.DataLayer);
                }
            }
            catch (Exception e)
            {
                resp.result = Result.ERROR;
                resp.status = Status.ERROR;
                resp.ERRORMESS = "DejPlatce2 - Chyba při ukládání dávky" + e.Message;
            }
        }

        internal static void smazStarouDavku(Session sesna, int extApp, string perioda, out bool dataSmazana, ref PLATCI_RESP resp)
        {
            dataSmazana = false;
            try
            {

                DBVal dbval = new DBVal(sesna);
                sesna.ExecuteNonQuery("delete from P_ODPADY_EULEVY where ZPRAC = 'A' and EA = " + extApp.ToString() + 
                                           " and ENTRYDATE < " + dbval.dbv.getCurrentDateTimeText + "-750");
                sesna.ExecuteNonQuery("delete from P_ODPADY_EULEVY where EA = " + extApp.ToString() + " and ZPRAC = 'E'" + 
                             " and PER = '" + perioda + "'"  );
                dataSmazana = true;
            }
            catch (Exception e)
            {
                resp.result = Result.ERROR;
                resp.status = Status.ERROR;
                resp.ERRORMESS = string.Format("Odmazání starých dat neproběhlo. \n {0}", e.Message);
            }
        }

        internal static void ulozSlevyDoDB(Session sesna, int EXT_APP_KOD, POPLATNIK_SLEVA sleva, ref POPLATNIK_SLEVA_RESP resp)
        {
            try
            {
                DBUtil dbu = new DBUtil(sesna);
                MyUnitOfWork uow = new MyUnitOfWork(sesna.DataLayer);
                P_ODPADY_EULEVY pul = null; 
                foreach (SLEVA item in sleva.SLEVA)
                {
                    pul = new P_ODPADY_EULEVY(uow);
                    pul.EUL_ID = dbu.LIZNI_SEQ("P_ODPADY_EULEVY_ID");
                    pul.DAVKA = sleva.DAVKA_ID;
                    pul.EA = EXT_APP_KOD;
                    pul.POPL = sleva.POPLATEK;
                    pul.PER = sleva.PERIODA;
                    pul.ROK = sleva.ROK;
                    pul.KC_ZAPER = item.SLEVA_KC;
                    pul.VS = item.VS;
                    pul.ZPRAC = "I";
                    pul.Save();
                }
                uow.CommitTransaction();
                uow.Dispose();
                resp.ZPRACOVANO = sleva.SLEVA.Count();
            }
            catch (Exception e)
            {
                resp.result = Result.ERROR;
                resp.status = Status.ERROR;
                resp.ERRORMESS = "PlatceZaKOSleva - Chyba při ukládání dávky" + e.Message;
            }
        }  //end ulozSlevyDoDB

        internal static bool smazStarouDavkuSlev(Session sesna, int extApp, string davka, string perioda, ref POPLATNIK_SLEVA_RESP resp)
        {
            try
            {

                sesna.ExecuteNonQuery("delete from P_ODPADY_EULEVY where ZPRAC = 'I' and EA = " + extApp.ToString() +
                                           " and DAVKA = '" + davka + "'");

                sesna.ExecuteNonQuery("delete from P_ODPADY_EULEVY where ZPRAC = 'I' and EA = " + extApp.ToString() +
                                           " and PER = '" + perioda + "'");

                return true;
            }
            catch (Exception e)
            {
                resp.ERRORMESS = string.Format("Chyba odmazání dávky {0}. \n", davka, e.Message);
                resp.result = Result.ERROR;
                resp.status = Status.ERROR;
                return false;

            }
        }


    }

}
