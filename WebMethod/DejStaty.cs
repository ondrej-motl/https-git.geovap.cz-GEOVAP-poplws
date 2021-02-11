using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Xpo.DB;

namespace PoplWS.WebMethod
{
    public class DejStaty
    {
            public STATY_RESP DejSeznamStaty(Session sesna, int EXT_APP_KOD)
            {
                STATY_RESP Resp = new STATY_RESP();

                sesna.DropIdentityMap();

                try
                {

                    if (EXT_APP_KOD == null)
                    { throw new Exception("kód externí aplikace není zadán"); }
                    P_EXTAPP EA = sesna.GetObjectByKey<P_EXTAPP>(EXT_APP_KOD);
                    if (EA == null)
                    { throw new Exception("chybný kód externí aplikace"); }


                    XPCollection<P_STAT> staty = new XPCollection<P_STAT>(sesna, CriteriaOperator.Parse("1 = 1"), new SortProperty("STAT_NAZEV", SortingDirection.Ascending));
                    if (staty != null)
                        Resp.STATY = new List<STAT>();
                    foreach (P_STAT item in staty)
                    {
                        STAT stat = new STAT();
                        stat.KOD = item.STAT_KOD;
                        stat.NAZEV = item.STAT_NAZEV;
                        Resp.STATY.Add(stat);
                    }

                    Resp.result = Result.OK;
                    Resp.status = Status.EXISTS;
                    return Resp;
                }
                catch (Exception exc)
                {
                    Resp.result = Result.ERROR;
                    


                    if (exc.InnerException == null)
                    {
                        Resp.ERRORMESS = exc.Message;
                    }
                    else
                    {
                        Resp.ERRORMESS = exc.InnerException.Message;
                    }
                    /*
                      throw new Exception(String.Format("chyba \n {0}", exc.InnerException.Message));
                      */
                    return Resp;
                }

            }
    }
}