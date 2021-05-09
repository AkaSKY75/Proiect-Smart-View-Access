using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using dotnetnewmvc.Models;
using System.IO;
using Microsoft.ClearScript.V8;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.AspNetCore.Http;

namespace dotnetnewmvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.failed_login = 0;
            return View();
        }

        [HttpPost]
        public IActionResult Index(IFormCollection form)
        {
            if (form["email"].ToString() != "" && form["password"].ToString() != "")
            {
                return View("Firebase", new Angajat(form["email"].ToString(), form["password"].ToString()));
            }
            else if (form["User"].ToString() != "")
            {
                if (form["is_Administrator"].ToString() == "true")
                    return View("Index_Administrator");
                else
                    return View("Index_Angajat");

            }
            else if(form["administrator_menu_item"].ToString() != "")
            {
                switch(form["administrator_menu_item"].ToString())
                {
                    case "1":   return View("Administrator_Menu_1");
                    case "2":   return View("Administrator_Menu_2");
                    case "3":   return View("Administrator_Menu_3");
                    case "4":   return View("Administrator_Menu_4");
                }
                return View("Index_Administrator");
            }
            else
            {
                ViewBag.failed_login = 1;
                return View();

            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
