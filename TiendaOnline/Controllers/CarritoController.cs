using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TiendaOnline.Helpers;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class CarritoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // 🔹 Ver carrito
        public ActionResult Index()
        {
            var carrito = SessionHelper.GetCarrito();
            return View(carrito);
        }

        // 🔹 Agregar producto
        public ActionResult Agregar(int id)
        {
            var producto = db.Productos.Find(id);
            if (producto == null) return HttpNotFound();

            var carrito = SessionHelper.GetCarrito();
            var item = carrito.FirstOrDefault(x => x.ProductoId == id);

            if (item != null)
            {
                item.Cantidad++;
            }
            else
            {
                carrito.Add(new CarritoItem
                {
                    ProductoId = producto.Id,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Cantidad = 1,
                    Imagen = producto.Imagen
                });
            }

            SessionHelper.SetCarrito(carrito);
            return RedirectToAction("Index");
        }

        // 🔹 Eliminar item
        public ActionResult Eliminar(int id)
        {
            var carrito = SessionHelper.GetCarrito();
            var item = carrito.FirstOrDefault(x => x.ProductoId == id);

            if (item != null)
                carrito.Remove(item);

            SessionHelper.SetCarrito(carrito);
            return RedirectToAction("Index");
        }

        // 🔹 Vaciar carrito
        public ActionResult Vaciar()
        {
            SessionHelper.Clear();
            return RedirectToAction("Index");
        }

        // 🔹 Confirmar compra
        [Authorize(Roles = "Usuario")]
        public ActionResult Confirmar()
        {
            var carrito = SessionHelper.GetCarrito();
            var userId = User.Identity.GetUserId();

            foreach (var item in carrito)
            {
                var producto = db.Productos.Find(item.ProductoId);

                if (producto != null && producto.Disponibilidad >= item.Cantidad)
                {
                    producto.Disponibilidad -= item.Cantidad;

                    var compra = new CompraModels
                    {
                        ProductoId = producto.Id,
                        UsuarioId = userId,
                        Precio = item.Precio,
                        Cantidad = item.Cantidad,
                        FechaCompra = DateTime.Now
                    };

                    db.Compras.Add(compra);
                }
            }

            db.SaveChanges();
            SessionHelper.Clear();

            TempData["Success"] = "Compra realizada correctamente";
            return RedirectToAction("Index", "Productos");
        }
    }
}