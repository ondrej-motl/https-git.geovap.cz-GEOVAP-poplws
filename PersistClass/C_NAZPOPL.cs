//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace PoplWS
{

    public partial class C_NAZPOPL : XPLiteObject
    {
        decimal fNAZPOPL_POPLATEK;
        [Indexed(@"NAZPOPL_PORVS", Name = @"C_NAZPOPL_uk", Unique = true)]
        [Key(AutoGenerate = true)]
        public decimal NAZPOPL_POPLATEK
        {
            get { return fNAZPOPL_POPLATEK; }
            set { SetPropertyValue<decimal>("NAZPOPL_POPLATEK", ref fNAZPOPL_POPLATEK, value); }
        }
        string fC_NAZPOPL_NAZEV;
        [Size(50)]
        [Persistent(@"NAZPOPL_NAZEV")]
        public string C_NAZPOPL_NAZEV
        {
            get { return fC_NAZPOPL_NAZEV; }
            set { SetPropertyValue<string>("C_NAZPOPL_NAZEV", ref fC_NAZPOPL_NAZEV, value); }
        }
        string fNAZPOPL_FORMATVS;
        [Size(14)]
        public string NAZPOPL_FORMATVS
        {
            get { return fNAZPOPL_FORMATVS; }
            set { SetPropertyValue<string>("NAZPOPL_FORMATVS", ref fNAZPOPL_FORMATVS, value); }
        }
        string fNAZPOPL_PREFIXFA;
        [Size(5)]
        public string NAZPOPL_PREFIXFA
        {
            get { return fNAZPOPL_PREFIXFA; }
            set { SetPropertyValue<string>("NAZPOPL_PREFIXFA", ref fNAZPOPL_PREFIXFA, value); }
        }
        decimal fNAZPOPL_PORVS;
        public decimal NAZPOPL_PORVS
        {
            get { return fNAZPOPL_PORVS; }
            set { SetPropertyValue<decimal>("NAZPOPL_PORVS", ref fNAZPOPL_PORVS, value); }
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
        decimal fNAZPOPL_ADRPOPL;
        public decimal NAZPOPL_ADRPOPL
        {
            get { return fNAZPOPL_ADRPOPL; }
            set { SetPropertyValue<decimal>("NAZPOPL_ADRPOPL", ref fNAZPOPL_ADRPOPL, value); }
        }
        char fNAZPOPL_PARPLATEB;
        public char NAZPOPL_PARPLATEB
        {
            get { return fNAZPOPL_PARPLATEB; }
            set { SetPropertyValue<char>("NAZPOPL_PARPLATEB", ref fNAZPOPL_PARPLATEB, value); }
        }
        char fNAZPOPL_TISKIHNED;
        public char NAZPOPL_TISKIHNED
        {
            get { return fNAZPOPL_TISKIHNED; }
            set { SetPropertyValue<char>("NAZPOPL_TISKIHNED", ref fNAZPOPL_TISKIHNED, value); }
        }
        string fNAZPOPL_VYSTIHNED;
        [Size(10)]
        public string NAZPOPL_VYSTIHNED
        {
            get { return fNAZPOPL_VYSTIHNED; }
            set { SetPropertyValue<string>("NAZPOPL_VYSTIHNED", ref fNAZPOPL_VYSTIHNED, value); }
        }
        char fNAZPOPL_ARCHIV_DLUH;
        public char NAZPOPL_ARCHIV_DLUH
        {
            get { return fNAZPOPL_ARCHIV_DLUH; }
            set { SetPropertyValue<char>("NAZPOPL_ARCHIV_DLUH", ref fNAZPOPL_ARCHIV_DLUH, value); }
        }
        string fNAZPOPL_PRIJUCET;
        [Size(20)]
        public string NAZPOPL_PRIJUCET
        {
            get { return fNAZPOPL_PRIJUCET; }
            set { SetPropertyValue<string>("NAZPOPL_PRIJUCET", ref fNAZPOPL_PRIJUCET, value); }
        }
        string fNAZPOPL_PRIJBANKA;
        [Size(5)]
        public string NAZPOPL_PRIJBANKA
        {
            get { return fNAZPOPL_PRIJBANKA; }
            set { SetPropertyValue<string>("NAZPOPL_PRIJBANKA", ref fNAZPOPL_PRIJBANKA, value); }
        }

        private string _NAZPOPL_IBAN;
        [Size(24)]
        public string NAZPOPL_IBAN
        {
            get
            {
                return _NAZPOPL_IBAN;
            }
            set
            {
                SetPropertyValue("NAZPOPL_IBAN", ref _NAZPOPL_IBAN, value);
            }
        }

        decimal fNAZPOPL_DISKONTDLE;
        public decimal NAZPOPL_DISKONTDLE
        {
            get { return fNAZPOPL_DISKONTDLE; }
            set { SetPropertyValue<decimal>("NAZPOPL_DISKONTDLE", ref fNAZPOPL_DISKONTDLE, value); }
        }
        decimal fNAZPOPL_ZVYS_PEVNACASTKA;
        public decimal NAZPOPL_ZVYS_PEVNACASTKA
        {
            get { return fNAZPOPL_ZVYS_PEVNACASTKA; }
            set { SetPropertyValue<decimal>("NAZPOPL_ZVYS_PEVNACASTKA", ref fNAZPOPL_ZVYS_PEVNACASTKA, value); }
        }
        decimal fNAZPOPL_ZVYS_PROC;
        public decimal NAZPOPL_ZVYS_PROC
        {
            get { return fNAZPOPL_ZVYS_PROC; }
            set { SetPropertyValue<decimal>("NAZPOPL_ZVYS_PROC", ref fNAZPOPL_ZVYS_PROC, value); }
        }
        decimal fNAZPOPL_ZVYS_MAXPROC;
        public decimal NAZPOPL_ZVYS_MAXPROC
        {
            get { return fNAZPOPL_ZVYS_MAXPROC; }
            set { SetPropertyValue<decimal>("NAZPOPL_ZVYS_MAXPROC", ref fNAZPOPL_ZVYS_MAXPROC, value); }
        }
        string fNAZPOPL_ZVYS_ZAOKR;
        [Size(2)]
        public string NAZPOPL_ZVYS_ZAOKR
        {
            get { return fNAZPOPL_ZVYS_ZAOKR; }
            set { SetPropertyValue<string>("NAZPOPL_ZVYS_ZAOKR", ref fNAZPOPL_ZVYS_ZAOKR, value); }
        }
        decimal fNAZPOPL_ENAK_PROC;
        public decimal NAZPOPL_ENAK_PROC
        {
            get { return fNAZPOPL_ENAK_PROC; }
            set { SetPropertyValue<decimal>("NAZPOPL_ENAK_PROC", ref fNAZPOPL_ENAK_PROC, value); }
        }
        decimal fNAZPOPL_ENAK_MIN;
        public decimal NAZPOPL_ENAK_MIN
        {
            get { return fNAZPOPL_ENAK_MIN; }
            set { SetPropertyValue<decimal>("NAZPOPL_ENAK_MIN", ref fNAZPOPL_ENAK_MIN, value); }
        }
        string fNAZPOPL_ENAK_ZAOKR;
        [Size(2)]
        public string NAZPOPL_ENAK_ZAOKR
        {
            get { return fNAZPOPL_ENAK_ZAOKR; }
            set { SetPropertyValue<string>("NAZPOPL_ENAK_ZAOKR", ref fNAZPOPL_ENAK_ZAOKR, value); }
        }
        string fNAZPOPL_NAZEV2;
        [Size(178)]
        public string NAZPOPL_NAZEV2
        {
            get { return fNAZPOPL_NAZEV2; }
            set { SetPropertyValue<string>("NAZPOPL_NAZEV2", ref fNAZPOPL_NAZEV2, value); }
        }
        char fNAZPOPL_DPH;
        public char NAZPOPL_DPH
        {
            get { return fNAZPOPL_DPH; }
            set { SetPropertyValue<char>("NAZPOPL_DPH", ref fNAZPOPL_DPH, value); }
        }
        char fNAZPOPL_DPHROUND;
        public char NAZPOPL_DPHROUND
        {
            get { return fNAZPOPL_DPHROUND; }
            set { SetPropertyValue<char>("NAZPOPL_DPHROUND", ref fNAZPOPL_DPHROUND, value); }
        }
        char fNAZPOPL_TYPPOPL;
        public char NAZPOPL_TYPPOPL
        {
            get { return fNAZPOPL_TYPPOPL; }
            set { SetPropertyValue<char>("NAZPOPL_TYPPOPL", ref fNAZPOPL_TYPPOPL, value); }
        }
        string fNAZPOPL_ZODP;
        [Size(32)]
        public string NAZPOPL_ZODP
        {
            get { return fNAZPOPL_ZODP; }
            set { SetPropertyValue<string>("NAZPOPL_ZODP", ref fNAZPOPL_ZODP, value); }
        }
        char fNAZPOPL_VPREDPV;
        public char NAZPOPL_VPREDPV
        {
            get { return fNAZPOPL_VPREDPV; }
            set { SetPropertyValue<char>("NAZPOPL_VPREDPV", ref fNAZPOPL_VPREDPV, value); }
        }
        string fNAZPOPL_PREVZ;
        [Size(10)]
        public string NAZPOPL_PREVZ
        {
            get { return fNAZPOPL_PREVZ; }
            set { SetPropertyValue<string>("NAZPOPL_PREVZ", ref fNAZPOPL_PREVZ, value); }
        }
        decimal fNAZPOPL_SS;
        public decimal NAZPOPL_SS
        {
            get { return fNAZPOPL_SS; }
            set { SetPropertyValue<decimal>("NAZPOPL_SS", ref fNAZPOPL_SS, value); }
        }
        string fNAZPOPL_ORG;
        [Size(4)]
        public string NAZPOPL_ORG
        {
            get { return fNAZPOPL_ORG; }
            set { SetPropertyValue<string>("NAZPOPL_ORG", ref fNAZPOPL_ORG, value); }
        }
        decimal fNAZPOPL_POKLDOK;
        public decimal NAZPOPL_POKLDOK
        {
            get { return fNAZPOPL_POKLDOK; }
            set { SetPropertyValue<decimal>("NAZPOPL_POKLDOK", ref fNAZPOPL_POKLDOK, value); }
        }

        [Association(@"EVPOPLreferencesNAZPOPL", typeof(C_EVPOPL))]
        public XPCollection<C_EVPOPL> C_EVPOPL
        {
            get { return GetCollection<C_EVPOPL>("C_EVPOPL"); }
        }

        public C_NAZPOPL(Session session) : base(session) { }
    }

}