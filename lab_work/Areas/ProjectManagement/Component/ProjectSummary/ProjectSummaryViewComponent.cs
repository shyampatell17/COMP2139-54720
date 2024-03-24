using System;
using lab_work.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace lab_work.Areas.ProjectManagement.Component
{
    public class ProjectSummaryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public ProjectSummaryViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int projectId)
        {
            var project = await _context.Projects.Include(p => p.Tasks)
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