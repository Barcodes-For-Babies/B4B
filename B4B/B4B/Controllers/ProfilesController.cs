﻿using System.Data.Entity;
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
using System.IO;
using System.Text.RegularExpressions;
using QRCoder;
using System.Xml.Linq;

namespace B4B.Controllers
{
    public class ProfilesController : Controller
    {
        static string baseUri = "https://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&key=" + WebConfigurationManager.AppSettings["GOOGLE_API_KEY"];
        string address;
        private ApplicationDbContext db = new ApplicationDbContext();
        private byte[] byteArray;

        //This method will send a text to the emergency contact provided by admin
        public void SendEmergencyText(string output, string address)
        {
           var client = new TwilioRestClient(WebConfigurationManager.AppSettings["TWILIO_SID"],
                WebConfigurationManager.AppSettings["TWILIO_AUTHTOKEN"]);
                
                var result = client.SendMessage(WebConfigurationManager.AppSettings["TWILIO_PHONE"], output, "Emergency button has been activated at :/n" + address);
          
        }

        [HttpPost]
        public ActionResult EmergencyText(int? id, string Latitude, string Longitude)
        {
            string pattern = "[0-9]";
            string phoneNumber = "+1";
            string phoneNumber2 = "+1";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }

            GoogleGeoCode(Latitude, Longitude);



            if (profile.EmergencyPhone != null)
            {
                foreach (Match m in Regex.Matches(profile.EmergencyPhone, pattern))
                    phoneNumber += m.Value;

                SendEmergencyText(phoneNumber, address);

            }
            if (profile.SecondEmergencyPhone != null)
            {
                foreach (Match m in Regex.Matches(profile.SecondEmergencyPhone, pattern))
                    phoneNumber2 += m.Value;

                SendEmergencyText(phoneNumber2, address);
            }
            return RedirectToAction("Index");
        }
        void GoogleGeoCode(string lat, string lng)
        {
            string url = string.Format(baseUri, lat, lng);

            dynamic googleResults = new Uri(url).GetDynamicJsonObject();
            //foreach (var result in googleResults.results)
            //{
            //    Console.WriteLine("[" + result.geometry.location.lat + "," + result.geometry.location.lng + "] " + result.formatted_address);
                
            //}
            var result = googleResults.results[0];
            address = result.formatted_address;
        }
  






        //public ActionResult EditProfile(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Profile profile = db.Profiles.Find(id);
        //    if (profile == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    if (profile.FirstName != null)
        //    {

        //        return RedirectToAction("Edit", "Profile");
        //    }
           
        //    return RedirectToAction("Index");
        //}








        private Bitmap renderQRCode(int id)
        {
            string url = "https://localhost:44378/Profiles/Details/" + id;
            PayloadGenerator.Url myUrl = new PayloadGenerator.Url(url);
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(myUrl.ToString(), QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            //Bitmap qrCodeImage = qrCode.GetGraphic(20);
            
            Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White,true);

            return qrCodeImage;
        }


        public FileContentResult myAction(int id)
        {
            Bitmap qrCodeImage = renderQRCode(id);
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
        [AllowAnonymous]
        public ActionResult Details(int? id, List<MedicalInfo> medInfos)
            
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WizardViewModel wizardViewModel = new WizardViewModel();
            Profile profile = db.Profiles.Find(id);

            if(profile == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
            
            //if(User.Identity.GetUserId() == null && id != null)
            //{
            //    var pics = new FileController().Index(Convert.ToInt32(id);
            //}
             return View(wizardViewModel);
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
            if (ModelState.IsValid && wizardViewModel._profile.disclaimer)
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
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WizardViewModel wizardViewModel = new WizardViewModel();
            Profile profile = db.Profiles.Find(id);

            wizardViewModel.medInfoList = profile.MedicalInfos.ToList();
            wizardViewModel._profile = profile;

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
        public ActionResult Edit(WizardViewModel wizardViewModel, HttpPostedFileBase upload)
        {
            wizardViewModel._profile.Admin = CurrentUser;
            for (int i = 0; i < wizardViewModel.medInfoList.Count; i++)
            {
                wizardViewModel.medInfoList[i].ProfileID = wizardViewModel._profile.ProfileID;
            }
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
                db.Entry(wizardViewModel._profile).State = EntityState.Modified;            //Adds profile object into database


                for(int i = 0; i < wizardViewModel.medInfoList.Count; i++)
                {
                    if(wizardViewModel.medInfoList[i].MedicalInfoID == 0 && db.MedicalInfoes.First() != wizardViewModel.medInfoList[i])
                    {
                        db.MedicalInfoes.Add(wizardViewModel.medInfoList[i]);
                    } else
                    {
                         var medicalInfoDB = db.MedicalInfoes.Find(wizardViewModel.medInfoList[i].MedicalInfoID);
                        wizardViewModel.medInfoList[i].Profile = db.Profiles.Find(wizardViewModel._profile.ProfileID);
                        db.Entry(medicalInfoDB).CurrentValues.SetValues(wizardViewModel.medInfoList[i]);
                        db.Entry(medicalInfoDB).State = EntityState.Modified;
                    }                 
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }



            return View(wizardViewModel);
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
