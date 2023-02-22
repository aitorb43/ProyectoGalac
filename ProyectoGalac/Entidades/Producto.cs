using DocumentFormat.OpenXml.Spreadsheet;
using System.ComponentModel.DataAnnotations;

namespace ProyectoGalac.Entidades
{
    public class Producto
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(200)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(20)]
        public String Precio { get ; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(10)]
        public String Existencia { get; set; }
    }
}
