using Gware.Common.Serialisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gware.Common.Reflection
{
    public static class ExtensionMethods
    {
        public delegate void ReflectionPropertyAction(object performOn, PropertyInfo property);

        public static object GetPropertyValue(string propertyType, string propertyValue)
        {
            if (propertyType.Equals(typeof(String).Name))
            {
                return propertyValue;
            }
            else if (propertyType.Equals(typeof(Int16).Name))
            {
                return Convert.ToInt16(propertyValue);
            }
            else if (propertyType.Equals(typeof(Boolean).Name))
            {
                return Convert.ToBoolean(propertyValue);
            }
            else if (propertyType.Equals(typeof(DateTime).Name))
            {
                return Convert.ToDateTime(propertyValue);
            }
            else if (propertyType.Equals(typeof(Int64).Name))
            {
                return Convert.ToInt64(propertyValue);
            }
            else if (propertyType.Equals(typeof(Int32).Name))
            {
                return Convert.ToInt32(propertyValue);
            }
            return null;
        }

        public static void FindAndSetProperty(this Type setOn, string propertyName, string valueSetTo)
        {
            FindAndSetProperty(propertyName, valueSetTo, setOn, null);
        }
        public static void FindAndSetProperty(this object setOn, string propertyName, string valueSetTo)
        {
            FindAndSetProperty(propertyName, valueSetTo, setOn.GetType(), setOn);
        }
        private static void FindAndSetProperty(string propertyName, string valueSetTo, Type type, object setOn)
        {
            IteratePropertiesPerformAction(type, setOn, new ReflectionPropertyAction(delegate(object performOn, PropertyInfo property)
            {
                if (property.Name.Equals(propertyName))
                {
                    object value = GetPropertyValue(property.GetType().Name, valueSetTo);
                    if (value != null)
                        property.SetValue(setOn, value);
                }
            }));
        }
        public static void FindAndSetProperty(this object setOn, string propertyName, byte[] serialisedValue)
        {
            IteratePropertiesPerformAction(setOn, new ReflectionPropertyAction(delegate(object performOn, PropertyInfo property)
            {
                if (property.Name.Equals(propertyName))
                {
                    object value = BinarySerialisation.DeserialiseObject(serialisedValue);
                    if (value != null)
                        property.SetValue(setOn, value);
                }
            }));
        }
        public static void IteratePropertiesPerformAction(this object performOn, ReflectionPropertyAction Action)
        {
            IteratePropertiesPerformAction(performOn.GetType(), performOn, Action);
        }
        public static void IteratePropertiesPerformAction(this Type type, ReflectionPropertyAction Action)
        {
            IteratePropertiesPerformAction(type, null, Action);
        }
        private static void IteratePropertiesPerformAction(Type typeOfObject, object performOn, ReflectionPropertyAction Action)
        {
            if (performOn != null)
            {
                PropertyInfo[] properties = typeOfObject.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    Action(performOn, property);
                }
            }
        }

        public static Type FindTypeFromName(this string name)
        {
            Type retVal = null;
            List<Assembly> searchAssemblys = GetCurrentMatchingAssemblys();
            Type foundType = null;
            foreach (Assembly assembly in searchAssemblys)
            {
                if ((foundType = assembly.FindTypeInAssembly(name)) != null)
                {
                    return foundType;
                }
            }
            return retVal;
        }
        private static List<Assembly> GetCurrentMatchingAssemblys()
        {
            List<Assembly> retVal = new List<Assembly>();
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            string[] assemblyStringSplit = currentAssembly.FullName.Split('.');
            if (assemblyStringSplit.Length > 0)
            {
                Assembly[] domainAssembly = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly assembly in domainAssembly)
                {
                    if (assembly.FullName.Contains(assemblyStringSplit[0]))
                    {
                        retVal.Add(assembly);
                    }
                }
            }
            return retVal;
        }
        public static Type FindTypeInAssembly(this Assembly assembly, String name)
        {
            Type[] assemblyTypes = assembly.GetTypes();
            foreach (Type assmemblyType in assemblyTypes)
            {
                if (assmemblyType.Name.Equals(name))
                {
                    return assmemblyType;
                }
            }

            return null;
        }


        public static string GetCompany(this Assembly assembly)
        {
            AssemblyCompanyAttribute att = assembly.GetCustomAttribute(typeof(AssemblyCompanyAttribute)) as AssemblyCompanyAttribute;
            if (att != null)
            {
                return att.Company;
            }
            return string.Empty;
        }
        public static string GetDescription(this Assembly assembly)
        {
            AssemblyDescriptionAttribute att = assembly.GetCustomAttribute(typeof(AssemblyDescriptionAttribute)) as AssemblyDescriptionAttribute;
            if (att != null)
            {
                return att.Description;
            }
            return string.Empty;
        }
        public static string GetTitle(this Assembly assembly)
        {
            AssemblyTitleAttribute att = assembly.GetCustomAttribute(typeof(AssemblyTitleAttribute)) as AssemblyTitleAttribute;
            if (att != null)
            {
                return att.Title;
            }
            return string.Empty;
        }
        public static string GetVersion(this Assembly assembly)
        {
            AssemblyVersionAttribute att = assembly.GetCustomAttribute(typeof(AssemblyVersionAttribute)) as AssemblyVersionAttribute;
            if (att != null)
            {
                return att.Version;
            }
            return string.Empty;
        }
        public static string GetProduct(this Assembly assembly)
        {
            AssemblyProductAttribute att = assembly.GetCustomAttribute(typeof(AssemblyProductAttribute)) as AssemblyProductAttribute;
            if (att != null)
            {
                return att.Product;
            }
            return string.Empty;
        }
    }
}
