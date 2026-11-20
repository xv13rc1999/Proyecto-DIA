using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TallerApp;

public partial class VistaCoches : UserControl
{
    public VistaCoches()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}