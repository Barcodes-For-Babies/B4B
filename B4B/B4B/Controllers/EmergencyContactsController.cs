using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using B4B.Models;

namespace B4B.Controllers
{
    public class EmergencyContactsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EmergencyContacts
        public ActionResult Index()
        {
            return View(db.EmergencyContacts.ToList());
        }

        // GET: EmergencyContacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmergencyContact emergencyContact = db.EmergencyContacts.Find(id);
            if (emergencyContact == null)
            {
                return HttpNotFound();
            }
            return View(emergencyContact);
        }

        // GET: EmergencyContacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmergencyContacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmergencyContactID,EmergencyName,EmergencyPhone")] EmergencyContact emergencyContact)
        {
            if (ModelState.IsValid)
            {
                db.EmergencyContacts.Add(emergencyContact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emergencyContact);
        }

        // GET: EmergencyContacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmergencyContact emergencyContact = db.EmergencyContacts.Find(id);
            if (emergencyContact == null)
            {
                return HttpNotFound();
            }
            return View(emergencyContact);
        }

        // POST: EmergencyContacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmergencyContactID,EmergencyName,EmergencyPhone")] EmergencyContact emergencyContact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emergencyContact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emergencyContact);
        }

        // GET: EmergencyContacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmergencyContact emergencyContact = db.EmergencyContacts.Find(id);
            if (emergencyContact == null)
            {
                return HttpNotFound();
            }
            return View(emergencyContact);
        }

        // POST: EmergencyContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmergencyContact emergencyContact = db.EmergencyContacts.Find(id);
            db.EmergencyContacts.Remove(emergencyContact);
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
