// Taller (c) 2026 DIA - Uvigo


namespace Taller.UI {
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    using System.Diagnostics;

    using Core.Busquedas;


    /// <summary>
    /// Ventana de búsqueda de personal.
    /// Los resultados se actualizan en cuanto cambia algún criterio.
    /// </summary>
    public partial class BuscaPersonalWindow: Window {
        /// <summary>
        /// Constructor sin parámetros: Avalonia lo exige para poder cargar el XAML
        /// (aviso AVLN3001). Usa un buscador vacío; la aplicación usa el otro constructor.
        /// </summary>
        public BuscaPersonalWindow()
            : this( new Buscador( new Core.Cliente[ 0 ],
                                  new Core.Empleado[ 0 ],
                                  new Core.Reparacion[ 0 ] ) )
        {
        }

        /// <summary>Crea la ventana y muestra todos los resultados.</summary>
        /// <param name="buscador">El <see cref="Buscador"/> a utilizar.</param>
        public BuscaPersonalWindow(Buscador buscador)
        {
            InitializeComponent();

            var edTexto = this.GetControl<TextBox>( "EdTexto" );
            var cbCampo = this.GetControl<ComboBox>( "CbCampo" );
            var btLimpiar = this.GetControl<Button>( "BtLimpiar" );

            Debug.Assert( edTexto != null, "edTexto not found in XAML" );
            Debug.Assert( cbCampo != null, "cbCampo not found in XAML" );
            Debug.Assert( btLimpiar != null, "btLimpiar not found in XAML" );

            // Búsqueda en vivo: se repite cada vez que cambia algún criterio
            edTexto.KeyUp += (_, _) => this.OnBuscar();
            cbCampo.SelectionChanged += (_, _) => this.OnBuscar();
            btLimpiar.Click += (_, _) => this.OnLimpiar();

            this.buscador = buscador;
            this.OnBuscar();        // Al abrir, muestra todo el personal
        }

        /// <summary>Carga el XAML y centra la ventana sobre la principal.</summary>
        void InitializeComponent()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            AvaloniaXamlLoader.Load( this );
        }

        /// <summary>Busca según los criterios actuales y muestra los resultados.</summary>
        void OnBuscar()
        {
            var edTexto = this.GetControl<TextBox>( "EdTexto" );
            var cbCampo = this.GetControl<ComboBox>( "CbCampo" );
            var dtResultados = this.GetControl<DataGrid>( "DtResultados" );
            var lblResultados = this.GetControl<Label>( "LblResultados" );

            Debug.Assert( edTexto != null, "edTexto not found in XAML" );
            Debug.Assert( cbCampo != null, "cbCampo not found in XAML" );
            Debug.Assert( dtResultados != null, "dtResultados not found in XAML" );
            Debug.Assert( lblResultados != null, "lblResultados not found in XAML" );

            // SelectionChanged puede dispararse antes de terminar de construir la ventana
            if ( this.buscador == null ) {
                return;
            }

            CampoPersona campo;

            switch ( cbCampo.SelectedIndex ) {
                case 1: campo = CampoPersona.Dni;
                    break;
                case 2: campo = CampoPersona.Nombre;
                    break;
                case 3: campo = CampoPersona.Email;
                    break;
                default: campo = CampoPersona.Todos;
                    break;
            }

            var resultados = this.buscador.BuscaEmpleados( edTexto.Text ?? "", campo );

            dtResultados.ItemsSource = resultados;
            lblResultados.Content = $"{resultados.Count} empleado(s) encontrado(s)";
        }

        /// <summary>Vacía los criterios y vuelve a mostrar todo.</summary>
        void OnLimpiar()
        {
            var edTexto = this.GetControl<TextBox>( "EdTexto" );
            var cbCampo = this.GetControl<ComboBox>( "CbCampo" );

            Debug.Assert( edTexto != null, "edTexto not found in XAML" );
            Debug.Assert( cbCampo != null, "cbCampo not found in XAML" );

            edTexto.Text = "";
            cbCampo.SelectedIndex = 0;
            this.OnBuscar();
        }

        /// <summary>El buscador; null solo mientras se construye la ventana.</summary>
        Buscador? buscador;
    }
}
