using System.ComponentModel;
using System.Globalization;
using FinancialOverview;

namespace FinancialOverview
{
    public class Enums
    {
        [TypeConverter(typeof(LocalizedEnumConverter))]
        public enum Month
        {
            [Description("WholeYear")] WholeYear,
            [Description("January")] January,
            [Description("February")] February,
            [Description("March")] March,
            [Description("April")] April,
            [Description("May")] May,
            [Description("June")] June,
            [Description("July")] July,
            [Description("August")] August,
            [Description("September")] September,
            [Description("October")] October,
            [Description("November")] November,
            [Description("December")] December
        }

        [TypeConverter(typeof(LocalizedEnumConverter))]
        public enum SaleRepeatCycle
        {
            [Description("Daily")] Daily,
            [Description("Weekly")] Weekly,
            [Description("Monthly")] Monthly,
            [Description("Yearly")] Yearly
        }

        public static List<string> GetLocalizedList(Type enumType, CultureInfo culture = null)
        {
            culture = culture ?? CultureInfo.CurrentUICulture;
            var converter = TypeDescriptor.GetConverter(enumType);
            return Enum.GetValues(enumType).Cast<Enum>().Select(item => converter.ConvertToString(null, culture, item)).ToList();
        }
    }
}
