using System.ComponentModel.DataAnnotations;

namespace ProyectoGalac.Entidades
{
    public class Log
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Accion { get; set; }

        [StringLength(250)]
        public string Descripcion { get; set; }        
        public DateTime Fecha { get; set; }
    }
}
