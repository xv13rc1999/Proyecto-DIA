namespace Taller.UI.ViewModels;

/// ViewModel de la ventana principal. Cada pestaña tiene su propio ViewModel;
/// por ahora solo está hecha la de coches.

public class MainViewModel : ViewModelBase
{
    public CochesViewModel Coches { get; }

    public MainViewModel(CochesViewModel coches)
    {
        Coches = coches;
    }


    public MainViewModel() : this(new CochesViewModel())
    {
    }
}
