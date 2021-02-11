using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;

namespace PoplWS.PersistClass
{
    public partial class P_DPH_SAZBA : XPLiteObject
    {
        decimal? fDPHS_SAZBA;
        public decimal? DPHS_SAZBA
        {
            get { return fDPHS_SAZBA; }
            set { SetPropertyValue<decimal?>("DPHS_SAZBA", ref fDPHS_SAZBA, value); }
        }
        DateTime fDPHS_DO;
        public DateTime DPHS_DO
        {
            get { return fDPHS_DO; }
            set { SetPropertyValue<DateTime>("DPHS_DO", ref fDPHS_DO, value); }
        }
        int fDPHS_ZAOKR;
        public int DPHS_ZAOKR
        {
            get { return fDPHS_ZAOKR; }
            set { SetPropertyValue<int>("DPHS_ZAOKR", ref fDPHS_ZAOKR, value); }
        }
        public struct CompoundKey1Struct
        {
            [Persistent("DPHS_KAT")]
            public char DPHS_KAT { get; set; }
            [Persistent("DPHS_OD")]
            public DateTime DPHS_OD { get; set; }
        }
        [Indexed(Name = @"P_DPH_SAZBA_PK", Unique = true)]
        [Key, Persistent]
        public CompoundKey1Struct CompoundKey1;

        public P_DPH_SAZBA(Session session) : base(session) { }

    }
}