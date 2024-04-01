using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using lab_work.Data;
using lab_work.Areas.ProjectManagement.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace lab_work.Areas.ProjectManagement.Controllers
{

    [Area("ProjectManagement")]
    [Route("[area]/[controller]/[action]")]
    public class ProjectController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProjectController(ApplicationDbContext db)
        {
            _db = db;
        }


        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var projects = await _db.Projects.ToListAsync();
            return View(_db.Projects.ToList());

        }


        [HttpGet("")]
        public async Task<IActionResult> Details(int id)
        {
            var project = await _db.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
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


        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _db.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId, Name, Description, StartDate, EndDate, Status")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(project);
                    await _db.SaveChangesAsync();
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


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Project? projectFromDb = await _db.Projects.FindAsync(id);
            //Project? projectFromDb = _db.Projects.FirstOrDefault(u=>u.Id==id);
            //Project? projectFromDb = _db.Projects.Where(u => u.Id == id).FirstOrDefault();

            if (projectFromDb == null)
            {
                return NotFound();
            }
            return View(projectFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            Project obj = await _db.Projects.FindAsync(id);
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