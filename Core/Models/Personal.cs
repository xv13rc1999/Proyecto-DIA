namespace Proyecto.Core.Models;

public class Personal
{
    public string Dni { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int TicketsAsignados { get; set; }

    public Personal() { }

    public Personal(string dni, string nombre, string email, int ticketsAsignados = 0)
    {
        Dni = dni;
        Nombre = nombre;
        Email = email;
        TicketsAsignados = ticketsAsignados;
    }

    public override string ToString() => $"{Nombre} ({Dni})";
}