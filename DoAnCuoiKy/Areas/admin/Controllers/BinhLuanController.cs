using DoAnCuoiKy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace DoAnCuoiKy.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanTri,NhanVien")]
    public class BinhLuanController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: admin/BinhLuan
        public ActionResult Index()
        {
           List<BinhLuan> lstBinhLuan = db.BinhLuans.ToList();
            return View(lstBinhLuan);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            else
            {
                var item = db.BinhLuans.FirstOrDefault(r => r.MaBL == id);
                return View(item);
            }
        }

        [HttpPost]
        public ActionResult Edit(BinhLuan r)
        {
            BinhLuan item = db.BinhLuans.FirstOrDefault(d => d.MaBL == r.MaBL);
            item.hienthi = r.hienthi;
            db.SaveChanges();
            ViewBag.Message = "Sửa Thành Công";
            return View(item);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            else
            {
                var item = db.BinhLuans.FirstOrDefault(r => r.MaBL == id);
                return View(item);
            }
        }

        [HttpPost]
        public ActionResult Delete(BinhLuan r)
        {
            List<Reply> reply = db.Replies.Where(re => re.MaBL == r.MaBL).ToList();
            var item = db.BinhLuans.FirstOrDefault(n => n.MaBL == r.MaBL);

            foreach(var itemreply in reply)
            {
                db.Replies.Remove(itemreply);
                db.SaveChanges();
            }
         
            db.BinhLuans.Remove(item);
            db.SaveChanges();
            Session["deleteBinhLuan"] = "Xóa Thành Công";
            return RedirectToAction(nameof(Index));
        }

        public PartialViewResult BinhLuanTheoNgay(DateTime Ngay, int page = 1)
        {
            var lst = db.BinhLuans.ToList();
            List<BinhLuan> listBinhLuan = new List<BinhLuan>();
            foreach (var item in lst)
            {
                if (item.NgayBinhLuan.Value.ToString("dd/MM/yyyy") == Ngay.ToString("dd/MM/yyyy"))
                {
                    listBinhLuan.Add(item);
                }
            }
            ViewBag.Ngay = Ngay;
            return PartialView(listBinhLuan);
        }
    }
}