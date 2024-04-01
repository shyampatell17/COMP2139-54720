using lab_work.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab_work.Areas.ProjectManagement.Components.ProjectSummary
{
    public class ProjectSummaryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public ProjectSummaryViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync(int projectId)
        {
            var project = await _db.Projects.Include(p => p.Tasks)
                                                  .FirstOrDefaultAsync(p => p.ProjectId == projectId);

            if (project == null)
            {
                // return HTML encoded text
                return Content("Project Not Found");
            }

            return View(project);
        }
    }
}
