using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PruebaSignalR.Models;

namespace PruebaSignalR.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Indexeeee
            return View();
        }       
    }
}
