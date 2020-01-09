using KutuphaneCoresuz.Models.Context;
using KutuphaneCoresuz.Models.Data;
using KutuphaneCoresuz.Models.ModelforDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web;

    //https://www.codeproject.com/Articles/1110431/Gridview-in-ASP-NET-MVC
namespace KutuphaneCoresuz.Controllers
{
   
    public class AdminController : Controller
    {

        private KutuphaneContext db = new KutuphaneContext();
        // GET: Admin
        [AllowAnonymous]
        public ActionResult IndexAdmin()
        {
            string role = "";

            string AktifUye = "";
            HttpContext.Session["KullaniciAdi"]=AktifUye;   
            List<KitapUyeViewModel> model = new List<KitapUyeViewModel>();
            var uyeResult = db.Uyeler.ToList();
          // var uyeler = db.Uyeler.Single();
           // int uyeID = uyeResult.ID;
           // var kitapIdResult = db.UyeKitap.Where(a => a.UyeID == uyeID).Select(a => a.KitapID).ToList();
            if (uyeResult.Count() != 0)
            {
                foreach (var item in uyeResult)
                {
                   // var kitapResult = db.Kitaplar.Where(z => z.ID == item).Single();
                   
                    if (item.RoleId == 1)
                    {
                        role = "Admin";
                        model.Add(new KitapUyeViewModel() { UyeIsim = item.isim, KullaniciAdi = item.KullaniciAdi, UyeSoyisim = item.Soyisim, UyeSifre = item.Sifre, UyeEmail = item.Email, Aciklama = item.Aciklama, Role = role });

                    }
                    role = "Uye";

                    //var yazarResult = db.Yazarlar.Where(y => y.ID == kitapResult.YazarID).FirstOrDefault();
                    model.Add(new KitapUyeViewModel() { UyeIsim = item.isim, KullaniciAdi = item.KullaniciAdi, UyeSoyisim=item.Soyisim, UyeSifre=item.Sifre,UyeEmail=item.Email, Aciklama=item.Aciklama, Role = role });
                   
                }
                if (model != null)
                {
                    return View(model);
                }
            }


            return View();
        }
      
       

        public ActionResult AdminUyeKitap(int id)
        {

            //SOR SOR SOR
            // Session["kullanciAdi"] = ViewBag.KullaniciAdi;

            Uye uye = new Uye();
            Kitap kitap = new Kitap();
            UyeKitap uyeKitap = new UyeKitap();
            Yazar yazarlar = new Yazar();
            int kitapdurum;
            string kitapDurum = "";
            if (HttpContext.Session["kullaniciAdi"] == null)
            {
                return Redirect("Login");
            }
            string AktifUye = HttpContext.Session["kullaniciAdi"].ToString();
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

                        kitapDurum = "Kitap Sizde";

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
    }
}