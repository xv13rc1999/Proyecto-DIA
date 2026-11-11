// Taller (c) 2026 DIA - Uvigo


namespace Taller.UI {
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    using System.Diagnostics;

    using Core;
    using Core.Busquedas;


    /// <summary>
    /// Ventana principal: da acceso a las tres búsquedas.
    /// Crea los registros y el <see cref="Buscador"/> que comparten.
    /// </summary>
    public partial class MainWindow: Window {
        /// <summary>Crea la ventana principal y carga los datos.</summary>
        public MainWindow()
        {
            InitializeComponent();

            var opSalir = this.GetControl<MenuItem>( "OpSalir" );
            var opBuscaClientes = this.GetControl<MenuItem>( "OpBuscaClientes" );
            var opBuscaPersonal = this.GetControl<MenuItem>( "OpBuscaPersonal" );
            var opBuscaReparaciones = this.GetControl<MenuItem>( "OpBuscaReparaciones" );
            var btBuscaClientes = this.GetControl<Button>( "BtBuscaClientes" );
            var btBuscaPersonal = this.GetControl<Button>( "BtBuscaPersonal" );
            var btBuscaReparaciones = this.GetControl<Button>( "BtBuscaReparaciones" );
            var lblResumen = this.GetControl<Label>( "LblResumen" );

            Debug.Assert( opSalir != null, "opSalir not found in XAML" );
            Debug.Assert( opBuscaClientes != null, "opBuscaClientes not found in XAML" );
            Debug.Assert( opBuscaPersonal != null, "opBuscaPersonal not found in XAML" );
            Debug.Assert( opBuscaReparaciones != null, "opBuscaReparaciones not found in XAML" );
            Debug.Assert( btBuscaClientes != null, "btBuscaClientes not found in XAML" );
            Debug.Assert( btBuscaPersonal != null, "btBuscaPersonal not found in XAML" );
            Debug.Assert( btBuscaReparaciones != null, "btBuscaReparaciones not found in XAML" );
            Debug.Assert( lblResumen != null, "lblResumen not found in XAML" );

            opSalir.Click += (_, _) => this.Close();
            opBuscaClientes.Click += (_, _) => this.OnBuscaClientes();
            btBuscaClientes.Click += (_, _) => this.OnBuscaClientes();
            opBuscaPersonal.Click += (_, _) => this.OnBuscaPersonal();
            btBuscaPersonal.Click += (_, _) => this.OnBuscaPersonal();
            opBuscaReparaciones.Click += (_, _) => this.OnBuscaReparaciones();
            btBuscaReparaciones.Click += (_, _) => this.OnBuscaReparaciones();

            // Datos de prueba (clases falsas); al integrar, aquí se recuperarán del XML
            this.Clientes = new RegistroClientes();
            this.Empleados = new RegistroEmpleados();
            this.Reparaciones = new RegistroReparaciones();
            DatosPrueba.Rellena( this.Clientes, this.Empleados, this.Reparaciones );

            this.Buscador = new Buscador( this.Clientes, this.Empleados, this.Reparaciones );

            lblResumen.Content = $"{this.Clientes.Count} clientes, "
                               + $"{this.Empleados.Count} empleados, "
                               + $"{this.Reparaciones.Count} reparaciones";
        }

        /// <summary>Carga el XAML y centra la ventana en la pantalla.</summary>
        void InitializeComponent()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            AvaloniaXamlLoader.Load( this );
        }

        /// <summary>Abre la ventana de búsqueda de clientes.</summary>
        void OnBuscaClientes()
        {
            new BuscaClientesWindow( this.Buscador ).ShowDialog( this );
        }

        /// <summary>Abre la ventana de búsqueda de personal.</summary>
        void OnBuscaPersonal()
        {
            new BuscaPersonalWindow( this.Buscador ).ShowDialog( this );
        }

        /// <summary>Abre la ventana de búsqueda de reparaciones.</summary>
        void OnBuscaReparaciones()
        {
            new BuscaReparacionesWindow( this.Buscador ).ShowDialog( this );
        }

        /// <summary>Obtiene el registro de clientes.</summary>
        public RegistroClientes Clientes { get; }

        /// <summary>Obtiene el registro de empleados.</summary>
        public RegistroEmpleados Empleados { get; }

        /// <summary>Obtiene el registro de reparaciones.</summary>
        public RegistroReparaciones Reparaciones { get; }

        /// <summary>Obtiene el buscador sobre los tres registros.</summary>
        public Buscador Buscador { get; }
    }
}
