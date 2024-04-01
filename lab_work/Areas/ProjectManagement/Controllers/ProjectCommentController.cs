using lab_work.Areas.ProjectManagement.Models;
using lab_work.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab_work.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")]
    [Route("[area]/[controller]/[action]")]
    public class ProjectCommentController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProjectCommentController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments(int projectId)
        {
            var comments = await _db.ProjectComments
                .Where(c => c.ProjectId == projectId)
                .OrderByDescending(c => c.DatePosted)
                .ToListAsync();

            return Json(comments);

        }

        [HttpPost]
        public async Task<IActionResult> AddComments([FromBody] ProjectComment comment)
        {
            if (ModelState.IsValid)
            {
                comment.DatePosted = DateTime.Now;
                _db.ProjectComments.Add(comment);
                await _db.SaveChangesAsync();

                return Json(new { success = true, message = "Comment added successfully" });
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = " Invalid Comment data.", error = errors });
        }
    }
}
