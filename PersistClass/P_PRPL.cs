using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using System.Xml.Serialization;

namespace PoplWS
{
  [Indices(@"CompoundKey;PRPL_VS", "CompoundKey;PRPL_ROK;PRPL_PORPER;PRPL_RECORD", "CompoundKey;PRPL_ROK;PRPL_PORPER;PRPL_RECORD;PRPL_VYSTUP")]
  public partial class P_PRPL : XPLiteObject
  {
    int fPRPL_ID;
    [Key, Persistent]
    public int PRPL_ID 
    {
      get { return fPRPL_ID; }
      set { SetPropertyValue<int>("PRPL_ID", ref fPRPL_ID, value); }
    }

    /*int fPRPL_ID;
    public int PRPL_ID
    {
      get { return fPRPL_ID; }
      set { SetPropertyValue<int>("PRPL_ID", ref fPRPL_ID, value); }
    }
    */

    decimal fPRPL_ROK;
    public decimal PRPL_ROK
    {
      get { return fPRPL_ROK; }
      set { SetPropertyValue<decimal>("PRPL_ROK", ref fPRPL_ROK, value); }
    }

    int fPRPL_PORPER;
    public int PRPL_PORPER
    {
      get { return fPRPL_PORPER; }
      set { SetPropertyValue<int>("PRPL_PORPER", ref fPRPL_PORPER, value); }
    }

    string fPRPL_RECORD;
    [Size(1)]
    public string PRPL_RECORD
    {
      get { return fPRPL_RECORD; }
      set { SetPropertyValue<string>("PRPL_RECORD", ref fPRPL_RECORD, value); }
    }

    decimal fPRPL_PORSANKCE;
    public decimal PRPL_PORSANKCE
    {
      get { return fPRPL_PORSANKCE; }
      set { SetPropertyValue<decimal>("PRPL_PORSANKCE", ref fPRPL_PORSANKCE, value); }
    }

    string fPRPL_VS;
    [Indexed(Name = @"PRPL_VS")]
    [Size(18)]
    public string PRPL_VS
    {
      get { return fPRPL_VS; }
      set { SetPropertyValue<string>("PRPL_VS", ref fPRPL_VS, value); }
    }
    
    decimal fPRPL_PREDPIS;
    public decimal PRPL_PREDPIS
    {
      get { return fPRPL_PREDPIS; }
      set { SetPropertyValue<decimal>("PRPL_PREDPIS", ref fPRPL_PREDPIS, value); }
    }
    decimal fPRPL_PROCSANKCE;
    public decimal PRPL_PROCSANKCE
    {
      get { return fPRPL_PROCSANKCE; }
      set { SetPropertyValue<decimal>("PRPL_PROCSANKCE", ref fPRPL_PROCSANKCE, value); }
    }
    decimal fPRPL_SANKCE;
    public decimal PRPL_SANKCE
    {
      get { return fPRPL_SANKCE; }
      set { SetPropertyValue<decimal>("PRPL_SANKCE", ref fPRPL_SANKCE, value); }
    }
    
    string fPRPL_VYSTUP;
    [Size(10)]
    public string PRPL_VYSTUP
    {
      get { return fPRPL_VYSTUP; }
      set { SetPropertyValue<string>("PRPL_VYSTUP", ref fPRPL_VYSTUP, value); }
    }
    
    string fPRPL_TISK;
    [Size(3)]
    public string PRPL_TISK
    {
      get { return fPRPL_TISK; }
      set { SetPropertyValue<string>("PRPL_TISK", ref fPRPL_TISK, value); }
    }
    
    DateTime fPRPL_VYSTAVENO;
    public DateTime PRPL_VYSTAVENO
    {
      get { return fPRPL_VYSTAVENO; }
      set { SetPropertyValue<DateTime>("PRPL_VYSTAVENO", ref fPRPL_VYSTAVENO, value); }
    }
    
    DateTime fPRPL_SPLATNO;
    public DateTime PRPL_SPLATNO
    {
      get { return fPRPL_SPLATNO; }
      set { SetPropertyValue<DateTime>("PRPL_SPLATNO", ref fPRPL_SPLATNO, value); }
    }
    
    decimal? fPRPL_DAVKA;
    public decimal? PRPL_DAVKA
    {
      get { return fPRPL_DAVKA; }
      set { SetPropertyValue<decimal?>("PRPL_DAVKA", ref fPRPL_DAVKA, value); }
    }
    
    string fPRPL_EXPFIN;
    [Size(3)]
    public string PRPL_EXPFIN
    {
      get { return fPRPL_EXPFIN; }
      set { SetPropertyValue<string>("PRPL_EXPFIN", ref fPRPL_EXPFIN, value); }
    }
    
