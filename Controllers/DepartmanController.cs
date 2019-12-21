using ofis_ise.Models.EntityFramework;
using ofis_ise.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ofis_ise.Controllers
{
    [Authorize(Roles = "A,U")]
    public class DepartmanController : Controller
    {
        // GET: Departman
        ofissEntities db = new ofissEntities();
        [HandleError]
        public ActionResult Index()
        {
            var model = db.Departman.ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult Yeni(string DepartmanAdi)
        {

            return View("DepartmanForm", new Departman());
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Kaydet(Departman departman)
        {
            if (!ModelState.IsValid)
            {
                return View("DepartmanForm");
            }
            MesajViewModel model = new MesajViewModel();
            if (departman.Id == 0)
            {
                db.Departman.Add(departman);
                model.Mesaj = departman.Ad+"başarıyla eklendi..";
            }
            else
            {
                var GuncellenecekDepartman = db.Departman.Find(departman.Id);
                if (GuncellenecekDepartman == null)
                {
                    return HttpNotFound();
                }
                GuncellenecekDepartman.Ad = departman.Ad;
                model.Mesaj = departman.Ad + "başarıyla güncellendi..";
            }
            //db.Departman.Add(departman);
            db.SaveChanges();
            model.Status=true;
            model.LinkText = "Departman listesi";
            model.Url = "/Departman";

            return View("_Mesaj",model);

        }

        public ActionResult Guncelle(int id)
        {
            var model = db.Departman.Find(id);
            if (model == null)
                return HttpNotFound();
            return View("DepartmanForm", model);
        }
        public ActionResult Sil(int id)
        {
            var SilinecekDepartman = db.Departman.Find(id);
            if (SilinecekDepartman == null)
                return HttpNotFound();
            db.Departman.Remove(SilinecekDepartman);
            //Entity validation hata tesbiti
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}