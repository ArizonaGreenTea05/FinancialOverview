using BusinessLogic;
using MauiMoneyMate.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            var financialOverview = new BusinessLogic.FinancialOverview();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton(new MainViewModel(ref financialOverview));

            builder.Services.AddTransient<FilePage>();
            builder.Services.AddSingleton(new FileViewModel(ref financialOverview));

            return builder.Build();
        }
    }
}