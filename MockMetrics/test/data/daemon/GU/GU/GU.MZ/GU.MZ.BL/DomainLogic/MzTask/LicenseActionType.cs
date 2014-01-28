using System;
using System.ComponentModel;
using System.Reflection;

namespace GU.MZ.BL.DomainLogic.MzTask
{
    /// <summary>
    /// Тип услуги лицензирования по типу действия над лицензией
    /// </summary>
    public enum LicenseActionType
    {
        [Description("Предоставление лицензии")]
        [EniseyNameAttribute("New")]
        New,

        [Description("Переоформление лицензии")]
        [EniseyNameAttribute("Reregistration")]
        Renewal,

        [Description("Прекращение лицензии")]
        [EniseyNameAttribute("Stop")]
        Stop,

        [Description("Копия лицензии")]
        [EniseyNameAttribute("Copy")]
        Copy,

        [Description("Дубликат лицензии")]
        [EniseyNameAttribute("Dublicat")]
        Duplicate
    }


    public static class EniseyUtils
    {
        public static string GetEniseyName(this Enum enumValue)
        {
            FieldInfo field = enumValue.GetType().GetField(enumValue.ToString());

            var attribute = Attribute.GetCustomAttribute(field, typeof(EniseyNameAttribute)) as EniseyNameAttribute;

            return attribute == null ? null : attribute.EniseyName;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class EniseyNameAttribute : Attribute
    {
        public string EniseyName { get; private set; }

        public EniseyNameAttribute(string eniseyName)
        {
            EniseyName = eniseyName;
        }
    }
}