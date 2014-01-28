using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace SpecManager.BL
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum enumValue)
        {
            FieldInfo field = enumValue.GetType().GetField(enumValue.ToString());

            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attribute == null ? enumValue.ToString() : attribute.Description;
        }

        public static Dictionary<T, string> ToDictionary<T>(this Enum enumeration)
        {
            if (enumeration.GetType() != typeof(T))
            {
                throw new ArgumentException("enumType");
            }

            return Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(t => t, t => (t as Enum).GetDescription());
        }
    }
}