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


//https://www.codeproject.com/Articles/1110431/Gridview-in-ASP-NET-MVC
namespace KutuphaneCoresuz.Controllers
{

    public class AdminController : Controller
    {

        private KutuphaneContext db = new KutuphaneContext();


        [AllowAnonymous]
        public ActionResult IndexAdmin()
        {
            string role = "";

            string AktifUye = "";
            // ViewBag.Login = "Nparsınız ya Allasen Giriş yapmadan Uye silmeye kalkıyosunuz!";
            //AktifUye = HttpContext.Session["KullaniciAdi"].ToString();
            if (HttpContext.Session["KullaniciAdi"] == null)
            {

                //ViewBag.Login = "Nparsınız ya Allasen Giriş yapmadan Uye silmeye kalkıyosunuz!";
                return RedirectToAction("Login", "Security");

            }
            AktifUye = HttpContext.Session["KullaniciAdi"].ToString();
            List<Uye> model = new List<Uye>();
            var uyeResult = db.Uyeler.ToList();
            // var uyeler = db.Uyeler.Single();
            // int uyeID = uyeResult.ID;
            // var kitapIdResult = db.UyeKitap.Where(a => a.UyeID == uyeID).Select(a => a.KitapID).ToList();
            if (uyeResult.Count() != 0)
            {
                foreach (var item in uyeResult)
                {
                    if (item.aktiflik == 1)
                    {
                        model.Add(new Uye() { ID = item.ID, isim = item.isim, KullaniciAdi = item.KullaniciAdi, Soyisim = item.Soyisim, Sifre = item.Sifre, Email = item.Email, Aciklama = item.Aciklama });

                    }

                    // var kitapResult = db.Kitaplar.Where(z => z.ID == item).Single();

                    //if (item.RoleId == 1)
                    //{
                    //    role = "Admin";
                    //    model.Add(new Uye() { id=item.ID, UyeIsim = item.isim, KullaniciAdi = item.KullaniciAdi, UyeSoyisim = item.Soyisim, UyeSifre = item.Sifre, UyeEmail = item.Email, Aciklama = item.Aciklama, Role = role });

                    //}
                    //role = "Uye";

                    //var yazarResult = db.Yazarlar.Where(y => y.ID == kitapResult.YazarID).FirstOrDefault();

                }
                if (model != null)
                {
                    return View(model);
                }
            }


            return View();
        }

        [AllowAnonymous]
        public ActionResult IndexUye()
        {
            string role = "";

            string AktifUye = "";
            // ViewBag.Login = "Nparsınız ya Allasen Giriş yapmadan Uye silmeye kalkıyosunuz!";
            //AktifUye = HttpContext.Session["KullaniciAdi"].ToString();
            if (HttpContext.Session["KullaniciAdi"] == null)
            {

                //ViewBag.Login = "Nparsınız ya Allasen Giriş yapmadan Uye silmeye kalkıyosunuz!";
                return RedirectToAction("Login", "Security");

            }
            AktifUye = HttpContext.Session["KullaniciAdi"].ToString();
            List<Uye> model = new List<Uye>();
            var uyeResult = db.Uyeler.ToList();
            // var uyeler = db.Uyeler.Single();
            // int uyeID = uyeResult.ID;
            // var kitapIdResult = db.UyeKitap.Where(a => a.UyeID == uyeID).Select(a => a.KitapID).ToList();
            if (uyeResult.Count() != 0)
            {
                foreach (var item in uyeResult)
                {
                    if (item.aktiflik == 1)
                    {
                        model.Add(new Uye() { ID = item.ID, isim = item.isim, KullaniciAdi = item.KullaniciAdi, Soyisim = item.Soyisim, Sifre = item.Sifre, Email = item.Email, Aciklama = item.Aciklama });

                    }

                    // var kitapResult = db.Kitaplar.Where(z => z.ID == item).Single();

                    //if (item.RoleId == 1)
                    //{
                    //    role = "Admin";
                    //    model.Add(new Uye() { id=item.ID, UyeIsim = item.isim, KullaniciAdi = item.KullaniciAdi, UyeSoyisim = item.Soyisim, UyeSifre = item.Sifre, UyeEmail = item.Email, Aciklama = item.Aciklama, Role = role });

                    //}
                    //role = "Uye";

                    //var yazarResult = db.Yazarlar.Where(y => y.ID == kitapResult.YazarID).FirstOrDefault();


                }
                if (model != null)
                {
                    return View(model);
                }
            }


            return View();
        }

     
        //uye ekleme ****** UYE EKLEME
        [AllowAnonymous]
        public ActionResult AddUyeAdmin()
        {
            return View();
        }

