using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KutuphaneCoresuz.Controllers
{
    [KutuphaneCoresuz.Controllers.AutorizeAdmin]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult IndexAdmin()
        {
            return View();
        }
    }
}