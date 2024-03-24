using System;
using System.ComponentModel.DataAnnotations;

namespace lab_work.Areas.ProjectManagement.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        [Display(Name = "Project Name")]
        [StringLength(100, ErrorMessage = "Project Name cannot exceed 100 characters")]
        public required string Name { get; set; }

        [Display(Name = "Project Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "Project Description cannot exceed 100 characters")]
        public string? Description { get; set; }

        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public required DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public required DateTime EndDate { get; set; }

        [Display(Name = "Project Status")]
        [StringLength(20, ErrorMessage = "Project Status cannot exceed 20 characters")]
        public string? Status { get; set; }

        //Connecting the Project Task
        public List<ProjectTask>?  Tasks { get; set; }

        public Project()
        {
            Tasks = new List<ProjectTask>();
        }
        
    }
}


