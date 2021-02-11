using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace PoplWS
{
    public class PLATCE_RESP : PLATCE
    {

        /// <summary>
        ///  pokud akce probehla bez chyb = OK
        /// </summary>
        [System.Xml.Serialization.XmlAttribute("result")]
        public Result result { get; set; }

        /// <summary>
        /// zda zaznam existoval, nebo byl vlozen
        /// </summary>
        [System.Xml.Serialization.XmlAttribute("status")]
        public Status status { get; set; }

        public string ERRORMESS { get; set; }

    }

    public class SLEVA
    {
        public string VS { get; set; }

        public decimal SLEVA_KC { get; set; }

    }

    public class POPLATNIK_SLEVA_RESP : POPLWS_RESPONSE
    {
        public int ZPRACOVANO { get; set; }
    }

    public class POPLATNIK_SLEVA : POPLWS_RESPONSE
    {

        public string DAVKA_ID { get; set; }

        public decimal POPLATEK { get; set; }

        public string PERIODA { get; set; }

        public decimal ROK { get; set; }  //v jakem roce bude sleva uplatnena

        public DateTime VYSTAVENO_DNE { get; set; }

        public SLEVA[] SLEVA { get; set; }

    }


    public class POPLATNIK : ZASTUPCE
    {
        [XmlElement("DLUH")]
        public decimal LIKEVS_DLUH { get; set; }

        [XmlElement("POVINNOST_KC")]
        public decimal RGP_KCROK { get; set; }
    }

    public class ZASTUPCE
    {
        public string ADRESA { get; set; }
        public string NAZEV { get; set; }
        public string VS { get; set; }
        public int ROK_NAROZENI { get; set; }
        public string PERIODA { get; set; }
    }

    public class PLATCE2
    {

        public ZASTUPCE ZASTUPCE { get; set; }

        public POPLATNIK POPLATNIK { get; set; }

    }

    public class PLATCI_RESP : POPLWS_RESPONSE
    {
        internal PLATCI_RESP()
        {
            DAVKA_ID = Guid.NewGuid().ToString();
            PLATNOST_DNI = 30;
        }
        
        [System.Xml.Serialization.XmlElement("DAVKA_ID")]
        public string DAVKA_ID { get; set; }

        public int PLATNOST_DNI { get; set; }

        [XmlElement("POPLATEK")]
        public decimal RGP_POPLATEK { get; set; }

        [XmlElement("PERIODA")]
        public string RGP_PER { get; set; }

        public List<PLATCE2> PLATCI = new List<PLATCE2>();
    }


}