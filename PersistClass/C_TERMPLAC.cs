using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;

namespace PoplWS
{
  public partial class C_TERMPLAC : XPLiteObject
  {
    string fTERMPLAC_TERMPLAC;
    [Key]
    [Size(5)]
    public string TERMPLAC_TERMPLAC
    {
      get { return fTERMPLAC_TERMPLAC; }
      set { SetPropertyValue<string>("TERMPLAC_TERMPLAC", ref fTERMPLAC_TERMPLAC, value); }
    }
    string fTERMPLAC_NAZEV;
    [Size(25)]
    public string TERMPLAC_NAZEV
    {
      get { return fTERMPLAC_NAZEV; }
      set { SetPropertyValue<string>("TERMPLAC_NAZEV", ref fTERMPLAC_NAZEV, value); }
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
    
    [Association(@"EVPOPLReferencesC_TERMPLAC", typeof(C_EVPOPL))]
    public XPCollection<C_EVPOPL> C_EVPOPL
    {
      get { return GetCollection<C_EVPOPL>("C_EVPOPL"); }
    }

    public C_TERMPLAC(Session session) : base(session) { }
  }
}