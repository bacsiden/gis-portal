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
            var db = DB.Entities;
            var canban = new ListPost();
            canban.HotPost = db.Post.Where(m => m.HinhThuc == (int)HinhThuc.CanBan && m.Status == 2)
                .OrderByDescending(m => m.ID).Take(4).ToList();
            if (canban.HotPost.Count == 4)
                canban.RelatePost = db.Post.Where(m => m.HinhThuc == (int)HinhThuc.CanBan && m.Status == 2)
                    .OrderByDescending(m => m.ID).Skip(4).Take(4).ToList();
            else
                canban.RelatePost = new List<Post>();
            canban.Title = "Bất động sản cần bán";
            canban.BottomTitle = "Tin bất động sản cần bán khác";

            var canmua = new ListPost();
            canmua.HotPost = db.Post.Where(m => m.HinhThuc == (int)HinhThuc.CanMua && m.Status == 2)
                .OrderByDescending(m => m.ID).Take(4).ToList();
            if (canmua.HotPost.Count == 4)
                canmua.RelatePost = db.Post.Where(m => m.HinhThuc == (int)HinhThuc.CanMua && m.Status == 2)
                    .OrderByDescending(m => m.ID).Skip(4).Take(4).ToList();
            else
                canmua.RelatePost = new List<Post>();
            canmua.Title = "Bất động sản cần mua";
            canmua.BottomTitle = "TIn bất động sản cần mua khác";

            ViewBag.listPost = canban;
            ViewBag.listBuyPost = canmua;
            
            return View();
        }
    }
}
