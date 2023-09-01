using CommunityToolkit.Maui.Views;
using MauiMoneyMate.Utils;
using MauiMoneyMate.Utils.ResourceItemTemplates;

namespace MauiMoneyMate.Popups;

public partial class UnsavedChangesPopup : Popup
{
    private Window _window;

    public UnsavedChangesPopup(Window window)
    {
        _window = window;
        InitializeComponent();
        CanBeDismissedByTappingOutsideOfPopup = false;
        new ResourceLabel(nameof(UnsavedChangesTitle), UnsavedChangesTitle);
        new ResourceLabel(nameof(UnsavedChangesTextLbl), UnsavedChangesTextLbl);
        new ResourceButton(nameof(SaveAndCloseBtn), SaveAndCloseBtn);
        new ResourceButton(nameof(CloseWithoutSavingBtn), CloseWithoutSavingBtn);
        new ResourceButton(nameof(CancelBtn), CancelBtn);
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