namespace Taller.View;


using Avalonia.Controls;


public partial class VistaClientes : UserControl
{
    public ListBox LbClientes { get; }
    public Button BtNuevo { get; }
    public Button BtEditar { get; }
    public Button BtEliminar { get; }
    public Button BtGuardar { get; }
    
    public VistaClientes()
    {
        InitializeComponent();
        
        this.BtNuevo = this.GetControl<Button>( "btNuevo" );
        this.BtEditar = this.GetControl<Button>( "btEditar" );
        this.BtEliminar = this.GetControl<Button>( "btEliminar" );
        this.BtGuardar = this.GetControl<Button>( "btGuardar" );
        this.LbClientes = this.GetControl<ListBox>( "lbClientes" );
    }
}