    string fPRPL_EXPORTOVANO;
    [Size(3)]
    public string PRPL_EXPORTOVANO
    {
      get { return fPRPL_EXPORTOVANO; }
      set { SetPropertyValue<string>("PRPL_EXPORTOVANO", ref fPRPL_EXPORTOVANO, value); }
    }
    
    decimal fPRPL_UCETMESIC;
    public decimal PRPL_UCETMESIC
    {
      get { return fPRPL_UCETMESIC; }
      set { SetPropertyValue<decimal>("PRPL_UCETMESIC", ref fPRPL_UCETMESIC, value); }
    }
    
    decimal fPRPL_UCETROK;
    public decimal PRPL_UCETROK
    {
      get { return fPRPL_UCETROK; }
      set { SetPropertyValue<decimal>("PRPL_UCETROK", ref fPRPL_UCETROK, value); }
    }
    
    string fPRPL_TYPSANKCE;
    public string PRPL_TYPSANKCE
    {
      get { return fPRPL_TYPSANKCE; }
      set { SetPropertyValue<string>("PRPL_TYPSANKCE", ref fPRPL_TYPSANKCE, value); }
    }
    
    decimal fPRPL_PEVNACASTKA;
    public decimal PRPL_PEVNACASTKA
    {
      get { return fPRPL_PEVNACASTKA; }
      set { SetPropertyValue<decimal>("PRPL_PEVNACASTKA", ref fPRPL_PEVNACASTKA, value); }
    }
    
    decimal fPRPL_NASOBEK;
    public decimal PRPL_NASOBEK
    {
      get { return fPRPL_NASOBEK; }
      set { SetPropertyValue<decimal>("PRPL_NASOBEK", ref fPRPL_NASOBEK, value); }
    }

    string fPRPL_PERNAS;
    public string PRPL_PERNAS
    {
      get { return fPRPL_PERNAS; }
      set { SetPropertyValue<string>("PRPL_PERNAS", ref fPRPL_PERNAS, value); }
    }
    
    DateTime fPRPL_SANKCEDO;
    public DateTime PRPL_SANKCEDO
    {
      get { return fPRPL_SANKCEDO; }
      set { SetPropertyValue<DateTime>("PRPL_SANKCEDO", ref fPRPL_SANKCEDO, value); }
    }
    
    string fPRPL_STAVSANKCE;
    [Size(1)]
    public string PRPL_STAVSANKCE
    {
      get { return fPRPL_STAVSANKCE; }
      set { SetPropertyValue<string>("PRPL_STAVSANKCE", ref fPRPL_STAVSANKCE, value); }
    }
    
    int? fPRPL_IDPREDPISU;
    [Indexed(@"PRPL_RECORD", Name = @"PRPL_ix4")]
    public int? PRPL_IDPREDPISU
    {
      get { return fPRPL_IDPREDPISU; }
      set { SetPropertyValue<int?>("PRPL_IDPREDPISU", ref fPRPL_IDPREDPISU, value); }
    }
    
    string fPRPL_KODROZUCT;
    public string PRPL_KODROZUCT
    {
      get { return fPRPL_KODROZUCT; }
      set { SetPropertyValue<string>("PRPL_KODROZUCT", ref fPRPL_KODROZUCT, value); }
    }
    
    decimal fPRPL_SPAROVANO;
    public decimal PRPL_SPAROVANO
    {
      get { return fPRPL_SPAROVANO; }
      set { SetPropertyValue<decimal>("PRPL_SPAROVANO", ref fPRPL_SPAROVANO, value); }
    }
    
    string fLOGIN;
    [Size(30)]
    public string LOGIN
    {
      get { return fLOGIN; }
      set { SetPropertyValue<string>("LOGIN", ref fLOGIN, value); }
    }
    
    DateTime fLASTUPDATE;
    public DateTime LASTUPDATE
    {
      get { return fLASTUPDATE; }
      set { SetPropertyValue<DateTime>("LASTUPDATE", ref fLASTUPDATE, value); }
    }
    
    decimal fPRPL_SPAROVANO_MINUSEM;
    public decimal PRPL_SPAROVANO_MINUSEM
    {
      get { return fPRPL_SPAROVANO_MINUSEM; }
      set { SetPropertyValue<decimal>("PRPL_SPAROVANO_MINUSEM", ref fPRPL_SPAROVANO_MINUSEM, value); }
    }
    
    string fPRPL_EXPSIPO;
    [Size(3)]
    public string PRPL_EXPSIPO
    {
      get { return fPRPL_EXPSIPO; }
      set { SetPropertyValue<string>("PRPL_EXPSIPO", ref fPRPL_EXPSIPO, value); }
    }
    
    decimal? fPRPL_OLDVS;
    public decimal? PRPL_OLDVS
    {
      get { return fPRPL_OLDVS; }
      set { SetPropertyValue<decimal?>("PRPL_OLDVS", ref fPRPL_OLDVS, value); }
    }
    
