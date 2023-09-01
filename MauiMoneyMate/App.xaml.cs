using MauiMoneyMate.Utils;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using WinRT.Interop;

namespace MauiMoneyMate;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
        WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
        {
            var nativeWindow = handler.PlatformView;
            nativeWindow.Activate();
            var appWindow =
                AppWindow.GetFromWindowId(
                    Win32Interop.GetWindowIdFromWindow(WindowNative.GetWindowHandle(nativeWindow)));
            appWindow.Closing += CommonFunctions.AppWindow_Closing;
        });
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);
        return window;
    }
}