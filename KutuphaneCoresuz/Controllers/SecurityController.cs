using KutuphaneCoresuz.Models.Context;
using KutuphaneCoresuz.Models.TableModels;

using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        [AllowAnonymous]
        // GET: Login
        [Authorize]
        public ActionResult Index()
        {
            var model = new Uye();
            return View();
            //return RedirectToAction("anasayfa");
        }
        [AllowAnonymous]
        [Authorize]
        public ActionResult Menu()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        
        [AllowAnonymous]
        [HttpPost]

        public ActionResult Login(Uye user, FormCollection nesneler)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString(); 
            string KAdi = nesneler["kAdi"].ToString();
            //böyle bir cookiemiz var mı diye soruyoruz
            if (GetCookie(actionName) == null)
            {
                //yoksa yeni bir cookie oluştuyoruz
                CreateCookie(actionName, KAdi);
            }
            else
            {
                //Ekranda görünmesini sitediğimiz mesajımız
               // ViewBag.Message = "Hoşgeldiniz!"+KAdi;
            }
           

            //if (ModelState.IsValid)
            //{
            //    Uye uyekontrol = new Uye();
            //    var control = UyelerLogin.KullaniciAdKontrol(KAdi);

            //    if (control==true)
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

            var u = UyelerLogin.UyelerInit().FirstOrDefault(x => x.kAdi == user.kAdi && x.uye_sifre == user.uye_sifre);
            if (u != null)
            {
                FormsAuthentication.SetAuthCookie(u.kAdi, false);
                //false -> beni hatırla kısmıyla alakalı
                return RedirectToAction("UyeSayfasi");
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

        public ActionResult UyeSayfasi()
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            //string KAdi = nesneler["kAdi"].ToString();
            //böyle bir cookiemiz var mı diye soruyoruz
            if(GetCookie(actionName) != null)
            {
                //yoksa yeni bir cookie oluştuyoruz
                ViewBag.Message = "Hoşgeldiniz!" + actionName;
            }
            



            return View();
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
