using BusinessLogic;
using CommunityToolkit.Maui.Views;
using MauiMoneyMate.Utils.ResourceItemTemplates;

namespace MauiMoneyMate.Popups;

public partial class HelpPopup : Popup
{
    private ResourceLabel HelpTitleLbl;
    private ResourceLabel HelpInfoLbl;
    private ResourceButton NewIssueBtn;

    public HelpPopup()
	{
		InitializeComponent();
        HelpTitleLbl = new(nameof(HelpTitleLbl), Title);
        HelpInfoLbl = new(nameof(HelpInfoLbl), Info);
        NewIssueBtn = new(nameof(NewIssueBtn), NewIssue);
    }

    private void NewIssue_Clicked(object sender, EventArgs e)
    {
        Browser.Default.OpenAsync(Constants.NEW_ISSUE_URL, BrowserLaunchMode.SystemPreferred);
    }
}