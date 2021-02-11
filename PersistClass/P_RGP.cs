using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using System.Xml.Serialization;
using DevExpress.Data.Filtering;

namespace PoplWS
{
    public partial class P_RGP : XPLiteObject
    {

        bool fIsModified = false;

        string fRGP_PLATBA;
        [Size(10)]
        public string RGP_PLATBA
        {
            get { return fRGP_PLATBA; }
            set { SetPropertyValue<string>("RGP_PLATBA", ref fRGP_PLATBA, value); }
        }
        string fRGP_ZADANO;
        [Size(1)]
        public string RGP_ZADANO
        {
            get { return fRGP_ZADANO; }
            set { SetPropertyValue<string>("RGP_ZADANO", ref fRGP_ZADANO, value); }
        }
        decimal? fRGP_SAZBA;
        public decimal? RGP_SAZBA
        {
            get { return fRGP_SAZBA; }
            set { SetPropertyValue<decimal?>("RGP_SAZBA", ref fRGP_SAZBA, value); }
        }
        decimal fRGP_KCROK;
        public decimal RGP_KCROK
        {
            get { return fRGP_KCROK; }
            set { SetPropertyValue<decimal>("RGP_KCROK", ref fRGP_KCROK, value); }
        }
        decimal fRGP_KCSPLATKA;
        public decimal RGP_KCSPLATKA
        {
            get { return fRGP_KCSPLATKA; }
            set { SetPropertyValue<decimal>("RGP_KCSPLATKA", ref fRGP_KCSPLATKA, value); }
        }
        decimal fRGP_POSLSPLATKA;
        public decimal RGP_POSLSPLATKA
        {
            get { return fRGP_POSLSPLATKA; }
            set { SetPropertyValue<decimal>("RGP_POSLSPLATKA", ref fRGP_POSLSPLATKA, value); }
        }
        decimal fRGP_PROCSANKCE;
        public decimal RGP_PROCSANKCE
        {
            get { return fRGP_PROCSANKCE; }
            set { SetPropertyValue<decimal>("RGP_PROCSANKCE", ref fRGP_PROCSANKCE, value); }
        }
        string fRGP_VYSTUP;
        [Size(10)]
        public string RGP_VYSTUP
        {
            get { return fRGP_VYSTUP; }
            set { SetPropertyValue<string>("RGP_VYSTUP", ref fRGP_VYSTUP, value); }
        }
        DateTime fRGP_FROMDATE;
        public DateTime RGP_FROMDATE
        {
            get { return fRGP_FROMDATE; }
            set { SetPropertyValue<DateTime>("RGP_FROMDATE", ref fRGP_FROMDATE, value); }
        }
        DateTime? fRGP_TODATE;
        public DateTime? RGP_TODATE
        {
            get { return fRGP_TODATE; }
            set { SetPropertyValue<DateTime?>("RGP_TODATE", ref fRGP_TODATE, value); }
        }
        DateTime fRGP_NEXTDATESPL;
        public DateTime RGP_NEXTDATESPL
        {
            get { return fRGP_NEXTDATESPL; }
            set { SetPropertyValue<DateTime>("RGP_NEXTDATESPL", ref fRGP_NEXTDATESPL, value); }
        }
        
        int fRGP_PORVS;
        public int RGP_PORVS
        {
            get { return fRGP_PORVS; }
            set { SetPropertyValue<int>("RGP_PORVS", ref fRGP_PORVS, value); }
        }


        //[PersistentAlias("_vs")]  //https://documentation.devexpress.com/#XPO/CustomDocument2875
        string _VS = null;
        public string VS
        {
          get
          {
            if (_VS == null)
            {
              DBUtil dbu = new DBUtil(this.Session);
              _VS = dbu.GetVS(this.CompoundKey1.RGP_POPLATEK, this.RGP_PORVS);
            }
            return _VS;
          }
        }

        string fRGP_EXPUCTO;
        [Size(3)]
        public string RGP_EXPUCTO
        {
            get { return fRGP_EXPUCTO; }
            set { SetPropertyValue<string>("RGP_EXPUCTO", ref fRGP_EXPUCTO, value); }
        }

