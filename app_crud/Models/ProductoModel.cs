using System.ComponentModel.DataAnnotations;

namespace app_crud.Models
{
    public class ProductoModel
    {
        [Display(Name = "Id Producto")]
        public int idProducto { get; set; }
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }
        [Display(Name = "Unid Medida")]
        public string uMedida { get; set; }
        [Display(Name = "Precio Unitario")]
        public decimal precio { get; set; }
        [Display(Name = "Stock")]
        public int stock { get; set; }

    }


}
