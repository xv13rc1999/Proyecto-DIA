namespace Taller.Core.Busquedas {
    using System;

    public class FiltroReparaciones {
        /// <summary>Crea un filtro vacío, que no descarta ninguna reparación.</summary>
        public FiltroReparaciones()
        {
            this.Texto = "";
            this.Cliente = "";
            this.Empleado = "";
            this.Estado = EstadoReparacion.Todas;
            this.Desde = null;
            this.Hasta = null;
        }

        public string Texto { get; set; }

        public string Cliente { get; set; }

        public string Empleado { get; set; }

        public EstadoReparacion Estado { get; set; }

        public DateTime? Desde { get; set; }

        public DateTime? Hasta { get; set; }
    }
}