    string fPRPL_POZNAMKA;
    [Size(110)]
    public string PRPL_POZNAMKA
    {
      get { return fPRPL_POZNAMKA; }
      set { SetPropertyValue<string>("PRPL_POZNAMKA", ref fPRPL_POZNAMKA, value); }
    }
    
    int fPRPL_PR;
    public int PRPL_PR
    {
      get { return fPRPL_PR; }
      set { SetPropertyValue<int>("PRPL_PR", ref fPRPL_PR, value); }
    }

    int fPRPL_IDSTORNO;
    public int PRPL_IDSTORNO
    {
      get { return fPRPL_IDSTORNO; }
      set { SetPropertyValue<int>("PRPL_IDSTORNO", ref fPRPL_IDSTORNO, value); }
    }

    string fPRPL_VYMAHAT;
    public string PRPL_VYMAHAT
    {
      get { return fPRPL_VYMAHAT; }
      set { SetPropertyValue<string>("PRPL_VYMAHAT", ref fPRPL_VYMAHAT, value); }
    }

    string fPRPL_VYMAHANO;
    public string PRPL_VYMAHANO
    {
      get { return fPRPL_VYMAHANO; }
      set { SetPropertyValue<string>("PRPL_VYMAHANO", ref fPRPL_VYMAHANO, value); }
    }

    decimal fPRPL_SS;
    public decimal PRPL_SS
    {
      get { return fPRPL_SS; }
      set { SetPropertyValue<decimal>("PRPL_SS", ref fPRPL_SS, value); }
    }

    decimal fPRPL_OBDOBIMES;
    public decimal PRPL_OBDOBIMES
    {
      get { return fPRPL_OBDOBIMES; }
      set { SetPropertyValue<decimal>("PRPL_OBDOBIMES", ref fPRPL_OBDOBIMES, value); }
    }

    decimal fPRPL_OBDOBIROK;
    public decimal PRPL_OBDOBIROK
    {
      get { return fPRPL_OBDOBIROK; }
      set { SetPropertyValue<decimal>("PRPL_OBDOBIROK", ref fPRPL_OBDOBIROK, value); }
    }

    DateTime fENTRYDATE;
    [IgnoreInsertUpdate]
    public DateTime ENTRYDATE
    {
      get { return fENTRYDATE; }
      set { SetPropertyValue<DateTime>("ENTRYDATE", ref fENTRYDATE, value); }
    }

    string fENTRYLOGIN;
    [Size(30)]
    [IgnoreInsertUpdate]
    public string ENTRYLOGIN
    {
      get { return fENTRYLOGIN; }
      set { SetPropertyValue<string>("ENTRYLOGIN", ref fENTRYLOGIN, value); }
    }

    P_C_TYPDANE fPRPL_TYPDANE;
    [Association(@"P_PRPLReferencesP_C_TYPDANE")]
    public P_C_TYPDANE PRPL_TYPDANE
    {
      get { return fPRPL_TYPDANE; }
      set { SetPropertyValue<P_C_TYPDANE>("PRPL_TYPDANE", ref fPRPL_TYPDANE, value); }
    }

    int? fPRPL_USERTYP;
    public int? PRPL_USERTYP
    {
      get { return fPRPL_USERTYP; }
      set { SetPropertyValue<int?>("PRPL_USERTYP", ref fPRPL_USERTYP, value); }
    }

    string fPRPL_IDENTIFIKACE;
    [Indexed(Name = @"P_PRPL_ix7")]
    [Size(32)]
    public string PRPL_IDENTIFIKACE
    {
      get { return fPRPL_IDENTIFIKACE; }
      set { SetPropertyValue<string>("PRPL_IDENTIFIKACE", ref fPRPL_IDENTIFIKACE, value); }
    }
   
    string fPRPL_EXTVS;
    [Size(10)]
    public string PRPL_EXTVS
    {
      get { return fPRPL_EXTVS; }
      set { SetPropertyValue<string>("PRPL_EXTVS", ref fPRPL_EXTVS, value); }
    }
    
    DateTime? fPRPL_UZP;
    public DateTime? PRPL_UZP
    {
      get { return fPRPL_UZP; }
      set { SetPropertyValue<DateTime?>("PRPL_UZP", ref fPRPL_UZP, value); }
    }

    int? fPRPL_DDCIS;
    public int? PRPL_DDCIS
    {
      get { return fPRPL_DDCIS; }
      set { SetPropertyValue<int?>("PRPL_DDCIS", ref fPRPL_DDCIS, value); }
    }

    DateTime? fPRPL_NPM;
    public DateTime? PRPL_NPM
    {
      get { return fPRPL_NPM; }
      set { SetPropertyValue<DateTime?>("PRPL_NPM", ref fPRPL_NPM, value); }
    }
    
