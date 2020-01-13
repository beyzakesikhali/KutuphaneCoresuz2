using KutuphaneCoresuz.Models;
using KutuphaneCoresuz.Models.Context;
using KutuphaneCoresuz.Models.Data;
using KutuphaneCoresuz.Models.ModelforDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KutuphaneCoresuz.Controllers
{
    public class KitapController : Controller
    {
        KutuphaneContext db = new KutuphaneContext();

        // GET: Kitap
        public ActionResult Index()
        {
            return View();
        }

        // GET: Kitaps
     
        [AllowAnonymous]
        public ActionResult IndexKitap()
        {
         
            if (HttpContext.Session["KullaniciAdi"] == null)
            {
                return RedirectToAction("Login","Security");
            }
            
            List<KitapYazarAddModel> model = new List<KitapYazarAddModel>();
            List<Kitap> kitap = new List<Kitap>();
            kitap = db.Kitaplar.ToList();
           // var kitaplar = db.Kitaplar.First();
            //var yazarResult = db.Yazarlar.First();
            if (kitap.Count()!= 0)
            {
                foreach (var item in kitap)
                {
                    
                    model.Add(new KitapYazarAddModel() { KitapAdi=item.Isim, Aciklama = item.Aciklama, yayinci = item.Yayinci, YazarAdi = item.Yazar.Isim, YazarSoyadi = item.Yazar.Soyisim });
                }
                if (model != null)
                {
                    return View(model);
                }
            }


            return View();
        }

        // GET: Kitaps/Details/5
        [AllowAnonymous]
        public ActionResult DetailsKitap(KitapYazarAddModel model)
        {
            int id = model.Id;
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kitap kitap = db.Kitaplar.Find(id);
            if (kitap == null)
            {
                return HttpNotFound();
            }
            return View(kitap);
        }
        [HttpPost]
        [AllowAnonymous]
        public JsonResult AdSoyad(int? id, string tip = "yazarAdGetir")
        {
            List<Yazar> yazarlarListesi = new List<Yazar>();
            yazarlarListesi = db.Yazarlar.ToList();
            List<SelectListItem> sonuc = new List<SelectListItem>();
            bool basariliMi = true;
            //string yazarlar = "";
            try
            {
                switch (tip)
                {
                    case "yazarAdGetir":
                        foreach (var ad in yazarlarListesi)
                        {
                           
                            sonuc.Add(new SelectListItem
                            {
                                Text = ad.Isim +" " +ad.Soyisim,
                                Value = ad.ID.ToString()
                            });

                        }
                       
                        break;
                    //case "yazarSoyadGetir":
                       
                    //    foreach (var soyad in db.Yazarlar.Where(y => y.ID ==id).ToList())
                            
                    //    {
                    //        if (id != null)
                    //        {
                    //            sonuc.Add(new SelectListItem
                    //            {
                    //                Text = soyad.Soyisim,
                    //                Value = soyad.ID.ToString()
                    //            });
                    //        }


                    //    }
                        //break;
                    default:
                        break;
                }
               
            }
          
            catch (Exception) 
            {
                basariliMi = false;
                sonuc = new List<SelectListItem>();
                sonuc.Add(new SelectListItem
                {
                    Text = "Bir hata oluştu!",
                    Value = "default"
                });

            }

           return Json(new { ok = basariliMi, text = sonuc });
        }

  
        // GET: Kitaps/Create
        [AllowAnonymous]
        [OutputCache(CacheProfile = "anaSayfaCache")]
        public ActionResult CreateKitap()
        {
            //Yazar yazarlar = new Yazar();
            //// List<string> yazarlistesi = new List<string>();
            //List<SelectListItem> yazarIsimListesi = (from k in db.Yazarlar
            //                                         select new SelectListItem
            //                                         {
            //                                             Text = k.Isim

            //                                         }
            //    ).ToList();
            ////ViewBag.yazarlarlistesi = new SelectList(yazarlistesi, "Isim");
            //ViewBag.yazarlarIsimlistesi = yazarIsimListesi;

            //List<SelectListItem> yazarSoyisimListesi = (from k in db.Yazarlar
            //                                            select new SelectListItem
            //                                            {
            //                                                Text = k.Soyisim

            //                                            }
            //   ).ToList();
            ////ViewBag.yazarlarlistesi = new SelectList(yazarlistesi, "Isim");
            //ViewBag.yazarlarSoyisimlistesi = yazarSoyisimListesi;

            if (HttpContext.Session["KullaniciAdi"] == null)
            {
               
                return RedirectToAction("Login","Security");
            }


            return View();


            //var yazarlar = db.Yazarlar.Select(q => q.Isim).OrderBy(q => q);

            //foreach (var item in yazarlar)
            //{
            //    yazarlistesi.Add(item);
            //}

        }
        [OutputCache(CacheProfile = "anaSayfaCache")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CreateKitap(KitapYazarAddModel model)
        {
           
            //dropdown doldurmak için
            KitapYazarAddModel kitapYazarAddModel = new KitapYazarAddModel();
            List<SelectListItem> modelAdList = new List<SelectListItem>();
            List<SelectListItem> modelSoyadList = new List<SelectListItem>();
            Kitap yeniKitap = new Kitap();
            UyeKitap yeniUyeKitap = new UyeKitap();
            int SeciliYazarID = Convert.ToInt32(model.YazarAdi);
            
            var SeciliKitapAdi = model.KitapAdi;
            string AktifUye = HttpContext.Session["KullaniciAdi"].ToString();
            var AktifUyeResult = db.Uyeler.Where(i => i.isim == AktifUye).FirstOrDefault();
            var KitapVarmi = db.Kitaplar.Where(k => k.Isim  == SeciliKitapAdi).FirstOrDefault();
            var YazarResult = db.Yazarlar.Where(i => i.ID == SeciliYazarID).FirstOrDefault();
     
            if (KitapVarmi == null)
            {
                if (SeciliYazarID != 0)
                {
                    if (ModelState.IsValid)
                    {
                        //önce kitap tablosu
                        yeniKitap.Isim =model.KitapAdi;
                        yeniKitap.Yayinci = model.yayinci;
                        yeniKitap.Aciklama = model.Aciklama;
                        yeniKitap.YazarID = SeciliYazarID;
                        yeniKitap.Yazar = YazarResult;

                        yeniUyeKitap.UyeID = AktifUyeResult.ID;
                        yeniUyeKitap.KitapID = yeniKitap.ID;
                        yeniUyeKitap.Kitap = yeniKitap;
                        yeniUyeKitap.Uye = AktifUyeResult;
                      
                        db.Kitaplar.Add(yeniKitap);
                        db.SaveChanges();
                    
                        db.UyeKitap.Add(yeniUyeKitap);
                        db.SaveChanges();
                        kitapYazarAddModel.YazarAdi = yeniKitap.Yazar.Isim;
                        kitapYazarAddModel.YazarSoyadi = yeniKitap.Yazar.Soyisim;
                        kitapYazarAddModel.YazarYorum = yeniKitap.Yazar.Yorum;

                       
                    }

                }
                else
                {
                    return View(ViewBag("Yazar Seçmediniz!"));
                }
            }
            else
            {
                yeniUyeKitap.UyeID = AktifUyeResult.ID;
                yeniUyeKitap.KitapID = KitapVarmi.ID;
                yeniUyeKitap.Kitap = KitapVarmi;
                yeniUyeKitap.Uye = AktifUyeResult;
                db.UyeKitap.Add(yeniUyeKitap);
                db.SaveChanges();
            }
            return View();
        }

        // GET: Kitaps/Edit/5
        [AllowAnonymous]
        public ActionResult EditKitap(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kitap kitap = db.Kitaplar.Find(id);
            if (kitap == null)
            {
                return HttpNotFound();
            }
            return View(kitap);
        }

        // POST: Kitaps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult EditKitap(KitapYazarAddModel kitap)
        {

            if (ModelState.IsValid)
            {
                db.Entry(kitap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexKitap");
            }
            return View(kitap);
        }

        // GET: Kitaps/Delete/5
        [AllowAnonymous]
        public ActionResult DeleteKitap(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kitap kitap = db.Kitaplar.Find(id);
            if (kitap == null)
            {
                return HttpNotFound();
            }
            return View(kitap);
        }

        // POST: Kitaps/Delete/5
        [HttpPost, ActionName("DeleteKitap")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult DeleteConfirmedKitap(int id)
        {
            Kitap kitap = db.Kitaplar.Find(id);
            db.Kitaplar.Remove(kitap);
            db.SaveChanges();
            return RedirectToAction("IndexKitap");
        }

    }
}