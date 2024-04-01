using System.ComponentModel.DataAnnotations;

namespace lab_work.Areas.ProjectManagement.Models
{
    public class ProjectComment
    {
        [Key]
        public int PrjectCommentId { get; set; }

        [Required]
        [Display(Name = "Comment")]
        [StringLength(500, ErrorMessage = "Project Description cannot exceed 100 characters")]
        public string? Content { get; set; }

        [Display(Name = "Posted Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DatePosted { get; set; }

        // Foreign Key
        public int ProjectId { get; set; }

        // Navigation Property
        public Project? Project { get; set; }
    }
}
