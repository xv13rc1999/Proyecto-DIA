# Proyecto-DIA

1. Convertir las ventanas actuales en UserControl
Actualmente, cada compañero tiene un archivo .axaml que empieza por <Window>. Debéis cambiar todas esas pantallas secundarias (Clientes, Coches, Gráficos, etc.) para que sean controles incrustables.

En el archivo XAML (ej. VistaGraficos.axaml):
Cambia la etiqueta principal <Window ...> por <UserControl ...>. Elimina propiedades exclusivas de ventanas como Title.

En el archivo C# (ej. VistaGraficos.axaml.cs):
Cambia la herencia de la clase de Window a UserControl.

C#
public partial class VistaGraficos : UserControl // Antes era : Window
2. Diseñar el nuevo MainWindow.axaml (Contenedor Principal)
Cread un nuevo MainWindow que será el esqueleto de la aplicación. Para hacer un menú lateral que se despliegue y muestre el contenido a la derecha, la herramienta ideal en Avalonia es el SplitView.

XML
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        x:Class="TallerApp.MainWindow"
        Title="Gestión de Taller" Width="1024" Height="768">

    <SplitView IsPaneOpen="True" 
               DisplayMode="Inline" 
               OpenPaneLength="250">
        
        <!-- MENÚ LATERAL (PANE) -->
        <SplitView.Pane>
            <StackPanel Background="#2C3E50" Spacing="5" Padding="10">
                <TextBlock Text="MENÚ PRINCIPAL" Foreground="White" FontWeight="Bold" Margin="0,0,0,20"/>
                
                <Button Name="BtnClientes" Content="Gestión de Clientes" HorizontalAlignment="Stretch"/>
                <Button Name="BtnCoches" Content="Gestión de Coches" HorizontalAlignment="Stretch"/>
                <Button Name="BtnPersonal" Content="Gestión de Personal" HorizontalAlignment="Stretch"/>
                <Button Name="BtnReparaciones" Content="Gestión de Reparaciones" HorizontalAlignment="Stretch"/>
                <Button Name="BtnBusquedas" Content="Búsquedas" HorizontalAlignment="Stretch"/>
                <Button Name="BtnGraficos" Content="Gráficos y Estadísticas" HorizontalAlignment="Stretch"/>
            </StackPanel>
        </SplitView.Pane>

        <!-- ÁREA DE CONTENIDO DINÁMICO -->
        <SplitView.Content>
            <!-- Aquí se inyectará la pantalla que el usuario seleccione -->
            <ContentControl Name="AreaContenido" Margin="20"/>
        </SplitView.Content>
        
    </SplitView>
</Window>
3. Programar la navegación en MainWindow.axaml.cs
En el código de la ventana principal, debes enlazar los clics de los botones del menú lateral para que sustituyan el contenido del ContentControl por la vista correspondiente creada por cada compañero.

C#
using Avalonia.Controls;

namespace TallerApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Enlazar los botones del menú a la lógica de navegación
        this.GetControl<Button>("BtnClientes").Click += (_, _) => MostrarVista(new VistaClientes());
        this.GetControl<Button>("BtnCoches").Click += (_, _) => MostrarVista(new VistaCoches());
        this.GetControl<Button>("BtnPersonal").Click += (_, _) => MostrarVista(new VistaPersonal());
        this.GetControl<Button>("BtnReparaciones").Click += (_, _) => MostrarVista(new VistaReparaciones());
        this.GetControl<Button>("BtnBusquedas").Click += (_, _) => MostrarVista(new VistaBusquedas());
        this.GetControl<Button>("BtnGraficos").Click += (_, _) => MostrarVista(new VistaGraficos()); // Tu parte[cite: 1]

        // Cargar una vista por defecto al abrir la aplicación
        MostrarVista(new VistaGraficos());
    }

    // Método para inyectar el UserControl en el área principal
    private void MostrarVista(UserControl nuevaVista)
    {
        var areaContenido = this.GetControl<ContentControl>("AreaContenido");
        areaContenido.Content = nuevaVista;
    }
}
4. Flujo de trabajo en grupo
Proyecto unificado: Debéis crear un único proyecto .csproj.

Carpetas por módulo: Cread carpetas dentro del proyecto (ej. Views/Graficos, Views/Clientes) para que cada compañero meta sus archivos (.axaml y .cs) sin pisarse el código.

Modelos compartidos: Poneos de acuerdo en tener una única carpeta Models donde estén las clases genéricas que usaréis todos (ej. la clase Reparacion con DNI, FechaInicial, etc.)[cite: 1] para que las altas que haga un compañero puedan ser leídas por tus gráficos.