﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;   
using TeamRoles.Models;

namespace TeamRoles.Controllers
{

    [Authorize]
    public class UploadFilesController : Controller
    {

        /*private ApplicationDbContext db;
        private UserManager<ApplicationUser> _userManager;

        public UploadFilesController()
        {
            db = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }*/
            // GET: UploadFiles
       /* public ActionResult Index(string coursename)
        {
            _coursename = coursename;
            try
            {
                string username = User.Identity.GetUserName();
                var dir = new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "Users\\" + username +"\\" + _coursename);
                DirectoryInfo di = Directory.CreateDirectory(dir.ToString());

                System.IO.FileInfo[] fileNames = dir.GetFiles();

                List<string> items = new List<string>();

                foreach (var file in fileNames)
                {
                    items.Add(file.Name);
                }

                return View(items);
            }
            catch
            {
                ViewBag.Message = "Files Not Uploaded Yet";
                return View(ViewBag.MessageForList);
            }

        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string filepath = Path.Combine(Server.MapPath("~/Users\\"+User.Identity.GetUserName()+"\\"+_coursename), filename);
                    file.SaveAs(filepath);
                }

                var dir = new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "Users\\"+User.Identity.GetUserName() + "\\" + _coursename);

                System.IO.FileInfo[] fileNames = dir.GetFiles();

                List<string> items = new List<string>();

                foreach (var files in fileNames)
                {
                    items.Add(files.Name);
                }


                ViewBag.Message = "Uploaded Filed Saved.";
                return View(items);
            }
            catch
            {
                List<string> items = new List<string>();
                ViewBag.Message = "Uploaded Filed not Saved.";
                return View(items);
            }

        }*/

        [HttpPost]
        public ActionResult UploadAnswear(HttpPostedFileBase file,string coursename,string teachername)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string filepath = Path.Combine(Server.MapPath("~/Users\\" + teachername + "\\" + coursename +"\\Submits\\"), filename);
                    file.SaveAs(filepath);
                }

                ViewBag.Message = "Uploaded Filed Saved.";
                ViewBag.Coursename = coursename;
                return View();
            }
            catch
            {
                ViewBag.Message = "Uploaded Filed not Saved.";
                ViewBag.Coursename = coursename;
                return View();
            }

        }

        public ActionResult GetSubmits(string coursename, string username)
        {
            try
            {
                var dir = new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "Users\\" + username + "\\" + coursename+"\\Submits\\");

                System.IO.FileInfo[] fileNames = dir.GetFiles();

                List<string> items = new List<string>();

                foreach (var file in fileNames)
                {
                    items.Add(file.Name);
                }
                GetSubmitViewModel model = new GetSubmitViewModel();
                model.filenames.AddRange(items);
                model.username = username;
                model.coursename = coursename;
                model.mode = "submits";
                return View(model);
            }
            catch
            {
                ViewBag.Message = "Files Not Uploaded Yet";
                return View(ViewBag.Message);
            }
        }

        
        public ActionResult DownloadFile(string filename, string coursename, string username, string mode)
        {
            string path = "";

            if (mode=="assignments")
            {
                path = AppDomain.CurrentDomain.BaseDirectory + "Users\\" + username + "\\" + coursename + "\\";
            }
            else if(mode=="submits")
            {
                path = AppDomain.CurrentDomain.BaseDirectory + "Users\\" + username + "\\" + coursename + "\\Submits\\" ;
            }
            

            byte[] fileBytes = System.IO.File.ReadAllBytes(path + filename);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }



        public ActionResult DeleteFile(string filename, string coursename, string username, string mode)
        {
            string path = "";

            if (mode == "assignments")
            {
                path = AppDomain.CurrentDomain.BaseDirectory + "Users\\" + username + "\\" + coursename + "\\";
            }
            else if (mode == "submits")
            {
                path = AppDomain.CurrentDomain.BaseDirectory + "Users\\" + username + "\\" + coursename + "\\Submits\\";
            }

            System.IO.File.Delete(path + filename);

            return RedirectToAction("ListAssignments","Assignments",new { coursename = coursename });
        }
    }
}