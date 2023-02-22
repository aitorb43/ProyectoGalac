using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoGalac.Entidades;
using ProyectoGalac.Interfaces;

namespace ProyectoGalac.Controllers
{
    public class LogController : ControllerBase
    {
        private AplicationDBContext aplicationDBContext;
        private IServicioProducto servicioProducto;

        public LogController(AplicationDBContext AplicationDBContext, IServicioProducto servicioProducto)
        {
            this.aplicationDBContext = AplicationDBContext;
            this.servicioProducto = servicioProducto;
        }

        [HttpGet("[controller]/{Producto}")]
        public IActionResult Get(String Producto)
        {
            var ListaLogPorProducto = aplicationDBContext.Logs.Where(x => x.Descripcion.Contains(Producto)).OrderByDescending(x => x.Id).ToList();
            return ListaLogPorProducto.Count() > 0 ? Ok(ListaLogPorProducto) : NotFound("No hay registro en el log del producto");
        }

        [HttpGet("[controller]/ListaLog")]
        public IActionResult Get()
        {
            var ListaLog = aplicationDBContext.Logs.ToList();

            return ListaLog != null? Ok(ListaLog) :NotFound("no hay registro en el log");
        }
    }
}
