using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Xpo.DB;

namespace PoplWS
{

    public class DBValue
    {
        string _DBUserName = "";
        DateTime? predchDatum;
        Session sesna;


        //0.31
        private DBValue(Session session)
        { sesna = session; }

        public static DBValue Instance(Session session)
        {
            return new DBValue(session);
        }

        /// <summary>
        /// datum bez casoveho udaje
        /// je aktualizovano pouze jednou za 3600 vterin viz promenna timeDiference
        /// </summary>
        public DateTime DBSysDate
        {
            get
            {
                DateTime dt = predchDatum ?? new DateTime(1, 1, 1);
                if (! predchDatum.HasValue)
                {
                    DBVal dbv = new DBVal(sesna);
                    dt = dbv.DBSysDate;
                    predchDatum = dt;
                }

                int timeDiference = 3600;  
                DateTime d = DateTime.Now;
                double diffmilisec = (d - dt).TotalMilliseconds;
                if (diffmilisec > (timeDiference * 1000))  //casova diference je > timeDiference sec.
                {
                  DBVal dbv = new DBVal(sesna);
                  dt = dbv.DBSysDate;
                  predchDatum = dt;
                }
                 
                return new DateTime(dt.Year, dt.Month, dt.Day);
            }

          set {}

        }

        /// <summary>
        /// je aktualizovano pouze jednou za 10 vterin
        /// viz promenna timeDiference
        /// </summary>
        public DateTime DBSysDateTime
        {
            get
            {
                DateTime dt = predchDatum ?? new DateTime(1, 1, 1);
                if (! predchDatum.HasValue)
                {
                    DBVal dbv = new DBVal(sesna);
                    dt = dbv.DBSysDate;
                    predchDatum = dt;
                }

                int timeDiference = 10;
                DateTime d = DateTime.Now;
                double diffmilisec = (d - dt).TotalMilliseconds;
                if (diffmilisec > (timeDiference * 1000))  //casova diference je > 10 sec.
                {
                    DBVal dbv = new DBVal(sesna);
                    dt = dbv.DBSysDate;
                    predchDatum = dt;
                    //DBValue.DBDate = new DateTime(dt.Year, dt.Month, dt.Day);

                    return dt;
                }
                else
                {
                    return dt;
                }
            }
        }

        public string DBUserName
        {
            get
            {
                if (!(_DBUserName == ""))
                {
                    return _DBUserName;
                }
                else
                {
                    DBVal dbVal = new DBVal(sesna);
                    string cmdText = dbVal.dbv.getUserNameCommandText;

                    SelectedData sd = sesna.ExecuteQuery(cmdText);
                    _DBUserName = sd.ResultSet[0].Rows[0].Values[0].ToString();
                    return _DBUserName;
                }
            }
        }
    }

    internal class DBVal
        {
          DateTime? _DBSysDate = null;   
          Session sesna;
          public readonly IDBValFactory dbv;

          public DBVal(Session session)
          {
            this.sesna = session;
            dbv = CreateDBV();
          }

          public IDBValFactory CreateDBV()
          {
            string typDB = "";
            typDB = DejTypDB();  //0.33

            IDBValFactory dbtyp = null;
            if (typDB.ToUpper().Contains("MSSQLSERVER"))
            {
              dbtyp = new MSSQL();
            }
            if (typDB.ToUpper().Contains("ORACLE"))
            {
              dbtyp = new ORACLE();
            }
            return dbtyp;
          }

          /// <summary>
          /// vraci cas z databaze
          /// </summary>
          public DateTime DBSysDate
          {
            get
            {
              CriteriaOperator funcNow = new FunctionOperator(FunctionOperatorType.Now);
              _DBSysDate = (DateTime)sesna.Evaluate(typeof(DUAL), funcNow, null);
              return (DateTime)_DBSysDate;
            }
          }

          public string DBUserName
          {
            get
            {
              return string.Empty;
            }
          }


          private string DejTypDB()
          {
              string typDB = "";
              Type providerType = (((DevExpress.Xpo.Helpers.BaseDataLayer)this.sesna.DataLayer).ConnectionProvider).GetType();
              if (typeof(OracleConnectionProvider).IsAssignableFrom(providerType))
                  typDB = "ORACLE";
              else
                  if (typeof(MSSqlConnectionProvider).IsAssignableFrom(providerType))
                     typDB = "MSSQLSERVER";
                  else
                      throw new Exception("Nepodporovaný typ DataLayer: " + (((DevExpress.Xpo.Helpers.BaseDataLayer)this.sesna.DataLayer).ConnectionProvider).GetType().ToString());

              return typDB;
          }
        
        }

    interface IDBValFactory
    {
        string getUserNameCommandText { get; }
        string getSessionIDText { get; }
        string DatabaseName { get; }
        string getParamText(string param);
        /// <summary>
        /// priklad: if ...  == typeof(MSSQL)
        /// </summary>
        Type Database { get; }
        string getCurrentDateTimeText { get; }
    }

    internal class MSSQL : IDBValFactory
    {
        public string getUserNameCommandText
        {
            get { return "select system_user from dual"; }
        }

        public string getSessionIDText
        {
            get { return "select @@SPID from dual"; }
        }

        public string DatabaseName
        {
          get { return this.GetType().Name; //MSSQL
              }
        }
        public string getParamText(string param)
        {
            return "@" + param;
        }

        public Type Database
        {
          get { return this.GetType(); }
        }

        public string getCurrentDateTimeText
        {
            get { return "getdate()"; }
        }
    }

    internal class ORACLE : IDBValFactory
    {

        public string getUserNameCommandText
        {
            get { return "select user from dual"; }
        }

        public string getSessionIDText
        {
            get { return "select sys.get_sessid from dual"; }
        }

        public string DatabaseName
        {
          get { return this.GetType().Name;  //ORACLE 
              }
        }

        public string getParamText(string param)
        {
          return ":" + param;
        }

        public Type Database
        {
          get { return this.GetType(); }
        }

        public string getCurrentDateTimeText
        {
            get { return "getdate()"; }
        }

    }


}