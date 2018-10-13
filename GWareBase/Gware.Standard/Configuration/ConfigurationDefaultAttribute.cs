using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Gware.Standard.Configuration
{
    public class ConfigurationDefaultAttribute : Attribute
    {
        public byte[] Default { get; }

        public ConfigurationDefaultAttribute(bool defaultValue)
        {
            Default = BitConverter.GetBytes(defaultValue);
        }
        public ConfigurationDefaultAttribute(int defaultValue)
        {
            Default = BitConverter.GetBytes(defaultValue);
        }
        public ConfigurationDefaultAttribute(long defaultValue)
        {
            Default = BitConverter.GetBytes(defaultValue);
        }
        public ConfigurationDefaultAttribute(string defaultValue)
        {
            Default = Encoding.Unicode.GetBytes(defaultValue);
        }
        public ConfigurationDefaultAttribute(DateTime defaultValue)
        {
            Default = BitConverter.GetBytes(defaultValue.Ticks);
        }
        public ConfigurationDefaultAttribute(IConfigurationType configurationType)
        {
            Default = configurationType.GetBytes();
        }

        public static byte[] GetDefaultValue<T>(T value) where T : Enum
        {
            MemberInfo[] memberInfo = typeof(T).GetMember(value.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(ConfigurationDefaultAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((ConfigurationDefaultAttribute)attrs[0]).Default;
                }
            }
            return null;
        }
    }
}
