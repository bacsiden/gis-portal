using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NationalIT.Controllers
{
    [Authorize]
    public class MoneyFilterController : BaseController
    {
        public ActionResult Index()
        {
            return View(DB.Entities.MoneyFilter.ToList());
        }
        //
        public ActionResult NewOrEdit(int? id = 0)
        {
            var db = DB.Entities;

            var model = DB.Entities.MoneyFilter.FirstOrDefault(m => m.ID == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult NewOrEdit(MoneyFilter model, FormCollection frm)
        {
            var db = DB.Entities;
            try
            {
                if (model.ID == 0)
                {
                    // Edit                    
                    db.MoneyFilter.AddObject(model);
                }
                else
                {
                    // Add new      
                    db.AttachTo("MoneyFilter", model);
                    db.ObjectStateManager.ChangeObjectState(model, System.Data.EntityState.Modified);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        public ActionResult Delete(string arrayID = "")
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
                        var obj = db.MoneyFilter.FirstOrDefault(m => m.ID == tmpID);
                        db.MoneyFilter.DeleteObject(obj);
                    }
                    db.SaveChanges();
                }
            }
            catch
            {

            }
            return RedirectToAction("Index");
        }
    }
}
