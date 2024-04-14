using PulsePeak.Core.Entities;

namespace PulsePeak.DAL.RepositoryAbstraction
{
    public interface IRepositoryBase<TEntity> where TEntity : class, IEntityBase
    {
        // Arsen -- something for you to take care of 

        // here should be a lot of methods like below
        // TEntity Add(TEntity entity);
        // bool Update(TEntity entity);
        // and so on ...
    }
}
