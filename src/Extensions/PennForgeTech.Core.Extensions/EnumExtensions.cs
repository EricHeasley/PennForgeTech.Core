using System;
using System.ComponentModel;
using System.Reflection;

namespace PennForgeTech.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            string name = value.ToString();
            FieldInfo? field = value.GetType().GetField(name);

            if (field is null)
            {
                return name;
            }

            DescriptionAttribute? attribute = field.GetCustomAttribute<DescriptionAttribute>();

            if (attribute is null)
            {
                return name;
            }

            return attribute.Description;
        }
    }
}
