using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App_course.Models
{
    public class ApplicationType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
