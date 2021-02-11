using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;

namespace PoplWS
{
  public partial class P_PRPL_DPH : XPLiteObject
  {
    int fDPH_ID;
    [Key(true)]
    public int DPH_ID
    {
      get { return fDPH_ID; }
      set { SetPropertyValue<int>("DPH_ID", ref fDPH_ID, value); }
    }


    P_PRPL fDPH_PRPL_ID;
    [Indexed(Name = @"P_PRPL_DPH_ix1")]
    [Association(@"PRPL_DPHreferencesPRPL")]
    public P_PRPL DPH_PRPL_ID
    {
      get { return fDPH_PRPL_ID; }
      set { SetPropertyValue<P_PRPL>("DPH_PRPL_ID", ref fDPH_PRPL_ID, value); }
    }

    decimal? fDPH_SAZBA;
    public decimal? DPH_SAZBA
    {
      get { return fDPH_SAZBA; }
      set { SetPropertyValue<decimal?>("DPH_SAZBA", ref fDPH_SAZBA, value); }
    }
    decimal fDPH_ZAKLAD;
    public decimal DPH_ZAKLAD
    {
      get { return fDPH_ZAKLAD; }
      set { SetPropertyValue<decimal>("DPH_ZAKLAD", ref fDPH_ZAKLAD, value); }
    }
    decimal fDPH_DAN;
    public decimal DPH_DAN
    {
      get { return fDPH_DAN; }
      set { SetPropertyValue<decimal>("DPH_DAN", ref fDPH_DAN, value); }
    }
    decimal fDPH_KC;
    public decimal DPH_KC
    {
      get { return fDPH_KC; }
      set { SetPropertyValue<decimal>("DPH_KC", ref fDPH_KC, value); }
    }
    string fDPH_POZNAMKA;
    [Size(80)]
    public string DPH_POZNAMKA
    {
      get { return fDPH_POZNAMKA; }
      set { SetPropertyValue<string>("DPH_POZNAMKA", ref fDPH_POZNAMKA, value); }
    }
    char fDPH_KAT;
    public char DPH_KAT
    {
      get { return fDPH_KAT; }
      set { SetPropertyValue<char>("DPH_KAT", ref fDPH_KAT, value); }
    }
    decimal fDPH_ZAOKR;
    public decimal DPH_ZAOKR
    {
      get { return fDPH_ZAOKR; }
      set { SetPropertyValue<decimal>("DPH_ZAOKR", ref fDPH_ZAOKR, value); }
    }
    DateTime fENTRYDATE;
    [IgnoreInsertUpdate]
    public DateTime ENTRYDATE
    {
      get { return fENTRYDATE; }
      set { SetPropertyValue<DateTime>("ENTRYDATE", ref fENTRYDATE, value); }
    }
    string fENTRYLOGIN;
    [Size(35)]
    [IgnoreInsertUpdate]
    public string ENTRYLOGIN
    {
      get { return fENTRYLOGIN; }
      set { SetPropertyValue<string>("ENTRYLOGIN", ref fENTRYLOGIN, value); }
    }
    DateTime fLASTUPDATE;
    public DateTime LASTUPDATE
    {
      get { return fLASTUPDATE; }
      set { SetPropertyValue<DateTime>("LASTUPDATE", ref fLASTUPDATE, value); }
    }
    string fLASTLOGIN;
    [Size(35)]
    public string LASTLOGIN
    {
      get { return fLASTLOGIN; }
      set { SetPropertyValue<string>("LASTLOGIN", ref fLASTLOGIN, value); }
    }
    public P_PRPL_DPH(Session session) : base(session) { }
  }
}