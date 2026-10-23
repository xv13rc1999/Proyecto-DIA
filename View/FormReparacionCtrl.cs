namespace Proyecto.View;

using System;
using System.Collections.Generic;
using Proyecto.Core.Models;
using Proyecto.Core.Models.Clases_Apoyo;
using Proyecto.Core.Models.Enum;
using Avalonia.Controls;

public class FormReparacionCtrl
{
    private readonly Reparacion? _original;
    private Action<Reparacion?>? _callback;

    public FormReparacion View { get; }

    public FormReparacionCtrl(Window parentOwner, Reparacion? reparacion = null)
    {
        _original = reparacion;
        View = new FormReparacion();

        // Datos de prueba para simular los módulos de Clientes y Personal del proyecto
        var listaClientes = new List<Cliente>
        {
            new("12345678A", "Carlos Sainz", "carlos@gmail.com"),
            new("87654321B", "Fernando Alonso", "fernando@gmail.com")
        };

        var listaPersonal = new List<Personal>
        {
            new("11111111X", "Paco Mecánico", "paco@taller.com", 2),
            new("22222222Y", "Luis Técnico", "luis@taller.com", 1)
        };

        View.CbCliente.ItemsSource = listaClientes;
        View.CbTrabajador.ItemsSource = listaPersonal;

        if (_original != null)
        {
            View.TxtAsunto.Text = _original.Asunto;
            View.CbEstado.SelectedIndex = (int)_original.Estado;
            View.DpFechaInicial.SelectedDate = _original.FechaInicial;
            View.DpFechaFinal.SelectedDate = _original.FechaFinal;
            View.TxtNotas.Text = _original.Notas;
        }
        else
        {
            View.DpFechaInicial.SelectedDate = DateTime.Now;
            View.CbEstado.SelectedIndex = 0;
        }

        View.BtnGuardar.Click += (_, _) =>
        {
            var resultado = new Reparacion
            {
                Id = _original?.Id ?? 0,
                Asunto = View.TxtAsunto.Text ?? "",
                Cliente = (Cliente)View.CbCliente.SelectedItem!,
                TrabajadorEncargado = (Personal)View.CbTrabajador.SelectedItem!,
                Estado = (EstadoReparacion)View.CbEstado.SelectedIndex,
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
}