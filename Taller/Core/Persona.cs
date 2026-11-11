namespace Taller.Core {
    public abstract class Persona {
        /// <summary>
        /// Crea una persona a partir de sus datos.
        /// </summary>
        /// <param name="dni">El DNI.</param>
        /// <param name="nombre">El nombre completo.</param>
        /// <param name="email">El correo electrónico.</param>
        protected Persona(string dni, string nombre, string email)
        {
            this.Dni = dni;
            this.Nombre = nombre;
            this.Email = email;
        }

        /// <summary>Obtiene o modifica el DNI, como cadena.</summary>
        public string Dni { get; set; }

        /// <summary>Obtiene o modifica el nombre completo, como cadena.</summary>
        public string Nombre { get; set; }

        /// <summary>Obtiene o modifica el correo electrónico, como cadena.</summary>
        public string Email { get; set; }

        /// <summary>Devuelve la persona como cadena.</summary>
        /// <returns>El nombre seguido del DNI entre paréntesis.</returns>
        public override string ToString()
        {
            return $"{this.Nombre} ({this.Dni})";
        }
    }
}
