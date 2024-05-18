namespace ProyectoFin.Views;

public partial class vLogin : ContentPage
{
	public vLogin()
	{
		InitializeComponent();
	}

    private void btnLogin_Clicked(object sender, EventArgs e)
    {
		Navigation.PushAsync(new Views.vStores());
    }
}