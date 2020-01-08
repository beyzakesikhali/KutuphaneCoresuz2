using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KutuphaneCoresuz.Controllers
{
    public class AutorizeAdminAttiribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
            //tetiklendiği anda devreye girer.
            //OnActionResult olsaydı sonuc dönderilmeden önce devreye girerdi.
        {

            if (filterContext.HttpContext.Session["KullaniciAdi"].ToString() != "AdminBeyza")
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary { { "Security", "IndexAdmin" }, { "action", "login" } });
            } 
            
            base.OnActionExecuting(filterContext);
        }

           
        }
    }
}
  