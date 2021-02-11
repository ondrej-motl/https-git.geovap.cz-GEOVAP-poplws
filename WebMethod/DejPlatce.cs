using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml.Serialization;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Data.Filtering;
using System.Text;

namespace PoplWS.WebMethod
{
 public class NajdiPlatce
 {
     /// <summary>
     /// vraci pouze hodnotu PLATCE_ID, datumy platnosti ...
     /// OSOBA_ID, KCROK, KCSPLATKA, POSLSPLATKA vraci hodnotu nula
     /// metoda je urcena pro stare pripady, kdy ext. agenda zna VS a potrebuje zjistit PLATCE_ID, aby mohla 
     /// pridavat predpisy
     /// </summary>
     /// <param name="sesna"></param>
     /// <param name="EXT_APP_KOD"></param>
     /// <param name="inParams"></param>
     /// <returns></returns>
     public PLATCE_RESP DejPlatce(Session sesna, int EXT_APP_KOD, GET_PLATCE_PARAMS inParams)
     {
         PLATCE_RESP Resp = new PLATCE_RESP();
         
         Resp.status = Status.NOTEXISTS;

         sesna.DropIdentityMap();

         int popl;
      try
      {
        
        #region kontrola vyberovych parametru
          if ( string.IsNullOrWhiteSpace(inParams.VS) )
             throw new Exception("Výběrová podmínka není zadána.");

          popl = decimal.ToInt16(inParams.POPLATEK);

        #endregion kontrola vyberovych parametru


        #region kontrola prava na poplatek
        if (EXT_APP_KOD == null)
        { throw new Exception("kód externí aplikace není zadán"); }

        KONTROLA_POPLATKU kp = new KONTROLA_POPLATKU(sesna, EXT_APP_KOD);
        if (!kp.EAexist())
        { throw new Exception("chybný kód externí aplikace"); }
        if (!kp.existPravoNadPoplatkem(popl))
        { 
            throw new Exception("Ext. aplikace nemá oprávnění k typu pohledávky.");
        }
        #endregion kontrola prava na poplatek

        #region kontrola prava na cteni platce
        PravoNadPoplatkem pnp = null;
        try
        {
            pnp = new PravoNadPoplatkem(sesna);
        }
        catch (Exception)
        {
            throw new Exception("kontrola přístp. práv uživatele nad daty Příjmové agendy skončila chybou");
        }

        
        if (!pnp.PravoExist(popl, PravoNadPoplatkem.PrtabTable.RGP, PravoNadPoplatkem.SQLPerm.SELECT))
            throw new Exception("PoplWS - nedostatečná oprávnění pro čtení plátce.");

        #endregion kontrola prava nad platce


        CriteriaOperator criteria;
        criteria = CriteriaOperator.Parse("CompoundKey1.LIKEVS_POPLATEK = ? and LIKEVS_VS = ?", inParams.POPLATEK, inParams.VS);
        XPCollection<P_LIKEVS> vsExist = new XPCollection<P_LIKEVS>(sesna, criteria);
        XPCollection<P_RGP> plce;
        if (vsExist.Count > 0)
        {
            criteria = CriteriaOperator.Parse("CompoundKey1.RGP_POPLATEK=? and RGP_PORVS=?",
                                                vsExist[0].CompoundKey1.LIKEVS_POPLATEK,
                                                vsExist[0].LIKEVS_PORVS);
            plce = new XPCollection<P_RGP>(sesna, criteria, new SortProperty("RGP_ID", SortingDirection.Ascending));  //order by, sort
            if (plce.Count == 0)
            {
                Resp.status = Status.NOTEXISTS;
                Resp.ERRORMESS = "Plátce neexistuje.";
                return Resp;
            }
        }
        else
        {
            Resp.status = Status.NOTEXISTS;
            Resp.ERRORMESS = "VS neexistuje.";
            return Resp;
        }

        Utils.copy.CopyDlePersistentAttr<PLATCE_RESP>(plce[0], Resp);

          Resp.RGP_ID = plce[0].RGP_ID;
          Resp.OSOBA_ID = 0;
          Resp.RGP_KCROK = 0;
          Resp.RGP_KCSPLATKA = 0;
          Resp.RGP_POSLSPLATKA = 0;
          Resp.RGP_POPLATEK = plce[0].CompoundKey1.RGP_POPLATEK;
          Resp.RGP_PER = plce[0].CompoundKey1.RGP_PER;
          Resp.RGP_DOPLKOD = plce[0].CompoundKey1.RGP_DOPLKOD;
          Resp.EXPORTOVAT_DO_FINANCI = (SouhlasAnoNe)Enum.Parse(typeof(SouhlasAnoNe), plce[0].RGP_EXPUCTO);  

          Resp.result = Result.OK;
          Resp.status = Status.EXISTS;
          return Resp;
      }
      catch (Exception exc)
      {
          Resp.result = Result.ERROR;
          Resp.status = Status.ERROR;

          if (exc.InnerException == null)
          {
              Resp.ERRORMESS = exc.Message;
          }
          else
          {
              Resp.ERRORMESS = exc.InnerException.Message;
          }
          return Resp;
      }

     }
 }
}