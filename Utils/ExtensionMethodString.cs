using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoplWS   
{
  internal static class ExtensionMethodString
  {
    /// <summary>
    /// vrati zprava pozadovany pocet znaku
    /// </summary>
    /// <param name="text"></param>
    /// <param name="Length"></param>
    /// <returns></returns>
    public static string SubstrRight(this string text, int Length)
    {
      if (string.IsNullOrEmpty(text) || Length <= 0)
      {
        return string.Empty;
      }

      if (Length < text.Length)
      {
        return text.Substring(text.Length - Length);
      }

      return text;
    }

    /// <summary>
    /// vrati zleva pozadovany pocet znaku
    /// </summary>
    /// <param name="text"></param>
    /// <param name="Length"></param>
    /// <returns></returns>
    public static string SubstrLeft(this string text, int Length)
      {
         return text.Substring(0, Math.Min(text.Length, 30));
      }
  }
}