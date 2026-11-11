namespace Taller.Core {
    using System.Collections.ObjectModel;
    /// <summary>
    /// Colección de objetos <see cref="Cliente"/>.
    /// Deriva de ObservableCollection para poder ser
    /// utilizada como fuente de datos en un DataGrid.
    /// </summary>
    public class RegistroClientes: ObservableCollection<Cliente> {
    }
}
