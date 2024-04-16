using PulsePeak.Core.Entities;
using System.Linq.Expressions;

namespace PulsePeak.DAL.RepositoryAbstraction
{
    public interface IRepositoryBase<TEntity> where TEntity : class, IEntityBase
    {
        // Arsen -- something for you to take care of 

        // here should be a lot of methods like below
        // TEntity Add(TEntity entity);
        // bool Update(TEntity entity);
        // and so on ...

        TEntity Add(TEntity entity);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
        bool Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        Task<ICollection<TEntity>> GetAllAsync();
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate);
        Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> IfAnyAsync(Expression<Func<TEntity, bool>> predicate);
        void Delete(TEntity entity);
        void DeleteWhere(Expression<Func<TEntity, bool>> predicate);
    }
}