        // POST: Uyes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        public ActionResult AddUyeAdmin(Uye uye)
        {

            string Kadi = uye.KullaniciAdi;
            string sifre = uye.Sifre;
            string HashDegeri;
            int role = 2;

            var KAdiResult = db.Uyeler.Where(u => u.KullaniciAdi == Kadi).FirstOrDefault();

            if (KAdiResult == null)//uye asla kayıtlı değilse
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                if (ModelState.IsValid)
                {
                    HashDegeri = Crypto.HashPassword(sifre);
                    uye.Sifre = HashDegeri;
                    uye.aktiflik = 1;
                    db.Uyeler.Add(uye);
                    db.SaveChanges();
                    ViewBag.UyeKaydedildi = "Başarılı Bir Şekilde Kayıt Oldunuz. Şimdi Giriş Yapınız";
                    return RedirectToAction("IndexAdmin", "Admin");

                }

            }
            else if (KAdiResult != null && KAdiResult.aktiflik == 0)
            {
                KAdiResult.aktiflik = 1;
                KAdiResult.isim = uye.isim;
                KAdiResult.KullaniciAdi = uye.Soyisim;
                KAdiResult.Soyisim = uye.Soyisim;
                KAdiResult.Aciklama = uye.Aciklama;
                KAdiResult.Email = uye.Email;
                KAdiResult.Sifre = Crypto.HashPassword(uye.Sifre);
                db.Entry(KAdiResult).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexAdmin", "Admin");


            }
            ViewBag.KadiMesaj = "Bu üye zaten var!";
            return View(uye);
        }

        //****** ADD UYE SONU *****

        // ******** EDİT UYE *******
        [HttpPost]
        [AllowAnonymous]

        public JsonResult AdminEditUye(int? id, string isim)
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
                    //return View(model);

                    return Json(new { ok = basarliMi, text = sonuc });

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
        public ActionResult AdminEditUyeResult(int? id)
        {
            List<Uye> model = new List<Uye>();
            // string gelneisim = isim;

            var uyeResult = db.Uyeler.Where(u => u.ID == id).FirstOrDefault();
            if (uyeResult.aktiflik == 1)
            {
                if (uyeResult.RoleId == 0)
                {

                    model.Add(new Uye() { ID = uyeResult.ID, isim = uyeResult.isim, KullaniciAdi = uyeResult.KullaniciAdi, Soyisim = uyeResult.Soyisim, Email = uyeResult.Email, Sifre = "", Aciklama = uyeResult.Aciklama, RoleId = 0 });

                }
            }
            else
            {

                model.Add(new Uye() { ID = uyeResult.ID, isim = uyeResult.isim, KullaniciAdi = uyeResult.KullaniciAdi, Soyisim = uyeResult.Soyisim, Email = uyeResult.Email, Sifre = "", Aciklama = uyeResult.Aciklama, RoleId = 1 });

            }
            return View(model);

        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult AdminEditUyeEdit(int? id, string isim, string kullaniciadi, string soyisim, string email, string aciklama)
        {

            string uyeAdi = "";
            string Dbsifre = "";
            int sifreKontrol = 0;
            SifreKontrol sifreKontrolMetod = new SifreKontrol();
            uyeAdi = HttpContext.Session["KullaniciAdi"].ToString();
            var MevcutKullanici = db.Uyeler.Where(m => m.isim == uyeAdi && m.KullaniciAdi == "admin").FirstOrDefault();
            //var EditUye = db.Uyeler.Where(u => u.ID == id).FirstOrDefault();
            Dbsifre = MevcutKullanici.Sifre;
            var uye = db.Uyeler.Where(u => u.ID == id).FirstOrDefault();
            Uye editUye = new Uye();
            editUye.isim = isim;
            editUye.Soyisim = soyisim;
            editUye.KullaniciAdi = kullaniciadi;
            editUye.Email = email;
            editUye.Aciklama = aciklama;


            //sifreKontrolMetod.SifreKontrolEt(gelenSifre, Dbsifre);
            if (MevcutKullanici != null && uye != null)
            {


                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                //if (sifreKontrol == 1)
                //{


                var errors = ModelState.Values.SelectMany(v => v.Errors);
                if (ModelState.IsValid)
                {


                    // db.Uyeler.Where(u => u.ID == id).Select(u => { u.isim = isim; u.KullaniciAdi = kullaniciadi; u.Soyisim = soyisim; u.Email = email; u.Aciklama = aciklama; });
                    uye.isim = isim;
                    uye.Soyisim = soyisim;
                    uye.KullaniciAdi = kullaniciadi;
                    uye.Email = email;
                    uye.Aciklama = aciklama;
                    db.Entry(uye).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("IndexAdmin", "Admin");

                }
                ViewBag.DbHata = "Db de Hata var";
                return View(ViewBag.DbHata);
                //}

            }

            else
            {
                ViewBag.terslik = "Bir şeyler ters gitti";
                return View(ViewBag.Sifre);



            }

            // return View();
        }


        //RoleEdit



        //****** UYE EDit SOnu
        //****** UYE EDit SOnu

        //ADMİN ADD UYE KİTAP
        //ADMİN ADD UYE KİTAP//ADMİN ADD UYE KİTAP//ADMİN ADD UYE KİTAP//ADMİN ADD UYE KİTAP//ADMİN ADD UYE KİTAP


        //****** Uyeye Kitap Ekle kısmı/******
        [AllowAnonymous]
        public ActionResult AdminUyeKitapView(int id)
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

            int uyeID = id;
            var kitapIdResult = db.UyeKitap.Where(a => a.UyeID == uyeID).Select(a => a.KitapID).ToList();
            if (kitapIdResult.Count() != 0)
            {
                foreach (var item in kitapIdResult)
                {
                    var kitapResult = db.Kitaplar.Where(z => z.ID == item).Single();
                    kitapdurum = kitapResult.KitapDurum;
                    if (kitapdurum == 1)
                    {

                        kitapDurum = "Kitap Uyede";

                    }
                    kitapDurum = "Kitap Kutuphanede";

                    var yazarResult = db.Yazarlar.Where(y => y.ID == kitapResult.YazarID).FirstOrDefault();
                    model.Add(new KitapYazarAddModel() { KitapAdi = kitapResult.Isim, Aciklama = kitapResult.Aciklama, yayinci = kitapResult.Yayinci, KitapDurum = kitapDurum, YazarAdi = yazarResult.Isim, YazarSoyadi = yazarResult.Soyisim });
                }
                if (model != null)
                {
                    return View(model);
                }
            }


            return View();


        }



