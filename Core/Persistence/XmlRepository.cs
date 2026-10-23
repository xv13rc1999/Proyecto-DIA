namespace Proyecto.Core.Persistence;

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Proyecto.Core.Models;

public class XmlRepository
{
    private readonly string _rutaArchivo;

    public XmlRepository(string rutaArchivo = "reparaciones.xml")
    {
        _rutaArchivo = rutaArchivo;
    }

    public List<Reparacion> CargarReparaciones()
    {
        if (!File.Exists(_rutaArchivo))
        {
            return new List<Reparacion>();
        }

        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Reparacion>));
            using (FileStream fs = new FileStream(_rutaArchivo, FileMode.Open))
            {
                return (List<Reparacion>)(serializer.Deserialize(fs) ?? new List<Reparacion>());
            }
        }
        catch (Exception)
        {
            return new List<Reparacion>();
        }
    }

    public void GuardarReparaciones(List<Reparacion> reparaciones)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<Reparacion>));
        using (StreamWriter writer = new StreamWriter(_rutaArchivo))
        {
            serializer.Serialize(writer, reparaciones);
        }
    }
}