using DoAnCuoiKy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCuoiKy.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanTri")]
    public class QuyenController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {

            return View(db.Quyens.OrderBy(n => n.TenQuyen));
        }
        [HttpGet]
        public ActionResult ThemQuyen()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemQuyen(Quyen quyen)
        {
            if (ModelState.IsValid)
            {
                db.Quyens.Add(quyen);
                db.SaveChanges();
                ViewBag.Message = "Thêm Thành Công";
                return View();
            }

            ViewBag.Message = "Thêm Thất Bại";
            return View();
        }

        [HttpGet]
        public ActionResult SuaQuyen(string ma)
        {
            var quyen = db.Quyens.FirstOrDefault(q => q.MaQuyen == ma);
            return View(quyen);
        }

        [HttpPost]
        public ActionResult SuaQuyen(Quyen q)
        {
            if(ModelState.IsValid)
            {
                var item = db.Quyens.FirstOrDefault(qu => qu.MaQuyen == q.MaQuyen);

                item.TenQuyen = q.TenQuyen;
                db.SaveChanges();

                ViewBag.Message = "Sửa Thành Công";
                return View(item);
            }
            return View();
        }

        [HttpGet]
        public ActionResult XoaQuyen(string ma)
        {
            if(ma == null)
            {
                return HttpNotFound();
            }
            var quyen = db.Quyens.FirstOrDefault(q => q.MaQuyen == ma);
            return View(quyen);
        }
        
        [HttpPost]
        public ActionResult XoaQuyen(Quyen q)
        {
            var item = db.Quyens.FirstOrDefault(qu => qu.MaQuyen == q.MaQuyen);
            if (ModelState.IsValid)
            {
                db.Quyens.Remove(item);
                db.SaveChanges();
                Session["deleteQuyen"] = "Xóa Thành Công";
                return RedirectToAction("Index");
            }
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}