using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using lab_work.Data;
using lab_work.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace lab_work.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProjectController(ApplicationDbContext db)
        {
            _db = db;
        }


        [HttpGet]
        public IActionResult Index()
        {
            /*
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
            */

            return View(_db.Projects.ToList());


        }

        public IActionResult Details(int id)
                {
                    var project = _db.Projects.FirstOrDefault(p => p.ProjectId == id);
                    if(project == null)
                    {
                        return NotFound();
                    }
                    return View(project);
                }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                _db.Projects.Add(project);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }




        [HttpGet]
        public IActionResult Edit(int id)
        {
            var project = _db.Projects.Find(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProjectId, Name, Description, StartDate, EndDate")] Project project)
        {
            if(id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(project);
                    _db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }

                }
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        private bool ProjectExists(int id)
        {
            return _db.Projects.Any(e => e.ProjectId == id);
        }

        //[HttpGet]
        //public IActionResult Delete(int id)
        //{
        //    var project = _db.Projects.FirstOrDefault(p => p.ProjectId == id);
        //    if (project == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(project);
        //}

        //[HttpPost, ActionName("DeleteConfirmed")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    var project = _db.Projects.Find(id);
        //    if(project != null)
        //    {
        //        _db.Projects.Remove(project);
        //        _db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    // When no project is found
        //    return NotFound();
        //}

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Project? projectFromDb = _db.Projects.Find(id);
            //Project? projectFromDb = _db.Projects.FirstOrDefault(u=>u.Id==id);
            //Project? projectFromDb = _db.Projects.Where(u => u.Id == id).FirstOrDefault();

            if (projectFromDb == null)
            {
                return NotFound();
            }
            return View(projectFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Project obj = _db.Projects.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Projects.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category Deleted Sucessfully";
            return RedirectToAction("Index");
        }

        [HttpGet("Search")]
        // [HttpGet("Search/{searchString?}")]
        public async Task<IActionResult> Search(string searchString)
        {
            var projectQuery = from p in _db.Projects
                               select p;

            bool searchPerformed = !string.IsNullOrEmpty(searchString);
            if (searchPerformed)
            {
                projectQuery = projectQuery.Where(p => p.Name.Contains(searchString)
                                                       || p.Description.Contains(searchString));
            }

            var projects = await projectQuery.ToListAsync();
            ViewData["SearchPerformed"] = searchPerformed;
            ViewData["SearchString"] = searchString;
            return View("Index", projects);
        }

        //[HttpPost]
        //public IActionResult Create(Project project)
        //{
        //    return RedirectToAction("Index");
        //}
    }
}



