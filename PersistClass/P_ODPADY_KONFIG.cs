using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace PoplWS
{

    public class P_ODPADY_KONFIG : XPLiteObject
    {
        decimal fKONF_POPLATEK;
        [Key] //[Key]
        public decimal KONF_POPLATEK
        {
            get { return fKONF_POPLATEK; }
            set { SetPropertyValue<decimal>("KONF_POPLATEK", ref fKONF_POPLATEK, value); }
        }
        string fKONF_PERIODA;
        [Size(1)]
        public string KONF_PERIODA
        {
            get { return fKONF_PERIODA; }
            set { SetPropertyValue<string>("KONF_PERIODA", ref fKONF_PERIODA, value); }
        }
        decimal fKONF_ROK;
        public decimal KONF_ROK
        {
            get { return fKONF_ROK; }
            set { SetPropertyValue<decimal>("KONF_ROK", ref fKONF_ROK, value); }
        }
        decimal fKONF_CASTKA;
        public decimal KONF_CASTKA
        {
            get { return fKONF_CASTKA; }
            set { SetPropertyValue<decimal>("KONF_CASTKA", ref fKONF_CASTKA, value); }
        }
        decimal fKONF_VEKDITE;
        public decimal KONF_VEKDITE
        {
            get { return fKONF_VEKDITE; }
            set { SetPropertyValue<decimal>("KONF_VEKDITE", ref fKONF_VEKDITE, value); }
        }
        decimal fKONF_VEKDUCH;
        public decimal KONF_VEKDUCH
        {
            get { return fKONF_VEKDUCH; }
            set { SetPropertyValue<decimal>("KONF_VEKDUCH", ref fKONF_VEKDUCH, value); }
        }
        decimal fKONF_DITE_PROCENTO;
        public decimal KONF_DITE_PROCENTO
        {
            get { return fKONF_DITE_PROCENTO; }
            set { SetPropertyValue<decimal>("KONF_DITE_PROCENTO", ref fKONF_DITE_PROCENTO, value); }
        }
        decimal fKONF_DUCH_PROCENTO;
        public decimal KONF_DUCH_PROCENTO
        {
            get { return fKONF_DUCH_PROCENTO; }
            set { SetPropertyValue<decimal>("KONF_DUCH_PROCENTO", ref fKONF_DUCH_PROCENTO, value); }
        }
        decimal fKONF_ROUNDMETODA;
        public decimal KONF_ROUNDMETODA
        {
            get { return fKONF_ROUNDMETODA; }
            set { SetPropertyValue<decimal>("KONF_ROUNDMETODA", ref fKONF_ROUNDMETODA, value); }
        }
        decimal fKONF_ROUNDUROVEN;
        public decimal KONF_ROUNDUROVEN
        {
            get { return fKONF_ROUNDUROVEN; }
            set { SetPropertyValue<decimal>("KONF_ROUNDUROVEN", ref fKONF_ROUNDUROVEN, value); }
        }
        string fKONF_ON_LINE;
        [Size(1)]
        public string KONF_ON_LINE
        {
            get { return fKONF_ON_LINE; }
            set { SetPropertyValue<string>("KONF_ON_LINE", ref fKONF_ON_LINE, value); }
        }
        string fKONF_HEAD1;
        [Size(80)]
        public string KONF_HEAD1
        {
            get { return fKONF_HEAD1; }
            set { SetPropertyValue<string>("KONF_HEAD1", ref fKONF_HEAD1, value); }
        }
        string fKONF_HEAD2;
        [Size(80)]
        public string KONF_HEAD2
        {
            get { return fKONF_HEAD2; }
            set { SetPropertyValue<string>("KONF_HEAD2", ref fKONF_HEAD2, value); }
        }
        string fKONF_HEAD3;
        [Size(80)]
        public string KONF_HEAD3
        {
            get { return fKONF_HEAD3; }
            set { SetPropertyValue<string>("KONF_HEAD3", ref fKONF_HEAD3, value); }
        }
        decimal fKONF_PORPER;
        public decimal KONF_PORPER
        {
            get { return fKONF_PORPER; }
            set { SetPropertyValue<decimal>("KONF_PORPER", ref fKONF_PORPER, value); }
        }
        int fKONF_MIN_CASTKA;
        public int KONF_MIN_CASTKA
        {
            get { return fKONF_MIN_CASTKA; }
            set { SetPropertyValue<int>("KONF_MIN_CASTKA", ref fKONF_MIN_CASTKA, value); }
        }
        string fKONF_PER_POR;
        [Size(254)]
        public string KONF_PER_POR
        {
            get { return fKONF_PER_POR; }
            set { SetPropertyValue<string>("KONF_PER_POR", ref fKONF_PER_POR, value); }
        }
        string fKONF_SUPERUSER;
        [Size(32)]
        public string KONF_SUPERUSER
        {
            get { return fKONF_SUPERUSER; }
            set { SetPropertyValue<string>("KONF_SUPERUSER", ref fKONF_SUPERUSER, value); }
        }
        decimal fKONF_UCETROK;
        public decimal KONF_UCETROK
        {
            get { return fKONF_UCETROK; }
            set { SetPropertyValue<decimal>("KONF_UCETROK", ref fKONF_UCETROK, value); }
        }
        string fKONF_ARCHIV;
        [Size(10)]
        public string KONF_ARCHIV
        {
            get { return fKONF_ARCHIV; }
            set { SetPropertyValue<string>("KONF_ARCHIV", ref fKONF_ARCHIV, value); }
        }
        string fUNIQUE_PER;
        [Size(16)]
        public string UNIQUE_PER
        {
            get { return fUNIQUE_PER; }
            set { SetPropertyValue<string>("UNIQUE_PER", ref fUNIQUE_PER, value); }
        }
        int fKONF_MIN_VRATKA;
        public int KONF_MIN_VRATKA
        {
            get { return fKONF_MIN_VRATKA; }
            set { SetPropertyValue<int>("KONF_MIN_VRATKA", ref fKONF_MIN_VRATKA, value); }
        }
        int fKONF_UCETMES;
        public int KONF_UCETMES
        {
            get { return fKONF_UCETMES; }
            set { SetPropertyValue<int>("KONF_UCETMES", ref fKONF_UCETMES, value); }
        }
        string fKONF_PERPOPL;
        [Size(254)]
        public string KONF_PERPOPL
        {
            get { return fKONF_PERPOPL; }
            set { SetPropertyValue<string>("KONF_PERPOPL", ref fKONF_PERPOPL, value); }
        }
        string fKONF_SPL_TERMPLAC;
        [Size(5)]
        public string KONF_SPL_TERMPLAC
        {
            get { return fKONF_SPL_TERMPLAC; }
            set { SetPropertyValue<string>("KONF_SPL_TERMPLAC", ref fKONF_SPL_TERMPLAC, value); }
        }
        decimal fKONF_SPL_POCDNU;
        public decimal KONF_SPL_POCDNU
        {
            get { return fKONF_SPL_POCDNU; }
            set { SetPropertyValue<decimal>("KONF_SPL_POCDNU", ref fKONF_SPL_POCDNU, value); }
        }
        string fKONF_ISEODUVOD;
        [Size(64)]
        public string KONF_ISEODUVOD
        {
            get { return fKONF_ISEODUVOD; }
            set { SetPropertyValue<string>("KONF_ISEODUVOD", ref fKONF_ISEODUVOD, value); }
        }
        decimal fKONF_SPL_PPPDNU;
        public decimal KONF_SPL_PPPDNU
        {
            get { return fKONF_SPL_PPPDNU; }
            set { SetPropertyValue<decimal>("KONF_SPL_PPPDNU", ref fKONF_SPL_PPPDNU, value); }
        }
        string fKONF_SLPERS;
        [Size(16)]
        public string KONF_SLPERS
        {
            get { return fKONF_SLPERS; }
            set { SetPropertyValue<string>("KONF_SLPERS", ref fKONF_SLPERS, value); }
        }

        public P_ODPADY_KONFIG(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
