using KutuphaneCoresuz.Models;
using KutuphaneCoresuz.Models.Context;
using KutuphaneCoresuz.Models.Data;
using KutuphaneCoresuz.Models.TableModels;
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
            Yazar yazarlar = new Yazar();
            // List<string> yazarlistesi = new List<string>();
            List<SelectListItem> yazarlistesi = (from k in db.Yazarlar
                                                 select new SelectListItem
                                                 {
                                                     Text = k.Isim

                                                 }
                ).ToList();
            //ViewBag.yazarlarlistesi = new SelectList(yazarlistesi, "Isim");
            ViewBag.yazarlarlistesi = yazarlistesi;
            return View();

            ;
            //var yazarlar = db.Yazarlar.Select(q => q.Isim).OrderBy(q => q);
            
            //foreach (var item in yazarlar)
            //{
            //    yazarlistesi.Add(item);
            //}
           
        }

        // POST: Kitaps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult CreateKitap([Bind(Include = "kitapID,Isim,Yayinci,Aciklama")]Kitap kitap, Uye uye, Yazar yazar, int yazarID, int uyeID, FormCollection nesneler)
        {

            var seciliYazar = nesneler["yazalarlistesi"];
            if (seciliYazar != null)
            {
                var mevcutKitap = db.Kitaplar.Where(k => k.Isim == kitap.Isim);
                if (mevcutKitap == null)
                {
                    if (ModelState.IsValid)
                    {
                        db.Kitaplar.Add(kitap);
                        db.SaveChanges();
                        var yazarIdResult = db.Yazarlar.Where(r => r.Isim == seciliYazar).Select(r => r.yazarID);
                        //yazar yazarlar tablosunda varsa id sini çek 
                        if (yazarIdResult == null)
                        {
                            //id boş değilse yani yazar yazarlar tablosunda yoksa,
                            //hem yazarlarikitaplarina hem yazar tablosuna eklencek.


                            YazarlarinKitaplari YeniYazar = new YazarlarinKitaplari();
                            YeniYazar.YazarID = Convert.ToInt32(yazarIdResult);
                            YeniYazar.KitapID = kitap.kitapID;
                            db.YazarlarinKitaplariDb.Add(YeniYazar);

                            Yazar YeniYazarT = new Yazar();
                            YeniYazarT.yazarID = Convert.ToInt32(yazarIdResult);
                            db.Yazarlar.Add(YeniYazarT);
                        }
                    }


                }
                return RedirectToAction("IndexKitap");
            }

            return View(kitap);
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
        public ActionResult EditKitap([Bind(Include = "kitapID,k_isim,yazar_adi,uye_id,yazar_id,yayinci,k_aciklama")] Kitap kitap)
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