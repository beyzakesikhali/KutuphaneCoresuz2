using KutuphaneCoresuz.Models.Context;
using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KutuphaneCoresuz.Controllers
{
    public class HomeController : Controller
    {
        public KutuphaneContext context  = new KutuphaneContext();


        public ActionResult Index()
        {

            //var name = User.Claims.Where(c => c.Type == ClaimTypes.Name)
            //       .Select(c => c.Value).SingleOrDefault();


            return Redirect("Login");
        }
    }
}