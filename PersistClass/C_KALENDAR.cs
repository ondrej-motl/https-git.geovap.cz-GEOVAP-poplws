using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;

namespace PoplWS
{
    public partial class C_KALENDAR : XPLiteObject
    {
      string fKALENDAR_NAZOBD;
      [Size(25)]
      public string KALENDAR_NAZOBD
      {
        get { return fKALENDAR_NAZOBD; }
        set { SetPropertyValue<string>("KALENDAR_NAZOBD", ref fKALENDAR_NAZOBD, value); }
      }
      DateTime fKALENDAR_FROMDATE;
      public DateTime KALENDAR_FROMDATE
      {
        get { return fKALENDAR_FROMDATE; }
        set { SetPropertyValue<DateTime>("KALENDAR_FROMDATE", ref fKALENDAR_FROMDATE, value); }
      }
      DateTime fKALENDAR_TODATE;
      public DateTime KALENDAR_TODATE
      {
        get { return fKALENDAR_TODATE; }
        set { SetPropertyValue<DateTime>("KALENDAR_TODATE", ref fKALENDAR_TODATE, value); }
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
      public struct CompoundKey1Struct
      {
        [Size(1)]
        [Persistent("KALENDAR_PERIODA")]
        [Association(@"KALENDARReferencesC_PERIODA")]
        public C_PERIODA KALENDAR_PERIODA { get; set; }
        [Persistent("KALENDAR_PORPER")]
        public decimal KALENDAR_PORPER { get; set; }
      }
      [Indexed(Name = @"KALENDAR_PK", Unique = true)]
      [Key, Persistent]
      public CompoundKey1Struct CompoundKey1;
      public C_KALENDAR(Session session) : base(session) { }
  }
}