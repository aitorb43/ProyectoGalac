using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using ProyectoGalac.Entidades;
using ProyectoGalac.Interfaces;
using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace ProyectoGalac.Servicios
{
    public class ServicioProducto : IServicioProducto
    {
        private AplicationDBContext aplicationDBContext;
        public ServicioProducto(AplicationDBContext AplicationDBContext) {

            this.aplicationDBContext = AplicationDBContext;
        }

        public Boolean registrarProducto(Producto producto)
        {
            Boolean respuesta;
            Double i;

            try
            {
                if (Double.TryParse(producto.Precio, out i))
                    producto.Precio = Math.Round(i, 2).ToString();

                aplicationDBContext.Productos.Add(producto);

                var log = ArmandoLog(1, producto, String.Empty, String.Empty);

                aplicationDBContext.Logs.Add(log);
                aplicationDBContext.SaveChanges();

                respuesta = true;
            }
            catch (Exception)
            {
                respuesta = false;
            }

            return respuesta;        
        }

        public Boolean ConsultaDescripcion(Producto producto, int estatus)
        {

            Boolean respuesta = true ;

            if (estatus == 1)
                respuesta = null == aplicationDBContext.Productos.Where(x => x.Descripcion == producto.Descripcion).FirstOrDefault() ? false : true;


            if (estatus == 2)
                respuesta = aplicationDBContext.Productos.Where(x => x.Descripcion == producto.Descripcion && x.Id != producto.Id).FirstOrDefault() == null ? false : true;

            return respuesta;
        }

        public List<Producto> ListaProductos() 
        {
            return aplicationDBContext.Productos.ToList(); 
        }

        public Producto ObtenerProducto(int Id)
        {
            return aplicationDBContext.Productos.Where(x => x.Id == Id).FirstOrDefault();
        }

        public Boolean ActualizarProducto(Producto producto) 
        {
            Double i;

            if (aplicationDBContext.Productos.Any(x => x.Id == producto.Id)) 
            {
                if (Double.TryParse(producto.Precio, out i))
                    producto.Precio = Math.Round(i, 2).ToString();

                aplicationDBContext.Entry(producto).State = EntityState.Modified;

                var log = ArmandoLog(2, producto, String.Empty, String.Empty);
                aplicationDBContext.Logs.Add(log);
                aplicationDBContext.SaveChanges();

                return true;
            }
            else
            return false;
        }

        public String validacionesProducto(int estatus, Producto producto)      
        {
            String result = String.Empty;
            Double i;
            
            if(estatus == 1)
            if (!producto.Id.Equals(0))
                result = "El Id debe ser 0";

            // validando si el precio es entero 
            if (!Double.TryParse(producto.Precio, out i))
                result = "El precio del producto no es un valor numerico";

            //validando si el la existencia es entero 
            if (!Double.TryParse(producto.Existencia, out i))
                result = "La existencia del producto no es un valor numerico";

            // vlaida que el producto no este vacio 
            if (String.IsNullOrEmpty(producto.Descripcion.Trim()))
                result = "La descripcion del producto no puede estar vacio";

            // valida que no re repitan productos con la misma descripcion 
                if (ConsultaDescripcion(producto, estatus))
                    result = "Ya existe un producto con esa descripcion";

            return result;
        }

        public Boolean EliminarProducto(int Id)
        {
            Boolean rpta;
            try
            {
                Producto producto = aplicationDBContext.Productos.Find(Id);
                aplicationDBContext.Remove(producto);

               var log = ArmandoLog(3, producto, String.Empty, String.Empty);
                aplicationDBContext.Logs.Add(log);
                aplicationDBContext.SaveChanges();

                rpta = true;
            }
            catch (Exception e)
            {
                string error = e.Message;
                rpta = false;
            }
            return rpta;
        }

        public Boolean ActualizarExistencia(ActualizarExistencia data)
        {
            Boolean rpta;
            try
            {
             
                Producto producto = aplicationDBContext.Productos.Find(data.Id);

                int cantStock = Convert.ToInt32(producto.Existencia);

                String accion = data.Descontar == true ? "-":"+";

                producto.Existencia = data.Descontar == true ? (cantStock - data.Cantidad).ToString() : (cantStock + data.Cantidad).ToString();

                aplicationDBContext.Entry(producto).State = EntityState.Modified;

                var log = ArmandoLog(4, producto, accion, data.Cantidad.ToString());

                aplicationDBContext.Logs.Add(log);
                aplicationDBContext.SaveChanges();

                rpta = true;
            }
            catch (Exception e)
            {
                string error = e.Message;
                rpta = false;
            }
            return rpta;
        }

        public Boolean AjustePrecioMasivo(Boolean incremento, Double porcentaje)
        {
            Boolean rpta;
            try
            {
                var listaProducto = ListaProductos();

                if (listaProducto.Count() > 0)
                {
                    foreach (var producto in listaProducto)
                    {
                        Double precio = Convert.ToDouble(producto.Precio);
                        var accion = incremento == true ? "aumento" : "descuento";

                        if (incremento == true)
                            producto.Precio = Math.Round((precio * (100 + porcentaje)) / 100, 2).ToString();
                        else
                            producto.Precio = Math.Round(precio - ((porcentaje * precio) / 100), 2).ToString();

                        aplicationDBContext.Entry(producto).State = EntityState.Modified;

                        var log = ArmandoLog(5, producto, accion, porcentaje.ToString());

                        aplicationDBContext.Logs.Add(log);
                        aplicationDBContext.SaveChanges();
                    }

                    rpta = true;
                }
                else
                    rpta = false;
            }
            catch (Exception e)
            {
                string error = e.Message;
                rpta = false;
            }

            return rpta;
        }

        private Log ArmandoLog(int operacion, Producto producto, String accion, string Data)
        {

            Log log = new Log();
            String tituloAccion = String.Empty;
            String TituloDescripcion = String.Empty;

            if(operacion == 1)
            {
                tituloAccion = "Registro";
                TituloDescripcion = "Se registro";
            }

            if (operacion == 2)
            {
                tituloAccion = "Modificacion";
                TituloDescripcion = "Se modifico";
            }
            if (operacion == 3)
            {
                tituloAccion = "Eliminacion";
                TituloDescripcion = "Se elimino";
            }
            if (operacion == 4)
            {
                tituloAccion = "Actualizacion de existencia";
                TituloDescripcion = "Se modifico el precio";
                log.Descripcion = "Se modifico la existencia del producto: " +
                    producto.Descripcion + ", " + accion + " " + Data +
                    ", existencia: " + producto.Existencia;

            }
            if (operacion == 5)
            {
                tituloAccion = "Ajuste de precio";
                TituloDescripcion = "Se modifico la existencia";
                log.Descripcion = "Se modifico el precio del producto: " +
                    producto.Descripcion +
                    " un " + accion + " de " + Data + " % " +
                    ", precio : " + producto.Precio;

            }
            log.Accion = tituloAccion +" del producto";
            log.Fecha = DateTime.Now;

            if (operacion == 1 || operacion==2 || operacion ==3) {
                log.Descripcion = TituloDescripcion +" el producto: " +
                    producto.Descripcion +
                    ", precio : " + producto.Precio +
                    ", existencia: " + producto.Existencia;
            }

            return log;

        }

        public IndexViewModel ViewListaProducto(int pagina)
        {
            var cantidadRegistrosPorPagina = 5; // parámetro

            var productos = aplicationDBContext.Productos.OrderBy(x => x.Id)
                .Skip((pagina - 1) * cantidadRegistrosPorPagina)
                .Take(cantidadRegistrosPorPagina).ToList();
            var totalDeRegistros = aplicationDBContext.Productos.Count();

            var modelo = new IndexViewModel();
            modelo.productos = productos;
            modelo.PaginaActual = pagina;
            modelo.TotalDeRegistros = totalDeRegistros;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;

            return modelo;
        }

        public string ValidacionActualizacionExistencia(ActualizarExistencia data)
        {
            Producto producto = aplicationDBContext.Productos.Find(data.Id);
            if (producto == null)
                return " No existe el producto a modificar";

            int cantStock = Convert.ToInt32(producto.Existencia);

            if (cantStock < data.Cantidad && data.Descontar == true)
                return " la existencia del producto es menor que la cantidad a descontar";

            return String.Empty;
        }
    }
}
