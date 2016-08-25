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
    public class MedicalInfoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MedicalInfoes
        public ActionResult Index()
        {
            var medicalInfoes = db.MedicalInfoes.Include(m => m.Profiles);
            return View(medicalInfoes.ToList());
        }

        // GET: MedicalInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalInfo medicalInfo = db.MedicalInfoes.Find(id);
            if (medicalInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProfileID = new SelectList(db.Profiles, "ProfileID", "FirstName", medicalInfo.ProfileID);
            return View(medicalInfo);
        }

        // GET: MedicalInfoes/Create
        public ActionResult Create()
        {
            ViewBag.ProfileID = new SelectList(db.Profiles, "ProfileID", "FirstName");
            return View();
        }

        // POST: MedicalInfoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MedicalInfoID,MedicalCondition,Symptoms,RelatedInformation,ProfileID")] MedicalInfo medicalInfo)
        {
            if (ModelState.IsValid)
            {
                db.MedicalInfoes.Add(medicalInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProfileID = new SelectList(db.Profiles, "ProfileID", "FirstName", medicalInfo.ProfileID);
            return View(medicalInfo);
        }

        // GET: MedicalInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalInfo medicalInfo = db.MedicalInfoes.Find(id);
            if (medicalInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProfileID = new SelectList(db.Profiles, "ProfileID", "FirstName", medicalInfo.ProfileID);
            return View(medicalInfo);
        }

        // POST: MedicalInfoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MedicalInfoID,MedicalCondition,Symptoms,RelatedInformation,ProfileID")] MedicalInfo medicalInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medicalInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProfileID = new SelectList(db.Profiles, "ProfileID", "FirstName", medicalInfo.ProfileID);
            return View(medicalInfo);
        }

        // GET: MedicalInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MedicalInfo medicalInfo = db.MedicalInfoes.Find(id);
            if (medicalInfo == null)
            {
                return HttpNotFound();
            }
            return View(medicalInfo);
        }

        // POST: MedicalInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MedicalInfo medicalInfo = db.MedicalInfoes.Find(id);
            db.MedicalInfoes.Remove(medicalInfo);
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
