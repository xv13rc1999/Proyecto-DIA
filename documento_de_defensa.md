# Guía Definitiva de Defensa: Panel de Estadísticas en Avalonia y OxyPlot

Esta guía explica detalladamente todo el código de tu aplicación de taller mecánico (tanto la lógica C# como la estructura visual XAML). Está pensada para que entiendas cada concepto, tipo de dato y librería desde cero, de modo que puedas defender el proyecto con total soltura ante tu profesor sin que quepa duda de que lo domina por completo.

---

## 1. El Núcleo de la Aplicación: `App.axaml` y `App.axaml.cs`

Antes de que aparezca cualquier ventana, el motor de .NET y Avalonia necesitan inicializar los estilos visuales globales y saber cuál es la ventana principal que debe abrirse.

### `App.axaml` (Estilos Globales)

```xml
<Application xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             x:Class="Taller.App"
             RequestedThemeVariant="Default">

    <Application.Styles>
        <FluentTheme />
        <StyleInclude Source="avares://Avalonia.Controls.DataGrid/Themes/Fluent.xaml"/>
        <StyleInclude Source="avares://OxyPlot.Avalonia/Themes/Default.axaml"/>
    </Application.Styles>
</Application>
```

* **¿Qué es?** Es el archivo de configuración global de la aplicación.
* **`RequestedThemeVariant="Default"`:** Indica que la app adoptará el tema claro u oscuro por defecto del sistema operativo del usuario.
* **`<Application.Styles>`:** Aquí se cargan hojas de estilo globales obligatorias:
  * `<FluentTheme />`: Proporciona la apariencia moderna estilo Windows 11 (botones redondeados, efectos visuales limpios).
  * `<StyleInclude Source="avares://OxyPlot.Avalonia/Themes/Default.axaml"/>`: **Vital.** Si olvidas esta línea, los gráficos de OxyPlot se calcularán en segundo plano, pero al intentar dibujarse en pantalla aparecerán completamente transparentes o invisibles porque Avalonia no conocería los pinceles por defecto de la librería de gráficos.

### `App.axaml.cs` (Lógica de Arranque)

```csharp
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace Taller;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
```

* **`partial class App : Application`:** Hereda de la clase base `Application` de Avalonia. La palabra `partial` significa que este archivo se complementa con la estructura visual de `App.axaml`.
* **`Initialize()`:** Llama a `AvaloniaXamlLoader.Load(this)` para compilar y cargar en memoria el código XML que escribiste en el archivo `.axaml`.
* **`OnFrameworkInitializationCompleted()`:** Es el evento que se dispara justo cuando el sistema operativo termina de arrancar el entorno gráfico. Comprueba si la aplicación se está ejecutando en un escritorio tradicional y asigna `desktop.MainWindow = new MainWindow();`, haciendo que aparezca tu interfaz al hacer `dotnet run`.

---

## 2. La Estructura Visual: `MainWindow.axaml`

El archivo `MainWindow.axaml` define la interfaz gráfica de usuario (GUI) utilizando **XAML** (un lenguaje basado en XML). Siguiendo los apuntes de tu profesor sobre Avalonia, las interfaces se organizan mediante contenedores llamados **Paneles** y **Controles básicos**.

A continuación tienes el código completo de tu diseño y su explicación línea por línea:

```xml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:oxy="clr-namespace:OxyPlot.Avalonia;assembly=OxyPlot.Avalonia"
        x:Class="Taller.MainWindow"
        Title="Panel de Control - Taller"
        Width="850" Height="720"
        Background="#F4F6F9"
        FontFamily="Segoe UI, Arial, sans-serif">

    <ScrollViewer>
        <StackPanel Margin="25">
            
            <!-- TÍTULO PRINCIPAL -->
            <Label Content="Estadísticas del Taller" 
                   FontSize="24" 
                   FontWeight="Bold" 
                   Foreground="#1E293B" 
                   Margin="0,0,0,20"/>

            <!-- TARJETA 1: ESTADÍSTICAS GENERALES -->
            <Border Background="White" CornerRadius="10" Padding="20" Margin="0,0,0,25" BoxShadow="0 4 12 0 #15000000">
                <StackPanel>
                    <Label Content="1. Reparaciones Generales" 
                           FontSize="16" 
                           FontWeight="Bold" 
                           Foreground="#334155" 
                           Margin="0,0,0,12"/>
                    
                    <StackPanel Orientation="Horizontal" Margin="0,0,0,15">
                        <Button Name="BtCalcular" 
                                Content="📅 Mensuales" 
                                Background="#3B82F6" 
                                Foreground="White" 
                                FontWeight="SemiBold"
                                Padding="15,8" 
                                CornerRadius="6" 
                                Margin="0,0,10,0" />
                                
                        <Button Name="BtCalcularAnual" 
                                Content="📊 Anuales" 
                                Background="#2563EB" 
                                Foreground="White" 
                                FontWeight="SemiBold"
                                Padding="15,8" 
                                CornerRadius="6" />
                    </StackPanel>
                    
                    <oxy:PlotView Name="PlotEstadisticas" Height="230" Background="White"/>
                </StackPanel>
            </Border>
            
            <!-- TARJETA 2: ESTADÍSTICAS POR CLIENTE -->
            <Border Background="White" CornerRadius="10" Padding="20" BoxShadow="0 4 12 0 #15000000">
                <StackPanel>
                    <Label Content="2. Análisis por Cliente" 
                           FontSize="16" 
                           FontWeight="Bold" 
                           Foreground="#334155" 
                           Margin="0,0,0,12"/>
                    
                    <StackPanel Orientation="Horizontal" Margin="0,0,0,15">
                        <Button Name="BtCalcularClientes" 
                                Content="🏆 Total Histórico" 
                                Background="#10B981" 
                                Foreground="White" 
                                FontWeight="SemiBold"
                                Padding="15,8" 
                                CornerRadius="6" 
                                Margin="0,0,10,0" />
                                
                        <Button Name="BtCalcularClienteMensual" 
                                Content="📈 Desglose Mensual" 
                                Background="#059669" 
                                Foreground="White" 
                                FontWeight="SemiBold"
                                Padding="15,8" 
                                CornerRadius="6" 
                                Margin="0,0,10,0" />
                                
                        <Button Name="BtCalcularClienteAnual" 
                                Content="🗂️ Desglose Anual" 
                                Background="#047857" 
                                Foreground="White" 
                                FontWeight="SemiBold"
                                Padding="15,8" 
                                CornerRadius="6" />
                    </StackPanel>
                    
                    <!-- Gráfico Histórico General -->
                    <oxy:PlotView Name="PlotEstadisticasClientes" Height="230" Background="White"/>

                    <!-- Lista de Mini-Gráficos -->
                    <ScrollViewer Height="260">
                        <ItemsControl Name="ListaGraficosClientes">
                            <ItemsControl.ItemTemplate>
                                <DataTemplate x:CompileBindings="False">
                                    <Border Background="#F8FAFC" BorderBrush="#E2E8F0" BorderThickness="1" CornerRadius="8" Padding="12" Margin="0,0,10,15">
                                        <StackPanel>
                                            <TextBlock Text="{Binding Titulo}" 
                                                       FontSize="14" 
                                                       FontWeight="Bold" 
                                                       Foreground="#0F172A" 
                                                       Margin="5,0,0,8"/>
                                            <oxy:PlotView Model="{Binding Modelo}" Height="140" Background="Transparent"/>
                                        </StackPanel>
                                    </Border>
                                </DataTemplate>
                            </ItemsControl.ItemTemplate>
                        </ItemsControl>
                    </ScrollViewer>
                </StackPanel>
            </Border>

        </StackPanel>
    </ScrollViewer>
</Window>
```

### Explicación detallada de los elementos visuales de Avalonia:

* **`<Window ...>`:** Representa la ventana principal de la aplicación de escritorio.
  * `xmlns:oxy="clr-namespace:OxyPlot.Avalonia;assembly=OxyPlot.Avalonia"`: Es un espacio de nombres personalizado. Le enseña a Avalonia que cualquier etiqueta que empiece por `oxy:` pertenece a la librería externa de gráficos OxyPlot.
  * `Width="850" Height="720"`: Fija las dimensiones iniciales de la ventana.
  * `Background="#F4F6F9"`: Establece un color de fondo gris claro para toda la ventana, dando un aspecto de panel moderno.

* **`<ScrollViewer>`:** Es un control contenedor que añade barras de desplazamiento automático vertical y horizontal si el contenido de la ventana es más largo que la pantalla del usuario.

* **`<StackPanel Margin="25">`:** Como vimos en los apuntes de clase, es un panel que organiza los controles de manera secuencial. Al no especificar `Orientation`, por defecto los apila **en vertical** (de arriba a abajo). El `Margin="25"` deja un espacio de separación alrededor de todo el contenido interior.

* **`<Label>`:** Control básico de Avalonia empleado para mostrar texto estático no editable (como los títulos de las secciones). Usamos propiedades como `FontSize="24"`, `FontWeight="Bold"` y `Foreground` (color del texto en formato hexadecimal).

* **`<Border>`:** Es un contenedor decorativo muy potente que nos permite simular el efecto de "tarjetas" flotantes. 
  * `CornerRadius="10"`: Redondea las esquinas de la tarjeta.
  * `Padding="20"`: Deja espacio interior para que los controles no se peguen a los bordes de la tarjeta.
  * `BoxShadow`: Añade una sombra sutil alrededor de la tarjeta para darle profundidad visual.

* **`<Button>`:** Control básico interactivo para capturar la acción del usuario.
  * `Name="..."`: **Crucial para la defensa.** El atributo `Name` permite referenciar este botón directamente desde el archivo de código C# (`MainWindow.axaml.cs`) para programar su evento `Click`.
  * `Background` y `Foreground`: Personalizan los colores del botón (por ejemplo, azul o verde esmeralda según la categoría).
  * `CornerRadius="6"`: Redondea los bordes del botón.

* **`<oxy:PlotView>`:** Es el control visual que aporta la librería OxyPlot para incrustar un lienzo gráfico dentro de Avalonia. 
  * `Name="PlotEstadisticas"`: Permite inyectarle un modelo matemático (`PlotModel`) desde C# cuando el usuario pulsa un botón.

* **`<ItemsControl>` y `<DataTemplate>` (Dinámica de Mini-Gráficos):**
  * **`ItemsControl`:** Es un control avanzado que sirve para renderizar listas dinámicas de elementos. No dibuja una sola cosa, sino que repite una estructura visual por cada objeto que le inyectemos en su propiedad `ItemsSource` desde C#.
  * **`DataTemplate`:** Define la "plantilla" o molde visual que se repetirá en cada elemento de la lista.
  * `x:CompileBindings="False"`: Desactiva el chequeo estricto de tipos en tiempo de compilación para permitir que la plantilla lea dinámicamente las propiedades `Titulo` y `Modelo` de la clase auxiliar `MiniGraficoCliente` en tiempo de ejecución sin dar errores de compilación.

---

## 3. Los Datos de Prueba y Clases Auxiliares (`MainWindow.axaml.cs`)

Para alimentar los gráficos sin necesidad de una base de datos real, creamos clases de datos (estructuras para organizar la información) y un método generador de pruebas.

### Las Clases de Datos

```csharp
public class EstadisticaCliente
{
    public string? Cliente { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
    public int TotalReparaciones { get; set; }
}

public class MiniGraficoCliente
{
    public string? Titulo { get; set; }
    public OxyPlot.PlotModel? Modelo { get; set; }
}

public class Reparacion
{
    public string? NombreCliente { get; set; }
    public DateTime FechaInicial { get; set; }
}

public class EstadisticaMensual
{
    public int Ano { get; set; }
    public int Mes { get; set; }
    public int TotalReparaciones { get; set; }
}
```

* **¿Qué es una clase aquí?** Son moldes u objetos contenedores de propiedades con `get; set;` (leer y escribir).
* **El signo `?` (Tipos anulables):** En `string? Cliente`, el interrogante le indica a C# que el texto puede aceptar valores nulos sin romper el programa.
* **`MiniGraficoCliente`:** Es un puente clave para el `ItemsControl`. Contiene un `Titulo` (el texto que identifica al cliente) y un `Modelo` de tipo `PlotModel` (el objeto gráfico generado por OxyPlot que se dibujará en ese mini-recuadro).

### El Generador de Datos Falsos

```csharp
private List<Reparacion> ObtenerDatosDePrueba()
{
    return new List<Reparacion>
    {
        new Reparacion { NombreCliente = "Juan", FechaInicial = new DateTime(2024, 1, 10) },
        new Reparacion { NombreCliente = "Ana", FechaInicial = new DateTime(2024, 1, 15) },
        new Reparacion { NombreCliente = "Luis", FechaInicial = new DateTime(2024, 2, 5) },
        new Reparacion { NombreCliente = "Juan", FechaInicial = new DateTime(2024, 2, 20) },
        new Reparacion { NombreCliente = "Juan", FechaInicial = new DateTime(2024, 2, 22) },
        new Reparacion { NombreCliente = "Marta", FechaInicial = new DateTime(2024, 2, 28) },
        new Reparacion { NombreCliente = "Ana", FechaInicial = new DateTime(2023, 12, 5) }
    };
}
```

* **`List<Reparacion>`:** Devuelve una lista genérica en memoria con 7 objetos de tipo `Reparacion`.
* **`new DateTime(Año, Mes, Día)`:** Estructura nativa de C# que encapsula fechas exactas para poder filtrarlas y agruparlas después mediante LINQ.

---

## 4. El Constructor y el Enlace de Botones

```csharp
public MainWindow()
{
    InitializeComponent();

    // --- BOTONES SECCIÓN 1: ESTADÍSTICAS GENERALES ---
    var btCalcular = this.GetControl<Button>("BtCalcular");
    btCalcular.Click += (_, _) => CalcularEstadisticas();

    var btCalcularAnual = this.GetControl<Button>("BtCalcularAnual");
    btCalcularAnual.Click += (_, _) => CalcularEstadisticasAnuales();

    // --- BOTONES SECCIÓN 2: ESTADÍSTICAS POR CLIENTE ---
    var btCalcularClientes = this.GetControl<Button>("BtCalcularClientes");
    btCalcularClientes.Click += (_, _) => CalcularEstadisticasPorCliente();

    var btCalcularClienteMensual = this.GetControl<Button>("BtCalcularClienteMensual");
    btCalcularClienteMensual.Click += (_, _) => CalcularEstadisticasClienteMensual();

    var btCalcularClienteAnual = this.GetControl<Button>("BtCalcularClienteAnual");
    btCalcularClienteAnual.Click += (_, _) => CalcularEstadisticasClienteAnual();
}

private void InitializeComponent()
{
    AvaloniaXamlLoader.Load(this);
}
```

* **`MainWindow()`:** Es el método constructor que se ejecuta automáticamente al abrir la ventana.
* **`this.GetControl<Button>("Nombre")`:** Es el método de Avalonia para buscar un control gráfico definido en el archivo XAML utilizando su atributo `Name="..."` y vincularlo al código de C#.
* **`+= (_, _) => ...`:** Esto es una **expresión lambda** (función anónima). Suscribe un evento: cada vez que el usuario hace clic (`Click`) en el botón, C# ejecuta la función que hay a la derecha de la flecha `=>`. Los subrayados `(_, _)` descartan los parámetros del evento que no necesitamos.

---

## 5. Sección 1: Estadísticas Generales (Meses y Años)

Estos métodos recogen todas las reparaciones del taller, las agrupan por fechas utilizando LINQ y generan un gráfico de barras horizontales estilizado.

### Desglose Mensual (`CalcularEstadisticas`)

```csharp
private void CalcularEstadisticas()
{
    List<Reparacion> todasLasReparaciones = ObtenerDatosDePrueba();

    // 1. Agrupación por LINQ
    var reparacionesPorMes = todasLasReparaciones
        .GroupBy(r => new { r.FechaInicial.Year, r.FechaInicial.Month })
        .Select(grupo => new EstadisticaMensual
        {
            Ano = grupo.Key.Year,
            Mes = grupo.Key.Month,
            TotalReparaciones = grupo.Count()
        })
        .OrderBy(x => x.Ano).ThenBy(x => x.Mes)
        .ToList();

    // 2. Creación del Lienzo (PlotModel)
    var plotModel = new OxyPlot.PlotModel
    {
        Title = "Reparaciones por Mes",
        TitleFont = "Segoe UI",
        TitleFontSize = 15,
        TitleFontWeight = OxyPlot.FontWeights.Bold,
        DefaultFont = "Segoe UI",
        TextColor = OxyPlot.OxyColor.Parse("#334155"),
        PlotAreaBorderThickness = new OxyPlot.OxyThickness(0),
        Background = OxyPlot.OxyColors.Transparent
    };

    // 3. Configuración de Ejes para BarSeries (Barras horizontales)
    var categoryAxis = new OxyPlot.Axes.CategoryAxis
    {
        Position = OxyPlot.Axes.AxisPosition.Left,
        TickStyle = OxyPlot.Axes.TickStyle.None,
        AxislineStyle = OxyPlot.LineStyle.Solid,
        AxislineColor = OxyPlot.OxyColor.Parse("#CBD5E1"),
        Key = "EjeY"
    };

    var valueAxis = new OxyPlot.Axes.LinearAxis
    {
        Position = OxyPlot.Axes.AxisPosition.Bottom,
        MinimumPadding = 0,
        AbsoluteMinimum = 0,
        Minimum = 0,
        MajorStep = 1,
        MinorStep = 1,
        MaximumPadding = 0.15,
        MajorGridlineStyle = OxyPlot.LineStyle.Dash,
        MajorGridlineColor = OxyPlot.OxyColor.Parse("#E2E8F0"),
        AxislineStyle = OxyPlot.LineStyle.Solid,
        AxislineColor = OxyPlot.OxyColor.Parse("#CBD5E1"),
        Key = "EjeX"
    };

    plotModel.Axes.Add(categoryAxis);
    plotModel.Axes.Add(valueAxis);

    // 4. Creación de la Serie de Datos
    var barSeries = new OxyPlot.Series.BarSeries
    {
        Title = "Reparaciones",
        FillColor = OxyPlot.OxyColor.Parse("#3B82F6"),   // Azul moderno
        StrokeColor = OxyPlot.OxyColor.Parse("#1D4ED8"), // Borde azul oscuro
        StrokeThickness = 1,
        BarWidth = 0.6,
        LabelFormatString = "{0:#}",
        LabelPlacement = OxyPlot.Series.LabelPlacement.Outside,
        TextColor = OxyPlot.OxyColor.Parse("#1E293B"),
        XAxisKey = "EjeX",
        YAxisKey = "EjeY"
    };

    // 5. Rellenar datos en los ejes y series
    foreach (var dato in reparacionesPorMes)
    {
        categoryAxis.Labels.Add($"{dato.Mes:00}/{dato.Ano}");
        barSeries.Items.Add(new OxyPlot.Series.BarItem { Value = dato.TotalReparaciones });
    }

    plotModel.Series.Add(barSeries);

    // 6. Inyectar el modelo en el control visual PlotView de la ventana
    var plotView = this.GetControl<OxyPlot.Avalonia.PlotView>("PlotEstadisticas");
    plotView.Model = plotModel;
    plotView.InvalidatePlot(true);
}
```

* **¿Qué es LINQ (`GroupBy`, `Select`, `OrderBy`)?** Es la herramienta de C# para consultar y transformar listas de objetos con sintaxis muy limpia. Aquí agrupamos las reparaciones por año y mes, contamos cuántas hay (`grupo.Count()`) y las ordenamos cronológicamente.
* **¿Por qué `BarSeries` con ejes cruzados?** En OxyPlot, `BarSeries` dibuja barras horizontales. Para ello, exige que el eje de categorías (meses o años) esté a la izquierda (`AxisPosition.Left`) y el eje de números esté abajo (`AxisPosition.Bottom`). Esto hace que los nombres largos de los meses o años se lean perfectamente sin solaparse.

*(Nota: `CalcularEstadisticasAnuales` hace exactamente lo mismo pero agrupando únicamente por `r.FechaInicial.Year`).*

---

## 6. Sección 2: Estadísticas por Cliente (Histórico y Mini-Gráficos)

### Total Histórico (`CalcularEstadisticasPorCliente`)

```csharp
private void CalcularEstadisticasPorCliente()
{
    List<Reparacion> todasLasReparaciones = ObtenerDatosDePrueba();

    var reparacionesPorCliente = todasLasReparaciones
        .GroupBy(r => r.NombreCliente)
        .Select(grupo => new EstadisticaCliente
        {
            Cliente = grupo.Key,
            Ano = 0, 
            Mes = 0,
            TotalReparaciones = grupo.Count()
        })
        .OrderBy(x => x.Cliente)
        .ToList();

    var plotModel = new OxyPlot.PlotModel 
    { 
        Title = "Total Histórico de Reparaciones por Cliente",
        TitleFont = "Segoe UI",
        TitleFontSize = 15,
        TitleFontWeight = OxyPlot.FontWeights.Bold,
        DefaultFont = "Segoe UI",
        TextColor = OxyPlot.OxyColor.Parse("#334155"),
        PlotAreaBorderThickness = new OxyPlot.OxyThickness(0),
        Background = OxyPlot.OxyColors.Transparent
    };

    var categoryAxis = new OxyPlot.Axes.CategoryAxis { Position = OxyPlot.Axes.AxisPosition.Left, Key = "EjeY" };
    var valueAxis = new OxyPlot.Axes.LinearAxis { Position = OxyPlot.Axes.AxisPosition.Bottom, MinimumPadding = 0, AbsoluteMinimum = 0, Key = "EjeX" };
    
    plotModel.Axes.Add(categoryAxis);
    plotModel.Axes.Add(valueAxis);

    var barSeries = new OxyPlot.Series.BarSeries 
    { 
        Title = "Reparaciones", 
        FillColor = OxyPlot.OxyColor.Parse("#10B981"),
        StrokeColor = OxyPlot.OxyColor.Parse("#059669"),
        StrokeThickness = 1,
        XAxisKey = "EjeX",
        YAxisKey = "EjeY"
    };

    foreach (var dato in reparacionesPorCliente)
    {
        categoryAxis.Labels.Add(dato.Cliente ?? "Desconocido");
        barSeries.Items.Add(new OxyPlot.Series.BarItem { Value = dato.TotalReparaciones });
    }

    plotModel.Series.Add(barSeries);

    // 1. Mostramos el gráfico general de histórico y asignamos los datos
    var plotView = this.GetControl<OxyPlot.Avalonia.PlotView>("PlotEstadisticasClientes");
    plotView.IsVisible = true;
    plotView.Model = plotModel;
    plotView.InvalidatePlot(true); 

    // 2. Vaciamos la lista de mini-gráficos para que desaparezca
    var listaControles = this.GetControl<Avalonia.Controls.ItemsControl>("ListaGraficosClientes");
    listaControles.ItemsSource = null;
}
```

### Desglose Mensual por Cliente (`CalcularEstadisticasClienteMensual`)

```csharp
private void CalcularEstadisticasClienteMensual()
{
    List<Reparacion> todasLasReparaciones = ObtenerDatosDePrueba();
    if (!todasLasReparaciones.Any()) return;

    var clientesUnicos = todasLasReparaciones
        .Select(r => r.NombreCliente ?? "Desconocido")
        .Distinct()
        .OrderBy(c => c)
        .ToList();

    var listaMiniGraficos = new List<MiniGraficoCliente>();

    foreach (var cliente in clientesUnicos)
    {
        var plotModel = new OxyPlot.PlotModel
        {
            DefaultFont = "Segoe UI",
            TextColor = OxyPlot.OxyColor.Parse("#334155"),
            PlotAreaBorderThickness = new OxyPlot.OxyThickness(0),
            Background = OxyPlot.OxyColors.Transparent,
            PlotMargins = new OxyPlot.OxyThickness(55, 5, 25, 25)
        };

        var categoryAxis = new OxyPlot.Axes.CategoryAxis
        {
            Position = OxyPlot.Axes.AxisPosition.Left,
            Key = "EjeY"
        };

        var valueAxis = new OxyPlot.Axes.LinearAxis
        {
            Position = OxyPlot.Axes.AxisPosition.Bottom,
            Minimum = 0,
            MajorStep = 1,
            MinorStep = 1,
            MaximumPadding = 0.2,
            Key = "EjeX"
        };

        plotModel.Axes.Add(categoryAxis);
        plotModel.Axes.Add(valueAxis);

        var barSeries = new OxyPlot.Series.BarSeries
        {
            FillColor = OxyPlot.OxyColor.Parse("#059669"),
            StrokeColor = OxyPlot.OxyColor.Parse("#047857"),
            StrokeThickness = 1,
            BarWidth = 0.55,
            LabelFormatString = "{0:#}",
            LabelPlacement = OxyPlot.Series.LabelPlacement.Outside,
            TextColor = OxyPlot.OxyColor.Parse("#0F172A"),
            XAxisKey = "EjeX",
            YAxisKey = "EjeY"
        };

        var datosCliente = todasLasReparaciones
            .Where(r => (r.NombreCliente ?? "Desconocido") == cliente)
            .GroupBy(r => new { r.FechaInicial.Year, r.FechaInicial.Month })
            .Select(g => new
            {
                Anio = g.Key.Year,
                Mes = g.Key.Month,
                MesAno = $"{g.Key.Month:00}/{g.Key.Year}",
                Total = g.Count()
            })
            .OrderBy(x => x.Anio).ThenBy(x => x.Mes)
            .ToList();

        foreach (var dato in datosCliente)
        {
            categoryAxis.Labels.Add(dato.MesAno);
            barSeries.Items.Add(new OxyPlot.Series.BarItem { Value = dato.Total });
        }

        plotModel.Series.Add(barSeries);

        listaMiniGraficos.Add(new MiniGraficoCliente
        {
            Titulo = $"👤 {cliente} — Desglose Mensual",
            Modelo = plotModel
        });
    }

    // 1. Ocultar el PlotView del histórico general
    var plotView = this.GetControl<OxyPlot.Avalonia.PlotView>("PlotEstadisticasClientes");
    plotView.IsVisible = false;
    plotView.Model = null;

    // 2. Cargar los mini-gráficos en el ItemsControl
    var listaControles = this.GetControl<Avalonia.Controls.ItemsControl>("ListaGraficosClientes");
    listaControles.ItemsSource = listaMiniGraficos;
}
```

* **¿Qué es `Distinct()`?** Filtra una lista para eliminar duplicados. Como Juan aparece varias veces en los datos de prueba, `Distinct()` nos devuelve "Juan" una sola vez para saber cuántas tarjetas independientes debemos crear.
* **¿Cómo funciona el `ItemsControl`?** Le pasamos una lista de objetos `MiniGraficoCliente` a la propiedad `ItemsSource`. Gracias a la plantilla visual del XAML (`DataTemplate`), Avalonia coge cada elemento, pinta una tarjeta bonita (`Border`), coloca el texto del título y dibuja dentro el gráfico (`PlotView`) correspondiente de forma automática.

*(Nota: `CalcularEstadisticasClienteAnual` hace exactamente lo mismo pero agrupando por `r.FechaInicial.Year`).*

---

## 7. Argumentos Clave para tu Defensa Oral

Si el profesor te pregunta detalles críticos durante la exposición, responde con seguridad basándote en estos argumentos técnicos:

1. **¿Cómo se estructuran los paneles visuales en Avalonia?**
   * *Respuesta:* "Utilizamos un `ScrollViewer` combinado con controles `StackPanel` para apilar verticalmente las tarjetas de contenido. Dentro de cada tarjeta aplicamos contenedores `Border` para dar estilo de tarjeta moderna con esquinas redondeadas y sombras, y un `ItemsControl` con plantillas `DataTemplate` para generar dinámicamente los mini-gráficos."
2. **¿Por qué usas `BarSeries` en lugar de `ColumnSeries`?**
   * *Respuesta:* "En las versiones recientes de OxyPlot (2.1 en adelante), la clase `ColumnSeries` fue deprecada y unificada dentro de `BarSeries`. Configurando correctamente las posiciones de los ejes (`CategoryAxis` a la izquierda y `LinearAxis` abajo), `BarSeries` renderiza barras perfectamente adaptadas y evita errores de compilación."
3. **¿Cómo funciona el enlace de datos en los mini-gráficos dinámicos?**
   * *Respuesta:* "Utilizamos un control contenedor `ItemsControl` en Avalonia combinado con una lista tipada de objetos auxiliares (`List<MiniGraficoCliente>`). Cada objeto contiene un título en texto plano y un modelo matemático independiente (`PlotModel`), permitiendo renderizar tarjetas personalizadas en tiempo de ejecución de manera eficiente."
4. **¿Por qué manejas los eventos con expresiones lambda (`=>`)?**
   * *Respuesta:* "Porque es el estándar moderno en C# y .NET Core. Permite suscribir manejadores a los eventos `Click` de los botones de forma directa, limpia y sin necesidad de declarar métodos sueltos para cada pequeña acción, manteniendo el código cohesionado."

¡Estudiándote este desglose paso a paso del XAML y del C#, dominarás el proyecto por completo y podrás defenderlo con matrícula!