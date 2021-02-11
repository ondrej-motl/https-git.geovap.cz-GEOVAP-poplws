using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;

namespace PoplWS
{
    public class C_PERIODA : XPLiteObject
    {
        string fPERIODA_PERIODA;
        [Key]
        [Size(1)]
        public string PERIODA_PERIODA
        {
            get { return fPERIODA_PERIODA; }
            set { SetPropertyValue<string>("PERIODA_PERIODA", ref fPERIODA_PERIODA, value); }
        }
        string fPERIODA_NAZEV;
        [Size(25)]
        public string PERIODA_NAZEV
        {
            get { return fPERIODA_NAZEV; }
            set { SetPropertyValue<string>("PERIODA_NAZEV", ref fPERIODA_NAZEV, value); }
        }
        decimal fPERIODA_POC;
        public decimal PERIODA_POC
        {
            get { return fPERIODA_POC; }
            set { SetPropertyValue<decimal>("PERIODA_POC", ref fPERIODA_POC, value); }
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

        [Association(@"EVPOPLReferencesC_PERIODA", typeof(C_EVPOPL))]
        public XPCollection<C_EVPOPL> C_EVPOPL
        {
            get { return GetCollection<C_EVPOPL>("C_EVPOPL"); }
        }

        [Association(@"KALENDARReferencesC_PERIODA", typeof(C_KALENDAR))]
        public XPCollection<C_KALENDAR> C_KALENDAR
        {
          get { return GetCollection<C_KALENDAR>("C_KALENDAR"); }
        }

        public C_PERIODA(Session session) : base(session) { }
    }
}