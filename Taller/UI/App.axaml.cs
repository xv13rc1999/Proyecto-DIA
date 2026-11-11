// Taller (c) 2026 DIA - Uvigo


namespace Taller.UI {
    using Avalonia;
    using Avalonia.Controls.ApplicationLifetimes;
    using Avalonia.Markup.Xaml;


    /// <summary>La aplicación Avalonia.</summary>
    public class App: Application {
        /// <summary>Carga el XAML de la aplicación (estilos).</summary>
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load( this );
        }

        /// <summary>Crea la ventana principal al terminar de arrancar.</summary>
        public override void OnFrameworkInitializationCompleted()
        {
            if ( ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ) {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
