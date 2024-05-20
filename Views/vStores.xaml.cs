
using Newtonsoft.Json;
using System.Collections.ObjectModel;
namespace ProyectoFin.Views;

public partial class vStores : ContentPage
{
    public ObservableCollection<Store> Stores { get; set; }
    public vStores()
    {
        InitializeComponent();
        Stores = new ObservableCollection<Store>();
        LoadData();

    }
    private async void LoadData()
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetStringAsync("http://10.0.2.2:7000/api/almacen/listar");
            System.Diagnostics.Debug.WriteLine($"Response: {response}"); // Imprime la respuesta en la consola de salida
            var storeResponse = JsonConvert.DeserializeObject<StoreResponse>(response);
            foreach (var store in storeResponse.Datos)
            {
                Stores.Add(store);
                System.Diagnostics.Debug.WriteLine($"Store: {store.Nombre}, Address: {store.Direccion}"); // Imprime los datos en la consola de salida
            }
            listaStores.ItemsSource = Stores;
        }
    }


    private async void listaStores_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        loadingIndicator.IsRunning = true;
        loadingIndicator.IsVisible = true;
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Permiso denegado", "No se puede continuar sin permiso para usar la geolocalización.", "OK");
                return;
            }
        }
        var store = (Store)e.SelectedItem;

        try
        {
            double latitud = Convert.ToDouble(store.Latitud);
            double longitud = Convert.ToDouble(store.Longitud);

            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
            {
                Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}");
                Console.WriteLine($"Store Latitude: {latitud}, Store Longitude: {longitud}");
                double range = 0.001; // Define your range

                if (Math.Abs(latitud - location.Latitude) <= range && Math.Abs(longitud - location.Longitude) <= range)
                {
                    loadingIndicator.IsRunning = false;
                    loadingIndicator.IsVisible = false;
                    await DisplayAlert("Exito!", "Validación de la ubicación exitosa", "OK");
                    Navigation.PushAsync(new ListaProductos(store));
                }
                else
                {
                    loadingIndicator.IsRunning = false;
                    loadingIndicator.IsVisible = false;
                    await DisplayAlert("Error", "La ubicación la tienda no coincide con la ubicación actual del dispositivo.", "OK");
                }
            }
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
            // Handle not supported on device exception
            await DisplayAlert("Error", "La geolocalización no está soportada en este dispositivo.", "OK");
        }
        catch (FeatureNotEnabledException fneEx)
        {
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
            // Handle not enabled on device exception
            await DisplayAlert("Error", "La geolocalización no está habilitada en este dispositivo.", "OK");
        }
        catch (PermissionException pEx)
        {
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
            // Handle permission exception
            await DisplayAlert("Error", "No se ha concedido permiso para usar la geolocalización.", "OK");
        }
        catch (Exception ex)
        {
            loadingIndicator.IsRunning = false;
            loadingIndicator.IsVisible = false;
            // Unable to get location
            await DisplayAlert("Error", "No se pudo obtener la ubicación.", "OK");
        }
   
    }
}