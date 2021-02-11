﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace PoplWS
{

    public partial class P_ODPADY_EULEVY : XPLiteObject
    {
        int fID;
        [Key, Persistent]  
        public int EUL_ID
        {
            get { return fID; }
            set { SetPropertyValue<int>("ID", ref fID, value); }
        }
        
        string fDAVKA;
        [Size(36)]
        public string DAVKA
        {
            get { return fDAVKA; }
            set { SetPropertyValue<string>("DAVKA", ref fDAVKA, value); }
        }
        DateTime fENTRYDATE;
        [FetchOnly]  //od v 18.2 
        public DateTime ENTRYDATE
        {
            get { return fENTRYDATE; }
            set { SetPropertyValue<DateTime>("ENTRYDATE", ref fENTRYDATE, value); }
        }
        string fENTRYLOGIN;
        [Size(32)]
        //[ColumnDbDefaultValue("(suser_sname())")]
        [FetchOnly]  //od v 18.2
        public string ENTRYLOGIN
        {
            get { return fENTRYLOGIN; }
            set { SetPropertyValue<string>("ENTRYLOGIN", ref fENTRYLOGIN, value); }
        }
        string fVS;
        [Size(10)]
        public string VS
        {
            get { return fVS; }
            set { SetPropertyValue<string>("VS", ref fVS, value); }
        }

        decimal fPOPL;
        public decimal POPL
        {
            get { return fPOPL; }
            set { SetPropertyValue<decimal>("POPL", ref fPOPL, value); }
        }

        string fPER;
        [Size(1)]
        public string PER
        {
            get { return fPER; }
            set { SetPropertyValue<string>("PER", ref fPER, value); }
        }
        decimal fKC_ZAPER;
        public decimal KC_ZAPER
        {
            get { return fKC_ZAPER; }
            set { SetPropertyValue<decimal>("KC_ZAPER", ref fKC_ZAPER, value); }
        }

        decimal fROK;
        public decimal ROK
        {
            get { return fROK; }
            set { SetPropertyValue<decimal>("ROK", ref fROK, value); }
        }

        int fEA;
        public int EA
        {
            get { return fEA; }
            set { SetPropertyValue<int>("EA", ref fEA, value); }
        }
        string fZPRAC;
        //[ColumnDbDefaultValue("((0))")]
        public string ZPRAC
        {
            get { return fZPRAC; }
            set { SetPropertyValue<string>("ZPRAC", ref fZPRAC, value); }
        }

        public P_ODPADY_EULEVY(Session session) : base(session) 
        {
        }
    }

}