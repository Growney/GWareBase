using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Gware.Standard.Data
{
    public class EnumDisplayAttribute : Attribute
    {
        public string Text { get; }

        public EnumDisplayAttribute(string text)
        {
            Text = text;
        }

        public static string GetEnumString<T>(T value) where T : Enum
        {
            MemberInfo[] memberInfo = typeof(T).GetMember(value.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(EnumDisplayAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((EnumDisplayAttribute)attrs[0]).Text;
                }
            }
            return value.ToString();
        }
    }
}
