using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCuoiKy.Models;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace DoAnCuoiKy.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanTri,QuanLyDonHang,NhanVien")]
    public class QuanLyDonHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
       
        public ActionResult Index()
        {
            var lstChuaThanhToan = ListDonHangChuaThanhToan();
            var lstChuaGiaoHang = ListDonHangChuaGiaoHang();
            var lstDaGiaoHang = ListDonHangDaGiaoHang();

            ViewBag.lstKhachHang = db.KhachHangs.ToList();
            ViewBag.lstChuaThanhToan = lstChuaThanhToan;
            ViewBag.lstChuaGiaoHang = lstChuaGiaoHang;
            ViewBag.lstDaGiaoHang = lstDaGiaoHang;

            return View();
        }

        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {      
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);

            KhachHang KH = db.KhachHangs.FirstOrDefault(kh => kh.MaKH == model.MaKH);

            ViewBag.khachhang = KH;
            if (model == null)
            {
                return HttpNotFound();
            }          
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == id);
           
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(model);

        }

        [HttpPost]
        public ActionResult DuyetDonHang(DonDatHang ddh)
        {
          
            DonDatHang ddhUpdate = db.DonDatHangs.Single(n => n.MaDDH == ddh.MaDDH);
            ddhUpdate.DaThanhToan = ddh.DaThanhToan;
            ddhUpdate.TinhTrangGiaoHang = ddh.TinhTrangGiaoHang;
            ddhUpdate.NgayGiao = ddh.NgayGiao;
            ddhUpdate.DaHuy = ddh.DaHuy;
            ddhUpdate.DaXoa = ddh.DaXoa;

            db.SaveChanges();

       
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == ddh.MaDDH).ToList();
            ViewBag.ListChiTietDH = lstChiTietDH;

            KhachHang KH = db.KhachHangs.FirstOrDefault(kh => kh.MaKH == ddh.MaKH);

            ViewBag.khachhang = KH;

            string content =  CreateBody(ddhUpdate, lstChiTietDH);
            SendMail("Xác đơn hàng của Trang Web", KH.Email, "kimtoan1997.ld@gmail.com", "12345678@#", content);
            return View(ddhUpdate);
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
        public IEnumerable<DonDatHang> ListDonHangChuaThanhToan()
        {

            return db.DonDatHangs.Where(n => n.DaThanhToan == false).OrderBy(n => n.NgayDat).ToList();
        }

        public IEnumerable<DonDatHang> ListDonHangChuaGiaoHang()
        {
            return db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == false && n.DaThanhToan == true).OrderBy(n => n.NgayGiao).ToList();
        }

        public IEnumerable<DonDatHang> ListDonHangDaGiaoHang()
        {
            return db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == true && n.DaThanhToan == true).ToList();
        }

        private string CreateBody(DonDatHang ddh, List<ChiTietDonDatHang> lst)
        {
            string body = string.Empty;

            string noidung = "";
            foreach (var item in lst)
            {
                noidung += item.SanPham.TenSP + " "+
                            item.SoLuong + " "+
                            item.DonGia +
                        " <br>";
            }

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Content/template/DonDatHang.html")))
            {

                body =  reader.ReadToEnd();

            }

            int soluong = (int)ddh.ChiTietDonDatHangs.Sum(n => n.SoLuong);
           string tongtien =  ddh.ChiTietDonDatHangs.Sum(n => n.SanPham.DonGia).Value.ToString("#,##");
            //string content = Content(ddh, lst);
            body = body.Replace("{MaDDH}", ddh.MaDDH.ToString());
            body = body.Replace("{content}", noidung); //replacing Parameters
            body = body.Replace( "{soluong}",soluong.ToString());
            body = body.Replace("{tongtien}", tongtien.ToString());

            return body;

        }

        public PartialViewResult QuanLyDonHangTheoNgay(DateTime date)
        {
            var lstChuaThanhToan = db.DonDatHangs.ToList();
            var lstChuaGiaoHang = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == false && n.DaThanhToan == true).ToList(); 
            var lstDaGiaoHang = db.DonDatHangs.Where(n => n.TinhTrangGiaoHang == true && n.DaThanhToan == true).ToList(); ;

            List<DonDatHang> listChuaThanhToan = new List<DonDatHang>();
            List<DonDatHang> listChuaGiao = new List<DonDatHang>();
            List<DonDatHang> listDaGiao = new List<DonDatHang>();

            foreach(var item in lstChuaThanhToan)
            {
                if(item.NgayDat.Value.ToString("dd/MM/yyyy") == date.ToString("dd/MM/yyyy"))
                {
                    listChuaThanhToan.Add(item);
                }
            }

            foreach (var item in lstChuaGiaoHang)
            {
                if (item.NgayDat.Value.ToString("dd/MM/yyyy") == date.ToString("dd/MM/yyyy"))
                {
                    listChuaGiao.Add(item);
                }
            }


            foreach (var item in lstDaGiaoHang)
            {
                if (item.NgayDat.Value.ToString("dd/MM/yyyy") == date.ToString("dd/MM/yyyy"))
                {
                    listDaGiao.Add(item);
                }
            }

            ViewBag.lstKhachHang = db.KhachHangs.ToList();
            ViewBag.lstChuaThanhToan = listChuaThanhToan;
            ViewBag.lstChuaGiaoHang = listChuaGiao;
            ViewBag.lstDaGiaoHang = listDaGiao;

            return PartialView();
        }

       
    }
}