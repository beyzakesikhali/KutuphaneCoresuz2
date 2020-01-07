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
        // GET: Yazar
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]

        public ActionResult IndexYazar()
        {
            return View(db.Yazarlar.ToList());
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
            int code =0;
            try
            {
                if (ModelState.IsValid)
                {
                    Yazar yeniYazar = new Yazar();
                    yeniYazar.Isim = yazar.YazarAdi;
                    yeniYazar.Soyisim = yazar.YazarSoyadi;
                    yeniYazar.Yorum = yazar.YazarYorum;
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

            
   

        //  GET: Yazars/Edit/5
        [AllowAnonymous]
        public ActionResult EditYazarId(Yazar yazar)
        {
            if (HttpContext.Session["kullaniciAdi"] == null)
            {
                return RedirectToAction("Login","Security");
            }

            return View("EditYazarId");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult EditYazarId(int? id, Yazar model)
        {
            if (HttpContext.Session["kullaniciAdi"] == null)
            {
                return RedirectToAction("Login","Security");
            }
            else
            {
                
                var yazarIdResult = db.Yazarlar.Single(y => y.Isim == model.Isim && y.Soyisim == model.Soyisim);
                int gelenId = yazarIdResult.ID;
                // var ad = model.Isim;
                id = gelenId;
                if (gelenId == 0)
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



        [AllowAnonymous]

        // GET: Yazars/Delete/5
        public ActionResult DeleteYazar(Yazar model)
        {
            //Yazar yazarSorgu = new Yazar();
            var yazarIdResult = db.Yazarlar.Where(y => y.Isim == model.Isim).Single();
            int id = yazarIdResult.ID;
            if (id == 0)
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

        // POST: Yazars/Delete/5
        [HttpPost, ActionName("DeleteYazar")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult DeleteConfirmedYazar(int id)
        {
            Yazar yazar = db.Yazarlar.Find(id);
            db.Yazarlar.Remove(yazar);
            db.SaveChanges();
            return RedirectToAction("IndexYazar");
        }
    }
}