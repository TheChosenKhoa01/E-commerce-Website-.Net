using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnCuoiKy.Models;
using PagedList;
using DoAnCuoiKy.Areas.admin.Models;
using System.Web.Security;

namespace DoAnCuoiKy.Controllers
{
    public class SanPhamController : Controller
    {
        //
        // GET: /SanPham/
        //Sử dụng partial view
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        //Tạo 2 partial view sản phẩm 1 và 2 để hiển thị sản phẩm theo 2 style khác nhau
        [ChildActionOnly]
        public ActionResult SanPhamStyle1Partial()
        {
            ViewBag.lstSP = db.SanPhams;
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult SanPhamStyle2Partial()
        {
            ViewBag.lstSP = db.SanPhams;
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult SanPhamNoiBat()
        {
            ViewBag.lstSP = db.SanPhams;
            return PartialView();
        }
        //Xây dựng trang xem chi tiết 
        public ActionResult XemChiTiet(int? id,string tensp)
        {
         
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
      
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id && n.DaXoa == false);
            if (sp == null)
            {
              
                return HttpNotFound();
            }

            sp.LuotXem = sp.LuotXem + 1;
            db.SaveChanges();
            ViewBag.lstSP = db.SanPhams;

            ViewBag.lstBinhLuan = db.BinhLuans.ToList();

            var countbinhluan = db.BinhLuans.Where(bl => bl.MaSP == sp.MaSP).Count();
            var sumRating = db.BinhLuans.Where(bl => bl.MaSP == sp.MaSP).Sum(bl => bl.danhgia);
            var rate = 0;
            if (countbinhluan > 0 && sumRating != null)
            {
                rate = (int)Math.Ceiling((decimal)sumRating / countbinhluan);
            }
               
            ViewBag.CountStar = rate;
            return View(sp);
        }

        public ActionResult XemSanPhamMaSP(int MaLoaiSP, int? page)
        {
            if(MaLoaiSP == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            IEnumerable<SanPham> sp = db.SanPhams.Where(s => s.MaLoaiSP == MaLoaiSP);

            int PageSize = 6;
            //Tạo biến thứ 2: Số trang hiện tại
            int PageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            sp = sp.OrderBy(n => n.MaSP).ToPagedList(PageNumber, PageSize);

            ViewBag.lstSP = db.SanPhams;
            return View(sp);
        }
      
        public ActionResult SanPham(int? MaLoaiSP, int? MaNSX,int? page)
        {         
            if (MaLoaiSP == null || MaNSX==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);
            if (lstSP.Count() == 0)
            {
               
                return HttpNotFound();  
            }
           
            int PageSize = 6;
       
            int PageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;

            ViewBag.lstSP = db.SanPhams;
            return View(lstSP.OrderBy(n=>n.MaSP).ToPagedList(PageNumber,PageSize));
        }

        public ActionResult DanhMucPartial()
        {
            var listSP = db.SanPhams;
            ViewBag.lstSP = listSP;

            return PartialView();
        }

        public ActionResult TagPartial()
        {
            var listSP = db.SanPhams;
            ViewBag.lstSP = listSP;

            return PartialView();
        }

        [Route("SanPham/SapXep")]
        public ActionResult SapXep(int selectSort, int? MaLoaiSP, int? MaNSX)
        {
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX);
            if (lstSP.Count() == 0)
            {
               
                return HttpNotFound();
            }
                 
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;

                     
            if (selectSort == 1)
            {
                return PartialView(lstSP.OrderBy(n => n.DonGia).ToList());
            }

            return PartialView(lstSP.OrderByDescending(n => n.DonGia).ToList());
        }

      
        public ActionResult SapXepTheoMaNSX(int selectSort, int? MaLoaiSP)
        {
            var lstSP = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP);
            if (lstSP.Count() == 0)
            {
              
                return HttpNotFound();
            }
        
            ViewBag.MaLoaiSP = MaLoaiSP;
           
            if (selectSort == 1)
            {
                return PartialView(lstSP.OrderBy(n => n.DonGia).ToList());
            }

            return PartialView(lstSP.OrderByDescending(n => n.DonGia).ToList());
        }

        public ActionResult SapXepTheoTuKhoa(int selectSort, string sTuKhoa)
        {
            var lstSP = db.SanPhams.Where(n=>n.TenSP.Contains(sTuKhoa));
            if (lstSP.Count() == 0)
            {
                
                return HttpNotFound();
            }
                                             
            ViewBag.TuKhoa = sTuKhoa;
            Session["TuKhoa"] = sTuKhoa;
            if (selectSort == 1)
            {
                return PartialView(lstSP.OrderBy(n => n.DonGia).ToList());
            }
            return PartialView(lstSP.OrderByDescending(n => n.DonGia).ToList());
        }

        [HttpPost]
        public ActionResult BinhLuan(BinhLuan bl)
      {
            if(Session["TaiKhoan"] == null)
            {                
                return Content("Phai Dang Nhap");
            }
            var tv = Session["TaiKhoan"] as ThanhVien;
            bl.MaThanhVien = tv.MaThanhVien;
            bl.NgayBinhLuan = DateTime.Now;

            var sp = db.SanPhams.Find(bl.MaSP);
            sp.LuotBinhLuan = sp.LuotBinhLuan + 1;
            bl.hienthi = true;
            db.BinhLuans.Add(bl);
            db.SaveChanges();
            return Content("<script>window.location.reload();</script>");
        }

        public PartialViewResult GetPaging(int? page,int id)
        {
            int pageSize = 1;
            int pageNumber = (page ?? 1);
            var lstBinhLuan = db.BinhLuans.Where(bl=>bl.MaSP == id).Where(bl=>bl.hienthi == true).ToList();
            var lstReply = db.Replies.Where(r => r.BinhLuan.MaSP == id).Where(r=>r.hienthi == true);
            ViewBag.lstReply = lstReply;
            return PartialView("BinhLuanPartial", lstBinhLuan.ToPagedList(pageNumber, pageSize));
        }

       public ActionResult TraLoiBinhLuan(Reply rp)
        { 
            if (rp.MaThanhVien != 0 || rp.MaThanhVien > 0)
            {
                rp.NgayTraLoi = DateTime.Now;
                rp.hienthi = true;
                db.Replies.Add(rp);
                db.SaveChanges();
                return Content("<script>window.location.reload();</script>");    
            }
            return Content("<script>alert(\"Bạn phải đăng nhập trước khi trả lời!\")</script>");
        }

        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Home");
        }
    }
}