﻿using B4B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B4B.Controllers
{
    public class FileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: File
        [AllowAnonymous]
        public ActionResult Index(int id)
        {
            var fileToRetrieve = db.Profiles.Find(id);

            if(fileToRetrieve.PhotoBytes != null && fileToRetrieve.PhotoType != null)
                return File(fileToRetrieve.PhotoBytes, fileToRetrieve.PhotoType);

            return View();
        }
    }
}