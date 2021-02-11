using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;


namespace PoplWS
{
    public class DUAL : XPLiteObject
    {
        string fdummy;
        public string DUMMY
        {
            get { return fdummy; }
        }

        public DUAL(Session session) : base(session) { }
    }
}