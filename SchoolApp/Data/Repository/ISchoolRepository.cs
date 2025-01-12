using System.Linq.Expressions;

namespace SchoolApp.Data.Repository
{
    public interface ISchoolRepository<T>
    {
        Task<List<T>> GetAll();

        Task<T> GetById(Expression<Func<T,bool>> filter, bool useAsNoTracking = false);

        Task<T> GetByName(Expression<Func<T,bool>> filter);

        Task<T> Create(T dbRecord);

        Task<T> Update(T dbRecord);

        Task<bool> Delete(T dbRecord);

    }
}
