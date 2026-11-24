using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Taller.Core;
using Taller.Utils.DTOs;

namespace Taller.Views.Forms;

public partial class FormularioPersonal : Window
{
    public string Dni = string.Empty;
    public string Nombre = string.Empty;
    public string Email = string.Empty;
    public List<SeleccionReparacionDTO> SelectorReparaciones;
    public PersonalAsignadoDTO? Resultado { get; private set; } = null;

    public FormularioPersonal(ContenedorDatos datosTaller, Personal? p = null)
    {
        SelectorReparaciones = [.. datosTaller.ListaReparaciones.Where( r => r.FechaFin == null)
                .Select( r => new SeleccionReparacionDTO{
                        Asunto = r.Asunto,
                        Seleccionada = false
                })];
        
        List<string> reparacionesAsignadas = [.. datosTaller.ListaReparaciones
                                                .Where( r => r.Empleado != null && r.Empleado.Equals( p ) )
                                                .Select( r => r.Asunto )];

        if (p != null)
        {
            for (int i=0; i<SelectorReparaciones.Count; i++)
            {
                if (reparacionesAsignadas.Contains(SelectorReparaciones[i].Asunto))
                {
                    SelectorReparaciones[i] =
                        new SeleccionReparacionDTO
                        {
                            Asunto = SelectorReparaciones[i].Asunto,
                            Seleccionada = true
                        };
                }
            }

            Dni = p.Dni;
            Nombre = p.Nombre;
            Email = p.Email;
        }
    }

    private void GuardarCambios(Window? window)
    {
        Resultado = new PersonalAsignadoDTO
        {
            Dni = this.Dni.Trim().ToUpper(),
            Nombre = this.Nombre,
            Email = this.Email,
            ReparacionesAsignadas = [.. SelectorReparaciones
                                        .Where( r => r.Seleccionada )
                                        .Select( r => r.Asunto )]
        };
        window?.Close(true);
    }

    private void Cancelar(Window? window)
    {
        window?.Close(false);
    }
}