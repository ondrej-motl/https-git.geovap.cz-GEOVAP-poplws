using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;

namespace PoplWS
{
  /*======================================================================*/
  public partial class P_EXTAPP_POPL : XPLiteObject
  {
    public struct CompoundKey1Struct
    {
      /// <summary>
      /// ID externi aplikace
      /// </summary>
      [Persistent("ID")]
      public int ID { get; set; }
      [Persistent("POPLATEK")]
      public decimal POPLATEK { get; set; }
    }
  
    [Indexed(Name = @"P_EXTAPP_POPL_PK", Unique = true)]
    [Key, Persistent]
    public CompoundKey1Struct CompoundKey1;
    public P_EXTAPP_POPL(Session session) : base(session) { }
  }

  
  public partial class P_EXTAPP : XPLiteObject
  {
    int fID;
    [Key]
    [Size(32)]
    public int ID
    {
      get { return fID; }
      set { SetPropertyValue<int>("ID", ref fID, value); }
    }

    string fNAZEV;
    [Size(32)]
    public string NAZEV
    {
      get { return fNAZEV; }
      set { SetPropertyValue<string>("NAZEV", ref fNAZEV, value); }
    }


    public P_EXTAPP(Session session) : base(session) { }

  }


}