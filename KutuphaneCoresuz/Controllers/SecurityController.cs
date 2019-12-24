using KutuphaneCoresuz.Models.Context;
using KutuphaneCoresuz.Models.TableModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace KutuphaneCoresuz.Controllers
{
    public class SecurityController : Controller
    {
        private KutuphaneContext db = new KutuphaneContext();
        [AllowAnonymous]
        // GET: Login
        [Authorize]
        public ActionResult Index()
        {
            var model = new Uye();
            //return Redirect(Login);
            return Redirect("Login");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(Uye user)
        {
            var u = UyelerLogin.UyelerInit().FirstOrDefault(x => x.kAdi == user.kAdi && x.uye_sifre == user.uye_sifre);
            if (u != null)
            {
                FormsAuthentication.SetAuthCookie(u.kAdi, false);
                //false -> beni hatırla kısmıyla alakalı
                return Redirect("Yazars/Edit");
            }
            else
            {
                ViewBag.LoginError = "hatalı kullanıcı adı veya şifre";

            }
            return View();

        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult anasayfa()
        {

            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        ///<summary>
        ///
        /// UYE
        /// 
        /// </summary>
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



        /// <summary>
        /// KITAP
        /// </summary>





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
        public ActionResult CreateKitap([Bind(Include = "kitapID,k_isim,yazar_adi,uye_id,yazar_id,yayinci,k_aciklama")] Kitap kitap)
        {
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


        /// <summary>
        /// YAZAR
        /// </summary>
         // GET: Yazars
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

        public ActionResult CreateYazar([Bind(Include = "yazarID,yazar_ismi,yazar_sifre,yazar_email,kitap_id")] Yazar yazar)
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


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

}
