using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.EntityFrameworkCore;
using lab_work.Models;

namespace lab_work.Data
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectTask> ProjectTasks { get; set; }
    }
}