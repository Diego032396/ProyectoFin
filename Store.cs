namespace ProyectoFin
{
    public class Store
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Estado { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class StoreResponse
    {
        public string Mensaje { get; set; }
        public bool Satisfactorio { get; set; }
        public List<Store> Datos { get; set; }
    }
}
