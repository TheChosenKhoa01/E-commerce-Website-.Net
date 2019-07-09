using DoAnCuoiKy.Areas.admin.Models;
using DoAnCuoiKy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCuoiKy.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanTri,QuanLyThanhVien,NhanVien")]
    public class QuanLyThanhVienController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            var lstThanhVien = db.ThanhViens.ToList();
           
            var lstLoaiThanhVien = db.LoaiThanhViens;

            ViewBag.lstLoaiThanhVien = lstLoaiThanhVien;
            return View(lstThanhVien);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhViens.OrderBy(n=>n.TenLoai), "MaLoaiTV","TenLoai");
            return View();
        }

        [HttpPost]
        public ActionResult Create(ThanhVien tv)
        {
            ViewBag.MaLoaiTV = new SelectList(db.LoaiThanhViens.OrderBy(n => n.TenLoai), "MaLoaiTV", "TenLoai");
            if (!ModelState.IsValid)
            {
                return View(tv);
            }

            if(KiemTraTaiKhoan(tv.TaiKhoan))
            {
                ModelState.AddModelError("", "Tài Khoản Đã Tồn Tại. Bạn Hãy Tạo Lại Tài Khoản Khác");
                return View(tv);
            }

            if(KiemTraEmail(tv.Email))
            {
                ModelState.AddModelError("", "Email Đã Tồn Tại. Bạn Hãy Nhập Lại Tài Khoản Khác");
                return View(tv);
            }

            tv.MatKhau = MD5.GetMD5(tv.MatKhau);
            db.ThanhViens.Add(tv);
            db.SaveChanges();

            ViewBag.Message = "Thêm Thành Công";
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var lstItem = db.LoaiThanhViens.ToList();
            ThanhVien thanhvien = db.ThanhViens.Find(id);
            LoaiThanhVien loaithanhvien = db.LoaiThanhViens.Find(thanhvien.MaLoaiTV);
            lstItem.Insert(0, loaithanhvien);
            ViewBag.MaLoaiTV = new SelectList(lstItem, "MaLoaiTV", "TenLoai");

            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            ThanhVien item = db.ThanhViens.FirstOrDefault(tv => tv.MaThanhVien == id);
            if(item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(ThanhVien tv)
        {
            var lstItem = db.LoaiThanhViens.ToList();
            ThanhVien thanhvien = db.ThanhViens.Find(tv.MaThanhVien);
            LoaiThanhVien loaithanhvien = db.LoaiThanhViens.Find(thanhvien.MaLoaiTV);
            lstItem.Insert(0, loaithanhvien);
            ViewBag.MaLoaiTV = new SelectList(lstItem, "MaLoaiTV", "TenLoai");
            ViewBag.MaLoaiTV = new SelectList(lstItem, "MaLoaiTV", "TenLoai");        
            var item = db.ThanhViens.FirstOrDefault(n => n.MaThanhVien == tv.MaThanhVien);
            item.MaLoaiTV = tv.MaLoaiTV;

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

            var item = db.ThanhViens.FirstOrDefault(n => n.MaThanhVien == id);
            if(item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(ThanhVien tv)
        {
            var item = db.ThanhViens.FirstOrDefault(n => n.MaThanhVien == tv.MaThanhVien);
            var khachhang = db.KhachHangs.SingleOrDefault(n => n.MaThanhVien == tv.MaThanhVien);
            db.KhachHangs.Remove(khachhang);
            db.ThanhViens.Remove(item);
            db.SaveChanges();
            Session["deleteThanhVien"] = "Xóa Thành Công";
            return RedirectToAction(nameof(Index));
        }

        public bool KiemTraEmail(string email )
        {
            return db.ThanhViens.FirstOrDefault(n => n.Email == email) != null;
        }

        public bool KiemTraTaiKhoan(string TaiKhoan)

        {
            return db.ThanhViens.FirstOrDefault(tv => tv.TaiKhoan == TaiKhoan) != null;
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