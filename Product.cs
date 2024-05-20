namespace ProyectoFin
{
    public class Product
    {
    public int id { get; set; }
    public int AlmacenId { get; set; }
    public string Nombre { get; set; }
    public DateTime FechaCducidad { get; set; }
    public string Precio { get; set; }
    public string Imagen { get; set; }
    public string Estado { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
}
