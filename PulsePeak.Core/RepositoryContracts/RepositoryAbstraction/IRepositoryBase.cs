using System.Linq.Expressions;
using PulsePeak.Core.Entities;

namespace PulsePeak.Core.RepositoryContracts.RepositoryAbstraction
{
    public interface IRepositoryBase<TEntity> where TEntity : class, IEntityBase
    {
        TEntity Add(TEntity entity);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
        bool Update(TEntity entity);
        bool UpdateRange(IEnumerable<TEntity> entities);
        Task<ICollection<TEntity>> GetAllAsync();
        Task<ICollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate);
        Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> IfAnyAsync(Expression<Func<TEntity, bool>> predicate);
        void Delete(TEntity entity);
        void DeleteWhere(Expression<Func<TEntity, bool>> predicate);
    }
}
