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
            string sifre = uye.Sifre;
            var KAdiResult = db.Uyeler.Where(u=>u.KullaniciAdi==Kadi).FirstOrDefault();
            

            if (KAdiResult == null) 
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                if (ModelState.IsValid)
                {
                    string HashDegeri = Crypto.HashPassword(sifre);
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
        [AllowAnonymous]
        public ActionResult EditUye()
        {
            if (HttpContext.Session["KullaniciAdi"] == null)
            {
                return RedirectToAction("Login", "Security");
            }
            return View();

        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult EditUye(int? id, Uye uye)
        {
            string uyeAdi = "";
           uyeAdi=HttpContext.Session["kullaniciAdi"].ToString();

                var uyeIdResult = db.Uyeler.Where(u => u.KullaniciAdi ==uyeAdi ).FirstOrDefault();
                //int uyeId = 0;
                id = uyeIdResult.ID;
                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //Uye uye = db.Uyeler.Find(id);
                if (uye == null)
                {   
                    return HttpNotFound();
                }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {       db.Entry(uye).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Login","Security");
            }
            return RedirectToAction("Login", "Security");
          
        }

        [AllowAnonymous]
        public ActionResult DeleteUye(KitapUyeViewModel kitapUye)
        {

            Uye uye = new Uye();
            Kitap kitap = new Kitap();
            UyeKitap uyeKitap = new UyeKitap();
            Yazar yazarlar = new Yazar();
            if (HttpContext.Session["kullaniciAdi"] == null)
            {
                return Redirect("Login");
            }
            string AktifUye = HttpContext.Session["kullaniciAdi"].ToString();
            List<KitapUyeViewModel> model = new List<KitapUyeViewModel>();
            var uyeResult = db.Uyeler.Where(x => x.KullaniciAdi == AktifUye).FirstOrDefault();
            int uyeID = uyeResult.ID;
            var kitapIdResult = db.UyeKitap.Where(a => a.UyeID == uyeID).Select(a => a.KitapID).ToList();
            if (kitapIdResult.Count() != 0)
            {
                foreach (var item in kitapIdResult)
                {
                    var kitapResult = db.Kitaplar.Where(z => z.ID == item).Single();
                    var yazarResult = db.Yazarlar.Where(y => y.ID == kitapResult.YazarID).FirstOrDefault();
                    model.Add(new KitapUyeViewModel() {UyeIsim=uyeResult.isim, KullaniciAdi=AktifUye, UyeSoyisim=uyeResult.Soyisim, UyeEmail=uyeResult.Email, Aciklama = uyeResult.Aciklama, KitapAdi=kitapResult.Isim, yayinci = kitapResult.Yayinci, YazarAdi = yazarResult.Isim, YazarSoyadi = yazarResult.Soyisim, YazarYorum=yazarResult.Yorum });
                }
                if (model != null)
                {
                    return View(model);
                }
            }


            return View();

          
                
             
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
                    if(uyeResult!=null && gelenSifre==uyeResult.Sifre)
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



        [AllowAnonymous]
        [HttpPost, ActionName("DeleteUye")]
        public ActionResult DeleteUye(int?id, KitapUyeViewModel model)
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
                    return RedirectToAction("UyeAnayfasi","Security");
                }
            }
        }

    }
}