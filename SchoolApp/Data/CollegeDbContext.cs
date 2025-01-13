using Microsoft.EntityFrameworkCore;
using SchoolApp.Data.Config;

namespace SchoolApp.Data
{
    public class CollegeDbContext : DbContext 
    {
        public CollegeDbContext(DbContextOptions<CollegeDbContext> options) : base(options) 
        {
            
        }
        public DbSet<Student> Students {  get; set; }

        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new StudentConfig());

            modelBuilder.ApplyConfiguration(new DepartmentConfig());

        }
    }
}
