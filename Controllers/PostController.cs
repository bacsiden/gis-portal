using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using System.Data.Objects.SqlClient;
using Microsoft.Reporting.WebForms;
using System.Data;
using System.IO;

namespace NationalIT.Controllers
{

    public class PostController : BaseController
    {
        int pageSize = 5;
        [Authorize]
        public ActionResult AdminIndex(int? page, int? hinhthuc, int? loaiID, int? status, int? noibat)
        {
            var db = DB.Entities;
            var lst = db.Post.Where(m => !m.Deleted && (loaiID == null ? true : m.CateID == loaiID)
                && (status == null || m.Status == status) && (noibat == null || m.Hot == true));

            var list = lst.Where(m => m.LanguageID == CurrentLanguage.ID).OrderByDescending(m => m.ID).ToPagedList(!page.HasValue ? 0 : page.Value, pageSize);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AdminIndex", list);
            }
            SelectOption();
            return View(list);
        }
        [Authorize]
        public ActionResult ListDeleted(int? page)
        {
            var db = DB.Entities;
            var lst = db.Post.Where(m => m.Deleted);
            return View(lst.OrderBy(m => m.ID).ToPagedList(!page.HasValue ? 0 : page.Value, pageSize));
        }
        void SelectOption()
        {
            #region SELECT OPTION
            var db = DB.Entities;

            string dataLoai = "<option value=''>- Tất cả kênh tin -</option>";
            foreach (var item in db.Cate.ToList())
            {
                dataLoai += string.Format("<option value='{0}'>{1}</option>", item.ID, item.Title);
            }
            ViewBag.dataLoai = dataLoai;
            #endregion
        }
        [Authorize]
        public ActionResult AdminEdit(int? id = 0)
        {
            var obj = DB.Entities.Post.FirstOrDefault(m => m.ID == id);
            return View(obj);
        }

        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult AdminEdit(Post model, FormCollection frm, HttpPostedFileBase file)
        {
            var db = DB.Entities;
            Post obj = null;
            if (model.ID == 0)
            {
                obj = new Post();
                obj.Created = DateTime.Now.Date;
                obj.UserID = CurrentUser.ID;
                obj.LanguageID = CurrentLanguage.ID;
                obj.Status = (int)PostStatus.Disabled;
                obj.Hot = true;
                obj.CateID = 4;
                obj.ViewCount = 0;
                obj.Deleted = false;
            }
            else
                obj = db.Post.FirstOrDefault(m => m.ID == model.ID);
            obj.Title = model.Title;
            obj.Summary = model.Summary;
            obj.Content = model.Content;
            if (file != null)
            {
                var now = DateTime.Now;
                var fileName = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", now.Day, now.Hour, now.Minute, now.Second,
                    now.Millisecond, CurrentUser.Email.Replace("@", "--")) + Path.GetExtension(file.FileName);
                file.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                obj.ImageUrl = "/Uploads/" + fileName;
            }
            if (model.ID == 0)
                db.Post.AddObject(obj);
            db.SaveChanges();
            return RedirectToAction("AdminIndex");
        }
        [Authorize]
        public ActionResult AdminDelete(int id)
        {
            var db = DB.Entities;
            var obj = db.Post.FirstOrDefault(m => m.ID == id);
            db.DeleteObject(obj);
            db.SaveChanges();
            return RedirectToAction("ListDeleted");
        }
        [Authorize]
        public ActionResult AdminDeleteAll()
        {
            var db = DB.Entities;
            var lst = db.Post.Where(m => m.Deleted);
            foreach (var item in lst)
            {
                db.DeleteObject(item);
            }
            db.SaveChanges();
            return RedirectToAction("ListDeleted");
        }
        [Authorize]
        public ActionResult SetDeleted(int id)
        {
            var db = DB.Entities;
            var obj = db.Post.FirstOrDefault(m => m.ID == id);
            obj.Deleted = true;
            db.SaveChanges();
            return RedirectToAction("AdminIndex");
        }
        [Authorize]
        public ActionResult Restore(int id)
        {
            var db = DB.Entities;
            var obj = db.Post.FirstOrDefault(m => m.ID == id);
            obj.Deleted = false;
            db.SaveChanges();
            return RedirectToAction("ListDeleted");
        }
        [Authorize]
        public string DoEnable(int id)
        {
            try
            {
                var db = DB.Entities;
                var obj = db.Post.FirstOrDefault(m => m.ID == id);
                obj.Status = obj.Status == 1 ? 2 : 1;
                db.ObjectStateManager.ChangeObjectState(obj, System.Data.EntityState.Modified);
                db.SaveChanges();
                if (obj.Status == 2)
                {
                    return "Đã đăng";
                }
                return "<span class='validation-summary-errors'>Chờ đăng</span>";
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        [Authorize]
        public string DoHot(int id)
        {
            try
            {
                var db = DB.Entities;
                var obj = db.Post.FirstOrDefault(m => m.ID == id);
                obj.Hot = !obj.Hot;
                db.ObjectStateManager.ChangeObjectState(obj, System.Data.EntityState.Modified);
                db.SaveChanges();
                if (obj.Hot == true)
                {
                    return "Tin nổi bật";
                }
                return "<span class='validation-summary-errors'>Tin thường</span>";
            }
            catch
            {
                return "";
            }
        }

        //#region Present page
        //public ActionResult Filter(int? page, int? khuvuc, int? loaiID, int? hinhthuc)
        //{
        //    int pageSize = 2;
        //    var db = DB.Entities;
        //    var list = db.Post.Where(m => !m.Deleted && (hinhthuc == null || m.HinhThuc == hinhthuc) && (khuvuc == null || m.District == khuvuc)
        //        && (loaiID == null || m.LoaiBDSID == loaiID) && m.Status == 2).Take(10).OrderByDescending(m => m.ID).OrderByDescending(m => m.ID).ToPagedList(!page.HasValue ? 0 : page.Value, pageSize);
        //    ViewBag.Title = "Kết quả tìm kiếm bất động sản";
        //    if (Request.IsAjaxRequest())
        //    {
        //        return PartialView("_ListPost", list);
        //    }
        //    return View(list);
        //}
        //public ActionResult ListHinhThuc(int? page, int? hinhthuc, int? khuvuc, int? loaiID, string title)
        //{
        //    int pageSize = 5;
        //    var db = DB.Entities;
        //    var list = db.Post.Where(m => !m.Deleted && (hinhthuc == null || m.HinhThuc == hinhthuc) && (khuvuc == null || m.District == khuvuc)
        //        && (loaiID == null || m.LoaiBDSID == loaiID) && m.Status == 2).Take(10).OrderByDescending(m => m.ID).OrderByDescending(m => m.ID).ToPagedList(!page.HasValue ? 0 : page.Value, pageSize);
        //    ViewBag.Title = "Kết quả tìm kiếm";
        //    if (hinhthuc == 1)
        //    {
        //        ViewBag.Title = "Bất động sản cần bán";
        //    }
        //    if (hinhthuc == 2)
        //    {
        //        ViewBag.Title = "Bất động sản cần mua";
        //    }
        //    if (Request.IsAjaxRequest())
        //    {
        //        return PartialView("_ListPost", list);
        //    }
        //    return View(list);
        //}
        //public ActionResult ListKhuVuc(int? khuvuc, string title)
        //{
        //    var db = DB.Entities;
        //    ViewBag.listPost = new Models.ListPost()
        //    {
        //        HotPost = db.Post.Where(m => !m.Deleted && (khuvuc == null || m.District == khuvuc) && m.Hot).OrderByDescending(m => m.Created).ToList(),
        //        RelatePost = db.Post.Where(m => !m.Deleted && (khuvuc == null || m.District == khuvuc) && !m.Hot).OrderByDescending(m => m.Created).Take(5).ToList(),
        //        Title = title
        //    };
        //    return View();
        //}
        //public ActionResult ListLoai(int? loai, string title)
        //{
        //    var db = DB.Entities;
        //    ViewBag.listPost = new Models.ListPost()
        //    {
        //        HotPost = db.Post.Where(m => !m.Deleted && (loai == null || m.LoaiBDSID == loai) && m.Hot).OrderByDescending(m => m.Created).ToList(),
        //        RelatePost = db.Post.Where(m => !m.Deleted && (loai == null || m.LoaiBDSID == loai) && !m.Hot).OrderByDescending(m => m.Created).Take(5).ToList(),
        //        Title = title
        //    };
        //    return View();
        //}
        //public ActionResult Details(int id = 0)
        //{
        //    var db = DB.Entities;
        //    var obj = db.Post.FirstOrDefault(m => m.ID == id);
        //    if (obj != null)
        //        obj.ViewCount += 1;
        //    db.SaveChanges();
        //    ViewBag.relatePost = new Models.ListPost()
        //    {
        //        HotPost = db.Post.Where(m => m.HinhThuc == obj.HinhThuc && m.ID != id).OrderByDescending(m => m.Created).Take(5).ToList(),
        //        RelatePost = new List<Post>(),
        //        //db.Post.Where(m => !m.Hot && m.ID != id).OrderByDescending(m => m.Created).Take(5).ToList(),
        //        Title = "Các tin bất động sản khác"
        //    };
        //    return View(obj);
        //}
        //public ActionResult DangTin()
        //{
        //    Models.PostBDS model = new Models.PostBDS() { Province = 1 };
        //    if (CurrentUser != null)
        //    {
        //        model.ContactName = CurrentUser.Name;
        //        model.ContactPhone = CurrentUser.PhoneNumber;
        //        model.ContactEmail = CurrentUser.Email;
        //    }
        //    return View(model);
        //}
        //[HttpPost, ValidateInput(false)]
        //public ActionResult DangTin(Models.PostBDS model, FormCollection frm, IEnumerable<HttpPostedFileBase> files)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var post = new Post();
        //            post.Title = model.Title;
        //            post.Content = model.Content;
        //            post.Hot = false;
        //            post.Created = DateTime.Now;
        //            if (CurrentUser != null)
        //                post.Status = (int)PostStatus.Enabled;
        //            else
        //                post.Status = (int)PostStatus.Init;
        //            post.ContactName = model.ContactName;
        //            post.ContactPhone = model.ContactPhone;
        //            post.ContactEmail = model.ContactEmail;
        //            if (model.Money != null)
        //                post.Money = (decimal)model.Money;
        //            post.Province = model.Province;
        //            post.District = model.District;
        //            post.Ward = model.Ward;
        //            post.LoaiBDSID = model.LoaiBDSID;
        //            post.HinhThuc = model.HinhThuc;

        //            var db = DB.Entities;
        //            db.Post.AddObject(post);
        //            db.SaveChanges();
        //            #region Uploads
        //            int autoIncrement = 1;
        //            foreach (var item in files)
        //            {
        //                if (item != null)
        //                {
        //                    string filename = post.ID + "_" + autoIncrement + ".jpg";
        //                    item.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), filename));
        //                    var img = new ImagePost();
        //                    img.PostID = post.ID;
        //                    img.Url = "/Uploads/" + filename;
        //                    img.Title = frm["imgtitle" + autoIncrement];
        //                    db.ImagePost.AddObject(img);
        //                    autoIncrement++;
        //                }
        //            }
        //            db.SaveChanges();
        //            #endregion
        //            return RedirectToAction("Thanks", new { @hidefilter = 1 });
        //        }
        //        catch { return View(model); }
        //    }
        //    return View(model);
        //}

        //public ActionResult Thanks()
        //{
        //    return View();
        //}
        //public ActionResult List()
        //{
        //    return View();
        //}
        //#endregion
    }
}
