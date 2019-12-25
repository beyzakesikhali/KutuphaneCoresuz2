using KutuphaneCoresuz.Models;
using KutuphaneCoresuz.Models.Context;
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
        private KutuphaneContext db = new KutuphaneContext();
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
            return View();
        }

        // POST: Kitaps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult CreateKitap([Bind(Include = "kitapID,k_isim,yazar_adi,yayinci,k_aciklama")]Kitap kitap, Uye uye, Yazar yazar,int yazarID, int uyeID, FormCollection nesneler)
        {
            string KAdi = nesneler["k_ismi"].ToString();
            string yazarAdi = nesneler["yazar_adi"].ToString();
            //var yazarSonuc = db.Yazarlar.Where(i => i.yazar_ismi == yazarAdi);
            var result1=from b in db.Yazarlar
                        select new
                        {
                            b.yazarID,
                            b.kitap_id,
                            checked=((from ab in db.))
                        }
            if(KAdi !=null && yazarAdi!=null)
            {
                if (yazarSonuc == null)
                {
                    Kitap kitapYeni = new Kitap();
                    kitapYeni.k_isim = KAdi;

                    

                    db.Kitaplar.Add(kitap);
                    Yazar yeniYazar = new Yazar();
                    yeniYazar.yazar_ismi = yazarAdi;
                   db.Yazarlar.Add(y=>y.yazar_ismi=yazarAdi);
               }
            }
           
                
            if (ModelState.IsValid)
            {
                db.Kitaplar.Add(kitap);
                db.SaveChanges();
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