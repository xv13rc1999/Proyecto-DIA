using Taller.Core.Coches;
using Taller.UI.ViewModels;

namespace Taller.Tests;

public class CochesViewModelTests
{
    private static CochesViewModel ViewModelConDatos(string? fichero = null)
    {
        var g = new GestorCoches();
        g.Inserta("1234BCD", "Seat", "Ibiza");
        g.Inserta("5678FGH", "Renault", "Clio");
        return new CochesViewModel(g, fichero ?? Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xml"));
    }

    [Fact]
    public void Arranca_MuestraTodosLosCoches()
    {
        var vm = ViewModelConDatos();
        Assert.Equal(2, vm.CochesVisibles.Count);
        Assert.Equal("2 de 2 coches", vm.Resumen);
    }

    [Fact]
    public void Busqueda_FiltraLaLista()
    {
        var vm = ViewModelConDatos();
        vm.TextoBusqueda = "clio";
        Assert.Single(vm.CochesVisibles);
        Assert.Equal("1 de 2 coches", vm.Resumen);
    }

    [Fact]
    public void Seleccionar_RellenaFormulario()
    {
        var vm = ViewModelConDatos();
        vm.CocheSeleccionado = vm.CochesVisibles[1];

        Assert.Equal("5678FGH", vm.Matricula);
        Assert.Equal("Renault", vm.Marca);
        Assert.Equal("Modificar coche", vm.TituloFormulario);
    }

    [Fact]
    public void Guarda_SinSeleccion_DaDeAltaYSelecciona()
    {
        var vm = ViewModelConDatos();
        vm.Matricula = "9999 zzz";
        vm.Marca = "Ford";
        vm.Modelo = "Focus";

        vm.GuardaCommand.Execute(null);

        Assert.Equal(3, vm.CochesVisibles.Count);
        Assert.Equal("9999ZZZ", vm.CocheSeleccionado?.Matricula);
        Assert.Equal("9999ZZZ", vm.Matricula); // formulario con la matrícula normalizada
        Assert.False(vm.EsError);
    }

    [Fact]
    public void Guarda_DatosInvalidos_MuestraErrorYNoAnade()
    {
        var vm = ViewModelConDatos();
        vm.Matricula = "12AB";
        vm.Marca = "Ford";
        vm.Modelo = "Focus";

        vm.GuardaCommand.Execute(null);

        Assert.True(vm.EsError);
        Assert.Contains("formato", vm.Mensaje);
        Assert.Equal(2, vm.CochesVisibles.Count);
    }

    [Fact]
    public void Guarda_ConSeleccion_Modifica()
    {
        var vm = ViewModelConDatos();
        vm.CocheSeleccionado = vm.CochesVisibles[1];
        vm.Matricula = "1111-bbb";
        vm.Modelo = "Megane";

        vm.GuardaCommand.Execute(null);

        Assert.False(vm.EsError);
        Assert.Equal(2, vm.CochesVisibles.Count);
        Assert.Equal("1111BBB", vm.CocheSeleccionado?.Matricula);
        Assert.Equal("Megane", vm.CocheSeleccionado?.Modelo);
        Assert.Equal("1111BBB", vm.Matricula);
    }

    [Fact]
    public void Guarda_ModificaAMatriculaRepetida_MuestraError()
    {
        var vm = ViewModelConDatos();
        vm.CocheSeleccionado = vm.CochesVisibles[1];
        vm.Matricula = "1234BCD";

        vm.GuardaCommand.Execute(null);

        Assert.True(vm.EsError);
        Assert.Equal("5678FGH", vm.CocheSeleccionado?.Matricula);
    }

    [Fact]
    public void Elimina_SoloDisponibleConSeleccion()
    {
        var vm = ViewModelConDatos();
        Assert.False(vm.EliminaCommand.CanExecute(null));

        vm.CocheSeleccionado = vm.CochesVisibles[0];
        Assert.True(vm.EliminaCommand.CanExecute(null));
    }

    [Fact]
    public void Elimina_QuitaElCocheYLimpiaFormulario()
    {
        var vm = ViewModelConDatos();
        vm.CocheSeleccionado = vm.CochesVisibles[0];

        vm.EliminaCommand.Execute(null);

        Assert.Single(vm.CochesVisibles);
        Assert.Null(vm.CocheSeleccionado);
        Assert.Equal("", vm.Matricula);
    }

    [Fact]
    public void Nuevo_LimpiaSeleccionYFormulario()
    {
        var vm = ViewModelConDatos();
        vm.CocheSeleccionado = vm.CochesVisibles[0];

        vm.NuevoCommand.Execute(null);

        Assert.Null(vm.CocheSeleccionado);
        Assert.Equal("", vm.Marca);
        Assert.Equal("Nuevo coche", vm.TituloFormulario);
    }

    [Fact]
    public void GuardaYRecuperaFichero_ConservaLosDatos()
    {
        string dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        string fichero = Path.Combine(dir, "coches.xml"); // carpeta que aún no existe
        try
        {
            var vm = ViewModelConDatos(fichero);
            vm.GuardaFicheroCommand.Execute(null);
            Assert.False(vm.EsError);

            var otro = new CochesViewModel(new GestorCoches(), fichero);
            otro.RecuperaFicheroCommand.Execute(null);

            Assert.False(otro.EsError);
            Assert.Equal(2, otro.CochesVisibles.Count);
        }
        finally
        {
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
        }
    }

    [Fact]
    public void RecuperaFichero_Corrupto_MuestraErrorYConservaDatos()
    {
        string fichero = Path.GetTempFileName();
        try
        {
            File.WriteAllText(fichero, "esto no es xml");
            var vm = ViewModelConDatos(fichero);

            vm.RecuperaFicheroCommand.Execute(null);

            Assert.True(vm.EsError);
            Assert.Equal(2, vm.CochesVisibles.Count);
        }
        finally
        {
            File.Delete(fichero);
        }
    }
}
