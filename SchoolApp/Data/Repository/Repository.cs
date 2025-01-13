using Microsoft.EntityFrameworkCore;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SchoolApp.Data.Repository
{
    public class Repository : SchoolRepository<Student> , IStudentRepository
    {
        private readonly CollegeDbContext _context;
        public Repository(CollegeDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetStudentsByFeeStatus(int feeStatus)
        {
            return null;
        }
    }
}
