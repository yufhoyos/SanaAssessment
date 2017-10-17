using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SanaAssessment.Web.Models;
using SanaAssessment.Web.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SanaAssessment.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.Set<string>(SessionExtensions.SessionKeyStorageType, "SQL");
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
      
        public ViewComponentResult ChangeStorage(string storageSelected)
        {
            HttpContext.Session.Set<string>(SessionExtensions.SessionKeyStorageType, storageSelected);
            return ViewComponent("StorageConfiguration");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
