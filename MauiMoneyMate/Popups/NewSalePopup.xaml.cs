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
    private ResourceLabel NewSaleTitleLbl;
    private ResourceButton CancelBtn;
    private ResourceButton SaveBtn;
    private ResourceEntry SalesEntry;
    private ResourceEntry NameEntry;
    private ResourceEntry AdditionEntry;
    private ResourceLabel StartDateLbl;
    private ResourceLabel EndDateLbl;
    private ResourceLabel RepeatCycleTextLbl;
    private ResourceLabel RepeatCycleMultiplierTextLbl;
    private ResourceEntry RepeatCycleMultiplierEntry;
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
        NewSaleTitleLbl = new ResourceLabel(nameof(NewSaleTitleLbl), Title);
        CancelBtn = new ResourceButton(nameof(CancelBtn), Cancel);
        SaveBtn = new ResourceButton(nameof(SaveBtn), Save);
        SalesEntry = new ResourceEntry(nameof(SalesEntry), Sales);
        NameEntry = new ResourceEntry(nameof(NameEntry), Name);
        AdditionEntry = new ResourceEntry(nameof(AdditionEntry), Addition);
        new ResourceLabel(nameof(IsOneTimeOrderLbl), IsOneTimeOrderLbl);
        IsOneTimeOrderSwitch.IsToggled = false;
        StartDateLbl = new ResourceLabel(nameof(StartDateLbl), StartDateText);
        StartDatePkr.Format = CommonProperties.DatePickerFormat;
        EndDatePkr.Format = CommonProperties.DatePickerFormat;
        EndDateLbl = new ResourceLabel(nameof(EndDateLbl), EndDateText);
        RepeatCycleTextLbl = new ResourceLabel(nameof(RepeatCycleTextLbl), RepeatCycleText);
        RepeatCyclePkr.Items.Clear();
        foreach (var item in GetLocalizedList(typeof(SaleRepeatCycle)))
            RepeatCyclePkr.Items.Add(item);
        RepeatCyclePkr.SelectedIndex = 2;
        RepeatCycleMultiplierTextLbl = new ResourceLabel(nameof(RepeatCycleMultiplierTextLbl), RepeatCycleMultiplierText);
        RepeatCycleMultiplierEntry = new ResourceEntry(nameof(RepeatCycleMultiplierEntry), RepeatCycleMultiplier);
        RepeatCycleMultiplier.Text = "1";

        if (_salesObject == null) return;
        Sales.Text = _salesObject.Value.ToString(CultureInfo.CurrentCulture);
        Name.Text = _salesObject.Name;
        Addition.Text = _salesObject.Addition;
        IsOneTimeOrderSwitch.IsToggled = _salesObject.StartDate ==  _salesObject.EndDate;
        StartDatePkr.Date = _salesObject.StartDate;
        EndDatePkr.Date = _salesObject.EndDate;
        RepeatCyclePkr.SelectedIndex = (int)_salesObject.RepeatCycle;
        RepeatCycleMultiplier.Text = _salesObject.RepeatCycleMultiplier.ToString();
    }

    private void IsOneTimeOrderSwitch_OnToggled(object sender, ToggledEventArgs e)
    {
        EndDatePkr.IsEnabled = RepeatCyclePkr.IsEnabled = RepeatCycleMultiplier.IsEnabled = !e.Value;
        EndDatePkr.Date = StartDatePkr.Date;
        if (e.Value)
        {
            RepeatCyclePkr.SelectedIndex = 0;
            RepeatCycleMultiplier.Text = "0";
        }
        else
        {
            RepeatCyclePkr.SelectedIndex = 2;
            RepeatCycleMultiplier.Text = "1";
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
        if (string.IsNullOrWhiteSpace(Name.Text))
        {
            Toast.Make(string.Format(LanguageResource.IsNoValidName, Name.Text)).Show();
            return;
        }

        if (string.IsNullOrWhiteSpace(Sales.Text) || !decimal.TryParse(Sales.Text, out var tmpSales))
        {
            Toast.Make(string.Format(LanguageResource.IsNoValidDecimal, Sales.Text)).Show();
            return;
        }

        if (string.IsNullOrWhiteSpace(RepeatCycleMultiplier.Text) ||
            !int.TryParse(RepeatCycleMultiplier.Text, out var tmpRepeatCycleMultiplier))
        {
            Toast.Make(string.Format(LanguageResource.IsNoValidInteger, RepeatCycleMultiplier.Text)).Show();
            return;
        }

        if (IsOneTimeOrderSwitch.IsToggled) tmpRepeatCycleMultiplier = 1;

        if (tmpRepeatCycleMultiplier <= 0)
        {
            Toast.Make(LanguageResource.ThereCanBeNoNegaticeOrNonExistentNumberOfRepetitions).Show();
            return;
        }

        var isEdit = _salesObject != null;

        _salesObject = new SalesObject(tmpSales, Name.Text, Addition.Text, StartDatePkr.Date, EndDatePkr.Date,
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