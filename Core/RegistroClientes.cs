namespace TallerClientes.Core;


using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;


public class RegistroClientes
{
    private List<Cliente> listaClientes;
    public IEnumerable<Cliente> All => new List<Cliente>( this.listaClientes );
    private XElement root;
    
    public RegistroClientes()
    {
        if (!File.Exists("clientes.xml"))
        {
            this.listaClientes = new List<Cliente>();
        }

        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Cliente>));
            using (FileStream fs = new FileStream("clientes.xml", FileMode.Open))
            {
                this.listaClientes = (List<Cliente>)(serializer.Deserialize(fs) ?? new List<Cliente>());
            }
        }
        catch (Exception)
        {
            this.listaClientes = new List<Cliente>();
        }
    }    

    public void Add(Cliente c)
    {
        this.listaClientes.Add(c);
    }

    public void Remove(Cliente c)
    {
        this.listaClientes.Remove(c);
    }

    public XElement ToXml()
    {
        XElement toret = new XElement("Clientes");

        foreach(Cliente c in this.listaClientes)
        {
            toret.Add(c.ToXml());
        }

        return toret;
    }

    public override string ToString()
    {
        return string.Join( "\n", this.listaClientes );
    }
}
