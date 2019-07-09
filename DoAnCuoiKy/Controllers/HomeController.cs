using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCuoiKy.Models;
using System.Web.Security;
using System.Net.Mail;
using System.Net;
using DoAnCuoiKy.Areas.admin.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DoAnCuoiKy.Controllers
{

    [Route("Home")]
    public class HomeController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        [Route("Home")]
        [Route("Home/Index")]
        [Route("/")]
        public ActionResult Index()
        {
          
            var lstDTM = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == 1 && n.DaXoa == false);
         
            ViewBag.ListDTM = lstDTM;

        
            var lstLT = db.SanPhams.Where(n => n.MaLoaiSP == 3 && n.Moi == 1 && n.DaXoa == false);
           
            ViewBag.ListLTM = lstLT;

           
            var lstMTB = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1 && n.DaXoa == false);
        
            ViewBag.ListMTBM = lstMTB;

            var lstSanPham = db.SanPhams;

            ViewBag.lstSanPham = lstSanPham;
            return View();
        }

        public ActionResult SanPhamNoiBat()
        {
            
            return PartialView();
        }
        public ActionResult MenuPartial()
        {
            //Truy vấn lấy về 1 list các sản phẩm
            var lstSP = db.SanPhams;
            return PartialView(lstSP); 
        }

       
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy([Bind(Exclude = "IsEmailVerified,ActivationCode")] ThanhVien tv,FormCollection f)
        {
            bool Status = false;
            string message = "";
                if (ModelState.IsValid)
                {
                    if(CheckEmail(tv.Email))
                    {
                        ModelState.AddModelError("", "Email của bạn đã được đăng ký, hãy chọn Email khác");
                        return View(tv);
                    }

                if (CheckUsername(tv.TaiKhoan))
                {
                    ModelState.AddModelError("", "Tài Khoản của bạn đã được đăng ký, hãy chọn Tài Khoản khác");
                    return View(tv);
                }
                tv.ActivationCode = Guid.NewGuid();

                tv.MaLoaiTV = 4;
                tv.Startdate = DateTime.Now;
                tv.month = DateTime.Now.Month;
                tv.year = DateTime.Now.Year;
                tv.MatKhau = MD5.GetMD5(tv.MatKhau);
              
                    db.ThanhViens.Add(tv);
                    db.SaveChanges();

                //Send Email to User
                SendVerificationLinkEmail(tv.Email, tv.ActivationCode.ToString());
                ViewBag.ThongBao = "Đăng Ký Thành Công. Đường link xác nhận " +
                    " đã được gửi đến Email: " + tv.Email;
                Status = true;
            }
                else
                { 
                    ViewBag.ThongBao = "Thêm thất bại";
                }
                return View();
        }
      
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            string sTaiKhoan = f["taikhoan"].ToString();
            string sMatKhau = f["matkhau"].ToString();
            sMatKhau = MD5.GetMD5(sMatKhau);
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            if (tv != null)
            {
                var lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);

                string Quyen = "";
                if (lstQuyen.Count() != 0)
                {
                    foreach (var item in lstQuyen)
                    {
                        Quyen += item.Quyen.MaQuyen + ",";
                    }
                    Quyen = Quyen.Substring(0, Quyen.Length - 1);
                    string[] arrayquyen = Quyen.Split(',');
                    PhanQuyen(tv.TaiKhoan.ToString(), Quyen);
                    Session["TaiKhoan"] = tv;

                    string tenQuanTri = db.LoaiThanhViens.Find(tv.MaLoaiTV).TenLoai;
                    if(tenQuanTri == "QuanTri" || tenQuanTri == "NhanVien")
                    {
                        return RedirectToAction("Index", "ThongKe", new { area = "admin" });
                    }
                    return RedirectToAction("Index","Home");
                }
            }

            ViewBag.Message = "Tên Tài Khoản Hoặc Mật Khẩu Không Đúng";
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {          
            string sTaiKhoan = f["txtTenDangNhap"].ToString();
            string sMatKhau = f["txtMatKhau"].ToString();
            sMatKhau = MD5.GetMD5(sMatKhau);
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            if (tv != null)
            {              
                var lstQuyen = db.LoaiThanhVien_Quyen.Where(n => n.MaLoaiTV == tv.MaLoaiTV);
             
                string Quyen = "";
                if (lstQuyen.Count() != 0)
                {
                    foreach (var item in lstQuyen)
                    {
                        Quyen += item.Quyen.MaQuyen + ",";
                    }
                    Quyen = Quyen.Substring(0, Quyen.Length - 1);
                    PhanQuyen(tv.TaiKhoan.ToString(), Quyen);
                    Session["TaiKhoan"] = tv;
                    return Content("<script>window.location.reload();</script>");
                }
            }
           
            return Content("Tài khoản hoặc mật khẩu không đúng!");
        }
       
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        public void PhanQuyen(string TaiKhoan, string Quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1,
                                          TaiKhoan, 
                                          DateTime.Now, 
                                          DateTime.Now.AddHours(3), 
                                          false, 
                                          Quyen, 
                                          FormsAuthentication.FormsCookiePath);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);
        }

        public ActionResult KhongDuocPhep()
        {
            return View();
        }

        [NonAction]
        public bool CheckEmail(string Email)
        {
            var item = db.ThanhViens.FirstOrDefault(tv=>tv.Email == Email);
            if(item != null)
            {
                return true;
            }
            return false;
        }

        [NonAction]
        public bool CheckUsername(string username)
        {
            var item = db.ThanhViens.FirstOrDefault(tv => tv.TaiKhoan == username);
            if (item != null)
            {
                return true;
            }
            return false;
        }



        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/Home/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("kimtoan1997.ld@gmail.com", "Store Shop");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "12345678@#"; 

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Tạo Tài Khoản Thành Công!";
                body = "<br/><br/>Tài Khoản Của Bạn" +
                    " Đã tạo thành công. Hãy click vào đường dẫn bên dưới để xác thực" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";

            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Đổi Mật Khẩu";
                body = "Hi,<br/>br/>Hãy Click vào đường link này để đổi mật khẩu" +
                    "<br/><br/><a href=" + link + ">Đổi Mật Khẩu</a>";
            }


            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }


        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
          
                db.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                                                                // Confirm password does not match issue on save changes
                var v = db.ThanhViens.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    db.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            
            ViewBag.Status = Status;
            return View();
        }

        //Part 3 - Forgot Password

        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string Email)
        {
            //Verify Email ID
            //Generate Reset password link 
            //Send Email 
            string message = "";
            bool status = false;
                var account = db.ThanhViens.Where(a => a.Email == Email).FirstOrDefault();
                if (account != null)
                {
                    //Send email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.Email, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;
                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                    //in our model class in part 1
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                   message = "Địa Chỉ thay đổi mật khẩu đã được gửi tói email.";
                }
                else
                {
                    message = "Không tìm thấy tài khoản";
                }
            
            ViewBag.Message = message;
            return View();
        }

        public ActionResult ResetPassword(string id)
        {

            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

          
                var user = db.ThanhViens.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
              
                    var user = db.ThanhViens.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.MatKhau = model.NewPassword;
                        user.ResetPasswordCode = "";
                        db.Configuration.ValidateOnSaveEnabled = false;
                        db.SaveChanges();
                        message = "Mật Khẩu Đã Được Đổi";
                    }
                
            }
            else
            {
                message = "Có Lỗi Khi Đổi Mật Khẩu";
            }
            ViewBag.Message = message;
            return View(model);
        }

        [HttpGet]
        public ActionResult ThongTinThanhVien(int id)
        {
            var info = db.ThanhViens.Find(id);
            return View(info);
        }
        
        [HttpPost]
        public ActionResult ThongTinThanhVien(ThanhVien tv)
        {
            var thanhvien = db.ThanhViens.Find(tv.MaThanhVien);
            if (ModelState.IsValid)
            {
                if(!tv.TaiKhoan.Contains(thanhvien.TaiKhoan))
                {
                    if (CheckUsername(tv.TaiKhoan))
                    {
                        ModelState.AddModelError("", "Tài Khoản của bạn đã được đăng ký, hãy chọn Tài Khoản khác");
                        return View(tv);
                    }
                }
                if(!tv.Email.Contains(thanhvien.Email))
                {
                    if (CheckEmail(tv.Email))
                    {
                        ModelState.AddModelError("", "Email của bạn đã được đăng ký, hãy chọn Email khác");
                        return View(tv);
                    }
                }

                if(tv.MatKhau != null)
                {
                    tv.MatKhau = MD5.GetMD5(tv.MatKhau);
                    thanhvien.MatKhau = tv.MatKhau;
                }

                thanhvien.TaiKhoan = tv.TaiKhoan;
                thanhvien.SoDienThoai = tv.SoDienThoai;
                thanhvien.HoTen = tv.HoTen;
                thanhvien.DiaChi = tv.DiaChi;
                thanhvien.Email = tv.Email;           
                db.SaveChanges();
                Session["TaiKhoan"] = thanhvien;
                //Session["TaiKhoan"] = null;
                //FormsAuthentication.SignOut();
            }
            else
            {
                ViewBag.ThongBao = "Thay đổi thông tin thất bại";
            }
            return RedirectToAction(nameof(Index));
        }
        public JsonResult GetLstSanPham()
        {
            var lstSanPham = db.SanPhams.ToList();       
            return new JsonResult() { Data = lstSanPham };
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
    
        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(Contact c)
        {
            GuiEmail("Xác Nhận Đã Contact", c.Email, "kimtoan1997.ld@gmail.com", "12345678@#", "Cảm ơn bạn đã liên hệ với chúng tôi");
            db.Contacts.Add(c);
            db.SaveChanges();
            ViewBag.ThongBao = "Cảm ơn bạn đã góp ý với chúng tôi";
            return View();
        }

        public void GuiEmail(string Title, string ToEmail, string FromEmail, string PassWord, string Content)
        {

            MailMessage mail = new MailMessage();
            mail.To.Add(ToEmail);
            mail.From = new MailAddress(ToEmail);
            mail.Subject = Title;
            mail.Body = Content;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential
            (FromEmail, PassWord);
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        public PartialViewResult BannerPartial()
        {
            List<BannerInfo> lstBanner = db.BannerInfoes.ToList();
            return PartialView(lstBanner);
        }
        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {

            var productnames = (from sp in db.SanPhams
                                where sp.TenSP.Contains(prefix)
                                select new
                                {
                                    tenSp = sp.TenSP
                                }).ToList();

            return Json(productnames);
        }
    }
}