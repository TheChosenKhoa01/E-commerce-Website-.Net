using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCuoiKy.Models;
using System.IO;
using System.Net.Mail;

namespace DoAnCuoiKy.Controllers
{
    public class GioHangController : Controller
    {

        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
      
        public List<ItemGioHang> LayGioHang()
        {     
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            { 
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        public double TinhTongSoLuong()
        { 
       
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }
     
        public decimal TinhTongTien()
        {
         
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);
        }

        public ActionResult GioHangPartial()
        {
            if (TinhTongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }

    
        public ActionResult XemGioHang()
        {
          
            List<ItemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }

      
        [HttpGet]
        public ActionResult SuaGioHang(int MaSP)
        {          
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
           
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
               
                Response.StatusCode = 404;
                return null;
            }
         
            List<ItemGioHang> lstGioHang = LayGioHang();
         
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
        
            ViewBag.GioHang = lstGioHang;

       
            return View(spCheck);
        }
   
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        { 
            SanPham spCheck = db.SanPhams.Single(n => n.MaSP == itemGH.MaSP);
            if (spCheck.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }        
            List<ItemGioHang> lstGH = LayGioHang();     
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
            itemGHUpdate.SoLuong = itemGH.SoLuong;
            itemGHUpdate.ThanhTien = itemGHUpdate.SoLuong * itemGHUpdate.DonGia;
            return RedirectToAction("XemGioHang");
        }

        public ActionResult XoaGioHang(int MaSP)
        {
         
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
           }     
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
            
                Response.StatusCode = 404;
                return null;
            }
       
            List<ItemGioHang> lstGioHang = LayGioHang();
       
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
       
            lstGioHang.Remove(spCheck);
            return RedirectToAction("XemGioHang");
        }
       
        public ActionResult DatHang(KhachHang kh)
        {
         
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            KhachHang khang = new KhachHang();
            if (Session["TaiKhoan"] == null)
            {
             
                TempData["Message"] = "Bạn Phải Đăng Nhập Trước Khi Mua Hàng";
                return RedirectToAction("Login", "Home");
            }
            else
            { 
           
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                khang.TenKH = tv.HoTen;
                khang.DiaChi = tv.DiaChi;
                khang.Email = tv.Email;
                khang.SoDienThoai = tv.SoDienThoai;
                khang.MaThanhVien = tv.MaThanhVien;
                db.KhachHangs.Add(khang);
                db.SaveChanges();
            }
          
         
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = khang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            ddh.DaHuy = false;
            ddh.DaXoa = false;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();
           
            List<ItemGioHang> lstGH = LayGioHang();
            List<ChiTietDonDatHang> lst = new List<ChiTietDonDatHang>();
            foreach (var item in lstGH)
            {
                ChiTietDonDatHang ctdh = new ChiTietDonDatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDonDatHangs.Add(ctdh);
                lst.Add(ctdh);
            }
            string content = CreateBody(ddh, lst);
            // string customerMail = KH.Email;
            SendMail("Xác đơn hàng của Trang Web", khang.Email, "kimtoan1997.ld@gmail.com", "12345678@#", content);
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XemGioHang");
        }
       
        public ActionResult ThemGioHangAjax(int MaSP, string strURL)
        {
            
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
              
                Response.StatusCode = 404;
                return null;
            }
          
            List<ItemGioHang> lstGioHang = LayGioHang();
          
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);
            if (spCheck != null)
            {
             
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return Content("<script> alert(\"Sản phẩm đã hết hàng!\")</script>");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                ViewBag.TongSoLuong = TinhTongSoLuong();
                ViewBag.TongTien = TinhTongTien();
                return PartialView("GioHangPartial");
            }

            ItemGioHang itemGH = new ItemGioHang(MaSP);
            if (sp.SoLuongTon < itemGH.SoLuong)
            {
                return Content("<script> alert(\"Sản phẩm đã hết hàng!\")</script>");
            }

            lstGioHang.Add(itemGH);
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView("GioHangPartial");
        }
        private string CreateBody(DonDatHang ddh, List<ChiTietDonDatHang> lst)
        {
            string body = string.Empty;

            string noidung = "";
            foreach (var item in lst)
            {
                noidung += item.TenSP + " " +
                            item.SoLuong + " " +
                            item.DonGia +
                        " <br>";
            }

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Content/template/ThongTinDonHang.html")))
            {

                body = reader.ReadToEnd();

            }

            int soluong = (int)ddh.ChiTietDonDatHangs.Sum(n => n.SoLuong);
            string tongtien = ddh.ChiTietDonDatHangs.Sum(n => n.DonGia).Value.ToString("#,##");
            //string content = Content(ddh, lst);
            body = body.Replace("{MaDDH}", ddh.MaDDH.ToString());
            body = body.Replace("{content}", noidung); //replacing Parameters
            body = body.Replace("{soluong}", soluong.ToString());
            body = body.Replace("{tongtien}", tongtien.ToString());

            return body;

        }
        public void SendMail(string Title, string ToEmail, string FromEmail, string PassWord, string Content)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail);
            mail.From = new MailAddress(FromEmail);
            mail.Subject = Title;
            mail.Body = Content;// phần thân của mail ở trên
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new System.Net.NetworkCredential
            (FromEmail, PassWord);
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}