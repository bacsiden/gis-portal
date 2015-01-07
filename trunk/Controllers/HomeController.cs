using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using NationalIT.Models;

namespace NationalIT.Controllers
{
    public class HomeController : BaseController
    {
        //[Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}
