using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml.Serialization;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;

namespace PoplWS
{

  public class UHRADA_PREDPISU_RESP : UHRADY_RESP
  {
    public decimal PREDPIS_KC { get; set; }
    public decimal PREDPIS_KC_UHRAZENO { get; set; }
  }

  public class UHRADY_NEW_RESP : UHRADY_RESP
  {
    public string DAVKA { get; set; }
  }

  
  internal class KONTROLA_POPLATKU
  {
    private Session sesna;
    private int ExtApp;
    private XPCollection<P_EXTAPP_POPL> EAP = null;
    private XPCollection<P_EXTAPP> EA = null;

    internal KONTROLA_POPLATKU(Session sesna, int ExtApp)
    {
      this.sesna = sesna;
      this.ExtApp = ExtApp;
    }

    /// <summary>
    /// est app. existuje
    /// </summary>
    /// <returns></returns>
    internal bool EAexist()
    {
      P_EXTAPP EA = sesna.GetObjectByKey<P_EXTAPP>(ExtApp);
      return (EA != null);
    }

    internal bool existPravoNadPoplatkem(decimal Poplatek)
    {

      if (EAP == null)
      { 
         EAP = new XPCollection<P_EXTAPP_POPL>
                                    (sesna, CriteriaOperator.Parse("CompoundKey1.ID = ? ", ExtApp));

      } 

      
      EAP.Filter = (CriteriaOperator.Parse("CompoundKey1.POPLATEK = ?", Poplatek));

      return (EAP.Count != 0);
    }

  }


  public class UHRADY_RESP
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

    public List<PLATBA> UHRADY;

    public UHRADY_RESP()
    {
      UHRADY = new List<PLATBA>();
    }

  }


  public class PLATBA
  {
    [XmlIgnore]
    internal int PLATBA_ID { get; set; }

    /// <summary>
    /// varibilni symbol
    /// </summary>
    [XmlElement("VS")]
    public string PLATBA_VS { get; set; }

    [XmlElement("DATUM_UHRADY")]
    public DateTime PLATBA_PLDATE { get; set; }

    [XmlElement("KC")]
    public decimal PLATBA_PLKC { get; set; }

    [XmlElement("DATUM_NA_UCET")]
    public DateTime PLATBA_NAUCETDNE { get; set; }

    [XmlElement("SS")]
    public decimal PLATBA_SS { get; set; }  

    [XmlElement("BANKA_KOD")]
    public string PLATBA_BANKSPOJ { get; set; }

    [XmlElement("BANKA_UCET")]
    public string PLATBA_BANKU { get; set; }

    [XmlElement("ZAPLATIL")]
    public string PLATBA_PLATCE { get; set; }

    string fPLATBA_DOKLAD;
    [XmlElement("DOKLAD")]
    public string PLATBA_DOKLAD
    {
      get 
      { if (PLATBA_POKLDOK != null)
          return PLATBA_POKLDOK.ToString();
      else
        return fPLATBA_DOKLAD;
      }
      set { fPLATBA_DOKLAD = value;  }
    }

    [XmlIgnore]
    internal int? PLATBA_POKLDOK { get; set; }

    [XmlElement("POZNAMKA")]
    public string PLATBA_POZNAMKA { get; set; }
  }




}