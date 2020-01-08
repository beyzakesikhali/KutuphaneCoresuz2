using KutuphaneCoresuz.Controllers.OrtakSiniflar;
using KutuphaneCoresuz.Helper;
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
        /*
         * 
         * 
         * 
         * 
         * COOKİEE
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




            COOKİE

    */

         KutuphaneContext db = new KutuphaneContext();
        [AllowAnonymous]

        public ActionResult Login()
        {
            if (Session["AdminIsLogedIn"] != null)
            {
                return RedirectToAction("index", "AdminProfile");
            }

            return View();
        }
        [AutorizeAdminAttiribute]
        public ActionResult AdminAnasayfasi()
        {


            return View();

        }




        [AllowAnonymous]
        [HttpPost]

        public ActionResult Login(Uye uye)
        {
            SifreKontrol kontrol = new SifreKontrol();

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            var mevcut = db.Uyeler.Where(p => p.KullaniciAdi == uye.KullaniciAdi).FirstOrDefault();
            string gelenSifre = uye.Sifre;
            int sifreKontrol = 0;
            sifreKontrol=kontrol.SifreKontrolEt(gelenSifre,mevcut.Sifre);

            //  IEnumerable<Uye> sonuc = db.Uyeler.Where(x => x.KullaniciAdi == uye.KullaniciAdi && x.Sifre == uye.Sifre);

            if (mevcut != null && sifreKontrol==1)
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
     
        [OutputCache(CacheProfile = "anaSayfaCache")]
        public ActionResult UyeAnasayfasi()
        {

            //SOR SOR SOR
            // Session["kullanciAdi"] = ViewBag.KullaniciAdi;
           
            Uye uye = new Uye();
            Kitap kitap = new Kitap();
            UyeKitap uyeKitap = new UyeKitap();
            Yazar yazarlar = new Yazar();
            if (HttpContext.Session["kullaniciAdi"]==null)
            {
                return Redirect("Login");
            }
            string AktifUye = HttpContext.Session["kullaniciAdi"].ToString();
            List<KitapYazarAddModel> model = new List<KitapYazarAddModel>();
            var uyeResult = db.Uyeler.Where(x => x.KullaniciAdi == AktifUye).FirstOrDefault();
            int uyeID = uyeResult.ID;
            var kitapIdResult = db.UyeKitap.Where(a => a.UyeID == uyeID).Select(a => a.KitapID).ToList();
            if(kitapIdResult.Count()!=0)
            {       foreach (var item in kitapIdResult)
                {
                    var kitapResult = db.Kitaplar.Where(z => z.ID == item).Single();
                    var yazarResult = db.Yazarlar.Where(y => y.ID == kitapResult.YazarID).FirstOrDefault();
                    model.Add(new KitapYazarAddModel() { KitapAdi = kitapResult.Isim , Aciklama = kitapResult.Aciklama, yayinci = kitapResult.Yayinci, YazarAdi = yazarResult.Isim, YazarSoyadi = yazarResult.Soyisim });
                }
                    if(model!=null)
                    {
                         return View(model);
                    }
            }
          
           
            return View();


        }

        public ActionResult UyeKitaplarSayfasi()
        {
            UyeKitap uyeKitap = new UyeKitap();
            Uye uye = new Uye();
            Kitap kitap = new Kitap();

            return View();
        }
        //
        public ActionResult LogOut()
        {
            HttpContext.Session.Remove("KullaniciAdi");
            return RedirectToAction("Login", "Security");

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
