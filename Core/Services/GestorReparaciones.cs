namespace Proyecto.Core.Services;

using System.Collections.Generic;
using System.Linq;
using Proyecto.Core.Models;
using Proyecto.Core.Models.Enum;
using Proyecto.Core.Persistence;

public class GestorReparaciones
{
    private readonly XmlRepository _repository;
    private readonly List<Reparacion> _reparaciones;

    public GestorReparaciones(string rutaXml = "reparaciones.xml")
    {
        _repository = new XmlRepository(rutaXml);
        _reparaciones = _repository.CargarReparaciones();
    }

    // Devuelve todas las reparaciones
    public IEnumerable<Reparacion> Todas => new List<Reparacion>(_reparaciones);

    // 1. Filtrado dinámico por cualquier valor del enum EstadoReparacion
    public IEnumerable<Reparacion> ObtenerPorEstado(EstadoReparacion estado)
    {
        return _reparaciones.Where(r => r.Estado == estado);
    }

    // 2. Diccionario agrupado automáticamente por todos los estados del enum (escalable)
    public IDictionary<EstadoReparacion, List<Reparacion>> ObtenerAgrupadasPorEstado()
    {
        return Enum.GetValues<EstadoReparacion>()
                   .ToDictionary(
                       estado => estado,
                       estado => _reparaciones.Where(r => r.Estado == estado).ToList()
                   );
    }

    public void Alta(Reparacion reparacion)
    {
        reparacion.Id = _reparaciones.Count > 0 ? _reparaciones.Max(r => r.Id) + 1 : 1;
        _reparaciones.Add(reparacion);
    }

    public bool Modificacion(Reparacion reparacionModificada)
    {
        int index = _reparaciones.FindIndex(r => r.Id == reparacionModificada.Id);
        if (index >= 0)
        {
            _reparaciones[index] = reparacionModificada;
            return true;
        }
        return false;
    }

    public bool Baja(int id)
    {
        var item = _reparaciones.FirstOrDefault(r => r.Id == id);
        if (item != null)
        {
            _reparaciones.Remove(item);
            return true;
        }
        return false;
    }

    public void Guardar()
    {
        _repository.GuardarReparaciones(_reparaciones);
    }
}