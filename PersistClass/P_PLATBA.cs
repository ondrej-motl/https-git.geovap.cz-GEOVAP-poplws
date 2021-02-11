using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;

namespace PoplWS
{
  [Indices(@"PLATBA_POPLATEK;PLATBA_ICO;PLATBA_DOPLKOD")]
  public partial class P_PLATBA : XPLiteObject
  {
    int fPLATBA_ID;
    [Indexed(@"PLATBA_VS", Name = @"PLATBA_i1")]
    [Key]
    public int PLATBA_ID
    {
      get { return fPLATBA_ID; }
      set { SetPropertyValue<int>("PLATBA_ID", ref fPLATBA_ID, value); }
    }
    string fPLATBA_VS;
    [Size(18)]
    public string PLATBA_VS
    {
      get { return fPLATBA_VS; }
      set { SetPropertyValue<string>("PLATBA_VS", ref fPLATBA_VS, value); }
    }
    int fPLATBA_PORADI;
    public int PLATBA_PORADI
    {
      get { return fPLATBA_PORADI; }
      set { SetPropertyValue<int>("PLATBA_PORADI", ref fPLATBA_PORADI, value); }
    }
    DateTime fPLATBA_PLDATE;
    public DateTime PLATBA_PLDATE
    {
      get { return fPLATBA_PLDATE; }
      set { SetPropertyValue<DateTime>("PLATBA_PLDATE", ref fPLATBA_PLDATE, value); }
    }
    decimal fPLATBA_PLKC;
    public decimal PLATBA_PLKC
    {
      get { return fPLATBA_PLKC; }
      set { SetPropertyValue<decimal>("PLATBA_PLKC", ref fPLATBA_PLKC, value); }
    }
    DateTime fPLATBA_NAUCETDNE;
    [Indexed(Name = @"p_platba_ix4")]
    public DateTime PLATBA_NAUCETDNE
    {
      get { return fPLATBA_NAUCETDNE; }
      set { SetPropertyValue<DateTime>("PLATBA_NAUCETDNE", ref fPLATBA_NAUCETDNE, value); }
    }
    string fPLATBA_BANKSPOJ;
    [Size(30)]
    public string PLATBA_BANKSPOJ
    {
      get { return fPLATBA_BANKSPOJ; }
      set { SetPropertyValue<string>("PLATBA_BANKSPOJ", ref fPLATBA_BANKSPOJ, value); }
    }
    string fPLATBA_BANKU;
    [Size(30)]
    public string PLATBA_BANKU
    {
      get { return fPLATBA_BANKU; }
      set { SetPropertyValue<string>("PLATBA_BANKU", ref fPLATBA_BANKU, value); }
    }
    string fPLATBA_PLATCE;
    [Size(30)]
    public string PLATBA_PLATCE
    {
      get { return fPLATBA_PLATCE; }
      set { SetPropertyValue<string>("PLATBA_PLATCE", ref fPLATBA_PLATCE, value); }
    }
    string fPLATBA_DOKLAD;
    [Size(20)]
    public string PLATBA_DOKLAD
    {
      get { return fPLATBA_DOKLAD; }
      set { SetPropertyValue<string>("PLATBA_DOKLAD", ref fPLATBA_DOKLAD, value); }
    }
    decimal fPLATBA_UCETMESIC;
    public decimal PLATBA_UCETMESIC
    {
      get { return fPLATBA_UCETMESIC; }
      set { SetPropertyValue<decimal>("PLATBA_UCETMESIC", ref fPLATBA_UCETMESIC, value); }
    }
    decimal fPLATBA_UCETROK;
    [Indexed(@"PLATBA_UCETMESIC", Name = @"P_PLATBA_ix5")]
    public decimal PLATBA_UCETROK
    {
      get { return fPLATBA_UCETROK; }
      set { SetPropertyValue<decimal>("PLATBA_UCETROK", ref fPLATBA_UCETROK, value); }
    }
    decimal? fPLATBA_KODROZUCT;
    public decimal? PLATBA_KODROZUCT
    {
      get { return fPLATBA_KODROZUCT; }
      set { SetPropertyValue<decimal?>("PLATBA_KODROZUCT", ref fPLATBA_KODROZUCT, value); }
    }
    decimal? fPLATBA_DAVKA;
    public decimal? PLATBA_DAVKA
    {
      get { return fPLATBA_DAVKA; }
      set { SetPropertyValue<decimal?>("PLATBA_DAVKA", ref fPLATBA_DAVKA, value); }
    }
    string fPLATBA_EXPFIN;
    [Size(3)]
    public string PLATBA_EXPFIN
    {
      get { return fPLATBA_EXPFIN; }
      set { SetPropertyValue<string>("PLATBA_EXPFIN", ref fPLATBA_EXPFIN, value); }
    }
    string fPLATBA_EXPORTOVANO;
    [Size(3)]
    public string PLATBA_EXPORTOVANO
    {
      get { return fPLATBA_EXPORTOVANO; }
      set { SetPropertyValue<string>("PLATBA_EXPORTOVANO", ref fPLATBA_EXPORTOVANO, value); }
    }
    string fPLATBA_RECORD;
    [Indexed(@"PLATBA_VS;PLATBA_PLDATE;PLATBA_NAUCETDNE", Name = @"P_PLATBA_ix2")]
    [Size(1)]
    public string PLATBA_RECORD
    {
      get { return fPLATBA_RECORD; }
      set { SetPropertyValue<string>("PLATBA_RECORD", ref fPLATBA_RECORD, value); }
    }
    decimal fPLATBA_SPAROVANO;
    public decimal PLATBA_SPAROVANO
    {
      get { return fPLATBA_SPAROVANO; }
      set { SetPropertyValue<decimal>("PLATBA_SPAROVANO", ref fPLATBA_SPAROVANO, value); }
    }
    decimal fPLATBA_SS;
    public decimal PLATBA_SS
    {
      get { return fPLATBA_SS; }
      set { SetPropertyValue<decimal>("PLATBA_SS", ref fPLATBA_SS, value); }
    }
    string fLOGIN;
    [Size(30)]
    public string LOGIN
    {
      get { return fLOGIN; }
      set { SetPropertyValue<string>("LOGIN", ref fLOGIN, value); }
    }
    DateTime fLASTUPDATE;
    public DateTime LASTUPDATE
    {
      get { return fLASTUPDATE; }
      set { SetPropertyValue<DateTime>("LASTUPDATE", ref fLASTUPDATE, value); }
    }
    double fPLATBA_SPAROVANO_MINUSEM;
    public double PLATBA_SPAROVANO_MINUSEM
    {
      get { return fPLATBA_SPAROVANO_MINUSEM; }
      set { SetPropertyValue<double>("PLATBA_SPAROVANO_MINUSEM", ref fPLATBA_SPAROVANO_MINUSEM, value); }
    }
    char fPLATBA_TYP;
    public char PLATBA_TYP
    {
      get { return fPLATBA_TYP; }
      set { SetPropertyValue<char>("PLATBA_TYP", ref fPLATBA_TYP, value); }
    }
    int? fPLATBA_POKLDOK;
    public int? PLATBA_POKLDOK
    {
      get { return fPLATBA_POKLDOK; }
      set { SetPropertyValue<int?>("PLATBA_POKLDOK", ref fPLATBA_POKLDOK, value); }
    }
    decimal? fPLATBA_OLDVS;
    public decimal? PLATBA_OLDVS
    {
      get { return fPLATBA_OLDVS; }
      set { SetPropertyValue<decimal?>("PLATBA_OLDVS", ref fPLATBA_OLDVS, value); }
    }
    decimal fPLATBA_POPLATEK;
    [Indexed(Name = @"P_PLATBA_ix7")]
    public decimal PLATBA_POPLATEK
    {
      get { return fPLATBA_POPLATEK; }
      set { SetPropertyValue<decimal>("PLATBA_POPLATEK", ref fPLATBA_POPLATEK, value); }
    }
    string fPLATBA_PER;
    [Size(1)]
    public string PLATBA_PER
    {
      get { return fPLATBA_PER; }
      set { SetPropertyValue<string>("PLATBA_PER", ref fPLATBA_PER, value); }
    }
    string fPLATBA_ICO;
    [Size(15)]
    public string PLATBA_ICO
    {
      get { return fPLATBA_ICO; }
      set { SetPropertyValue<string>("PLATBA_ICO", ref fPLATBA_ICO, value); }
    }
    string fPLATBA_DOPLKOD;
    [Size(25)]
    public string PLATBA_DOPLKOD
    {
      get { return fPLATBA_DOPLKOD; }
      set { SetPropertyValue<string>("PLATBA_DOPLKOD", ref fPLATBA_DOPLKOD, value); }
    }
    int fPLATBA_PR;
    public int PLATBA_PR
    {
      get { return fPLATBA_PR; }
      set { SetPropertyValue<int>("PLATBA_PR", ref fPLATBA_PR, value); }
    }
    /*short fPLATBA_SALDO;
    public short PLATBA_SALDO
    {
      get { return fPLATBA_SALDO; }
      set { SetPropertyValue<short>("PLATBA_SALDO", ref fPLATBA_SALDO, value); }
    }
     */ 
    string fPLATBA_INTOZN;
    [Size(10)]
    public string PLATBA_INTOZN
    {
      get { return fPLATBA_INTOZN; }
      set { SetPropertyValue<string>("PLATBA_INTOZN", ref fPLATBA_INTOZN, value); }
    }
    string fPLATBA_POZNAMKA;
    [Size(50)]
    public string PLATBA_POZNAMKA
    {
      get { return fPLATBA_POZNAMKA; }
      set { SetPropertyValue<string>("PLATBA_POZNAMKA", ref fPLATBA_POZNAMKA, value); }
    }
    decimal fPLATBA_OBDOBIMES;
    public decimal PLATBA_OBDOBIMES
    {
      get { return fPLATBA_OBDOBIMES; }
      set { SetPropertyValue<decimal>("PLATBA_OBDOBIMES", ref fPLATBA_OBDOBIMES, value); }
    }
    decimal fPLATBA_OBDOBIROK;
    public decimal PLATBA_OBDOBIROK
    {
      get { return fPLATBA_OBDOBIROK; }
      set { SetPropertyValue<decimal>("PLATBA_OBDOBIROK", ref fPLATBA_OBDOBIROK, value); }
    }
    DateTime fENTRYDATE;
    public DateTime ENTRYDATE
    {
      get { return fENTRYDATE; }
      set { SetPropertyValue<DateTime>("ENTRYDATE", ref fENTRYDATE, value); }
    }
    string fENTRYLOGIN;
    [Size(30)]
    public string ENTRYLOGIN
    {
      get { return fENTRYLOGIN; }
      set { SetPropertyValue<string>("ENTRYLOGIN", ref fENTRYLOGIN, value); }
    }
    char fPLATBA_ROZDLEKO;
    public char PLATBA_ROZDLEKO
    {
      get { return fPLATBA_ROZDLEKO; }
      set { SetPropertyValue<char>("PLATBA_ROZDLEKO", ref fPLATBA_ROZDLEKO, value); }
    }

    int fPLATBA_EA;
    public int PLATBA_EA
    {
      get { return fPLATBA_EA; }
      set { SetPropertyValue<int>("PLATBA_EA", ref fPLATBA_EA, value); }
    }

    public P_PLATBA(Session session) : base(session) { }

    protected override void OnSaving()
    {
      base.OnSaving();
      
      PLATBA_OBDOBIMES = -1;
      PLATBA_OBDOBIROK = -1;
      PLATBA_POPLATEK = -1;
      PLATBA_ROZDLEKO = 'N';
      if (PLATBA_PORADI == 0) PLATBA_PORADI = PLATBA_ID;
     if ((PLATBA_EXPFIN == null) || (PLATBA_EXPFIN == "")) PLATBA_EXPFIN = "NE";
     if ((PLATBA_EXPORTOVANO == null) || (PLATBA_EXPORTOVANO == "")) PLATBA_EXPORTOVANO = "NE";
    }

  }

}