using DoAnCuoiKy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCuoiKy.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanTri,NhaCungCap")]
    public class NhaCungCapController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
      
        public ActionResult Index()
        {
            var lstNhaCungCap = db.NhaCungCaps;
            return View(lstNhaCungCap);
        }

        [HttpGet]
        public ActionResult ThemNhaCungCap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ThemNhaCungCap(NhaCungCap ncc)
        {
            if(!ModelState.IsValid)
            {
                return View(ncc);
            }

            db.NhaCungCaps.Add(ncc);
            db.SaveChanges();
            ViewBag.Message = "Thêm Thành Công";
            return View();
        }

        [HttpGet]
        public ActionResult SuaNhaCungCap(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            var item = db.NhaCungCaps.FirstOrDefault(ncc => ncc.MaNCC == id);
            return View(item);
        }

        [HttpPost]
        public ActionResult SuaNhaCungCap(NhaCungCap ncc)
        {
            if(!ModelState.IsValid)
            {
                return View(ncc);
            }
            
            var item = db.NhaCungCaps.FirstOrDefault(nccap => nccap.MaNCC == ncc.MaNCC);
            item.TenNCC = ncc.TenNCC;
            item.DiaChi = ncc.DiaChi;
            item.Email = ncc.Email;
            item.SoDienThoai = ncc.SoDienThoai;
            item.Fax = ncc.Fax;

            db.SaveChanges();
            ViewBag.Message = "Sửa Thành Công";
            return View(item);
        }

        [HttpGet]
        public ActionResult XoaNhaCungCap(int? id)
        {
            if(id == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            var item = db.NhaCungCaps.FirstOrDefault(ncc => ncc.MaNCC == id);
            return View(item);

        }
        
        [HttpPost]
        public ActionResult XoaNhaCungCap(NhaCungCap ncc)
        {
            var item = db.NhaCungCaps.FirstOrDefault(nccap => nccap.MaNCC == ncc.MaNCC);
            if(item != null)
            {
                db.NhaCungCaps.Remove(item);
                db.SaveChanges();
                Session["deleteNhaCungCap"] = "Xóa Thành Công";
                return RedirectToAction("Index");

            }
            return HttpNotFound();
        }

    }
}