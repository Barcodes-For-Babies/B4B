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
using System.Collections.Generic;
using System.Drawing;
//using QRCoder;
using System.IO;
using System.Text.RegularExpressions;
using QRCoder;

namespace B4B.Controllers
{
    public class ProfilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private byte[] byteArray;

        //This method will send a text to the emergency contact provided by admin
        public void SendEmergencyText(string output)
        {
           var client = new TwilioRestClient(WebConfigurationManager.AppSettings["TWILIO_SID"],
                WebConfigurationManager.AppSettings["TWILIO_AUTHTOKEN"]);

                var result = client.SendMessage(WebConfigurationManager.AppSettings["TWILIO_PHONE"], output, "Emergency button has been activated!");
          
        }

        public ActionResult EmergencyText(int? id)
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

            string pattern = "[0-9]";
            string phoneNumber = "+1";
            string phoneNumber2 = "+1";

            if (profile.EmergencyPhone != null)
            {
                foreach (Match m in Regex.Matches(profile.EmergencyPhone, pattern))
                    phoneNumber += m.Value;

                SendEmergencyText(phoneNumber);

            }
            if (profile.SecondEmergencyPhone != null)
            {
                foreach (Match m in Regex.Matches(profile.SecondEmergencyPhone, pattern))
                    phoneNumber2 += m.Value;

                SendEmergencyText(phoneNumber2);
            }
            return RedirectToAction("Index");
        }

        private Bitmap renderQRCode()
        {
            string url = Request.Url.ToString();
            PayloadGenerator.Url myUrl = new PayloadGenerator.Url(url);
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(myUrl.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            //Bitmap qrCodeImage = qrCode.GetGraphic(20);
            
            Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.BlueViolet, Color.Black,true);

            return qrCodeImage;
        }


        public FileContentResult myAction()
        {
            Bitmap qrCodeImage = renderQRCode();
            byteArray = ImageToByte2(qrCodeImage);
            return new FileContentResult(byteArray, "image/jpg");
        }

        public static byte[] ImageToByte2(Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
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
                ViewBag.CurrentUser = CurrentUser;
                return View(profile);
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: Profiles/Create
        public ActionResult Create()
        {
            return View(new WizardViewModel());
        }

        // POST: Profiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase upload, WizardViewModel wizardViewModel)
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

                    wizardViewModel._profile.PhotoName = avatar.PhotoName;           //Adds name of photo to db
                    wizardViewModel._profile.FileType = avatar.FileType;             //Adds FileType of photo to db
                    wizardViewModel._profile.PhotoType = avatar.PhotoType;           //Adds the extension type of photo to db
                    wizardViewModel._profile.PhotoBytes = avatar.PhotoBytes;         //Adds the byte array representation of photo to db
                }
                var profileID = 0;
                var profilesList = db.Profiles.ToList();
                profileID = profilesList.Count() + 1;

                var medInfoID = 0;
                var medInfosList = db.MedicalInfoes.ToList();
                medInfoID = medInfosList.Count() + 1;
              
                wizardViewModel._profile.Admin = CurrentUser;                               //Assigns current user to as admin of profile
                wizardViewModel._profile.ProfileID = profileID;
                db.Profiles.Add(wizardViewModel._profile);                                  //Adds profile object into database
                wizardViewModel._medicalInfo.MedicalInfoID = medInfoID;
                wizardViewModel._medicalInfo.ProfileID = profileID;                 //Assigns profile to medicalInfo
                db.MedicalInfoes.Add(wizardViewModel._medicalInfo);                         //Adds medical info object to database
                db.SaveChanges();                                                           //Saves everything just added to database
                return RedirectToAction("Admin", "Home");                                   //Redirect you to admin home
            }

            return View(wizardViewModel);
        }

        // GET: Profiles/Edit/5
        public ActionResult Edit(int? id, List<MedicalInfo> medInfos)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WizardViewModel wizardViewModel = new WizardViewModel();
            Profile profile = db.Profiles.Find(id);
            wizardViewModel._profile = profile;

            medInfos = db.MedicalInfoes.ToList();          
            foreach (var mi in medInfos)
            {
                if (mi.ProfileID == profile.ProfileID)
                    wizardViewModel._medicalInfo = db.MedicalInfoes.Find(mi.ProfileID);
            }
            
            if (profile == null)
            {
                return HttpNotFound();
            }
            if (CurrentUser.Profiles.Contains(wizardViewModel._profile))
            {
                return View(wizardViewModel);
            }
            
            
            return RedirectToAction("Login", "Account");
         }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "_profile, _medicalInfo")] WizardViewModel wizardViewModel, HttpPostedFileBase upload)
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

                    wizardViewModel._profile.PhotoName = avatar.PhotoName;           //Adds name of photo to db
                    wizardViewModel._profile.FileType = avatar.FileType;             //Adds FileType of photo to db
                    wizardViewModel._profile.PhotoType = avatar.PhotoType;           //Adds the extension type of photo to db
                    wizardViewModel._profile.PhotoBytes = avatar.PhotoBytes;         //Adds the byte array representation of photo to db
                }
                wizardViewModel._profile.Admin = CurrentUser;
                db.Entry(wizardViewModel._profile).State = EntityState.Modified;            //Adds profile object into database

                wizardViewModel._medicalInfo.ProfileID = wizardViewModel._profile.ProfileID;
                var medicalInfoList = db.MedicalInfoes.ToList();
                foreach(var mi in medicalInfoList)
                {
                    if(mi.ProfileID == wizardViewModel._profile.ProfileID)
                    {
                        wizardViewModel._medicalInfo.MedicalInfoID = mi.MedicalInfoID;
                    }
                }
                var medicalInfoDB = db.MedicalInfoes.Find(wizardViewModel._medicalInfo.MedicalInfoID);

                wizardViewModel._medicalInfo.Profiles = db.Profiles.Find(wizardViewModel._profile.ProfileID);
                db.Entry(medicalInfoDB).CurrentValues.SetValues(wizardViewModel._medicalInfo);
                db.Entry(medicalInfoDB).State = EntityState.Modified;


                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(wizardViewModel._profile);
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
