using ProyectoGalac.Entidades;

namespace ProyectoGalac.Interfaces
{
    public interface IServicioProducto
    {
        public Boolean registrarProducto(Producto producto);
        public Boolean ConsultaDescripcion(Producto producto, int estatus);
        public List<Producto> ListaProductos();
        public Producto ObtenerProducto(int id);
        public Boolean ActualizarProducto(Producto producto);
        public String validacionesProducto(int estatus, Producto producto);
        public Boolean EliminarProducto(int Id);
        public Boolean ActualizarExistencia(ActualizarExistencia data);
        public Boolean AjustePrecioMasivo(Boolean incremento, Double porcentaje);
        public IndexViewModel ViewListaProducto(int pagina);
        public string ValidacionActualizacionExistencia(ActualizarExistencia data);
    }
}
