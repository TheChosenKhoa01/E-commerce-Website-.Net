using DoAnCuoiKy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCuoiKy.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanTri,QuanLyPhieuNhap,NhanVien")]
    public class QuanLyPhieuNhapController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
     
        public ActionResult Index()
        {
            var listChiTietPhieuNhap = db.ChiTietPhieuNhaps.ToList();
            ViewBag.lstChiTietPhieuNhap = listChiTietPhieuNhap;
            return View();
        }

        [HttpGet]  
        public ActionResult NhapHang()
        {
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            return View();
        }

        [HttpPost]
        public ActionResult NhapHang(PhieuNhap model, IEnumerable<ChiTietPhieuNhap> lstModel)
        {
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            model.DaXoa = false;
            db.PhieuNhaps.Add(model);
            db.SaveChanges();        
            SanPham sp;
            foreach (var item in lstModel)
            {
                
                sp = db.SanPhams.Single(n => n.MaSP == item.MaSP);
                sp.SoLuongTon += item.SoLuongNhap;
                
                item.MaPN = model.MaPN;
            }
            db.ChiTietPhieuNhaps.AddRange(lstModel);
            db.SaveChanges();
            return View();
        }
        [HttpGet]
        public ActionResult DSSPHetHang()
        {
           
            var lstSP = db.SanPhams.Where(n => n.DaXoa == false && n.SoLuongTon <= 5);
            return View(lstSP);

        }
       
        [HttpGet]
        public ActionResult NhapHangDon(int? id)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");          
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
            return View(sp);

        }      
        [HttpPost]
        public ActionResult NhapHangDon(PhieuNhap model, ChiTietPhieuNhap ctpn)
        {
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);

            model.NgayNhap = DateTime.Now;
            model.DaXoa = false;
            db.PhieuNhaps.Add(model);
            db.SaveChanges();       
            ctpn.MaPN = model.MaPN;      
            SanPham sp = db.SanPhams.Single(n => n.MaSP == ctpn.MaSP);
            sp.SoLuongTon += ctpn.SoLuongNhap;
            db.ChiTietPhieuNhaps.Add(ctpn);
            db.SaveChanges();
            return View(sp);

        }
        
        public PartialViewResult ChiTietPhieuNhapTheoNgay(DateTime ngaynhap)
        {
            //var listChiTietPhieuNhap = db.ChiTietPhieuNhaps.Where(p => p.PhieuNhap.NgayNhap.Value.ToString("dd/MM/yyyy").Equals(ngaynhap.ToString("dd/MM/yyyy")));
            var lst = db.ChiTietPhieuNhaps.ToList();
            List<ChiTietPhieuNhap> listChiTietPhieuNhap = new List<ChiTietPhieuNhap>();
            foreach(var item in lst)
            {
                if(item.PhieuNhap.NgayNhap.Value.ToString("dd/MM/yyyy")== ngaynhap.ToString("dd/MM/yyyy"))
                {
                    listChiTietPhieuNhap.Add(item);
                }
            }

            ViewBag.lstChiTietPhieuNhap = listChiTietPhieuNhap;
            return PartialView();
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