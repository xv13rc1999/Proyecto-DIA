// Taller (c) 2026 DIA - Uvigo


namespace Taller.UI {
    using System;
    using Avalonia;


    /// <summary>Punto de entrada de la aplicación.</summary>
    class Program {
        /// <summary>Arranca la interfaz gráfica.</summary>
        /// <param name="args">Los argumentos de la línea de comandos.</param>
        [STAThread]
        public static void Main(string[] args)
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime( args );
        }

        /// <summary>Configura Avalonia; también lo usa el diseñador visual.</summary>
        /// <returns>El constructor de la aplicación configurado.</returns>
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
#if DEBUG
                .WithDeveloperTools()
#endif
                .WithInterFont()
                .LogToTrace();
    }
}