        string fRGP_VRATKA;
        [Size(3)]
        public string RGP_VRATKA
        {
            get { return fRGP_VRATKA; }
            set { SetPropertyValue<string>("RGP_VRATKA", ref fRGP_VRATKA, value); }
        }
        
        string fRGP_TYPSANKCE;
        [Size(1)]
        public string RGP_TYPSANKCE     //zmena char na string, protoze pri serializaci byla misto znaku jeho asci hodnota 
        {
            get { return fRGP_TYPSANKCE; }
            set { SetPropertyValue<string>("RGP_TYPSANKCE", ref fRGP_TYPSANKCE, value); }
        }
        decimal fRGP_PEVNACASTKA;
        public decimal RGP_PEVNACASTKA
        {
            get { return fRGP_PEVNACASTKA; }
            set { SetPropertyValue<decimal>("RGP_PEVNACASTKA", ref fRGP_PEVNACASTKA, value); }
        }
        decimal fRGP_NASOBEK;
        public decimal RGP_NASOBEK
        {
            get { return fRGP_NASOBEK; }
            set { SetPropertyValue<decimal>("RGP_NASOBEK", ref fRGP_NASOBEK, value); }
        }
        string fRGP_PERNAS;
        [Size(1)]
        public string RGP_PERNAS        //zmena char na string, protoze pri serializaci byla misto znaku jeho asci hodnota 
        {
            get { return fRGP_PERNAS; }
            set { SetPropertyValue<string>("RGP_PERNAS", ref fRGP_PERNAS, value); }
        }
        string fRGP_POZNAMKA;
        [Size(700)]
        public string RGP_POZNAMKA
        {
            get { return fRGP_POZNAMKA; }
            set { SetPropertyValue<string>("RGP_POZNAMKA", ref fRGP_POZNAMKA, value); }
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
        string fRGP_TERMPLAC;
        [Size(5)]
        public string RGP_TERMPLAC
        {
            get { return fRGP_TERMPLAC; }
            set { SetPropertyValue<string>("RGP_TERMPLAC", ref fRGP_TERMPLAC, value); }
        }
        decimal fRGP_POCDNU;
        public decimal RGP_POCDNU
        {
            get { return fRGP_POCDNU; }
            set { SetPropertyValue<decimal>("RGP_POCDNU", ref fRGP_POCDNU, value); }
        }
        decimal? fRGP_OLDVS;
        public decimal? RGP_OLDVS
        {
            get { return fRGP_OLDVS; }
            set { SetPropertyValue<decimal?>("RGP_OLDVS", ref fRGP_OLDVS, value); }
        }
        double fRGP_PREV_Z_OBDOBI;
        public double RGP_PREV_Z_OBDOBI
        {
            get { return fRGP_PREV_Z_OBDOBI; }
            set { SetPropertyValue<double>("RGP_PREV_Z_OBDOBI", ref fRGP_PREV_Z_OBDOBI, value); }
        }


        private int _rgp_id;
        [Key(true)] 
        public int RGP_ID
        {
          get { return this._rgp_id; }
          set { 
                SetPropertyValue<int>("RGP_ID", ref _rgp_id, value); 
              }
          }

        /*DateTime fENTRYDATE;
        public DateTime ENTRYDATE
        {
            get { return fENTRYDATE; }
            set { SetPropertyValue<DateTime>("ENTRYDATE", ref fENTRYDATE, value); }
        } */


        int fRGP_EA;
        public int RGP_EA
        {
          get { return fRGP_EA; }
          set { SetPropertyValue<int>("RGP_EA", ref fRGP_EA, value); }
        }


        [Association(@"DPHSPLReferencesRGP"), Aggregated]   
        public XPCollection<P_RGP_DPHSPL> P_RGP_DPHSPL
        {
            get { return GetCollection<P_RGP_DPHSPL>("P_RGP_DPHSPL"); }
        }


        [Association(@"DPHPSPLReferencesRGP"), Aggregated]
        public XPCollection<P_RGP_DPHPSPL> P_RGP_DPHPSPL
        {
            get { return GetCollection<P_RGP_DPHPSPL>("P_RGP_DPHPSPL"); }
        }

        public struct CompoundKey1Struct
        {
            [Persistent("RGP_POPLATEK")]
            public decimal RGP_POPLATEK { get; set; }
            [Size(1)]
            [Persistent("RGP_PER")]
            public string RGP_PER { get; set; }
            [Size(15)]
            [Persistent("RGP_ICO")]
            public string RGP_ICO { get; set; }
            [Size(25)]
            [Persistent("RGP_DOPLKOD")]
            public string RGP_DOPLKOD { get; set; }
        }
        [Indexed(Name = @"RGP_PK", Unique = true)]

        [Persistent]
        public CompoundKey1Struct CompoundKey1;
        public P_RGP(Session session) : base(session) { }

        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            if (!IsLoading && !IsSaving)
            {
                fIsModified = true;
            }
        }

