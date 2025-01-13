using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace SchoolApp.Data.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(x => x.Id);


            builder.Property(n => n.StudentName).HasMaxLength(250);

            builder.Property(n => n.Address).IsRequired(false).HasMaxLength(250);

            builder.Property(n => n.Email).IsRequired().HasMaxLength(250);

            builder.HasData(new List<Student>()
            {
               new Student() {
                   Id = 3,
                   StudentName="Iqbal",
                   Address="dhaka",
                   Email="rahat@yahoo.com",
                   DOB = new DateTime(2000,12,12 )
                   },

               new Student() {
                   Id = 2,
                   StudentName="Rifat",
                   Address="dhaka",
                   Email="rifat@yahoo.com",
                   DOB = new DateTime(2001,12,12 )
                   }


            });

            builder.HasOne(n => n.Department)
                .WithMany(n => n.Students)
                .HasForeignKey(n => n.DeptId)
                .HasConstraintName("FK_Students_Department");
        }
    }
}
