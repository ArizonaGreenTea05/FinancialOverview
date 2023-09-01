using System.ComponentModel;
using System.Globalization;

namespace FinancialOverview
{
    internal sealed class LocalizedEnumConverter : EnumConverter
    {
        public LocalizedEnumConverter(Type type) : base(type)
        {
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || destinationType == typeof(int);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            if (value == null) return string.Empty;
            if (destinationType == typeof(int)) return Convert.ToInt32(value);
            var fieldInfo = value.GetType().GetField(value.ToString());
            if (destinationType != typeof(string)) return string.Empty;
            var attribute = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault() as DescriptionAttribute;

            if (attribute != null)
                return Strings.ResourceManager.GetString(attribute.Description, culture) ?? value.ToString();

            return value.ToString();
        }
    }
}
