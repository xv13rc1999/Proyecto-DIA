using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Taller.Core;
using Taller.Core.Busquedas;
using Taller.Views;

namespace Taller;

public partial class MainWindow : Window
{
    private ContenedorDatos _datosTaller;
    
    public MainWindow()
    {
        InitializeComponent();

        // Botón del menú
        var btnToggle = this.GetControl<Button>("BtnToggle");
        var menuLateral = this.GetControl<SplitView>("MenuLateral");
        btnToggle.Click += (_, _) => menuLateral.IsPaneOpen = !menuLateral.IsPaneOpen;

        _datosTaller = CargarDatos("datosTaller.xml");

        // Navegación
        //this.GetControl<Button>("BtnClientes").Click += (_, _) => CambiarVista(new VistaClientes());
        //this.GetControl<Button>("BtnCoches").Click += (_, _) => CambiarVista(new VistaCoches());
        this.GetControl<Button>("BtnPersonal").Click += (_, _) => CambiarVista(new VistaPersonal(_datosTaller));
        //this.GetControl<Button>("BtnReparaciones").Click += (_, _) => CambiarVista(new VistaReparaciones());
        this.GetControl<Button>("BtnGraficos").Click += (_, _) => CambiarVista(new Estadisticas());

        // Cargar tu módulo por defecto al abrir
        CambiarVista(new VistaPersonal(_datosTaller));
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void CambiarVista(UserControl nuevaVista)
    {
        var areaContenido = this.GetControl<ContentControl>("AreaContenido");
        areaContenido.Content = nuevaVista;
    }

    private void GuardarDatos()
    {
        Utils.PersistenciaXML.Guardar<ContenedorDatos>("datosTaller.xml", _datosTaller);
    }

    private ContenedorDatos CargarDatos(string? rutaArchivo = null)
    {
        ContenedorDatos toret = new();
        
        try
        {
            if (!string.IsNullOrEmpty(rutaArchivo))
            {
                toret = Utils.PersistenciaXML.Cargar<ContenedorDatos>(rutaArchivo);
            }
        }
        catch(System.Exception) {}

        return toret;
    }
}