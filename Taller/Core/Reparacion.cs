namespace Taller.Core {
    using System;
    public class Reparacion {
        /// <summary>Crea una reparación a partir de sus datos.</summary>
        /// <param name="empleado">El <see cref="Empleado"/> encargado.</param>
        /// <param name="cliente">El <see cref="Cliente"/> que la encarga.</param>
        /// <param name="asunto">El asunto de la reparación.</param>
        /// <param name="fechaInicio">La fecha en la que comienza.</param>
        /// <param name="fechaFin">La fecha en la que termina, o null si está pendiente.</param>
        /// <param name="notas">Notas libres.</param>
        public Reparacion(Empleado empleado, Cliente cliente, string asunto,
                          DateTime fechaInicio, DateTime? fechaFin, string notas)
        {
            this.Empleado = empleado;
            this.Cliente = cliente;
            this.Asunto = asunto;
            this.FechaInicio = fechaInicio;
            this.FechaFin = fechaFin;
            this.Notas = notas;
        }

        /// <summary>Obtiene o modifica el trabajador encargado.</summary>
        public Empleado Empleado { get; set; }

        /// <summary>Obtiene o modifica el cliente que encarga la reparación.</summary>
        public Cliente Cliente { get; set; }

        /// <summary>Obtiene o modifica el asunto, como cadena.</summary>
        public string Asunto { get; set; }

        /// <summary>Obtiene o modifica la fecha de comienzo.</summary>
        public DateTime FechaInicio { get; set; }

        /// <summary>Obtiene o modifica la fecha de fin; null si sigue pendiente.</summary>
        public DateTime? FechaFin { get; set; }

        /// <summary>Obtiene o modifica las notas, como cadena.</summary>
        public string Notas { get; set; }

        /// <summary>Obtiene si la reparación está terminada (tiene fecha de fin).</summary>
        public bool EstaTerminada => this.FechaFin != null;

        /// <summary>Devuelve la reparación como cadena.</summary>
        /// <returns>El asunto, el cliente y el empleado.</returns>
        public override string ToString()
        {
            return $"{this.Asunto} - {this.Cliente} - {this.Empleado}";
        }
    }
}
