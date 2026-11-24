using System.Collections.Generic;
using System.Linq;
using Taller.Utils;

namespace Taller.Core;

public class ContenedorDatos{

    private List<Personal> _listaPersonal;
    private List<Reparacion> _listaReparaciones;
    private List<Cliente> _listaClientes;

    public IReadOnlyList<Personal> ListaPersonal => _listaPersonal;
    public IReadOnlyList<Reparacion> ListaReparaciones => _listaReparaciones;
    public IReadOnlyList<Cliente> ListaClientes => _listaClientes;

    public ContenedorDatos()
    {
        this._listaPersonal = [];
        this._listaReparaciones = [];
        this._listaClientes = [];
    }
    
    public ContenedorDatos(List<Personal> personal, List<Reparacion> reparaciones, List<Cliente> clientes)
    {
        this._listaPersonal = personal;
        this._listaReparaciones = reparaciones;
        this._listaClientes = clientes;
    }

    public CodigoValidacion AñadirPersonal(string dni, string nombre, string email, List<string>? reparacionesAsignadas)
    {
        CodigoValidacion temp = ValidadorComun.ComprobarDni(dni);
        if(!temp.Equals(CodigoValidacion.Ok)){
            return temp;
        }

        IQueryable<Personal> query = _listaPersonal.AsQueryable();
        if(query.Where( p => p.Dni.Equals(dni)).ToList().Count > 0){
            return CodigoValidacion.DniDuplicated;
        }

        temp = ValidadorComun.ComprobarNombre(nombre);
        if(!temp.Equals(CodigoValidacion.Ok)){
            return temp;
        }

        temp = ValidadorComun.ComprobarEmail(email);
        if(!temp.Equals(CodigoValidacion.Ok)){
            return temp;
        }

        if(query.Where( p => p.Email.Equals(email)).ToList().Count > 0){
            return CodigoValidacion.EmailDuplicated;
        }

        Personal nuevoPersonal = new(dni, nombre.Trim(), email);
        _listaPersonal.Add(nuevoPersonal);

        if (reparacionesAsignadas is not null)
        {
            List<Reparacion> listaReparacionesAModificar =
                        [.. _listaReparaciones.Where( r => reparacionesAsignadas.Contains(r.Asunto))];

            foreach (Reparacion anteriorReparacion in listaReparacionesAModificar)
            {
                _listaReparaciones.Remove(anteriorReparacion);
                _listaReparaciones.Add( new Reparacion(
                    nuevoPersonal,
                    anteriorReparacion.Cliente,
                    anteriorReparacion.Asunto,
                    anteriorReparacion.FechaInicio,
                    anteriorReparacion.FechaFin,
                    anteriorReparacion.Notas
                ));
            }
        }
        return CodigoValidacion.Ok;
    }

    public CodigoValidacion EliminarPersonal(string dni)
    {
        List<Personal> queryList = [.. _listaPersonal.AsQueryable().Where( p => p.Dni.Equals(dni) )];

        if (queryList.Count == 0) {return CodigoValidacion.PersonalNotFound;}
        

        List<Reparacion> reparacionesAsignadas = [ .._listaReparaciones.AsQueryable()
                                                        .Where( r => r.Empleado != null && r.Empleado.Equals(dni) )];
        foreach (Reparacion r in reparacionesAsignadas)
        {
            _listaReparaciones.Remove(r);
            _listaReparaciones.Add(new Reparacion(
                null,
                r.Cliente,
                r.Asunto,
                r.FechaInicio,
                r.FechaFin,
                r.Notas
            ));
        }

        if (_listaPersonal.RemoveAll( p => p.Dni.Equals(dni) ) > 0) {return CodigoValidacion.Ok;}
        else {return CodigoValidacion.PersonalDeleteUnexpected;}
    }

    public CodigoValidacion ActualizarPersonal(string dni, string nombre, string email, List<string>? reparacionesAsignadas)
    {
        IQueryable<Personal> query = _listaPersonal.AsQueryable();

        //Comprobar que existe Personal y es único
        List<Personal> queryList = [.. query.Where( p => p.Dni.Equals(dni))];
        if (queryList.Count == 0) {return CodigoValidacion.PersonalNotFound;}
        if (queryList.Count > 1) {return CodigoValidacion.PersonalDuplicated;}

        //Comprobar que al menos 1 atributo es distinto a los originales
        Personal datosActuales = queryList[0];
        if (datosActuales.Nombre.Equals(nombre)
            && datosActuales.Email.Equals(email))
        {
            return CodigoValidacion.PersonalModificationSameValues;
        }

        CodigoValidacion temp = ValidadorComun.ComprobarNombre(nombre);
        if (!temp.Equals(CodigoValidacion.Ok)){
            return temp;
        }

        //Si Email nuevo == Email actual => OK
        // Si Email nuevo == Email de OTRO usuario => MAL, EmailDuplicated
        if (!datosActuales.Email.Equals(email))
        {
            if(queryList.Where( p => p.Email.Equals(email)).ToList().Count != 0)
            {
                return CodigoValidacion.EmailDuplicated;
            }

            temp = ValidadorComun.ComprobarEmail(email);
            if(!temp.Equals(CodigoValidacion.Ok)) return temp;
        }

        _listaPersonal.Remove(datosActuales);
        _listaPersonal.Add(new Personal(dni, nombre, email));
        return CodigoValidacion.Ok;
    }
}