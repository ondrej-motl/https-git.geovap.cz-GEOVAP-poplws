using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;

namespace PoplWS
{
  [Indices(@"CompoundKey1")]
  public partial class P_LIKEVS : XPLiteObject
  {
   
    decimal fLIKEVS_PORVS;
    public decimal LIKEVS_PORVS
    {
      get { return fLIKEVS_PORVS; }
      set { SetPropertyValue<decimal>("LIKEVS_PORVS", ref fLIKEVS_PORVS, value); }
    }
    
    string fLIKEVS_VS;
    [Indexed(Name = @"LIKEVS_DUPLICITNI_VS", Unique = true)]
    [Size(10)]
    public string LIKEVS_VS
    {
      get { return fLIKEVS_VS; }
      set { SetPropertyValue<string>("LIKEVS_VS", ref fLIKEVS_VS, value); }
    }

    decimal fLIKEVS_POHL;
    public decimal LIKEVS_POHL
    {
        get { return fLIKEVS_POHL; }
        set { SetPropertyValue<decimal>("LIKEVS_POHL", ref fLIKEVS_POHL, value); }
    }

    public struct CompoundKey1Struct
    {
      [Persistent("LIKEVS_POPLATEK")]
      public decimal LIKEVS_POPLATEK { get; set; }
      [Size(15)]
      [Persistent("LIKEVS_ICO")]
      public string LIKEVS_ICO { get; set; }
      [Size(25)]
      [Persistent("LIKEVS_DOPLKOD")]
      public string LIKEVS_DOPLKOD { get; set; }
    }
    [Indexed(@"LIKEVS_PORVS", Name = @"LIKEVS_DUPLICITNI_PORVS", Unique = true)]
    [Key, Persistent]
    public CompoundKey1Struct CompoundKey1;
    public P_LIKEVS(Session session) : base(session) { }
  }
}