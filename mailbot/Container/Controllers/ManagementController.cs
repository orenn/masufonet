using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Container;
using Container.Authorize;
using Container.Logic;

namespace Container.Controllers
{
    [Authorize]
    public class ManagementController : Controller
    {
        private meichalimEntities db = new meichalimEntities();

        // GET: Management
        [RoleAuthorizeAttribute(1)]
        public ActionResult ClientsList()
        {
            return View(db.tbl_users.ToList());
        }

       
        //public ActionResult PaperTypesList()
        //{
        //    return View(db.tbl_Papertypes.ToList());
        //}
        // GET: Management/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_users tbl_users = db.tbl_users.Find(id);
            if (tbl_users == null)
            {
                return HttpNotFound();
            }
            return View(tbl_users);
        }

        // GET: Management/Create
        [RoleAuthorizeAttribute(1)]
        public ActionResult Create()
        {
            return View();
        }
        
        // POST: Management/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RoleAuthorizeAttribute(1)]
        public ActionResult Create([Bind(Include = "UserNum,UserName,User_Level,UserEmail,password")] tbl_users tbl_users)
        {
            if (ModelState.IsValid)
            {
                tbl_users.password = GenericHandler.EncodePasswordToBase64(tbl_users.password);
                db.tbl_users.Add(tbl_users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_users);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult EditPaper([Bind(Include = "PaperId,PaperTitle")] tbl_Papertypes paper)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        if (paper.PaperId == 0)
        //        {
        //            db.tbl_Papertypes.Add(paper);
        //            db.SaveChanges();
                    
        //        }
        //        else
        //        {
        //            db.Entry(paper).State = EntityState.Modified;
        //            db.SaveChanges();
        //        }
        //        return RedirectToAction("PaperTypesList");
        //    }
            
        //    return View(paper);
        //}

        // GET: Management/Edit/5
        [RoleAuthorizeAttribute(1)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_users tbl_users = db.tbl_users.Find(id);
            if (tbl_users == null)
            {
                return HttpNotFound();
            }
            return View(tbl_users);
        }

        // POST: Management/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [RoleAuthorizeAttribute(1)]
        public ActionResult Edit([Bind(Include = "UserNum,UserName,User_Level,UserEmail,password")] tbl_users tbl_users)
        {
            if (ModelState.IsValid)
            {
                tbl_users.password = GenericHandler.EncodePasswordToBase64(tbl_users.password);
                db.Entry(tbl_users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_users);
        }

        //// GET: Management/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tbl_users tbl_users = db.tbl_users.Find(id);
        //    if (tbl_users == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tbl_users);
        //}

        // POST: Management/Delete/5
        [HttpPost, ActionName("Delete")]
        [RoleAuthorizeAttribute(1)]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_users tbl_users = db.tbl_users.Find(id);
            db.tbl_users.Remove(tbl_users);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        [RoleAuthorizeAttribute(1)]
        public ActionResult Index()
        {
            return View();
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
