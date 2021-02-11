using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;

namespace PoplWS
{
  public partial class P_PAROVANI : XPLiteObject
  {
    string fPAR_VYTVRUCNE;
    [Size(3)]
    public string PAR_VYTVRUCNE
    {
      get { return fPAR_VYTVRUCNE; }
      set { SetPropertyValue<string>("PAR_VYTVRUCNE", ref fPAR_VYTVRUCNE, value); }
    }
    decimal fPAR_SPARKC;
    public decimal PAR_SPARKC
    {
      get { return fPAR_SPARKC; }
      set { SetPropertyValue<decimal>("PAR_SPARKC", ref fPAR_SPARKC, value); }
    }

    string fPAR_DATE;
    [Size(20)]
    public string PAR_DATE
    {
      get { return fPAR_DATE; }
      set { SetPropertyValue<string>("PAR_DATE", ref fPAR_DATE, value); }
    }
    public struct CompoundKey1Struct
    {
      [Persistent("PAR_PRPL_ID")]
      public int PAR_PRPL_ID { get; set; }
      [Persistent("PAR_PLATBA_ID")]
      public int PAR_PLATBA_ID { get; set; }
    }
    [Indexed(Name = @"PAR_PK", Unique = true)]
    [Key, Persistent]
    public CompoundKey1Struct CompoundKey1;

    public P_PAROVANI(Session session) : base(session) { }
  }

}