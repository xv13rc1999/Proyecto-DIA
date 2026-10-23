namespace Proyecto.View;

using System.Linq;
using Proyecto.Core.Models;
using Proyecto.Core.Models.Enum;
using Proyecto.Core.Services;

public class ReparacionesCtrl
{
    private readonly GestorReparaciones _gestor;
    public ReparacionesView View { get; }

    public ReparacionesCtrl()
    {
        _gestor = new GestorReparaciones();
        View = new ReparacionesView();

        // Cargar las opciones del ComboBox dinámicamente con "Todas" + los valores del Enum
        CargarFiltroEstado();

        View.BtnAlta.Click += (_, _) => OnAlta();
        View.BtnModificar.Click += (_, _) => OnModificar();
        View.BtnBaja.Click += (_, _) => OnBaja();
        View.CbFiltroEstado.SelectionChanged += (_, _) => ActualizarLista();
        View.Closing += (_, _) => _gestor.Guardar();

        ActualizarLista();
    }

    private void CargarFiltroEstado()
    {
        var opciones = new List<object> { "Todas" };
        opciones.AddRange(Enum.GetValues<EstadoReparacion>().Cast<object>());
        
        View.CbFiltroEstado.ItemsSource = opciones;
        View.CbFiltroEstado.SelectedIndex = 0;
    }

    private void ActualizarLista()
    {
        View.LbReparaciones.ItemsSource = null;

        var seleccion = View.CbFiltroEstado.SelectedItem;

        // Si la opción seleccionada es un valor del enum, filtramos por él dinámicamente
        if (seleccion is EstadoReparacion estado)
        {
            View.LbReparaciones.ItemsSource = _gestor.ObtenerPorEstado(estado);
        }
        else
        {
            // Si es "Todas", muestra la lista completa
            View.LbReparaciones.ItemsSource = _gestor.Todas;
        }
    }

    private void OnAlta()
    {
        var dlg = new FormReparacionCtrl(View);
        dlg.Show((nuevaReparacion) =>
        {
            if (nuevaReparacion != null)
            {
                _gestor.Alta(nuevaReparacion);
                _gestor.Guardar();
                ActualizarLista();
            }
        });
    }

    private void OnModificar()
    {
        if (View.LbReparaciones.SelectedItem is Reparacion seleccionada)
        {
            var dlg = new FormReparacionCtrl(View, seleccionada);
            dlg.Show((reparacionEditada) =>
            {
                if (reparacionEditada != null)
                {
                    _gestor.Modificacion(reparacionEditada);
                    _gestor.Guardar();
                    ActualizarLista();
                }
            });
        }
    }

    private void OnBaja()
    {
        if (View.LbReparaciones.SelectedItem is Reparacion seleccionada)
        {
            _gestor.Baja(seleccionada.Id);
            _gestor.Guardar();
            ActualizarLista();
        }
    }
}