        public void UyeyeKitapEkle(int id, KitapUyeViewModel model)
        {
            KitapYazarAddModel kitapYazarAddModel = new KitapYazarAddModel();
            List<SelectListItem> modelAdList = new List<SelectListItem>();
            List<SelectListItem> modelSoyadList = new List<SelectListItem>();
            Kitap yeniKitap = new Kitap();
            UyeKitap yeniUyeKitap = new UyeKitap();
            //select option yapısından dolayı Id direk gelir.
            if (model.YazarAdi == "Yazar Adı Seçiniz" || model.YazarAdi == null && model.KitapAdi == null)
            {
                ViewBag.GirisHata = "Yazar Adı Uye Adı yada Kitap Adı Boş Geçilemez!";

            }
            int SeciliYazarID = Convert.ToInt32(model.YazarAdi);
            var SeciliKitapAdi = model.KitapAdi;

            var KitapEklenecekUye = db.Uyeler.Where(u => u.ID == id).FirstOrDefault();
            //string AktifUye = HttpContext.Session["KullaniciAdi"].ToString();
            //var AktifUyeResult = db.Uyeler.Where(i => i.KullaniciAdi == AktifUye).Single();
            var KitapVarmi = db.Kitaplar.Where(k => k.Isim == SeciliKitapAdi).FirstOrDefault();
            var YazarResult = db.Yazarlar.Where(i => i.ID == SeciliYazarID).FirstOrDefault();
            Yazar yazarinKitaplari = new Yazar();
            if (KitapVarmi == null)//kitap ilk defa eklenecekse
            {
                if (ModelState.IsValid)
                {
                    //önce kitap tablosu
                    yeniKitap.Isim = model.KitapAdi;
                    yeniKitap.Yayinci = model.yayinci;
                    yeniKitap.Aciklama = model.Aciklama;
                    yeniKitap.aktiflik = 1;
                    yeniKitap.YazarID = SeciliYazarID;
                    yeniKitap.Yazar = YazarResult;
                    if (model.KitapDurum == "Kutuphanede")
                    {
                        yeniKitap.KitapDurum = 1;

                    }
                    else
                    {
                        yeniKitap.KitapDurum = 2;
                    }

                    yeniUyeKitap.UyeID = KitapEklenecekUye.ID;
                    yeniUyeKitap.KitapID = yeniKitap.ID;
                    yeniUyeKitap.Kitap = yeniKitap;

                    if (yeniKitap.KitapDurum == 1)
                    {
                        yeniUyeKitap.aktiflik = 0;
                    }
                    else
                    {
                        yeniUyeKitap.aktiflik = 1;
                    }
                    yeniUyeKitap.Uye = KitapEklenecekUye;

                    yeniUyeKitap.Uye = KitapEklenecekUye;

                    db.Kitaplar.Add(yeniKitap);
                    db.SaveChanges();
                    //sonra FK lardan herhangi biri olan YazarKitap
                    db.UyeKitap.Add(yeniUyeKitap);
                    db.SaveChanges();
                    kitapYazarAddModel.YazarAdi = yeniKitap.Yazar.Isim;
                    kitapYazarAddModel.YazarSoyadi = yeniKitap.Yazar.Soyisim;
                    kitapYazarAddModel.YazarYorum = yeniKitap.Yazar.Yorum;
                    //yazarinKitaplari.Kitaplar.Add(new Kitap { ID = yeniKitap.ID, YazarID = YazarResult.ID, Isim = yeniKitap.Isim, Yazar = yeniKitap.Yazar, Yayinci = yeniKitap.Yayinci, Aciklama = yeniKitap.Aciklama, KitapDurum = yeniKitap.KitapDurum });



                }
            }
            else
            {
                //kitap zaten varsa Fk yı ekle direk
                //Kitapvarmi üzerinden eklenmeli


                yeniUyeKitap.UyeID = id;
                yeniUyeKitap.KitapID = KitapVarmi.ID;
                yeniUyeKitap.Kitap = KitapVarmi;
                yeniUyeKitap.Uye = KitapEklenecekUye;
                db.UyeKitap.Add(yeniUyeKitap);
                db.SaveChanges();

            }



        }
    


    [AllowAnonymous]
    public ActionResult AdminAddUyeKitap(int? id)
    {
        KitapUyeViewModel model = new KitapUyeViewModel();
        //ViewBag.seciliUyeID=id;
        model.id = Convert.ToInt32(id);
        return View(model);


    }




