using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NationalIT.Controllers
{
    [Authorize]
    public class PortfolioController : BaseController
    {
        public ActionResult WardIndex()
        {
            return View(DB.Entities.Ward.ToList());
        }
        public ActionResult WardUpdate(int? id = 0)
        {
            var db = DB.Entities;

            var model = DB.Entities.Ward.FirstOrDefault(m => m.ID == id);
            ViewBag.District = CommonFunction.BuildDropdown(db.District.Select(m => m.ID).ToArray(), 
                db.District.Select(m => m.Title).ToArray(), model == null ? 0 : model.DistrictID, null);
            return View(model);
        }

        [HttpPost]
        public ActionResult WardUpdate(Ward model, FormCollection frm)
        {
            var db = DB.Entities;
            try
            {
                if (model.ID == 0)
                {
                    // Edit                    
                    db.Ward.AddObject(model);
                }
                else
                {
                    // Add new      
                    db.AttachTo("Ward", model);
                    db.ObjectStateManager.ChangeObjectState(model, System.Data.EntityState.Modified);
                }
                db.SaveChanges();
                return RedirectToAction("WardIndex");
            }
            catch
            {
                ViewBag.District = CommonFunction.BuildDropdown(db.District.Select(m => m.ID).ToArray(),
                db.District.Select(m => m.Title).ToArray(), model == null ? 0 : model.DistrictID, null);
                return View(model);
            }
        }

        public ActionResult WardDelete(string arrayID = "")
        {
            try
            {
                // TODO: Add delete logic here
                var lstID = arrayID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var db = DB.Entities;
                if (lstID.Length > 0)
                {
                    foreach (var item in lstID)
                    {
                        int tmpID = int.Parse(item);
                        var obj = db.Ward.FirstOrDefault(m => m.ID == tmpID);
                        db.Ward.DeleteObject(obj);
                    }
                    db.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("WardIndex");
        }

        #region District
        public ActionResult DistrictIndex()
        {
            return View(DB.Entities.District.ToList());
        }
        public ActionResult DistrictUpdate(int? id = 0)
        {
            var db = DB.Entities;

            var model = DB.Entities.District.FirstOrDefault(m => m.ID == id);
            ViewBag.Province = CommonFunction.BuildDropdown(db.Province.Select(m => m.ID).ToArray(),
                db.Province.Select(m => m.Title).ToArray(), model == null ? 0 : model.ProvinceID, null);
            return View(model);
        }
        [HttpPost]
        public ActionResult DistrictUpdate(District model, FormCollection frm)
        {
            var db = DB.Entities;
            try
            {
                if (model.ID == 0)
                {
                    // Edit                    
                    db.District.AddObject(model);
                }
                else
                {
                    // Add new      
                    db.AttachTo("District", model);
                    db.ObjectStateManager.ChangeObjectState(model, System.Data.EntityState.Modified);
                }
                db.SaveChanges();
                return RedirectToAction("DistrictIndex");
            }
            catch
            {
                ViewBag.Province = CommonFunction.BuildDropdown(db.Province.Select(m => m.ID).ToArray(),
                db.Province.Select(m => m.Title).ToArray(), model == null ? 0 : model.ProvinceID, null);
                return View(model);
            }
        }

        public ActionResult DistrictDelete(string arrayID = "")
        {
            try
            {
                // TODO: Add delete logic here
                var lstID = arrayID.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var db = DB.Entities;
                if (lstID.Length > 0)
                {
                    foreach (var item in lstID)
                    {
                        int tmpID = int.Parse(item);
                        var obj = db.District.FirstOrDefault(m => m.ID == tmpID);
                        db.District.DeleteObject(obj);
                    }
                    db.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("DistrictIndex");
        }
        #endregion
    }
}
