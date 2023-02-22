using Microsoft.AspNetCore.Mvc;
using ProyectoGalac.Entidades;
using ProyectoGalac.Interfaces;

namespace ProyectoGalac.Controllers
{
    public class HomeController : Controller
    {
        private AplicationDBContext aplicationDBContext;
        private IServicioProducto servicioProducto;

        public HomeController(AplicationDBContext AplicationDBContext, IServicioProducto servicioProducto)
        {
            this.aplicationDBContext = AplicationDBContext;
            this.servicioProducto = servicioProducto;
        }

        
        [HttpGet("Home/Index")]
        public ActionResult Index(int pagina = 1)
        {          
            return View(servicioProducto.ViewListaProducto(pagina));
        }
        
    }
}
