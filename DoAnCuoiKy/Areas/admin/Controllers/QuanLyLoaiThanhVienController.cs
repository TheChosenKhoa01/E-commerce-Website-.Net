using DoAnCuoiKy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCuoiKy.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanTri")]
    public class QuanLyLoaiThanhVienController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            var lstLoaiThanhVien = db.LoaiThanhViens.ToList();
            return View(lstLoaiThanhVien);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(LoaiThanhVien ltv)          
        {
            if(!ModelState.IsValid)
            {
                return View(ltv);
            }

            ltv.UuDai = 0;
            db.LoaiThanhViens.Add(ltv);
            db.SaveChanges();
            ViewBag.Message = "Thêm Thành Công";
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
         
            var item = db.LoaiThanhViens.FirstOrDefault(ltv => ltv.MaLoaiTV == id);
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(LoaiThanhVien ltv)
        {
            if (!ModelState.IsValid)
            {
                return View(ltv);
            }

            var item = db.LoaiThanhViens.FirstOrDefault(loaiTV => loaiTV.MaLoaiTV == ltv.MaLoaiTV);
            item.TenLoai = ltv.TenLoai;
            item.UuDai = ltv.UuDai;

            db.SaveChanges();

            ViewBag.Message = "Sửa Thành Công";
            return View(item);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var item = db.LoaiThanhViens.FirstOrDefault(ltv => ltv.MaLoaiTV == id);
            return View(item);
            
        }

        [HttpPost]
        public ActionResult Delete(LoaiThanhVien ltv)
        {     
            var item = db.LoaiThanhViens.FirstOrDefault(loaitv => loaitv.MaLoaiTV == ltv.MaLoaiTV);

            if (item == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            db.LoaiThanhViens.Remove(item);
            db.SaveChanges();
            Session["deleteLoaiThanhVien"] = "Xóa Thành Công";
            return RedirectToAction(nameof(Index));
        }
    }
}