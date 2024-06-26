using Newtonsoft.Json;
using System.Collections.ObjectModel;
namespace ProyectoFin.Views;

public partial class vProductos : ContentPage
{
    Store localStore;
    public ObservableCollection<Product> Products { get; set; }
    public vProductos(Store store)
    {
        InitializeComponent();
        localStore = store;
        InitializeComponent();
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
            var products = JsonConvert.DeserializeObject<List<Product>>(response);
            foreach (var product in products)
            {
                Products.Add(product);
                System.Diagnostics.Debug.WriteLine($"Product: {product.Nombre}, Price: {product.Precio}"); // Imprime los datos en la consola de salida
            }
            listaProducts.ItemsSource = Products;
        }
    }

    private void listaProducts_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var objetoStore = (Store)e.SelectedItem;
        Navigation.PushAsync(new vProductos(objetoStore));
    }

    private void btnRegistrar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ProductForm(localStore));
    }
}