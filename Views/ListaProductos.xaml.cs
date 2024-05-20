using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProyectoFin.Views;

public partial class ListaProductos : ContentPage
{
    Store localStore;
    public ObservableCollection<Product> Products { get; set; }
    public ListaProductos(Store store)
	{
        InitializeComponent();
        localStore = store;
        Products = new ObservableCollection<Product>();
        LoadData();
    }
    private async void LoadData()
    {
        System.Diagnostics.Debug.WriteLine(localStore.Nombre);
        using (var client = new HttpClient())
        {
            var response = await client.GetStringAsync("http://10.0.2.2:7000/api/producto/listar");
            System.Diagnostics.Debug.WriteLine($"Response: {response}"); // Imprime la respuesta en la consola de salida
            var products = JsonSerializer.Deserialize<List<Product>>(response);
            foreach (var product in products)
            {
                Products.Add(product);
            }
            System.Diagnostics.Debug.WriteLine($"Products: {Products}"); // Imprime los datos en la consola de salida
            listaStores.ItemsSource = Products;
        }
    }

    private void listaStores_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var objetoProduct = (Product)e.SelectedItem;
        Navigation.PushAsync(new ProductUpdate(localStore, objetoProduct));
    }

    private void btnRegistrar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ProductForm(localStore));
    }

}