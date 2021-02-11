using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace PoplWS
{

    public partial class P_ODPADY_MAJITEL : XPLiteObject
    {
        decimal fMAJITEL_OBJEKT_ID;
        public decimal MAJITEL_OBJEKT_ID
        {
            get { return fMAJITEL_OBJEKT_ID; }
            set { SetPropertyValue<decimal>("MAJITEL_OBJEKT_ID", ref fMAJITEL_OBJEKT_ID, value); }
        }
        decimal fMAJITEL_OBJEKT_BYT;
        [Indexed(@"MAJITEL_OBJEKT_ID;MAJITEL_ICO", Name = @"P_ix13")]
        [Key]
        public decimal MAJITEL_OBJEKT_BYT
        {
            get { return fMAJITEL_OBJEKT_BYT; }
            set { SetPropertyValue<decimal>("MAJITEL_OBJEKT_BYT", ref fMAJITEL_OBJEKT_BYT, value); }
        }
        string fMAJITEL_ICO;
        [Indexed(Name = @"P_ODPADY_MAJ_ICO", Unique = true)]
        [Size(15)]
        public string MAJITEL_ICO
        {
            get { return fMAJITEL_ICO; }
            set { SetPropertyValue<string>("MAJITEL_ICO", ref fMAJITEL_ICO, value); }
        }
        char fMAJITEL_PLATCE;
        public char MAJITEL_PLATCE
        {
            get { return fMAJITEL_PLATCE; }
            set { SetPropertyValue<char>("MAJITEL_PLATCE", ref fMAJITEL_PLATCE, value); }
        }
        string fMAJITEL_PERIODA;
        [Size(1)]
        public string MAJITEL_PERIODA
        {
            get { return fMAJITEL_PERIODA; }
            set { SetPropertyValue<string>("MAJITEL_PERIODA", ref fMAJITEL_PERIODA, value); }
        }
        string fMAJITEL_POZNAMKA;
        [Size(250)]
        public string MAJITEL_POZNAMKA
        {
            get { return fMAJITEL_POZNAMKA; }
            set { SetPropertyValue<string>("MAJITEL_POZNAMKA", ref fMAJITEL_POZNAMKA, value); }
        }
        string fLOGIN;
        [Size(35)]
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
        DateTime fENTRYDATE;
        public DateTime ENTRYDATE
        {
            get { return fENTRYDATE; }
            set { SetPropertyValue<DateTime>("ENTRYDATE", ref fENTRYDATE, value); }
        }
        int fMAJITEL_BUD;
        [ColumnDbDefaultValue("((0))")]
        public int MAJITEL_BUD
        {
            get { return fMAJITEL_BUD; }
            set { SetPropertyValue<int>("MAJITEL_BUD", ref fMAJITEL_BUD, value); }
        }

        public P_ODPADY_MAJITEL(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

    }
}
