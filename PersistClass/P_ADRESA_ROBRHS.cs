using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using System.Xml.Serialization;

namespace PoplWS
{

    public enum TypOsoby
    {
        [XmlEnumAttribute(Name = "pravnicka")] P,   //Attribute lze vynechat
        [XmlEnum(Name = "fyzicka")] F,
        [XmlEnum(Name = "cizinec")] C,
        [XmlEnum(Name = "OSVC")]    O
    }

    /// <summary>
    /// jen pro interni pouziti
    /// slouzi k prevodu ANO/NE na A/N
    /// </summary>
    internal enum SouhlasAN
    { 
       [XmlEnum("A")] A,
       [XmlEnum("N")] N
    }
    

    public enum SouhlasAnoNe  
    {   
        [XmlEnum("A")] ANO,
        [XmlEnum("N")] NE
    }
    

    public partial class P_ADRESA_ROBRHS : XPLiteObject
    {
        string fADR_ICO;
        [Indexed(@"ADR_ADRPOPL", Name = @"ROBRHS_UQ_1", Unique = true)]

        [Size(15)]
        public string ADR_ICO
        {
            get { return fADR_ICO; }
            set { SetPropertyValue<string>("ADR_ICO", ref fADR_ICO, value); }
        }

        //0.8
        string fADR_SICO;
        [Size(15)]
        public string ADR_SICO
        {
            get { return fADR_SICO; }
            set { SetPropertyValue<string>("ADR_SICO", ref fADR_SICO, value); }
        }

        string fADR_TYP;
        [Size(8)]
        public string ADR_TYP
        {
            get { return fADR_TYP; }
            set { SetPropertyValue<string>("ADR_TYP", ref fADR_TYP, value); }
        }

