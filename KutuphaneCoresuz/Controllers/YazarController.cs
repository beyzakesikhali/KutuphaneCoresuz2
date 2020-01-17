using KutuphaneCoresuz.Models.Context;
using KutuphaneCoresuz.Models.Data;
using KutuphaneCoresuz.Models.ModelforDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace KutuphaneCoresuz.Controllers
{
    public class YazarController : Controller
    {
        private KutuphaneContext db = new KutuphaneContext();
               
        [AllowAnonymous]

        public ActionResult IndexYazar()
        {
            List<Yazar> yazarlar = new List<Yazar>();
            yazarlar = db.Yazarlar.ToList();
            List<Yazar> gosterilecekler = new List<Yazar>();
            foreach (var item in yazarlar)
            {
                if(item.aktiflik==1)
                {
                    gosterilecekler.Add(new Yazar { ID = item.ID, aktiflik = 1, Isim = item.Isim, Soyisim = item.Soyisim, Yorum = item.Yorum });
                    
                }
            }
 
            return View(gosterilecekler);
        }
        //yazarad soyad getirecek
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
                            if(ad.aktiflik==1)
                            {
                                sonuc.Add(new SelectListItem
                                {
                                    Text = ad.Isim + " " + ad.Soyisim,
                                    Value = ad.ID.ToString()
                                });
                            }
                            

                        }

                        break;
                    //case "yazarSoyadGetir":

                    //    foreach (var soyad in db.Yazarlar.Where(y => y.ID ==id).ToList())

                    //    {
                    //        if (id != null)
                    //        {
                    //            sonuc.Add(new SelectListItem
                    //            {
                    //                Text = soyad.Soyisim,
                    //                Value = soyad.ID.ToString()
                    //            });
                    //        }


                    //    }
                    //break;
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


        [AllowAnonymous]
        public ActionResult CreateYazarAction()
        {
            if (HttpContext.Session["KullaniciAdi"].Equals("admin") == false)
            {
                return RedirectToAction("Login", "Security");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult CreateYazarAction(string yazaradi,string yazarsoyadi, string yazaryorum)
        {
            var yazarVarMi = db.Yazarlar.Where(y => y.Isim == yazaradi && y.Soyisim == yazarsoyadi && y.Yorum == yazaryorum).FirstOrDefault();
            if(yazarVarMi==null)
            {
                if (ModelState.IsValid)
                {
                    Yazar yeniYazar = new Yazar();
                    yeniYazar.Isim = yazaradi;
                    yeniYazar.Soyisim = yazarsoyadi;
                    yeniYazar.Yorum = yazaryorum;
                    yeniYazar.aktiflik = 1;
                    
                    db.Yazarlar.Add(yeniYazar);
                    db.SaveChanges();
                    //    KitapYazarAddModel kitapYazarModel = new KitapYazarAddModel();
                    //    List<SelectListItem> adi = (from i in db.Yazarlar.ToList()
                    //                                select new SelectListItem
                    //                                {
                    //                                    Text = i.Isim,
                    //                                    Value = i.ID.ToString()

                    //                                }).ToList();
                    //    List<SelectListItem> soyadi = (from j in db.Yazarlar.ToList()
                    //                                   select new SelectListItem
                    //                                   {
                    //                                       Text = j.Soyisim,
                    //                                       Value = j.ID.ToString()
                    //                                   }).ToList();
                }
            }
            else
            {
                yazarVarMi.aktiflik = 1;
                yazarVarMi.Isim = yazaradi;
                yazarVarMi.Soyisim = yazarsoyadi;
                yazarVarMi.Yorum = yazaryorum;
                db.Entry(yazarVarMi).State = EntityState.Modified;
                db.SaveChanges();



            }
           

            return View();
        }
        // GET: Yazars/Details/5

        [AllowAnonymous]
        public ActionResult DetailsYazar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yazar yazar = db.Yazarlar.Find(id);
            if (yazar == null)
            {
                return HttpNotFound();
            }
            return View(yazar);
        }

        // GET: Yazars/Create
        //[AllowAnonymous]

        //public ActionResult CreateYazar()
        //{
        //    return View();
        //}

        // POST: Yazars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        [AllowAnonymous]

        public JsonResult CreateYazar(KitapYazarAddModel yazar)
        {
            bool basariliMi = true;
            int code = 0;
            try
            {
                
                if (ModelState.IsValid)
                {
                    Yazar yeniYazar = new Yazar();
                    yeniYazar.Isim = yazar.YazarAdi;
                    yeniYazar.Soyisim = yazar.YazarSoyadi;
                    yeniYazar.Yorum = yazar.YazarYorum;
                    yeniYazar.aktiflik = 1;
                    db.Yazarlar.Add(yeniYazar);
                    db.SaveChanges();
                    KitapYazarAddModel kitapYazarModel = new KitapYazarAddModel();
                    List<SelectListItem> adi = (from i in db.Yazarlar.ToList()
                                                select new SelectListItem
                                                {
                                                    Text = i.Isim,
                                                    Value = i.ID.ToString()

                                                }).ToList();
                    List<SelectListItem> soyadi = (from j in db.Yazarlar.ToList()
                                                   select new SelectListItem
                                                   {
                                                       Text = j.Soyisim,
                                                       Value = j.ID.ToString()
                                                   }).ToList();
                    kitapYazarModel.YazarAdlari = adi;
                    kitapYazarModel.YazarSoyadlari = soyadi;
                    //ViewBag.adlar = adi;
                    //ViewBag.soyadlar = soyadi;

                    //model.Add(new KitapYazarAddModel() { YazarSoyadi = yazar.Soyisim });




                }
                code = 1; //CreateKitap

                return Json(new { ok = basariliMi, text = code });//CreateKitap sayfasına yönlendirecek code=1
            }


            catch (Exception)
            {
                basariliMi = false;
                code = 2;
                return Json(new { ok = basariliMi, text = code });//CreateYazarı tekrar açacak kod
            }

        }



        //*************EDİT YAZAR **********
        [HttpPost]
        [AllowAnonymous]


        public JsonResult EditYazarJson(int? id, string isim)
        {

            bool basarliMi = true;
            string sonuc = "EditUye";
            List<Yazar> model = new List<Yazar>();
            try
            {
                if (HttpContext.Session["KullaniciAdi"] == null)

                {
                    sonuc = "Login";
                    return Json(new { ok = basarliMi, text = sonuc });
                }
                //sonuc = "EditUye";
                var yazarResult = db.Yazarlar.Where(u => u.ID == id).FirstOrDefault();

                if (yazarResult != null)
                {
                    sonuc = "editYazar";
                    model.Add(new Yazar() { ID = yazarResult.ID, Isim = yazarResult.Isim, Yorum = yazarResult.Yorum});
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
        public ActionResult EditYazarId(int? id)
        {
            List<Yazar> model = new List<Yazar>();
            // string gelneisim = isim;
            var yazarResult = db.Yazarlar.Where(u => u.ID == id).FirstOrDefault();
            if(yazarResult.aktiflik==1)
            {
                model.Add(new Yazar() { ID = yazarResult.ID, Isim = yazarResult.Isim, Soyisim=yazarResult.Soyisim, Yorum = yazarResult.Yorum });
                return View(model);

            }
            return View("HATA");

        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult EditYazarId(int? id, string isim, string soyisim, string yorum)
        {
            if (HttpContext.Session["KullaniciAdi"] == null)
            {
                return RedirectToAction("Login", "Security");
            }
            else
            {
                //var yazarIdResult = db.Yazarlar.Where(y => y.ID == id).FirstOrDefault();

              //  var yazarIdResult = db.Yazarlar.Single(y => y.Isim == model.Isim && y.Soyisim == model.Soyisim);
                //int gelenId = yazarIdResult.ID;
                // var ad = model.Isim;
               // id = gelenId;
                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Yazar yazar = db.Yazarlar.Find(id);
                if (yazar == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    yazar.Isim = isim;
                    yazar.Soyisim = soyisim;
                    yazar.Yorum = yorum;
                   
                    db.Entry(yazar).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("IndexYazar");
                    //return View(yazar);

                }

            }

        }

        // POST: Yazars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.



        //[AllowAnonymous]
        //public ActionResult EditYazar(Yazar yazar)
        //{
        //    if (HttpContext.Session["kullaniciAdi"] == null)
        //    {
        //        return Redirect("Login");
        //    }
        //    //var yazarIdResult = db.Yazarlar.Where(y => y.Isim == model.Isim && y.Soyisim == model.Soyisim).Single();
        //    //int id = yazarIdResult.ID;

        //    //if (id == null)
        //    //{
        //    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    //}
        //    //else
        //    //{
        //    //Yazar yazar = db.Yazarlar.Find(id);
        //    if (yazar == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    else
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            db.Entry(yazar).State = EntityState.Modified;
        //            db.SaveChanges();
        //            return RedirectToAction("IndexYazar");
        //        }
        //        return View(yazar);

        //    }
        //}


        //delete delete delete delete YAZAR****** DELETE YAZAR*****


        [HttpPost]
        [AllowAnonymous]

        public JsonResult DeleteYazarJson(int? id, string isim)
        {

            bool basarliMi = true;
            string sonuc = "deleteyazar";
            List<Yazar> model = new List<Yazar>();
            try
            {
                if (HttpContext.Session["KullaniciAdi"] == null)

                {
                    sonuc = "Login";
                    return Json(new { ok = basarliMi, text = sonuc });
                }
                //sonuc = "EditUye";
                var yazarResult = db.Yazarlar.Where(u => u.ID == id).FirstOrDefault();

                if (yazarResult != null)
                {
                    sonuc = "deleteyazar";
                    model.Add(new Yazar() { ID = yazarResult.ID, Isim = yazarResult.Isim, Soyisim = yazarResult.Soyisim, Yorum = yazarResult.Yorum});
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

        // GET: Yazars/Delete/5
        public ActionResult DeleteYazar(int? id)
        {
            Yazar model = new Yazar();
            string mevcutKullanici = "";
            mevcutKullanici = HttpContext.Session["KullaniciAdi"].ToString();
            if (mevcutKullanici == null)
            {
                RedirectToAction("Login", "Security");
            }
            //var YazarResult = db.Yazarlar.Where(u => u.ID == id).FirstOrDefault();
            model = db.Yazarlar.Find(id);
           
            if(model.aktiflik==1)
            {
                return View(model);
            }
            //model.Add(new Yazar() { ID = YazarResult.ID, Isim = YazarResult.Isim, Soyisim = YazarResult.Soyisim,Yorum= YazarResult.Yorum});
            return View("HATA");

        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult DeleteYazar(int? id, string isim)
        {
            if (HttpContext.Session["KullaniciAdi"] == null)
            {
                return RedirectToAction("Login", "Security");
            }
            else
            {
                
                //var deleteUye = db.Yazarlar.Where(u => u.ID == id).FirstOrDefault();
                //int uyeId = 0;
                //  id = uyeIdResult.ID;
                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                else
                {
                    Yazar yazar = db.Yazarlar.Find(id);
                    List<Kitap> kitap = db.Kitaplar.Where(k => k.YazarID == Convert.ToInt32(id)).ToList();
                    foreach (var item in kitap)
                    {
                        if(item.KitapDurum==1)
                        {
                            yazar.aktiflik = 1;
                            db.Entry(yazar).State = EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("IndexYazar", "Yazar");
                        }
                        else
                        {
                            return View("Hata Yazarın herhangi bir kitabı üyede");
                        }

                    }
                    return View();
                   
                  
                }
            }

        }



        //delete delete delete delete YAZAR****** DELETE YAZAR SONUUUU*****

        //// POST: Yazars/Delete/5
        //[HttpPost, ActionName("DeleteYazar")]
        //[ValidateAntiForgeryToken]
        //[AllowAnonymous]
        //public ActionResult DeleteConfirmedYazar(int id)
        //{
        //    Yazar yazar = db.Yazarlar.Find(id);
        //    db.Yazarlar.Remove(yazar);
        //    db.SaveChanges();
        //    return RedirectToAction("IndexYazar");
        //}

    }





}