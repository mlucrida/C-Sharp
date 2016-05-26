using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Entities;

namespace Stash.Controllers
{
    public class BoxesController : Controller
    {
        private StashEntities db = new StashEntities();

        // GET: Boxes
        public ActionResult Index()
        {
            return View(db.Boxes.ToList());
        }

        // GET: Boxes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Box box = db.Boxes.Find(id);
            if (box == null)
            {
                return HttpNotFound();
            }
            return View(box);
        }

        // GET: Boxes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Boxes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "boxkey,name,object_description,create_date,delete_date")] Box box)
        {
            if (ModelState.IsValid)
            {
                db.Boxes.Add(box);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(box);
        }

        // GET: Boxes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Box box = db.Boxes.Find(id);
            if (box == null)
            {
                return HttpNotFound();
            }
            return View(box);
        }

        // POST: Boxes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "boxkey,name,object_description,create_date,delete_date")] Box box)
        {
            if (ModelState.IsValid)
            {
                db.Entry(box).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(box);
        }

        // GET: Boxes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Box box = db.Boxes.Find(id);
            if (box == null)
            {
                return HttpNotFound();
            }
            return View(box);
        }

        // POST: Boxes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Box box = db.Boxes.Find(id);
            db.Boxes.Remove(box);
            db.SaveChanges();
            return RedirectToAction("Index");
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
