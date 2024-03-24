using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using lab_work.Data;
using lab_work.Areas.ProjectManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace lab_work.Areas.ProjectManagement.Controllers
{

    [Area("ProjectManagement")]
    [Route("[area]/[controller]/[action]")]
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TaskController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index(int projectId)
        {
            var tasks = _db.ProjectTasks
                .Where(task => task.ProjectId == projectId)
                .ToList();

            ViewBag.ProjectId = projectId;
            return View(tasks);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var task = _db.ProjectTasks
                       .Include(t => t.Project)
                       .FirstOrDefault(task => task.ProjectTaskId == id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpGet]
        public IActionResult Create(int projectId)
        {
            var project = _db.Projects.Find(projectId);
            if (project == null)
            {
                return NotFound();
            }

            var task = new ProjectTask
            {
                ProjectId = projectId
            };
            return View(task);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (ModelState.IsValid)
            {
                _db.ProjectTasks.Add(task);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }
            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpGet]
        public IActionResult Edit(int projectTaskId)
        {
            var task = _db.ProjectTasks
           .Include(t => t.Project)
           .FirstOrDefault(t => t.ProjectTaskId == projectTaskId);

            if (task == null)
            {
                return NotFound();
            }
            ViewBag.Projects = new SelectList(_db.Projects, "ProjectTaskId", "Name", task.ProjectId);
            return View(task);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProjectTaskId", "Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (id != task.ProjectTaskId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Update(task);
                _db.SaveChanges();
                return RedirectToAction("Index", new { projectId = task.ProjectId });
            }
            ViewBag.Projects = new SelectList(_db.Projects, "ProjectTaskId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpGet]
        public IActionResult Delete(int projectTaskId)
        {
            var task = _db.ProjectTasks
           .Include(t => t.Project)
           .FirstOrDefault(t => t.ProjectTaskId == projectTaskId);

            if (task == null)
            {
                return NotFound();
            }
            ViewBag.Projects = new SelectList(_db.Projects, "ProjectTaskId", "Name", task.ProjectId);
            return View(task);
        }


        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int projectTaskId)
        {
            var task = _db.ProjectTasks.Find(projectTaskId);
            if (task != null)
            {
                _db.ProjectTasks.Remove(task);
                _db.SaveChanges();
                return RedirectToAction("Index", new { projectId = task.ProjectId });
            }
            return NotFound();
        }

        //public async Task<IActionResult> Search(int projectId, string searchString)
        //{
        //    var taskQuery = _db.ProjectTasks.Where(t => t.ProjectId == projectId);

        //    if (!string.IsNullOrEmpty(searchString))
        //    {
        //        taskQuery = taskQuery.Where(t => t.Title.Contains(searchString)
        //                                          || t.Description.Contains(searchString));
        //    }

        //    var tasks = await taskQuery.ToListAsync();
        //    ViewBag.ProjectId = projectId;
        //    return View("Index", tasks);
        //}

    }
}

