using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PoplWS
{
    public enum Result { ERROR, OK }
    public enum Status { ERROR, INSERTED, EXISTS, NOTEXISTS, TOOMANY }

    public class OSOBA_RESP : OSOBA
    {

        /// <summary>
        ///  pokud akce probehla bez chyb = OK
        /// </summary>
        [System.Xml.Serialization.XmlAttribute("result")]
        public Result result { get; set; }

        /// <summary>
        /// zda osoba existovala, nebo byla vlozena
        /// </summary>
        [System.Xml.Serialization.XmlAttribute("status")]
        public Status status { get; set; }

        public string ERRORMESS { get; set; }


    }

    public class DEJOSOBU_RESP
    {
      /// <summary>
      ///  pokud akce probehla bez chyb = OK
      /// </summary>
      [System.Xml.Serialization.XmlAttribute("result")]
      public Result result { get; set; }

      /// <summary>
      /// zda osoba existovala, nebo byla vlozena
      /// </summary>
      [System.Xml.Serialization.XmlAttribute("status")]
      public Status status { get; set; }

      public string ERRORMESS { get; set; }


      public List<OSOBA> OSOBY = new List<OSOBA>();

    }

}