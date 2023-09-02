using CommunityToolkit.Maui.Views;
using MauiMoneyMate.Utils;

namespace MauiMoneyMate.Popups;

public partial class UnsavedChangesPopup : Popup
{
    private Window _window;

    public UnsavedChangesPopup(Window window)
    {
        _window = window;
        InitializeComponent();
        CanBeDismissedByTappingOutsideOfPopup = false;
        UnsavedChangesTitle.LoadFromResource(nameof(UnsavedChangesTitle));
        UnsavedChangesTextLbl.LoadFromResource(nameof(UnsavedChangesTextLbl));
        SaveAndCloseBtn.LoadFromResource(nameof(SaveAndCloseBtn));
        CloseWithoutSavingBtn.LoadFromResource(nameof(CloseWithoutSavingBtn));
        CancelBtn.LoadFromResource(nameof(CancelBtn));
    }

    private void CloseApplication()
    {
        Application.Current.CloseWindow(_window);
        Environment.Exit(1);
    }

    private void SaveAndCloseBtn_OnClicked(object sender, EventArgs e)
    {
        if (CommonFunctions.SaveFileAction()) CloseApplication();
    }

    private void CloseWithoutSavingBtn_OnClicked(object sender, EventArgs e)
    {
        CloseApplication();
    }

    private void CancelBtn_OnClicked(object sender, EventArgs e)
    {
        Close();
    }
}