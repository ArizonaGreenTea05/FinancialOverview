﻿using MauiMoneyMate.Utils;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using WinRT.Interop;
using CommunityToolkit.Maui.Alerts;
using MauiMoneyMate.Translations;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

using Microsoft.UI.Xaml;
using Microsoft.Windows.AppLifecycle;
using Windows.ApplicationModel.Activation;
using CommunityToolkit.Maui.Alerts;
using MauiMoneyMate.Utils;
using LaunchActivatedEventArgs = Microsoft.UI.Xaml.LaunchActivatedEventArgs;

namespace MauiMoneyMate.Platforms.Windows
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
            {
                var nativeWindow = handler.PlatformView;
                nativeWindow.Activate();
                var appWindow =
                    AppWindow.GetFromWindowId(
                        Win32Interop.GetWindowIdFromWindow(WindowNative.GetWindowHandle(nativeWindow)));
                appWindow.Closing += (sender, args) =>
                {
                    args.Cancel = true;
                    CommonFunctions.ExitAction();
                };
            });

            var currentProcess = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(currentProcess.ProcessName);
            if (processes.Length > 1)
            {
                Toast.Make(LanguageResource.AnInstanceOfMauiMoneyMateAlreadyExists).Show();
                CommonFunctions.SetForegroundWindow(processes.Where(p => p.Id != currentProcess.Id)
                    .Select(p => p.MainWindowHandle).ToList()[0]);
                Environment.Exit(1);
            }
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            var goodArgs = AppInstance.GetCurrent().GetActivatedEventArgs();

            switch (goodArgs.Kind)
            {
                case ExtendedActivationKind.File:
                    var data = goodArgs.Data as IFileActivatedEventArgs;
                    var paths = data.Files.Select(file => file.Path).ToArray();
                    CommonProperties.FilePathFromEventArgs = paths[0];
                    break;
            }
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}