namespace Proyecto.View;

using System.Linq;
using Proyecto.Core.Models;
using Proyecto.Core.Services;

public class ReparacionesCtrl
{
    private readonly GestorReparaciones _gestor;

    public ReparacionesView View { get; }

    public ReparacionesCtrl()
    {
        _gestor = new GestorReparaciones();
        View = new ReparacionesView();

        View.BtnAlta.Click += (_, _) => OnAlta();
        View.BtnModificar.Click += (_, _) => OnModificar();
        View.BtnBaja.Click += (_, _) => OnBaja();
        View.CbFiltroEstado.SelectionChanged += (_, _) => ActualizarLista();
        View.Closing += (_, _) => _gestor.Guardar();

        ActualizarLista();
    }

    private void ActualizarLista()
    {
        int index = View.CbFiltroEstado.SelectedIndex;
        View.LbReparaciones.ItemsSource = null;

        if (index == 1) // Pendientes
            View.LbReparaciones.ItemsSource = _gestor.Pendientes;
        else if (index == 2) // Terminadas
            View.LbReparaciones.ItemsSource = _gestor.Terminadas;
        else // Todas
            View.LbReparaciones.ItemsSource = _gestor.Todas;
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