using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;

namespace PoplWS
{
  public class P_PRISTUP_TABLE : XPLiteObject
  {
    //public string PRTAB_LOGIN {get; set;}
    //public string PRTAB_POPLATEK {get; set;}
    [Size(1)]
    private string fPRTAB_RGP;
    public string PRTAB_RGP 
     {
       get { return fPRTAB_RGP; }
       set { SetPropertyValue<string>("PRTAB_RGP", ref fPRTAB_RGP, value); }  
     }
    public string PRTAB_PRPL {get; set;}
    public string PRTAB_PLATBA {get; set;}
    public string PRTAB_POCITEJ {get; set;}
    public string PRTAB_SANKCE {get; set;}
    public string PRTAB_VYSTUPY {get; set;}
    public string PRTAB_VYMAHANI {get; set;}

    public struct CompoundKey1Struct
        {
            [Persistent("PRTAB_LOGIN")]
            public string PRTAB_LOGIN { get; set; }
            [Persistent("PRTAB_POPLATEK")]
            public string PRTAB_POPLATEK { get; set; }
        }

    [Key, Persistent]
    public CompoundKey1Struct pk;

    public P_PRISTUP_TABLE(Session session) : base(session) { }

  }
}