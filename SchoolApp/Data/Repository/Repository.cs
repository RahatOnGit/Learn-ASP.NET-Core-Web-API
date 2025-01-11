using Microsoft.EntityFrameworkCore;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SchoolApp.Data.Repository
{
    public class Repository : IStudentRepository
    {
        private readonly CollegeDbContext _context;
        public Repository(CollegeDbContext context)
        {
            _context = context;
        }
        public async Task<List<Student>> GetAll()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetById(int id, bool useAsNoTracking = false)
        {
          if (useAsNoTracking==false)
                return await _context.Students.Where(c=>c.Id==id).FirstOrDefaultAsync();
          else
              return await _context.Students.AsNoTracking().Where(c => c.Id == id).FirstOrDefaultAsync();

        }

        public async Task<Student> GetByName(string name)
        {
            return await _context.Students.Where(c => c.StudentName==name).FirstOrDefaultAsync();
        }
        public async Task<int> Create(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            return student.Id;
        }

        public async Task<bool> Delete(Student student)
        {


             _context.Students.Remove(student);
             await _context.SaveChangesAsync();

             return true;
            
           

           
        }

       

        public async Task<int> Update(Student student)
        {


            _context.Students.Update(student);
            await _context.SaveChangesAsync();

            return student.Id;
        }
    }
}
