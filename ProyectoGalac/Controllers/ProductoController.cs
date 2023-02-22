using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProyectoGalac.Entidades;
using ProyectoGalac.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Reflection.Metadata.Ecma335;

namespace ProyectoGalac.Controllers
{
    
    public class ProductoController : Controller
    {
        private AplicationDBContext aplicationDBContext;
        private IServicioProducto servicioProducto;

        public ProductoController(AplicationDBContext AplicationDBContext, IServicioProducto servicioProducto)
        {
            this.aplicationDBContext = AplicationDBContext;
            this.servicioProducto = servicioProducto;
        }


        [HttpGet("[controller]/ListaProductos", Name = "GetListaProducto")]
        public IActionResult Get()
        {
            var ListaProducto = servicioProducto.ListaProductos();
            return ListaProducto.Count() > 0 ? Ok(ListaProducto) : NotFound("No hay productos registrados") ;
        }
  

        [HttpGet("[controller]/{Id}", Name = "ConsultaProducto")]
        public IActionResult Get(int Id)
        {
            var producto = servicioProducto.ObtenerProducto(Id);
            return producto== null? NotFound("No existe el producto"): Ok(producto);
        }
        

        [HttpPost("[controller]/registrar",Name = "InsetarProducto")]
        public IActionResult Registrar([FromBody] Producto producto)
        {
            if (producto == null)
                return BadRequest("Error en parametros de entrada");
            
            //validaciones
            String result = servicioProducto.validacionesProducto(1,producto);

            if (String.IsNullOrEmpty(result))
                return servicioProducto.registrarProducto(producto) ? Ok("Se registro con exito el producto") : NotFound("No se pudo registrar el producto");
            else
                return BadRequest(result);

        }


        [HttpPut("[controller]/Actualizar")]
        public IActionResult Actualizar([FromBody] Producto producto)
        {
            if (producto == null)
                return NotFound("parametros de entrada invalidos");

            String result = servicioProducto.validacionesProducto(2, producto);

            if (!String.IsNullOrEmpty(result))
                return BadRequest(result);

            return servicioProducto.ActualizarProducto(producto) == true ? Ok("Se Actualizo el producto exitosamente"): NotFound("El producto que desea actualizar no existe");

        }

        [HttpPut("[controller]/ActualizarExistencia")]
        public IActionResult ActualizarExistencia([FromBody] ActualizarExistencia data)
        {
            if (data == null)
                return BadRequest("Parametros de entrada invalidos");

           String resp = servicioProducto.ValidacionActualizacionExistencia(data);

           if (!String.IsNullOrEmpty(resp))
                return BadRequest(resp);

            return servicioProducto.ActualizarExistencia(data) == true ? Ok("Se actualizo la existencia del producto exitosamente") : NotFound( "No se encontro el producto a actualizar");
        }

        [HttpPut("[controller]/AjustePrecioMasivo")]
        public IActionResult AjustePrecioMasivo(Boolean incremento, Double porcentaje)
        {
            if (porcentaje <= 0)
                return BadRequest("porcentaje tiene que ser mayor a 0");

            return servicioProducto.AjustePrecioMasivo(incremento, porcentaje)== true? Ok("Se realizo el ajuste de precio masivo con exito"):NotFound("No se pudo realizar el ajusto de precio masivo");
        }

        [HttpDelete("[controller]/Eliminar/{Id}", Name = "EliminarProducto")]
        public IActionResult Eliminar(int Id)
        {
            return servicioProducto.EliminarProducto(Id)==true? Ok("Se elimino el producto exitosamente") :NotFound("No se encontro el producto que desea eliminar");
        }
    }
}
