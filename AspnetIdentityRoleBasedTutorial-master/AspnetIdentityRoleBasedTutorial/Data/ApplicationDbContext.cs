    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    namespace AspnetIdentityRoleBasedTutorial.Data
    {
        public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }
            
            public DbSet<Category> Categories { get; set; }
            public DbSet<Course> Courses { get; set; }
            public DbSet<Topic> Topics { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Category)
                .WithMany(cat => cat.Courses)
                .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<Topic>()
                .HasOne(t => t.Course)
                .WithMany(c => c.Topics)
                .HasForeignKey(t => t.CourseId);
            }
        }
    }