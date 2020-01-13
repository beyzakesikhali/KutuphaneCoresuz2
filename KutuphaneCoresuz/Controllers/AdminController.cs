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
            ViewBag.Login = "Nparsınız ya Allasen Giriş yapmadan Uye silmeye kalkıyosunuz!";
            //AktifUye = HttpContext.Session["KullaniciAdi"].ToString();
            if (HttpContext.Session["KullaniciAdi"] == null)
            {

                ViewBag.Login = "Nparsınız ya Allasen Giriş yapmadan Uye silmeye kalkıyosunuz!";
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
                    // var kitapResult = db.Kitaplar.Where(z => z.ID == item).Single();

                    //if (item.RoleId == 1)
                    //{
                    //    role = "Admin";
                    //    model.Add(new Uye() { id=item.ID, UyeIsim = item.isim, KullaniciAdi = item.KullaniciAdi, UyeSoyisim = item.Soyisim, UyeSifre = item.Sifre, UyeEmail = item.Email, Aciklama = item.Aciklama, Role = role });

                    //}
                    //role = "Uye";

                    //var yazarResult = db.Yazarlar.Where(y => y.ID == kitapResult.YazarID).FirstOrDefault();
                    model.Add(new Uye() { ID = item.ID, isim = item.isim, KullaniciAdi = item.KullaniciAdi, Soyisim = item.Soyisim, Sifre = item.Sifre, Email = item.Email, Aciklama = item.Aciklama });

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
                    return RedirectToAction("IndexAdmin", "Admin");

                }

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
            model.Add(new Uye() { ID = uyeResult.ID, isim = uyeResult.isim, KullaniciAdi = uyeResult.KullaniciAdi, Soyisim = uyeResult.Soyisim, Email = uyeResult.Email, Sifre = "", Aciklama = uyeResult.Aciklama });
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
            var uyeResult = db.Uyeler.Where(x => x.KullaniciAdi == AktifUye).FirstOrDefault();
            int uyeID = uyeResult.ID;
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
            if (KitapVarmi == null)
            {
                if (ModelState.IsValid)
                {
                    //önce kitap tablosu
                    yeniKitap.Isim = model.KitapAdi;
                    yeniKitap.Yayinci = model.yayinci;
                    yeniKitap.Aciklama = model.Aciklama;
                    yeniKitap.YazarID = SeciliYazarID;
                    yeniKitap.Yazar = YazarResult;

                    yeniUyeKitap.UyeID = KitapEklenecekUye.ID;
                    yeniUyeKitap.KitapID = yeniKitap.ID;
                    yeniUyeKitap.Kitap = yeniKitap;
                    yeniUyeKitap.Uye = KitapEklenecekUye;
                    db.Kitaplar.Add(yeniKitap);
                    db.SaveChanges();
                    //sonra FK lardan herhangi biri olan YazarKitap
                    db.UyeKitap.Add(yeniUyeKitap);
                    db.SaveChanges();
                    kitapYazarAddModel.YazarAdi = yeniKitap.Yazar.Isim;
                    kitapYazarAddModel.YazarSoyadi = yeniKitap.Yazar.Soyisim;
                    kitapYazarAddModel.YazarYorum = yeniKitap.Yazar.Yorum;
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

            var seciliUyeResult = db.Uyeler.Where(u => u.isim == model.UyeIsim).FirstOrDefault();
            if (seciliUyeResult != null)
            {
                seciliUyeId = seciliUyeResult.ID;
                if (model.id != seciliUyeId)
                {
                    UyeyeKitapEkle(seciliUyeId, model);
                }
                else
                {
                    UyeyeKitapEkle(model.id, model);
                }

            }

            UyeyeKitapEkle(model.id, model);
            return View(model);

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
            HttpContext.Session["KullaniciAdi"] = mevcutKullanici;
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
                Uye uye = new Uye();
                HttpContext.Session["KullaniciAdi"] = uye.KullaniciAdi;

                var deleteUye = db.Uyeler.Where(u => u.ID == id).FirstOrDefault();
                //int uyeId = 0;
                //  id = uyeIdResult.ID;
                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                else
                {
                    db.Uyeler.Remove(deleteUye);
                    db.SaveChanges();
                    return RedirectToAction("IndexAdmin", "Admin");
                }
            }

        }


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
         

    }




}
