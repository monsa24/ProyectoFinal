using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class ProductosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // 🔹 TODOS pueden ver el catálogo
        public ActionResult Index()
        {
            var productos = db.Productos.ToList();
            return View(productos);
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var producto = db.Productos.Find(id);
            if (producto == null) return HttpNotFound();

            return View(producto);
        }

        // ============================
        // 🔐 ADMIN: CREAR (GET)
        // ============================
        [Authorize(Roles = "Admin")]
        public ActionResult Crear()
        {
            return View();
        }

        // ============================
        // 🔐 ADMIN: CREAR (POST)
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Crear(ProductoModels producto, HttpPostedFileBase ImagenFile)
        {
            if (ModelState.IsValid)
            {
                if (ImagenFile != null && ImagenFile.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(ImagenFile.InputStream))
                    {
                        producto.Imagen = reader.ReadBytes(ImagenFile.ContentLength);
                    }
                }

                db.Productos.Add(producto);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(producto);
        }

        // ============================
        // 🔐 ADMIN: EDITAR
        // ============================
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var producto = db.Productos.Find(id);
            if (producto == null) return HttpNotFound();

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(ProductoModels producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producto);
        }

        // ============================
        // 🔐 ADMIN: ELIMINAR
        // ============================
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var producto = db.Productos.Find(id);
            if (producto == null) return HttpNotFound();

            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            var producto = db.Productos.Find(id);
            db.Productos.Remove(producto);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // ============================
        // 🛒 USUARIO: COMPRAR
        // ============================
        [Authorize(Roles = "Usuario")]
        public ActionResult Comprar(int id)
        {
            var producto = db.Productos.Find(id);

            if (producto == null)
                return HttpNotFound();

            if (producto.Disponibilidad <= 0)
            {
                TempData["Error"] = "Sin stock";
                return RedirectToAction("Index");
            }

            var compra = new CompraModels
            {
                ProductoId = producto.Id,

                // Enlace el user con la compra
                UsuarioId = User.Identity.GetUserId(),

                Precio = producto.Precio,
                Cantidad = 1,
                FechaCompra = DateTime.Now
            };

            producto.Disponibilidad -= 1;

            db.Compras.Add(compra);
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        // ============================
        // 🛒 USUARIO: MIS COMPRAS
        // ============================

        [Authorize(Roles = "Usuario")]
        public ActionResult MisCompras()
        {
            var userId = User.Identity.GetUserId();

            var compras = db.Compras
                            .Include("Producto")
                            .Where(c => c.UsuarioId == userId)
                            .ToList();

            return View(compras);
        }
        // ============================
        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}