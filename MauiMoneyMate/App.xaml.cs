using MauiMoneyMate.Utils;

namespace MauiMoneyMate;

public partial class App : Application
{
    public static Window Window;

    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        Window = base.CreateWindow(activationState);
        Window.Title = nameof(MauiMoneyMate);
        CommonFunctions.RemoveNonZipFiles(CommonProperties.UpdateDirectory);
        return Window;
    }
}