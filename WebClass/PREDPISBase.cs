using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Xpo;
using System.Xml.Serialization;

namespace PoplWS
{
    public class PREDPISBase
    {
        internal Session sesna;

        public PREDPISBase(Session session)
        {
            sesna = session;
            RADKY_DPH = new List<RADEK_DPH>();
        }

        public PREDPISBase()
        {
            RADKY_DPH = new List<RADEK_DPH>();
        }


        [XmlElement("PREDPIS_ID")]
        public int PRPL_ID { get; set; }  

        [XmlElement("ROK")]
        public decimal PRPL_ROK { get; set; }

        [XmlIgnore]   
        internal string PRPL_RECORD { get; set; }

        [XmlIgnore]
        internal decimal PRPL_PORSANKCE { get; set; }

        [XmlElement("VS")]
        public string PRPL_VS { get; set; }

        [XmlElement("KC")]
        public decimal PRPL_PREDPIS { get; set; }
        [XmlIgnore]
        internal decimal PRPL_PROCSANKCE { get; set; }
        [XmlIgnore]
        internal decimal PRPL_SANKCE { get; set; }

        [XmlIgnore]
        internal string PRPL_VYSTUP { get; set; }

        [XmlIgnore]
        internal string PRPL_TISK { get; set; }

        DateTime _vystavenoDne;
        [XmlElement("VYSTAVENO_DNE")]
        public DateTime PRPL_VYSTAVENO 
        { 
            get { return _vystavenoDne; }
            set { _vystavenoDne = value.Date;} 
        }

        DateTime _splatnoDne;
        [XmlElement("SPLATNO_DNE")]
        public DateTime PRPL_SPLATNO
        { 
            get { return _splatnoDne; }
            set { _splatnoDne = value.Date; } 
        }

        [XmlIgnore]
        public decimal? PRPL_DAVKA { get; set; }


        [XmlIgnore]
        internal string PRPL_EXPORTOVANO { get; set; }
        [XmlIgnore]
        internal decimal PRPL_UCETMESIC { get; set; }
        [XmlIgnore]
        internal decimal PRPL_UCETROK { get; set; }
        [XmlIgnore]
        internal string PRPL_TYPSANKCE { get; set; }
        [XmlIgnore]
        internal decimal PRPL_PEVNACASTKA { get; set; }
        [XmlIgnore]
        internal decimal PRPL_NASOBEK { get; set; }
        [XmlIgnore]
        internal string PRPL_PERNAS { get; set; }
        [XmlIgnore]
        internal DateTime? PRPL_SANKCEDO { get; set; }
        [XmlIgnore]
        internal string PRPL_STAVSANKCE { get; set; }
        [XmlIgnore]
        internal int? PRPL_IDPREDPISU { get; set; }
        [XmlIgnore]
        internal string PRPL_KODROZUCT { get; set; }
        [XmlIgnore]
        internal decimal PRPL_SPAROVANO { get; set; }
        [XmlIgnore]
        internal string LOGIN { get; set; }
        [XmlIgnore]
        internal DateTime LASTUPDATE { get; set; }
        [XmlIgnore]
        internal decimal PRPL_SPAROVANO_MINUSEM { get; set; }
        [XmlIgnore]
        internal string PRPL_EXPSIPO { get; set; }

        [XmlIgnore]
        internal decimal? PRPL_OLDVS { get; set; }

        [XmlElement("POZNAMKA")]
        public string PRPL_POZNAMKA { get; set; }

        [XmlIgnore]
        internal int PRPL_PR { get; set; }
        [XmlIgnore]
        internal int PRPL_IDSTORNO { get; set; }

        [XmlIgnore]
        internal string PRPL_VYMAHAT { get; set; }

        [XmlIgnore]
        internal string PRPL_VYMAHANO { get; set; }

        [XmlElement("SS_SYMBOL")]
        public decimal? PRPL_SS { get; set; }

        [XmlIgnore]
        internal decimal PRPL_OBDOBIMES { get; set; }
        [XmlIgnore]
        internal decimal PRPL_OBDOBIROK { get; set; }
        [XmlIgnore]
        internal DateTime ENTRYDATE { get; set; }
        [XmlIgnore]
        internal string ENTRYLOGIN { get; set; }

        [XmlElement("PREDPIS_TYP")]  
        public short PRPL_TYPDANE { get; set; }

        [XmlIgnore]
        internal int? PRPL_USERTYP { get; set; }

        [XmlElement("IDENTIFIKACE")]
        public string PRPL_IDENTIFIKACE { get; set; }

        [XmlIgnore]
        internal string PRPL_EXTVS { get; set; }

        [XmlElement("DATUM_UZP")]
        public DateTime? PRPL_UZP { get; set; }

        [XmlIgnore]
        internal int? PRPL_DDCIS { get; set; }
        [XmlIgnore]
        internal DateTime? PRPL_NPM { get; set; }
        [XmlIgnore]
        internal DateTime? PRPL_ROZHODNUTI { get; set; }
        [XmlIgnore]
        internal DateTime? PRPL_VYKONATELNOST { get; set; }
        [XmlIgnore]
        internal decimal PRPL_ODPIS { get; set; }
        [XmlIgnore]
        internal int? PRPL_POKLDOK { get; set; }
        [XmlIgnore]
        internal int? PRPL_PREVID { get; set; }

        [XmlElement("POPLATEK_TYP")]  
        internal decimal PRPL_POPLATEK { get; set; }
        [XmlIgnore]
        internal string PRPL_PER { get; set; }
        [XmlIgnore]
        internal string PRPL_ICO { get; set; }
        [XmlIgnore]
        internal string PRPL_DOPLKOD { get; set; }
        [XmlIgnore]
        internal int PRPL_EA { get; set; }

        public List<RADEK_DPH> RADKY_DPH;

    }
}
