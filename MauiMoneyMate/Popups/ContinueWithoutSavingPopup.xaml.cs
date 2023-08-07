using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using MauiMoneyMate.Resources.Languages;
using MauiMoneyMate.Utils.ResourceItemTemplates;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using ABI.Windows.Media.Control;

namespace MauiMoneyMate.Popups;

public partial class ContinueWithoutSavingPopup : Popup
{
    private ResourceLabel UpdateTitleLbl;

    public ContinueWithoutSavingPopup()
    {
        InitializeComponent();
        UpdateTitleLbl = new ResourceLabel(nameof(UpdateTitleLbl), Title);
    }
}