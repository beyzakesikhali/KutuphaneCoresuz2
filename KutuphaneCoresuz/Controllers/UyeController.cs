using KutuphaneCoresuz.Helper;
using KutuphaneCoresuz.Models.Context;
using KutuphaneCoresuz.Models.Data;
using KutuphaneCoresuz.Models.ModelforDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;


namespace KutuphaneCoresuz.Controllers
{
    public class UyeController : Controller
    {
        private KutuphaneContext db = new KutuphaneContext();

        // GET: Uye

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

        [AllowAnonymous]
        public ActionResult DetailsUye(Uye uye)
        {

            if (HttpContext.Session["KullaniciAdi"] != null)
            {

                HttpContext.Session["KullaniciAdi"] = uye.KullaniciAdi;

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
        [AllowAnonymous]
        public ActionResult CreateUye(Uye uye)
        {

            string Kadi = uye.KullaniciAdi;
            string sifre = uye.Sifre;
            string HashDegeri;
            int role = 2;

            var KAdiResult = db.Uyeler.Where(u => u.KullaniciAdi == Kadi).FirstOrDefault();

            if (KAdiResult == null)
            {

                var errors = ModelState.Values.SelectMany(v => v.Errors);
                if (ModelState.IsValid)
                {
                    HashDegeri = Crypto.HashPassword(sifre);
                    uye.Sifre = HashDegeri;
                    db.Uyeler.Add(uye);
                    db.SaveChanges();
                    ViewBag.LoginMesaj = "Başarılı Bir Şekilde Kayıt Oldunuz. Şimdi Giriş Yapınız";
                    return RedirectToAction("Login", "Security");

                }

            }
            ViewBag.KadiMesaj = "Başka bir kullanıcı adı deneyin!";
            return View(uye);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult EditUye(int? id)
        {
            bool basarliMi = true;
            string sonuc = "EditUye";
            List<Uye> model = new List<Uye>();
            try
            {
                if (HttpContext.Session["KullaniciAdi"] == null)

                {
                    sonuc = "Login";
                    return Json(new { ok = basarliMi, text = sonuc });
                }
                //sonuc = "EditUye";
                var uyeResult = db.Uyeler.Where(u => u.ID == id).FirstOrDefault();

                if (uyeResult != null)
                {
                    sonuc = "editUye";
                    model.Add(new Uye() { ID = uyeResult.ID, isim = uyeResult.isim, KullaniciAdi = uyeResult.KullaniciAdi, Soyisim = uyeResult.Soyisim, Email = uyeResult.Email, Sifre = "", Aciklama = uyeResult.Aciklama });
                    return View(model);

                }
                sonuc = "hata";
                return Json(new { ok = basarliMi, text = sonuc });



            }
            catch (Exception)
            {
                basarliMi = false;
                sonuc = "Başarısız İşlem";
                return Json(new { ok = basarliMi, text = sonuc });
                throw;
            }


        }
        [AllowAnonymous]
        public ActionResult PostEditUye()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult PostEditUye(int? id, string gelenSifre)
        {
            bool basariliMi = true;
            int sonuc = 0;// login
            string uyeAdi = "";
            string Dbsifre = "";
            int sifreKontrol = 0;
            SifreKontrol sifreKontrolMetod = new SifreKontrol();
            uyeAdi = HttpContext.Session["KullaniciAdi"].ToString();
            var MevcutKullanici = db.Uyeler.Where(m => m.isim == uyeAdi).FirstOrDefault();
            var EditUye = db.Uyeler.Where(u => u.ID == id).FirstOrDefault();
            Dbsifre = MevcutKullanici.Sifre;
            var uye = db.Uyeler.Where(u => u.ID == id).FirstOrDefault();
            //sifreKontrolMetod.SifreKontrolEt(gelenSifre, Dbsifre);

            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            if (sifreKontrol == 1)
            {


                var errors = ModelState.Values.SelectMany(v => v.Errors);
                if (ModelState.IsValid)
                {
                    sonuc = 1;
                    db.Entry(EditUye).State = EntityState.Modified;
                    db.SaveChanges();
                    return View();



                }
                ViewBag.DbHata = "Db de Hata var";
                return View(ViewBag.DbHata);

            }
            return View();
        }


        [AllowAnonymous]
        public ActionResult DeleteUye(Uye kitapUye)
        {
            string uyeAdi = "";
            uyeAdi = HttpContext.Session["KullaniciAdi"].ToString();
            if (uyeAdi != null)
            {
                return View();
            }


            return RedirectToAction("Login", "Security");

        }

        [AllowAnonymous]
        [HttpPost, ActionName("DeleteUye")]
        public ActionResult DeleteUye(int? id, Uye model)
        {

            if (HttpContext.Session["KullaniciAdi"] == null)
            {
                return RedirectToAction("Login", "Security");
            }
            else
            {
                Uye uye = new Uye();
                HttpContext.Session["KullaniciAdi"] = uye.KullaniciAdi;

                var uyeIdResult = db.Uyeler.Where(u => u.KullaniciAdi == uye.KullaniciAdi).FirstOrDefault();
                //int uyeId = 0;
                id = uyeIdResult.ID;
                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                else
                {
                    db.Uyeler.Remove(uye);
                    db.SaveChanges();
                    return RedirectToAction("UyeAnayfasi", "Security");
                }
            }
        }



        public JsonResult SifreKontrol(KitapUyeViewModel model)
        {
            bool basariliMi = true;
            int code = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    string gelenSifre = model.UyeSifre;
                    var uyeResult = db.Uyeler.Where(u => u.KullaniciAdi == model.KullaniciAdi).Single();
                    if (uyeResult != null && gelenSifre == uyeResult.Sifre)
                    {

                        code = 1; //DeleteUye
                        return Json(new { ok = basariliMi, text = code });//Sifre doğru

                    }
                    code = 2;

                }

                return Json(new { ok = basariliMi, text = code });//sifre yanlış uyarısı 

            }


            catch (Exception)
            {
                basariliMi = false;
                code = 3;
                return Json(new { ok = basariliMi, text = code });//CreateYazarı tekrar açacak kod
            }

        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult AdminKontrol(Uye admin)
        {
            bool basariliMi = true;
            int code = 0;
            string gelenSifre = "";
            string gelenKullaniciAdi = "";
            // gelenKullaniciAdi = HttpContext.Session["KullaniciAdi"].ToString();
            gelenKullaniciAdi = admin.KullaniciAdi;
            int sifreKontrol = 0;
            var result = db.Uyeler.Where(u => u.KullaniciAdi == gelenKullaniciAdi).FirstOrDefault();

            SifreKontrol kontrol = new SifreKontrol();
            sifreKontrol = kontrol.SifreKontrolEt(gelenSifre, result.Sifre);
            try
            {
                if (result != null && sifreKontrol == 1)
                {
                    code = 1;//rol değiştirilecek

                    return Json(new { ok = basariliMi, text = code });

                }
                else
                {
                    code = 2; //hata mesajı
                    return Json(new { ok = basariliMi, text = code });
                }
            }


            catch (Exception)
            {
                basariliMi = false;
                code = 2;
                return Json(new { ok = basariliMi, text = code });//CreateYazarı tekrar açacak kod
            }

        }

    }
}