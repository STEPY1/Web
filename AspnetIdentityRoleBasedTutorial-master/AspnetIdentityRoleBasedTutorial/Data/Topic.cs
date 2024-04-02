using AspnetIdentityRoleBasedTutorial.Data;

namespace AspnetIdentityRoleBasedTutorial.Data
{
    public class Topic
    {
        public int TopicId { get; set; }
        public string? TopicName { get; set; }
        public string? Description { get; set; }

        // Foreign key
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        // Foreign key
        public string? StudentId { get; set; }
        public ApplicationUser? Student { get; set; }

    }
}
