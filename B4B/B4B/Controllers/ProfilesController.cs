using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using B4B.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Twilio;
using System;
using System.Web.Configuration;

namespace B4B.Controllers
{
    public class ProfilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //This method will send a text to the emergency contact provided by admin
        public ActionResult SendEmergencyText(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }

            var client = new TwilioRestClient(WebConfigurationManager.AppSettings["TWILIO_SID"],
            WebConfigurationManager.AppSettings["TWILIO_AUTHTOKEN"]);
            
            foreach (var contact in profile.EmergencyContacts)
            {
                var result = client.SendMessage(WebConfigurationManager.AppSettings["TWILIO_PHONE"], contact.EmergencyPhone, "Emergency button has been activated!");
            }

            return RedirectToAction("Index");
        }
       
        private ApplicationUser CurrentUser
        {
            get
            {
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
                return currentUser;
            }
        }

        // GET: Profiles
        public ActionResult Index()
        {
            return View(CurrentUser.Profiles.ToList());
        }

        // GET: Profiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            if (CurrentUser.Profiles.Contains(profile))
            {
                return View(profile);
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: Profiles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProfileID,FirstName,LastName,PhotoID,StreetAdress,City,State,ZipCode,MedicalInfos,EmergencyName,EmergencyPhone")] Profile profile, HttpPostedFileBase upload, WizardViewModel wizardViewModel)
        {            
            if (ModelState.IsValid)
            {
                // if user uploaded a photo this runs (gets picture)
                if (upload != null && upload.ContentLength > 0)
                {
                    var avatar = new Profile
                    {
                        PhotoName = System.IO.Path.GetFileName(upload.FileName),
                        FileType = FileType.Avatar,
                        PhotoType = upload.ContentType
                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        avatar.PhotoBytes = reader.ReadBytes(upload.ContentLength);
                    }

                    profile.PhotoName = avatar.PhotoName;           //Adds name of photo to db
                    profile.FileType = avatar.FileType;             //Adds FileType of photo to db
                    profile.PhotoType = avatar.PhotoType;           //Adds the extension type of photo to db
                    profile.PhotoBytes = avatar.PhotoBytes;         //Adds the byte array representation of photo to db
                }
                
                wizardViewModel._profile.Admin = CurrentUser;                               //Assigns current user to as admin of profile
                db.Profiles.Add(wizardViewModel._profile);                                  //Adds profile object into database
                wizardViewModel._emergencyContact.User = CurrentUser;                       //Assigns current user as creator emergency contact 
                wizardViewModel._emergencyContact.Profiles = CurrentUser.Profiles;          //Assigns profile to emergency contact
                wizardViewModel._medicalInfo.ProfileID = profile.ProfileID;                 //Assigns profile to medicalInfo
                db.EmergencyContacts.Add(wizardViewModel._emergencyContact);                //Adds emergency contact object to database
                db.MedicalInfoes.Add(wizardViewModel._medicalInfo);                         //Adds medical info object to database
                db.SaveChanges();                                                           //Saves everything just added to database
                return RedirectToAction("Admin", "Home");                                   //Redirect you to admin home
            }

            return View(profile);
        }

        // GET: Profiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            if (CurrentUser.Profiles.Contains(profile))
            {
                return View(profile);
            }
            return RedirectToAction("Login", "Account");
         }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProfileID,FirstName,LastName,PhotoID,StreetAdress,City,State,ZipCode,MedicalInfoID,EmergencyName,EmergencyPhone")] Profile profile, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var avatar = new Profile
                    {
                        PhotoName = System.IO.Path.GetFileName(upload.FileName),
                        FileType = FileType.Avatar,
                        PhotoType = upload.ContentType
                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        avatar.PhotoBytes = reader.ReadBytes(upload.ContentLength);
                    }

                    profile.PhotoName = avatar.PhotoName;
                    profile.FileType = avatar.FileType;
                    profile.PhotoType = avatar.PhotoType;
                    profile.PhotoBytes = avatar.PhotoBytes;
                }
                db.Entry(profile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(profile);
        }

        // GET: Profiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            if (CurrentUser.Profiles.Contains(profile))
            {
                return View(profile);
            }
            return RedirectToAction("Login", "Account");
        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Profile profile = db.Profiles.Find(id);
            db.Profiles.Remove(profile);
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
