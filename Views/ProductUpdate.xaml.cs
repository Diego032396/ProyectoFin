using Plugin.Media;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ProyectoFin.Views;

public partial class ProductUpdate : ContentPage
{
    Store localStore;
    Product localProduct;
    public ProductUpdate(Store store, Product product)
	{
		InitializeComponent();
        localStore = store;
        localProduct = product;
        initializer();
    }

    private void initializer()
    {
        txtNombre.Text = localProduct.Nombre;
        txtPrecio.Text = localProduct.Precio.ToString();
        lblImagen.Text = localProduct.Imagen;
        dateFechaCducidad.Date = localProduct.FechaCducidad;
    }

    private void btnEliminar_Clicked(object sender, EventArgs e)
    {
        try
        {
            WebClient cliente = new WebClient();
            var parametros = new System.Collections.Specialized.NameValueCollection();
            String url = "http://10.0.2.2:7000/api/producto/" + localProduct.id.ToString();
            byte[] responseBytes = cliente.UploadValues(url, "DELETE", parametros);
            string response = Encoding.UTF8.GetString(responseBytes);
            DisplayAlert("Alert", "Producto Eliminado", "Cerrar");
            Navigation.PushAsync(new ListaProductos(localStore));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            DisplayAlert("Alert", "Error en Eliminar el producto", "Cerrar");
        }
    }

    private async void btnTakePhoto_Clicked(object sender, EventArgs e)
    {
        try
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = localStore.Id + "-images",
                Name = localProduct.id.ToString() + localProduct.Nombre +".jpg"
            });

            if (file == null)
                return;

            lblImagen.Text = file.Path;
        }
        catch (Exception ex)
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
            parametros.Add("AlmacenId", localStore.Id.ToString());
            DateTime fechaCducidad = dateFechaCducidad.Date;
            parametros.Add("FechaCducidad", fechaCducidad.ToString("yyyy-MM-dd HH:mm:ss.fff zzz"));

            var json = JsonSerializer.Serialize(parametros);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            System.Diagnostics.Debug.WriteLine(fechaCducidad.ToString("yyyy-MM-dd HH:mm:ss.fff zzz"));
            byte[] responseBytes = cliente.UploadValues("http://10.0.2.2:7000/api/producto/"+localProduct.id.ToString(), "PUT", parametros);
            string response = Encoding.UTF8.GetString(responseBytes);
            DisplayAlert("Alert", "Producto Actualizado", "Cerrar");
            Navigation.PushAsync(new ListaProductos(localStore));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            DisplayAlert("Alert", "Error en Actualizar el producto", "Cerrar");
        }
    }
}