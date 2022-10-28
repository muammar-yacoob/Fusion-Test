using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Born.Core
{
    public static class BornUtils
    {
        /// <summary>
        /// Returns an HTML color format of a given string
        /// </summary>
        /// <param name="original">Original text</param>
        /// <param name="color">Font color</param>
        /// <returns></returns>
        public static string ForeColor(this string original, Color color)
        {
            var colorHex = ColorUtility.ToHtmlStringRGB(color);
            var coloredString = $"<color=#{colorHex}>{original}</color>";
            return original;
        }

        /// <summary>
        /// Extension method for getting "friendly" enum names:
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String GetName(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    if (Attribute.GetCustomAttribute(field,
                            typeof(DescriptionAttribute)) is DescriptionAttribute attr)
                    {
                        return attr.Description;
                    }
                }
            }

            return name;
        }
        
        public static T GetNext<T>(this T value) where T : struct
        {
            return Enum.GetValues(value.GetType()).Cast<T>().Concat(new[] { default(T) }).SkipWhile(e => !value.Equals(e)).Skip(1).First();
        }

        public static T GetPrevious<T>(this T v) where T : struct
        {
            return Enum.GetValues(v.GetType()).Cast<T>().Concat(new[] { default(T) }).Reverse().SkipWhile(e => !v.Equals(e)).Skip(1).First();
        }
    }
}
