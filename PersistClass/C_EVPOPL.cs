using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace PoplWS
{
        public partial class C_EVPOPL : XPLiteObject
        {
            DateTime fEVPOPL_FROMDATE;
            public DateTime EVPOPL_FROMDATE
            {
                get { return fEVPOPL_FROMDATE; }
                set { SetPropertyValue<DateTime>("EVPOPL_FROMDATE", ref fEVPOPL_FROMDATE, value); }
            }
            DateTime fEVPOPL_TODATE;
            public DateTime EVPOPL_TODATE
            {
                get { return fEVPOPL_TODATE; }
                set { SetPropertyValue<DateTime>("EVPOPL_TODATE", ref fEVPOPL_TODATE, value); }
            }

            C_TERMPLAC fC_TERMPLAC;
            [Size(5)]
            [Association(@"EVPOPLReferencesC_TERMPLAC")]
            public C_TERMPLAC EVPOPL_TERMPLAC
            {
                get { return fC_TERMPLAC; }
                set { SetPropertyValue<C_TERMPLAC>("EVPOPL_TERMPLAC", ref fC_TERMPLAC, value); }
            }

            decimal fEVPOPL_POCDNU;
            public decimal EVPOPL_POCDNU
            {
                get { return fEVPOPL_POCDNU; }
                set { SetPropertyValue<decimal>("EVPOPL_POCDNU", ref fEVPOPL_POCDNU, value); }
            }
            string fEVPOPL_FORMTISK;
            [Size(10)]
            public string EVPOPL_FORMTISK
            {
                get { return fEVPOPL_FORMTISK; }
                set { SetPropertyValue<string>("EVPOPL_FORMTISK", ref fEVPOPL_FORMTISK, value); }
            }
            decimal fEVPOPL_PROCSANKCE;
            public decimal EVPOPL_PROCSANKCE
            {
                get { return fEVPOPL_PROCSANKCE; }
                set { SetPropertyValue<decimal>("EVPOPL_PROCSANKCE", ref fEVPOPL_PROCSANKCE, value); }
            }
            string fEVPOPL_KCDLEDNI;
            [Size(3)]
            public string EVPOPL_KCDLEDNI
            {
                get { return fEVPOPL_KCDLEDNI; }
                set { SetPropertyValue<string>("EVPOPL_KCDLEDNI", ref fEVPOPL_KCDLEDNI, value); }
            }
            string fEVPOPL_EXPFIN;
            [Size(3)]
            public string EVPOPL_EXPFIN
            {
                get { return fEVPOPL_EXPFIN; }
                set { SetPropertyValue<string>("EVPOPL_EXPFIN", ref fEVPOPL_EXPFIN, value); }
            }
            decimal fEVPOPL_KODUCTOV;
            public decimal EVPOPL_KODUCTOV
            {
                get { return fEVPOPL_KODUCTOV; }
                set { SetPropertyValue<decimal>("EVPOPL_KODUCTOV", ref fEVPOPL_KODUCTOV, value); }
            }
            decimal fEVPOPL_ROKUVCYKLU;
            public decimal EVPOPL_ROKUVCYKLU
            {
                get { return fEVPOPL_ROKUVCYKLU; }
                set { SetPropertyValue<decimal>("EVPOPL_ROKUVCYKLU", ref fEVPOPL_ROKUVCYKLU, value); }
            }
            string fEVPOPL_TYPSANKCE;  //zamena char za string
            [Size(1)]
            public string EVPOPL_TYPSANKCE
            {
                get { return fEVPOPL_TYPSANKCE; }
                set { SetPropertyValue<string>("EVPOPL_TYPSANKCE", ref fEVPOPL_TYPSANKCE, value); }
            }
            decimal fEVPOPL_PEVNACASTKA;
            public decimal EVPOPL_PEVNACASTKA
            {
                get { return fEVPOPL_PEVNACASTKA; }
                set { SetPropertyValue<decimal>("EVPOPL_PEVNACASTKA", ref fEVPOPL_PEVNACASTKA, value); }
            }
            decimal fEVPOPL_NASOBEK;
            public decimal EVPOPL_NASOBEK
            {
                get { return fEVPOPL_NASOBEK; }
                set { SetPropertyValue<decimal>("EVPOPL_NASOBEK", ref fEVPOPL_NASOBEK, value); }
            }
            string fEVPOPL_PERNAS;    //zamena char za string
            [Size(1)]
            public string EVPOPL_PERNAS
            {
                get { return fEVPOPL_PERNAS; }
                set { SetPropertyValue<string>("EVPOPL_PERNAS", ref fEVPOPL_PERNAS, value); }
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
            string fEVPOPL_TERMPLT_RGP;   //zamena char za string
            [Size(1)]
            public string EVPOPL_TERMPLT_RGP
            {
                get { return fEVPOPL_TERMPLT_RGP; }
                set { SetPropertyValue<string>("EVPOPL_TERMPLT_RGP", ref fEVPOPL_TERMPLT_RGP, value); }
            }
            string fEVPOPL_SPLT_VYST; //zamena char za string
            [Size(1)]
            public string EVPOPL_SPLT_VYST
            {
                get { return fEVPOPL_SPLT_VYST; }
                set { SetPropertyValue<string>("EVPOPL_SPLT_VYST", ref fEVPOPL_SPLT_VYST, value); }
            }
            string fEVPOPL_KCDLEMES;    //zamena char za string
            [Size(1)]
            public string EVPOPL_KCDLEMES
            {
                get { return fEVPOPL_KCDLEMES; }
                set { SetPropertyValue<string>("EVPOPL_KCDLEMES", ref fEVPOPL_KCDLEMES, value); }
            }
            string fEVPOPL_EDIT_SPLT;     //zamena char za string
            [Size(1)]
            public string EVPOPL_EDIT_SPLT
            {
                get { return fEVPOPL_EDIT_SPLT; }
                set { SetPropertyValue<string>("EVPOPL_EDIT_SPLT", ref fEVPOPL_EDIT_SPLT, value); }
            }
            string fEVPOPL_SPLATZAP;
            [Size(1)]
            public string EVPOPL_SPLATZAP
            {
                get { return fEVPOPL_SPLATZAP; }
                set { SetPropertyValue<string>("EVPOPL_SPLATZAP", ref fEVPOPL_SPLATZAP, value); }
            }
            string fEVPOPL_SPLATSANKCE;   //zamena char za string
            [Size(1)]
            public string EVPOPL_SPLATSANKCE
            {
                get { return fEVPOPL_SPLATSANKCE; }
                set { SetPropertyValue<string>("EVPOPL_SPLATSANKCE", ref fEVPOPL_SPLATSANKCE, value); }
            }
            decimal fEVPOPL_SS;
            public decimal EVPOPL_SS
            {
                get { return fEVPOPL_SS; }
                set { SetPropertyValue<decimal>("EVPOPL_SS", ref fEVPOPL_SS, value); }
            }

            public struct CompoundKey1Struct
            {
                //C_NAZPOPL fEVPOPL_KOD;
                [Persistent("EVPOPL_KOD")]
                [Association(@"EVPOPLreferencesNAZPOPL")]
                public C_NAZPOPL EVPOPL_KOD { get; set; }
                [Size(1)]
                [Persistent("EVPOPL_PER")]
                [Association(@"EVPOPLReferencesC_PERIODA")]
                public C_PERIODA EVPOPL_PER { get; set; }

            }

            [Indexed(Name = @"EVPOPL_PK", Unique = true)]
            [Key, Persistent]
            public CompoundKey1Struct CompoundKey1;

            public C_EVPOPL(Session session) : base(session) { }
        }
}