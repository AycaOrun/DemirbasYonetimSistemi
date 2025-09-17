using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using Demirbaş_Yönetim_sistemi.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNet.Identity;

namespace Demirbaş_Yönetim_sistemi.Controllers
{
    [Authorize]
    public class ustunezimmetController : Controller
    {

        public ActionResult personel_listele()
        {
            using (var db = new demirbasEntities1()) // kendi context'ini kullan
            {
                List<personel> personelListesi = new List<personel>();
                personelListesi = db.personel.ToList();

                ViewBag.ZimmetTipleri = personelListesi;
                db.SaveChanges();

                return View();
            }
        }

        public ActionResult demirbas_sec()
        {
            using (var db = new demirbasEntities1()) // kendi context'ini kullan

            {






                List<demirbas> demirbaslar = new List<demirbas>();

                demirbaslar = db.demirbas.ToList();




                ViewBag.ZimmetTipleri = demirbaslar;

                return View();
            }
        }
        public ActionResult zimmet_tanimlama(ustune_zimmet model)
        {

            using (demirbasEntities1 db = new demirbasEntities1()) // kendi context'ini kullan
            {

                List<demirbas> demirbaslar = new List<demirbas>();

                demirbaslar = db.demirbas.ToList();// demirbasların tamamini aşldık

                List<personel> PersonelListesi = new List<personel>();
                PersonelListesi = db.personel.ToList();

                ViewBag.ZimmetTipleri = demirbaslar;
                ViewBag.Personeller = PersonelListesi;

                var liste = db.view_ustunezimmet.ToList();
                ViewBag.ustune_zimmetler = liste;



            }



            if (model.personel_id > 0 && model.demirbas_id > 0)
            {
                using (demirbasEntities1 db = new demirbasEntities1())
                {
                    model.kayit_tarihi = DateTime.Now;
                    model.kayit_kullanici_id = User.Identity.GetUserId();
                    db.ustune_zimmet.Add(model);
                    db.SaveChanges();
                }

                return RedirectToAction("zimmet_tanimlama");
            }

            return View();


        }
        public ActionResult view_ustunezimmet()
        {
            using (demirbasEntities1 db = new demirbasEntities1())
            {
                var liste = db.view_ustunezimmet.ToList();
                ViewBag.ustune_zimmetler = liste;
                return View();
            }
        }
        [HttpPost]
        public ActionResult PasifEt(int id)
        {
            using (var db = new demirbasEntities1())
            {
                var zimmet = db.ustune_zimmet.FirstOrDefault(z => z.id == id);
                if (zimmet != null)
                {
                    zimmet.durum = 0; // pasif yap
                    zimmet.zimmet_bitis = DateTime.Now; // bitiş tarihini ata
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Zimmet bulunamadı" });
            }
        }

        [HttpPost]
        public ActionResult BitisTarihiGuncelle(int id, DateTime zimmet_bitis)
        {
            using (var db = new demirbasEntities1())
            {
                var zimmet = db.ustune_zimmet.FirstOrDefault(z => z.id == id);
                if (zimmet != null)
                {
                    zimmet.zimmet_bitis = zimmet_bitis;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("zimmet_tanimlama");

        }




    

        [HttpPost]
        public ActionResult PersonelSil(int id)
        {
            using (var db = new demirbasEntities1())
            {
                var personel = db.personel.FirstOrDefault(p => p.id == id);
                if (personel == null)
                {
                    return Json(new { success = false, message = "Personel bulunamadı." });
                }

            
                bool aktifZimmetVar = db.ustune_zimmet.Any(z =>
                    z.personel_id == id &&
                    z.durum == 1 &&
                    z.zimmet_bitis == null
                );

                if (aktifZimmetVar)
                {
                    return Json(new { success = false, message = "Bu personele ait demirbaş bulunmaktadır." });
                }

                db.personel.Remove(personel);
                db.SaveChanges();

                return Json(new { success = true, message = "Personel başarıyla silindi." });
            }

        }

        public ActionResult ZimmetlerExcel()
        {
            var db = new demirbasEntities1();
            var zimmetler = db.view_ustunezimmet.ToList();
            db.Dispose();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Zimmetler");

                // Başlıklar
                worksheet.Cell(1, 1).Value = "Personel";
                worksheet.Cell(1, 2).Value = "Demirbaş";
                worksheet.Cell(1, 3).Value = "Zimmet Başlangıç";
                worksheet.Cell(1, 4).Value = "Zimmet Bitiş";

                // Veri
                for (int i = 0; i < zimmetler.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = zimmetler[i].adsoyad;
                    worksheet.Cell(i + 2, 2).Value = zimmetler[i].adi;
                    worksheet.Cell(i + 2, 3).Value = zimmetler[i].zimmet_baslangic.ToString("dd.MM.yyyy");
                    worksheet.Cell(i + 2, 4).Value = zimmetler[i].zimmet_bitis?.ToString("dd.MM.yyyy");
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Zimmetler.xlsx");
                }
            }
        }
        demirbasEntities1 db = new demirbasEntities1();
        public ActionResult ZimmetlerPdf()
        {

            var zimmetler = db.view_ustunezimmet.ToList();

            // Bellekte PDF dosyası oluşturmak için stream
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);

                // Başlık
                Paragraph header = new Paragraph("Zimmet Raporu")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(18)
                   
                    .SetMarginBottom(20);
                document.Add(header);

                // Tablo (4 sütun)
                Table table = new Table(4).UseAllAvailableWidth();

                // Başlık satırı
                table.AddHeaderCell("Personel");
                table.AddHeaderCell("Demirbaş");
                table.AddHeaderCell("Başlangıç");
                table.AddHeaderCell("Bitiş");

                // Veriler
                foreach (var item in zimmetler)
                {
                    table.AddCell(item.adsoyad);
                    table.AddCell(item.adi);
                    table.AddCell(item.zimmet_baslangic.ToString("dd.MM.yyyy"));
                    table.AddCell(item.zimmet_bitis?.ToString("dd.MM.yyyy") ?? "-");
                }

                document.Add(table);
                document.Close();

                // PDF'i geri döndür
                byte[] fileBytes = ms.ToArray();
                return File(fileBytes, "application/pdf", "ZimmetRaporu.pdf");
            }
        }

    }
} 

        
    