    [OutputCache(CacheProfile = "anaSayfaCache")]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    [HttpPost]
    public ActionResult AdminAddUyeKitap(int? id, KitapUyeViewModel model)
    {

        int seciliUyeId = 0;
        //int gelenUyeId = Convert.ToInt32(ViewBag.seciliUyeID);

        var seciliUyeResult = db.Uyeler.Where(u => u.ID == id).FirstOrDefault();
        seciliUyeId = seciliUyeResult.ID;
        if (seciliUyeResult != null)
        {
            //seciliUyeId = seciliUyeResult.ID;
            if (model.id != seciliUyeResult.ID)
            {
                UyeyeKitapEkle(seciliUyeId, model);
            }
            else
            {
                UyeyeKitapEkle(model.id, model);
            }

        }

        //UyeyeKitapEkle(model.id, model);

        //  return View(model)
        return RedirectToAction("UyeDetailsResult", "Admin");


       



    }

    //yazar adsoyad getir
    [HttpPost]
    [AllowAnonymous]
    public JsonResult AdSoyad(int? id, string tip = "yazarAdGetir")
    {
        List<Yazar> yazarlarListesi = new List<Yazar>();
        yazarlarListesi = db.Yazarlar.ToList();
        List<SelectListItem> sonuc = new List<SelectListItem>();
        bool basariliMi = true;
        //string yazarlar = "";
        try
        {
            switch (tip)
            {
                case "yazarAdGetir":
                    foreach (var ad in yazarlarListesi)
                    {

                        sonuc.Add(new SelectListItem
                        {
                            Text = ad.Isim + " " + ad.Soyisim,
                            Value = ad.ID.ToString()
                        });

                    }

                    break;

                default:
                    break;
            }

        }

        catch (Exception)
        {
            basariliMi = false;
            sonuc = new List<SelectListItem>();
            sonuc.Add(new SelectListItem
            {
                Text = "Bir hata oluştu!",
                Value = "default"
            });

        }

        return Json(new { ok = basariliMi, text = sonuc });
    }
    //uye adsoyad getir
    [HttpPost]
    [AllowAnonymous]
    public JsonResult UyeAdSoyad(int? id, string tip = "uye")
    {
        List<Uye> uyelerListesi = new List<Uye>();
        uyelerListesi = db.Uyeler.ToList();
        List<SelectListItem> sonuc = new List<SelectListItem>();
        bool basariliMi = true;
        //string yazarlar = "";
        try
        {
            switch (tip)
            {
                case "uye":
                    foreach (var ad in uyelerListesi)
                    {

                        sonuc.Add(new SelectListItem
                        {
                            Text = ad.isim + " " + ad.Soyisim,
                            Value = ad.ID.ToString()
                        });

                    }

                    break;

                default:
                    break;
            }

        }

        catch (Exception)
        {
            basariliMi = false;
            sonuc = new List<SelectListItem>();
            sonuc.Add(new SelectListItem
            {
                Text = "Bir hata oluştu!",
                Value = "default"
            });

        }

        return Json(new { ok = basariliMi, text = sonuc });
    }




    //****** Uyeye Kitap Ekle kısmı SONUUUU/******

    //deleteuye kısmı 

    [HttpPost]
    [AllowAnonymous]

