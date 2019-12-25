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
    public class UyeController : Controller
    {
        private KutuphaneContext db = new KutuphaneContext();
        // GET: Uye
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult IndexUye()
        {
            return View(db.Uyeler.ToList());
        }

        // GET: Uyes/Details/5
        [AllowAnonymous]
        public ActionResult DetailsUye(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uye uye = db.Uyeler.Find(id);
            if (uye == null)
            {
                return HttpNotFound();
            }
            return View(uye);
        }

        // GET: Uyes/Create
        [AllowAnonymous]
        public ActionResult CreateUye()
        {
            return View();
        }

        // POST: Uyes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult CreateUye([Bind(Include = "uyeID,isim,kAdi,soyisim,uye_sifre,uye_email,aciklama")] Uye uye)
        {
            if (ModelState.IsValid)
            {
                db.Uyeler.Add(uye);
                db.SaveChanges();
                return RedirectToAction("IndexUye");
            }

            return View(uye);
        }

        // GET: Uyes/Edit/5
        [AllowAnonymous]
        public ActionResult EditUye(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uye uye = db.Uyeler.Find(id);
            if (uye == null)
            {
                return HttpNotFound();
            }
            return View(uye);
        }

        // POST: Uyes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult EditUye([Bind(Include = "uyeID,isim,kAdi,soyisim,uye_sifre,uye_email,aciklama")] Uye uye)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uye).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexUye");
            }
            return View(uye);
        }

        // GET: Uyes/Delete/5
        [AllowAnonymous]
        public ActionResult DeleteUye(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uye uye = db.Uyeler.Find(id);
            if (uye == null)
            {
                return HttpNotFound();
            }
            return View(uye);
        }

        // POST: Uyes/Delete/5
        [AllowAnonymous]
        [HttpPost, ActionName("DeleteUye")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedUye(int id)
        {
            Uye uye = db.Uyeler.Find(id);
            db.Uyeler.Remove(uye);
            db.SaveChanges();
            return RedirectToAction("IndexUye");
        }


    }
}