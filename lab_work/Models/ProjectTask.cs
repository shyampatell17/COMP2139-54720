using System;
using System.ComponentModel.DataAnnotations;

namespace lab_work.Models
{
	public class ProjectTask
	{
        [Key]
		public int ProjectTaskId { get; set; }

		[Required]
		public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        //Foreign Key
        public int ProjectId { get; set; }

        //Navigation Properties
        // This property allows for easy access to relate Project Entity from the Task Entity.
        public Project? Project { get; set; }

    }
}

