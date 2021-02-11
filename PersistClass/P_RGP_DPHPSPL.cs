using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;

namespace PoplWS
{
  public partial class P_RGP_DPHPSPL : XPLiteObject
  {

    [Persistent("RDPHP_ID")]

    [Key(true)]
    private int _rdphp_id;
    [PersistentAlias("rdphp_id")]
    public int RDPHP_ID { get { return this._rdphp_id; } }


    decimal fRDPHP_ZAKLAD;
    public decimal RDPHP_ZAKLAD
    {
      get { return fRDPHP_ZAKLAD; }
      set { SetPropertyValue<decimal>("RDPHP_ZAKLAD", ref fRDPHP_ZAKLAD, value); }
    }
    decimal? fRDPHP_SAZBA;
    public decimal? RDPHP_SAZBA
    {
      get { return fRDPHP_SAZBA; }
      set { SetPropertyValue<decimal?>("RDPHP_SAZBA", ref fRDPHP_SAZBA, value); }
    }
    decimal fRDPHP_DAN;
    public decimal RDPHP_DAN
    {
      get { return fRDPHP_DAN; }
      set { SetPropertyValue<decimal>("RDPHP_DAN", ref fRDPHP_DAN, value); }
    }
    decimal fRDPHP_KC;
    public decimal RDPHP_KC
    {
      get { return fRDPHP_KC; }
      set { SetPropertyValue<decimal>("RDPHP_KC", ref fRDPHP_KC, value); }
    }
    char fRDPHP_KAT;
    public char RDPHP_KAT
    {
      get { return fRDPHP_KAT; }
      set { SetPropertyValue<char>("RDPHP_KAT", ref fRDPHP_KAT, value); }
    }
    decimal fRDPHP_ZAOKR;
    public decimal RDPHP_ZAOKR
    {
      get { return fRDPHP_ZAOKR; }
      set { SetPropertyValue<decimal>("RDPHP_ZAOKR", ref fRDPHP_ZAOKR, value); }
    }
    string fRDPHP_POZNAMKA;
    [Size(80)]
    public string RDPHP_POZNAMKA
    {
      get { return fRDPHP_POZNAMKA; }
      set { SetPropertyValue<string>("RDPHP_POZNAMKA", ref fRDPHP_POZNAMKA, value); }
    }
    DateTime fENTRYDATE;
    public DateTime ENTRYDATE
    {
      get { return fENTRYDATE; }
    }
    string fENTRYLOGIN;
    [Size(35)]
    public string ENTRYLOGIN
    {
      get { return fENTRYLOGIN; }
    }
    DateTime fLASTUPDATE;
    public DateTime LASTUPDATE
    {
      get { return fLASTUPDATE; }
    }
    string fLASTLOGIN;
    [Size(35)]
    public string LASTLOGIN
    {
      get { return fLASTLOGIN; }
    }

    P_RGP frgp;
    [Persistent("RDPHP_RGP_ID")]
    [Association(@"DPHPSPLReferencesRGP")]
    public P_RGP P_RGP
    {
        get { return frgp; }
        set { SetPropertyValue("P_RGP", ref frgp, value); }
    }
      
      public P_RGP_DPHPSPL(Session session) : base(session) { }
  }

}