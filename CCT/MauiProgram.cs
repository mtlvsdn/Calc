using Microsoft.Extensions.Logging;

namespace CalculatorApp;

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

        builder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddHandler(typeof(Microsoft.Maui.Controls.Image), typeof(Microsoft.Maui.Handlers.ImageHandler));
        });


#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
} 