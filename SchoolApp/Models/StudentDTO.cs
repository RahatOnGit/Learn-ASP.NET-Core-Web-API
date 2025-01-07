using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SchoolApp.Validators;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.CompilerServices;

namespace SchoolApp.Models
{
    public class StudentDTO
    {
        [ValidateNever]
        public int Id { get; set; }
        [Required]
        [UpperCaseName]
        public string StudentName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }

        public DateTime DOB { get; set; }






    }
}
