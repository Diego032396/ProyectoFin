using Plugin.Media;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;

namespace ProyectoFin.Views;

public partial class ProductForm : ContentPage
{
    Store producStore;
    public ProductForm(Store store)
    {
        InitializeComponent();
        producStore = store;
    }

    private async void btnTakePhoto_Clicked(object sender, EventArgs e)
    {
        try { 
        await CrossMedia.Current.Initialize();

        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
        {
            await DisplayAlert("No Camera", "No camera available.", "OK");
            return;
        }

        var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
        {
            Directory = producStore.Id + "-images",
            Name = producStore.Id + producStore.Nombre+ txtNombre + ".jpg"
        });

        if (file == null)
            return;

        lblImagen.Text = file.Path;
        }catch(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            await DisplayAlert("Error", "Error al tomar la foto", "Cerrar");
        }
    }

    private void btnRegistrarForm_Clicked(object sender, EventArgs e)
    {
        try
        {
            WebClient cliente = new WebClient();
            var parametros = new System.Collections.Specialized.NameValueCollection();
            parametros.Add("Nombre", txtNombre.Text);
            parametros.Add("Precio", txtPrecio.Text);
            parametros.Add("Imagen", lblImagen.Text);
            parametros.Add("AlmacenId", producStore.Id.ToString());
            DateTime fechaCducidad = dateFechaCducidad.Date;
            parametros.Add("FechaCducidad", fechaCducidad.ToString("yyyy-MM-dd HH:mm:ss.fff zzz"));

            var json = JsonSerializer.Serialize(parametros);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            System.Diagnostics.Debug.WriteLine(fechaCducidad.ToString("yyyy-MM-dd HH:mm:ss.fff zzz"));
            cliente.UploadValues("http://10.0.2.2:7000/api/producto/crear", "POST", parametros);
            DisplayAlert("Alert", "Producto Registrado", "Cerrar");
            Navigation.PushAsync(new ListaProductos(producStore));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            DisplayAlert("Alert", "Error en Registrar el producto", "Cerrar");
        }
    }
}