    DateTime? fPRPL_ROZHODNUTI;
    public DateTime? PRPL_ROZHODNUTI
    {
      get { return fPRPL_ROZHODNUTI; }
      set { SetPropertyValue<DateTime?>("PRPL_ROZHODNUTI", ref fPRPL_ROZHODNUTI, value); }
    }
    
    DateTime? fPRPL_VYKONATELNOST;
    public DateTime? PRPL_VYKONATELNOST
    {
      get { return fPRPL_VYKONATELNOST; }
      set { SetPropertyValue<DateTime?>("PRPL_VYKONATELNOST", ref fPRPL_VYKONATELNOST, value); }
    }
    
    decimal fPRPL_ODPIS;
    public decimal PRPL_ODPIS
    {
      get { return fPRPL_ODPIS; }
      set { SetPropertyValue<decimal>("PRPL_ODPIS", ref fPRPL_ODPIS, value); }
    }
    
    int? fPRPL_POKLDOK;
    public int? PRPL_POKLDOK
    {
      get { return fPRPL_POKLDOK; }
      set { SetPropertyValue<int?>("PRPL_POKLDOK", ref fPRPL_POKLDOK, value); }
    }
    
    int? fPRPL_PREVID;
    public int? PRPL_PREVID
    {
      get { return fPRPL_PREVID; }
      set { SetPropertyValue<int?>("PRPL_PREVID", ref fPRPL_PREVID, value); }
    }

    int fPRPL_EA;
    public int PRPL_EA
    {
      get { return fPRPL_EA; }
      set { SetPropertyValue<int>("PRPL_EA", ref fPRPL_EA, value); }
    }

    string fPRPL_EXTDOKLAD;
    public string PRPL_EXTDOKLAD
    {
        get { return fPRPL_EXTDOKLAD; }
        set { SetPropertyValue<string>("PRPL_EXTDOKLAD", ref fPRPL_EXTDOKLAD, value); }
    }

    [NonPersistent]
    public decimal USER_PREDPIS
    { get {return PRPL_PREDPIS + PRPL_SANKCE;}  }

    public struct CompositePrplField1Struct
    {
      [Persistent("PRPL_POPLATEK")]
      public decimal PRPL_POPLATEK;
      [Size(1)]
      [Persistent("PRPL_PER")]
      public string PRPL_PER;
      [Size(15)]
      [Persistent("PRPL_ICO")]
      public string PRPL_ICO;
      [Size(25)]
      [Persistent("PRPL_DOPLKOD")]
      public string PRPL_DOPLKOD;
    }

    [Indexed(Name = @"P_PRPL_ix6")]

    [Persistent]
    public CompositePrplField1Struct CompoundKey;

    public P_PRPL(Session session) : base(session) { }


    [Association(@"PRPL_DPHreferencesPRPL", typeof(P_PRPL_DPH))]
    public XPCollection<P_PRPL_DPH> P_PRPL_DPH
    {
      get { return GetCollection<P_PRPL_DPH>("P_PRPL_DPH"); }
    }


    protected override void OnSaving()
    {
      base.OnSaving();

      if (this.Session.IsNewObject(this)) 
      {
          PRPL_OBDOBIMES = -1;
          PRPL_OBDOBIROK = -1;
          if (string.IsNullOrEmpty(PRPL_TISK)) PRPL_TISK = "NE";
          if (string.IsNullOrEmpty(PRPL_EXPORTOVANO)) PRPL_EXPORTOVANO = "NE";
          if (string.IsNullOrEmpty(PRPL_EXPFIN)) PRPL_EXPFIN = "NE";
          if (string.IsNullOrEmpty(PRPL_TYPSANKCE)) PRPL_TYPSANKCE = "D";
          if (string.IsNullOrEmpty(PRPL_PERNAS)) PRPL_PERNAS = "D";
          if (string.IsNullOrEmpty(PRPL_STAVSANKCE)) PRPL_STAVSANKCE = " ";

          if (string.IsNullOrEmpty(ENTRYLOGIN))
          {
              DBValue dbv = DBValue.Instance(this.Session);
              this.ENTRYDATE = dbv.DBSysDateTime;
              this.ENTRYLOGIN = dbv.DBUserName;
          }

          this.LOGIN = this.ENTRYLOGIN;
          this.LASTUPDATE = this.ENTRYDATE;

          if (string.IsNullOrEmpty(PRPL_EXPSIPO)) fPRPL_EXPSIPO = "NE";
          if (string.IsNullOrEmpty(PRPL_VYMAHAT)) PRPL_VYMAHAT = "N";
          if (string.IsNullOrEmpty(PRPL_VYMAHANO)) PRPL_VYMAHANO = "N";
      }
    }

  }
}