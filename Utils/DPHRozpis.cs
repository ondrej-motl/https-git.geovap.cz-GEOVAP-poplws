using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using PoplWS.PersistClass;


namespace PoplWS
{
  public class RADEK_DPH
  {
    public decimal ZAKLAD { get; set; }
    public decimal? SAZBA { get; set; }
    public string POZNAMKA { get; set; }
  }

  internal class DPHRozpis
  {
    [Persistent("RDPHS_ZAKLAD")]
    internal decimal ZAKLAD { get; set; }
    [Persistent("RDPHS_SAZBA")]
    internal decimal? SAZBA { get; set; }
    [Persistent("RDPHS_DAN")]
    internal decimal DAN { get; set; }
    [Persistent("RDPHS_KC")]
    internal decimal KC { get; set; }
    [Persistent("RDPHS_KAT")]
    internal char KAT { get; set; }
    [Persistent("RDPHS_ZAOKR")]
    internal decimal ZAOKROUHLENI { get; set; }
    [Persistent("RDPHS_POZNAMKA")]
    internal string POZNAMKA { get; set; }
  }

  internal class DPHRozpisTmp
  {
    [Persistent("RDPHS_ZAKLAD")]
    internal decimal RDPHP_ZAKLAD { get; set; }
    [Persistent("RDPHS_SAZBA")]
    internal decimal? RDPHP_SAZBA { get; set; }
    [Persistent("RDPHS_DAN")]
    internal decimal RDPHP_DAN { get; set; }
    [Persistent("RDPHS_KC")]
    internal decimal RDPHP_KC { get; set; }
    [Persistent("RDPHS_KAT")]
    internal char RDPHP_KAT { get; set; }
    [Persistent("RDPHS_ZAOKR")]
    internal decimal RDPHP_ZAOKR { get; set; }
    [Persistent("RDPHS_POZNAMKA")]
    internal string RDPHP_POZNAMKA { get; set; }
  }

  internal class DPHRozpisPredp
  {
    [Persistent("DPH_ZAKLAD")]
    internal decimal ZAKLAD { get; set; }
    [Persistent("DPH_SAZBA")]
    internal decimal? SAZBA { get; set; }
    [Persistent("DPH_DAN")]
    internal decimal DAN { get; set; }
    [Persistent("DPH_KC")]
    internal decimal KC { get; set; }
    [Persistent("DPH_KAT")]
    internal char KAT { get; set; }
    [Persistent("DPH_ZAOKR")]
    internal decimal ZAOKROUHLENI { get; set; }
    [Persistent("DPH_POZNAMKA")]
    internal string POZNAMKA { get; set; }
  }

  internal class DPHZaokrouhleni
  {
    private struct sazDane
    {
      internal decimal? sazba;
      internal decimal KC;
    }

    /// <summary>
    /// radky DPH, pripadne zaokrouhli a stanovi zaokrouleni
    /// napnlni vstupni List dopocitanymi hodnotami (kod sazby dane, dan, KC, zaokrouhleni, radek dorovanani na celkovou castku)
    /// </summary>

