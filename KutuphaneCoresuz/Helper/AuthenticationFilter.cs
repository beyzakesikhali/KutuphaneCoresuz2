using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using AuthenticationContext = System.Web.Mvc.Filters.AuthenticationContext;

namespace KutuphaneCoresuz.Helper
{
    //public interface AuthenticationFilter 
    //{
    //    void OnAuthentication(AuthenticationContext filterContext);

    //    void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext);
    //}

    //public class CustomAuthenticationAttribute : ActionFilterAttribute, IAuthenticationFilter
    //{
    //    public void OnAuthentication(AuthenticationContext filterContext)
    //    {



    //    }

    //    public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
    //    {
    //    }
    //}
    public class AuthenticationAttiribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["KullaniciAdi"] != null)
            {
                return true;
            }
            else
            {
                httpContext.Response.Redirect("/Security/Login");
                return false;
            }

        }
    }

}
