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
        private ExistControl control = new ExistControl();
        // GET: Kitap
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        // GET: Kitaps
        public ActionResult IndexKitap()
        {
            return View(db.Kitaplar.ToList());
        }

        // GET: Kitaps/Details/5
        [AllowAnonymous]
        public ActionResult DetailsKitap(int? id)
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

            Kitap yeniKitap = new Kitap();
            Yazar yeniYazar = new Yazar();
            YazarlarinKitaplari yeniYazarKitap = new YazarlarinKitaplari();
            UyelerinKitaplari yeniUyeKitap = new UyelerinKitaplari();
            var SeciliYazarAdi = model.YazarAdi;
            var SeciliYazarSoyadi = model.YazarSoyadi;
            var SeciliKitapAdi = model.KitapAdi;
            string AktifUye = HttpContext.Session["kullaniciAdi"].ToString();
            var AktifUyeResult = db.Uyeler.Where(i => i.KullaniciAdi == AktifUye).Single();
            var KitapVarmi = db.Kitaplar.Where(k => k.Isim == SeciliKitapAdi).Single();
            var YazarIsmi = db.Yazarlar.Where(i => i.Isim == SeciliYazarAdi).Single();
            var YazarSoyismi = db.Yazarlar.Where(s => s.Soyisim == SeciliYazarSoyadi).Single();
            var yazarIdResult = db.Yazarlar.Where(r => r.Isim == SeciliYazarAdi).Where(r => r.Soyisim == SeciliYazarSoyadi).Single();
            yeniUyeKitap.UyeID = AktifUyeResult.uyeID;
            if (KitapVarmi == null)
            {
                if (yazarIdResult == null)
                {
                    if (ModelState.IsValid)
                    {
                        yeniKitap.Isim = model.KitapAdi;
                        yeniKitap.Yayinci = model.yayinci;
                        yeniKitap.Aciklama = model.Aciklama;
                        yeniYazar.Isim = model.YazarAdi;
                        yeniYazar.Soyisim = model.YazarSoyadi;
                        yeniYazar.yazarID = yazarIdResult.yazarID;
                        yeniYazarKitap.YazarID = yazarIdResult.yazarID;
                        yeniYazarKitap.KitapID = KitapVarmi.kitapID;

                        yeniUyeKitap.KitapID = KitapVarmi.kitapID;
                        db.Yazarlar.Add(yeniYazar);
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
                return View(ViewBag("Kitap İsmi Giriniz!s"));
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