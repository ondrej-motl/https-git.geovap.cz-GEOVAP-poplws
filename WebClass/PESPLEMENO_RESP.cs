using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoplWS
{
  public class PESPLEMENA_RESP : POPLWS_RESPONSE
  {
    public List<PLEMENO> PLEMENA; 
  }

  public class PLEMENO
  {
    public decimal KOD;
    public string NAZEV;
  }
}