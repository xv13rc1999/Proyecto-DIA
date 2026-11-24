using System.Xml.Linq;
namespace Taller.Core.Coches;


public class GestorCoches
{
    private const string EtqCoches = "coches";
    private const string EtqCoche = "coche";
    private const string EtqMatricula = "matricula";
    private const string EtqMarca = "marca";
    private const string EtqModelo = "modelo";


    private readonly List<Coche> coches = new();
    public event EventHandler? Cambiado;

    //Deja la lista en modo solo Lectura, devuelve la misma lista con sólo
    //métodos para recorrerla, quien la reciba no puede usar ni add ni Remove
    // por lo que no puede saltarse las validaciones

    public IReadOnlyList<Coche> Coches => coches;
    public int Count => coches.Count;

    //El primer inserta recibe los datos y llama al segundo y luego devuelve el coche
    public Coche Inserta(string matricula, string marca, string modelo)
    {
        var coche = new Coche(matricula, marca, modelo);
        Inserta(coche);
        return coche;
    }

    //Comprueba si es nulo y si ya existe otro coche con la misma matrícula
    //lo añade a la lista de coches
    public void Inserta(Coche coche)
    {
        ArgumentNullException.ThrowIfNull(coche);

        if (Contiene(coche.Matricula))
            throw new InvalidOperationException($"Ya existe un coche con matrícula {coche.Matricula}.");

        coches.Add(coche);
        OnCambiado();
    }

    //Devuelve falase si el coche no existe, true si lo borra
    public bool Elimina(string matricula)
    {
        var coche = Busca(matricula);
        if (coche is null)
            return false;

        coches.Remove(coche);
        OnCambiado();
        return true;
    }

    /* Modifica el coche con la matrícula como parámetro, el resto de atributos a null para poder modificar
    el que queremos. 
    
    */
    public void Modifica(string matricula, string? nuevaMatricula = null, string? nuevaMarca = null, string? nuevoModelo = null)
    {
        var coche = Busca(matricula)
            ?? throw new KeyNotFoundException($"No existe ningún coche con matrícula {matricula}.");

        // Validamos todo antes de tocar nada, para no dejar el coche a medias.
        string mat = nuevaMatricula is null ? coche.Matricula : Coche.NormalizaMatricula(nuevaMatricula);
        string marca = nuevaMarca is null ? coche.Marca : Coche.CompruebaTexto(nuevaMarca, nameof(nuevaMarca));
        string modelo = nuevoModelo is null ? coche.Modelo : Coche.CompruebaTexto(nuevoModelo, nameof(nuevoModelo));

        if (mat != coche.Matricula && Contiene(mat))
            throw new InvalidOperationException($"Ya existe un coche con matrícula {mat}.");

        coche.Matricula = mat;
        coche.Marca = marca;
        coche.Modelo = modelo;
        OnCambiado();
    }

    //Vacía las listas y avisa
    public void Clear()
    {
        coches.Clear();
        OnCambiado();
    }

    // ---------- Búsquedas ----------

    //Devuelve el coche si existe,
    //Devuelve null si no existe o la matrícula no es válida
    //Normaliza la matrícula y recorre la lista
    public Coche? Busca(string matricula)
    {
        if (string.IsNullOrWhiteSpace(matricula))
            return null;

        string mat = matricula.Replace(" ", "").Replace("-", "").ToUpperInvariant();
        return coches.Find(c => c.Matricula == mat);
    }


    public bool Contiene(string matricula) => Busca(matricula) is not null;

    //Devuelve todos los coches de una marca. Where filtra una colección por condición
    //StringComparison.OrdinalIgnoreCase compara sin distinguir mayúsculas y minúsculas
    public IEnumerable<Coche> BuscaPorMarca(string marca) =>
        coches.Where(c => c.Marca.Equals(marca.Trim(), StringComparison.OrdinalIgnoreCase));

    //Si el texto está vacío devuelve todos los coches
    //Devuelve los coches que coincidan en alguno de los campos 
    public IEnumerable<Coche> Filtra(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return coches;

        string t = texto.Trim();
        string tMat = t.Replace(" ", "").Replace("-", "");
        return coches.Where(c =>
            c.Matricula.Contains(tMat, StringComparison.OrdinalIgnoreCase)
            || c.Marca.Contains(t, StringComparison.OrdinalIgnoreCase)
            || c.Modelo.Contains(t, StringComparison.OrdinalIgnoreCase));
    }

    // ---------- Salvaguarda y recuperación ----------

    public void Guarda(string fichero)
    {
        var doc = new XDocument(
            new XElement(EtqCoches,
                coches.Select(c => new XElement(EtqCoche,
                    new XAttribute(EtqMatricula, c.Matricula),
                    new XElement(EtqMarca, c.Marca),
                    new XElement(EtqModelo, c.Modelo)))));

        doc.Save(fichero);
    }

    /*  
        Recarga los datos en un gestor que ya existe
        Lee el fichero XML y reconstruye los coches
        Si el fichero no existe, el gestor queda vacío
    */
    public void Recupera(string fichero)
    {
        var nuevos = new List<Coche>();

        if (File.Exists(fichero))
        {
            var raiz = XElement.Load(fichero);

            foreach (var e in raiz.Elements(EtqCoche))
            {
                var coche = new Coche(
                    (string?)e.Attribute(EtqMatricula) ?? "",
                    (string?)e.Element(EtqMarca) ?? "",
                    (string?)e.Element(EtqModelo) ?? "");

                if (nuevos.Any(c => c.Matricula == coche.Matricula))
                    throw new InvalidDataException($"Matrícula repetida en {fichero}: {coche.Matricula}.");

                nuevos.Add(coche);
            }
        }

        coches.Clear();
        coches.AddRange(nuevos);
        OnCambiado();
    }
    
    //Crea un gestor nuevo y le carga los datos de una vez
    public static GestorCoches Carga(string fichero)
    {
        var gestor = new GestorCoches();
        gestor.Recupera(fichero);
        return gestor;
    }

    private void OnCambiado() => Cambiado?.Invoke(this, EventArgs.Empty);
}
