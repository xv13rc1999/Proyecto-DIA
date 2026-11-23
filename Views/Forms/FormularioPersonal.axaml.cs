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
    public List<SeleccionReparacionDTO> ReparacionesAsignadas;

    public PersonalAsignadoDTO? Resultado { get; private set; } = null;

    public FormularioPersonal(ContenedorDatos datosTaller, Personal? p = null)
    {
        ReparacionesAsignadas = [.. datosTaller.ListaReparaciones.Where( r => r.FechaFin == null)
                .Select( r => new SeleccionReparacionDTO{
                        Asunto = r.Asunto,
                        Seleccionada = false
                })];

        if (p != null)
        {
            //TODO Marcar las reparaciones que tenga asignadas
            
            /*
            for (int i=0; i<ReparacionesAsignadas.Count; i++)
            {
                if (p.ReparacionesAsignadas.Contains(ReparacionesAsignadas[i].IdReparacion))
                {
                    ReparacionesAsignadas[i] =
                        new SeleccionReparacion
                        {
                            IdReparacion = ReparacionesAsignadas[i].IdReparacion,
                            Seleccionada = true
                        };
                }
            }
            */

            Dni = p.Dni;
            Nombre = p.Nombre;
            Email = p.Email;
        }
    }

    private void GuardarCambios(Window? window)
    {
        Resultado = new PersonalAsignadoDTO
        (
            this.Dni.Trim().ToUpper(),
            this.Nombre,
            this.Email,
            ReparacionesAsignadas = [.. ReparacionesAsignadas
                                        .Where( r => r.Seleccionada )
                                        .Select( r => r.IdReparacion )]
        );
        //TODO Asignar reparaciones
        window?.Close(true);
    }

    private void Cancelar(Window? window)
    {
        window?.Close(false);
    }
}