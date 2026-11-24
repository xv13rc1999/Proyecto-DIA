using System.Text.RegularExpressions;
namespace Taller.Core.Coches;

//Coche del taller, matrícula única. 
//Las propiedades se cambian a través de GestorCoches

public partial class Coche
{
    public string Matricula { get; internal set; }
    public string Marca { get; internal set; }
    public string Modelo { get; internal set; }

    public Coche(string matricula, string marca, string modelo)
    {
        Matricula = NormalizaMatricula(matricula);
        Marca = CompruebaTexto(marca, nameof(marca));
        Modelo = CompruebaTexto(modelo, nameof(modelo));
    }

    //Comprueba que no esté vacía,
    //Quita espacios, guiones y lo pasa a mayúsculas
    
    public static string NormalizaMatricula(string matricula)
    {
        if (string.IsNullOrWhiteSpace(matricula))
            throw new ArgumentException("La matrícula no puede estar vacía.", nameof(matricula));

        string normalizada = matricula.Replace(" ", "").Replace("-", "").ToUpperInvariant();

        if (!EsMatriculaValida(normalizada))
            throw new ArgumentException($"La matrícula '{matricula}' no tiene un formato válido.", nameof(matricula));

        return normalizada;
    }

    
    public static bool EsMatriculaValida(string matriculaNormalizada) =>
        MatriculaActual().IsMatch(matriculaNormalizada) || MatriculaProvincial().IsMatch(matriculaNormalizada);

    internal static string CompruebaTexto(string valor, string campo)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException($"El campo '{campo}' no puede estar vacío.", campo);

        return valor.Trim();
    }

    public override string ToString() => $"{Matricula} - {Marca} {Modelo}";

    //Formato nuevo de matrícula : 4 dígitos + 3 consonantes, sin Ñ ni Q.
    [GeneratedRegex("^[0-9]{4}[BCDFGHJKLMNPRSTVWXYZ]{3}$")]
    private static partial Regex MatriculaActual();

    //Formato viejo de matrícula (antes de 2000): 1-2 letras + 4 dígitos + 1-2 letras.
    [GeneratedRegex("^[A-Z]{1,2}[0-9]{4}[A-Z]{1,2}$")]
    private static partial Regex MatriculaProvincial();
}
