using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

using System.Web.Routing;
using System.Web.Security;

namespace DoAnCuoiKy
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //GlobalConfiguration.Configure(DoAnCuoiKy.Register);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();       
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Application["SoLuongNguoiTryCap"] = 0;
            Application["Online"] = 0;
        }

        protected void Session_Start()
        {
            Application.Lock(); 
            Application["SoLuongNguoiTryCap"] = (int)Application["SoLuongNguoiTryCap"] + 1;
            Application["Online"] = (int)Application["Online"] + 1;
            Application.UnLock();
        }

        protected void Session_End()
        {
            Application.Lock();
            Application["Online"] = (int)Application["Online"] - 1;
            Application.UnLock();
        }
        public void Application_End()
        {
            Application.Lock();
            Application["Online"] = (int)Application["Online"] - 1;
            Application.UnLock();
        }
      
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)

        {
            var TaiKhoanCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (TaiKhoanCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(TaiKhoanCookie.Value);
                var Quyen = authTicket.UserData.Split(new Char[] { ',' });
                var userPrincipal = new GenericPrincipal(new GenericIdentity(authTicket.Name), Quyen);
                Context.User = userPrincipal;
            }
        }
    }
}
