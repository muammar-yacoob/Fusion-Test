using System;
using System.ComponentModel;
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
        public static String GetDescription(this Enum value)
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
    }
}