    internal void DPHZaokrouhli(List<DPHRozpis> dphRows, decimal poplatek, Session sesna, bool provestZaokrouhleni = true)
    {

      CriteriaOperator criteria;
      criteria = CriteriaOperator.Parse("NAZPOPL_POPLATEK = ?", poplatek);
      C_NAZPOPL cNazp = sesna.FindObject<C_NAZPOPL>(criteria);

      if (cNazp.NAZPOPL_DPH == 'N')
      { throw new Exception("Součástí poplatku není DPH"); }

      decimal suma = 0;
      List<sazDane> sazbySoucty = new List<sazDane>();
      foreach (DPHRozpis item in dphRows)
      {
        var sd = new sazDane();
        sd.sazba = item.SAZBA;
        sd.KC = item.ZAKLAD * (1 + (item.SAZBA ?? 0) / 100);

        int i = sazbySoucty.FindIndex(x => x.sazba == item.SAZBA);  //i toto nalezne null hodnotu
        if (i == -1)
        {
          sazbySoucty.Add(sd);
        }
        else
        {
          sd.KC = sazbySoucty[i].KC + (item.ZAKLAD * (1 + (item.SAZBA ?? 0) / 100));
          sazbySoucty[i] = sd;
        }

        suma += item.ZAKLAD * (1 + (item.SAZBA ?? 0) / 100);
      }

      if (sazbySoucty.Count == 0)
      { //neexistuji radky dane
        return;
      }

      //nactu platne sazby dane
      DBValue dbv = DBValue.Instance(sesna); //new DBValue(sesna);
      XPCollection<P_DPH_SAZBA> dphSazby = new XPCollection<P_DPH_SAZBA>(
                                      sesna,
                                      CriteriaOperator.Parse("CompoundKey1.DPHS_OD <= ? and DPHS_DO > ?",
                                          dbv.DBSysDate, dbv.DBSysDate.AddDays(-1)));


      //naleznu nejvyssi castku a tim i zaokrouhlovanou sazbu
      //zaroven zkontroluji platnost sazby DPH
      sazDane dphRowMaxCastka;
      dphRowMaxCastka.sazba = null;
      dphRowMaxCastka.KC = 0;

      foreach (sazDane item in sazbySoucty)
      {
        if (item.sazba == null)
        { dphSazby.Filter = (CriteriaOperator.Parse("DPHS_SAZBA is null")); }
        else
        { dphSazby.Filter = (CriteriaOperator.Parse("DPHS_SAZBA = ?", item.sazba)); }

        if (dphSazby.Count == 0)
        {
          throw new Exception(String.Format("sazba DPH \"{0}\" neni platná", item.sazba == null ? "bez daně" : item.sazba.ToString()));
        }
        if ((item.KC > dphRowMaxCastka.KC) && (item.sazba != 0) && (item.sazba.HasValue))
        {
          dphRowMaxCastka = item;
        }
      }

      //doplnim kategorii sazby, dan a celkovou castku
      int presnostZaokr = 0;
      foreach (DPHRozpis item in dphRows)
      {
        item.DAN = item.ZAKLAD * (item.SAZBA ?? 0) / 100;
        item.DAN = Utils.Mathm.ZaokrouhliDouble(item.DAN, 2);
        item.KC = item.ZAKLAD + item.DAN;
        item.ZAOKROUHLENI = 0;

        if (item.SAZBA == null)
        { dphSazby.Filter = (CriteriaOperator.Parse("DPHS_SAZBA is null")); }
        else
        { dphSazby.Filter = (CriteriaOperator.Parse("DPHS_SAZBA = ?", item.SAZBA)); }

        item.KAT = dphSazby[0].CompoundKey1.DPHS_KAT;

        if (dphRowMaxCastka.sazba == dphSazby[0].DPHS_SAZBA)
        {
          presnostZaokr = dphSazby[0].DPHS_ZAOKR;
        }
      }

      DPHRozpis zaokrRadek = dphRows.Find(x => x.SAZBA == dphRowMaxCastka.sazba);
      if (zaokrRadek == null)
      {//neexistuje radek s nenulovou sazbou DPH
        return;
      }

      //zda pocitat DPH ze zaokrouhleni
              P_NASTAVENI nast = sesna.GetObjectByKey<P_NASTAVENI>("DPH_ZEZAOKR");
              bool DphZeza;
              if (nast != null)
                  DphZeza = nast.HODNOTA == "1";
              else
                  DphZeza = true;

      //zaokrouhledni DPH
      if (provestZaokrouhleni && (cNazp.NAZPOPL_DPHROUND == 'A'))
      {
          // sazDane zaokrSazDane = lsd.Find(x => x.sazba == zaokrRadek.SAZBA);
        decimal sazba = dphRowMaxCastka.sazba ?? 1;
        decimal koef = sazba / 100m / (1 + (sazba / 100m));  //dle 235/2004 Sb. o DPH §37 odst 2.
        koef = Utils.Mathm.ZaokrouhliDouble(koef, 4);

        decimal dph = zaokrRadek.DAN;
        decimal zaokr = Utils.Mathm.ZaokrouhliDouble(suma, 0) - suma;
        zaokr = Utils.Mathm.ZaokrouhliDouble(zaokr, 2);

         if ( DphZeza )
         {
            decimal dph_zaokr = zaokr * koef;                //DPH ze zaokrouhleni
            dph = Utils.Mathm.ZaokrouhliDouble(dph + dph_zaokr, 2); //upravene DPH o zaokrouhleni
         }

        zaokrRadek.DAN = dph;
        zaokrRadek.ZAOKROUHLENI = zaokr;
      }
    }
  }
}
