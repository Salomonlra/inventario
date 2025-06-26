namespace Inventario.Frontend.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public int Cantidad { get; set; }
        public required string Unidad { get; set; }
        public required string Descripcion { get; set; }
        public required string Dependencia { get; set; }
        public int Stock { get; set; }
    }
}
