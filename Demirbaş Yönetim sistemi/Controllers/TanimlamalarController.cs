using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Demirbaş_Yönetim_sistemi.Models;
using Microsoft.AspNet.Identity;



namespace Demirbaş_Yönetim_sistemi.Controllers
{
    [Authorize]
    public class TanimlamalarController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DemirbasTanimlama(demirbas model)
        {
            if (model.adi != "" && model.adi != null)
            {

                using (demirbasEntities1 vt = new demirbasEntities1())
                {
                    model.kayit_tarihi = DateTime.Now;
                    model.kayit_kullanici_id = User.Identity.GetUserId();
                    vt.demirbas.Add(model);
                    vt.SaveChanges();
                }
            }
            using (demirbasEntities1 vt = new demirbasEntities1())
            {


                List<demirbas> demirbas = new List<demirbas>();

                demirbas = vt.demirbas.ToList();

                ViewBag.liste = demirbas;
                var liste = vt.demirbas.ToList();
            

            }
            return View();
        }

        public ActionResult PersonelTanimlama(personel model)
        {
            if (model.ad != "" && model.ad != null)
            {
                using (demirbasEntities1 vt = new demirbasEntities1())
                {
                    model.kayit_tarihi = DateTime.Now;
                    model.kayit_kullanici_id = User.Identity.GetUserId();
                    vt.personel.Add(model);


                    
                    vt.SaveChanges();
                }

           

         
            }
            using (demirbasEntities1 vt = new demirbasEntities1())
            {
              

                List<departman> departman = new List<departman>();

                departman = vt.departman.ToList();

                ViewBag.birimTanımı = departman;
                var tanim = vt.view_PersonelDepartman.ToList();
                ViewBag.PersonelDepartman = tanim;

            }
  

            return View();
        }
        public ActionResult DepartmanTanimla(departman model)
        {
            if (model.Adi != "" && model.Adi != null)
            {

                using (demirbasEntities1 vt = new demirbasEntities1())
                {

                    vt.departman.Add(model);
                    vt.SaveChanges();
                }
            }
            return View();
        }
        

        public ActionResult view_PersonelDepartman()
        {
            using (demirbasEntities1 vt = new demirbasEntities1())
            {
                var tanim = vt.view_PersonelDepartman.ToList();
                ViewBag.PersonelDepartman = tanim;
                return View();
            }
        }
        public ActionResult PersonelDuzenle(int id)
        {
            using (var db = new demirbasEntities1())
            {
                var personel = db.personel.Find(id);
                if (personel == null)
                {
                    return HttpNotFound();
                }

                ViewBag.birimTanımı = db.departman.ToList(); // dropdown için
                return View(personel);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersonelDuzenle(personel model)
        {
           
                using (var db = new demirbasEntities1())
                {
                    var mevcut = db.personel.Find(model.id);
                    if (mevcut == null)
                        return HttpNotFound();

                    mevcut.ad = model.ad;
                    mevcut.soyad = model.soyad;
                    mevcut.departman_Id = model.departman_Id;
                    mevcut.ise_baslangic = model.ise_baslangic;
                    mevcut.durum = model.durum;

                    db.SaveChanges();
                    return RedirectToAction("PersonelTanimlama");
                }
            

           
              

           
        }
        public ActionResult PersonelSil(int id)
        {
            bool sonuc = true;
            string mesaj = "";
            try
            {
                using (var db = new demirbasEntities1())
                {
                    var personel = db.personel.Find(id);
                    if (personel == null)
                    {
                        return HttpNotFound();
                    }

                    db.personel.Remove(personel);
                    db.SaveChanges();

                }

            }
            catch (Exception ex)
            {
                sonuc=false;
                mesaj=ex.Message;
            }
           

            return Json(new { success = sonuc,mesaj=mesaj });
        }
         public ActionResult DemirbasDuzenle(int id)
 {
          using (var db = new demirbasEntities1())
           {
         var demirbas = db.demirbas.Find(id);
         if (demirbas == null)
         {
             return HttpNotFound();
         }

         ViewBag.liste = db.demirbas.ToList(); // dropdown için
         return View(demirbas);
           }
 }
 [HttpPost]
 [ValidateAntiForgeryToken]
 public ActionResult DemirbasDuzenle(demirbas model)
 {
    
         using (var db = new demirbasEntities1())
         {
             var mevcut = db.demirbas.Find(model.id);
             if (mevcut == null)
                 return HttpNotFound();

             mevcut.adi = model.adi;
             mevcut.seri_no = model.seri_no;
             mevcut.mac = model.mac;
  

             db.SaveChanges();
             return RedirectToAction("DemirbasTanimlama");
         }
     

    
       

    
 }

        [HttpPost]
        public JsonResult DemirbasSil(int id)
        {
            try
            {
                using (var db = new demirbasEntities1())
                {
                    var demirbas = db.demirbas.Find(id);
                    if (demirbas == null)
                    {
                        return Json(new { success = false, mesaj = "Kayıt bulunamadı." });
                    }

                    db.demirbas.Remove(demirbas);
                    db.SaveChanges();

                    return Json(new { success = true });
                }
            }
            catch (Exception)
            {
               
                return Json(new { success = false, mesaj= "Demirbaş personel üzerine zimmetli ,kontrol edip tekrar deneyeiniz" });
            }

        }
       


    }





}


