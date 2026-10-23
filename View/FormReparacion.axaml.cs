namespace Proyecto.View;

using Avalonia.Controls;

public partial class FormReparacion : Window
{
    public FormReparacion()
    {
        InitializeComponent();
        TxtAsunto = this.GetControl<TextBox>("txtAsunto");
        CbCliente = this.GetControl<ComboBox>("cbCliente");
        CbTrabajador = this.GetControl<ComboBox>("cbTrabajador");
        CbEstado = this.GetControl<ComboBox>("cbEstado");
        DpFechaInicial = this.GetControl<CalendarDatePicker>("dpFechaInicial");
        DpFechaFinal = this.GetControl<CalendarDatePicker>("dpFechaFinal");
        TxtNotas = this.GetControl<TextBox>("txtNotas");
        BtnGuardar = this.GetControl<Button>("btnGuardar");
        BtnCancelar = this.GetControl<Button>("btnCancelar");
    }

    public TextBox TxtAsunto { get; }
    public ComboBox CbCliente { get; }
    public ComboBox CbTrabajador { get; }
    public ComboBox CbEstado { get; }
    public CalendarDatePicker DpFechaInicial { get; }
    public CalendarDatePicker DpFechaFinal { get; }
    public TextBox TxtNotas { get; }
    public Button BtnGuardar { get; }
    public Button BtnCancelar { get; }
}