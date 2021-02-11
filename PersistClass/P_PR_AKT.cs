using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;

namespace PoplWS
{
  public class P_PR_AKT : XPLiteObject
  {
    public P_PR_AKT(Session session) : base(session) { }

    int fPRA_PRPL_ID;
    public int PRA_PRPL_ID
    {
      get { return fPRA_PRPL_ID; }
      set { SetPropertyValue<int>("PRA_PRPL_ID", ref fPRA_PRPL_ID, value); }
    }

    int fPRA_PR;
    public int PRA_PR
    {
      get { return fPRA_PR; }
      set { SetPropertyValue<int>("PRA_PR", ref fPRA_PR, value); }
    }

    string fPRA_TYP;
    [Size(1)]
    public string PRA_TYP
    {
      get { return fPRA_TYP; }
      set { SetPropertyValue<string>("PRA_TYP", ref fPRA_TYP, value); }
    }
  }
}