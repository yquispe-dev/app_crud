using app_crud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.DataClassification;
using System.Data;

namespace app_crud.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IConfiguration _config;
        public ProductoController(IConfiguration config)
        {
            _config = config;
        }

        IEnumerable<ProductoModel> productos()
        {
            List<ProductoModel> listaProductos = new List<ProductoModel>();

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:conexSQL"]))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("exec usp_productos", cn);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listaProductos.Add(new ProductoModel
                    {
                        idProducto = dr.GetInt32(0),
                        descripcion = dr.GetString(1),
                        uMedida = dr.GetString(2),
                        precio = dr.GetDecimal(3),
                        stock = dr.GetInt32(4),
                    });
                }
                dr.Close();
            }
            return listaProductos;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoModel obj)
        {
            if (!ModelState.IsValid)
            {
                return View(obj);
            }
            
            string mensaje = "";
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:conexSQL"]))
            {
                await cn.OpenAsync();

               
                SqlCommand cmd = new SqlCommand("usp_insertar_producto", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@descripcion", obj.descripcion);
                cmd.Parameters.AddWithValue("@uMedida", obj.uMedida);
                cmd.Parameters.AddWithValue("@precio", obj.precio);
                cmd.Parameters.AddWithValue("@stock", obj.stock);

                int filas = await cmd.ExecuteNonQueryAsync();

                mensaje = $"Se inserto {filas} registros";
            }
            TempData["mensaje"] = mensaje;
            return RedirectToAction("index");
        }


        public async Task<ActionResult> Index()
        {
            return View(await Task.Run(() => productos()));
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}