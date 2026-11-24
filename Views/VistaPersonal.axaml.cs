using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Taller.Core;
using Taller.Core.Busquedas;
using Taller.Views.Forms;
using Taller.Utils.DTOs;

namespace Taller.Views;

public partial class VistaPersonal : UserControl
{
    ContenedorDatos _datosTaller;
    Buscador _buscador;
    
    public VistaPersonal(ContenedorDatos datos)
    {
        InitializeComponent();

        this.GetControl<TextBox>( "EdTexto" ).KeyUp += (_, _) => this.OnBuscar();
        this.GetControl<ComboBox>( "CbCampo" ).SelectionChanged += (_, _) => this.OnBuscar();
        this.GetControl<Button>( "BtLimpiar" ).Click += (_, _) => this.OnLimpiar();

        this.GetControl<Button>("BtAnadir").Click += async (_,_) => await this.OnAñadirAsync();
        this.GetControl<Button>("BtModificar").Click += async (_,_) => await this.OnModificarAsync();
        this.GetControl<Button>("BtEliminar").Click += (_,_) => this.OnEliminar();

        this._datosTaller = datos;
        _buscador = new Buscador(_datosTaller.ListaClientes, _datosTaller.ListaPersonal, _datosTaller.ListaReparaciones);
        this.OnBuscar();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public void OnBuscar()
    {
        var edTexto = this.GetControl<TextBox>( "EdTexto" );
        var cbCampo = this.GetControl<ComboBox>( "CbCampo" );
        var dtResultados = this.GetControl<DataGrid>( "DtResultados" );
        var lblResultados = this.GetControl<Label>( "LblResultados" );

        // SelectionChanged puede dispararse antes de terminar de construir la ventana
        if ( this._buscador == null ) {
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

        var resultados = this._buscador.BuscaEmpleados( edTexto.Text ?? "", campo );

        dtResultados.ItemsSource = resultados;
        lblResultados.Content = $"{resultados.Count} empleado(s) encontrado(s)";
    }

    public void OnLimpiar()
    {
        var edTexto = this.GetControl<TextBox>( "EdTexto" );
        var cbCampo = this.GetControl<ComboBox>( "CbCampo" );

        edTexto.Text = "";
        cbCampo.SelectedIndex = 0;
        this.OnBuscar();
    }
    
    public async Task OnAñadirAsync()
    {
        await MostrarFormPersonalAsync();
    }

    public async Task OnModificarAsync()
    {
        object? personalSeleccionado = this.GetControl<DataGrid>( "DtResultados" ).SelectedItem;
        if (personalSeleccionado is not null)
        {
            await MostrarFormPersonalAsync((Personal) personalSeleccionado);
        }
    }

    public async Task MostrarFormPersonalAsync(Personal? p = null)
    {
        FormularioPersonal formularioDialogo = new(_datosTaller, p);
        
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop && desktop.MainWindow != null)
        {
            var resultadoDialogo = await formularioDialogo.ShowDialog<bool>(desktop.MainWindow);
            
            PersonalAsignadoDTO? nuevoPersonal = formularioDialogo.Resultado;
            if (resultadoDialogo && nuevoPersonal is not null)
            {
                if (p is null) //Añadir
                {
                    _datosTaller.AñadirPersonal(nuevoPersonal.Dni, nuevoPersonal.Nombre,
                                    nuevoPersonal.Email, nuevoPersonal.ReparacionesAsignadas);
                }
                else //Modificar
                {
                    _datosTaller.ActualizarPersonal(nuevoPersonal.Dni, nuevoPersonal.Nombre,
                                    nuevoPersonal.Email, nuevoPersonal.ReparacionesAsignadas);
                }
                
            }

            this.OnBuscar();
        }
    }

    public void OnEliminar()
    {
        object? personalSeleccionado = this.GetControl<DataGrid>( "DtResultados" ).SelectedItem;
        if (personalSeleccionado is not null)
        {
            Personal eliminable = (Personal) personalSeleccionado;
            _datosTaller.EliminarPersonal(eliminable.Dni);
        }

        this.OnBuscar();
    }
}