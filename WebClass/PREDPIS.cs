using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using System.Xml.Serialization;

namespace PoplWS
{
  public class PREDPIS : PREDPISBase
  {
      internal Session sesna;

        public int PLATCE_ID { get; set; }

        private int _PRPL_PORPER;
        [XmlElement("PORADI_PERIODY")]
        public int PRPL_PORPER
        {
            get
            {
                if (_PRPL_PORPER == 0)
                    return 1;
                else
                    return _PRPL_PORPER;
            }
            set { _PRPL_PORPER = value; }
        }

        SouhlasAnoNe? fEXPFIN;
        public SouhlasAnoNe? EXPORTOVAT_DO_FINANCI
        {
            get { return fEXPFIN; }
            set
            {
                fEXPFIN = value; 
            }
        }

        [XmlIgnore]
        private string PRPL_EXPFIN
        {
            get
            {
                if (fEXPFIN == null)
                    return SouhlasAnoNe.NE.ToString();
                else
                    return fEXPFIN.Value.ToString();
            }
            set { }
        }

      public PREDPIS(Session session)
        {
          sesna = session;
        }

      public PREDPIS()
        {
        }

  }

  public class PREDPISBaseUhr : PREDPISBase
  {
      public decimal KC_UHRAZENO { get; set; }
      public string TYP_NAZEV { get; set; }
      public int POPLATEK_KOD { get; set; }
      public string POPLATEK_NAZEV { get; set; }
  }
}