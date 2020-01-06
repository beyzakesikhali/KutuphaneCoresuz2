﻿using KutuphaneCoresuz.Models;
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
        public ActionResult IndexKitap()
        {
            return View(db.Kitaplar.ToList());
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

            List<SelectListItem> sonuc = new List<SelectListItem>();
            bool basariliMi = true;
            try
            {
                switch (tip)
                {
                    case "yazarAdGetir":
                        foreach (var ad in db.Yazarlar.ToList())
                        {
                            sonuc.Add(new SelectListItem
                            {
                                Text = ad.Isim,
                                Value = ad.YazarID.ToString()
                            });

                        }
                        break;
                    case "yazarSoyadGetir":
                       
                        foreach (var soyad in db.Yazarlar.Where(y => y.YazarID ==id).ToList())
                            
                        {
                            if (id != null)
                            {
                                sonuc.Add(new SelectListItem
                                {
                                    Text = soyad.Soyisim,
                                    Value = soyad.YazarID.ToString()
                                });
                            }


                        }
                        break;
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
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CreateKitap(KitapYazarAddModel model)
        {
            //List<KitapYazarAddModel> modelKy = new List<KitapYazarAddModel>();
            //foreach (var item in modelKy)
            //{
            //    return View(modelKy);

            //}

            //dropdown doldurmak için
            KitapYazarAddModel kitapYazarAddModel = new KitapYazarAddModel();
            List<SelectListItem> modelAdList = new List<SelectListItem>();
            List<SelectListItem> modelSoyadList = new List<SelectListItem>();
            //modelSoyadList = kitapYazarAddModel.YazarSoyadlari.ToList();
            //modelAdList = kitapYazarAddModel.YazarAdlari.ToList();
            //ViewBag.adlar = modelAdList;
           // ViewBag.soyadlar = modelSoyadList;


            Kitap yeniKitap = new Kitap();
            Yazar yeniYazar = new Yazar();
            YazarlarinKitaplari yeniYazarKitap = new YazarlarinKitaplari();
            UyelerinKitaplari yeniUyeKitap = new UyelerinKitaplari();
            int SeciliYazarID = Convert.ToInt32(model.YazarAdi);
            
         
            var SeciliKitapAdi = model.KitapAdi;
            string AktifUye = HttpContext.Session["kullaniciAdi"].ToString();
            var AktifUyeResult = db.Uyeler.Where(i => i.KullaniciAdi == AktifUye).Single();
            var KitapVarmi = db.Kitaplar.Where(k => k.Isim == SeciliKitapAdi).FirstOrDefault();
            var YazarResult = db.Yazarlar.Where(i => i.YazarID == SeciliYazarID).FirstOrDefault();
            //var YazarSoyismi = db.Yazarlar.Where(s => s.Soyisim == SeciliYazarSoyadi).FirstOrDefault();
            //var yazarIdResult = db.Yazarlar.Where(r => r.Isim == SeciliYazarAdi).Where(r => r.Soyisim == SeciliYazarSoyadi).Single();
            yeniUyeKitap.UyeID = AktifUyeResult.UyeID;
            if (KitapVarmi == null)
            {
                if (SeciliYazarID != 0)
                {
                    if (ModelState.IsValid)
                    {
                        yeniKitap.Isim =model.KitapAdi;
                        yeniKitap.Yayinci = model.yayinci;
                        yeniKitap.Aciklama = model.Aciklama;
                        //var UyeFkResult = db.UyelerinKitaplariDb.Where(i => i.UyeID == AktifUyeResult.ID).FirstOrDefault();
                        //yeniKitap.UyeKitapFK = UyeFkResult;
                        //var YazarFkResult = db.YazarlarinKitaplariDb.Where(i => i.YazarID == SeciliYazarID).FirstOrDefault();
                        //yeniKitap.YazarKitapFK = YazarFkResult;

                        yeniYazar.Isim = YazarResult.Isim;
                        yeniYazar.Soyisim = YazarResult.Soyisim;
                        yeniYazar.YazarID = YazarResult.YazarID;
                        yeniYazarKitap.YazarID = YazarResult.YazarID;
                        
                        //db.Yazarlar.Add(yeniYazar);
                        db.Kitaplar.Add(yeniKitap);
                       
                        db.YazarlarinKitaplariDb.Add(yeniYazarKitap);
                        db.UyelerinKitaplariDb.Add(yeniUyeKitap);
                        db.SaveChanges();
                    }

                }
                else
                {
                    return View(ViewBag("Yazar Seçmediniz!"));
                }


            }
            else
            {
                return View(ViewBag("Kitap İsmi Giriniz!"));
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