using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCuoiKy.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanTri")]
    public class HomeAdmminController : Controller
    {
        // GET: admin/HomeAdmmin
        public ActionResult Index()
        {
            return View("Index","ThongKe");
        }
    }
}