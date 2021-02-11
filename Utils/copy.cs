using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DevExpress.Xpo;
using System.Reflection;
using System.Collections;

namespace PoplWS.Utils
{
    public static class copy
    {
        /*======================================================================*/                                             
        internal static void CopyDlePersistentAttr<T>(object source, T target)
        {

            BindingFlags bFlags;
            bFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    
            HashSet<Type> copyTypes = new HashSet<Type> { typeof(string), typeof(DateTime), typeof(DateTime?), typeof(Decimal), typeof(Decimal?),
                          typeof(int), typeof(int?) 
                         };
    
            PersistentAttribute persistentAttr;
            Type type = source.GetType();



            foreach (PropertyInfo sourceInfo in type.GetProperties(bFlags)) 
            {
                bool persAttrSourceExists = false;
                PropertyInfo targetInfo = null;
                //prochazeni Class-Properties Attributes  
                foreach (Attribute attr in sourceInfo.GetCustomAttributes(true))
                {
                    persistentAttr = attr as PersistentAttribute;
                    persAttrSourceExists = (persistentAttr != null) && (persistentAttr.MapTo != null);
                    if (null != persistentAttr) 
                    {
                        string targetPropName = GetPropertNameFromPersistAttr(target, persistentAttr.MapTo);
                        if (targetPropName == string.Empty) 
                        {   
                          if (persistentAttr.MapTo != null)
                                 targetInfo = typeof(T).GetProperty(persistentAttr.MapTo, bFlags);
                          if (targetInfo == null)
                          {
                            targetInfo = typeof(T).GetProperty(sourceInfo.Name, bFlags);
                          }
                        }
                        else
                        {  
                            targetInfo = typeof(T).GetProperty(targetPropName, bFlags);
                        }
                    }
                }

               if (targetInfo == null)
                {
                  string targetPropName = GetPropertNameFromPersistAttr(target, sourceInfo.Name);
                  if (targetPropName == string.Empty)
                  {   
                    targetInfo = typeof(T).GetProperty(sourceInfo.Name, bFlags);
                  }
                  else
                  {  
                    targetInfo = typeof(T).GetProperty(targetPropName, bFlags);
                  }

                }

                if ( (!(targetInfo == null)) && (targetInfo.CanWrite)
                     && targetInfo.PropertyType.IsAssignableFrom(sourceInfo.PropertyType)
                    )
                {
                  targetInfo.SetValue(target, sourceInfo.GetValue(source, null), null);
                }
            }
        }

        /*======================================================================*/                                             
        private static bool CanChangeType(object value, Type toType)
        {
          if (value == null)
          {
            return false;
          }

          if (toType == null)
          {
            return false;
          }

          IConvertible convertible = value as IConvertible;

          if (convertible == null)
          {
            return false;
          }

          return true;
        }
        /*======================================================================*/
        internal static string GetPropertNameFromPersistAttr(object obj, string MapTo)
        {
          if (MapTo == null)
            return String.Empty;

            PersistentAttribute persistentAttr;
            Type type = obj.GetType();
            //public i private instancni properties
            BindingFlags bFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            foreach (PropertyInfo prop in type.GetProperties(bFlags))
            {
                foreach (Attribute attr in prop.GetCustomAttributes(true))
                {
                    persistentAttr = attr as PersistentAttribute;
                    if (null != persistentAttr)
                    {
                        if (MapTo == persistentAttr.MapTo)
                        {
                            return prop.Name;
                        }
                    }
                }
            }
            return string.Empty;
        }


      /*======================================================================*/
        /// <summary>
        /// zkopiruje
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void DeepCopyCI<T>(object source, T target)
        {
          if (source == null)
          { return; }

          BindingFlags bf = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

          foreach (System.Reflection.PropertyInfo sourceInfo in source.GetType().GetProperties())
          {
            System.Reflection.PropertyInfo targetInfo = typeof(T).GetProperty(sourceInfo.Name, System.Reflection.BindingFlags.IgnoreCase |
                                                                                               System.Reflection.BindingFlags.Instance |
                                                                                               System.Reflection.BindingFlags.FlattenHierarchy |
                                                                                               System.Reflection.BindingFlags.Public |  //pouze public
                                                                                               System.Reflection.BindingFlags.NonPublic); //pouze internal
            if (typeof(T).GetProperty(sourceInfo.Name, bf) == null)
              continue;

            if (typeof(T).GetProperty(sourceInfo.Name, bf).PropertyType.GetInterface("IList", false) == null)
            {
              if (targetInfo != null && targetInfo.CanWrite)
              {
                targetInfo.SetValue(target, sourceInfo.GetValue(source, null), null);
              }
            }
            else
            {
              object t = Activator.CreateInstance<object>();  

              IList sourceListObject = (IList)typeof(T).GetProperty(sourceInfo.Name, bf).GetValue(source, null);
              IList targetListObject = (IList)typeof(T).GetProperty(sourceInfo.Name, bf).GetValue(target, null);
              if (sourceListObject != null)
              {

                foreach (object item in sourceListObject)
                {

                  Type mt = item.GetType();  

                  MethodInfo method = t.GetType().GetMethod("CopyDlePersistentAttr");
                  MethodInfo deepCopyConstructed = method.MakeGenericMethod(mt);
                  Type[] typeArguments = deepCopyConstructed.GetGenericArguments();


                  Type type = targetInfo.PropertyType;
                  Type itemType = null;
                  if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                  {
                    itemType = type.GetGenericArguments()[0];
                    object genericListItem = Activator.CreateInstance(itemType);
                    targetListObject.Add(genericListItem);

                    object[] args = { item, genericListItem };
                    deepCopyConstructed.Invoke(null, args);
                  }
                }
              }  //else
            }
          }
        }

        /// <summary>
        /// zdrojem je CwdIszrWS.asmx.cs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void DeepCopyListCI<T>(object source, T target)
        {
          if ((source == null) || (typeof(T).GetInterface("IList", false) == null))
          { return; }


          object t = Activator.CreateInstance<object>();  

          foreach (object item in (IList)source)
          {
            Type type = typeof(T);
            Type itemType = null;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
              itemType = type.GetGenericArguments()[0];
              object genericListItem = Activator.CreateInstance(itemType);
              Type mt = genericListItem.GetType();
              MethodInfo method = t.GetType().GetMethod("CopyDlePersistentAttr");
              MethodInfo deepCopyConstructed = method.MakeGenericMethod(mt);
              Type[] typeArguments = deepCopyConstructed.GetGenericArguments();

              IList targetList = (IList)target;
              targetList.Add(genericListItem);
              object[] args = { item, genericListItem };
              deepCopyConstructed.Invoke(null, args);
            }
          }
        }

    }
}