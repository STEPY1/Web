using System.ComponentModel.DataAnnotations;

namespace AspnetIdentityRoleBasedTutorial.Data
{
    public class Category
    {
        [Key]
        public int Id { get; set; }  
        public string? Name { get; set; }
        public string? Description { get; set; }

        // Navigation property
        public ICollection<Course>? Courses { get; set; }
    }
}