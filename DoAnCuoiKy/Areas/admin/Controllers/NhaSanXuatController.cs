using DoAnCuoiKy.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCuoiKy.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanTri")]
    public class NhaSanXuatController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: admin/NhaSanXuat
        public ActionResult Index()
        {
            var lstNhaSanXuat = db.NhaSanXuats;
            return View(lstNhaSanXuat);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(NhaSanXuat nsx, HttpPostedFileBase HinhAnh)
        {
            if(!ModelState.IsValid)
            {
                return View(nsx);
            }
           
                if (HinhAnh != null)
                {
                    //Kiểm tra nội dung hình ảnh
                    if (HinhAnh.ContentLength > 0)
                    {

                        var fileName = Path.GetFileName(HinhAnh.FileName);

                        var path = Path.Combine(Server.MapPath("~/Content/LogoNSX"), fileName);

                        if (!System.IO.File.Exists(path))
                        {
                            HinhAnh.SaveAs(path);
                        }
                    }
                }

            nsx.Logo = HinhAnh.FileName;
            db.NhaSanXuats.Add(nsx);
            db.SaveChanges();

            ViewBag.Message = "Thêm Thành Công";
            return View();
        }

        [HttpGet]
        public ActionResult Eidt(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            else
            {
                var item = db.NhaSanXuats.FirstOrDefault(nsx => nsx.MaNSX == id);
                return View(item);
            }
        }
        
        [HttpPost]
        public ActionResult Edit(NhaSanXuat nsx, HttpPostedFileBase HinhAnh)
        {
            if(!ModelState.IsValid)
            {
                return View(nsx);
            }

            if (HinhAnh != null)
            {
                //Kiểm tra nội dung hình ảnh
                if (HinhAnh.ContentLength > 0)
                {

                    var fileName = Path.GetFileName(HinhAnh.FileName);

                    var path = Path.Combine(Server.MapPath("~/Content/LogoNSX"), fileName);

                    if (!System.IO.File.Exists(path))
                    {
                        HinhAnh.SaveAs(path);
                    }
                }
            }

            nsx.Logo = HinhAnh.FileName;

            var item = db.NhaSanXuats.FirstOrDefault(nsxuat => nsxuat.MaNSX == nsx.MaNSX);

            item.TenNSX = nsx.TenNSX;
            item.Logo = nsx.Logo;
            item.ThongTin = nsx.ThongTin;

            db.SaveChanges();
            ViewBag.Message = "Sửa Thành Công";
            return View(item);
        }

        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }

            var item = db.NhaSanXuats.FirstOrDefault(nsx => nsx.MaNSX == id);
            return View(item);
        }

        public ActionResult Delete(NhaSanXuat nsx)
        {
            var item = db.NhaSanXuats.FirstOrDefault(nsxuat => nsxuat.MaNSX == nsx.MaNSX);
            db.NhaSanXuats.Remove(item);
            db.SaveChanges();
            Session["deleteNhaSanXuat"] = "Xóa Thành Công";
            return RedirectToAction(nameof(Index));
        }       
    }
}