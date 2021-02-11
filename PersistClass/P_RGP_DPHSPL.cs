using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;

namespace PoplWS
{
  public partial class P_RGP_DPHSPL : XPLiteObject
  {
   /* int fRDPHS_ID;
    [Key(true)]
    public int RDPHS_ID
    {
      get { return fRDPHS_ID; }
      set { SetPropertyValue<int>("RDPHS_ID", ref fRDPHS_ID, value); }
    }
    */
    [Persistent("RDPHS_ID")]
    [IgnoreInsertUpdate]
    [Key(true)]
    private int _rdphs_id;
    [PersistentAlias("rdphs_id")]
    public int RDPHS_ID { get { return this._rdphs_id; } }


    decimal fRDPHS_ZAKLAD;
    public decimal RDPHS_ZAKLAD
    {
      get { return fRDPHS_ZAKLAD; }
      set { SetPropertyValue<decimal>("RDPHS_ZAKLAD", ref fRDPHS_ZAKLAD, value); }
    }
    decimal? fRDPHS_SAZBA;
    public decimal? RDPHS_SAZBA
    {
      get { return fRDPHS_SAZBA; }
      set { SetPropertyValue<decimal?>("RDPHS_SAZBA", ref fRDPHS_SAZBA, value); }
    }
    decimal fRDPHS_DAN;
    public decimal RDPHS_DAN
    {
      get { return fRDPHS_DAN; }
      set { SetPropertyValue<decimal>("RDPHS_DAN", ref fRDPHS_DAN, value); }
    }
    decimal fRDPHS_KC;
    public decimal RDPHS_KC
    {
      get { return fRDPHS_KC; }
      set { SetPropertyValue<decimal>("RDPHS_KC", ref fRDPHS_KC, value); }
    }
    char fRDPHS_KAT;
    public char RDPHS_KAT
    {
      get { return fRDPHS_KAT; }
      set { SetPropertyValue<char>("RDPHS_KAT", ref fRDPHS_KAT, value); }
    }
    decimal fRDPHS_ZAOKR;
    public decimal RDPHS_ZAOKR
    {
      get { return fRDPHS_ZAOKR; }
      set { SetPropertyValue<decimal>("RDPHS_ZAOKR", ref fRDPHS_ZAOKR, value); }
    }
    string fRDPHS_POZNAMKA;
    [Size(80)]
    public string RDPHS_POZNAMKA
    {
      get { return fRDPHS_POZNAMKA; }
      set { SetPropertyValue<string>("RDPHS_POZNAMKA", ref fRDPHS_POZNAMKA, value); }
    }

    private DateTime fENTRYDATE;
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
    [Persistent("RDPHS_RGP_ID")]
    [Association(@"DPHSPLReferencesRGP")]
    public P_RGP P_RGP
    {
        get { return frgp; }
        set { SetPropertyValue("P_RGP", ref frgp, value); }
    }

    public P_RGP_DPHSPL(Session session) : base(session) { }
  }
}