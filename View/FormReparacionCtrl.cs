namespace Proyecto.View;

using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Proyecto.Core.Models;
using Proyecto.Core.Models.Enum;
using Proyecto.Core.Models.Clases_Apoyo;

public class FormReparacionCtrl
{
    private readonly Reparacion? _original;
    private Action<Reparacion?>? _callback;

    public FormReparacion View { get; }

    public FormReparacionCtrl(Window parentOwner, Reparacion? reparacion = null)
    {
        _original = reparacion;
        View = new FormReparacion();

        // 1. Cargar directamente todos los valores del Enum EstadoReparacion
        View.CbEstado.ItemsSource = Enum.GetValues<EstadoReparacion>();

        // (Carga de datos auxiliares para clientes y mecánico)
        View.CbCliente.ItemsSource = obtenerClientes();
        View.CbTrabajador.ItemsSource = obtenerPersonal();

        // 2. Si es edición, seleccionar el estado actual del objeto
        if (_original != null)
        {
            View.TxtAsunto.Text = _original.Asunto;
            View.CbEstado.SelectedItem = _original.Estado; // Asignación directa del Enum
            View.DpFechaInicial.SelectedDate = _original.FechaInicial;
            View.DpFechaFinal.SelectedDate = _original.FechaFinal;
            View.TxtNotas.Text = _original.Notas;
        }
        else
        {
            View.DpFechaInicial.SelectedDate = DateTime.Now;
            View.CbEstado.SelectedItem = EstadoReparacion.Pendiente; // Valor por defecto
        }

        View.BtnGuardar.Click += (_, _) =>
        {
            var resultado = new Reparacion
            {
                Id = _original?.Id ?? 0,
                Asunto = View.TxtAsunto.Text ?? "",
                Cliente = (Cliente)View.CbCliente.SelectedItem!,
                TrabajadorEncargado = (Personal)View.CbTrabajador.SelectedItem!,
                
                // 3. Obtener el valor seleccionado casateando el SelectedItem al Enum
                Estado = (EstadoReparacion)View.CbEstado.SelectedItem!,
                
                FechaInicial = View.DpFechaInicial.SelectedDate ?? DateTime.Now,
                FechaFinal = View.DpFechaFinal.SelectedDate,
                Notas = View.TxtNotas.Text ?? ""
            };

            _callback?.Invoke(resultado);
            View.Close();
        };

        View.BtnCancelar.Click += (_, _) =>
        {
            _callback?.Invoke(null);
            View.Close();
        };
    }

    public void Show(Action<Reparacion?> callback)
    {
        _callback = callback;
        View.Show();
    }

    // =======================================================
    // MÉTODOS AUXILIARES (Datos simulados para pruebas)
    // =======================================================

    private List<Cliente> obtenerClientes()
    {
        return new List<Cliente>
        {
            new Cliente("12345678A", "Carlos Sainz", "carlos@gmail.com"),
            new Cliente("87654321B", "Fernando Alonso", "fernando@gmail.com")
        };
    }

    private List<Personal> obtenerPersonal()
    {
        return new List<Personal>
        {
            new Personal("11111111X", "Paco Mecánico", "paco@taller.com", 2),
            new Personal("22222222Y", "Luis Técnico", "luis@taller.com", 1)
        };
    }
}