using System.Linq.Expressions;

namespace LibraryAPI.Repositories.Abstracts
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll(bool trackChanges);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
        Task<T> CreateAsync(T entity);
        Task CreateAsync(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
    }
}
