using Avalonia.Controls;
using Avalonia.Markup.Xaml;
namespace TALLER
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Botón para colapsar/expandir el menú
            var btnToggle = this.GetControl<Button>("BtnToggle");
            var menuLateral = this.GetControl<SplitView>("MenuLateral");
            btnToggle.Click += (_, _) => menuLateral.IsPaneOpen = !menuLateral.IsPaneOpen;

            // Enlaces de navegación
            this.GetControl<Button>("BtnClientes").Click += (_, _) => CambiarVista(new VistaClientes());
            this.GetControl<Button>("BtnCoches").Click += (_, _) => CambiarVista(new VistaCoches());
            this.GetControl<Button>("BtnPersonal").Click += (_, _) => CambiarVista(new VistaPersonal());
            this.GetControl<Button>("BtnReparaciones").Click += (_, _) => CambiarVista(new VistaReparaciones());
            this.GetControl<Button>("BtnBusquedas").Click += (_, _) => CambiarVista(new VistaBusquedas());
            this.GetControl<Button>("BtnGraficos").Click += (_, _) => CambiarVista(new Estadisticas());

            // Vista por defecto al abrir la aplicación
            CambiarVista(new VistaReparaciones());
        }

        // Método para inyectar el UserControl en el área principal
        private void CambiarVista(UserControl nuevaVista)
        {
            var areaContenido = this.GetControl<ContentControl>("AreaContenido");
            areaContenido.Content = nuevaVista;
        }
         private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}