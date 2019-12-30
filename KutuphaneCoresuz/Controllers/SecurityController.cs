using KutuphaneCoresuz.Models.Context;
using KutuphaneCoresuz.Models.Data;
using KutuphaneCoresuz.Models.ModelforDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace KutuphaneCoresuz.Controllers
{
    public class SecurityController : Controller
    {

        private void CreateCookie(string name, string value)
        {
            HttpCookie cookieVisitor = new HttpCookie(name, value);
            // cookieVisitor.Expires = DateTime.Now.AddDays(2);
            Response.Cookies.Add(cookieVisitor);
        }
        private string GetCookie(string name)
        {
            //Böyle bir cookie mevcut mu kontrol ediyoruz
            if (Request.Cookies.AllKeys.Contains(name))
            {
                //böyle bir cookie varsa bize geri değeri döndürsün
                return Request.Cookies[name].Value;
            }
            return null;
        }
        private void DeleteCookie(string name)
        {
            //Böyle bir cookie var mı kontrol ediyoruz
            if (GetCookie(name) != null)
            {
                //Varsa cookiemizi temizliyoruz
                Response.Cookies.Remove(name);
                //ya da 
                Response.Cookies[name].Expires = DateTime.Now.AddDays(-1);
            }
        }



        //public object Application { get; private set; }
        private KutuphaneContext db = new KutuphaneContext();
        [AllowAnonymous]

        public ActionResult Login()
        {
            if (Session["AdminIsLogedIn"] != null)
            {
                return RedirectToAction("index", "AdminProfile");
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]

        public ActionResult Login([Bind(Include = "uyeID,isim,KullaniciAdi,Soyisim,Sifre,Email,Aciklama")] Uye uye)
        {

            bool mevcut = db.Uyeler.Any(p => p.KullaniciAdi == uye.KullaniciAdi);
            //  IEnumerable<Uye> sonuc = db.Uyeler.Where(x => x.KullaniciAdi == uye.KullaniciAdi && x.Sifre == uye.Sifre);

            if (mevcut != false)
            {
                //Session.Add("KullaniciAdi",u.isim.ToString());

                HttpContext.Session["kullaniciAdi"] = uye.KullaniciAdi;
                return RedirectToAction("UyeAnaSayfasi", "Security");
                //FormsAuthentication.SetAuthCookie(uye.KullaniciAdi, false);
                ////false -> beni hatırla kısmıyla alakalı
                //return RedirectToAction("UyeAnasayfasi", "Security");
            }
            else
            {
                ViewBag.LoginError = "hatalı kullanıcı adı veya şifre";

            }
            return View();

        }
        [AllowAnonymous]
        public ActionResult UyeAnasayfasi()
        {
            Session["kullanciAdi"] = ViewBag.KullaniciAdi;
            Uye uye = new Uye();
            Kitap kitap = new Kitap();
            UyelerinKitaplari uyeKitap = new UyelerinKitaplari();
            var uyeResult = db.Uyeler.Where(x => x.KullaniciAdi == Session["kullaniciAdi"].ToString()).Single();
            int uyeID = uyeResult.uyeID;
            var kitapResult = db.UyelerinKitaplariDb.Where(z => z.UyeID == uyeID).Single();
            int kitapID = kitapResult.KitapID;
            var UyeKitapResult = db.UyelerinKitaplariDb.Where(a => a.UyeID == uyeID).Where(a => a.KitapID == kitapID).Single();
            if (UyeKitapResult != null)
            {
                return View(UyeKitapResult);
            }
            return View(ViewBag("Hiç Kitap Eklememişsiniz"));

        }

        public ActionResult UyeKitaplarSayfasi()
        {
            UyelerinKitaplari uyeKitap = new UyelerinKitaplari();
            Uye uye = new Uye();
            Kitap kitap = new Kitap();

            return View();
        }
        //
        public ActionResult LogOut()
        {
            HttpContext.Session.Remove("KullaniciAdi");
            return RedirectToAction("Login", "Security");

            //FormsAuthentication.SignOut();
            //return RedirectToAction("Login");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //    
        //}

    }
}
