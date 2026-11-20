using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TallerApp;

public partial class VistaReparaciones : UserControl
{
    public VistaReparaciones()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}