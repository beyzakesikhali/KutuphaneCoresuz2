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
                    if(item.aktiflik==1)
                    {
                        model.Add(new KitapYazarAddModel() { Id = item.ID, KitapAdi = item.Isim, Aciklama = item.Aciklama, yayinci = item.Yayinci, YazarAdi = item.Yazar.Isim, YazarSoyadi = item.Yazar.Soyisim });
                    }
                    
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
            int id = 0;
            //modelin içinde kitap id var 
            
                id = model.Id;
                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                // List<KitapYazarAddModel> gosterilecek = new List<KitapYazarAddModel>();
                //List<Kitap> kitaplar = db.Kitaplar.Where(k=>k.ID==id).ToList();


           
           


                if (model == null)
                {
                    return HttpNotFound();
                }
                return View(model);
            
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
                            if(ad.aktiflik==1)
                            {
                             sonuc.Add(new SelectListItem
                                         {
                                            Text = ad.Isim +" " +ad.Soyisim,
                                            Value = ad.ID.ToString()
                                          });                                                             

                            }                           
                           

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
            var KitapVarmi = db.Kitaplar.Where(k => k.Isim == SeciliKitapAdi).FirstOrDefault();
            var YazarResult = db.Yazarlar.Where(i => i.ID == SeciliYazarID).FirstOrDefault();
            if (AktifUye.Equals("admin") == true)//admine kitap eklemesin admin sadece sisteme kitap eklesin diye
            {
                if (KitapVarmi == null)//yeni kitap gelmiş demektir
                {
                    if (SeciliYazarID != 0)//yazar seçmediniz hatası için
                    {
                        if (ModelState.IsValid)
                        {
                            //önce kitap tablosu
                            yeniKitap.Isim = model.KitapAdi;
                            yeniKitap.Yayinci = model.yayinci;
                            yeniKitap.Aciklama = model.Aciklama;
                            yeniKitap.YazarID = SeciliYazarID;
                            yeniKitap.Yazar = YazarResult;
                            yeniKitap.aktiflik = 1;
                            //sadece kitap ekleyecek
                            //Fk ve ilişili tablolara ekleme işlemi yapılmayacak aktifUye eğer adminse
                            db.Kitaplar.Add(yeniKitap);
                            db.SaveChanges();

                        }
                        else
                        {
                            ViewBag.DbHata = "Database da hata var";
                            return View(ViewBag.DbHata);
                        }
                    }
                    else
                    {

                        ViewBag.Yazarsec = "Yazar Secin";
                        return View(ViewBag.Yazarsec);
                    }


                }//kitap önceden eklenmis ama aktifliği değişirilmiş yani silinmişse
                else if (KitapVarmi.aktiflik == 0)
                {
                    if (SeciliYazarID != 0)//yazar seçmediniz hatası için
                    {
                        if (ModelState.IsValid)
                        {
                            //önce kitap tablosu
                            KitapVarmi.Isim = model.KitapAdi;
                            KitapVarmi.Yayinci = model.yayinci;
                            KitapVarmi.Aciklama = model.Aciklama;
                            KitapVarmi.YazarID = SeciliYazarID;
                            KitapVarmi.Yazar = YazarResult;
                            KitapVarmi.aktiflik = 1;
                            //sadece kitap ekleyecek
                            //Fk ve ilişili tablolara ekleme işlemi yapılmayacak aktifUye eğer adminse
                            db.Kitaplar.Add(KitapVarmi);
                            db.SaveChanges();

                        }
                        else
                        {
                            ViewBag.DbHata = "Database da hata var";
                            return View(ViewBag.DbHata);
                        }
                    }
                }
                else
                {
                    ViewBag.Kitapmevcut = "Kitap Zaten var!!";
                    return View(ViewBag.Kitapmevcut);
                }

            }
            else//admin değilse kitap ekleyemesin zaten
            {
                ViewBag.adminHata = "Admin Değilsiniz, Sisteme Kitap ekleyemezsiniz";
                return View(ViewBag.adminHata);

            }
            return View();

        }
        //*********EDİT KİTAP BAŞI *********

        [HttpPost]
        [AllowAnonymous]


        public JsonResult EditKitapJson(int? id, string isim)
        {

            bool basarliMi = true;
            string sonuc = "editKitap";
            List<KitapYazarAddModel> model = new List<KitapYazarAddModel>();
            try
            {
                if (HttpContext.Session["KullaniciAdi"] == null)

                {
                    sonuc = "Login";
                    return Json(new { ok = basarliMi, text = sonuc });
                }
                //sonuc = "EditUye";
                var kitapResult = db.Kitaplar.Where(u => u.ID == id).FirstOrDefault();

                if (kitapResult != null)
                {
                    sonuc = "editKitap";
                    //model.Add(new KitapYazarAddModel() { Id = yazarResult.ID, Isim = yazarResult.Isim, YazarYorum = yazarResult.Yorum ,});
                    ////return View(model);

                    return Json(new { ok = basarliMi, text = sonuc });

                }
                sonuc = "hata";
                return Json(new { ok = basarliMi, text = sonuc });

            }
            catch (Exception)
            {
                basarliMi = false;
                sonuc = "Başarısız İşlem";
                return Json(new { ok = basarliMi, text = sonuc });
                throw;
            }


        }
       
        // GET: Kitaps/Edit/5
        [AllowAnonymous]
        public ActionResult EditKitap(int? id,int uyeId)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kitap kitap = db.Kitaplar.Find(id);
            List<KitapYazarAddModel> model = new List<KitapYazarAddModel>();
            ////////hata var bakılcak
            /////düzeltildi
            
            model.Add(new KitapYazarAddModel { Id = kitap.ID, KitapAdi = kitap.Isim, YazarAdi = kitap.Yazar.Isim, YazarSoyadi = kitap.Yazar.Soyisim, yayinci = kitap.Yayinci, Aciklama = kitap.Aciklama, YazarYorum = kitap.Yazar.Yorum, });

            if (kitap == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Kitaps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        public ActionResult EditKitap(int? id, string kitapadi,int yazarId, string yayinci, string aciklama,string yazaryorum )
        {
           
            var yazarResult = db.Yazarlar.Where(y => y.ID == yazarId).FirstOrDefault();
            Yazar gelenYazar = new Yazar();
           
            if(id==0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            Kitap kitap = db.Kitaplar.Find(id);
            if(kitap==null)
            {
                return HttpNotFound();
            }
            if (kitapadi != null)
            {

                if (yazarResult== null)
                {
                    if (ModelState.IsValid)
                    {

                        ///KİTAP GÜNCELLEME
                        kitap.Isim = kitapadi;
                        kitap.Yayinci = yayinci;
                        kitap.Aciklama = aciklama;
                        kitap.YazarID = yazarId;
                        kitap.Yazar.Isim = yazarResult.Isim;
                        kitap.Yazar.Soyisim = yazarResult.Soyisim;
                        kitap.Yazar.Yorum = yazaryorum;
                        db.Entry(kitap).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("IndexKitap", "Kitap");
                    }
                }
                else //yazar zaten varsa kitabın yazarı değiştirilmek isteniyorsa
                {
                    //    int gelenYazarId = yazarVarmi.ID;
                    //    kitap.YazarID = gelenYazarId;
                    //    kitap.Yazar.Isim = yazaradi;
                    //    kitap.Yazar.Soyisim = yazarsoyadi;
                    //    kitap.Yazar.Yorum = yazaryorum;
                    //    kitap.Aciklama = aciklama;
                    //    kitap.Isim = kitapadi;
                    //    kitap.Yayinci = kitap.Yayinci;
                    ViewBag.yazarHata = "Yazar Seçmediniz ya da boş bir yazar seçtiniz";
                    return View(ViewBag.yazarHata);
                                       
                }


            }
            
            return View();
        }

    // // -*******EDİT KİTAP SONU ****//////
        //



        //
        //**********DELETE KİTAP BAŞI **********
        //

        [HttpPost]
        [AllowAnonymous]

        public JsonResult DeleteKitapJson(int? id, string isim)
        {

            bool basarliMi = true;
            string sonuc = "deleteKitap";
            List<Yazar> model = new List<Yazar>();
            try
            {
                if (HttpContext.Session["KullaniciAdi"] == null)

                {
                    sonuc = "Login";
                    return Json(new { ok = basarliMi, text = sonuc });
                }
                //sonuc = "EditUye";
                var yazarResult = db.Yazarlar.Where(u => u.ID == id).FirstOrDefault();

                if (yazarResult != null)
                {
                    sonuc = "deleteKitap";
                    model.Add(new Yazar() { ID = yazarResult.ID, Isim = yazarResult.Isim, Soyisim = yazarResult.Soyisim, Yorum = yazarResult.Yorum });
                    //return View(model);

                    return Json(new { ok = basarliMi, text = sonuc });

                }
                sonuc = "hata";
                return Json(new { ok = basarliMi, text = sonuc });

            }
            catch (Exception)
            {
                basarliMi = false;
                sonuc = "Başarısız İşlem";
                return Json(new { ok = basarliMi, text = sonuc });
                throw;
            }


        }
        // GET: Kitaps/Delete/5
        [AllowAnonymous]
        public ActionResult DeleteKitap(int? id)
        {
            List<KitapYazarAddModel> model = new List<KitapYazarAddModel>();
            string yazarAdi = "";
            string yazarSoyadi = "";
            string yazarYorum = "";
            string mevcutKullanici = HttpContext.Session["KullaniciAdi"].ToString();
            if (mevcutKullanici == null)
            {
                RedirectToAction("Login", "Security");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }  
            var kitaplar = db.Kitaplar.Where(k => k.ID == id).FirstOrDefault();
            if (kitaplar == null)
            {
                return HttpNotFound();
            }
            else if(kitaplar.aktiflik==1)
            {
                yazarAdi = kitaplar.Yazar.Isim;
                yazarSoyadi = kitaplar.Yazar.Soyisim;
                yazarYorum = kitaplar.Yazar.Yorum;
                model.Add(new KitapYazarAddModel { Id = kitaplar.ID, KitapAdi = kitaplar.Isim, YazarAdi = yazarAdi, YazarSoyadi = yazarSoyadi, yayinci = kitaplar.Yayinci, YazarYorum = yazarYorum });
                return View(model);
            }
            return View(model);

          
           
        }

        //// POST: Kitaps/Delete/5
        //[HttpPost, ActionName("DeleteKitap")]
        //[ValidateAntiForgeryToken]
        //[AllowAnonymous]
        //public ActionResult DeleteConfirmedKitap(int id)
        //{
        //    Kitap kitap = db.Kitaplar.Find(id);
        //    db.Kitaplar.Remove(kitap);
        //    db.SaveChanges();
        //    return RedirectToAction("IndexKitap");
        //}



        [HttpPost]
        [AllowAnonymous]
        public ActionResult DeleteKitap(int? id, string kitapismi)
        {
          
            
            if (HttpContext.Session["KullaniciAdi"] == null)
            {
                return RedirectToAction("Login", "Security");
            }
            else
            {

                //var deleteUye = db.Yazarlar.Where(u => u.ID == id).FirstOrDefault();
                //int uyeId = 0;
                //  id = uyeIdResult.ID;
                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                else
                {
                   Kitap deletekitap = db.Kitaplar.Find(id);
                   if(deletekitap.KitapDurum==1)
                    {
                        deletekitap.aktiflik = 0;
                        db.Entry(deletekitap).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("IndexKitap", "Kitap");
                    }
                   else
                    {
                        return View("HATA! Kitap Üyede silemezsiniz.");
                    }
                    

                }

            }

        }

    }
}