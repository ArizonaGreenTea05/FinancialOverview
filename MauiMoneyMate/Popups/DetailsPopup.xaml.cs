using FinancialOverview;
using CommunityToolkit.Maui.Views;
using MauiMoneyMate.Utils.ResourceItemTemplates;
using System.ComponentModel;
using System.Globalization;
using MauiMoneyMate.Utils;

namespace MauiMoneyMate.Popups;

public partial class DetailsPopup : Popup
{
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
        SaleTextLbl.LoadFromResource(nameof(SaleTextLbl));
        NameTextLbl.LoadFromResource(nameof(NameTextLbl));
        AdditionTextLbl.LoadFromResource(nameof(AdditionTextLbl));
        StartDateTextLbl.LoadFromResource(nameof(StartDateTextLbl));
        EndDateTextLbl.LoadFromResource(nameof(EndDateTextLbl));
        RepeatCycleTextLbl.LoadFromResource(nameof(RepeatCycleTextLbl));
        RepeatCycleMultiplierTextLbl.LoadFromResource(nameof(RepeatCycleMultiplierTextLbl));
        EditBtn.LoadFromResource(nameof(EditBtn));
        DeleteBtn.LoadFromResource(nameof(DeleteBtn));
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
