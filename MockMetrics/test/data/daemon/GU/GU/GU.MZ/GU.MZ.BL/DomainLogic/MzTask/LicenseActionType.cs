using System;
using System.ComponentModel;
using System.Reflection;

namespace GU.MZ.BL.DomainLogic.MzTask
{
    /// <summary>
    /// ��� ������ �������������� �� ���� �������� ��� ���������
    /// </summary>
    public enum LicenseActionType
    {
        [Description("�������������� ��������")]
        [EniseyNameAttribute("New")]
        New,

        [Description("�������������� ��������")]
        [EniseyNameAttribute("Reregistration")]
        Renewal,

        [Description("����������� ��������")]
        [EniseyNameAttribute("Stop")]
        Stop,

        [Description("����� ��������")]
        [EniseyNameAttribute("Copy")]
        Copy,

        [Description("�������� ��������")]
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