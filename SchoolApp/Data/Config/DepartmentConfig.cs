using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace SchoolApp.Data.Config
{
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");
            builder.HasKey(x => x.Id);


            builder.Property(n => n.Name).HasMaxLength(50);

            builder.Property(n => n.Description).IsRequired(false).HasMaxLength(500);


            builder.HasData(new List<Department>()
            {
               new Department() {
                   Id = 1,
                   Name="EEE",
                   Description = "Dept. of EEE"
                   },

               new Department() {
                   Id = 2,
                   Name="CSE",
                   Description = "Dept. of CSE" 
                   }


            });
        }
    }
}
