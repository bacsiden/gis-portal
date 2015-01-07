using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NationalIT.Controllers
{
    [Authorize]
    public class LoaiBDSController : BaseController
    {
        public ActionResult Index()
        {
            return View(DB.Entities.LoaiBDS.ToList());
        }
        //
        public ActionResult NewOrEdit(int? id = 0)
        {
            var db = DB.Entities;

            var model = DB.Entities.LoaiBDS.FirstOrDefault(m => m.ID == id);
            return View(model);
        }

        [HttpPost]
        public ActionResult NewOrEdit(LoaiBDS model, FormCollection frm)
        {
            var db = DB.Entities;
            try
            {
                if (model.ID == 0)
                {
                    // Edit                    
                    db.LoaiBDS.AddObject(model);
                }
                else
                {
                    // Add new      
                    db.AttachTo("LoaiBDS", model);
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
                        var obj = db.LoaiBDS.FirstOrDefault(m => m.ID == tmpID);
                        db.LoaiBDS.DeleteObject(obj);
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
