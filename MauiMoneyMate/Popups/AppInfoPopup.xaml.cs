using CommunityToolkit.Maui.Views;
using MauiMoneyMate.Utils;

namespace MauiMoneyMate.Popups;

public partial class AppInfoPopup : Popup
{
    public AppInfoPopup()
    {
        InitializeComponent();
        Size = new Size(400, 250);
        ApplicationNameLbl.LoadFromResource(nameof(ApplicationNameLbl));
        VersionLbl.LoadFromResource(nameof(VersionLbl));
        VersionLbl.Text = string.Format(VersionLbl.Text,
            $"{CommonProperties.CurrentVersion.VersionId}{(CommonProperties.CurrentVersion.VersionId[0] == '0' ? " (Beta)" : "")}");
        CopyrightLbl.LoadFromResource(nameof(CopyrightLbl));
        RightsReservedLbl.LoadFromResource(nameof(RightsReservedLbl));
        CreditsLbl.LoadFromResource(nameof(CreditsLbl));
        CreditsLbl.Text = string.Format(CreditsLbl.Text, "Nindo.de", "Openai.com/chatgpt");
    }
}