        string fADR_NAZEV1;
        [Indexed(Name = @"ROBRHS_ix1")]
        [Size(255)]
        public string ADR_NAZEV1
        {
            get { return fADR_NAZEV1; }
            set { SetPropertyValue<string>("ADR_NAZEV1", ref fADR_NAZEV1, value); }
        }
        string fADR_NAZEV2;
        [Size(35)]
        public string ADR_NAZEV2
        {
            get { return fADR_NAZEV2; }
            set { SetPropertyValue<string>("ADR_NAZEV2", ref fADR_NAZEV2, value); }
        }
        decimal? fADR_ICZUJ;
        public decimal? ADR_ICZUJ
        {
            get
            {
                return fADR_ICZUJ;
            }
            set { SetPropertyValue<decimal?>("ADR_ICZUJ", ref fADR_ICZUJ, value); }
        }
        string fADR_ICZUJ_NAZEV;
        [Size(48)]
        public string ADR_ICZUJ_NAZEV
        {
            get { return fADR_ICZUJ_NAZEV; }
            set { SetPropertyValue<string>("ADR_ICZUJ_NAZEV", ref fADR_ICZUJ_NAZEV, value); }
        }
        decimal? fADR_KODCOB;
        public decimal? ADR_KODCOB
        {
            get { return fADR_KODCOB; }
            set { SetPropertyValue<decimal?>("ADR_KODCOB", ref fADR_KODCOB, value); }
        }
        string fADR_KODCOB_NAZEV;
        [Size(48)]
        public string ADR_KODCOB_NAZEV
        {
            get { return fADR_KODCOB_NAZEV; }
            set { SetPropertyValue<string>("ADR_KODCOB_NAZEV", ref fADR_KODCOB_NAZEV, value); }
        }
        decimal? fADR_KODUL;
        public decimal? ADR_KODUL
        {
            get { return fADR_KODUL; }
            set { SetPropertyValue<decimal?>("ADR_KODUL", ref fADR_KODUL, value); }
        }
        string fADR_KODUL_NAZEV;
        [Size(48)]
        public string ADR_KODUL_NAZEV
        {
            get { return fADR_KODUL_NAZEV; }
            set { SetPropertyValue<string>("ADR_KODUL_NAZEV", ref fADR_KODUL_NAZEV, value); }
        }
        decimal? fADR_CIS_DOMU;
        public decimal? ADR_CIS_DOMU
        {
            get { return fADR_CIS_DOMU; }
            set { SetPropertyValue<decimal?>("ADR_CIS_DOMU", ref fADR_CIS_DOMU, value); }
        }
        decimal? fADR_CIS_OR;
        public decimal? ADR_CIS_OR
        {
            get { return fADR_CIS_OR; }
            set { SetPropertyValue<decimal?>("ADR_CIS_OR", ref fADR_CIS_OR, value); }
        }
        string fADR_ZNAK_CO;
        [Size(1)]
        public string ADR_ZNAK_CO
        {
            get { return fADR_ZNAK_CO; }
            set { SetPropertyValue<string>("ADR_ZNAK_CO", ref fADR_ZNAK_CO, value); }
        }
        string fADR_PSC;
        [Size(5)]
        public string ADR_PSC
        {
            get { return fADR_PSC; }
            set { SetPropertyValue<string>("ADR_PSC", ref fADR_PSC, value); }
        }
        string fADR_KNAZEV1;
        [Size(35)]
        public string ADR_KNAZEV1
        {
            get { return fADR_KNAZEV1; }
            set { SetPropertyValue<string>("ADR_KNAZEV1", ref fADR_KNAZEV1, value); }
        }
        string fADR_KNAZEV2;
        [Size(35)]
        public string ADR_KNAZEV2
        {
            get { return fADR_KNAZEV2; }
            set { SetPropertyValue<string>("ADR_KNAZEV2", ref fADR_KNAZEV2, value); }
        }
        decimal? fADR_KICZUJ;
        public decimal? ADR_KICZUJ
        {
            get { return fADR_KICZUJ; }
            set { SetPropertyValue<decimal?>("ADR_KICZUJ", ref fADR_KICZUJ, value); }
        }
        string fADR_KICZUJ_NAZEV;
        [Size(48)]
        public string ADR_KICZUJ_NAZEV
        {
            get { return fADR_KICZUJ_NAZEV; }
            set { SetPropertyValue<string>("ADR_KICZUJ_NAZEV", ref fADR_KICZUJ_NAZEV, value); }
        }
        decimal? fADR_KKODCOB;
        public decimal? ADR_KKODCOB
        {
            get { return fADR_KKODCOB; }
            set { SetPropertyValue<decimal?>("ADR_KKODCOB", ref fADR_KKODCOB, value); }
        }
        string fADR_KKODCOB_NAZEV;
        [Size(48)]
        public string ADR_KKODCOB_NAZEV
        {
            get { return fADR_KKODCOB_NAZEV; }
            set { SetPropertyValue<string>("ADR_KKODCOB_NAZEV", ref fADR_KKODCOB_NAZEV, value); }
        }
        decimal? fADR_KKODUL;
        public decimal? ADR_KKODUL
        {
            get { return fADR_KKODUL; }
            set { SetPropertyValue<decimal?>("ADR_KKODUL", ref fADR_KKODUL, value); }
        }
        string fADR_KKODUL_NAZEV;
        [Size(48)]
        public string ADR_KKODUL_NAZEV
        {
            get { return fADR_KKODUL_NAZEV; }
            set { SetPropertyValue<string>("ADR_KKODUL_NAZEV", ref fADR_KKODUL_NAZEV, value); }
        }
        decimal? fADR_KCIS_DOMU;
        public decimal? ADR_KCIS_DOMU
        {
            get { return fADR_KCIS_DOMU; }
            set { SetPropertyValue<decimal?>("ADR_KCIS_DOMU", ref fADR_KCIS_DOMU, value); }
        }
        decimal? fADR_KCIS_OR;
        public decimal? ADR_KCIS_OR
        {
            get { return fADR_KCIS_OR; }
            set { SetPropertyValue<decimal?>("ADR_KCIS_OR", ref fADR_KCIS_OR, value); }
        }
        string fADR_KZNAK_CO;
        [Size(1)]
        public string ADR_KZNAK_CO
        {
            get { return fADR_KZNAK_CO; }
            set { SetPropertyValue<string>("ADR_KZNAK_CO", ref fADR_KZNAK_CO, value); }
        }
        string fADR_KPSC;
        [Size(5)]
        public string ADR_KPSC
        {
            get { return fADR_KPSC; }
            set { SetPropertyValue<string>("ADR_KPSC", ref fADR_KPSC, value); }
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
        string fADR_SIPO;
        [Size(10)]
        public string ADR_SIPO
        {
            get { return fADR_SIPO; }
            set { SetPropertyValue<string>("ADR_SIPO", ref fADR_SIPO, value); }
        }
        string fADR_ZMENAROB;
        [Size(1)]
        public string ADR_ZMENAROB
        {
            get { return fADR_ZMENAROB; }
            set { SetPropertyValue<string>("ADR_ZMENAROB", ref fADR_ZMENAROB, value); }
        }
        string fADR_JMENO;
        [Size(24)]
        public string ADR_JMENO
        {
            get { return fADR_JMENO; }
            set { SetPropertyValue<string>("ADR_JMENO", ref fADR_JMENO, value); }
        }
        string fADR_KJMENO;
        [Size(24)]
        public string ADR_KJMENO
        {
            get { return fADR_KJMENO; }
            set { SetPropertyValue<string>("ADR_KJMENO", ref fADR_KJMENO, value); }
        }
        string fADR_PRIJMENI;
        [Size(30)]
        public string ADR_PRIJMENI
        {
            get { return fADR_PRIJMENI; }
            set { SetPropertyValue<string>("ADR_PRIJMENI", ref fADR_PRIJMENI, value); }
        }
        string fADR_KPRIJMENI;
        [Size(30)]
        public string ADR_KPRIJMENI
        {
            get { return fADR_KPRIJMENI; }
            set { SetPropertyValue<string>("ADR_KPRIJMENI", ref fADR_KPRIJMENI, value); }
        }
        string fADR_TITUL_PRED;
        [Size(20)]
        public string ADR_TITUL_PRED
        {
            get { return fADR_TITUL_PRED; }
            set { SetPropertyValue<string>("ADR_TITUL_PRED", ref fADR_TITUL_PRED, value); }
        }
        string fADR_KTITUL_PRED;
        [Size(20)]
        public string ADR_KTITUL_PRED
        {
            get { return fADR_KTITUL_PRED; }
            set { SetPropertyValue<string>("ADR_KTITUL_PRED", ref fADR_KTITUL_PRED, value); }
        }
        string fADR_TITUL_ZA;
        [Size(20)]
        public string ADR_TITUL_ZA
        {
            get { return fADR_TITUL_ZA; }
            set { SetPropertyValue<string>("ADR_TITUL_ZA", ref fADR_TITUL_ZA, value); }
        }
        string fADR_KTITUL_ZA;
        [Size(20)]
        public string ADR_KTITUL_ZA
        {
            get { return fADR_KTITUL_ZA; }
            set { SetPropertyValue<string>("ADR_KTITUL_ZA", ref fADR_KTITUL_ZA, value); }
        }
        decimal fADR_ADRPOPL;
        public decimal ADR_ADRPOPL
        {
            get { return fADR_ADRPOPL; }
            set { SetPropertyValue<decimal>("ADR_ADRPOPL", ref fADR_ADRPOPL, value); }
        }
        string fADR_ICO_OPROS;
        [Size(15)]
        public string ADR_ICO_OPROS
        {
            get { return fADR_ICO_OPROS; }
            set { SetPropertyValue<string>("ADR_ICO_OPROS", ref fADR_ICO_OPROS, value); }
        }
        char fADR_OPROS_ROZH;
        public char ADR_OPROS_ROZH
        {
            get { return fADR_OPROS_ROZH; }
            set { SetPropertyValue<char>("ADR_OPROS_ROZH", ref fADR_OPROS_ROZH, value); }
        }
        DateTime? fADR_DATNAR;
        public DateTime? ADR_DATNAR
        {
            get { return fADR_DATNAR; }
            set { SetPropertyValue<DateTime?>("ADR_DATNAR", ref fADR_DATNAR, value); }
        }
        DateTime fADR_ZMENAROB_ZPRAC;
        public DateTime ADR_ZMENAROB_ZPRAC
        {
            get { return fADR_ZMENAROB_ZPRAC; }
            set { SetPropertyValue<DateTime>("ADR_ZMENAROB_ZPRAC", ref fADR_ZMENAROB_ZPRAC, value); }
        }
        string fADR_POSTA;
        [Size(48)]
        public string ADR_POSTA
        {
            get { return fADR_POSTA; }
            set { SetPropertyValue<string>("ADR_POSTA", ref fADR_POSTA, value); }
        }
        string fADR_KPOSTA;
        [Size(48)]
        public string ADR_KPOSTA
        {
            get { return fADR_KPOSTA; }
            set { SetPropertyValue<string>("ADR_KPOSTA", ref fADR_KPOSTA, value); }
        }
        string fADR_TELEFON;
        [Size(16)]
        public string ADR_TELEFON
        {
            get { return fADR_TELEFON; }
            set { SetPropertyValue<string>("ADR_TELEFON", ref fADR_TELEFON, value); }
        }
        string fADR_EMAIL;
        [Size(50)]
        public string ADR_EMAIL
        {
            get { return fADR_EMAIL; }
            set { SetPropertyValue<string>("ADR_EMAIL", ref fADR_EMAIL, value); }
        }

        string fADR_PLATCEDPH;  
        [Size(1)]  
        public string ADR_PLATCEDPH
        {
            get { return fADR_PLATCEDPH; }
            set { SetPropertyValue<string>("ADR_PLATCEDPH", ref fADR_PLATCEDPH, value); }
        }

        string fADR_DIC;
        [Size(12)]
        public string ADR_DIC
        {
            get { return fADR_DIC; }
            set { SetPropertyValue<string>("ADR_DIC", ref fADR_DIC, value); }
        }
        DateTime fADR_ISIR;
        [Indexed(Name = @"P_ADRESA_ROBRHS_ISIR")]
        public DateTime ADR_ISIR
        {
            get { return fADR_ISIR; }
            set { SetPropertyValue<DateTime>("ADR_ISIR", ref fADR_ISIR, value); }
        }

        string fADR_IDDS;
        [Size(7)]
        public string ADR_IDDS
        {
            get { return fADR_IDDS; }
            set { SetPropertyValue<string>("ADR_IDDS", ref fADR_IDDS, value); }
        }
        string fADR_PAS;
        [Size(20)]
        public string ADR_PAS
        {
            get { return fADR_PAS; }
            set { SetPropertyValue<string>("ADR_PAS", ref fADR_PAS, value); }
        }
        string fADR_STAT;
        [Size(35)]
        public string ADR_STAT
        {
            get { return fADR_STAT; }
            set { SetPropertyValue<string>("ADR_STAT", ref fADR_STAT, value); }
        }

        //0.8 - do teto verze bylo decimal?
        decimal fADR_ID;
        [Key]
        public decimal ADR_ID
        {
            get { return fADR_ID; }
            set { SetPropertyValue<decimal>("ADR_ID", ref fADR_ID, value); }
        }

        string fADR_DADRESA;
        [Size(250)]
        public string ADR_DADRESA
        {
            get { return fADR_DADRESA; }
            set { SetPropertyValue<string>("ADR_DADRESA", ref fADR_DADRESA, value); }
        }

        int fADR_EA;
        public int ADR_EA
        {
          get { return fADR_EA; }
          set { SetPropertyValue<int>("ADR_EA", ref fADR_EA, value); }
        }

        public int ADR_ZAHR { get; set; }

        string fADR_ZAHR_ULICE_A_CISLO;
        [Size (89)]
        public string ADR_ZAHR_ULICE_A_CISLO 
        { 
            get { return fADR_ZAHR_ULICE_A_CISLO; }
            set { SetPropertyValue<string>("ADR_ZAHR_ULICE_A_CISLO", ref fADR_ZAHR_ULICE_A_CISLO, value);
                  ADR_ZAHR = ADR_ZAHR | Convert.ToInt32(!String.IsNullOrWhiteSpace(value));
                 }
        }

        string fADR_ZAHR_KOD_A_MESTO;
        [Size (48)]
        public string ADR_ZAHR_KOD_A_MESTO 
        { 
            get { return fADR_ZAHR_KOD_A_MESTO; }
            set { SetPropertyValue<string>("ADR_ZAHR_KOD_A_MESTO", ref fADR_ZAHR_KOD_A_MESTO, value);
                  ADR_ZAHR = ADR_ZAHR | Convert.ToInt32(!String.IsNullOrWhiteSpace(value));
                }
        }


        public P_ADRESA_ROBRHS(Session session) : base(session) 
        {
            ADR_ZAHR = 0;
        }
        public P_ADRESA_ROBRHS() 
        {
            ADR_ZAHR = 0;
        }

        protected override void OnSaving()
        {
            bool inserted = ((ADR_ID == 0) || (ADR_ID == null));
            base.OnSaving();

            DBValue dbv = DBValue.Instance(this.Session); 
            LOGIN = dbv.DBUserName;
            LASTUPDATE = dbv.DBSysDateTime;

            if (inserted)
            {
                DBUtil dbu = new DBUtil(this.Session);
                ADR_ID = dbu.LIZNI_SEQ("P_SEQ_ADR_ID");

                if (this.ADR_ICO_OPROS == null) { ADR_ICO_OPROS = "-1"; }
                        ADR_OPROS_ROZH = Convert.ToChar("N");
                ADR_ADRPOPL = -1m;  //neodsouhlasena adresa
                if (ADR_PLATCEDPH == null) { ADR_PLATCEDPH = "N"; }

                if ((string.IsNullOrEmpty(ADR_NAZEV1)) && (!string.IsNullOrEmpty(ADR_JMENO) 
                                                           || !string.IsNullOrEmpty(ADR_PRIJMENI)))
                    ADR_NAZEV1 = (ADR_PRIJMENI + ' ' + ADR_JMENO).Trim();
                
                if (!string.IsNullOrEmpty(ADR_NAZEV1) && !string.IsNullOrEmpty(ADR_TITUL_PRED))
                      ADR_NAZEV1 = ADR_TITUL_PRED + " " + ADR_NAZEV1;
                if (!string.IsNullOrEmpty(ADR_NAZEV1) && !string.IsNullOrEmpty(ADR_TITUL_ZA)) 
                      ADR_NAZEV1 += ", " + ADR_TITUL_ZA;
                

                if (string.IsNullOrWhiteSpace(ADR_ICO)) ADR_ICO = null;  //0.8
                if ((ADR_ICO != null) && ADR_ICO.Contains('#')) { throw new Exception("nepovolený tvar IČ/RČ"); }

                //0.8
                if (ADR_ICO == null) ADR_ICO = "#" + ADR_ID.ToString();
                if (ADR_TYP == null) { throw new Exception("TYP osoby musí být vyplněn"); }
                if (ADR_NAZEV1 == null) { throw new Exception("NAZEV osoby musí být vyplněn"); }
                if (((ADR_TYP == "F") || (ADR_TYP == "C")) && (ADR_DATNAR == null)) { throw new Exception("datum narození osoby musí být vyplněn"); }
                if (((ADR_ICZUJ_NAZEV == null) && (ADR_ZAHR == 0))
                    || ((ADR_ZAHR_ULICE_A_CISLO == null) && (ADR_ZAHR == 1)))
                   { throw new Exception("název ulice musí být vyplněn"); }    
                if ( (ADR_PLATCEDPH != "N") && (ADR_PLATCEDPH != "A"))
                            { throw new Exception("chybná hodnota plátce DPH (povolená hodnota-[A/N])"); }
            }

        }
    }


 }