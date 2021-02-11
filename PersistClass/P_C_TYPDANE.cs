using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;

namespace PoplWS
{
  public partial class P_C_TYPDANE : XPLiteObject
  {
    short fTYPDANE_KOD;
    [Key]
    public short TYPDANE_KOD
    {
      get { return fTYPDANE_KOD; }
      set { SetPropertyValue<short>("TYPDANE_KOD", ref fTYPDANE_KOD, value); }
    }
    string fTYPDANE_NAZEV;
    [Size(35)]
    public string TYPDANE_NAZEV
    {
      get { return fTYPDANE_NAZEV; }
      set { SetPropertyValue<string>("TYPDANE_NAZEV", ref fTYPDANE_NAZEV, value); }
    }
    decimal fTYPDANE_PARPOR;
    public decimal TYPDANE_PARPOR
    {
      get { return fTYPDANE_PARPOR; }
      set { SetPropertyValue<decimal>("TYPDANE_PARPOR", ref fTYPDANE_PARPOR, value); }
    }
    string fTYPDANE_REC;
    [Size(1)]
    public string TYPDANE_REC
    {
      get { return fTYPDANE_REC; }
      set { SetPropertyValue<string>("TYPDANE_REC", ref fTYPDANE_REC, value); }
    }
    string fTYPDANE_NABIDKAREC;
    [Size(35)]
    public string TYPDANE_NABIDKAREC
    {
      get { return fTYPDANE_NABIDKAREC; }
      set { SetPropertyValue<string>("TYPDANE_NABIDKAREC", ref fTYPDANE_NABIDKAREC, value); }
    }
    string fTYPDANE_NAZEVREC;
    [Size(35)]
    public string TYPDANE_NAZEVREC
    {
      get { return fTYPDANE_NAZEVREC; }
      set { SetPropertyValue<string>("TYPDANE_NAZEVREC", ref fTYPDANE_NAZEVREC, value); }
    }
    [Association(@"P_PRPLReferencesP_C_TYPDANE", typeof(P_PRPL))]
    public XPCollection<P_PRPL> P_PRPL { get { return GetCollection<P_PRPL>("P_PRPL"); } }
    public P_C_TYPDANE(Session session) : base(session) { }
  }
}