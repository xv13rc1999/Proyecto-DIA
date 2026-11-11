namespace Taller.Core {
    public class Empleado: Persona {
        /// <summary>Crea un empleado a partir de sus datos.</summary>
        /// <param name="dni">El DNI.</param>
        /// <param name="nombre">El nombre completo.</param>
        /// <param name="email">El correo electrónico.</param>
        public Empleado(string dni, string nombre, string email)
            : base( dni, nombre, email )
        {
        }
    }
}
