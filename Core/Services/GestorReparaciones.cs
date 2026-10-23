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

    public IEnumerable<Reparacion> Todas => new List<Reparacion>(_reparaciones);

    public IEnumerable<Reparacion> Pendientes => _reparaciones.Where(r => r.Estado == EstadoReparacion.Pendiente);

    public IEnumerable<Reparacion> Terminadas => _reparaciones.Where(r => r.Estado == EstadoReparacion.Terminada);

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