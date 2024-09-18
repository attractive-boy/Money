using Microsoft.Extensions.Logging;
using System;

namespace money
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

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            // Configure DatabaseService with SQLite
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "app.db");
            builder.Services.AddSingleton<DatabaseService>(s => new DatabaseService(dbPath));

            // Set the expiration date
            var expirationDate = new DateTime(2024, 8, 10); // Set your desired expiration date

            // Check if current date exceeds the expiration date
            if (DateTime.Now > expirationDate)
            {
                // Handle the expiration logic
                HandleExpiration();
            }

            return builder.Build();
        }

        private static void HandleExpiration()
        {
            // Show an expiration message and terminate the app or disable functionality
            Console.WriteLine("This application has expired and can no longer be used.");

            // For example, terminate the app (for demonstration purposes)
            Environment.Exit(0);
        }
    }
}
