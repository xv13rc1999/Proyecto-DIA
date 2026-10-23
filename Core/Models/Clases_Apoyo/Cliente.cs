namespace Proyecto.Core.Models.Clases_Apoyo;

public class Cliente
{
    public string Dni { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public Cliente() { }

    public Cliente(string dni, string nombre, string email)
    {
        Dni = dni;
        Nombre = nombre;
        Email = email;
    }

    public override string ToString() => $"{Nombre} ({Dni})";
}