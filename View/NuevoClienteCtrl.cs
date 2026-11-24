namespace Taller.View;


using System;
using AvUi = Avalonia.Controls;

using Core;
using System.Diagnostics;

public class NuevoClienteCtrl
{
    public NuevoCliente View { get; }
    private AvUi.Window parent;
    private Action<Cliente> actWhenOk;
    
    public NuevoClienteCtrl(AvUi.Window parent)
    {
        this.parent = parent;
        this.actWhenOk = (g) => {};
        this.View = new NuevoCliente();
        this.View.BtOk.Click += (_, _) => this.OnOk();
        this.View.BtCancelar.Click += (_, _) => this.OnCancel();
    }

    public NuevoClienteCtrl(AvUi.Window parent, string dni, string nom, string ema)
    {
        this.parent = parent;
        this.actWhenOk = (g) => {};
        this.View = new NuevoCliente();
        this.View.BtOk.Click += (_, _) => this.OnOk();
        this.View.BtCancelar.Click += (_, _) => this.OnCancel();

        this.View.EdDni.Text = dni;
        this.View.EdNombre.Text = nom;
        this.View.EdEmail.Text = ema;
    }
    
    public void Show(Action<Cliente> act)
    {
        this.actWhenOk = act;
        this.View.ShowDialog( parent );
    }
    
    private void OnOk()
    {
        var cliente = new Cliente(this.View.EdDni.Text ?? "", this.View.EdNombre.Text ?? "", this.View.EdEmail.Text ?? "");
        
        this.actWhenOk( cliente );
        this.View.Close();
    }
    
    private void OnCancel()
    {
        this.View.Close();
    }
}
