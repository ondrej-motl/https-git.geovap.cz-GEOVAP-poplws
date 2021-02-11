using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace PoplWS
{

    public partial class P_ODPADY_BYDLIC : XPLiteObject
    {
        decimal fBYDLICI_OBJEKT_BYT;
        [ColumnDbDefaultValue("((-1))")]
        public decimal BYDLICI_OBJEKT_BYT
        {
            get { return fBYDLICI_OBJEKT_BYT; }
            set { SetPropertyValue<decimal>("BYDLICI_OBJEKT_BYT", ref fBYDLICI_OBJEKT_BYT, value); }
        }
        DateTime fBYDLICI_DATUM_DO;
        public DateTime BYDLICI_DATUM_DO
        {
            get { return fBYDLICI_DATUM_DO; }
            set { SetPropertyValue<DateTime>("BYDLICI_DATUM_DO", ref fBYDLICI_DATUM_DO, value); }
        }
        string fBYDLICI_NAZEV;
        [Size(35)]
        public string BYDLICI_NAZEV
        {
            get { return fBYDLICI_NAZEV; }
            set { SetPropertyValue<string>("BYDLICI_NAZEV", ref fBYDLICI_NAZEV, value); }
        }
        char fBYDLICI_OSVOBOZEN;
        public char BYDLICI_OSVOBOZEN
        {
            get { return fBYDLICI_OSVOBOZEN; }
            set { SetPropertyValue<char>("BYDLICI_OSVOBOZEN", ref fBYDLICI_OSVOBOZEN, value); }
        }
        string fBYDLICI_POZNAMKA;
        [Size(500)]
        public string BYDLICI_POZNAMKA
        {
            get { return fBYDLICI_POZNAMKA; }
            set { SetPropertyValue<string>("BYDLICI_POZNAMKA", ref fBYDLICI_POZNAMKA, value); }
        }
        DateTime fBYDLICI_SLEVA_OD;
        public DateTime BYDLICI_SLEVA_OD
        {
            get { return fBYDLICI_SLEVA_OD; }
            set { SetPropertyValue<DateTime>("BYDLICI_SLEVA_OD", ref fBYDLICI_SLEVA_OD, value); }
        }
        DateTime fBYDLICI_SLEVA_DO;
        public DateTime BYDLICI_SLEVA_DO
        {
            get { return fBYDLICI_SLEVA_DO; }
            set { SetPropertyValue<DateTime>("BYDLICI_SLEVA_DO", ref fBYDLICI_SLEVA_DO, value); }
        }
        decimal fBYDLICI_SLEVA_PROCENT;
        public decimal BYDLICI_SLEVA_PROCENT
        {
            get { return fBYDLICI_SLEVA_PROCENT; }
            set { SetPropertyValue<decimal>("BYDLICI_SLEVA_PROCENT", ref fBYDLICI_SLEVA_PROCENT, value); }
        }
        decimal fBYDLICI_OSVOB_KOD;
        public decimal BYDLICI_OSVOB_KOD
        {
            get { return fBYDLICI_OSVOB_KOD; }
            set { SetPropertyValue<decimal>("BYDLICI_OSVOB_KOD", ref fBYDLICI_OSVOB_KOD, value); }
        }
        string fBYDLICI_PERIODA;
        [Size(1)]
        public string BYDLICI_PERIODA
        {
            get { return fBYDLICI_PERIODA; }
            set { SetPropertyValue<string>("BYDLICI_PERIODA", ref fBYDLICI_PERIODA, value); }
        }
        string fBYDLICI_REFERENT;
        [Size(15)]
        public string BYDLICI_REFERENT
        {
            get { return fBYDLICI_REFERENT; }
            set { SetPropertyValue<string>("BYDLICI_REFERENT", ref fBYDLICI_REFERENT, value); }
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
        int fBYDLICI_ID;
        [Indexed(Name = @"P_ODPADY_BYDLIC_ix4", Unique = true)]
        public int BYDLICI_ID
        {
            get { return fBYDLICI_ID; }
            set { SetPropertyValue<int>("BYDLICI_ID", ref fBYDLICI_ID, value); }
        }
        int fBYDLICI_BUD;
        [ColumnDbDefaultValue("((0))")]
        public int BYDLICI_BUD
        {
            get { return fBYDLICI_BUD; }
            set { SetPropertyValue<int>("BYDLICI_BUD", ref fBYDLICI_BUD, value); }
        }
        int fBYDLICI_BYT;
        [ColumnDbDefaultValue("((0))")]
        public int BYDLICI_BYT
        {
            get { return fBYDLICI_BYT; }
            set { SetPropertyValue<int>("BYDLICI_BYT", ref fBYDLICI_BYT, value); }
        }
        decimal fBYDLICI_ULEVA_KC;
        public decimal BYDLICI_ULEVA_KC
        {
            get { return fBYDLICI_ULEVA_KC; }
            set { SetPropertyValue<decimal>("BYDLICI_ULEVA_KC", ref fBYDLICI_ULEVA_KC, value); }
        }
        DateTime fBYDLICI_ULEVA_OD;
        public DateTime BYDLICI_ULEVA_OD
        {
            get { return fBYDLICI_ULEVA_OD; }
            set { SetPropertyValue<DateTime>("BYDLICI_ULEVA_OD", ref fBYDLICI_ULEVA_OD, value); }
        }
        DateTime fBYDLICI_ULEVA_DO;
        public DateTime BYDLICI_ULEVA_DO
        {
            get { return fBYDLICI_ULEVA_DO; }
            set { SetPropertyValue<DateTime>("BYDLICI_ULEVA_DO", ref fBYDLICI_ULEVA_DO, value); }
        }
        public struct CompoundKey1Struct
        {
            [Persistent("BYDLICI_OBJEKT_ID")]
            public decimal BYDLICI_OBJEKT_ID { get; set; }
            [Size(15)]
            [Persistent("BYDLICI_ICO")]
            public string BYDLICI_ICO { get; set; }
            [Persistent("BYDLICI_DATUM_OD")]
            public DateTime BYDLICI_DATUM_OD { get; set; }
        }
        [Key, Persistent]
        public CompoundKey1Struct CompoundKey1;

        public P_ODPADY_BYDLIC(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }

    }
}
