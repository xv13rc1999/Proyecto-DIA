// Taller (c) 2026 DIA - Uvigo


namespace Tests {
    using System;
    using Taller.Core;
    using Taller.Core.Busquedas;


    /// <summary>
    /// Pruebas de unidad del <see cref="Buscador"/>,
    /// usando los datos de <see cref="DatosPrueba"/>:
    /// 5 clientes, 3 empleados y 8 reparaciones (3 pendientes).
    /// </summary>
    public class TestBuscador {
        /// <summary>Crea registros nuevos con los datos de prueba antes de cada test.</summary>
        [SetUp]
        public void Setup()
        {
            this.clientes = new RegistroClientes();
            this.empleados = new RegistroEmpleados();
            this.reparaciones = new RegistroReparaciones();

            DatosPrueba.Rellena( this.clientes, this.empleados, this.reparaciones );
            this.buscador = new Buscador( this.clientes, this.empleados, this.reparaciones );
        }

        /// <summary>Un texto vacío devuelve todos los clientes.</summary>
        [Test]
        public void TestClientesTextoVacio()
        {
            var res = this.buscador!.BuscaClientes( "", CampoPersona.Todos );

            Assert.That( res.Count, Is.EqualTo( 5 ) );
        }

        /// <summary>Busca por nombre sin distinguir mayúsculas.</summary>
        [Test]
        public void TestClientesNombreSinMayusculas()
        {
            Assert.That( this.buscador!.BuscaClientes( "pérez", CampoPersona.Nombre ).Count,
                         Is.EqualTo( 2 ) );
            Assert.That( this.buscador!.BuscaClientes( "PÉREZ", CampoPersona.Nombre ).Count,
                         Is.EqualTo( 2 ) );
        }

        /// <summary>Solo busca en el campo indicado.</summary>
        [Test]
        public void TestClientesCampo()
        {
            Assert.That( this.buscador!.BuscaClientes( "uvigo", CampoPersona.Email ).Count,
                         Is.EqualTo( 2 ) );
            Assert.That( this.buscador!.BuscaClientes( "uvigo", CampoPersona.Dni ).Count,
                         Is.EqualTo( 0 ) );
        }

        /// <summary>Los espacios alrededor del texto se ignoran.</summary>
        [Test]
        public void TestClientesEspacios()
        {
            var res = this.buscador!.BuscaClientes( "  ana  ", CampoPersona.Nombre );

            Assert.That( res.Count, Is.EqualTo( 1 ) );
            Assert.That( res[ 0 ].Dni, Is.EqualTo( "11111111A" ) );
        }

        /// <summary>Los resultados salen ordenados por nombre.</summary>
        [Test]
        public void TestClientesOrdenados()
        {
            var res = this.buscador!.BuscaClientes( "", CampoPersona.Todos );

            Assert.That( res[ 0 ].Nombre, Is.EqualTo( "Ana Pérez López" ) );
            Assert.That( res[ res.Count - 1 ].Nombre, Is.EqualTo( "Elena Castro Mato" ) );
        }

        /// <summary>Un cliente dado de alta después aparece en la siguiente búsqueda.</summary>
        [Test]
        public void TestClientesAltaPosterior()
        {
            this.clientes!.Add( new Cliente( "66666666F", "Fran Nova", "fran@correo.com" ) );

            Assert.That( this.buscador!.BuscaClientes( "", CampoPersona.Todos ).Count,
                         Is.EqualTo( 6 ) );
        }

        /// <summary>Busca personal por DNI.</summary>
        [Test]
        public void TestEmpleadosDni()
        {
            Assert.That( this.buscador!.BuscaEmpleados( "", CampoPersona.Todos ).Count,
                         Is.EqualTo( 3 ) );
            Assert.That( this.buscador!.BuscaEmpleados( "0002", CampoPersona.Dni ).Count,
                         Is.EqualTo( 1 ) );
        }

        /// <summary>Un filtro vacío devuelve todas las reparaciones.</summary>
        [Test]
        public void TestReparacionesFiltroVacio()
        {
            var res = this.buscador!.BuscaReparaciones( new FiltroReparaciones() );

            Assert.That( res.Count, Is.EqualTo( 8 ) );
        }

        /// <summary>Filtra por estado: pendientes y terminadas.</summary>
        [Test]
        public void TestReparacionesEstado()
        {
            var pendientes = new FiltroReparaciones();
            var terminadas = new FiltroReparaciones();

            pendientes.Estado = EstadoReparacion.Pendientes;
            terminadas.Estado = EstadoReparacion.Terminadas;

            Assert.That( this.buscador!.BuscaReparaciones( pendientes ).Count, Is.EqualTo( 3 ) );
            Assert.That( this.buscador!.BuscaReparaciones( terminadas ).Count, Is.EqualTo( 5 ) );
        }

        /// <summary>Busca el texto tanto en el asunto como en las notas.</summary>
        [Test]
        public void TestReparacionesTexto()
        {
            var enAsunto = new FiltroReparaciones();
            var enNotas = new FiltroReparaciones();

            enAsunto.Texto = "cambio";
            enNotas.Texto = "pieza";

            Assert.That( this.buscador!.BuscaReparaciones( enAsunto ).Count, Is.EqualTo( 2 ) );
            Assert.That( this.buscador!.BuscaReparaciones( enNotas ).Count, Is.EqualTo( 1 ) );
        }

        /// <summary>Combina criterios: trabajador y estado a la vez.</summary>
        [Test]
        public void TestReparacionesCombinadas()
        {
            var filtro = new FiltroReparaciones();

            filtro.Cliente = "ana";
            Assert.That( this.buscador!.BuscaReparaciones( filtro ).Count, Is.EqualTo( 3 ) );

            filtro.Cliente = "";
            filtro.Empleado = "lucía";
            filtro.Estado = EstadoReparacion.Pendientes;
            Assert.That( this.buscador!.BuscaReparaciones( filtro ).Count, Is.EqualTo( 1 ) );
        }

        /// <summary>El rango de fechas incluye los dos extremos.</summary>
        [Test]
        public void TestReparacionesFechas()
        {
            var anho = new FiltroReparaciones();
            var dia = new FiltroReparaciones();

            anho.Desde = new DateTime( 2026, 1, 1 );
            anho.Hasta = new DateTime( 2026, 12, 31 );
            dia.Desde = new DateTime( 2025, 3, 10 );
            dia.Hasta = new DateTime( 2025, 3, 10 );

            Assert.That( this.buscador!.BuscaReparaciones( anho ).Count, Is.EqualTo( 5 ) );
            Assert.That( this.buscador!.BuscaReparaciones( dia ).Count, Is.EqualTo( 1 ) );
        }

        /// <summary>Las reparaciones salen de la más reciente a la más antigua.</summary>
        [Test]
        public void TestReparacionesOrdenadas()
        {
            var res = this.buscador!.BuscaReparaciones( new FiltroReparaciones() );

            Assert.That( res[ 0 ].FechaInicio, Is.EqualTo( new DateTime( 2026, 10, 19 ) ) );
            Assert.That( res[ res.Count - 1 ].FechaInicio, Is.EqualTo( new DateTime( 2025, 3, 10 ) ) );
        }

        /// <summary>El registro de clientes de cada test.</summary>
        RegistroClientes? clientes;

        /// <summary>El registro de empleados de cada test.</summary>
        RegistroEmpleados? empleados;

        /// <summary>El registro de reparaciones de cada test.</summary>
        RegistroReparaciones? reparaciones;

        /// <summary>El buscador a probar.</summary>
        Buscador? buscador;
    }
}
