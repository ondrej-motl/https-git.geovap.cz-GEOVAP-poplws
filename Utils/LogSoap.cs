using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using System.Web.Services.Protocols;
using System.Reflection;

namespace PoplWS
{
  /// <summary>
  /// SOAP extenze pro sledování komunikace mezi klientem a serverem
  /// </summary>
  public class SpySoapExtension : SoapExtension
  {

    #region private fields
    private string m_fileName;
    private Stream m_oldStream;
    private Stream m_newStream;
    private string m_LogMode;
    #endregion private fields
    /// <summary>
    /// Standardní konstruktor
    /// </summary>
    public SpySoapExtension()
    {
      m_oldStream = null;
      m_newStream = null;
      m_fileName = "";
      m_LogMode = System.Web.Configuration.WebConfigurationManager.AppSettings["LogMode"];
      if (string.IsNullOrWhiteSpace(m_LogMode))
          m_LogMode = "0";
      else
          m_LogMode = "1";
    }
    /// <summary>
    /// Přepsání metody GetInitializer, ktera je volana pri volani WebMethody
    /// </summary>
    /// <param name="methodInfo">Informace o metodě, na kterou je extenze aplikována</param>
    /// <param name="attribute">Instance třídy <see cref="SpySoapExtensionAttribute"/>, z ní je odvozen název souboru do kterého mají být ukládány SOAP zprávy</param>
    /// <returns>Název souboru, do kterého budou uloženy SOAP zprávy</returns>
    public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
    {

      SpyExtensionAttribute spyAttribute = attribute as SpyExtensionAttribute;

      return spyAttribute.FileName;

    }
    /// <summary>
    /// Pøepsání metody GetInitializer, která je volána, pokud je SOAP extenze zaregistrována ve web.config
    /// </summary>
    /// <param name="serviceType">Typ WWW služby</param>
    /// <returns>Název souboru, do kterého budou uloženy SOAP zprávy</returns>
    public override object GetInitializer(Type serviceType)
    {
      return (serviceType.Assembly.FullName + ".spy");
    }

    /// <summary>
    /// Inicializace, která probíhá při každém volání WWW metody
    /// </summary>
    /// <param name="initializer">objekt vrácený z metody <see cref="GetInitializer"/></param>
    public override void Initialize(object initializer)
    {
      //Uložení názvu souboru do privátní promìnné
      m_fileName = (string)initializer;
    }

    /// <summary>
    /// Metoda, ve které uschováme referenci na pøedaný stream a vrátíme nový stream
    /// </summary>
    /// <param name="stream">originální <see cref="Stream"/></param>
    /// <returns>Nový stream, do kterého mohou být provádìny zmìny</returns>
    public override Stream ChainStream(Stream stream)
    {
      m_oldStream = stream;
      m_newStream = new MemoryStream();
      return m_newStream;

    }

    /// <summary>
    /// Zpracování zpráv podle fáze požadavku+ì
    /// </summary>
    /// <param name="message">SOAP zpráva</param>
    public override void ProcessMessage(SoapMessage message)
    {
      switch (message.Stage)
      {
        case SoapMessageStage.BeforeDeserialize:
          {
            WriteClientRequest();
            break;

          }
        case SoapMessageStage.AfterDeserialize:
          {
            break;
          }

        case SoapMessageStage.BeforeSerialize:
          {

            break;
          }

        case SoapMessageStage.AfterSerialize:
          {
            WriteServerResponse();
            break;
          }
      }
    }

    /// <summary>
    /// Zapsání SOAP zprávy od klienta do souboru
    /// </summary>
    private void WriteClientRequest()
    {
            MemoryStream wStream = new MemoryStream();
            CopyStream(m_oldStream, wStream, false);

            m_oldStream.Position = 0;
            m_newStream.Position = 0;

            CopyStream(m_oldStream, m_newStream);

            if (m_LogMode == "1")
            {
                m_newStream.Position = 0;
                wStream.Position = 0;

                FileStream fs = new FileStream(m_fileName + "_requests", FileMode.Append);

                #region nahrazeni hesla
                try
                {
                    StreamReader sr = new StreamReader(m_oldStream);
                    StreamWriter writer = new StreamWriter(wStream);
                    m_oldStream.Position = 0;
                    

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        int posS = line.ToUpper().IndexOf("<PASSWORD>");
                        int posE = line.ToUpper().IndexOf("</PASSWORD>");
                            if ((posS >= 0) && (posE >= 0))
                            {
                                string pwd = line.Substring(posS + 10, posE - posS - 10);
                                line = line.Replace(pwd, "xxx");
                            }
                        writer.WriteLine(line);
                    }
                    writer.Flush();
                    sr.Close();

                    wStream.Position = 0;
                }
                catch (Exception)
                {
                }
                
                #endregion nahrazeni hesla

                CopyStream(wStream, fs, true);

                fs.Close();
                wStream.Close();
            }
            m_newStream.Position = 0;
    }

