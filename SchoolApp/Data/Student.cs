using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolApp.Data
{
    public class Student
    {
        public int Id { get; set; }

        public string StudentName { get; set; }

        public string Email { get; set; }

        public string? Address { get; set; }

        public DateTime DOB { get; set; }

        public int? DeptId { get; set; }

        [ForeignKey("DeptId")]
        public Department? Department { get; set; }

    }
}
