using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TALLER
{
    public partial class VistaClientes : UserControl
    {
        public VistaClientes()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}