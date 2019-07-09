using DoAnCuoiKy.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DoAnCuoiKy.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanTri,QuanLySanPham,NhanVien")]
    public class QuanLySanPhamController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
      

        public ActionResult Index()
        {
            return View(db.SanPhams.Where(n => n.DaXoa == false).OrderByDescending(n => n.MaSP));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");
            return View();

        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(SanPham sp, HttpPostedFileBase[] HinhAnh)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX");

            for (int i = 0; i < HinhAnh.Count(); i++)
            {
                if (HinhAnh[i] != null)
                {
                   
                    if (HinhAnh[i].ContentLength > 0)
                    {

                        var fileName = Path.GetFileName(HinhAnh[i].FileName);

                        var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), fileName);

                        if (!System.IO.File.Exists(path))
                        {
                            HinhAnh[i].SaveAs(path);
                        }
                    }
                }
            }

            sp.LuotXem = 0;
            sp.SoLanMua = 0;
            sp.LuotBinhChon = 0;
            sp.LuotBinhLuan = 0;

            sp.HinhAnh = HinhAnh[0].FileName;
            sp.HinhAnh1 = HinhAnh[1].FileName;
            sp.HinhAnh2 = HinhAnh[2].FileName;
            sp.HinhAnh3 = HinhAnh[3].FileName;
            sp.HinhAnh4 = HinhAnh[4].FileName;

            db.SanPhams.Add(sp);
            db.SaveChanges();
            ViewBag.Message = "Thêm Thành Công";
            return View();
        }


        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }

            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);

            return View(sp);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(SanPham model, HttpPostedFileBase[] HinhAnh)
        {

            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", model.MaLoaiSP);
            ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", model.MaNSX);
            var oldSP = db.SanPhams.FirstOrDefault(sp => sp.MaSP == model.MaSP);
          
            for (int i = 0; i < HinhAnh.Count(); i++)
            {
                if (HinhAnh[i] != null)
                {
                  
                    if (HinhAnh[i].ContentLength > 0)
                    {

                        var fileName = Path.GetFileName(HinhAnh[i].FileName);

                        var path = Path.Combine(Server.MapPath("~/Content/HinhAnhSP"), fileName);

                        if (!System.IO.File.Exists(path))
                        {
                            HinhAnh[i].SaveAs(path);
                        }
                    }
                }
            }

            if (HinhAnh[0] == null)
            {
                model.HinhAnh = oldSP.HinhAnh;
            }
            else
            {
                model.HinhAnh = HinhAnh[0].FileName;
            }
            if (HinhAnh[1] == null)
            {
                model.HinhAnh1 = oldSP.HinhAnh1;
            }
            else
            {
                model.HinhAnh1 = HinhAnh[1].FileName;
            }

            if (HinhAnh[2] == null)
            {
                model.HinhAnh2 = oldSP.HinhAnh2;
            }
            else
            {
                model.HinhAnh2 = HinhAnh[2].FileName;
            }

            if (HinhAnh[3] == null)
            {
                model.HinhAnh3 = oldSP.HinhAnh3;
            }
            else
            {
                model.HinhAnh3 = HinhAnh[3].FileName;
            }

            if (HinhAnh[4] == null)
            {
                model.HinhAnh4 = oldSP.HinhAnh4;
            }
            else
            {
                model.HinhAnh4 = HinhAnh[4].FileName;
            }


            //db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.Entry(oldSP).State = EntityState.Detached; // just added this line
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.Message = "Sửa Thành Công";
            return View(model);
        }

        //[HttpGet]
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        Response.StatusCode = 404;
        //        return null;
        //    }
        //    SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
        //    if (sp == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
        //    ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
        //    ViewBag.MaNSX = new SelectList(db.NhaSanXuats.OrderBy(n => n.MaNSX), "MaNSX", "TenNSX", sp.MaNSX);
        //    return View(sp);
        //}

        [HttpPost]
        public ActionResult Delete(int id)
        {
            SanPham model = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.SanPhams.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");
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