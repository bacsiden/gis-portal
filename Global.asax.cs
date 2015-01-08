using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Globalization;
using System.Threading;

namespace NationalIT
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
            "details",
            "{site_language}/details/{id}",
            new { controller = "Post", action = "Details", site_language = "vi", id = UrlParameter.Optional },
            new { site_language = "vi|en|zh" });
            routes.MapRoute(
            "Site Language1", // Route name
            "{site_language}/{controller}/{action}/{id}", // URL with parameters
            new { controller = "Home", action = "Index", site_language = "vi", id = UrlParameter.Optional }, // Parameter defaults
            new { site_language = "vi|en|zh" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
        public const string DEFAULT_LANGUAGE_CODE = "vi";

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session != null)
            {
                string languageCode = DEFAULT_LANGUAGE_CODE;
                var languageRoute = Request.RequestContext.RouteData.Values["site_language"];
                if (languageRoute != null)
                    languageCode = languageRoute.ToString();

                //Kiểm tra nếu URL có sự thay đổi về host và language thì cập nhật Session
                //if ((Session[Constant.SESSION_CURRENT_LANGUAGE_CODE] == null ||
                //    !string.Equals(Session[Constant.SESSION_CURRENT_LANGUAGE_CODE].ToString(),
                //    languageCode, StringComparison.OrdinalIgnoreCase)))
                {
                    CultureInfo culture = CultureInfo.CreateSpecificCulture(languageCode);
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                    Session[Constant.SESSION_CURRENT_LANGUAGE_CODE] = languageCode;

                }
            }

        }
        protected void Session_Start()
        {
            Session[Constant.SESSION_CURRENT_LANGUAGE_CODE] = null;
        }
    }
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private string siteLanguage;
        public NinjectControllerFactory(string siteLanguage)
            : base()
        {
            this.siteLanguage = siteLanguage;
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            var xx = requestContext.RouteData.Values["site_language"];
            if (xx != null)
                siteLanguage = xx.ToString();
            CultureInfo culture = CultureInfo.CreateSpecificCulture(siteLanguage);

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            try
            {
                return base.GetControllerInstance(requestContext, controllerType);
            }
            catch
            {
                return null;
            }
        }

    }
}