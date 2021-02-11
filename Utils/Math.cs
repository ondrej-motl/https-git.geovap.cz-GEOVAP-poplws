using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoplWS.Utils
{
  public class Mathm
  {
    /// <summary>
    /// desetinna cast cisla decimal
    /// </summary>
    /// <param name="d"></param>
    /// <returns></returns>
    internal static decimal Frac(decimal d)
    {
      return d - (int)d;
    }

    public enum SmerZaokrouhlovani { aritmeticky, nahoru, dolu }

    internal static decimal ZaokrouhliDouble(decimal cislo, short pocDesMist)
    {
      int i = (int)(cislo + 0.000001m);
      cislo = (cislo - i) * (decimal)Math.Pow(10, (double)pocDesMist);

      cislo = cislo + 0.0000001m; 
      cislo = Math.Round(cislo);  
      return i + (cislo / (Decimal)Math.Pow(10, (double)pocDesMist));

    }

    internal static decimal Zaokrouhli(decimal cislo, short presnost, SmerZaokrouhlovani smerZaokr)
    {
      decimal d;
      decimal koef;
      int trunc_cislo;

      int smer = (int)smerZaokr;

      if ((presnost < 0) || (smer < 0) || (smer == 3))   //nezaokrouhlovat
      {
        return cislo;
      }

      trunc_cislo = (int)cislo;
      koef = 0;

      switch (presnost)
      {
        case 0: koef = 10;      //desetiny
          break;
        case 1: koef = 1;       //na cele
          break;
        case 2: koef = 0.1m;    //na desitky
          break;
        case 3: koef = 0;       //padesatniky
          break;
        case 4: koef = 0.01m;   //na stovky
          break;
      }

      if (new[] { 0, 1, 2, 4 }.Contains(presnost)) { cislo = cislo * koef; };
      if (presnost == 3) { cislo = Frac(cislo) * 2; }

      if (smer == 0)  //aritmeticke zaokrouhlovani
      {
        if (Frac(cislo) == 0.5m)
        { cislo = cislo + 0.1m; }
        cislo = Math.Round(cislo);
      }

      if (smer == 1)  //zaokrouhlim nahoru, '-12,3' se zaokr. na '-12'
      {
        d = cislo;
        cislo = (int)d;
        if (Frac(d) > 0)
        { cislo = cislo + 1; }
      }

      if (smer == 2)  //zaokrouhlim dolu,  '-12,3' se zaokr. na '-13'
      {
        d = cislo;
        cislo = (int)d;
        if (Frac(d) < 0)
        { cislo = cislo - 1; }
      }


      if (presnost == 3)
      { return trunc_cislo + (cislo * 0.5m); }
      else
      { return cislo / koef; }
    }

    internal static uint FNV1aHash(string text)
    {
      const uint FNV_offset_basis = 2502136361;
      const uint FNV_prime = 31776619;

      uint result;
      result = FNV_offset_basis;

      foreach (char c in text)
      {
        Byte b = Convert.ToByte(c);
        result = (result ^ b) * FNV_prime;
      }

      return result;
    }

  }
}