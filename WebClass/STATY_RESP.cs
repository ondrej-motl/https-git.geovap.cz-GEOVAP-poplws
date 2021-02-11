using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoplWS
{
    public class STATY_RESP : POPLWS_RESPONSE
    {
      public List<STAT> STATY; 
    }

    public class STAT
    {
        public string KOD;
        public string NAZEV;
    }

}