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
    public class UyeController : Controller
    {
        private KutuphaneContext db = new KutuphaneContext();

        // GET: Uye
        public ActionResult IndexAdminUye()
        {
            return View(db.Uyeler.ToList());
        }
        [AllowAnonymous]
        public ActionResult IndexUye(Uye uye)
        {
            HttpContext.Session["KullaniciAdi"] = uye.KullaniciAdi;
            var result = db.Uyeler.Find(uye.KullaniciAdi);
            if (result == null)
            {
                return RedirectToAction("Login", "Security");
            }
            else
            {
                List<Uye> kullaniciResult = new List<Uye>();
                kullaniciResult = db.Uyeler.Where(u => u.KullaniciAdi == uye.KullaniciAdi).ToList();
                return View(kullaniciResult);

            }

        }

        // GET: Uyes/Details/5
        [AllowAnonymous]
        public ActionResult DetailsUye(Uye uye)
        {

            if (HttpContext.Session["KullaniciAdi"] != null)
            {

                HttpContext.Session["kullaniciAdi"] = uye.KullaniciAdi;

                var uyeIdResult = db.Uyeler.Where(u => u.KullaniciAdi == uye.KullaniciAdi).FirstOrDefault();
                int uyeId = 0;
                uyeId = uyeIdResult.ID;
                if (uyeId == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Uye uye = db.Uyeler.Find(id);
                if (uye == null)
                {
                    return HttpNotFound();
                }
                return View(uye);
            }
            else
            {
                return View();
            }


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
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult CreateUye(Uye uye)
        {
            string Kadi = uye.KullaniciAdi;
            var KAdiResult = db.Uyeler.Where(u=>u.KullaniciAdi==Kadi).FirstOrDefault();
            if (KAdiResult == null)
            {
                if (ModelState.IsValid)
                {
                    db.Uyeler.Add(uye);
                    db.SaveChanges();
                    ViewBag.LoginMesaj = "Başarılı Bir Şekilde Kayıt Oldunuz. Şimdi Giriş Yapınız";
                    return RedirectToAction("Login", "Security");

                }

            }
            ViewBag.KadiMesaj = "Başka bir kullanıcı adı deneyin!";
            return View(uye);
        }


        // GET: Uyes/Edit/5  
        [HttpPost]
        [AllowAnonymous]
        public ActionResult EditUye(Uye uye)
        {
            if (HttpContext.Session["KullaniciAdi"] != null)
            {

                HttpContext.Session["kullaniciAdi"] = uye.KullaniciAdi;

                var uyeIdResult = db.Uyeler.Where(u => u.KullaniciAdi == uye.KullaniciAdi).FirstOrDefault();
                int uyeId = 0;
                uyeId = uyeIdResult.ID;
                if (uyeId == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Uye uye = db.Uyeler.Find(id);
                if (uye == null)
                {
                    return HttpNotFound();
                }
                if (ModelState.IsValid)
                {
                    db.Entry(uye).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("IndexUye");
                }
                return View(uye);

            }
            else
            {
                return RedirectToAction("Login", "Security");
            }
        }

        // POST: Uyes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.



        // GET: Uyes/Delete/5
        [AllowAnonymous]
        [HttpPost, ActionName("DeleteUye")]
        public ActionResult DeleteUye()
        {

            if (HttpContext.Session["KullaniciAdi"] == null)
            {
                return RedirectToAction("Login", "Security");
            }
            else
            {
                Uye uye = new Uye();
                HttpContext.Session["kullaniciAdi"] = uye.KullaniciAdi;

                var uyeIdResult = db.Uyeler.Where(u => u.KullaniciAdi == uye.KullaniciAdi).FirstOrDefault();
                int uyeId = 0;
                uyeId = uyeIdResult.ID;
                if (uyeId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
               
                else
                { 
                    db.Uyeler.Remove(uye);
                    db.SaveChanges();
                    return RedirectToAction("UyeAnayfasi","Security");
                }
            }
        }

    }
}