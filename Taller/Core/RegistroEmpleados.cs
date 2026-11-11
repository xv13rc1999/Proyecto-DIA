namespace Taller.Core {
    using System.Collections.ObjectModel;
    /// <summary>
    /// Colección de objetos <see cref="Empleado"/>.
    /// Deriva de ObservableCollection para poder ser
    /// utilizada como fuente de datos en un DataGrid.
    /// </summary>
    public class RegistroEmpleados: ObservableCollection<Empleado> {
    }
}
