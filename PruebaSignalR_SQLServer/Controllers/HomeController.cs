using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PruebaSignalR_SQLServer.Models;
using PruebaSignalR_SQLServer.Models.Repositorio;

namespace PruebaSignalR_SQLServer.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult ListarPersonas()
        {
            String con = @"Server=DESKTOP-MRNQERB\SQLEXPRESS; Database=PruebaSignalR; Trusted_Connection=True; MultipleActiveResultSets=true";
            PersonaRepositorio pr = new PersonaRepositorio(con);
            List<Persona> personas = pr.Listar();
            return Json(personas);
        }
    }
}
