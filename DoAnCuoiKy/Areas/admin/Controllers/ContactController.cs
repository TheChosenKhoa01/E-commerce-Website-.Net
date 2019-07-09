using DoAnCuoiKy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCuoiKy.Areas.admin.Controllers
{
    [Authorize(Roles = "QuanTri,NhanVien")]
    public class ContactController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
    
        public ActionResult Index()
        {
            var lstContact = db.Contacts.ToList();
            return View(lstContact);
        }      
        public ActionResult Delete(int id)
        {
            var contact = db.Contacts.Find(id);
            return View(contact);
        }

        [HttpPost]
        public ActionResult Delete(Contact c)
        {
            var item = db.Contacts.Find(c.ContactID);
            db.Contacts.Remove(item);
            db.SaveChanges();
            Session["deleteContact"] = "Xóa Thành Công";
            return RedirectToAction(nameof(Index));
        }
    }
}