using KutuphaneCoresuz.Models.Context;
using KutuphaneCoresuz.Models.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KutuphaneCoresuz.Controllers
{
    public class YazarController : Controller
    {
        private KutuphaneContext db = new KutuphaneContext();
        // GET: Yazar
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]

        public ActionResult IndexYazar()
        {
            return View(db.Yazarlar.ToList());
        }

        // GET: Yazars/Details/5

        [AllowAnonymous]
        public ActionResult DetailsYazar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yazar yazar = db.Yazarlar.Find(id);
            if (yazar == null)
            {
                return HttpNotFound();
            }
            return View(yazar);
        }

        // GET: Yazars/Create
        [AllowAnonymous]

        public ActionResult CreateYazar()
        {
            return View();
        }

        // POST: Yazars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]

        public ActionResult CreateYazar(Yazar yazar)
        {
            if (ModelState.IsValid)
            {
                db.Yazarlar.Add(yazar);
                db.SaveChanges();
                return RedirectToAction("IndexYazar");
            }

            return View(yazar);
        }

        // GET: Yazars/Edit/5
        [AllowAnonymous]

        public ActionResult EditYazar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yazar yazar = db.Yazarlar.Find(id);
            if (yazar == null)
            {
                return HttpNotFound();
            }
            return View(yazar);
        }



        // POST: Yazars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult EditYazar([Bind(Include = "yazarID,yazar_ismi,yazar_sifre,yazar_email,kitap_id")] Yazar yazar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(yazar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexYazar");
            }
            return View(yazar);
        }

        [AllowAnonymous]

        // GET: Yazars/Delete/5
        public ActionResult DeleteYazar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yazar yazar = db.Yazarlar.Find(id);
            if (yazar == null)
            {
                return HttpNotFound();
            }
            return View(yazar);
        }

        // POST: Yazars/Delete/5
        [HttpPost, ActionName("DeleteYazar")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult DeleteConfirmedYazar(int id)
        {
            Yazar yazar = db.Yazarlar.Find(id);
            db.Yazarlar.Remove(yazar);
            db.SaveChanges();
            return RedirectToAction("IndexYazar");
        }
    }
}