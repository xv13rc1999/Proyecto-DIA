using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TallerApp;

public partial class VistaBusquedas : UserControl
{
    public VistaBusquedas()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}