    private void WriteServerResponse()
    {
      m_newStream.Position = 0;

      if (m_LogMode == "1")
      {
          FileStream fs = new FileStream(m_fileName + "_responses", FileMode.Append);

          CopyStream(m_newStream, fs, true);

          fs.Close();

          m_newStream.Position = 0;
      }

      CopyStream(m_newStream, m_oldStream, false);
    }

    private void CopyStream(Stream from, Stream to, bool writeSeparator = false)
    {
      TextReader sr = new StreamReader(from);
      TextWriter sw = new StreamWriter(to);

      if (writeSeparator)
      {
        sw.WriteLine();
        sw.WriteLine(String.Format(@"{0}, {1}", DateTime.Now, new String('-', 60)));

      }
      sw.Write(sr.ReadToEnd());
      sw.Flush();

    }

  }

  #region definice attributu SpyExtensionAttribute
  /// <summary>
  /// Summary description for SpySoapExtensionAttribute.
  /// deklarovanim tridy SpyExtensionAttribute je vytvoren attribut "SpyExtension" - suffix 
  /// "attribute"  je c sharpem automaticky odstranen
  ///     Note: it is a convention to use the word Attribute as a suffix in attribute class names. 
  ///     However, when we attach the attribute to a program entity, we are free not to include 
  ///     the Attribute suffix. The compiler first searches the attribute in System.Attribute derived 
  ///     classes. If no class is found, the compiler will add the word Attribute to the 
  ///     specified attribute name and search for it.
  /// </summary>
  [AttributeUsage(AttributeTargets.Method)]
  public class SpyExtensionAttribute : SoapExtensionAttribute
  {
    #region private variables
    private string m_fileName;
    private int m_priority;
    #endregion private variables
    #region constructors
    /// <summary>
    /// Standardní konstruktor
    /// </summary>
    public SpyExtensionAttribute()
      : this("testLog.spy")
    {


    }
    /// <summary>
    /// Konstruktor přijímající název souboru (bez cesty)
    /// </summary>
    /// <param name="fileName">Název souboru</param>
    public SpyExtensionAttribute(string fileName)
    {
        //0.27 vytvarel se adresar LogSoap i pri vypnutem logovani
        int logMode = 0;
        int.TryParse(System.Web.Configuration.WebConfigurationManager.AppSettings["LogMode"], out logMode);
        if (logMode == 0)
            return;

      m_fileName = DataDir() + "/" + fileName;
      m_priority = 1;
    }
    #endregion constructors
    #region public properties
    /// <summary>
    /// Vrácení typu extenze, pro kterou byl atribut vytvoøen
    /// </summary>
    public override Type ExtensionType
    {
      get
      {
        return typeof(SpySoapExtension);
      }
    }
    /// <summary>
    /// Priorita volání SOAP extenze
    /// </summary>
    public override int Priority
    {
      get
      {
        return m_priority;
      }
      set
      {
        m_priority = value;
      }
    }

    public string FileName
    {
      get
      {
        return m_fileName;
      }
      set
      {
        m_fileName = value;
      }
    }

    internal static string DataDir()
    {
       
        string currDir;

        try
        {
            currDir = HttpContext.Current.Server.MapPath(".");
        }
        catch (Exception)
        {
            currDir = System.IO.Path.GetTempPath();
        }


      string dataDir = currDir + "/LogSoap";
      bool exists = System.IO.Directory.Exists(dataDir);
      if (!exists)
        System.IO.Directory.CreateDirectory(dataDir);

      return dataDir;
    }

    #endregion public properties

  }
  #endregion definice attributu

}