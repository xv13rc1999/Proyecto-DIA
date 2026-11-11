namespace Taller.Core.Busquedas {
    using System.Collections.Generic;
    using System.Linq;
    public class Buscador {
        /// <summary>Crea un buscador sobre las colecciones dadas.</summary>
        /// <param name="clientes">Los <see cref="Cliente"/>s en los que buscar.</param>
        /// <param name="empleados">Los <see cref="Empleado"/>s en los que buscar.</param>
        /// <param name="reparaciones">Las <see cref="Reparacion"/>es en las que buscar.</param>
        public Buscador(IEnumerable<Cliente> clientes,
                        IEnumerable<Empleado> empleados,
                        IEnumerable<Reparacion> reparaciones)
        {
            this.clientes = clientes;
            this.empleados = empleados;
            this.reparaciones = reparaciones;
        }

        /// <summary>Busca clientes que contengan el texto en el campo dado.</summary>
        /// <param name="texto">Texto a buscar. Vacío: todos los clientes.</param>
        /// <param name="campo">El <see cref="CampoPersona"/> en el que buscar.</param>
        /// <returns>Los clientes encontrados, ordenados por nombre.</returns>
        public List<Cliente> BuscaClientes(string texto, CampoPersona campo)
        {
            return this.clientes
                        .Where( c => Coincide( c, texto, campo ) )
                        .OrderBy( c => c.Nombre )
                        .ToList();
        }

        /// <summary>Busca empleados que contengan el texto en el campo dado.</summary>
        /// <param name="texto">Texto a buscar. Vacío: todo el personal.</param>
        /// <param name="campo">El <see cref="CampoPersona"/> en el que buscar.</param>
        /// <returns>Los empleados encontrados, ordenados por nombre.</returns>
        public List<Empleado> BuscaEmpleados(string texto, CampoPersona campo)
        {
            return this.empleados
                        .Where( e => Coincide( e, texto, campo ) )
                        .OrderBy( e => e.Nombre )
                        .ToList();
        }

        /// <summary>Busca reparaciones que cumplan todos los criterios del filtro.</summary>
        /// <param name="filtro">Los criterios, como <see cref="FiltroReparaciones"/>.</param>
        /// <returns>Las reparaciones encontradas, de la más reciente a la más antigua.</returns>
        public List<Reparacion> BuscaReparaciones(FiltroReparaciones filtro)
        {
            IEnumerable<Reparacion> toret = this.reparaciones;

            toret = toret.Where( r => Contiene( r.Asunto, filtro.Texto )
                                   || Contiene( r.Notas, filtro.Texto ) );
            toret = toret.Where( r => Coincide( r.Cliente, filtro.Cliente, CampoPersona.Todos ) );
            toret = toret.Where( r => Coincide( r.Empleado, filtro.Empleado, CampoPersona.Todos ) );

            if ( filtro.Estado == EstadoReparacion.Pendientes ) {
                toret = toret.Where( r => !r.EstaTerminada );
            }
            else
            if ( filtro.Estado == EstadoReparacion.Terminadas ) {
                toret = toret.Where( r => r.EstaTerminada );
            }

            if ( filtro.Desde != null ) {
                toret = toret.Where( r => r.FechaInicio.Date >= filtro.Desde.Value.Date );
            }

            if ( filtro.Hasta != null ) {
                toret = toret.Where( r => r.FechaInicio.Date <= filtro.Hasta.Value.Date );
            }

            return toret.OrderByDescending( r => r.FechaInicio ).ToList();
        }

        /// <summary>
        /// Comprueba si una persona (cliente o empleado) contiene
        /// el texto en el campo indicado.
        /// Recibe una <see cref="Persona"/>, así que sirve para ambos (polimorfismo).
        /// </summary>
        /// <param name="p">La persona a comprobar.</param>
        /// <param name="texto">El texto a buscar.</param>
        /// <param name="campo">El campo en el que buscar.</param>
        /// <returns>true si coincide, false en otro caso.</returns>
        static bool Coincide(Persona p, string texto, CampoPersona campo)
        {
            bool toret;

            if ( campo == CampoPersona.Dni ) {
                toret = Contiene( p.Dni, texto );
            }
            else
            if ( campo == CampoPersona.Nombre ) {
                toret = Contiene( p.Nombre, texto );
            }
            else
            if ( campo == CampoPersona.Email ) {
                toret = Contiene( p.Email, texto );
            }
            else {
                toret = Contiene( p.Dni, texto )
                     || Contiene( p.Nombre, texto )
                     || Contiene( p.Email, texto );
            }

            return toret;
        }

        /// <summary>
        /// Comprueba si el dato contiene el texto, sin distinguir mayúsculas
        /// e ignorando los espacios al principio y al final del texto.
        /// </summary>
        /// <param name="dato">El dato en el que buscar.</param>
        /// <param name="texto">El texto a buscar. Vacío o null siempre coincide.</param>
        /// <returns>true si lo contiene, false en otro caso.</returns>
        static bool Contiene(string? dato, string? texto)
        {
            string buscado = ( texto ?? "" ).Trim().ToLower();

            return ( dato ?? "" ).ToLower().Contains( buscado );
        }

        IEnumerable<Cliente> clientes;

        IEnumerable<Empleado> empleados;

        IEnumerable<Reparacion> reparaciones;
    }
}
