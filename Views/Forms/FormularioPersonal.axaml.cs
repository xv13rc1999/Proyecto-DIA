using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Markup.Xaml;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Taller.Core;
using Taller.Utils.DTOs;
using Taller.Utils;

namespace Taller.Views.Forms;

public partial class FormularioPersonal : Window
{
    public string Dni { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<SeleccionReparacionDTO> SelectorReparaciones { get; set; }
    public PersonalAsignadoDTO? Resultado { get; private set; } = null;

    public FormularioPersonal(ContenedorDatos datosTaller, Personal? p = null)
    {
        InitializeComponent();
        
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

        this.GetControl<Button>("BtCancelar").Click += (_,_) => this.OnCancelar();
        this.GetControl<Button>("BtGuardar").Click += (_,_) => this.OnGuardar();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void OnGuardar()
    {
        bool estaCorrecto = true;
        var campoDni = this.GetControl<TextBox>("CampoDni");        
        var campoNombre = this.GetControl<TextBox>("CampoNombre");        
        var campoEmail = this.GetControl<TextBox>("CampoEmail");        


        DataValidationErrors.ClearErrors(campoDni);
        if(ValidadorComun.ComprobarDni(campoDni.Text) != CodigoValidacion.Ok){
            DataValidationErrors.SetError(campoDni, new Exception($"{campoDni.Text}"));
            estaCorrecto = false;
        }

        DataValidationErrors.ClearErrors(campoNombre);
        if(ValidadorComun.ComprobarNombre(campoNombre.Text) != CodigoValidacion.Ok){
            DataValidationErrors.SetError(campoNombre, new Exception("Revise el Nombre"));
            estaCorrecto = false;
        }

        DataValidationErrors.ClearErrors(campoEmail);
        if(ValidadorComun.ComprobarEmail(campoEmail.Text) != CodigoValidacion.Ok){
            DataValidationErrors.SetError(campoEmail, new Exception("Revise el Email"));
            estaCorrecto=false;
        }
        
        if(estaCorrecto){
            Resultado = new PersonalAsignadoDTO
            {
                Dni = campoDni.Text.Trim().ToUpper(),
                Nombre = campoNombre.Text,
                Email = campoEmail.Text,
                ReparacionesAsignadas = [.. SelectorReparaciones
                                            .Where( r => r.Seleccionada )
                                            .Select( r => r.Asunto )]
            };
            this.Close(true);
        }
    }

    private void OnCancelar()
    {
        this.Close(false);
    }
}