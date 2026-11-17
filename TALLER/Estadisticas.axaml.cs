using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Avalonia;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace TALLER
{
    // CUIDADO AQUÍ: Ahora hereda de UserControl, no de Window
    public partial class Estadisticas : UserControl
    {
        public Estadisticas()
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

        // ====================================================================
        // SECCIÓN 1: MÉTODOS DE ESTADÍSTICAS GENERALES DEL TALLER (TONOS AZULES)
        // ====================================================================

        private void CalcularEstadisticas()
        {
            List<Reparacion> todasLasReparaciones = ObtenerDatosDePrueba();

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

            var barSeries = new OxyPlot.Series.BarSeries
            {
                Title = "Reparaciones",
                FillColor = OxyPlot.OxyColor.Parse("#3B82F6"),   
                StrokeColor = OxyPlot.OxyColor.Parse("#1D4ED8"), 
                StrokeThickness = 1,
                BarWidth = 0.6,
                LabelFormatString = "{0:#}",
                LabelPlacement = OxyPlot.Series.LabelPlacement.Outside,
                TextColor = OxyPlot.OxyColor.Parse("#1E293B"),
                XAxisKey = "EjeX",
                YAxisKey = "EjeY"
            };

            foreach (var dato in reparacionesPorMes)
            {
                categoryAxis.Labels.Add($"{dato.Mes:00}/{dato.Ano}");
                barSeries.Items.Add(new OxyPlot.Series.BarItem { Value = dato.TotalReparaciones });
            }

            plotModel.Series.Add(barSeries);

            var plotView = this.GetControl<OxyPlot.Avalonia.PlotView>("PlotEstadisticas");
            plotView.Model = plotModel;
            plotView.InvalidatePlot(true);
        }

        private void CalcularEstadisticasAnuales()
        {
            List<Reparacion> todasLasReparaciones = ObtenerDatosDePrueba();

            var reparacionesPorAno = todasLasReparaciones
                .GroupBy(r => r.FechaInicial.Year)
                .Select(grupo => new
                {
                    Ano = grupo.Key,
                    TotalReparaciones = grupo.Count()
                })
                .OrderBy(x => x.Ano)
                .ToList();

            var plotModel = new OxyPlot.PlotModel
            {
                Title = "Reparaciones por Año",
                TitleFont = "Segoe UI",
                TitleFontSize = 15,
                TitleFontWeight = OxyPlot.FontWeights.Bold,
                DefaultFont = "Segoe UI",
                TextColor = OxyPlot.OxyColor.Parse("#334155"),
                PlotAreaBorderThickness = new OxyPlot.OxyThickness(0),
                Background = OxyPlot.OxyColors.Transparent
            };

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

            var barSeries = new OxyPlot.Series.BarSeries
            {
                Title = "Reparaciones",
                FillColor = OxyPlot.OxyColor.Parse("#2563EB"),   
                StrokeColor = OxyPlot.OxyColor.Parse("#1E40AF"),
                StrokeThickness = 1,
                BarWidth = 0.55,
                LabelFormatString = "{0:#}",
                LabelPlacement = OxyPlot.Series.LabelPlacement.Outside,
                TextColor = OxyPlot.OxyColor.Parse("#1E293B"),
                XAxisKey = "EjeX",
                YAxisKey = "EjeY"
            };

            foreach (var dato in reparacionesPorAno)
            {
                categoryAxis.Labels.Add(dato.Ano.ToString());
                barSeries.Items.Add(new OxyPlot.Series.BarItem { Value = dato.TotalReparaciones });
            }

            plotModel.Series.Add(barSeries);

            var plotView = this.GetControl<OxyPlot.Avalonia.PlotView>("PlotEstadisticas");
            plotView.Model = plotModel;
            plotView.InvalidatePlot(true);
        }

        // ====================================================================
        // SECCIÓN 2: MÉTODOS DE ESTADÍSTICAS POR CLIENTE (TONOS ESMERALDA)
        // ====================================================================

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

            var barSeries = new OxyPlot.Series.BarSeries
            {
                Title = "Reparaciones",
                FillColor = OxyPlot.OxyColor.Parse("#10B981"),   
                StrokeColor = OxyPlot.OxyColor.Parse("#059669"),
                StrokeThickness = 1,
                BarWidth = 0.6,
                LabelFormatString = "{0:#}",
                LabelPlacement = OxyPlot.Series.LabelPlacement.Outside,
                TextColor = OxyPlot.OxyColor.Parse("#1E293B"),
                XAxisKey = "EjeX",
                YAxisKey = "EjeY"
            };

            foreach (var dato in reparacionesPorCliente)
            {
                categoryAxis.Labels.Add(dato.Cliente ?? "Desconocido");
                barSeries.Items.Add(new OxyPlot.Series.BarItem { Value = dato.TotalReparaciones });
            }

            plotModel.Series.Add(barSeries);

            var plotView = this.GetControl<OxyPlot.Avalonia.PlotView>("PlotEstadisticasClientes");
            plotView.IsVisible = true;
            plotView.Model = plotModel;
            plotView.InvalidatePlot(true);

            var listaControles = this.GetControl<Avalonia.Controls.ItemsControl>("ListaGraficosClientes");
            listaControles.ItemsSource = null;
        }

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
                    TickStyle = OxyPlot.Axes.TickStyle.None,
                    AxislineStyle = OxyPlot.LineStyle.Solid,
                    AxislineColor = OxyPlot.OxyColor.Parse("#CBD5E1"),
                    Key = "EjeY"
                };

                var valueAxis = new OxyPlot.Axes.LinearAxis
                {
                    Position = OxyPlot.Axes.AxisPosition.Bottom,
                    Minimum = 0,
                    MajorStep = 1,
                    MinorStep = 1,
                    MaximumPadding = 0.2,
                    MajorGridlineStyle = OxyPlot.LineStyle.Dash,
                    MajorGridlineColor = OxyPlot.OxyColor.Parse("#E2E8F0"),
                    AxislineStyle = OxyPlot.LineStyle.Solid,
                    AxislineColor = OxyPlot.OxyColor.Parse("#CBD5E1"),
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

            var plotView = this.GetControl<OxyPlot.Avalonia.PlotView>("PlotEstadisticasClientes");
            plotView.IsVisible = false;
            plotView.Model = null;

            var listaControles = this.GetControl<Avalonia.Controls.ItemsControl>("ListaGraficosClientes");
            listaControles.ItemsSource = listaMiniGraficos;
        }

        private void CalcularEstadisticasClienteAnual()
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
                    TickStyle = OxyPlot.Axes.TickStyle.None,
                    AxislineStyle = OxyPlot.LineStyle.Solid,
                    AxislineColor = OxyPlot.OxyColor.Parse("#CBD5E1"),
                    Key = "EjeY"
                };

                var valueAxis = new OxyPlot.Axes.LinearAxis
                {
                    Position = OxyPlot.Axes.AxisPosition.Bottom,
                    Minimum = 0,
                    MajorStep = 1,
                    MinorStep = 1,
                    MaximumPadding = 0.2,
                    MajorGridlineStyle = OxyPlot.LineStyle.Dash,
                    MajorGridlineColor = OxyPlot.OxyColor.Parse("#E2E8F0"),
                    AxislineStyle = OxyPlot.LineStyle.Solid,
                    AxislineColor = OxyPlot.OxyColor.Parse("#CBD5E1"),
                    Key = "EjeX"
                };

                plotModel.Axes.Add(categoryAxis);
                plotModel.Axes.Add(valueAxis);

                var barSeries = new OxyPlot.Series.BarSeries
                {
                    FillColor = OxyPlot.OxyColor.Parse("#047857"),   
                    StrokeColor = OxyPlot.OxyColor.Parse("#065F46"),
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
                    .GroupBy(r => r.FechaInicial.Year)
                    .Select(g => new { Anio = g.Key.ToString(), Total = g.Count() })
                    .OrderBy(x => x.Anio)
                    .ToList();

                foreach (var dato in datosCliente)
                {
                    categoryAxis.Labels.Add(dato.Anio);
                    barSeries.Items.Add(new OxyPlot.Series.BarItem { Value = dato.Total });
                }

                plotModel.Series.Add(barSeries);

                listaMiniGraficos.Add(new MiniGraficoCliente
                {
                    Titulo = $"👤 {cliente} — Desglose Anual",
                    Modelo = plotModel
                });
            }

            var plotView = this.GetControl<OxyPlot.Avalonia.PlotView>("PlotEstadisticasClientes");
            plotView.IsVisible = false;
            plotView.Model = null;

            var listaControles = this.GetControl<Avalonia.Controls.ItemsControl>("ListaGraficosClientes");
            listaControles.ItemsSource = listaMiniGraficos;
        }

        // ====================================================================
        // CLASES AUXILIARES Y DATOS DE PRUEBA
        // ====================================================================

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
    }
}