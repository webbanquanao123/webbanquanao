using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo.Models;
using PagedList;
using PagedList.Mvc;

namespace WebBanQuanAo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        ShopQuanAoDataContext db = new ShopQuanAoDataContext();
        public ActionResult TrangChu()
        {
            return View();
        }

    }
}
