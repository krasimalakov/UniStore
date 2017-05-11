namespace UniStore.Models.Enums
{
    using System;
    using System.ComponentModel;

    public static class Enums
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            var attribute
                = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                    as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}