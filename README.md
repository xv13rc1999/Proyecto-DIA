# Sistema de Gestión de Taller de Coches — Módulo de Gestión de Reparaciones

**Asignatura:** Desenvolvemento e Integración de Aplicacións (DIA)  
**Tecnologías:** C# | .NET 8.0 / .NET 10.0 | Avalonia UI | Persistencia XML  
**Patrón de Diseño:** Separación de Capas (Core / View) con Controladores de Interfaz  

---

## 📋 Índice
1. [Descripción del Proyecto](#descripción-del-proyecto)
2. [Estructura del Proyecto](#estructura-del-proyecto)
3. [Explicación por Componentes](#explicación-por-componentes)
   - [1. Capa Core (Lógica de Negocio y Persistencia)](#1-capa-core-lógica-de-negocio-y-persistencia)
   - [2. Capa View (Interfaz de Usuario y Controladores)](#2-capa-view-interfaz-de-usuario-y-controladores)
4. [Justificación de Decisiones Técnicas ("¿Por qué de esta forma?")](#justificación-de-decisiones-técnicas-por-qué-de-esta-forma)
5. [Guía de Compilación y Ejecución](#guía-de-compilación-y-ejecución)
6. [Posibles Preguntas del Profesor para la Defensa](#posibles-preguntas-del-profesor-para-la-defensa)

---

## 🚗 Descripción del Proyecto

Este proyecto forma parte de una aplicación global de **Gestión de un Taller de Coches**. En concreto, este módulo implementa la **Gestión de Reparaciones**, permitiendo:
- **Altas, Bajas y Modificaciones** de órdenes de reparación.
- **Asignación de Trabajador Encargado y Cliente** involucrado.
- **Control de Estado** (Pendiente, Terminada, etc.) de forma escalable.
- **Gestión de Fechas y Notas** de diagnóstico/intervención.
- **Consultas y Filtrado** dinámico según el estado de la reparación.
- **Salvaguarda y Recuperación de datos** mediante serialización XML (`XmlSerializer`).

---

## 📁 Estructura del Proyecto

El código sigue estrictamente el estándar de organización exigido para la asignatura, separando la capa lógica (**Core**) de la capa de presentación (**View**):

```text
TallerCoches/
├── Core/
│   ├── Models/
│   │   ├── Cliente.cs              # Modelo básico de apoyo (simulado para integración)
│   │   ├── Personal.cs             # Modelo básico de apoyo (simulado para integración)
│   │   ├── EstadoReparacion.cs     # Enumerado escalable con los estados de la reparación
│   │   └── Reparacion.cs           # Modelo principal de datos
│   ├── Persistence/
│   │   └── XmlRepository.cs        # Lectura y escritura en fichero XML
│   └── Services/
│       └── GestorReparaciones.cs   # Servicio principal (CRUD y filtrado dinámico)
├── docs/
│   └── proyecto-9-taller.html      # Enunciado de la práctica en formato .html
├── View/
│   ├── App.axaml / App.axaml.cs    # Inicialización y estilos globales de Avalonia UI
│   ├── ReparacionesView.axaml      # XAML de la ventana principal de lista y filtros
│   ├── ReparacionesView.axaml.cs   # Code-Behind de la vista principal (exposición de controles)
│   ├── ReparacionesCtrl.cs         # Controlador de la vista principal (gestión de eventos)
│   ├── FormReparacion.axaml        # XAML del formulario modal (Alta / Edición)
│   ├── FormReparacion.axaml.cs     # Code-Behind del formulario
│   └── FormReparacionCtrl.cs      # Controlador del formulario modal
├── Program.cs                      # Punto de entrada de la aplicación
└── TallerCoches.csproj             # Configuración del proyecto y dependencias NuGet
```

---

## 🛠️ Explicación por Componentes

La aplicación está dividida en dos capas principales claramente diferenciadas: la capa **Core** (independiente de la interfaz gráfica y reutilizable en cualquier entorno) y la capa **View** (construida sobre Avalonia UI siguiendo el patrón XAML + Code-Behind + Controlador).

---

### 1. Capa Core (Lógica de Negocio, Dominio y Persistencia)

Esta capa contiene las entidades de datos, los algoritmos de filtrado/modificación y la persistencia en disco. **No tiene ninguna dependencia con Avalonia UI.**

#### 📦 `Core/Models/` (Entidades de Dominio)
* **`EstadoReparacion.cs`**:
  * **Función:** Enumerado C# (`enum`) que define los estados posibles de una reparación (`Pendiente`, `Terminada`, etc.).
  * **Propósito:** Sirve como la única fuente de verdad sobre el estado de las intervenciones, permitiendo que la interfaz y los servicios se adapten dinámicamente si se añaden nuevos estados en el futuro.
* **`Reparacion.cs`**:
  * **Función:** Modelo principal del dominio que representa una orden de trabajo en el taller.
  * **Propiedades:** `Id` (autonumerado), `TrabajadorEncargado` (`Personal`), `Cliente` (`Cliente`), `Asunto` (`string`), `FechaInicial` (`DateTime`), `FechaFinal` (`DateTime?`), `Notas` (`string`) y `Estado` (`EstadoReparacion`).
  * **Métodos:** Sobrescribe `ToString()` para ofrecer un resumen formateado de la orden de reparación.
* **`Cliente.cs` y `Personal.cs`**:
  * **Función:** Clases auxiliares de apoyo con los atributos requeridos (`DNI`, `Nombre`, `Email`, etc.).
  * **Propósito:** Proveen los objetos necesarios para simular la asignación de clientes y mecánicos a una reparación mientras los compañeros del grupo desarrollan sus respectivos módulos.

#### 💾 `Core/Persistence/` (Acceso a Datos)
* **`XmlRepository.cs`**:
  * **Función:** Encargado exclusivo de la persistencia de datos en disco mediante serialización XML.
  * **Métodos Clave:**
    * `CargarReparaciones()`: Lee el archivo XML (`reparaciones.xml`) utilizando `XmlSerializer`. Si el fichero no existe o ocurre un fallo durante la deserialización, captura la excepción y retorna una lista vacía para evitar que la aplicación colapse.
    * `GuardarReparaciones(List<Reparacion>)`: Serializa y sobrescribe la lista completa de reparaciones en el archivo XML.

#### ⚙️ `Core/Services/` (Servicios de Negocio)
* **`GestorReparaciones.cs`**:
  * **Función:** Servicio central de la lógica del módulo de reparaciones.
  * **Responsabilidades:**
    * Mantiene en memoria la colección de reparaciones cargada desde `XmlRepository`.
    * **Altas (`Alta`):** Calcula automáticamente un nuevo `Id` autonumerado secuencial (`Max(Id) + 1`).
    * **Modificaciones (`Modificacion`) y Bajas (`Baja`):** Actualiza o elimina el registro correspondiente mediante LINQ.
    * **Consultas Dinámicas (`ObtenerPorEstado`):** Filtra las reparaciones según cualquier valor de `EstadoReparacion` sin requerir métodos duplicados o cableados.

---

### 2. Capa View (Interfaz de Usuario y Controladores)

Cada pantalla gráfica está compuesta por un **trío de archivos** para garantizar la separación entre el diseño visual, la exposición de componentes y la lógica de presentación.

#### 🖥️ Vista Principal de Reparaciones
* **`View/ReparacionesView.axaml` (Diseño XAML):**
  * Declara la estructura gráfica principal: barra superior con desplegable de filtros (`ComboBox`), listado central (`ListBox`) y botones de acción en la parte inferior (`Nueva Reparación`, `Editar Seleccionada`, `Eliminar`).
* **`View/ReparacionesView.axaml.cs` (Code-Behind):**
  * Llama a `InitializeComponent()` y obtiene las referencias de los controles definidos en el XAML utilizando `this.GetControl<T>("nombre")`.
  * **Regla de arquitectura:** No contiene lógica de negocio ni manejadores de eventos. Únicamente expone los controles como propiedades públicas (`BtnAlta`, `CbFiltroEstado`, `LbReparaciones`, etc.).
* **`View/ReparacionesCtrl.cs` (Controlador Principal):**
  * Instancia la vista (`ReparacionesView`) y el servicio (`GestorReparaciones`).
  * Suscribe métodos a los eventos de los controles de la vista (`BtnAlta.Click`, `CbFiltroEstado.SelectionChanged`, `Closing`).
  * Ejecuta la carga dinámicamente poblando el desplegable de estados y refrescando la lista cuando el usuario cambia la selección o guarda un registro.

#### 📝 Formulario Modal de Alta / Modificación
* **`View/FormReparacion.axaml` (Diseño XAML):**
  * Define el cuadro de diálogo para introducir o editar los datos de una reparación (`TextBox` para asunto/notas, `ComboBox` para cliente/mecánico/estado y `CalendarDatePicker` para las fechas).
* **`View/FormReparacion.axaml.cs` (Code-Behind):**
  * Mapea y expone los campos de entrada y los botones `Guardar` y `Cancelar`.
* **`View/FormReparacionCtrl.cs` (Controlador del Formulario):**
  * Recibe como parámetro una `Reparacion` opcional (si es `null` opera en modo *Alta*; si contiene un objeto opera en modo *Edición*).
  * Puebla el desplegable `CbEstado` con los valores del enumerado mediante `Enum.GetValues<EstadoReparacion>()`.
  * Rellena los datos de los clientes y personal (simulados vía métodos auxiliares `obtenerClientes()` y `obtenerPersonal()`).
  * Al pulsar `Guardar`, construye el objeto `Reparacion` validado y lo retorna al controlador principal mediante una función *callback*.

#### 🚀 Arranque y Configuración Global
* **`View/App.axaml` y `View/App.axaml.cs`:**
  * Configura el tema visual global (`FluentTheme`).
  * En `OnFrameworkInitializationCompleted()`, instancia el controlador principal `ReparacionesCtrl` y asigna su vista a la ventana inicial (`desktop.MainWindow`).
* **`Program.cs`:**
  * Punto de entrada de la aplicación en C#. Inicializa el motor de Avalonia UI con `AppBuilder.Configure<App>().UsePlatformDetect()` y lanza el ciclo de vida del escritorio (`StartWithClassicDesktopLifetime`).

---

## 💡 Justificación de Decisiones Técnicas

En esta sección se detallan las decisiones de arquitectura, diseño y patrones de software adoptadas durante el desarrollo del módulo de **Gestión de Reparaciones**, justificando por qué se eligieron frente a otras alternativas convencionales.

---

### 📊 Tabla Comparativa de Decisiones

| Decisión Implementada | Alternativa Descartada | Justificación Técnica y Beneficios |
| :--- | :--- | :--- |
| **Separación estricta Core / View** | Código monolítico en el formulario | **Reutilización e Independencia**: La capa `Core` es un ensamblado DLL neutro. Podría ser consumida por una interfaz web, de consola o móvil sin modificar una sola línea de lógica de negocio o persistencia. |
| **Patrón Controlador (`...Ctrl.cs`)** | Lógica de negocio en Code-Behind (`.axaml.cs`) | **Desacoplamiento UI**: Mantiene los archivos `.axaml.cs` limpios, limitados a exponer los controles. Facilita las pruebas unitarias de presentación y cumple rigurosamente los requisitos del profesor. |
| **Filtrado dinámico por `Enum`** | Métodos hardcodeados por estado (`ObtenerPendientes()`, etc.) | **Escalabilidad OCP (Open/Closed Principle)**: Si se agregan nuevos estados al taller (ej. `Presupuestada`, `EnEsperaPiezas`), el sistema los soporta sin tocar `GestorReparaciones.cs` ni el controlador. |
| **Poblado dinámico de `ComboBox` desde C#** | Elementos `<ComboBoxItem>` estáticos en XAML | **Sincronización Automática**: Elimina la duplicación de texto entre la vista y el modelo, garantizando que el desplegable siempre refleje los valores reales del tipo `EstadoReparacion`. |
| **Carga defensiva con `XmlSerializer`** | Deserialización directa sin captura de fallos | **Tolerancia a Fallos**: Evita que la aplicación colapse si el fichero XML no existe, está vacío o presenta corrupción de datos, garantizando una experiencia de usuario fluida. |
| **Mapeo desacoplado con Funciones Auxiliares** | Dependencia directa con repositorios no finalizados | **Desarrollo Paralelo**: Permite desarrollar y probar el módulo de Reparaciones mediante datos simulados (*Mock Data*) sin bloquear el avance del equipo a la espera de los módulos de Clientes y Personal. |

---

### 🔍 Explicación Detallada de las Decisiones

#### 1. Arquitectura en Capas y Patrón Controlador (`Core` / `View`)
* **Problema que resuelve:** En aplicaciones XAML pequeñas es común mezclar la lógica de guardado y filtrado dentro de los manejadores de eventos del archivo Code-Behind (`.axaml.cs`). Esto genera código espagueti difícil de mantener y probar.
* **Solución aplicada:** Se dividió la solución en dos proyectos/carpetas independientes. La capa `View` se descompuso en tres responsabilidades:
  1. **XAML (`.axaml`)**: Define la estructura visual declarativa.
  2. **Code-Behind (`.axaml.cs`)**: Instancia y expone los componentes visuales como propiedades públicas.
  3. **Controlador (`...Ctrl.cs`)**: Gestiona el flujo de pantalla, escucha eventos de usuario y orquesta llamadas al servicio `GestorReparaciones`.

#### 2. Escalabilidad mediante Enumerados y Vinculación Dinámica
* **Problema que resuelve:** Modificar los estados de un proceso (como una reparación) suele requerir cambios repetitivos en múltiples archivos XAML, métodos de filtrado y sentencias `switch/case`.
* **Solución aplicada:** Se definió el enumerado `EstadoReparacion.cs` como única fuente de verdad. El controlador carga las opciones mediante `Enum.GetValues<EstadoReparacion>()` y el servicio utiliza LINQ genérico (`_reparaciones.Where(r => r.Estado == estado)`). Añadir un nuevo estado al enum actualiza automáticamente la interfaz gráfica y los algoritmos de filtrado.

#### 3. Persistencia Defensiva en `XmlRepository`
* **Problema que resuelve:** Las excepciones no controladas en el parsing XML provocan cierres inesperados al arrancar la aplicación si el archivo de datos está dañado o no se ha creado aún.
* **Solución aplicada:** Se implementó una verificación previa mediante `File.Exists()`. Durante la lectura, el bloque `try-catch` captura cualquier `XmlException` o `InvalidOperationException`, inicializando una lista en blanco segura (`new List<Reparacion>()`) y registrando la incidencia sin interrumpir la ejecución del programa.

#### 4. Estrategia de Integración Modular (Puntos de Extensión)
* **Problema que resuelve:** En trabajos en equipo, el desarrollo de un módulo no debe depender de que los demás integrantes hayan completado la persistencia de sus respectivas entidades.
* **Solución aplicada:** En `FormReparacionCtrl.cs` se aislaron la obtención de `Cliente` y `Personal` en los métodos privados `obtenerClientes()` y `obtenerPersonal()`. Durante la fase individual devuelven colecciones *mock*, permitiendo que al momento de la integración conjunta solo sea necesario sustituir el cuerpo de dichos métodos por la consulta al repositorio final del grupo.

#### 5. Gestión de Conflictos en Controles de Selección de Avalonia UI
* **Problema que resuelve:** En Avalonia UI, si un control `ComboBox` tiene declarados elementos `<ComboBoxItem>` en el marcado XAML y posteriormente se le asigna un objeto a su propiedad `ItemsSource` desde C#, el motor lanza una excepción de conflicto de colección (`SetItemsSource`).
* **Solución aplicada:** Se eliminaron los elementos fijos del XAML y se añadió la llamada explícita `View.CbFiltroEstado.Items.Clear()` en la inicialización del controlador previo a asignar la fuente de datos, garantizando una carga limpia y estable en tiempo de ejecución.

---

## 🚀 Guía de Compilación y Ejecución

Esta guía detalla los requisitos previos, los comandos para compilar y ejecutar el módulo de **Gestión de Reparaciones** utilizando el CLI de .NET (`dotnet`), así como los procedimientos para limpiar la caché de compilación y resolver incidencias habituales en Avalonia UI.

---

### 1. Prerrequisitos del Sistema

Antes de compilar la aplicación, asegúrate de contar con las siguientes herramientas instaladas en tu equipo:

1. **SDK de .NET:**
   - Compatible con **.NET 8.0**, **.NET 9.0** o **.NET 10.0** (`net10.0`).
   - Puedes comprobar los SDKs instalados ejecutando en tu terminal:
     ```bash
     dotnet --list-sdks
     ```
2. **Paquetes NuGet de Avalonia UI:**
   El proyecto requiere las siguientes dependencias configuradas en el archivo `.csproj`:
   - `Avalonia` (v11.x / v12.x)
   - `Avalonia.Desktop` (Soporte para plataformas de escritorio)
   - `Avalonia.Themes.Fluent` (Tema visual Fluent)
   - `Avalonia.Fonts.Inter` (Tipografía predeterminada Inter)

---

### 2. Estructura del Archivo `.csproj`

Verifica que el archivo de proyecto (`TallerCoches.csproj` o `Proyecto.csproj`) contenga la inclusión explícita de los archivos `.axaml` como recursos de Avalonia:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <Nullable>enable</Nullable>
    <BuiltInComInteropSupport>true</BuiltInComInteropSupport>
    <ApplicationManifest>app.manifest</ApplicationManifest>
  </PropertyGroup>

  <!-- Inclusión obligatoria de archivos .axaml como recursos compilados -->
  <ItemGroup>
    <AvaloniaResource Include="**\*.axaml"/>
  </ItemGroup>

  <!-- Dependencias NuGet -->
  <ItemGroup>
    <PackageReference Include="Avalonia" Version="12.1.3"/>
    <PackageReference Include="Avalonia.Desktop" Version="12.1.3"/>
    <PackageReference Include="Avalonia.Themes.Fluent" Version="12.1.3"/>
    <PackageReference Include="Avalonia.Fonts.Inter" Version="12.1.3"/>
  </ItemGroup>
</Project>

```

Nota: Durante la fase de pruebas y desarrollo se mantiene ```<OutputType>Exe</OutputType>``` para visualizar excepciones XAML y mensajes de depuración por consola.

---

### 3. Comandos de Compilación y Ejecución

Abre una terminal en la raíz del proyecto (donde se ubica el archivo ```.csproj```):

#### 🔄 1. Restaurar dependencias NuGet

Descarga y registra los paquetes de Avalonia en la caché local:

``` bash
dotnet restore
```

#### 🛠️ 2. Compilar el proyecto (Debug)

Compila el código C# y las vistas XAML comprobando errores de sintaxis:

``` bash
dotnet build
```

#### ▶️ 3. Ejecutar la aplicación

Compila en memoria y arranca la interfaz gráfica de la aplicación:

``` bash
dotnet run
```

#### 📦 4. Generar ejecutable de producción (Release)

Genera los binarios optimizados en la carpeta ```bin/Release/net10.0/publish/```:

``` bash
dotnet run
```

---

## ❓ Posibles Preguntas del Profesor para la Defensa

1. ¿Cómo se garantiza la separación de responsabilidades entre el Core y la UI?

La capa ``Core`` no contiene ninguna referencia a ``Avalonia`` ni a elementos gráficos. La comunicación es unidireccional: los controladores de la capa ``View`` consumen los servicios del ``Core`` (``GestorReparaciones``), mientras que las vistas XAML y sus code-behind solo se encargan de exponer y renderizar componentes.

2. ¿Qué ocurre si ampliamos los estados de la reparación en el futuro?

La solución es totalmente escalable. Al añadir un nuevo estado a ``EstadoReparacion.cs``, el método ``ObtenerPorEstado`` en ``GestorReparaciones.cs`` y la carga dinámica del ``ComboBox`` en el controlador actualizarán la interfaz automáticamente sin necesidad de refactorizar código.

3. ¿Cómo maneja el sistema los fallos en la persistencia XML?

``XmlRepository`` verifica la presencia del archivo con ``File.Exists()``. Durante la deserialización, captura excepciones en un bloque ``try-catch``. Si el archivo no existe o contiene datos corruptos, devuelve una lista vacía para mantener la continuidad de la aplicación sin cierres inesperados.

4. ¿Cómo se integrará este módulo con los módulos de Clientes y Personal de los otros miembros del grupo?

Actualmente, ``FormReparacionCtrl.cs`` obtiene listas de prueba mediante métodos auxiliares. Para la integración final, basta con sustituir la devolución de esos métodos auxiliares por las llamadas a los repositorios o gestores desarrollados por los compañeros (``GestorClientes`` y ``GestorPersonal``).

5. ¿Por qué se vacía la colección Items antes de asignar ``ItemsSource`` en el ``ComboBox``?

En Avalonia UI, si un control ``ComboBox`` tiene elementos estáticos definidos en XAML y posteriormente se le asigna una colección vía C# mediante ``ItemsSource``, el motor lanza una excepción de conflicto. Al limpiar los elementos en el XAML y en el controlador, la vinculación de datos se realiza sin errores.