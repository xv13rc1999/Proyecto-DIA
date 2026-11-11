// Taller (c) 2026 DIA - Uvigo


namespace Taller.UI {
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    using System.Diagnostics;

    using Core.Busquedas;


    /// <summary>
    /// Ventana de búsqueda de reparaciones, con criterios combinables.
    /// Los resultados se actualizan en cuanto cambia algún criterio.
    /// </summary>
    public partial class BuscaReparacionesWindow: Window {
        /// <summary>
        /// Constructor sin parámetros: Avalonia lo exige para poder cargar el XAML
        /// (aviso AVLN3001). Usa un buscador vacío; la aplicación usa el otro constructor.
        /// </summary>
        public BuscaReparacionesWindow()
            : this( new Buscador( new Core.Cliente[ 0 ],
                                  new Core.Empleado[ 0 ],
                                  new Core.Reparacion[ 0 ] ) )
        {
        }

        /// <summary>Crea la ventana y muestra todas las reparaciones.</summary>
        /// <param name="buscador">El <see cref="Buscador"/> a utilizar.</param>
        public BuscaReparacionesWindow(Buscador buscador)
        {
            InitializeComponent();

            var edTexto = this.GetControl<TextBox>( "EdTexto" );
            var edCliente = this.GetControl<TextBox>( "EdCliente" );
            var edEmpleado = this.GetControl<TextBox>( "EdEmpleado" );
            var cbEstado = this.GetControl<ComboBox>( "CbEstado" );
            var dpDesde = this.GetControl<CalendarDatePicker>( "DpDesde" );
            var dpHasta = this.GetControl<CalendarDatePicker>( "DpHasta" );
            var btLimpiar = this.GetControl<Button>( "BtLimpiar" );

            Debug.Assert( edTexto != null, "edTexto not found in XAML" );
            Debug.Assert( edCliente != null, "edCliente not found in XAML" );
            Debug.Assert( edEmpleado != null, "edEmpleado not found in XAML" );
            Debug.Assert( cbEstado != null, "cbEstado not found in XAML" );
            Debug.Assert( dpDesde != null, "dpDesde not found in XAML" );
            Debug.Assert( dpHasta != null, "dpHasta not found in XAML" );
            Debug.Assert( btLimpiar != null, "btLimpiar not found in XAML" );

            // Búsqueda en vivo: se repite cada vez que cambia algún criterio
            edTexto.KeyUp += (_, _) => this.OnBuscar();
            edCliente.KeyUp += (_, _) => this.OnBuscar();
            edEmpleado.KeyUp += (_, _) => this.OnBuscar();
            cbEstado.SelectionChanged += (_, _) => this.OnBuscar();
            dpDesde.SelectedDateChanged += (_, _) => this.OnBuscar();
            dpHasta.SelectedDateChanged += (_, _) => this.OnBuscar();
            btLimpiar.Click += (_, _) => this.OnLimpiar();

            this.buscador = buscador;
            this.OnBuscar();        // Al abrir, muestra todas las reparaciones
        }

        /// <summary>Carga el XAML y centra la ventana sobre la principal.</summary>
        void InitializeComponent()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            AvaloniaXamlLoader.Load( this );
        }

        /// <summary>Crea el filtro con los criterios actuales y muestra los resultados.</summary>
        void OnBuscar()
        {
            var edTexto = this.GetControl<TextBox>( "EdTexto" );
            var edCliente = this.GetControl<TextBox>( "EdCliente" );
            var edEmpleado = this.GetControl<TextBox>( "EdEmpleado" );
            var cbEstado = this.GetControl<ComboBox>( "CbEstado" );
            var dpDesde = this.GetControl<CalendarDatePicker>( "DpDesde" );
            var dpHasta = this.GetControl<CalendarDatePicker>( "DpHasta" );
            var dtResultados = this.GetControl<DataGrid>( "DtResultados" );
            var lblResultados = this.GetControl<Label>( "LblResultados" );

            Debug.Assert( edTexto != null, "edTexto not found in XAML" );
            Debug.Assert( edCliente != null, "edCliente not found in XAML" );
            Debug.Assert( edEmpleado != null, "edEmpleado not found in XAML" );
            Debug.Assert( cbEstado != null, "cbEstado not found in XAML" );
            Debug.Assert( dpDesde != null, "dpDesde not found in XAML" );
            Debug.Assert( dpHasta != null, "dpHasta not found in XAML" );
            Debug.Assert( dtResultados != null, "dtResultados not found in XAML" );
            Debug.Assert( lblResultados != null, "lblResultados not found in XAML" );

            // Los eventos pueden dispararse antes de terminar de construir la ventana
            if ( this.buscador == null ) {
                return;
            }

            var filtro = new FiltroReparaciones();

            filtro.Texto = edTexto.Text ?? "";
            filtro.Cliente = edCliente.Text ?? "";
            filtro.Empleado = edEmpleado.Text ?? "";
            filtro.Desde = dpDesde.SelectedDate;
            filtro.Hasta = dpHasta.SelectedDate;

            switch ( cbEstado.SelectedIndex ) {
                case 1: filtro.Estado = EstadoReparacion.Pendientes;
                    break;
                case 2: filtro.Estado = EstadoReparacion.Terminadas;
                    break;
                default: filtro.Estado = EstadoReparacion.Todas;
                    break;
            }

            var resultados = this.buscador.BuscaReparaciones( filtro );

            dtResultados.ItemsSource = resultados;
            lblResultados.Content = $"{resultados.Count} reparación(es) encontrada(s)";
        }

        /// <summary>Vacía todos los criterios y vuelve a mostrar todo.</summary>
        void OnLimpiar()
        {
            var edTexto = this.GetControl<TextBox>( "EdTexto" );
            var edCliente = this.GetControl<TextBox>( "EdCliente" );
            var edEmpleado = this.GetControl<TextBox>( "EdEmpleado" );
            var cbEstado = this.GetControl<ComboBox>( "CbEstado" );
            var dpDesde = this.GetControl<CalendarDatePicker>( "DpDesde" );
            var dpHasta = this.GetControl<CalendarDatePicker>( "DpHasta" );

            Debug.Assert( edTexto != null, "edTexto not found in XAML" );
            Debug.Assert( edCliente != null, "edCliente not found in XAML" );
            Debug.Assert( edEmpleado != null, "edEmpleado not found in XAML" );
            Debug.Assert( cbEstado != null, "cbEstado not found in XAML" );
            Debug.Assert( dpDesde != null, "dpDesde not found in XAML" );
            Debug.Assert( dpHasta != null, "dpHasta not found in XAML" );

            edTexto.Text = "";
            edCliente.Text = "";
            edEmpleado.Text = "";
            cbEstado.SelectedIndex = 0;
            dpDesde.SelectedDate = null;
            dpHasta.SelectedDate = null;
            this.OnBuscar();
        }

        /// <summary>El buscador; null solo mientras se construye la ventana.</summary>
        Buscador? buscador;
    }
}
