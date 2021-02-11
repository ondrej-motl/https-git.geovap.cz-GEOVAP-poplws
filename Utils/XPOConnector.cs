using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;

using DevExpress.Xpo.Metadata;
using DevExpress.Xpo.Metadata.Helpers;

namespace PoplWS
{
  public class XPOConnector
  {

    private bool Connected = false;
    private MySession _Sesna = null;
    private string login;
    private string password;
    private AutoCreateOption _AutoCreateOption;
    public XPOConnector(string login, string password )
    {
      this.login = login;
      this.password = password;
      PoplOracleConnectionProvider.Register();  //0.8
      _AutoCreateOption = AutoCreateOption.SchemaAlreadyExists;
      Connect(_AutoCreateOption);  //autocreateOption je nastavovava az v GetDataLayer...

      
    }

    private void Connect(DevExpress.Xpo.DB.AutoCreateOption autoCreateOption)
    {
      if (!Connected)
      {
        XpoDefault.Session = null;
        Connected = true;
      }
    }
    /// <summary>
    /// connectonString tak jak je ulozeny v app.config 
    /// </summary>
    internal static string AppConfigConnectionString
    {
      get { 
            return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString; 
          }
    }
   
    internal string ConnectionString
    {
      get
      {
        return SlozConnectString();
          }
    }


    private IDataLayer GetDataLayer()
    {
      XpoDefault.Session = null;
      string conn = ConnectionString;
      XPDictionary dict = new ReflectionDictionary();
      IDataStore store;
      store = XpoDefault.GetConnectionProvider(conn, _AutoCreateOption);
       dict.GetDataStoreSchema(System.Reflection.Assembly.GetExecutingAssembly());
      IDataLayer dl = new SimpleDataLayer(dict, store);
      XpoDefault.DataLayer = dl;  //0.8
      return dl;
    }

    private IDataLayer GetDataLayerMultiThread()
    {
        XpoDefault.Session = null;
        string conn = ConnectionString;
        XPDictionary dict = new ReflectionDictionary();
        IDataStore store;
        store = XpoDefault.GetConnectionProvider(conn, _AutoCreateOption);
        dict.GetDataStoreSchema(System.Reflection.Assembly.GetExecutingAssembly());
        IDataLayer dl = new ThreadSafeDataLayer(dict, store);
        XpoDefault.DataLayer = dl;  //0.8
        return dl;
    }

    /// <summary>
    /// vraci stejnou session (singleton)
    /// </summary>
    /// <returns></returns>
    public MySession GetSession()
    {
      if (_Sesna != null)
      {
        return _Sesna;
      }
      else
      {
        _Sesna = new MySession(GetDataLayer());
        return _Sesna;
      }
    }

    internal MySession GetSessionMultiThread()
    {
        if (_Sesna != null)
        {
            return _Sesna;
        }
        else
        {
            _Sesna = new MySession(GetDataLayerMultiThread());
            return _Sesna;
        }
    }



    public MySession Sesna
    {
      get { return GetSession(); }
    }

    internal string Login
    {
      get
      { return login; }
    }

    internal string Password
    {
      get
      {  return password; }
    }

    private string SlozConnectString()
    {
      if ((password == null) || (login == null))
      {
        throw new Exception("nejsou zadány přihlašovací údaje");
      }
      string ConnectionString = XPOConnector.AppConfigConnectionString;
      string[] parametry = ConnectionString.Split(';');
      Dictionary<string, string> ConnectParam = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
      foreach (var param in parametry)
      {
        if (param.Length > 0)
        {
          string[] val = param.Split('=');
          ConnectParam.Add(val[0], val[1]);
        }
      }

      string typDB = "";
      string value = "";
      ConnectParam.TryGetValue("XpoProvider", out typDB);
      if (typDB.ToUpper().Contains("MSSQLSERVER"))
      {
      }
      if (typDB.ToUpper().Contains("ORACLE"))
      {
      }

      if ( !ConnectParam.TryGetValue("user id", out value) )
      {  
        return ConnectionString;
      }

      string key = "user id";
      if (ConnectParam.TryGetValue(key, out value))
        { //pokud by klic neexistoval, nasledujici radek by pridal zaznam s novym klicem do Dictionry
           ConnectParam[key] = login; 
        }
      key = "password";
      if (ConnectParam.TryGetValue(key, out value))
         { ConnectParam[key] = password; }


      string s = String.Empty;
      foreach (KeyValuePair<string, string> item in ConnectParam)
      {
        s += item.Key + "=" + item.Value + ";";
      }

      return s;
    }

  }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class IgnoreInsertUpdateAttribute : Attribute
    {
    }

    //vytvoreni vlastni tridy Session
    public class MySession : DevExpress.Xpo.Session
    {
        protected override MemberInfoCollection GetPropertiesListForUpdateInsert(object theObject, bool isUpdate, bool addDelayedReference)
        {
            var result = base.GetPropertiesListForUpdateInsert(theObject, isUpdate, addDelayedReference);
            var membersToRemove = new List<XPMemberInfo>();

            foreach (XPMemberInfo memberInfo in result)
            {
                if (memberInfo.HasAttribute(typeof(IgnoreInsertUpdateAttribute)))
                {
                    membersToRemove.Add(memberInfo);
                }
            }

            foreach (XPMemberInfo memberInfo in membersToRemove)
            {
                result.Remove(memberInfo);
            }

            return result;
        }

        public MySession(IDataLayer layer, params IDisposable[] disposeOnDisconnect) : base(layer, disposeOnDisconnect)
        {
        }
    }

    public class MyUnitOfWork : UnitOfWork
    {
        protected override MemberInfoCollection GetPropertiesListForUpdateInsert(object theObject, bool isUpdate, bool addDelayedReference)
        {
            var result = base.GetPropertiesListForUpdateInsert(theObject, isUpdate, addDelayedReference);
            var membersToRemove = new List<XPMemberInfo>();

            foreach (XPMemberInfo memberInfo in result)
            {
                if (memberInfo.HasAttribute(typeof(IgnoreInsertUpdateAttribute)))
                {
                    membersToRemove.Add(memberInfo);
                }
            }

            foreach (XPMemberInfo memberInfo in membersToRemove)
            {
                result.Remove(memberInfo);
            }

            return result;
        }

        public MyUnitOfWork(IDataLayer layer, params IDisposable[] disposeOnDisconnect) : base(layer, disposeOnDisconnect)
        {
        }

    }
}