namespace Proyecto.Core.Models;

using System;
using Proyecto.Core.Models.Clases_Apoyo;

public enum EstadoReparacion
{
    Pendiente,
    Terminada
}

public class Reparacion
{
    public int Id { get; set; }
    public Personal TrabajadorEncargado { get; set; } = new();
    public Cliente Cliente { get; set; } = new();
    public string Asunto { get; set; } = string.Empty;
    public DateTime FechaInicial { get; set; } = DateTime.Now;
    public DateTime? FechaFinal { get; set; }
    public string Notas { get; set; } = string.Empty;
    public EstadoReparacion Estado { get; set; } = EstadoReparacion.Pendiente;

    public Reparacion() { }

    public Reparacion(int id, Personal trabajador, Cliente cliente, string asunto, DateTime fechaInicial, DateTime? fechaFinal, string notas, EstadoReparacion estado)
    {
        Id = id;
        TrabajadorEncargado = trabajador;
        Cliente = cliente;
        Asunto = asunto;
        FechaInicial = fechaInicial;
        FechaFinal = fechaFinal;
        Notas = notas;
        Estado = estado;
    }

    public override string ToString()
    {
        string fechaFinStr = FechaFinal.HasValue ? FechaFinal.Value.ToString("dd/MM/yyyy") : "En proceso";
        return $"[#{Id}] [{Estado}] {Asunto} | Cliente: {Cliente?.Nombre} | Mecánico: {TrabajadorEncargado?.Nombre} | Inicio: {FechaInicial:dd/MM/yyyy} | Fin: {fechaFinStr}";
    }
}