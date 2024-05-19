
using System.Reflection.PortableExecutable;

namespace ProyectoFin.Views;

public partial class vStores : ContentPage
{
	
	public vStores()
	{
		InitializeComponent();
		
	}

    private void btnnavegar_Clicked(object sender, EventArgs e)
    {
		Navigation.PushAsync(new Views.vProductos());
    }

}