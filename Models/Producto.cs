namespace ProyectoFin.Models
{
    class Producto
    {
        public int Id { get; set; }
        public int almacenId { get; set; }
        public string nombre { get; set; }
        public DateTime fechaCducidad { get; set; }
        public string precio { get; set; }
        public string imgen { get; set; }

    }
}
