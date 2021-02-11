using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;

namespace PoplWS
{
  public partial class P_NASTAVENI : XPLiteObject
  {
    string fKLIC;
    [Key]
    [Size(32)]
    public string KLIC
    {
      get { return fKLIC; }
      set { SetPropertyValue<string>("KLIC", ref fKLIC, value); }
    }
    string fHODNOTA;
    [Size(128)]
    public string HODNOTA
    {
      get { return fHODNOTA; }
      set { SetPropertyValue<string>("HODNOTA", ref fHODNOTA, value); }
    }

    public P_NASTAVENI(Session session) : base(session) { }
  }
}