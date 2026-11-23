namespace Taller.Core {
    using System.Collections.ObjectModel;
    /// <summary>
    /// Colección de objetos <see cref="Reparacion"/>.
    /// Deriva de ObservableCollection para poder ser
    /// utilizada como fuente de datos en un DataGrid.
    /// </summary>
    public class RegistroReparaciones: ObservableCollection<Reparacion> {
    }
}
