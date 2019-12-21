using ofis_ise.Models.EntityFramework;
using ofis_ise.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ofis_ise.Controllers
{
    public class PersonelController : Controller
    {
        // GET: Personel

        ofissEntities db = new ofissEntities();
        public ActionResult Index()
        {
            var model = db.Personel.Include(x => x.Departman).ToList();
            return View(model);
        }
        [Authorize(Roles ="A")]

        public ActionResult Yeni()

        {
            var model = new PersonelFormViewModel()
            {
                Departmanlar = db.Departman.ToList(),
                Personel = new Personel()
            };


            return View("PersonelForm", model);
        }
        //[ValidateAntiForgeryToken]
        public ActionResult Kaydet(Personel personel)
        {
            if (!ModelState.IsValid)
            {
                var model = new PersonelFormViewModel()
                {
                    Departmanlar = db.Departman.ToList(),
                    Personel = personel
                };
                return View("PersonelForm", model);
            }
            if (personel.Id == 0) //ekleme işlemi
            {
                db.Personel.Add(personel);
            }
            else //Guncelle
            {
                db.Entry(personel).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Guncelle(int id)
        {
            var model = new PersonelFormViewModel()
            {
                Departmanlar = db.Departman.ToList(),
                Personel = db.Personel.Find(id)
            };

            return View("PersonelForm", model);
        }
        public ActionResult Sil(int id)
        {
            var SilinecekPersonel = db.Personel.Find(id);
            if (SilinecekPersonel == null)
            {
                return HttpNotFound();
            }
            db.Personel.Remove(SilinecekPersonel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult PersonelleriListele(int id)
        {
            var model = db.Personel.Where(x => x.DepartmanId == id).ToList();
            return PartialView(model);
        }
        public ActionResult ToplamMaas()
        {
            ViewBag.Maas = db.Personel.Sum(x=>x.Mass);
            return PartialView();
        }
    }
}