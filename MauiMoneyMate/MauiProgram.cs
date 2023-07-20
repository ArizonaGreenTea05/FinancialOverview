using CommunityToolkit.Maui;
using MauiMoneyMate.Pages;
using MauiMoneyMate.Popups;
using MauiMoneyMate.ViewModels;

namespace MauiMoneyMate
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Arial-Rounded.ttf", "ArialRounded");
                    fonts.AddFont("Arial-RoundedBold.ttf", "ArialRoundedBold");
                }).UseMauiCommunityToolkit();

            var financialOverview = new BusinessLogic.FinancialOverview();
            var commonVariables = new CommonVariables();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton(new MainViewModel(financialOverview, commonVariables));

            builder.Services.AddTransient<FilePage>();
            builder.Services.AddSingleton(new FileViewModel(financialOverview, commonVariables));

            return builder.Build();
        }
    }
}