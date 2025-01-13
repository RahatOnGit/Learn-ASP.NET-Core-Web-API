using System.Linq.Expressions;

namespace SchoolApp.Data.Repository
{
    public interface ISchoolRepository<T>
    {
        Task<List<T>> GetAll();

        Task<T> GetByFilterValue(Expression<Func<T,bool>> filter, bool useAsNoTracking = false);

       

        Task<T> Create(T dbRecord);

        Task<T> Update(T dbRecord);

        Task<bool> Delete(T dbRecord);

    }
}
