using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml.Serialization;
using DevExpress.Xpo;

namespace PoplWS
{
  //P_ADRESA_ROBRHS
  public class OSOBA
    {

        [Persistent(@"ADR_ICO")]
        public string RC_IC { get; set; }

        TypOsoby fTYP;
        public TypOsoby TYP
        {
            get
            {
                return fTYP;
            }
            set
            {
                fTYP = value;
                this.ADR_TYP = this.TYP.ToString();
            }

        }

        [XmlIgnore]
        string fADR_TYP;
        private string ADR_TYP
        {
            get
            {
                return fADR_TYP;
            }
            set
            {
                fADR_TYP = value;
                fTYP = (TypOsoby)Enum.Parse(typeof(TypOsoby), value);
            }
        }

        [Persistent(@"ADR_JMENO")]
        public string JMENO { get; set; }

        [Persistent(@"ADR_PRIJMENI")]
        public string PRIJMENI { get; set; }

        [Persistent(@"ADR_TITUL_PRED")]
        public string TITUL_PRED { get; set; }

        [Persistent(@"ADR_TITUL_ZA")]
        public string TITUL_ZA { get; set; }

        [Persistent(@"ADR_NAZEV1")]
        public string NAZEV { get; set; }

        [Persistent(@"ADR_NAZEV2")]
        public string DOPLNKOVY_NAZEV { get; set; }

        [Persistent(@"ADR_ICZUJ")]
        decimal? fOBEC_KOD;
        public decimal? OBEC_KOD
        {   get { return fOBEC_KOD; } //0.22
            set { fOBEC_KOD = value; }
        }

        [Persistent(@"ADR_ICZUJ_NAZEV")]
        public string OBEC_NAZEV { get; set; }

        [Persistent(@"ADR_KODCOB")]
        decimal? fCAST_OBCE_KOD;
        public decimal? CAST_OBCE_KOD 
        {
            get { return fCAST_OBCE_KOD; } //0.22
            set { fCAST_OBCE_KOD = value; } 
        }

        [Persistent(@"ADR_KODCOB_NAZEV")]
        public string CAST_OBCE_NAZEV { get; set; }

        [Persistent(@"ADR_KODUL")]
        decimal? fULICE_KOD;
        public decimal? ULICE_KOD 
        {
            get { return fULICE_KOD; } //0.22
            set { fULICE_KOD = value; } 
        }

        [Persistent(@"ADR_KODUL_NAZEV")]
        public string ULICE_NAZEV { get; set; }

        [Persistent(@"ADR_CIS_DOMU")]
        public decimal? CIS_DOMU { get; set; }

        [Persistent(@"ADR_CIS_OR")]
        public decimal? CIS_OR { get; set; }

        [Persistent(@"ADR_ZNAK_CO")]
        public string ZNAK_CO { get; set; }

        [Persistent(@"ADR_PSC")]
        public string PSC { get; set; }

        [XmlElement("OSOBA_ID")]
        public decimal? ADR_ID { get; set; }


        [Persistent(@"ADR_SIPO")]
        public string SIPO { get; set; }

        [Persistent(@"ADR_DATNAR")]
        public DateTime? DATUM_NAROZENI { get; set; }

        [Persistent(@"ADR_POSTA")]
        public string DORUC_POSTA { get; set; }

        [Persistent(@"ADR_TELEFON")]
        public string TELEFON { get; set; }

        [Persistent(@"ADR_EMAIL")]
        public string EMAIL { get; set; }

        SouhlasAnoNe? fPLATCEDPH;
        public SouhlasAnoNe? PLATCE_DPH
        {   get { return fPLATCEDPH; }
            set { fPLATCEDPH = value ?? SouhlasAnoNe.NE;
                  SouhlasAN an = ((SouhlasAN)(int)fPLATCEDPH);
                  ADR_PLATCEDPH = an.ToString(); }}

        [XmlIgnore]  
        private string ADR_PLATCEDPH { get; set; }
        

        [Persistent(@"ADR_DIC")]
        public string DIC { get; set; }

        [Persistent(@"ADR_IDDS")]
        public string IDDS { get; set; }

        [Persistent(@"ADR_PAS")]
        public string PAS { get; set; }

        [Persistent(@"ADR_STAT")]
        public string STAT { get; set; }

        [XmlIgnore]
        internal int ADR_EA { get; set; }

        public KONTAKTNI_ADRESA KONTAKTNI_ADRESA;

        public ZAHRANICNI_ADRESA ZAHRANICNI_ADRESA;

        internal Session sesna;
        public OSOBA(Session session)
        {
          sesna = session;
          KONTAKTNI_ADRESA = new KONTAKTNI_ADRESA();
        }

        public OSOBA()
        {
          KONTAKTNI_ADRESA = new KONTAKTNI_ADRESA();
        }
    }

    public class ZAHRANICNI_ADRESA //0.36
      {
          [Persistent(@"ADR_ZAHR_ULICE_A_CISLO")]
          public string ULICE_A_CISLO { get; set; }

          [Persistent(@"ADR_ZAHR_KOD_A_MESTO")]
          public string KOD_A_MESTO { get; set; }

          [Persistent(@"ADR_STAT")]
          public string STAT { get; set; }           
      }


    public class KONTAKTNI_ADRESA  
    {
        [Persistent(@"ADR_KJMENO")]
        public string JMENO { get; set; }

        [Persistent(@"ADR_KPRIJMENI")]
        public string PRIJMENI { get; set; }

        [Persistent(@"ADR_KTITUL_PRED")]
        public string TITUL_PRED { get; set; }

        [Persistent(@"ADR_KTITUL_ZA")]
        public string TITUL_ZA { get; set; }

        [Persistent(@"ADR_KNAZEV1")]
        public string NAZEV { get; set; }

        [Persistent(@"ADR_KNAZEV2")]
        public string DOPLNKOVY_NAZEV { get; set; }

        [Persistent(@"ADR_KICZUJ")]
        decimal? fOBEC_KOD;
        public decimal? OBEC_KOD
        {
            get { return fOBEC_KOD; } //0.22
            set { fOBEC_KOD = value; }
        }

        [Persistent(@"ADR_KICZUJ_NAZEV")]
        public string OBEC_NAZEV { get; set; }

        [Persistent(@"ADR_KKODCOB")]
        decimal? fCAST_OBCE_KOD;
        public decimal? CAST_OBCE_KOD
        {
            get { return fCAST_OBCE_KOD; } //0.22
            set { fCAST_OBCE_KOD = value; }
        }

        [Persistent(@"ADR_KKODCOB_NAZEV")]
        public string CAST_OBCE_NAZEV { get; set; }

        [Persistent(@"ADR_KKODUL")]
        decimal? fULICE_KOD;
        public decimal? ULICE_KOD
        {
            get { return fULICE_KOD; } //0.22
            set { fULICE_KOD = value; }
        }

        [Persistent(@"ADR_KKODUL_NAZEV")]
        public string ULICE_NAZEV { get; set; }

        [Persistent(@"ADR_KCIS_DOMU")]
        public decimal? CIS_DOMU { get; set; }

        [Persistent(@"ADR_KCIS_OR")]
        public decimal? CIS_OR { get; set; }

        [Persistent(@"ADR_KZNAK_CO")]
        public string ZNAK_CO { get; set; }

        [Persistent(@"ADR_KPSC")]
        public string PSC { get; set; }

        [Persistent(@"ADR_KPOSTA")]
        public string DORUC_POSTA { get; set; }
    }

}