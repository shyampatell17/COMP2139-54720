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

        [HttpGet("Index/{projectId:int?}")]
        public async Task<IActionResult> Index(int? projectId)
        {
            var tasksQuery = _db.ProjectTasks.AsQueryable();

            if (projectId.HasValue)
            {
                tasksQuery = tasksQuery.Where(t => t.ProjectId == projectId);

            }
            /*var tasks = _db.ProjectTasks
                .Where(task => task.ProjectId == projectId)
                .ToList();*/
            var tasks = await tasksQuery.ToListAsync();
            ViewBag.ProjectId = projectId;
            return View(tasks);
        }

        [HttpGet("Details/{projectTaskId:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var task = await _db.ProjectTasks
                       .Include(t => t.Project)
                       .FirstOrDefaultAsync(task => task.ProjectTaskId == id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpGet("Create/{projectId:int}")]
        public async Task<IActionResult> Create(int projectId)
        {
            var project = await _db.Projects.FindAsync(projectId);
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


        [HttpPost("Create/{projectId:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (ModelState.IsValid)
            {
                await _db.ProjectTasks.AddAsync(task);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
            }

            //Async call to retrive projects for Select List
            var projects = await _db.Projects.ToListAsync();

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        //[HttpGet("Edit/{projectTaskId:int}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int projectTaskId)
        {
            var task = await _db.ProjectTasks
           .Include(t => t.Project)
           .FirstOrDefaultAsync(t => t.ProjectTaskId == projectTaskId);

            if (task == null)
            {
                return NotFound();
            }

            //Async call to retrive projects for Select List
            var projects = await _db.Projects.ToListAsync();

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectTaskId", "Name", task.ProjectId);
            return View(task);
        }


        //[HttpPost("Edit/{id:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectTaskId", "Title", "Description", "ProjectId")] ProjectTask task)
        {
            if (id != task.ProjectTaskId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Update(task);
               await _db.SaveChangesAsync();
                return RedirectToAction("Index", new { projectId = task.ProjectId });
            }

            //Async call to retrive projects for Select List
            var projects = await _db.Projects.ToListAsync();

            ViewBag.Projects = new SelectList(_db.Projects, "ProjectTaskId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpGet("Delete/{projectTaskId:int}")]
        public async Task<IActionResult> Delete(int projectTaskId)
        {
            var task = await _db.ProjectTasks
           .Include(t => t.Project)
           .FirstOrDefaultAsync(t => t.ProjectTaskId == projectTaskId);

            if (task == null)
            {
                return NotFound();
            }
            ViewBag.Projects = new SelectList(_db.Projects, "ProjectTaskId", "Name", task.ProjectId);
            return View(task);
        }


        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int projectTaskId)
        {
            var task = await _db.ProjectTasks.FindAsync(projectTaskId);
            if (task != null)
            {
                _db.ProjectTasks.Remove(task);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", new { projectId = task.ProjectId });
            }
            return NotFound();
        }

        public async Task<IActionResult> Search(int projectId, string searchString)
        {
            var taskQuery = _db.ProjectTasks.Where(t => t.ProjectId == projectId);

            if (!string.IsNullOrEmpty(searchString))
            {
                taskQuery = taskQuery.Where(t => t.Title.Contains(searchString)
                                                  || t.Description.Contains(searchString));
            }

            var tasks = await taskQuery.ToListAsync();
            ViewBag.ProjectId = projectId;
            return View("Index", tasks);
        }

    }
}

