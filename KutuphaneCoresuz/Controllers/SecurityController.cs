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

        private KutuphaneContext db = new KutuphaneContext();

        public object Application { get; private set; }

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
            //string actionName = this.ControllerContext.RouteData.Values["action"].ToString(); 

            //if (ModelState.IsValid)
            //{
            //    Uye uyekontrol = new Uye();
            //    var control = UyelerLogin.KullaniciAdKontrol(KAdi);

            //    if (control==true)x=
            //    {

            //        FormsAuthentication.SetAuthCookie(uyekontrol.kAdi, false);
            //        //TempData["HGmesaj"] = "Hoşgeldiniz!   "+KAdi;
            //        return RedirectToAction("anasayfa");

            //    }
            //    else
            //    {
            //        ViewData["hata"] ="Email veya Şifre Hatalı!";
            //    }
            //} 

            //return View();
            // var u = (from s in db.Uyeler where s.KullaniciAdi == uye.KullaniciAdi select s.KullaniciAdi);
            //sor sor sor sor sor 
            bool mevcut = db.Uyeler.Any(p => p.KullaniciAdi == uye.KullaniciAdi);
                //  IEnumerable<Uye> sonuc = db.Uyeler.Where(x => x.KullaniciAdi == uye.KullaniciAdi && x.Sifre == uye.Sifre);

            if (mevcut != false)
            {
                //Session.Add("KullaniciAdi",u.isim.ToString());

                HttpContext.Session["kullaniciAdi"] = uye.KullaniciAdi;
                return RedirectToAction("UyeAnaSayfasi", "Security");
                FormsAuthentication.SetAuthCookie(uye.KullaniciAdi, false);
                //false -> beni hatırla kısmıyla alakalı
                return RedirectToAction("UyeAnasayfasi", "Security");
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
            UyelerinKitaplari uyeKitap = new UyelerinKitaplari();
            //var uyeID = db.Uyeler.Where(x => x.KullaniciAdi == Session["kullaniciAdi"].ToString()).Select(x => x.uyeID);
            //if(uyeID!=null)
            //{
            //    var uyeKitapIdResult = db.UyelerinKitaplariDb.Find();
            //}
            ////int UyeID = Convert.ToInt32(uyeID);
            ////uye tablosundaki uye id getirildi 


            //if(uyeKitapIdResult!=null)
            //{
            //    Kitap kitap = db.Kitaplar.Find(Convert.ToInt32(uyeKitapIdResult));
            //    if (kitap == null)
            //    {

            //        return View(ViewBag( "Hiç Kitap Eklmemişsiniz!"));
            //    }
            //    return View(kitap);
            //}

            return View();
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
            Session["AdminIsLogedIn"] = null;
            return RedirectToAction("Login", "Account");

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
