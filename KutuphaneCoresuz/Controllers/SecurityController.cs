
using KutuphaneCoresuz.Helper;
using KutuphaneCoresuz.Models.Context;
using KutuphaneCoresuz.Models.Data;
using KutuphaneCoresuz.Models.ModelforDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
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
        }"

            COOKİE

    */

        KutuphaneContext db = new KutuphaneContext();
        [AllowAnonymous]


        public ActionResult Login()
        {
            //string sifre = "123456";
            //string sifrele = Crypto.HashPassword(sifre);
            ////Uye admin = new Uye();
            //var admin = db.Uyeler.Where(u => u.KullaniciAdi == "admin" && u.Soyisim == "kesikhalı").FirstOrDefault();

            //db.Uyeler.Add(new Uye
            //{
            //    isim = "Beyza",
            //    KullaniciAdi = "admin",
            //    Soyisim = "Kesikhalı",
            //    Sifre = Crypto.HashPassword("123456789"),
            //    RoleId = 1
            //});

            //db.Uyeler.Remove(admin);
            //db.SaveChanges();
            //aktiflikDuzenle();
            return View();
        }



        [AllowAnonymous]
        [HttpPost]

        public ActionResult Login(Uye uye)
        {
            SifreKontrol kontrol = new SifreKontrol();

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            var mevcut = db.Uyeler.Where(p => p.KullaniciAdi == uye.KullaniciAdi).FirstOrDefault();
            //mevcut.aktiflik = 1;
            //db.Entry(mevcut).State = EntityState.Modified;
            //db.SaveChanges();
            
            string gelenSifre = uye.Sifre;
            int sifreKontrol = 0;
            int gelenRole = Convert.ToInt32(Role.Admin);
            if (mevcut == null)
            {
                TempData["kayitsiz"] = "Kayıtlı Değilsiniz.";
                return View();
            }

            else
            {
                if(mevcut.aktiflik==1)
                {
                    sifreKontrol = kontrol.SifreKontrolEt(gelenSifre, mevcut.Sifre);
                    if (sifreKontrol == 1)
                    {
                        Session.Add("KullaniciAdi", mevcut.KullaniciAdi.ToString());

                        if (mevcut.RoleId == Convert.ToInt32(Role.Admin))
                        {
                            return RedirectToAction("IndexAdmin", "Admin");
                        }
                        HttpContext.Session["KullaniciAdi"] = uye.KullaniciAdi;
                        TempData["loginbasarili"] = "Başarılı";
                        return RedirectToAction("UyeAnaSayfasi", "Security");

                    }
                    else
                    {
                        TempData["loginbasarisiz"] = "hatalı kullanıcı adı veya şifre";
                        return View();
                        //ViewBag.LoginError = "hatalı kullanıcı adı veya şifre";

                    }
                }
            }
 
           

            //  IEnumerable<Uye> sonuc = db.Uyeler.Where(x => x.KullaniciAdi == uye.KullaniciAdi && x.Sifre == uye.Sifre);

            //if (mevcut.KullaniciAdi == uye.KullaniciAdi && sifreKontrol == 1)
            //{

            //FormsAuthentication.SetAuthCookie(uye.KullaniciAdi, false);
            ////false -> beni hatırla kısmıyla alakalı
            //return RedirectToAction("UyeAnasayfasi", "Security");
            //}
            //  else
            // {
            //  
            //  }
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
            int kitapdurum;
            string kitapDurum = "";
            if (HttpContext.Session["KullaniciAdi"] == null)
            {
                return Redirect("Login");
            }
            string AktifUye = HttpContext.Session["KullaniciAdi"].ToString();
            List<KitapYazarAddModel> model = new List<KitapYazarAddModel>();
            var uyeResult = db.Uyeler.Where(x => x.KullaniciAdi == AktifUye).FirstOrDefault();
            int uyeID = uyeResult.ID;
            var kitapIdResult = db.UyeKitap.Where(a => a.UyeID == uyeID).Select(a => a.KitapID).ToList();
            if (kitapIdResult.Count() != 0)
            {
                foreach (var item in kitapIdResult)
                {
                    var kitapResult = db.Kitaplar.Where(z => z.ID == item).Single();
                    kitapdurum = kitapResult.KitapDurum;
                    //burası eklendi aktiflik 1 mi
                    if(kitapResult.aktiflik==1)
                    {
                        if (kitapdurum == 1)
                        {

                            kitapDurum = "Kitap Sizde";

                        }
                        kitapDurum = "Kitap Kutuphanede";

                        var yazarResult = db.Yazarlar.Where(y => y.ID == kitapResult.YazarID).FirstOrDefault();
                        model.Add(new KitapYazarAddModel() { Id = kitapResult.ID, KitapAdi = kitapResult.Isim, Aciklama = kitapResult.Aciklama, yayinci = kitapResult.Yayinci, KitapDurum = kitapDurum, YazarAdi = yazarResult.Isim, YazarSoyadi = yazarResult.Soyisim });

                    }
                    
                }
                if (model != null)
                {
                    return View(model);
                }
            }
            else
            {
                TempData["kitapyok"] = "Hiç Kitabınız Yok";
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
        public void aktiflikDuzenle()
        {
            List<Yazar> yazarlar = db.Yazarlar.ToList();
            List<Kitap> kitaplar = db.Kitaplar.ToList();
            List<Uye> uyeler = db.Uyeler.ToList();


            for (int i = 0; i < kitaplar.Count; i++)
            {
                if(kitaplar[i].aktiflik==0)
                {
                    kitaplar[i].aktiflik = 1;
                    db.Entry(kitaplar[i]).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }

            for (int y = 0; y < yazarlar.Count; y++)
            {
                if (yazarlar[y].aktiflik == 0 || yazarlar[y].Isim!=null)
                {
                    yazarlar[y].aktiflik = 1;
                    db.Entry(yazarlar[y]).State = EntityState.Modified;
                    db.SaveChanges();
                }
                db.Yazarlar.Remove(yazarlar[y]);
                db.SaveChanges();


            }
            for (int u = 0; u < uyeler.Count; u++)
            {
                if (uyeler[u].aktiflik == 0)
                {
                    uyeler[u].aktiflik = 1;
                    db.Entry(uyeler[u]).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
        }

        public List<KitapYazarAddModel> ModeleEkle()
        {
            List<Kitap> kitaplar = db.Kitaplar.ToList();
            List<Yazar> yazarlar = db.Yazarlar.ToList();
            string kitapDurum = "Uyede";
            List<KitapYazarAddModel> liste = new List<KitapYazarAddModel>();
            // kitaplar = db.Kitaplar.ToList();
            for (int k = 0; k < kitaplar.Count; k++)
            {
                    if (kitaplar[k].KitapDurum == 1 && kitaplar[k].aktiflik==1)
                    {
                        kitapDurum = "Kutuphanede";
                        liste.Add(new KitapYazarAddModel { Id = kitaplar[k].ID, KitapAdi = kitaplar[k].Isim, YazarAdi = kitaplar[k].Yazar.Isim, YazarSoyadi = kitaplar[k].Yazar.Soyisim, Aciklama = kitaplar[k].Aciklama, yayinci = kitaplar[k].Yayinci, KitapDurum = kitapDurum, });
                    }


                    kitapDurum = "Uyede";
                    liste.Add(new KitapYazarAddModel { Id = kitaplar[k].ID, KitapAdi = kitaplar[k].Isim, YazarAdi = kitaplar[k].Yazar.Isim, YazarSoyadi = kitaplar[k].Yazar.Soyisim, Aciklama = kitaplar[k].Aciklama, yayinci = kitaplar[k].Yayinci, KitapDurum = kitapDurum, });

                

            }
              
            return liste;
        }


        [AllowAnonymous]

        public ActionResult KitapAra()
        {
            var kitaplar = db.Kitaplar.ToList();
            var yazarlar = db.Yazarlar.ToList();
            string kitapDurum = "";
            string mevcut = HttpContext.Session["KullaniciAdi"].ToString();
            var result = db.Uyeler.Where(u => u.isim == mevcut).FirstOrDefault();
            if (mevcut != null)
            {
                List<KitapYazarAddModel> model = new List<KitapYazarAddModel>();

                foreach (var kitap in kitaplar)
                {
                    if (kitap.KitapDurum == 1 && kitap.aktiflik==1)
                    {
                        kitapDurum = "Kutuphanede";
                        model.Add(new KitapYazarAddModel { Id = kitap.ID, KitapAdi = kitap.Isim, YazarAdi = kitap.Yazar.Isim, YazarSoyadi = kitap.Yazar.Soyisim, Aciklama = kitap.Aciklama, yayinci = kitap.Yayinci, KitapDurum = kitapDurum, });
                    }


                    kitapDurum = "Uyede";
                    model.Add(new KitapYazarAddModel { Id = kitap.ID, KitapAdi = kitap.Isim, YazarAdi = kitap.Yazar.Isim, YazarSoyadi = kitap.Yazar.Soyisim, Aciklama = kitap.Aciklama, yayinci = kitap.Yayinci, KitapDurum = kitapDurum, });

                }

                return View(model);
            }
            else
            {
                TempData["kitaparayok"] = "Sistemde Hiç Kitap Yok";
                return RedirectToAction("Login", "Security");
            }
            //return View();

        }

        //KitapAra ajax
        [HttpPost]
        [AllowAnonymous]
        public PartialViewResult YazarSearch(string searchString)
        {
            //string isim = "beyza";
            List<KitapYazarAddModel> modelListe = new List<KitapYazarAddModel>();
            modelListe = ModeleEkle();
            //modelListe içinde veritabanındaki tüm yazarlar ve onalra ait tüm kitaplar var.

            if (Request.IsAjaxRequest())
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    //var searchedlist = (from list in modelListe where list.KitapAdi.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 || list.Aciklama.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 || list.YazarAdi.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0 select list).ToList();

                    var searchedlist = (from list in modelListe where  list.YazarAdi.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0  || list.YazarSoyadi.IndexOf(searchString,StringComparison.OrdinalIgnoreCase)>=0 select list).ToList();
                    return PartialView("_GridKitapPartialView", searchedlist);
                }
                else
                {
                    return PartialView("_GridKitapPartialView", modelListe);
                }
            }
            else
            {
                return PartialView("_GridKitapPartialView", modelListe);
            }
        }


    }
}
