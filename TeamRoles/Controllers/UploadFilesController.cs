using System;
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
        [HttpPost]
        public ActionResult UploadAnswear(HttpPostedFileBase file,string coursename,string teachername,string assignment)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string filename = Path.GetFileName(file.FileName);
                    string filepath = Path.Combine(Server.MapPath("~/Users\\" + teachername + "\\" + coursename +"\\Submits\\" + assignment), filename);
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

        public ActionResult GetSubmits(string coursename, string username, string assignment)
        {
            try
            {
                var dir = new System.IO.DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "Users\\" + username + "\\" + coursename+"\\Submits\\"+assignment);

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
                model.assignmentname = assignment;
                model.mode = "submits";
                return View(model);
            }
            catch
            {
                ViewBag.Message = "Files Not Uploaded Yet";
                return View(ViewBag.Message);
            }
        }

        
        public ActionResult DownloadFile(string filename, string coursename, string username, string filefolder, string mode)
        {
            string path = "";

            if (mode=="assignments")
            {
                path = AppDomain.CurrentDomain.BaseDirectory + "Users\\" + username + "\\" + coursename + "\\";
            }
            else if(mode=="submits")
            {
                path = AppDomain.CurrentDomain.BaseDirectory + "Users\\" + username + "\\" + coursename + "\\Submits\\" + filefolder+"\\";
            }
            else if (mode == "lectures")
            {
                path = AppDomain.CurrentDomain.BaseDirectory + "Users\\" + username + "\\" + coursename + "\\Lectures\\";
            }


            byte[] fileBytes = System.IO.File.ReadAllBytes(path + filename);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }



        public ActionResult DeleteFile(string filename, string coursename, string username, string filefolder, string mode)
        {
            string path = "";

            if (mode == "assignments")
            {
                path = AppDomain.CurrentDomain.BaseDirectory + "Users\\" + username + "\\" + coursename + "\\";
            }
            else if (mode == "submits")
            {
                path = AppDomain.CurrentDomain.BaseDirectory + "Users\\" + username + "\\" + coursename + "\\Submits\\" + filefolder+"\\";
            }
            else if(mode == "lectures")
            {
                path = AppDomain.CurrentDomain.BaseDirectory + "Users\\" + username + "\\" + coursename + "\\Lectures\\";
            }

            System.IO.File.Delete(path + filename);

            return RedirectToAction("ListAssignments","Assignments",new { coursename = coursename });
        }
    }
}