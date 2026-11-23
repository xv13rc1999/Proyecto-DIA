namespace Taller.Core {
    public class Cliente: Persona {
        /// <summary>Crea un cliente a partir de sus datos.</summary>
        /// <param name="dni">El DNI.</param>
        /// <param name="nombre">El nombre completo.</param>
        /// <param name="email">El correo electrónico.</param>
        public Cliente(string dni, string nombre, string email)
            : base( dni, nombre, email )
        {
        }
    }
}
