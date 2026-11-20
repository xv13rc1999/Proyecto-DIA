using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TallerApp;

public partial class VistaPersonal : UserControl
{
    public VistaPersonal()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}