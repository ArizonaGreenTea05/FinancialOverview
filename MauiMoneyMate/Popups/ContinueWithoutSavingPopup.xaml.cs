using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using MauiMoneyMate.Resources.Languages;
using MauiMoneyMate.Utils.ResourceItemTemplates;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using ABI.Windows.Media.Control;

namespace MauiMoneyMate.Popups;

public partial class ContinueWithoutSaving : Popup
{
    private ResourceLabel UpdateTitleLbl;
    private ResourceLabel UpdateInfoLbl;
    private ResourceButton UpdateBtn;

    public ContinueWithoutSaving()
    {
        InitializeComponent();
        UpdateTitleLbl = new ResourceLabel(nameof(UpdateTitleLbl), Title);
    }
}