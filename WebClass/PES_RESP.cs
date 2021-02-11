using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoplWS
{
  public class DEJPSY_RESP : POPLWS_RESPONSE
  {
    public List<PLATCE_PSA> PLATCI = new List<PLATCE_PSA>();
  }

  public class PLATCE_PSA
  {
    public OSOBA OSOBA;
    public bool DLUH;
    public List<PES> PSI = new List<PES>();
  }

  public class GET_PES_PARAMS
  {
      public int? PES_ZNAMKA;
      public string PES_ZNAMKA_CIP;
      public string PES_TETOVANI;
      public string PES_CIP;
      public string PES_JMENO;
      public int? PES_PLEMENO_KOD;
      public string POPLATNIK_PRIJMENI;
      public string POPLATNIK_FIRMA;
      public string POPLATNIK_ULICE;
      public int? POPLATNIK_CP;
  }

  public class GET_PLATCE_PARAMS
  {
      public string VS;
      public decimal POPLATEK;
      public string PERIODA;
  }

  public class POPLWS_RESPONSE
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

}