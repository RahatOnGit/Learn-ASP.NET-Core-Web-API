using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace SchoolApp.Data.Repository
{
    public class SchoolRepository<T> : ISchoolRepository<T> where T : class
    {
        private readonly CollegeDbContext _context;
        private DbSet<T> _dbSet;
        public SchoolRepository(CollegeDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> Create(T dbRecord)
        {
            await _dbSet.AddAsync(dbRecord);
            await _context.SaveChangesAsync();

            return dbRecord;
        }

        public async Task<bool> Delete(T dbRecord)
        {
             _dbSet.Remove(dbRecord);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<T> Update(T dbRecord)
        {
            _dbSet.Update(dbRecord);
            await _context.SaveChangesAsync();
            return dbRecord;
        }

        public async Task<List<T>> GetAll()
        {
            return await _dbSet.ToListAsync();

        }

        public async Task<T> GetByFilterValue(Expression<Func<T,bool>> filter, bool useAsNoTracking = false)
        {
            if (useAsNoTracking)
                return await _dbSet.AsNoTracking().Where(filter).FirstOrDefaultAsync();
            else
                return await _dbSet.Where(filter).FirstOrDefaultAsync();

        }

       


    }
}
