namespace Taller.View;


using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;


public partial class NuevoCliente : Window
{
    public TextBox EdDni { get; }
    public TextBox EdNombre { get; }
    public TextBox EdEmail { get; }
    public Button BtOk { get; }
    public Button BtCancelar { get; }
    
    public NuevoCliente()
    {
        InitializeComponent();
        
        this.EdDni = this.GetControl<TextBox>( "edDni" );
        this.EdNombre = this.GetControl<TextBox>( "edNombre" );
        this.EdEmail = this.GetControl<TextBox>( "edEmail" );
        this.BtOk = this.GetControl<Button>( "btOk" );
        this.BtCancelar = this.GetControl<Button>( "btCancelar" );
    }
}
