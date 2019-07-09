using DoAnCuoiKy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCuoiKy.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanTri,NhanVien")]
    public class BannerController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: admin/Banner
        public ActionResult Index()
        {
            List<BannerInfo> lstBanner = db.BannerInfoes.ToList();
            return View(lstBanner);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(BannerInfo bn)
        {
            if(ModelState.IsValid)
            {
                db.BannerInfoes.Add(bn);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Message = "Thêm Thành Công";
            return View(bn);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            BannerInfo item = db.BannerInfoes.FirstOrDefault(tv => tv.id == id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(BannerInfo bn)
        {
            if(ModelState.IsValid)
            {
                BannerInfo item = db.BannerInfoes.FirstOrDefault(tv => tv.id == bn.id);
                item.BannerTitle = bn.BannerTitle;
                item.BannerContent = bn.BannerContent;
                db.SaveChanges();
                ViewBag.Message = "Sửa Thành Công";
                return View(item);             
            }
            return View(bn);
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            BannerInfo item = db.BannerInfoes.FirstOrDefault(tv => tv.id == id.Value);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(BannerInfo bn)
        {
            db.BannerInfoes.Remove(bn);
            db.SaveChanges();
            Session["deleteBanner"] = "Xóa Thành Công";
            return RedirectToAction(nameof(Index));
        }
    }
}