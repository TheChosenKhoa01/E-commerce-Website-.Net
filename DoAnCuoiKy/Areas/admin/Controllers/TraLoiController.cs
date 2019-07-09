using DoAnCuoiKy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCuoiKy.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanTri,NhanVien")]
    public class TraLoiController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: admin/TraLoi
        public ActionResult Index()
        {
            List<Reply> lstTraLoi = db.Replies.ToList();
            return View(lstTraLoi);
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
                var item = db.Replies.FirstOrDefault(r=>r.MaBL == id);
                return View(item);
            }
        }

        [HttpPost]
        public ActionResult Edit(Reply r)
        {
            Reply item = db.Replies.FirstOrDefault(d => d.MaBL == r.MaBL);
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
                var item = db.Replies.FirstOrDefault(r => r.MaBL == id);
                return View(item);
            }
        }

        [HttpPost]
        public ActionResult Delete(Reply r)
        {
            var item = db.Replies.FirstOrDefault(n => n.MaBL == r.MaBL);
            db.Replies.Remove(item);
            db.SaveChanges();
            Session["deleteReply"] = "Xóa Thành Công";
            return RedirectToAction(nameof(Index));
        }
    }
}