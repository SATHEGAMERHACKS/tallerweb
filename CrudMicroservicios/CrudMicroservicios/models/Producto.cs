using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrudMicroservicios.Models
{
    public class Producto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "El estado es obligatorio.")]
        [RegularExpression("^(Activo|Inactivo)$", ErrorMessage = "El estado solo puede ser 'Activo' o 'Inactivo'.")]
        public string Estado { get; set; } = "Activo";
    }
}