    public JsonResult AdminDeleteUye(int? id)
    {

        bool basarliMi = true;
        string sonuc = "DeleteUye";
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
                sonuc = "DeleteUye";
                model.Add(new Uye() { ID = uyeResult.ID, isim = uyeResult.isim, KullaniciAdi = uyeResult.KullaniciAdi, Soyisim = uyeResult.Soyisim, Email = uyeResult.Email, Sifre = "", Aciklama = uyeResult.Aciklama });
                //return View(model);

                return Json(new { ok = basarliMi, text = sonuc });

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
    public ActionResult AdminDeleteUyeResult(int? id)
    {
        List<Uye> model = new List<Uye>();
        string mevcutKullanici = "";
        mevcutKullanici = HttpContext.Session["KullaniciAdi"].ToString();
        if (mevcutKullanici?.ToString() == null)
        {
            RedirectToAction("Login", "Security");
        }
        var uyeResult = db.Uyeler.Where(u => u.ID == id).FirstOrDefault();

        model.Add(new Uye() { ID = uyeResult.ID, isim = uyeResult.isim, KullaniciAdi = uyeResult.KullaniciAdi, Soyisim = uyeResult.Soyisim, Email = uyeResult.Email, Aciklama = uyeResult.Aciklama });
        return View(model);

    }

    [HttpPost]
    [AllowAnonymous]
    public ActionResult AdminDeleteUyeResult(int? id, string isim)
    {
        if (HttpContext.Session["KullaniciAdi"] == null)
        {
            return RedirectToAction("Login", "Security");
        }
        else
        {
            Uye uye = db.Uyeler.Find(id);
            HttpContext.Session["KullaniciAdi"] = uye.KullaniciAdi;

            //var deleteUye = db.Uyeler.Where(u => u.ID == id).FirstOrDefault();
            //int uyeId = 0;
            //  id = uyeIdResult.ID;
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            else

            {
                var UyeninkitaplariIDs = db.UyeKitap.Where(u => u.UyeID == id).Select(u => u.KitapID).ToList();
                if (UyeninkitaplariIDs != null)
                {
                    foreach (var item in UyeninkitaplariIDs)
                    {
                        var kitaplar = db.Kitaplar.Where(k => k.ID == item).FirstOrDefault();
                        if (kitaplar.KitapDurum == 2)
                        {
                            return View("Uyeye Ait Kitap var Silemezsiniz!");
                        }
                        else
                        {
                            uye.aktiflik = 0;
                            db.Entry(uye).State = EntityState.Modified;
                            db.SaveChanges();

                            //db.Uyeler.Remove(deleteUye);
                            //db.SaveChanges();
                            return RedirectToAction("IndexAdmin", "Admin");
                        }
                    }
                }

                uye.aktiflik = 0;
                db.Entry(uye).State = EntityState.Modified;
                db.SaveChanges();

                //db.Uyeler.Remove(deleteUye);
                //db.SaveChanges();
                return RedirectToAction("IndexAdmin", "Admin");


            }
        }

    }
    /*
     * 
     * 
     * 
     * 
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



     * */

    //addKitap addKitap AddKitap,,
    [OutputCache(CacheProfile = "anaSayfaCache")]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    [HttpPost]
    public ActionResult AddKitap(KitapYazarAddModel model)
    {
        //dropdown doldurmak için
        KitapYazarAddModel kitapYazarAddModel = new KitapYazarAddModel();
        List<SelectListItem> modelAdList = new List<SelectListItem>();
        List<SelectListItem> modelSoyadList = new List<SelectListItem>();
        Kitap yeniKitap = new Kitap();
        UyeKitap yeniUyeKitap = new UyeKitap();
        int SeciliYazarID = Convert.ToInt32(model.YazarAdi);
        var SeciliKitapAdi = model.KitapAdi;
        string AktifUye = HttpContext.Session["KullaniciAdi"].ToString();
        var AktifUyeResult = db.Uyeler.Where(i => i.isim == AktifUye).FirstOrDefault();
        var KitapVarmi = db.Kitaplar.Where(k => k.Isim == SeciliKitapAdi).FirstOrDefault();
        var YazarResult = db.Yazarlar.Where(i => i.ID == SeciliYazarID).FirstOrDefault();
        if (KitapVarmi == null)
        {
            if (SeciliYazarID != 0)
            {
                if (ModelState.IsValid)
                {
                    //önce kitap tablosu
                    yeniKitap.Isim = model.KitapAdi;
                    yeniKitap.Yayinci = model.yayinci;
                    yeniKitap.Aciklama = model.Aciklama;
                    yeniKitap.YazarID = SeciliYazarID;
                    yeniKitap.Yazar = YazarResult;

                    yeniUyeKitap.UyeID = AktifUyeResult.ID;
                    yeniUyeKitap.KitapID = yeniKitap.ID;
                    yeniUyeKitap.Kitap = yeniKitap;
                    yeniUyeKitap.Uye = AktifUyeResult;

                    db.Kitaplar.Add(yeniKitap);
                    db.SaveChanges();

                    db.UyeKitap.Add(yeniUyeKitap);
                    db.SaveChanges();
                    kitapYazarAddModel.YazarAdi = yeniKitap.Yazar.Isim;
                    kitapYazarAddModel.YazarSoyadi = yeniKitap.Yazar.Soyisim;
                    kitapYazarAddModel.YazarYorum = yeniKitap.Yazar.Yorum;


                }

            }
            else
            {
                return View(ViewBag("Yazar Seçmediniz!"));
            }
        }

        else
        {
            yeniUyeKitap.UyeID = AktifUyeResult.ID;
            yeniUyeKitap.KitapID = KitapVarmi.ID;
            yeniUyeKitap.Kitap = KitapVarmi;
            //yeniUyeKitap.Uye = AktifUyeResult;
            //sadece kitap ekleneceği için burası yorum

            db.UyeKitap.Add(yeniUyeKitap);
            db.SaveChanges();

        }
        return View();

    }


    //add Kitap sonu


    //*/*******UYE DETAY ****////


    [AllowAnonymous]
    [HttpPost]
    public JsonResult UyeDetails(int? id)
    {

        bool basarliMi = true;
        string sonuc = "UyeDetay";
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
                sonuc = "UyeDetay";
                model.Add(new Uye() { ID = uyeResult.ID, isim = uyeResult.isim, KullaniciAdi = uyeResult.KullaniciAdi, Soyisim = uyeResult.Soyisim, Email = uyeResult.Email, Sifre = "", Aciklama = uyeResult.Aciklama });
                //return View(model);

                return Json(new { ok = basarliMi, text = sonuc });

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
    public ActionResult UyeDetailsResult(int? id)
    {
        List<KitapUyeViewModel> model = new List<KitapUyeViewModel>();
        string mevcutKullanici = "";
        string kitapdurum = "";
        string yazarYok = "Kitap Olmadığı İçin Yazar Yok";
        string kitapyok = "Kayıtlı Kitabınız Yok";
        HttpContext.Session["KullaniciAdi"] = mevcutKullanici;
        if (mevcutKullanici?.ToString() == null)
        {
            RedirectToAction("Login", "Security");
        }
        var uyeResult = db.Uyeler.Where(u => u.ID == id).FirstOrDefault();
        //uyeye ait kitap idlerinin listesi
        var kitapIdResult = db.UyeKitap.Where(k => k.UyeID == id).ToList();

        List<Kitap> uyeninkitaplari = new List<Kitap>();
        if (uyeResult.UyeKitaplar.Count == 0)
        {
            model.Add(new KitapUyeViewModel() { id = uyeResult.ID, UyeIsim = uyeResult.isim, KullaniciAdi = uyeResult.KullaniciAdi, UyeSoyisim = uyeResult.Soyisim, UyeEmail = uyeResult.Email, Aciklama = uyeResult.Aciklama, KitapDurum = kitapyok, KitapAdi = kitapyok, yayinci = kitapyok, YazarAdi = yazarYok, YazarSoyadi = yazarYok, YazarYorum = yazarYok });

        }
        else
        {
            var ids = kitapIdResult.Select(q => q.KitapID).ToList();
            uyeninkitaplari = db.Kitaplar.Where(k => ids.Contains(k.ID)).ToList();

            foreach (var item in uyeninkitaplari)
            {
                if (item.KitapDurum == 1)
                {
                    kitapdurum = "Kütüphanede";
                    model.Add(new KitapUyeViewModel() { id = uyeResult.ID, UyeIsim = uyeResult.isim, KullaniciAdi = uyeResult.KullaniciAdi, Role = "Uye", UyeSoyisim = uyeResult.Soyisim, UyeEmail = uyeResult.Email, Aciklama = uyeResult.Aciklama, KitapDurum = kitapdurum, KitapAdi = item.Isim, yayinci = item.Yayinci, YazarAdi = item.Yazar.Isim, YazarSoyadi = item.Yazar.Soyisim, YazarYorum = item.Yazar.Yorum }); ;

                }
                kitapdurum = "Üyede";
                model.Add(new KitapUyeViewModel() { id = uyeResult.ID, UyeIsim = uyeResult.isim, KullaniciAdi = uyeResult.KullaniciAdi, Role = "Uye", UyeSoyisim = uyeResult.Soyisim, UyeEmail = uyeResult.Email, Aciklama = uyeResult.Aciklama, KitapDurum = kitapdurum, KitapAdi = item.Isim, yayinci = item.Yayinci, YazarAdi = item.Yazar.Isim, YazarSoyadi = item.Yazar.Soyisim, YazarYorum = item.Yazar.Yorum });


            }
        }


        return View(model);
    }

    //[HttpPost]
    //[AllowAnonymous]
    //public ActionResult UyeDetailsResult(int? id, string isim)
    //{
    //    if (HttpContext.Session["KullaniciAdi"] == null)
    //    {
    //        return RedirectToAction("Login", "Security");
    //    }
    //    else
    //    {
    //        Uye uye = new Uye();
    //        HttpContext.Session["KullaniciAdi"] = uye.KullaniciAdi;

    //        var deleteUye = db.Uyeler.Where(u => u.ID == id).FirstOrDefault();
    //        //int uyeId = 0;
    //        //  id = uyeIdResult.ID;
    //        if (id == 0)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }

    //        else
    //        {
    //            db.Uyeler.Remove(deleteUye);
    //            db.SaveChanges();
    //            return RedirectToAction("IndexAdmin", "Admin");
    //        }
    //    }

    //}
    [AllowAnonymous]
    [HttpPost]
    public JsonResult RoleEdit(string kullaniciadi, string sifre, string role)
    {
        bool basariliMi = true;
        int code = 1;//Sifre kontrolü doğru role değiştirebilir.
        string gelensifre = "";
        string gelenKullaniciAdi = "";
        string dbSifre = "";



        if (ModelState.IsValid)
        {
            //foreach (var item in Uye)
            //{
            //    sifre = item.Sifre;
            //    KullaniciAdi = item.KullaniciAdi;
            //}
            gelensifre = sifre;
            gelenKullaniciAdi = kullaniciadi;

            SifreKontrol sifreKontrol = new SifreKontrol();
            var adminMi = db.Uyeler.Where(u => u.KullaniciAdi == "admin").FirstOrDefault();
            if (adminMi != null)
            {
                dbSifre = adminMi.Sifre;

                if (sifreKontrol.SifreKontrolEt(gelensifre, dbSifre) == 1)
                {
                    return Json(new { ok = basariliMi, text = code });
                }
                else
                {
                    code = 2;
                    basariliMi = false;
                    return Json(new { ok = basariliMi, text = code });
                }
            }

        }
        basariliMi = false;
        code = 3;
        return Json(new { ok = basariliMi, text = code });


    }
    //*********Role Edit sonu ****************

    /// <summary>
    /// **************      AdminEditUyeKitapResult BAŞI
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isim"></param>
    /// <returns></returns>




    /*
     * 
     * KullaniciAdi Getir
     * 
     * 
     * */
    [HttpPost]
    [AllowAnonymous]
    public JsonResult UyeKullaniciAdi(int? id, string tip = "uye")
    {
        List<Uye> uyelerListesi = new List<Uye>();
        uyelerListesi = db.Uyeler.ToList();
        List<SelectListItem> sonuc = new List<SelectListItem>();
        bool basariliMi = true;
        //string yazarlar = "";
        try
        {
            switch (tip)
            {
                case "uye":
                    foreach (var ad in uyelerListesi)
                    {

                        sonuc.Add(new SelectListItem
                        {
                            Text = ad.KullaniciAdi,
                            Value = ad.ID.ToString()
                        });

                    }

                    break;

                default:
                    break;
            }

        }

        catch (Exception)
        {
            basariliMi = false;
            sonuc = new List<SelectListItem>();
            sonuc.Add(new SelectListItem
            {
                Text = "Bir hata oluştu!",
                Value = "default"
            });

        }

        return Json(new { ok = basariliMi, text = sonuc });
    }


    /*
     * 
     * 
     * Uyeye Ait Kitap Adlari Getir
     * 
     * 
     * */

    [HttpPost]
    [AllowAnonymous]
    public JsonResult UyeKitapJson(int? id, string tip = "uye")
    {
        List<SelectListItem> sonuc = new List<SelectListItem>();
        bool basariliMi = true;
        //gelen id kullanıcının id si
        //List<Uye> uyelerListesi = new List<Uye>();
        //uyelerListesi = db.Uyeler.ToList();
        var uye = db.Uyeler.Find(id);
        var uyeninKitapIDs = db.UyeKitap.Where(x => x.UyeID == id).Select(x => x.KitapID).ToList();
        //elimde uyenin kitaplarinin ıd leri var
        if (uyeninKitapIDs != null)
        {
            foreach (var item in uyeninKitapIDs)
            {
                var UyeninKitaplari = db.Kitaplar.Where(k => k.ID == item).ToList();
                foreach (var kitaplar in UyeninKitaplari)
                {
                    sonuc.Add(new SelectListItem
                    {

                        Text = kitaplar.Isim,
                        Value = kitaplar.ID.ToString()
                    });
                    return Json(new { ok = basariliMi, text = sonuc });
                }
                //elimde uyenin kitaplari var


            }
        }

        else
        {
            basariliMi = false;
            sonuc = new List<SelectListItem>();
            sonuc.Add(new SelectListItem
            {
                Text = "Bir hata oluştu!",
                Value = "default"
            });


        }

        return Json(new { ok = basariliMi, text = sonuc });


    }




    [HttpPost]
    [AllowAnonymous]



        public JsonResult AdminEditUyeKitapJson(int? id, string isim ,string kitapdurum)

    {

        bool basarliMi = true;
        string sonuc = "editKitap";

        int kitapDurum = 0;//üyede


        List<KitapYazarAddModel> model = new List<KitapYazarAddModel>();
            try
            {
                if (HttpContext.Session["KullaniciAdi"] == null)

                {
                    sonuc = "Login";
                    return Json(new { ok = basarliMi, text = sonuc });
                }
                //sonuc = "EditUye";
                var uyeninKitapIds = db.UyeKitap.Where(x => x.UyeID == id).Select(x => x.KitapID).ToList();
                if (uyeninKitapIds.Count != 0)
                {


                    var guncellenecekKitap = db.Kitaplar.Where(k => k.Isim == isim).FirstOrDefault();
                    if (guncellenecekKitap.KitapDurum == 2 && kitapdurum == "Kütüphanede")
                    {
                        kitapDurum = 1;
                        guncellenecekKitap.KitapDurum = kitapDurum;
                        db.Entry(guncellenecekKitap).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                    else if (guncellenecekKitap.KitapDurum == 1 && kitapdurum == "Üyede")
                    {
                        kitapDurum = 2;
                        guncellenecekKitap.KitapDurum = kitapDurum;
                        db.Entry(guncellenecekKitap).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    return Json(new { ok = basarliMi, text = sonuc });
                }
                else
                {

                    sonuc = "createkitap";
                    return Json(new { ok = basarliMi, text = sonuc });
                }
            }

            //model.Add(new KitapYazarAddModel() { Id = yazarResult.ID, Isim = yazarResult.Isim, YazarYorum = yazarResult.Yorum ,});
            ////return View(model);

            catch (Exception)
            {
                basarliMi = false;
                sonuc = "Başarısız İşlem";
                return Json(new { ok = basarliMi, text = sonuc });
                throw;
            }


        }

        /*
         * 
         * 
         * add edit uye kitap admin
         * 
         * */
        // GET: Kitaps/Edit/5



        [HttpPost]
[AllowAnonymous]


public JsonResult AdminEditUyeKitapResultJson(int? id)
{

    bool basarliMi = true;
    string sonuc = "editUyeKitap";
    int kitapDurum = 2;//üyede

    List<KitapYazarAddModel> model = new List<KitapYazarAddModel>();
    try
    {
        if (HttpContext.Session["KullaniciAdi"] == null)

        {
            sonuc = "Login";
            return Json(new { ok = basarliMi, text = sonuc });
        }
        //sonuc = "EditUye";
        var uyeninKitapIds = db.UyeKitap.Where(x => x.UyeID == id).Select(x => x.KitapID).ToList();
        if (uyeninKitapIds.Count != 0)
        {


            return Json(new { ok = basarliMi, text = sonuc });
        }

        sonuc = "createkitap";
        //model.Add(new KitapYazarAddModel() { Id = yazarResult.ID, Isim = yazarResult.Isim, YazarYorum = yazarResult.Yorum ,});
        ////return View(model);

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
public ActionResult AdminEditUyeKitapResult(int? id)
{
    List<KitapUyeViewModel> model = new List<KitapUyeViewModel>();
    string kitapdurum = "";
    //ilgili kullanıcının bilgilerini AdminEditUyeKitapResultta gösterecek olan metod
    //gelen id kullanicinin id si 
    if (id == null)
    {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    }
    Uye uye = db.Uyeler.Find(id);
    var uyeninKitaplariIDs = db.UyeKitap.Where(x => x.UyeID == id).Select(x => x.UyeID).ToList();
    if (uyeninKitaplariIDs != null)
    {
        foreach (var item in uyeninKitaplariIDs)
        {
            var kitaplari = db.Kitaplar.Where(k => k.ID == item).ToList();
            foreach (var kitaplar in kitaplari)
            {
                if (kitaplar.KitapDurum == 1)
                {
                    kitapdurum = "Kütüphanede";
                    model.Add(new KitapUyeViewModel { id = uye.ID, KullaniciAdi = uye.KullaniciAdi, KitapAdi = kitaplar.Isim, KitapDurum = kitapdurum });

                }
                kitapdurum = "Üyede";
                model.Add(new KitapUyeViewModel { id = uye.ID, KullaniciAdi = uye.KullaniciAdi, KitapAdi = kitaplar.Isim, KitapDurum = kitapdurum });

            }
        }
    }

    if (model == null)
    {
        return HttpNotFound();
    }
    return View(model);
}

// POST: Kitaps/Edit/5
// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
[HttpPost]
[AllowAnonymous]

//public ActionResult AdminEditUyeKitapResult(int? id, string yazarId, string kitapadi, string aciklama, string yazaryorum, string yayinci)
//{

//    // var yazarResult = db.Yazarlar.Where(y => y.ID == yazarId).FirstOrDefault();
//    //Yazar gelenYazar = new Yazar();

//    //if (id == 0)
//    //{
//    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

//    //}
//    //Kitap kitap = db.Kitaplar.Find(id);
//    //if (kitap == null)
//    //{
//    //    return HttpNotFound();
//    //}
//    //if (kitapadi != null)
//    //{

//    //    if (yazarResult == null)
//    //    {
//    //        if (ModelState.IsValid)
//    //        {

//    //            ///KİTAP GÜNCELLEME
//    //            kitap.Isim = kitapadi;
//    //            kitap.Yayinci = yayinci;
//    //            kitap.Aciklama = aciklama;
//    //           // kitap.YazarID = yazarId;
//    //            kitap.Yazar.Isim = yazarResult.Isim;
//    //            kitap.Yazar.Soyisim = yazarResult.Soyisim;
//    //            kitap.Yazar.Yorum = yazaryorum;
//    //            db.Entry(kitap).State = EntityState.Modified;
//    //            db.SaveChanges();
//    //            return RedirectToAction("IndexKitap", "Kitap");
//    //        }
//    //    }
//    //    else //yazar zaten varsa kitabın yazarı değiştirilmek isteniyorsa
//    //    {
//    //        //    int gelenYazarId = yazarVarmi.ID;
//    //        //    kitap.YazarID = gelenYazarId;
//    //        //    kitap.Yazar.Isim = yazaradi;
//    //        //    kitap.Yazar.Soyisim = yazarsoyadi;
//    //        //    kitap.Yazar.Yorum = yazaryorum;
//    //        //    kitap.Aciklama = aciklama;
//    //        //    kitap.Isim = kitapadi;
//    //        //    kitap.Yayinci = kitap.Yayinci;
//    //        ViewBag.yazarHata = "Yazar Seçmediniz ya da boş bir yazar seçtiniz";
//    //        return View(ViewBag.yazarHata);

//    //    }


//}

public ActionResult AdminEditUyeKitapResult(int? id, int yazarId, string kitapadi, string aciklama, string yazaryorum, string yayinci)
{

    var yazarResult = db.Yazarlar.Where(y => y.ID == yazarId).FirstOrDefault();
    Yazar gelenYazar = new Yazar();

    if (id == 0)
    {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

    }
    Kitap kitap = db.Kitaplar.Find(id);
    if (kitap == null)
    {
        return HttpNotFound();
    }
    if (kitapadi != null)
    {

        if (yazarResult == null)
        {
            if (ModelState.IsValid)
            {

                ///KİTAP GÜNCELLEME
                kitap.Isim = kitapadi;
                kitap.Yayinci = yayinci;
                kitap.Aciklama = aciklama;
                kitap.YazarID = yazarId;
                kitap.Yazar.Isim = yazarResult.Isim;
                kitap.Yazar.Soyisim = yazarResult.Soyisim;
                kitap.Yazar.Yorum = yazaryorum;
                db.Entry(kitap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexKitap", "Kitap");
            }
        }
        else //yazar zaten varsa kitabın yazarı değiştirilmek isteniyorsa
        {
            //    int gelenYazarId = yazarVarmi.ID;
            //    kitap.YazarID = gelenYazarId;
            //    kitap.Yazar.Isim = yazaradi;
            //    kitap.Yazar.Soyisim = yazarsoyadi;
            //    kitap.Yazar.Yorum = yazaryorum;
            //    kitap.Aciklama = aciklama;
            //    kitap.Isim = kitapadi;
            //    kitap.Yayinci = kitap.Yayinci;
            ViewBag.yazarHata = "Yazar Seçmediniz ya da boş bir yazar seçtiniz";
            return View(ViewBag.yazarHata);

        }


    }


    return View();
}



}

}
