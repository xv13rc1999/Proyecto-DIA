namespace Taller.Core {
    using System;
    public class DatosPrueba {
        /// <summary>
        /// Añade clientes, empleados y reparaciones de prueba.
        /// Hay reparaciones terminadas y pendientes, de 2025 y 2026.
        /// </summary>
        /// <param name="clientes">El registro de clientes a rellenar.</param>
        /// <param name="empleados">El registro de empleados a rellenar.</param>
        /// <param name="reparaciones">El registro de reparaciones a rellenar.</param>
        public static void Rellena(RegistroClientes clientes,
                                   RegistroEmpleados empleados,
                                   RegistroReparaciones reparaciones)
        {
            var c1 = new Cliente( "11111111A", "Ana Pérez López", "ana@correo.com" );
            var c2 = new Cliente( "22222222B", "Brais Otero Vila", "brais@correo.com" );
            var c3 = new Cliente( "33333333C", "Carmen Rial Souto", "carmen@uvigo.gal" );
            var c4 = new Cliente( "44444444D", "David Pérez Gil", "david@correo.com" );
            var c5 = new Cliente( "55555555E", "Elena Castro Mato", "elena@uvigo.gal" );

            var e1 = new Empleado( "90000001X", "Xoán Fernández", "xoan@taller.com" );
            var e2 = new Empleado( "90000002Y", "Lucía Martínez", "lucia@taller.com" );
            var e3 = new Empleado( "90000003Z", "Manuel Iglesias", "manuel@taller.com" );

            clientes.Add( c1 );
            clientes.Add( c2 );
            clientes.Add( c3 );
            clientes.Add( c4 );
            clientes.Add( c5 );

            empleados.Add( e1 );
            empleados.Add( e2 );
            empleados.Add( e3 );

            reparaciones.Add( new Reparacion( e1, c1, "Cambio de aceite",
                new DateTime( 2025, 3, 10 ), new DateTime( 2025, 3, 10 ), "Aceite 5W30" ) );
            reparaciones.Add( new Reparacion( e2, c2, "Frenos delanteros",
                new DateTime( 2025, 6, 2 ), new DateTime( 2025, 6, 4 ), "Pastillas y discos" ) );
            reparaciones.Add( new Reparacion( e1, c3, "Revisión ITV",
                new DateTime( 2025, 11, 20 ), new DateTime( 2025, 11, 21 ), "" ) );
            reparaciones.Add( new Reparacion( e3, c1, "Embrague",
                new DateTime( 2026, 1, 15 ), new DateTime( 2026, 1, 22 ), "Kit completo" ) );
            reparaciones.Add( new Reparacion( e2, c4, "Cambio de neumáticos",
                new DateTime( 2026, 5, 5 ), new DateTime( 2026, 5, 5 ), "4 neumáticos de invierno" ) );
            reparaciones.Add( new Reparacion( e3, c5, "Ruido en la dirección",
                new DateTime( 2026, 9, 28 ), null, "Pendiente de pieza" ) );
            reparaciones.Add( new Reparacion( e1, c2, "Batería",
                new DateTime( 2026, 10, 14 ), null, "No arranca en frío" ) );
            reparaciones.Add( new Reparacion( e2, c1, "Aire acondicionado",
                new DateTime( 2026, 10, 19 ), null, "Recarga de gas" ) );
        }
    }
}
