using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TallerApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Botón del menú
        var btnToggle = this.GetControl<Button>("BtnToggle");
        var menuLateral = this.GetControl<SplitView>("MenuLateral");
        btnToggle.Click += (_, _) => menuLateral.IsPaneOpen = !menuLateral.IsPaneOpen;

        // Navegación
        this.GetControl<Button>("BtnClientes").Click += (_, _) => CambiarVista(new VistaClientes());
        this.GetControl<Button>("BtnCoches").Click += (_, _) => CambiarVista(new VistaCoches());
        this.GetControl<Button>("BtnPersonal").Click += (_, _) => CambiarVista(new VistaPersonal());
        this.GetControl<Button>("BtnReparaciones").Click += (_, _) => CambiarVista(new VistaReparaciones());
        this.GetControl<Button>("BtnBusquedas").Click += (_, _) => CambiarVista(new VistaBusquedas());
        
        // Carga tu clase actual Estadisticas en lugar de VistaGraficos
        this.GetControl<Button>("BtnGraficos").Click += (_, _) => CambiarVista(new Estadisticas());

        // Cargar tu módulo por defecto al abrir
        CambiarVista(new Estadisticas());
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
}