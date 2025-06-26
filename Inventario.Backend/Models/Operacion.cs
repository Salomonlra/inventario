namespace Inventario.Backend.Models
{
    public class Operacion
    {
        public int Id { get; set; }

        public required string Tipo { get; set; }

        public required string Producto { get; set; }

        public DateTime Fecha { get; set; }

        public int Cantidad { get; set; }

        public int Stock { get; set; }

    }
}
