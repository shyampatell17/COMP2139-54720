using System;
using System.ComponentModel.DataAnnotations;

namespace lab_work.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        [DataType(DataType.Date)]
        public required DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public required DateTime EndDate { get; set; }

        public string? Status { get; set; }

        //Connecting the Project Task
        public List<ProjectTask>?  Tasks { get; set; }

        public Project()
        {
            Tasks = new List<ProjectTask>();
        }
    }
}


