using System.Text;
using System.Text.Json;

namespace ProyectoFin.Views;

public partial class vLogin : ContentPage
{
    private readonly HttpClient cliente = new HttpClient();

    public vLogin()
    {
        InitializeComponent();
    }

    private async void btnLogin_Clicked(object sender, EventArgs e)
    {
        try
        {
            string user = txtUsuario.Text;
            string pass = txtContraseña.Text;

            HttpClient cliente = new HttpClient();
            var parametros = new { Correo = user, Contrasena = pass };
            var json = JsonSerializer.Serialize(parametros);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await cliente.PostAsync("http://10.0.2.2:7000/api/usuario/autentica", data);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<Response>(responseBody);

                if (responseObject.satisfatorio)
                {
                    await DisplayAlert("Bienvenido", responseObject.mensaje, "OK");
                    await Navigation.PushAsync(new Views.vStores());
                }
                else
                {
                    await DisplayAlert("Error", "La validación falló. Por favor, inténtalo de nuevo.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Error en la respuesta del servidor", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Ocurrió un error durante la conexión: " + ex.Message, "OK");
        }
    }

    public class Response
    {
        public string mensaje { get; set; }
        public bool satisfatorio { get; set; }
        public object datos { get; set; }
    }

    private void nuevoRegistroBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.UsuarioForm());
    }
}