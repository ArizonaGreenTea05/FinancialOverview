using FinancialOverview;
using CommunityToolkit.Maui.Views;
using MauiMoneyMate.Utils.ResourceItemTemplates;
using System.ComponentModel;
using System.Globalization;

namespace MauiMoneyMate.Popups;

public partial class DetailsPopup : Popup
{
    private ResourceLabel SaleTextLbl;
    private ResourceLabel NameTextLbl;
    private ResourceLabel AdditionTextLbl;
    private ResourceLabel StartDateTextLbl;
    private ResourceLabel EndDateTextLbl;
    private ResourceLabel RepeatCycleTextLbl;
    private ResourceLabel RepeatCycleMultiplierTextLbl;
    private ResourceButton EditBtn;
    private ResourceButton DeleteBtn;

    private readonly SalesObject _salesObject;
    private readonly Action<SalesObject> _editAction;
    private readonly Action<SalesObject> _deleteAction;

    public DetailsPopup(SalesObject salesObject, Action<SalesObject> editAction, Action<SalesObject> deleteAction)
    {
        InitializeComponent();
        Size = new Size(800, 500);
        _salesObject = salesObject;
        _editAction = editAction;
        _deleteAction = deleteAction;
        LoadResources();
        LoadValues();
    }

    private void EditBtn_Clicked(object sender, EventArgs e)
    {
        _editAction.Invoke(_salesObject);
    }

    private void DeleteBtn_Clicked(object sender, EventArgs e)
    {
        Close();
        _deleteAction.Invoke(_salesObject);
    }

    private void LoadResources()
    {
        SaleTextLbl = new ResourceLabel(nameof(SaleTextLbl), SaleText);
        NameTextLbl = new ResourceLabel(nameof(NameTextLbl), NameText);
        AdditionTextLbl = new ResourceLabel(nameof(AdditionTextLbl), AdditionText);
        StartDateTextLbl = new ResourceLabel(nameof(StartDateTextLbl), StartDateText);
        EndDateTextLbl = new ResourceLabel(nameof(EndDateTextLbl), EndDateText);
        RepeatCycleTextLbl = new ResourceLabel(nameof(RepeatCycleTextLbl), RepeatCycleText);
        RepeatCycleMultiplierTextLbl = new ResourceLabel(nameof(RepeatCycleMultiplierTextLbl), RepeatCycleMultiplierText);
        EditBtn = new ResourceButton(nameof(EditBtn), Edit);
        DeleteBtn = new ResourceButton(nameof(DeleteBtn), Delete);
    }

    private void LoadValues()
    {
        Title.Text = _salesObject.Name;
        Sale.Text = _salesObject.ValueAsString;
        Name.Text = _salesObject.Name;
        Addition.Text = string.IsNullOrEmpty(_salesObject.Addition) ? "-" : _salesObject.Addition;
        StartDate.Text = _salesObject.StartDateAsString;
        EndDate.Text = _salesObject.EndDateAsString;
        RepeatCycle.Text = _salesObject.RepeatCycleAsString;
        RepeatCycleMultiplier.Text = _salesObject.RepeatCycleMultiplier.ToString();
    }
}
