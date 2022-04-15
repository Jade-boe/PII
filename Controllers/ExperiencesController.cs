using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Encodings.Web;

namespace NextStep.Controllers
{
    public class ExperiencesController : Controller
    {
        // 
        // GET: /Experiences/

        public IActionResult Index()
        {
            return View();
        }

        // 
        // GET: /Experiences/Recherche/ 
        public IActionResult Recherche(DateTime dateDebut, DateTime dateFin, string pays, string ville)
        {
            ViewData["dateDebut"] = dateDebut;
            ViewData["dateFin"] = dateFin;
            ViewData["pays"] = pays;
            ViewData["ville"] = ville;
            // appeler cette méthode avec un form en Home en transmettant les dates 
            return View();
        }
    }
}