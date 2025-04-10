using MauiAppArvoredo.Platforms.Android.Handlers;
using MauiAppArvoredo.Controls;
using Microsoft.Extensions.Logging;

namespace MauiAppArvoredo
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

            // Registro do handler personalizado
#if ANDROID
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("CustomEntryMapper", (handler, view) =>
            {
                if (view is CustomEntry customEntry)
                {
                    // Aplica o handler personalizado
                    CustomEntryHandler.MapBackground((CustomEntryHandler)handler, customEntry);
                }
            });
#endif

            return builder.Build();
        }
    }
}