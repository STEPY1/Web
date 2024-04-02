using AspnetIdentityRoleBasedTutorial.Data;

namespace AspnetIdentityRoleBasedTutorial.Data
{
    public class Course
    {
        public int CourseId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        // Foreign key
        public int CategoryId { get; set; }
        public Category? Category { get; set; } 

        // Navigation property
        public ICollection<Topic>? Topics { get; set; }
        // Foreign key
        public string? TrainerId { get; set; }
        public ApplicationUser? Trainer { get; set; } 
    }
}
