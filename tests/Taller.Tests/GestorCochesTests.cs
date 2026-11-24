using Taller.Core.Coches;

namespace Taller.Tests;

public class GestorCochesTests
{
    private static GestorCoches GestorConDatos()
    {
        var g = new GestorCoches();
        g.Inserta("1234BCD", "Seat", "Ibiza");
        g.Inserta("5678FGH", "Renault", "Clio");
        g.Inserta("M4321AB", "Seat", "León");
        return g;
    }

    [Theory]
    [InlineData("1234 BCD", "1234BCD")]
    [InlineData("1234-bcd", "1234BCD")]
    [InlineData("M-4321-AB", "M4321AB")]
    [InlineData("gr 1234 b", "GR1234B")]
    public void NormalizaMatricula_AceptaFormatosValidos(string entrada, string esperada) =>
        Assert.Equal(esperada, Coche.NormalizaMatricula(entrada));

    [Theory]
    [InlineData("")]
    [InlineData("1234AEI")] // vocales no permitidas en formato actual
    [InlineData("123BCD")]
    [InlineData("ABCDEFG")]
    public void NormalizaMatricula_RechazaFormatosInvalidos(string entrada) =>
        Assert.Throws<ArgumentException>(() => Coche.NormalizaMatricula(entrada));

    [Fact]
    public void Inserta_AnadeCoche()
    {
        var g = new GestorCoches();
        g.Inserta("1234 BCD", "Seat", "Ibiza");

        Assert.Single(g.Coches);
        Assert.Equal("1234BCD", g.Coches[0].Matricula);
    }

    [Fact]
    public void Inserta_MatriculaRepetida_Lanza()
    {
        var g = GestorConDatos();
        Assert.Throws<InvalidOperationException>(() => g.Inserta("1234-bcd", "Ford", "Focus"));
        Assert.Equal(3, g.Count);
    }

    [Theory]
    [InlineData("", "Ibiza")]
    [InlineData("Seat", "  ")]
    public void Inserta_CamposVacios_Lanza(string marca, string modelo) =>
        Assert.Throws<ArgumentException>(() => new GestorCoches().Inserta("1234BCD", marca, modelo));

    [Fact]
    public void Elimina_Existente_DevuelveTrue()
    {
        var g = GestorConDatos();
        Assert.True(g.Elimina("1234 BCD"));
        Assert.False(g.Contiene("1234BCD"));
        Assert.Equal(2, g.Count);
    }

    [Fact]
    public void Elimina_Inexistente_DevuelveFalse() =>
        Assert.False(GestorConDatos().Elimina("9999ZZZ"));

    [Fact]
    public void Modifica_CambiaSoloLoIndicado()
    {
        var g = GestorConDatos();
        g.Modifica("5678FGH", nuevoModelo: "Megane");

        var c = g.Busca("5678FGH")!;
        Assert.Equal("Renault", c.Marca);
        Assert.Equal("Megane", c.Modelo);
    }

    [Fact]
    public void Modifica_Matricula_ActualizaClave()
    {
        var g = GestorConDatos();
        g.Modifica("5678FGH", nuevaMatricula: "9999 ZZZ");

        Assert.False(g.Contiene("5678FGH"));
        Assert.Equal("Clio", g.Busca("9999ZZZ")!.Modelo);
    }

    [Fact]
    public void Modifica_AMatriculaExistente_LanzaYNoCambiaNada()
    {
        var g = GestorConDatos();
        Assert.Throws<InvalidOperationException>(() =>
            g.Modifica("5678FGH", nuevaMatricula: "1234BCD", nuevaMarca: "Ford"));

        Assert.Equal("Renault", g.Busca("5678FGH")!.Marca);
    }

    [Fact]
    public void Modifica_Inexistente_Lanza() =>
        Assert.Throws<KeyNotFoundException>(() => GestorConDatos().Modifica("9999ZZZ", nuevaMarca: "Ford"));

    [Fact]
    public void BuscaPorMarca_IgnoraMayusculas() =>
        Assert.Equal(2, GestorConDatos().BuscaPorMarca("SEAT").Count());

    [Theory]
    [InlineData("cl", 1)]      // modelo Clio
    [InlineData("seat", 2)]    // marca
    [InlineData("1234 b", 1)]  // matrícula con espacio
    [InlineData("", 3)]        // vacío = todos
    public void Filtra_BuscaEnTodosLosCampos(string texto, int esperados) =>
        Assert.Equal(esperados, GestorConDatos().Filtra(texto).Count());

    [Fact]
    public void Cambiado_SeLanzaEnAltasBajasYModificaciones()
    {
        var g = new GestorCoches();
        int veces = 0;
        g.Cambiado += (_, _) => veces++;

        g.Inserta("1234BCD", "Seat", "Ibiza");
        g.Modifica("1234BCD", nuevaMarca: "Cupra");
        g.Elimina("1234BCD");

        Assert.Equal(3, veces);
    }

    [Fact]
    public void GuardaYRecupera_ConservaLosDatos()
    {
        string fichero = Path.GetTempFileName();
        try
        {
            var original = GestorConDatos();
            original.Guarda(fichero);

            var cargado = GestorCoches.Carga(fichero);

            Assert.Equal(original.Count, cargado.Count);
            foreach (var c in original.Coches)
            {
                var r = cargado.Busca(c.Matricula)!;
                Assert.Equal(c.Marca, r.Marca);
                Assert.Equal(c.Modelo, r.Modelo);
            }
        }
        finally
        {
            File.Delete(fichero);
        }
    }

    [Fact]
    public void Recupera_FicheroInexistente_DejaGestorVacio()
    {
        var g = GestorConDatos();
        g.Recupera(Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xml"));
        Assert.Equal(0, g.Count);
    }
}
