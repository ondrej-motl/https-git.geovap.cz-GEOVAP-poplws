using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml.Serialization;

namespace PoplWS
{
  public class PES
  {
    [XmlIgnore]
    public decimal PES_CIS_PRIZNANI { get; set; }
    [XmlElement("JMENO")]
    public string PES_JMENO { get; set; }
    [XmlIgnore]
    public decimal PES_PLEMENO { get; set; }
    public string PLEMENO { get; set; }
    [XmlElement("POPIS")]
    public string PES_POPIS { get; set; }
    [XmlElement("ZNAMKA")]
    public decimal PES_ZNAMKA { get; set; }
    [XmlElement("CIP_ZNAMKA")]
    public string PES_CIP { get; set; }
    [XmlElement("NAROZEN")]
    public DateTime? PES_NAROZEN { get; set; }
    [XmlElement("DRZEN_OD")]
    public DateTime PES_DRZEN_OD { get; set; }
    [XmlIgnore]
    public DateTime PES_POPL_OD { get; set; }
    [XmlIgnore]
    public DateTime? PES_POPL_DO { get; set; }
    [XmlIgnore]
    public DateTime? PES_SLEVA_OD { get; set; }
    [XmlIgnore]
    public DateTime? PES_SLEVA_DO { get; set; }
    [XmlIgnore]
    public DateTime? PES_ZMENA_OD { get; set; }
    [XmlElement("OCKOVAN")]
    public DateTime? PES_OCKOVAN { get; set; }
    [XmlIgnore]
    public decimal PES_UCEL { get; set; }
    [XmlIgnore]
    public decimal PES_SAZPOPL_SAZBA { get; set; }
    [XmlElement("POHLAVI")]
    public string PES_POHLAVI { get; set; }
    [XmlIgnore]
    public string PES_OSVOBOZEN { get; set; }
    [XmlElement("DRZEN_DO")]
    public DateTime? PES_DRZEN_DO { get; set; }
    [XmlIgnore]
    public string PES_POZNAMKA { get; set; }
    [XmlElement("TETOVANI")]
    public string PES_TET { get; set; }
    [XmlElement("CIP_TET")]
    public string PES_CIP2 { get; set; }
    [XmlElement("ZNAMKA_VYDANA_DNE")]
    public DateTime? PES_ZNAMKA_VYD { get; set; }
    [XmlIgnore]
    public decimal PES_BARVA { get; set; }
    public string BARVA { get; set; }
    [XmlIgnore]
    public DateTime? PES_SLEVAC_OD { get; set; }
    [XmlIgnore]
    public DateTime? PES_SLEVAC_DO { get; set; }
    [XmlIgnore]
    public decimal PES_SLEVAC_KC { get; set; }
    [XmlIgnore]
    public int PES_UQ_ID { get; set; }
    [XmlIgnore]
    public string ENTRYLOGIN { get; set; }
    [XmlIgnore]
    public DateTime? ENTRYDATE { get; set; }
    [XmlIgnore]
    public string LASTLOGIN { get; set; }
    [XmlIgnore]
    public DateTime? LASTUPDATE { get; set; }
  }
}