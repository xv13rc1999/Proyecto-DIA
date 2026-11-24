using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Taller.Core.Coches;
using Taller.UI.ViewModels;
using Taller.UI.Views;

namespace Taller.UI;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Los datos se guardan en %APPDATA%\TallerDIA\coches.xml
            string fichero = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "TallerDIA", "coches.xml");

            var coches = new CochesViewModel(new GestorCoches(), fichero);
            coches.RecuperaFichero(); // recuperación automática al arrancar

            var ventana = new MainWindow
            {
                DataContext = new MainViewModel(coches),
            };
            ventana.Closing += (_, _) => coches.GuardaFichero(); // salvaguarda automática al cerrar

            desktop.MainWindow = ventana;
        }

        base.OnFrameworkInitializationCompleted();
    }
}