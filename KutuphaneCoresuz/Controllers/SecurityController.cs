using KutuphaneCoresuz.Models.Context;
using KutuphaneCoresuz.Models.TableModels;
using NUnit.Framework;
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
       
        public ActionResult Login(FormCollection nesneler)
        {
            //string kim="";
            //Session["Kimlik"] = kim.ToString();
            //ViewBag.kimlik = Application["ToplamZiyaretci"].ToString();
            return View();
        }

        [AllowAnonymous]
        [HttpPost]

        public ActionResult Login(Uye user, FormCollection nesneler)
        {
            //string actionName = this.ControllerContext.RouteData.Values["action"].ToString(); 

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

            var u = UyelerLogin.UyelerInit().FirstOrDefault(x => x.KullaniciAdi == user.KullaniciAdi && x.Sifre == user.Sifre);
            if (u != null)
            {
                Session.Add("KullaniciAdi",u.isim.ToString());

                        
                FormsAuthentication.SetAuthCookie(u.KullaniciAdi, false);
                //false -> beni hatırla kısmıyla alakalı
                return RedirectToAction("UyeAnasayfasi","Security");
            }
            else
            {
                ViewBag.LoginError = "hatalı kullanıcı adı veya şifre";

            }
            return View();

        }
        [AllowAnonymous]
        //[HttpGet]
        public ActionResult UyeAnasayfasi()
        {
            Session["KullaniciAdi"]=ViewBag.KullaniciAdi;
            
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
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
