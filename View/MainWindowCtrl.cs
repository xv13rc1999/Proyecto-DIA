namespace TallerClientes.View;


using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml.Linq;

using Core;


public class MainWindowCtrl
{
    public MainWindow View { get; }
    public IEnumerable<Cliente> All => this.reg.All;
    private RegistroClientes reg;
    
    public MainWindowCtrl()
    {
        this.reg = new RegistroClientes();
        this.View = new MainWindow();
        this.View.BtNuevo.Click += (_, _) => this.OnNew();
        this.View.BtEditar.Click += (_, _) => this.OnEdit();
        this.View.BtEliminar.Click += (_, _) => this.OnDelete();
        this.View.BtGuardar.Click += (_, _) => this.OnSave();

        this.List();
    }

    public void OnNew()
    {
        var dlg = new NuevoClienteCtrl( this.View );
        
        dlg.Show( (g) => { this.reg.Add( g ); this.List(); });
    }

    public void OnEdit()
    {
        Cliente c = (Cliente) this.View.LbClientes.SelectedItem;

        if(c != null)
        {
            var dlg = new EditarClienteCtrl(this.View, c.Dni, c.Nombre, c.Email);
        
            dlg.Show( (g) => { this.reg.Remove( c ); this.reg.Add( g ); this.List(); });
        }
    }

    public void OnDelete()
    {
        Cliente c = (Cliente) this.View.LbClientes.SelectedItem;

        if(c != null)
        {        
            this.reg.Remove( c );
            this.List();
        }
    }

    public void OnSave()
    {
        XElement root = this.reg.ToXml();

        root.Save("clientes.xml");
    }

    private void List()
    {
        this.View.LbClientes.ItemsSource = this.All;
    }
}