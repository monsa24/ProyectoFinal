using System.Collections.Generic;
using System.Web;
using TiendaOnline.Models;

namespace TiendaOnline.Helpers
{
    public static class SessionHelper
    {
        private const string KEY = "CARRITO";

        public static List<CarritoItem> GetCarrito()
        {
            if (HttpContext.Current.Session[KEY] == null)
                HttpContext.Current.Session[KEY] = new List<CarritoItem>();

            return (List<CarritoItem>)HttpContext.Current.Session[KEY];
        }

        public static void SetCarrito(List<CarritoItem> carrito)
        {
            HttpContext.Current.Session[KEY] = carrito;
        }

        public static void Clear()
        {
            HttpContext.Current.Session[KEY] = new List<CarritoItem>();
        }
    }
}