using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using DevExpress.Xpo.DB;

namespace PoplWS
{
    public class PoplOracleConnectionProvider : OracleConnectionProvider
    {
        public new const string XpoProviderTypeString = "PoplOracle";

        public PoplOracleConnectionProvider(IDbConnection connection, AutoCreateOption autoCreateOption)
            : base(connection, autoCreateOption)
        {
        }

        public new static void Register()
        {
            RegisterDataStoreProvider(XpoProviderTypeString, CreateProviderFromString);
        }

        public new static IDataStore CreateProviderFromString(string connectionString, AutoCreateOption
                                                                                           autoCreateOption,
                                                              out IDisposable[] objectsToDisposeOnDisconnect)
        {
            IDbConnection connection = CreateConnection(connectionString);
            objectsToDisposeOnDisconnect = new IDisposable[] { connection };
            return CreateProviderFromConnection(connection, autoCreateOption);
        }

        public new static IDataStore CreateProviderFromConnection(IDbConnection connection,
                                                                  AutoCreateOption autoCreateOption)
        {
            return new PoplOracleConnectionProvider(connection, autoCreateOption);
        }

        protected override string GetSeqName(string tableName)
        {
            if (tableName == "P_RGP") return "P_SEQ_RGP_ID";
            else if (tableName == "P_RGP_DPHSPL") return "P_SEQ_RGP_DPHSPL_ID";
            else if (tableName == "P_RGP_DPHPSPL") return "P_SEQ_RGP_DPHPSPL_ID";
            else return base.GetSeqName(tableName);
        }
    }

}