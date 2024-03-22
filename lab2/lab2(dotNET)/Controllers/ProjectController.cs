using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lab2_dotNET_.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace lab2_dotNET_.Controllers
{
    public class ProjectController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var projects = new List<Project>()
            {
                new Project{ProjectId = 1, Name = "Project 1", Description = "MVC Project based on ASP.NET" },
                new Project{ProjectId = 2, Name = "Project 2", Description = "DSA Project" },
                new Project{ProjectId = 3, Name = "Project 3", Description = "Python Project" }

            };
            if (projects == null)
            {
                return NotFound(); // or handle the case where the project is not found
            }

            return View(projects);


        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var projects = new Project { ProjectId = 1, Name = "Project 1", Description = "MVC Project based on ASP.NET" };
            return View(projects);
        }


        [HttpPost]
        public IActionResult Create(Project project)
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}


