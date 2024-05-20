using System.Net;
using System.Text;
using System.Text.Json;

namespace ProyectoFin.Views;

public partial class UsuarioForm : ContentPage
{
	public UsuarioForm()
	{
		InitializeComponent();
	}

    private void btnRegistrarForm_Clicked(object sender, EventArgs e)
    {
        try
        {
            WebClient cliente = new WebClient();
            var parametros = new System.Collections.Specialized.NameValueCollection();
            parametros.Add("NombreUsuario", txtUser.Text);
            parametros.Add("Correo", txtEmail.Text);
            parametros.Add("Contrasena", txtPassword.Text);

            var json = JsonSerializer.Serialize(parametros);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            cliente.UploadValues("http://10.0.2.2:7000/api/usuario/crear", "POST", parametros);
            DisplayAlert("Exito!", "Usuario Creado, puedes usar tus credenciales registradas para acceder al sistema", "continuar");
            Navigation.PushAsync(new vLogin());
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            DisplayAlert("Alert", "Error en Registrar nuevo usuario", "Cerrar");
        }
    }
}