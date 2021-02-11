using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo.DB;
using System.Reflection;

namespace PoplWS  
{
  internal static class ExtensionMethodXPOSelectedData
  {
    internal static string[] GetColNames(this DevExpress.Xpo.DB.SelectedData data)
    {
      string[] colNames = new string[data.ResultSet[0].Rows.Count()];
      for (int i = 0; i < data.ResultSet[0].Rows.Count(); i++)
      {
        SelectStatementResultRow rowColDefine = data.ResultSet[0].Rows[i];
        colNames[i] += (string)rowColDefine.Values[0];
      }

      return colNames;
    }

    internal static Dictionary<string, Type> GetColumnsInfo(this DevExpress.Xpo.DB.SelectedData data)
    {
      Dictionary<string, Type> colInfo = new Dictionary<string, Type>();
      for (int i = 0; i < data.ResultSet[0].Rows.Count(); i++)
      {
        SelectStatementResultRow rowColDefine = data.ResultSet[0].Rows[i];
        Type typ = DBColumn.GetType((DBColumnType)Enum.Parse(typeof(DBColumnType), (string)rowColDefine.Values[2]));
        try
        {
          colInfo.Add((string)rowColDefine.Values[0], typ);
        }
        catch (Exception exc)
        {
          throw new ArgumentException(exc.Message + "\n key: " + (string)rowColDefine.Values[0]);
        }
      }
      return colInfo;
    }
    /// <summary>
    /// vraci hodnotu sloupce, pripadne hodnotu null (referenci)
    /// </summary>
    /// <param name="data"></param>
    /// <param name="row"></param>
    /// <param name="ColumnName"></param>
    /// <returns></returns>
    internal static object ValByName(this SelectStatementResultRow row, DevExpress.Xpo.DB.SelectedData data, string ColumnName)
    {
      string colName, colType, stringValue;
      int colIndex = Int32.MaxValue;

      for (int i = 0; i < data.ResultSet[0].Rows.Count(); i++)
      {
        DevExpress.Xpo.DB.SelectStatementResultRow rowColDefine = data.ResultSet[0].Rows[i];
        colName = (string)rowColDefine.Values[0];
        colType = (string)rowColDefine.Values[2];   //Decimal, String ...
        if (String.Equals(colName, ColumnName, StringComparison.InvariantCultureIgnoreCase))
        {
          colIndex = i;
          break;
        }
      }
      if (colIndex <= row.Values.Count())
        return row.Values[colIndex];
      else
        throw new IndexOutOfRangeException(String.Format("column \"{0}\" v kolekci DataSetu neexistuje", ColumnName));

    }

    /*======================================================================*/
    internal static void copyToObject<T>(this SelectStatementResultRow row, DevExpress.Xpo.DB.SelectedData data, T target)
    {

      SelectStatementResultRow[] columns = data.ResultSet[0].Rows;
      SelectStatementResultRow[] values = data.ResultSet[1].Rows;
      Dictionary<string, Type> columnsInfo = data.GetColumnsInfo();

      BindingFlags bFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
      
      System.Reflection.PropertyInfo targetInfo = null;

      foreach (var column in columns)
      {
        string colName = (string)column.Values[0];
        object value = ValByName(row, data, colName);
        string targetPropName = Utils.copy.GetPropertNameFromPersistAttr(target, colName);
        if (targetPropName == string.Empty)
        {  
            targetInfo = typeof(T).GetProperty(colName, bFlags);
        }
        else
        {  
          targetInfo = typeof(T).GetProperty(targetPropName);
        }

        if ((!(targetInfo == null)) && (targetInfo.CanWrite))
        {
          bool dataZapsana = false;
          if (targetInfo.PropertyType.IsAssignableFrom(columnsInfo[colName]))
          {
            targetInfo.SetValue(target, value, null);
            dataZapsana = true;
          }
          else
          {
            HashSet<Type> IntTypes = new HashSet<Type> { typeof(int), typeof(Int16), typeof(Int32),
                                                                  typeof(Int64), typeof(short), typeof(uint),
                                                                  typeof(UInt16), typeof(UInt32), typeof(UInt64)
                                                                 };
            HashSet<Type> nullIntTypes = new HashSet<Type> { typeof(int?), typeof(Int16?), typeof(Int32?),
                                                                  typeof(Int64?), typeof(short?), typeof(uint?),
                                                                  typeof(UInt16?), typeof(UInt32?), typeof(UInt64?)
                                                                 };

            if ((IntTypes.Contains(targetInfo.PropertyType)) && (columnsInfo[colName] == typeof(System.Decimal)) &&
                  (((decimal)value % 1) == 0) 
              )
            {
              targetInfo.SetValue(target, (int)(decimal)value, null);
              dataZapsana = true;
            }

            if ((nullIntTypes.Contains(targetInfo.PropertyType)) && (columnsInfo[colName] == typeof(System.Decimal)))
            {
              if (value == null)
              {
                targetInfo.SetValue(target, value, null);
                dataZapsana = true;
              }
              else
              {
                if (((decimal)(value ?? 0) % 1) == 0) 
                {
                  targetInfo.SetValue(target, (int)(decimal)value, null);
                  dataZapsana = true;
                }
              }
            }
          }

          if (!dataZapsana)
            throw new Exception(String.Format("data sloupce \" {0} \" nelze zkopirovat", colName));
        }
      }
    }

  }
}