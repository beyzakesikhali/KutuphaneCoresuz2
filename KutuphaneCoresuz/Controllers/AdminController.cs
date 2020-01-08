using KutuphaneCoresuz.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KutuphaneCoresuz.Controllers
{
    [AutorizeAdminAttiribute]
    public class AdminController : Controller
    {

        private KutuphaneContext db = new KutuphaneContext();
        // GET: Admin
        public ActionResult IndexAdmin()
        {
            return View();
        }
        public ActionResult IndexAdminUye()
        {
            return View(db.Uyeler.ToList());
        }
    }
}