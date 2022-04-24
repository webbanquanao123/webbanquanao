using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using WedBanQuanAo.Models;
namespace WedBanQuanAo.Controllers
{
    public class SanPhamController : Controller
    {
        //
        // GET: /SanPham/

        ShopQuanAoDataContext db = new ShopQuanAoDataContext();
        public ActionResult TrangChu()
        {
            return View();
        }

    }
}
