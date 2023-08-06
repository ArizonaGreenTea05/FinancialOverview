using MauiMoneyMate.Utils;

namespace MauiMoneyMate;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);
        window.Title = nameof(MauiMoneyMate);
        CommonFunctions.RemoveNonZipFiles(CommonProperties.UpdateDirectory);
        window.Destroying += (sender, args) =>
        {
            Console.WriteLine("destroying");
        };
        return window;
    }
}