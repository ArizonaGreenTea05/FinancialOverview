using CommunityToolkit.Maui.Views;
using MauiMoneyMate.Utils;

namespace MauiMoneyMate.Popups;

public partial class UnsavedChangesPopup : Popup
{
    private readonly Action<object> _actionAfterConfirmation;

    public UnsavedChangesPopup(string unsavedChangesTextAddition, Action<object> actionAfterConfirmation)
    {
        _actionAfterConfirmation = actionAfterConfirmation;
        InitializeComponent();
        CanBeDismissedByTappingOutsideOfPopup = false;
        UnsavedChangesTitle.LoadFromResource(nameof(UnsavedChangesTitle));
        UnsavedChangesTextLbl.LoadFromResource(nameof(UnsavedChangesTextLbl));
        UnsavedChangesTextLbl.Text += $" {unsavedChangesTextAddition}";
        SaveAndContinueBtn.LoadFromResource(nameof(SaveAndContinueBtn));
        ContinueWithoutSavingBtn.LoadFromResource(nameof(ContinueWithoutSavingBtn));
        CancelBtn.LoadFromResource(nameof(CancelBtn));
    }

    private void SaveAndContinueBtn_OnClicked(object sender, EventArgs e)
    {
        if (CommonFunctions.SaveFileAction()) _actionAfterConfirmation.Invoke(null);
        Close();
    }

    private void ContinueWithoutSavingBtn_OnClicked(object sender, EventArgs e)
    {
        _actionAfterConfirmation.Invoke(null);
        Close();
    }

    private void CancelBtn_OnClicked(object sender, EventArgs e)
    {
        Close();
    }
}