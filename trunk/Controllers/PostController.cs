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
        int pageSize = 50;
        [Authorize]
        public ActionResult AdminIndex(int? page, int? hinhthuc, int? loaiID, int? status, int? noibat)
        {
            var db = DB.Entities;
            var lst = db.Post.Where(m => !m.Deleted && (hinhthuc == null || m.HinhThuc == hinhthuc.Value) && (loaiID == null ? true : m.LoaiBDSID == loaiID)
                && (status == null || m.Status == status) && (noibat == null || m.Hot == true));

            var list = lst.OrderByDescending(m => m.ID).ToPagedList(!page.HasValue ? 0 : page.Value, pageSize);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AdminIndexPartial", list);
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

            string dataLoai = "<option value=''>- Tất cả loại BĐS -</option>";
            foreach (var item in db.LoaiBDS.ToList())
            {
                dataLoai += string.Format("<option value='{0}'>{1}</option>", item.ID, item.Title);
            }
            ViewBag.dataLoai = dataLoai;
            #endregion
        }
        [Authorize]
        public ActionResult NewOrEdit(int? driverID, int? id = 0)
        {
            var obj = DB.Entities.Post.FirstOrDefault(m => m.ID == id);
            //if (obj == null)
            //{
            //    obj = new Post() { Driver = driverID, Picked = true, Current_Payroll = true, Customer_Invoiced_date = DateTime.Now.Date, Order_date = DateTime.Now, Pickup_date = DateTime.Now };
            //}

            //#region SELECT OPTION
            //string dataCustomer = "<option value='' >--Select Customer--</option>";
            //foreach (var item in NationalIT.DB.Entities.Customer_Info)
            //{
            //    if (obj != null && obj.Customer == item.Customer_ID)
            //    {
            //        dataCustomer += string.Format("<option value='{0}' selected='selected'>{1}</option>", item.Customer_ID, item.Customer_Name);
            //    }
            //    else
            //    {
            //        dataCustomer += string.Format("<option value='{0}'>{1}</option>", item.Customer_ID, item.Customer_Name);
            //    }
            //}
            //ViewBag.dataCustomer = dataCustomer;
            //string dataDispatchers = "<option value=''>--Select Dispatcher--</option>";
            //foreach (var item in NationalIT.DB.Entities.Dispatchers)
            //{
            //    if (obj != null && obj.Dispatcher == item.ID)
            //    {
            //        dataDispatchers += string.Format("<option value='{0}' selected='selected'>{1} {2}</option>", item.ID, item.Last_name, item.First_name);
            //    }
            //    else
            //    {
            //        dataDispatchers += string.Format("<option value='{0}'>{1} {2}</option>", item.ID, item.Last_name, item.First_name);
            //    }
            //}
            //ViewBag.dataDispatchers = dataDispatchers;

            //string dataDriver = "<option value=''>--Select Driver--</option>";
            //foreach (var item in NationalIT.DB.Entities.Driver_Info)
            //{
            //    if (obj != null && obj.Driver == item.ID)
            //    {
            //        dataDriver += string.Format("<option value='{0}' selected='selected'>{1} {2}</option>", item.ID, item.Last_name, item.First_name);
            //    }
            //    else
            //    {
            //        dataDriver += string.Format("<option value='{0}'>{1} {2}</option>", item.ID, item.Last_name, item.First_name);
            //    }
            //}
            //ViewBag.dataDriver = dataDriver;

            //string dataEquipment = "<option  value='' >--Select Equiment--</option>";
            //foreach (var item in NationalIT.DB.Entities.Equipment)
            //{
            //    if (obj != null && obj.Equipment_ID == item.ID)
            //    {
            //        dataEquipment += string.Format("<option value='{0}' selected='selected'>{1}</option>", item.ID, item.Equipment_number);
            //    }
            //    else
            //    {
            //        dataEquipment += string.Format("<option value='{0}'>{1}</option>", item.ID, item.Equipment_number);
            //    }
            //}
            //ViewBag.dataEquipment = dataEquipment;

            //var lstCompany = db.Company.ToList();
            //ViewBag.dataCompany = CommonFunction.BuildDropdown(lstCompany.Select(m => m.ID.ToString()).ToArray(),
            //    lstCompany.Select(m => m.Name + " - " + m.Address + " , " + m.FaxNumber).ToArray(), obj.Company, "--Select Company--");

            //#endregion

            return View(obj);
        }

        [Authorize]
        [HttpPost]
        public ActionResult NewOrEdit(Post model, FormCollection frm)
        {
            //try
            //{
            //    if (model.Dispatcher.HasValue)
            //    {
            //        var db = DB.Entities;
            //        model.Customer_Invoiced_date = CommonFunction.ChangeFormatDate(frm["Customer_Invoiced_date"]).Value;
            //        model.Order_date = CommonFunction.ChangeFormatDate(frm["Order_date"]);
            //        model.Delivery_date = CommonFunction.ChangeFormatDate(frm["Delivery_date"]);
            //        model.Pickup_date = CommonFunction.ChangeFormatDate(frm["Pickup_date"]);
            //        if (model.Trip_ID == 0 || model.Invoice == 0)
            //        {
            //            // New
            //            db.Trip_Info.AddObject(model);
            //            db.SaveChanges();
            //            model.Invoice = 9999 + model.Trip_ID;
            //            var income = new Income()
            //            {
            //                IncomeDate = DateTime.Now,
            //                AmountInvoiced = model.Total_charges,
            //                Comments = model.Comment,
            //                InvoiceNumber = model.Invoice,
            //                Driver = model.Driver_Info != null ? model.Driver_Info.Last_name + " " + model.Driver_Info.First_name : "",
            //                FundedAmount = model.Total_charges,
            //            };
            //            db.Income.AddObject(income);
            //        }
            //        else
            //        {
            //            var income = db.Income.FirstOrDefault(m => m.InvoiceNumber == model.Invoice);
            //            if (income != null)
            //            {
            //                income.IncomeDate = DateTime.Now;
            //                income.AmountInvoiced = model.Total_charges;
            //                income.Comments = model.Comment;
            //                income.InvoiceNumber = model.Invoice;
            //                income.Driver = model.Driver_Info != null ? model.Driver_Info.Last_name + " " + model.Driver_Info.First_name : "";
            //                income.FundedAmount = model.Total_charges;
            //            }
            //            db.AttachTo("Trip_Info", model);
            //            db.ObjectStateManager.ChangeObjectState(model, System.Data.EntityState.Modified);
            //        }
            //        db.SaveChanges();
            //        return RedirectToAction("Index");
            //    }
            //    else
            //    {
            //        ModelState.AddModelError("Dispatcher", "This field is required");
            //    }
            //}
            //catch
            //{

            //}
            //#region SELECT OPTION
            //string dataCustomer = "<option value=''>--Select Customer--</option>";
            //foreach (var item in NationalIT.DB.Entities.Customer_Info)
            //{
            //    if (model != null && model.Customer == item.Customer_ID)
            //    {
            //        //dataDispatchers += "{ \"id\": " + item.ID + ", \"label\": \"" + item.Last_name + " " + item.First_name + "\" }";
            //        dataCustomer += string.Format("<option value='{0}' selected='selected'>{1}</option>", item.Customer_ID, item.Customer_Name);
            //    }
            //    else
            //    {
            //        dataCustomer += string.Format("<option value='{0}'>{1}</option>", item.Customer_ID, item.Customer_Name);
            //    }
            //}
            //ViewBag.dataCustomer = dataCustomer;
            //string dataDispatchers = "<option value=''>--Select Dispatcher--</option>";
            //foreach (var item in NationalIT.DB.Entities.Dispatchers)
            //{
            //    if (model != null && model.Dispatcher == item.ID)
            //    {
            //        //dataDispatchers += "{ \"id\": " + item.ID + ", \"label\": \"" + item.Last_name + " " + item.First_name + "\" }";
            //        dataDispatchers += string.Format("<option value='{0}' selected='selected'>{1} {2}</option>", item.ID, item.Last_name, item.First_name);
            //    }
            //    else
            //    {
            //        dataDispatchers += string.Format("<option value='{0}'>{1} {2}</option>", item.ID, item.Last_name, item.First_name);
            //    }
            //}
            //ViewBag.dataDispatchers = dataDispatchers;

            //string dataDriver = "<option value=''>--Select Driver--</option>";
            //foreach (var item in NationalIT.DB.Entities.Driver_Info)
            //{
            //    if (model != null && model.Driver == item.ID)
            //    {
            //        dataDriver += string.Format("<option value='{0}' selected='selected'>{1} {2}</option>", item.ID, item.Last_name, item.First_name);
            //    }
            //    else
            //    {
            //        dataDriver += string.Format("<option value='{0}'>{1} {2}</option>", item.ID, item.Last_name, item.First_name);
            //    }
            //}
            //ViewBag.dataDriver = dataDriver;

            //string dataEquipment = "<option value='' >--Select Equiment--</option>";
            //foreach (var item in NationalIT.DB.Entities.Equipment)
            //{
            //    if (model != null && model.Equipment_ID == item.ID)
            //    {
            //        dataEquipment += string.Format("<option value='{0}' selected='selected'>{1}</option>", item.ID, item.Equipment_number);
            //    }
            //    else
            //    {
            //        dataEquipment += string.Format("<option value='{0}'>{1}</option>", item.ID, item.Equipment_number);
            //    }
            //}
            //ViewBag.dataEquipment = dataEquipment;
            //var lstCompany = DB.Entities.Company.ToList();
            //ViewBag.dataCompany = CommonFunction.BuildDropdown(lstCompany.Select(m => m.ID.ToString()).ToArray(),
            //    lstCompany.Select(m => m.Name + " - " + m.Address + " , " + m.FaxNumber).ToArray(), model.Company, "--Select Company--");
            //#endregion
            return View(model);
        }
        [Authorize]
        public ActionResult Delete(int id)
        {
            var db = DB.Entities;
            var obj = db.Post.FirstOrDefault(m => m.ID == id);
            db.DeleteObject(obj);
            db.SaveChanges();
            return RedirectToAction("ListDeleted");
        }
        [Authorize]
        public ActionResult DeleteAll()
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
        public string DoDisable(int id)
        {
            try
            {
                var db = DB.Entities;
                var obj = db.Post.FirstOrDefault(m => m.ID == id);
                obj.Status = (int)PostStatus.Disabled;
                db.ObjectStateManager.ChangeObjectState(obj, System.Data.EntityState.Modified);
                db.SaveChanges();
                //if (obj.Activated == true)
                //{
                //    return "Đã bật";
                //}
                return "<span class='validation-summary-errors'>Đã tắt</span>";
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

        #region Present page
        public ActionResult Filter(int? page, int? khuvuc, int? loaiID, int? hinhthuc)
        {
            int pageSize = 2;
            var db = DB.Entities;
            var list = db.Post.Where(m => !m.Deleted && (hinhthuc == null || m.HinhThuc == hinhthuc) && (khuvuc == null || m.District == khuvuc)
                && (loaiID == null || m.LoaiBDSID == loaiID) && m.Status == 2).Take(10).OrderByDescending(m => m.ID).OrderByDescending(m => m.ID).ToPagedList(!page.HasValue ? 0 : page.Value, pageSize);
            ViewBag.Title = "Kết quả tìm kiếm bất động sản";
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListPost", list);
            }
            return View(list);
        }
        public ActionResult ListHinhThuc(int? page, int? hinhthuc, int? khuvuc, int? loaiID, string title)
        {
            int pageSize = 5;
            var db = DB.Entities;
            var list = db.Post.Where(m => !m.Deleted && (hinhthuc == null || m.HinhThuc == hinhthuc) && (khuvuc == null || m.District == khuvuc)
                && (loaiID == null || m.LoaiBDSID == loaiID) && m.Status == 2).Take(10).OrderByDescending(m => m.ID).OrderByDescending(m => m.ID).ToPagedList(!page.HasValue ? 0 : page.Value, pageSize);
            ViewBag.Title = "Kết quả tìm kiếm";
            if (hinhthuc == 1)
            {
                ViewBag.Title = "Bất động sản cần bán";
            }
            if (hinhthuc == 2)
            {
                ViewBag.Title = "Bất động sản cần mua";
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ListPost", list);
            }
            return View(list);
        }
        public ActionResult ListKhuVuc(int? khuvuc, string title)
        {
            var db = DB.Entities;
            ViewBag.listPost = new Models.ListPost()
            {
                HotPost = db.Post.Where(m => !m.Deleted && (khuvuc == null || m.District == khuvuc) && m.Hot).OrderByDescending(m => m.Created).ToList(),
                RelatePost = db.Post.Where(m => !m.Deleted && (khuvuc == null || m.District == khuvuc) && !m.Hot).OrderByDescending(m => m.Created).Take(5).ToList(),
                Title = title
            };
            return View();
        }
        public ActionResult ListLoai(int? loai, string title)
        {
            var db = DB.Entities;
            ViewBag.listPost = new Models.ListPost()
            {
                HotPost = db.Post.Where(m => !m.Deleted && (loai == null || m.LoaiBDSID == loai) && m.Hot).OrderByDescending(m => m.Created).ToList(),
                RelatePost = db.Post.Where(m => !m.Deleted && (loai == null || m.LoaiBDSID == loai) && !m.Hot).OrderByDescending(m => m.Created).Take(5).ToList(),
                Title = title
            };
            return View();
        }
        public ActionResult Details(int id = 0)
        {
            var db = DB.Entities;
            var obj = db.Post.FirstOrDefault(m => m.ID == id);
            if (obj != null)
                obj.ViewCount += 1;
            db.SaveChanges();
            ViewBag.relatePost = new Models.ListPost()
            {
                HotPost = db.Post.Where(m => m.HinhThuc == obj.HinhThuc && m.ID != id).OrderByDescending(m => m.Created).Take(5).ToList(),
                RelatePost = new List<Post>(),
                //db.Post.Where(m => !m.Hot && m.ID != id).OrderByDescending(m => m.Created).Take(5).ToList(),
                Title = "Các tin bất động sản khác"
            };
            return View(obj);
        }
        public ActionResult DangTin()
        {
            Models.PostBDS model = new Models.PostBDS() { Province = 1 };
            if (CurrentUser != null)
            {
                model.ContactName = CurrentUser.Name;
                model.ContactPhone = CurrentUser.PhoneNumber;
                model.ContactEmail = CurrentUser.Email;
            }
            return View(model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DangTin(Models.PostBDS model, FormCollection frm, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var post = new Post();
                    post.Title = model.Title;
                    post.Content = model.Content;
                    post.Hot = false;
                    post.Created = DateTime.Now;
                    if (CurrentUser != null)
                        post.Status = (int)PostStatus.Enabled;
                    else
                        post.Status = (int)PostStatus.Init;
                    post.ContactName = model.ContactName;
                    post.ContactPhone = model.ContactPhone;
                    post.ContactEmail = model.ContactEmail;
                    if (model.Money != null)
                        post.Money = (decimal)model.Money;
                    post.Province = model.Province;
                    post.District = model.District;
                    post.Ward = model.Ward;
                    post.LoaiBDSID = model.LoaiBDSID;
                    post.HinhThuc = model.HinhThuc;

                    var db = DB.Entities;
                    db.Post.AddObject(post);
                    db.SaveChanges();
                    #region Uploads
                    int autoIncrement = 1;
                    foreach (var item in files)
                    {
                        if (item != null)
                        {
                            string filename = post.ID + "_" + autoIncrement + ".jpg";
                            item.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), filename));
                            var img = new ImagePost();
                            img.PostID = post.ID;
                            img.Url = "/Uploads/" + filename;
                            img.Title = frm["imgtitle" + autoIncrement];
                            db.ImagePost.AddObject(img);
                            autoIncrement++;
                        }
                    }
                    db.SaveChanges();
                    #endregion
                    return RedirectToAction("Thanks", new { @hidefilter = 1 });
                }
                catch { return View(model); }
            }
            return View(model);
        }

        public ActionResult Thanks()
        {
            return View();
        }
        public ActionResult List()
        {
            return View();
        }
        #endregion
    }
}
