namespace TallerClientes.Core;


using System.Xml.Linq;


public class Cliente
{
    public string Dni { get; }
    public string Nombre { get; }
    public string Email { get; }

    public Cliente(string dni, string nom, string ema)
    {
        this.Dni = dni;
        this.Nombre = nom;
        this.Email = ema;
    }

    public XElement ToXml()
    {
        return new XElement("Cliente",
            new XElement("Dni", this.Dni),
            new XElement("Nombre", this.Nombre),
            new XElement("Email", this.Email));
    }

    public override string ToString()
    {
        return $"{this.Dni}\t\t{this.Nombre}\t\t{this.Email}";
    }
}