        protected override void OnSaving() 
        {
            base.OnSaving();

            if (fIsModified)
            {

            }

            if (CompoundKey1.RGP_POPLATEK == 0) { throw new Exception("poplatek musí být vyplněn"); }
            if (CompoundKey1.RGP_PER == null) { throw new Exception("perioda musí být vyplněna"); }
            if (CompoundKey1.RGP_ICO == null) { throw new Exception("IČ/RČ musí být vyplněno"); }
            if ( this.CompoundKey1.RGP_DOPLKOD == null) { CompoundKey1.RGP_DOPLKOD = " "; }
            if (RGP_PLATBA == null) { RGP_PLATBA = " "; }
            if (RGP_ZADANO == null) { RGP_ZADANO = "R"; }
            if (RGP_VRATKA == null) { RGP_VRATKA = "ANO";  }
            RGP_SAZBA = null;

            DBUtil dbu = new DBUtil(this.Session);

#region vypocet splatky // 0.10 zjistuji pouze pocet splatek

            short pocSplatek = 0; short periodicita = 0;
            decimal kcSplatka = 0, poslSplatka = 0;
            dbu.GetRgpSplatky(RGP_KCROK, CompoundKey1.RGP_PER, ref kcSplatka, ref poslSplatka, ref pocSplatek, ref periodicita);
#endregion vypocet splatky

            DBValue dbv = DBValue.Instance(this.Session); 
            if ((RGP_FROMDATE == null) || (RGP_FROMDATE == DateTime.MinValue)) { RGP_FROMDATE = dbv.DBSysDate; }
            RGP_FROMDATE = RGP_FROMDATE.Date;
            if ((RGP_TODATE != null) && (RGP_FROMDATE != DateTime.MinValue)) { RGP_TODATE = ((DateTime)RGP_TODATE).Date; }

            if (pocSplatek == 365) RGP_TODATE = RGP_FROMDATE;
            
            #region data z EVPOPL, NAZPOPL
            string expfin = string.Empty;
            string typsankce = string.Empty;
            string pernas = string.Empty;
            string vystup = string.Empty;
            decimal procsankce = 0;
            decimal pevnacastka = 0;
            decimal nasobek = 0;
            int porvs = 0;

            dbu.GetEvpoplData(CompoundKey1.RGP_POPLATEK, CompoundKey1.RGP_PER,
                                        ref expfin, ref procsankce, ref typsankce,
                                        ref pevnacastka, ref nasobek, ref pernas,
                                        ref vystup, ref porvs);
            if (expfin == string.Empty) { throw new Exception("nelze určit export do financí - neexistující perioda pro poplatek"); }
            if (string.IsNullOrEmpty(RGP_EXPUCTO))
                 RGP_EXPUCTO = expfin;
            RGP_PROCSANKCE = procsankce;
            RGP_TYPSANKCE = typsankce;
            RGP_PEVNACASTKA = pevnacastka;
            RGP_NASOBEK = nasobek;
            RGP_PERNAS = pernas;
            RGP_VYSTUP = vystup;
            if (RGP_PORVS == 0) 
               RGP_PORVS = porvs + 1;
            #endregion data z EVPOPL, NAZPOPL

            this.LASTUPDATE = dbv.DBSysDateTime;
            this.LOGIN = dbv.DBUserName;
        }
    }
}