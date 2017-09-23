namespace ManyToManyRelation.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Student
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<StudentCourse> StudentsCourses { get; set; } = new List<StudentCourse>();
    }
}
