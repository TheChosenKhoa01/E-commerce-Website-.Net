using DoAnCuoiKy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCuoiKy.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanTri,NhanVien")]
    public class QuanLyKhachHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
  
        public ActionResult Index()
        {
            var lstKhachHang = db.KhachHangs.ToList();
            return View(lstKhachHang);
        }
    }
}