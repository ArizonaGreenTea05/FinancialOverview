using System.Collections.ObjectModel;
using System.Globalization;
using FinancialOverview;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using MauiMoneyMate.Translations;
using MauiMoneyMate.Utils;
using MauiMoneyMate.Utils.ResourceItemTemplates;
using static FinancialOverview.Enums;

namespace MauiMoneyMate.Popups;

public partial class NewSalePopup : Popup
{
    private SalesObject _salesObject;
    private readonly int _indexOfObject;

    public NewSalePopup(int indexOfObject = -1)
	{
        _indexOfObject = indexOfObject;
        _salesObject = _indexOfObject >= 0 ? CommonProperties.FinancialOverview.Sales[_indexOfObject] : null;
        InitializeComponent();
        HorizontalOptions = Microsoft.Maui.Primitives.LayoutAlignment.End;
        VerticalOptions = Microsoft.Maui.Primitives.LayoutAlignment.End;
        CanBeDismissedByTappingOutsideOfPopup = false;
        NewSaleTitleLbl.LoadFromResource(nameof(NewSaleTitleLbl));
        CancelBtn.LoadFromResource(nameof(CancelBtn));
        SaveBtn.LoadFromResource(nameof(SaveBtn));
        SalesEntry.LoadFromResource(nameof(SalesEntry));
        NameEntry.LoadFromResource(nameof(NameEntry));
        AdditionEntry.LoadFromResource(nameof(AdditionEntry));
        IsOneTimeOrderLbl.LoadFromResource(nameof(IsOneTimeOrderLbl));
        IsOneTimeOrderSwitch.IsToggled = false;
        StartDateLbl.LoadFromResource(nameof(StartDateLbl));
        StartDatePkr.Format = CommonProperties.DatePickerFormat;
        EndDatePkr.Format = CommonProperties.DatePickerFormat;
        EndDateLbl.LoadFromResource(nameof(EndDateLbl));
        RepeatCycleTextLbl.LoadFromResource(nameof(RepeatCycleTextLbl));
        RepeatCyclePkr.Items.Clear();
        foreach (var item in GetLocalizedList(typeof(SaleRepeatCycle)))
            RepeatCyclePkr.Items.Add(item);
        RepeatCyclePkr.SelectedIndex = 2;
        RepeatCycleMultiplierTextLbl.LoadFromResource(nameof(RepeatCycleMultiplierTextLbl));
        RepeatCycleMultiplierEntry.LoadFromResource(nameof(RepeatCycleMultiplierEntry));
        RepeatCycleMultiplierEntry.Text = "1";

        if (_salesObject == null) return;
        SalesEntry.Text = _salesObject.Value.ToString(CultureInfo.CurrentCulture);
        NameEntry.Text = _salesObject.Name;
        AdditionEntry.Text = _salesObject.Addition;
        IsOneTimeOrderSwitch.IsToggled = _salesObject.StartDate ==  _salesObject.EndDate;
        StartDatePkr.Date = _salesObject.StartDate;
        EndDatePkr.Date = _salesObject.EndDate;
        RepeatCyclePkr.SelectedIndex = (int)_salesObject.RepeatCycle;
        RepeatCycleMultiplierEntry.Text = _salesObject.RepeatCycleMultiplier.ToString();
    }

    private void IsOneTimeOrderSwitch_OnToggled(object sender, ToggledEventArgs e)
    {
        EndDatePkr.IsEnabled = RepeatCyclePkr.IsEnabled = RepeatCycleMultiplierEntry.IsEnabled = !e.Value;
        EndDatePkr.Date = StartDatePkr.Date;
        if (e.Value)
        {
            RepeatCyclePkr.SelectedIndex = 0;
            RepeatCycleMultiplierEntry.Text = "0";
        }
        else
        {
            RepeatCyclePkr.SelectedIndex = 2;
            RepeatCycleMultiplierEntry.Text = "1";
        }
    }

    private void StartDatePkr_DateSelected(object sender, DateChangedEventArgs e)
    {
        if (IsOneTimeOrderSwitch.IsToggled) EndDatePkr.Date = StartDatePkr.Date;
    }

    private void Cancel_Clicked(object sender, EventArgs e)
    {
        Close();
    }

    private void Save_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            Toast.Make(string.Format(LanguageResource.IsNoValidName, NameEntry.Text)).Show();
            return;
        }

        if (string.IsNullOrWhiteSpace(SalesEntry.Text) || !decimal.TryParse(SalesEntry.Text, out var tmpSales))
        {
            Toast.Make(string.Format(LanguageResource.IsNoValidDecimal, SalesEntry.Text)).Show();
            return;
        }

        if (string.IsNullOrWhiteSpace(RepeatCycleMultiplierEntry.Text) ||
            !int.TryParse(RepeatCycleMultiplierEntry.Text, out var tmpRepeatCycleMultiplier))
        {
            Toast.Make(string.Format(LanguageResource.IsNoValidInteger, RepeatCycleMultiplierEntry.Text)).Show();
            return;
        }

        if (IsOneTimeOrderSwitch.IsToggled) tmpRepeatCycleMultiplier = 1;

        if (tmpRepeatCycleMultiplier <= 0)
        {
            Toast.Make(LanguageResource.ThereCanBeNoNegaticeOrNonExistentNumberOfRepetitions).Show();
            return;
        }

        var isEdit = _salesObject != null;

        _salesObject = new SalesObject(tmpSales, NameEntry.Text, AdditionEntry.Text, StartDatePkr.Date, EndDatePkr.Date,
            (SaleRepeatCycle)RepeatCyclePkr.SelectedIndex, tmpRepeatCycleMultiplier);

        if (!isEdit && CommonProperties.FinancialOverview.Sales.Any(item => item.Name == _salesObject.Name && item.Value == _salesObject.Value))
        {
            Toast.Make(LanguageResource.ThisEntryAlreadyExists).Show();
            return;
        }

        if (_indexOfObject < 0) CommonProperties.FinancialOverview.Sales.Add(_salesObject);
        else _salesObject.CopyTo(CommonProperties.FinancialOverview.Sales[_indexOfObject]);
        Close();
    }
}