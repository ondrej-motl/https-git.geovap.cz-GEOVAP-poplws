using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml.Serialization;

namespace PoplWS
{
  public class PREDPIS_RESP : PREDPIS
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

  public class PREDPISY_RESP
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

      public List<PREDPISBaseUhr> PREDPISY { get; set; }
      
      public _BANKOVNI_SPOJENI BANKOVNI_SPOJENI { get; set; }

      public PREDPISY_RESP()
      {
          PREDPISY = new List<PREDPISBaseUhr>();
          BANKOVNI_SPOJENI = new _BANKOVNI_SPOJENI();
      }

      public class _BANKOVNI_SPOJENI
      {
          public string CISLO_UCTU { get; set; }
          public string SMER_KOD_BANKY { get; set; }
          public string IBAN { get; set; }
      }

  }

}