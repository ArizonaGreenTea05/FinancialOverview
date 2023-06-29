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
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

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