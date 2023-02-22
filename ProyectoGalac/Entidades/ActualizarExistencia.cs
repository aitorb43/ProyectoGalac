using System.ComponentModel.DataAnnotations;

namespace ProyectoGalac.Entidades
{
    public class ActualizarExistencia
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Cantidad { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public Boolean Descontar { get; set; }

    }
}
