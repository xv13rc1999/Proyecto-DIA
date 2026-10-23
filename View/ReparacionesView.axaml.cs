namespace Proyecto.View;

using Avalonia.Controls;

public partial class ReparacionesView : Window
{
    public ReparacionesView()
    {
        InitializeComponent();
        CbFiltroEstado = this.GetControl<ComboBox>("cbFiltroEstado");
        LbReparaciones = this.GetControl<ListBox>("lbReparaciones");
        BtnAlta = this.GetControl<Button>("btnAlta");
        BtnModificar = this.GetControl<Button>("btnModificar");
        BtnBaja = this.GetControl<Button>("btnBaja");
    }

    public ComboBox CbFiltroEstado { get; }
    public ListBox LbReparaciones { get; }
    public Button BtnAlta { get; }
    public Button BtnModificar { get; }
    public Button BtnBaja { get; }
}