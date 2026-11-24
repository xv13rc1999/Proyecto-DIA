using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Taller.Core.Coches;

namespace Taller.UI.ViewModels;


/// Lógica de la pantalla de coches: lista filtrable, formulario de
/// alta/modificación, baja, y salvaguarda/recuperación en XML.
/// No sabe nada de Avalonia: la vista (CochesView.axaml) solo se enlaza a sus propiedades y comandos.

public partial class CochesViewModel : ViewModelBase
{
    private readonly GestorCoches gestor;

    ///Ruta del fichero XML donde se guardan los coches.
    public string Fichero { get; }

    /// Coches que se ven en la lista (todos, o los que cumplen la búsqueda).
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Resumen))]
    public partial IReadOnlyList<Coche> CochesVisibles { get; set; } = [];

    /// Coche marcado en la lista. Si es null, el formulario sirve para dar de alta.
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TituloFormulario))]
    [NotifyCanExecuteChangedFor(nameof(EliminaCommand))]
    public partial Coche? CocheSeleccionado { get; set; }

    [ObservableProperty]
    public partial string TextoBusqueda { get; set; } = "";

    // Campos del formulario
    [ObservableProperty]
    public partial string Matricula { get; set; } = "";

    [ObservableProperty]
    public partial string Marca { get; set; } = "";

    [ObservableProperty]
    public partial string Modelo { get; set; } = "";

    // Mensaje para el usuario (confirmaciones o errores)
    [ObservableProperty]
    public partial string Mensaje { get; set; } = "";

    [ObservableProperty]
    public partial bool EsError { get; set; }

    public string TituloFormulario => CocheSeleccionado is null ? "Nuevo coche" : "Modificar coche";

    public string Resumen => $"{CochesVisibles.Count} de {gestor.Count} coches";

    public CochesViewModel(GestorCoches gestor, string fichero)
    {
        this.gestor = gestor;
        Fichero = fichero;

        // Cada alta, baja, modificación o recuperación refresca la lista.
        gestor.Cambiado += (_, _) => Refresca();
        Refresca();
    }

    ///Solo para la vista previa del diseñador de Avalonia.
    public CochesViewModel() : this(DatosDeEjemplo(), "coches.xml")
    {
    }

    // ---------- Reacciones a cambios de propiedades ----------
    // El toolkit llama a estos métodos parciales cuando cambia la propiedad correspondiente.

    partial void OnTextoBusquedaChanged(string value) => Refresca();

    partial void OnCocheSeleccionadoChanged(Coche? value) => RellenaFormulario(value);

    // ---------- Comandos (botones) ----------

    ///Deja el formulario vacío para dar de alta un coche nuevo.
    [RelayCommand]
    private void Nuevo()
    {
        CocheSeleccionado = null;
        Matricula = Marca = Modelo = "";
        Mensaje = "";
    }

    ///Alta si no hay coche seleccionado; modificación si lo hay.
    [RelayCommand]
    private void Guarda()
    {
        try
        {
            if (CocheSeleccionado is null)
            {
                var nuevo = gestor.Inserta(Matricula, Marca, Modelo);
                Selecciona(nuevo);
                MuestraInfo($"Coche {nuevo.Matricula} dado de alta.");
            }
            else
            {
                var coche = CocheSeleccionado;
                gestor.Modifica(coche.Matricula, Matricula, Marca, Modelo);
                RellenaFormulario(coche); // muestra los datos ya normalizados
                MuestraInfo($"Coche {coche.Matricula} modificado.");
            }
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or KeyNotFoundException)
        {
            MuestraError(ex.Message);
        }
    }

    [RelayCommand(CanExecute = nameof(HayCocheSeleccionado))]
    private void Elimina()
    {
        if (CocheSeleccionado is null)
            return;

        string matricula = CocheSeleccionado.Matricula;
        gestor.Elimina(matricula);
        Nuevo();
        MuestraInfo($"Coche {matricula} eliminado.");
    }

    private bool HayCocheSeleccionado() => CocheSeleccionado is not null;

    ///Salvaguarda: escribe todos los coches en el fichero XML.
    [RelayCommand]
    public void GuardaFichero()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(Fichero))!);
            gestor.Guarda(Fichero);
            MuestraInfo($"{gestor.Count} coches guardados.");
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            MuestraError($"No se pudo guardar: {ex.Message}");
        }
    }

    ///Recuperación: carga los coches del fichero XML, descartando los de memoria.
    [RelayCommand]
    public void RecuperaFichero()
    {
        try
        {
            gestor.Recupera(Fichero);
            Nuevo();
            MuestraInfo($"{gestor.Count} coches recuperados.");
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException
                                      or XmlException or InvalidDataException or ArgumentException)
        {
            MuestraError($"No se pudo recuperar: {ex.Message}");
        }
    }

    // ---------- Auxiliares ----------

    /// Vuelve a calcular la lista visible según la búsqueda, conservando
    /// la selección si el coche sigue estando en la lista.
    private void Refresca()
    {
        var seleccionado = CocheSeleccionado;

        // Se crea una lista nueva (en vez de modificar la anterior) para que
        // la vista redibuje todas las filas, también las de coches modificados.
        CochesVisibles = gestor.Filtra(TextoBusqueda).ToList();

        CocheSeleccionado = seleccionado is not null && CochesVisibles.Contains(seleccionado)
            ? seleccionado
            : null;
    }

    private void RellenaFormulario(Coche? coche)
    {
        Matricula = coche?.Matricula ?? "";
        Marca = coche?.Marca ?? "";
        Modelo = coche?.Modelo ?? "";
    }

    //Selecciona el coche si es visible con la búsqueda actual.
    private void Selecciona(Coche coche)
    {
        if (CochesVisibles.Contains(coche))
            CocheSeleccionado = coche;
    }

    private void MuestraInfo(string texto)
    {
        Mensaje = texto;
        EsError = false;
    }

    private void MuestraError(string texto)
    {
        Mensaje = texto;
        EsError = true;
    }

    private static GestorCoches DatosDeEjemplo()
    {
        var g = new GestorCoches();
        g.Inserta("1234BCD", "Seat", "Ibiza");

        return g;
    }
}
