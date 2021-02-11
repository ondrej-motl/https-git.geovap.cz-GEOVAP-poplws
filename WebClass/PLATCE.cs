using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using System.Xml.Serialization;
using DevExpress.Data.Filtering;
using PoplWS.PersistClass;

namespace PoplWS
{
    public class PLATCE
    {
        [XmlIgnore]
        internal string RGP_PLATBA { get; set; }

        [XmlIgnore]
        internal string RGP_ZADANO { get; set; }

        [XmlIgnore]
        internal decimal RGP_SAZBA { get; set; }

        [XmlElement("KC_ZAROK")]
        public decimal RGP_KCROK { get; set; }

        [XmlElement("SPLATKA")]
        public decimal RGP_KCSPLATKA { get; set; }

        [XmlElement("POSLEDNI_SPLATKA")]
        public decimal RGP_POSLSPLATKA { get; set; }

        [XmlIgnore]
        internal decimal RGP_PROCSANKCE { get; set; }

        [XmlIgnore]
        internal string RGP_VYSTUP { get; set; }

        [XmlElement("PLATNOST_OD")]
        public DateTime RGP_FROMDATE { get; set; }

        [XmlElement("PLATNOST_DO")]
        public DateTime? RGP_TODATE { get; set; }

        [XmlIgnore]
        internal DateTime RGP_NEXTDATESPL { get; set; }

        [XmlElement("PORADI_VS")]
        public int RGP_PORVS { get; set; }

        SouhlasAnoNe? fEXPFIN;
        //[Persistent("RGP_EXPUCTO")]
        public SouhlasAnoNe? EXPORTOVAT_DO_FINANCI
        {
            get { return fEXPFIN; }
            set
            {
              fEXPFIN = value; 
            }
        }

        [XmlIgnore]
        internal string RGP_EXPUCTO
        {
          get
          {
            if (fEXPFIN == null)
              return string.Empty;
            else
              return fEXPFIN.Value.ToString();
          }
          set { }
        }

        [XmlIgnore]
        internal string RGP_VRATKA { get; set; }

        [XmlIgnore]
        internal string RGP_TYPSANKCE { get; set; }   

        [XmlIgnore]
        internal decimal RGP_PEVNACASTKA { get; set; }

        [XmlIgnore]
        internal decimal RGP_NASOBEK { get; set; }

        [XmlIgnore]
        internal string RGP_PERNAS { get; set; }      

        [XmlElement("POZNAMKA")]
        public string RGP_POZNAMKA { get; set; }

        [XmlIgnore]
        internal string LOGIN { get; set; }

        [XmlIgnore]
        internal DateTime LASTUPDATE { get; set; }

        [XmlIgnore]
        internal string RGP_TERMPLAC { get; set; }

        [XmlIgnore]
        internal decimal RGP_POCDNU { get; set; }

        [XmlElement("PUVODNI_VS")]
        public decimal? RGP_OLDVS { get; set; }


        [XmlElement("PLATCE_ID")]
        public int RGP_ID { get; set; }

        /// <summary>
        /// pokud je zadan, je kontrolovana jeho spravnost s
        /// </summary>
        /// <returns> vraci VS platce </returns>
        public string VS { get; set; }   //pozadavek Ftt Technologies (Zlin) - vkladaji VS

        [XmlIgnore]
        private DateTime ENTRYDATE { get; set; }

        [XmlIgnore]
        internal int RGP_EA { get; set; }


        [XmlElement("POPLATEK")]
        public decimal RGP_POPLATEK { get; set; }

        [XmlElement("PERIODA")]
        public string RGP_PER { get; set; }

        [XmlIgnore]
        internal string RGP_ICO { get; set; }

        public int OSOBA_ID { get; set; }

        [XmlElement("DOPLKOD")]
        public string RGP_DOPLKOD { get; set; }

        public List<RADEK_DPH> RADKY_DPH;

        internal Session sesna;
        public PLATCE(Session session)
        {
          sesna = session;
          RADKY_DPH = new List<RADEK_DPH>();
        }

        public PLATCE()
        {
          RADKY_DPH = new List<RADEK_DPH>();
        }

 }
}

