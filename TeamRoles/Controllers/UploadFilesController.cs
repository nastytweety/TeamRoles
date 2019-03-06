using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeamRoles.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;

namespace TeamRoles.Controllers
{

    [Authorize]
    public class UploadFilesController : Controller
    {

        private ApplicationDbContext db;
        private UserManager<ApplicationUser> _userManager;
        private static string _coursename { get; set; }

        public UploadFilesController()
        {
            db = new ApplicationDbContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }
            // GET: UploadFiles
        public ActionResult Index(string coursename)
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

        }

        public ActionResult Download()
        {
            try
            {
                var dir = new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "Users\\" + User.Identity.GetUserName() + "\\" + _coursename);

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
                return View(ViewBag.Message);
            }
        }


        public ActionResult DownloadFile(string text)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Users\\" + User.Identity.GetUserName() + "\\" + _coursename;

            byte[] fileBytes = System.IO.File.ReadAllBytes(path + text);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, text);
        }


        public ActionResult DeleteFile(string text)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Users\\" + User.Identity.GetUserName() + "\\" + _coursename;

            System.IO.File.Delete(path + text);

            return RedirectToAction("Index");
        }
    }
}