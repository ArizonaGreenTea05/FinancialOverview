using System.ComponentModel;
using System.Globalization;
using static FinancialOverview.Enums;

namespace FinancialOverview
{
    public class SalesObject
    {
        #region Properties

        public SalesObject? Parent { get; set; } = null;

        public string StartDateAsString => StartDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
        private DateTime _startDate = DateTime.MinValue;
        public DateTime StartDate
        {
            get => Parent?.StartDate ?? _startDate;
            set
            {
                if (Parent != null) return;
                _startDate = value;
            }
        }

        public string EndDateAsString => EndDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
        private DateTime _endDate = DateTime.MaxValue;
        public DateTime EndDate
        {
            get => Parent?.EndDate ?? _endDate;
            set
            {
                if (Parent != null) return;
                _endDate = value;
            }
        }

        public string DateSpanAsString => $"{StartDateAsString} - {EndDateAsString}";

        public string RepeatCycleAsString => TypeDescriptor.GetConverter(typeof(SaleRepeatCycle)).ConvertToString(null, CultureInfo.CurrentUICulture, RepeatCycle) ?? RepeatCycle.ToString();
        private SaleRepeatCycle _repeatCycle = SaleRepeatCycle.Monthly;
        public SaleRepeatCycle RepeatCycle
        {
            get => Parent?.RepeatCycle ?? _repeatCycle;
            set
            {
                if (Parent != null) return;
                _repeatCycle = value;
            }
        }

        private int _repeatCycleMultiplier = 1;
        public int RepeatCycleMultiplier
        {
            get => Parent?.RepeatCycleMultiplier ?? _repeatCycleMultiplier;
            set
            {
                if (Parent != null) return;
                _repeatCycleMultiplier = value;
            }
        }

        public string FullRepeatCycleAsString => $"{RepeatCycleAsString}, {Strings.RepeatEvery} {RepeatCycleMultiplier}";

        public decimal Value { get; set; }
        public string ValueAsString => string.Format(CultureInfo.CurrentCulture, "{0:#,##0.00}", Math.Round(Value, 2));

        public string Name { get; set; }

        public string Addition { get; set; }

        #endregion

        #region Constructors

        private SalesObject()
        {
        }

        private SalesObject(SalesObject parent, int valueMultiplier)
        {
            Parent = parent;
            Parent.CopyTo(this);
            Value *= valueMultiplier;
            Name = $"{Name} ({valueMultiplier}x)";
        }

        public SalesObject(decimal value, string name, string addition, DateTime startDate, DateTime endDate,
            SaleRepeatCycle repeatCycle, int repeatCycleMultiplier)
        {
            if (value == 0) throw new ArgumentException("value cannot be 0");
            Value = value;
            Name = name;
            Addition = addition;
            StartDate = startDate;
            EndDate = endDate;
            RepeatCycle = repeatCycle;
            RepeatCycleMultiplier = repeatCycleMultiplier;
        }

        #endregion

        #region public Methods

        public SalesObject? GetForRange(int year)
        {
            return GetForRange(Month.WholeYear, year);
        }

        public SalesObject? GetForRange(Month month, int year)
        {
            var count = GetCountForRange(month, year);
            return count switch
            {
                <= 0 => null,
                1 => this,
                _ => new SalesObject(this, count)
            };
        }

        public SalesObject Copy()
        {
            return new SalesObject(Value, Name, Addition, StartDate, EndDate, RepeatCycle, RepeatCycleMultiplier);
        }

        public void CopyTo(SalesObject salesObject)
        {
            salesObject.Value = Value;
            salesObject.Name = Name;
            salesObject.Addition = Addition;
            salesObject.StartDate = StartDate;
            salesObject.EndDate = EndDate;
            salesObject.RepeatCycle = RepeatCycle;
            salesObject.RepeatCycleMultiplier = RepeatCycleMultiplier;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SalesObject tmp)) return false;
            return Value == tmp.Value
                   && Name == tmp.Name
                   && Addition == tmp.Addition
                   && StartDate == tmp.StartDate
                   && EndDate == tmp.EndDate
                   && RepeatCycle == tmp.RepeatCycle
                   && RepeatCycleMultiplier == tmp.RepeatCycleMultiplier;
        }

        public int GetCountForRange(int year)
        {
            if (StartDate.Year > year) return 0;
            if (EndDate.Year < year) return 0;

            var tmpDate = StartDate;
            while (tmpDate.Year < year)
                tmpDate = RepeatCycle switch
                {
                    SaleRepeatCycle.Daily => tmpDate.AddDays(RepeatCycleMultiplier),
                    SaleRepeatCycle.Weekly => tmpDate.AddDays(7 * RepeatCycleMultiplier),
                    SaleRepeatCycle.Monthly => tmpDate.AddMonths(RepeatCycleMultiplier),
                    SaleRepeatCycle.Yearly => tmpDate.AddYears(RepeatCycleMultiplier),
                    _ => tmpDate
                };
            if (tmpDate.Year != year || tmpDate.Year > EndDate.Year) return 0;
            var count = 0;
            for (; tmpDate.Year == year && tmpDate <= EndDate; ++count)
                tmpDate = RepeatCycle switch
                {
                    SaleRepeatCycle.Daily => tmpDate.AddDays(RepeatCycleMultiplier),
                    SaleRepeatCycle.Weekly => tmpDate.AddDays(7 * RepeatCycleMultiplier),
                    SaleRepeatCycle.Monthly => tmpDate.AddMonths(RepeatCycleMultiplier),
                    SaleRepeatCycle.Yearly => tmpDate.AddYears(RepeatCycleMultiplier),
                    _ => tmpDate
                };
            return count;
        }

        public int GetCountForRange(Month month, int year)
        {
            if (month == Month.WholeYear) return GetCountForRange(year);
            if (StartDate.Year > year) return 0;
            if (EndDate.Year < year) return 0;
            var tmpDate = StartDate;
            while (tmpDate.Year < year || tmpDate.Month < Convert.ToInt32(month))
            {
                tmpDate = RepeatCycle switch
                {
                    SaleRepeatCycle.Daily => tmpDate.AddDays(RepeatCycleMultiplier),
                    SaleRepeatCycle.Weekly => tmpDate.AddDays(7 * RepeatCycleMultiplier),
                    SaleRepeatCycle.Monthly => tmpDate.AddMonths(RepeatCycleMultiplier),
                    SaleRepeatCycle.Yearly => tmpDate.AddYears(RepeatCycleMultiplier),
                    _ => tmpDate
                };
                if (tmpDate.Year > year) return 0;
            }

            if (tmpDate.Year > EndDate.Year) return 0;
            if (tmpDate.Year == EndDate.Year && tmpDate.Month > EndDate.Month) return 0;
            if (tmpDate.Year != year || tmpDate.Month != Convert.ToInt32(month)) return 0;
            var count = 0;
            for (; tmpDate.Year == year && tmpDate.Month == Convert.ToInt32(month) && tmpDate <= EndDate; ++count)
                tmpDate = RepeatCycle switch
                {
                    SaleRepeatCycle.Daily => tmpDate.AddDays(RepeatCycleMultiplier),
                    SaleRepeatCycle.Weekly => tmpDate.AddDays(7 * RepeatCycleMultiplier),
                    SaleRepeatCycle.Monthly => tmpDate.AddMonths(RepeatCycleMultiplier),
                    SaleRepeatCycle.Yearly => tmpDate.AddYears(RepeatCycleMultiplier),
                    _ => tmpDate
                };
            return count;
        }

        #endregion
    }
}
