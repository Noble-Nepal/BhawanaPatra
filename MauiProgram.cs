using BhawanaPatra.Database;
using BhawanaPatra.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor;
using MudBlazor.Services;
using SQLite;
namespace BhawanaPatra
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
                });
            // SQLite Connection Setup
            builder.Services.AddSingleton<DatabaseConfiguration>(s=>new DatabaseConfiguration(ConnectionService.DatabasePath));

            // Register UserService for DI
            builder.Services.AddScoped<UserService>();
            builder.Services.AddSingleton<EntryService>();

            //Razor and MudBlazor
            builder.Services.AddMauiBlazorWebView();
            IServiceCollection serviceCollection = builder.Services.AddMudServices();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
            builder.Services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.ShowCloseIcon = true;
                config.SnackbarConfiguration.VisibleStateDuration = 3000;
            });
            
#endif

            return builder.Build();
        }
    }
}
