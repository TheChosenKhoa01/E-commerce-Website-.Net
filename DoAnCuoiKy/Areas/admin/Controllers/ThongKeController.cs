using DoAnCuoiKy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCuoiKy.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanTri,ThongKe")]
    public class ThongKeController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {     
            ViewBag.SoLuongNguoiTryCap = HttpContext.Application["SoLuongNguoiTryCap"].ToString();
            ViewBag.Online = HttpContext.Application["Online"].ToString();

             var lstnguoidung = db.ThanhViens.Where(tv=>tv.year == DateTime.Now.Year).ToList();
      
            List<DonDatHang> lstDonDatHang = db.DonDatHangs.ToList();
            lstDonDatHang = lstDonDatHang.Where(ddh => ddh.NgayDat.Value.ToString("yyyy").Contains( DateTime.Now.Year.ToString())).ToList();

            var lstSanPham = db.SanPhams.ToList();
            ViewBag.lstnguoidung = lstnguoidung;
            ViewBag.lstDonDatHang = lstDonDatHang;
            ViewBag.lstSanPham = lstSanPham ;
            return View();
        }
        public ActionResult SoLuongNguoiDangKyTheoNam(int year)
        {
            var lstnguoidung = db.ThanhViens.Where(tv => tv.year == year);
            return View();
        }

        public bool CheckYearEqual(DateTime date, string year)
        {
            return date.ToString("yyyy").Contains(year);
        }

       public PartialViewResult ThongKePartialView(int year = 2019)
        {
            var lstnguoidung = db.ThanhViens.Where(tv => tv.year == year).ToList();

            List<DonDatHang> lstDonDatHang = db.DonDatHangs.ToList();
            lstDonDatHang = lstDonDatHang.Where(ddh => ddh.NgayDat.Value.ToString("yyyy").Contains(year.ToString())).ToList();

            var lstSanPham = db.SanPhams.ToList();
            ViewBag.lstnguoidung = lstnguoidung;
            ViewBag.lstDonDatHang = lstDonDatHang;
            ViewBag.lstSanPham = lstSanPham;

            return PartialView();
        }
    }
}