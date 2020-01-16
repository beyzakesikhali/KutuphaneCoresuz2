﻿using KutuphaneCoresuz.Helper;
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
                kullaniciResult = db.Uyeler.Where(u => u.KullaniciAdi == uye.KullaniciAdi && u.aktiflik == 1).ToList();
                return View(kullaniciResult);

            }

        }

        [AllowAnonymous]
        public ActionResult DetailsUye()
        {
            Uye uye = new Uye();
            if (HttpContext.Session["KullaniciAdi"] != null)
            {

                uye.isim = HttpContext.Session["KullaniciAdi"].ToString();
                if(uye.isim!="Beyza")
                {
                    var uyeIdResult = db.Uyeler.Where(u => u.isim == uye.isim && u.aktiflik == 1).FirstOrDefault();
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
                    return View(uyeIdResult);
                }

                return View();
             
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
                    uye.aktiflik = 1;
                    uye.RoleId = role;
                    db.Uyeler.Add(uye);
                    db.SaveChanges();
                    ViewBag.LoginMesaj = "Başarılı Bir Şekilde Kayıt Oldunuz. Şimdi Giriş Yapınız";
                    return RedirectToAction("Login", "Security");

                }

            }
            else {//kullanici daha önceden varsa
                if (KAdiResult.aktiflik == 0)
                {
                    KAdiResult.aktiflik = 1;
                    KAdiResult.isim = uye.isim;
                    KAdiResult.Soyisim = uye.Soyisim;
                    KAdiResult.Sifre = Crypto.HashPassword(uye.Sifre);
                    KAdiResult.RoleId = 2;
                    KAdiResult.Aciklama = uye.Aciklama;
                    KAdiResult.Email = uye.Email;
                    db.Entry(KAdiResult).State = EntityState.Modified;
                    db.SaveChanges();
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
                var uyeResult = db.Uyeler.Where(u => u.ID == id && u.aktiflik == 1).FirstOrDefault();

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
        public ActionResult DeleteUye()
        {
            string uyeAdi = "";
            uyeAdi = HttpContext.Session["KullaniciAdi"].ToString();
            //kullanıcı isimleri benzersiz
            var uye = db.Uyeler.Where(u => u.KullaniciAdi == uyeAdi).FirstOrDefault();
            if (uyeAdi != null)
            {
                //TempData.Add("eminisin", "Hesabınızı Silmek İstediğinizden Emin Misiz?");
                return View(uye);
            }


            return RedirectToAction("Login", "Security");

        }

        //*******DELETE UYE yani kendi hesabnı silecekse ******/////s
        //Uye hesabını silecekse buraya gelecek......
        [AllowAnonymous]
        [HttpPost]
        public ActionResult DeleteUye(int? id, string isim, string KullaniciAdi, string soyisim)
        {
            string Mevcutisim = HttpContext.Session["KullaniciAdi"].ToString();
            if (Mevcutisim == null)
            {
                return RedirectToAction("Login", "Security");
            }
           Uye uye = db.Uyeler.Where(u => u.ID == id).FirstOrDefault();
           // var KitapDurumu = db.UyeKitap.Where(x => x.UyeID == id).Select(x => x.Kitap.KitapDurum);

            if (uye.isim != null)
            {
                if (uye.ID != 0)
                {
                    if (Convert.ToInt32(id) != 1)
                    {
                        uye.aktiflik = 0;
                        
                        db.Entry(uye).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Login","Security");
                    }

                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                }

            }
            TempData.Add("UyeSilindi", "<b> Hesabınız Silindi</b>");

            return RedirectToAction("Login", "Security");

            //var uyeIdResult = db.Uyeler.Where(u => u.KullaniciAdi == uye.KullaniciAdi && u.aktiflik==1).FirstOrDefault();
            //int uyeId = 0; return RedirectToAction("UyeAnayfasi", "Security");
            //db.Uyeler.Remove(uye);
            //db.SaveChanges();
            //yerine aktifliğini 0 yaparız.
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