using Microsoft.EntityFrameworkCore;
using PulsePeak.Core.Entities;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;
using PulsePeak.Core.Utils;
using System.Linq.Expressions;
using System.Net.Http.Headers;

namespace PulsePeak.DAL.RepositoryImplementation
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, IEntityBase
    {
        protected PulsePeakDbContext DbContext { get; set; }
        public RepositoryBase(PulsePeakDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public TEntity Add(TEntity entity)
        {
            this.DbContext.Set<TEntity>().Add(entity);
            return entity;
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            this.DbContext.Set<IEnumerable<TEntity>>().Add(entities);
            return entities;
        }

        public bool Update(TEntity entity)
        {
            try
            {
                this.DbContext.Update(entity);
                this.DbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating entity: {ex.Message}");
                return false;
            }
        }

        public bool UpdateRange(IEnumerable<TEntity> entities)
        {
            try
            {
                this.DbContext.UpdateRange(entities);
                this.DbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating a range of entities: {ex.Message}");
                return false;
            }
        }

        public async Task<ICollection<TEntity>> GetAllAsync()
        {
            try
            {
                return await this.DbContext.Set<TEntity>().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving all entities: {ex.Message}");
                return null;
            }
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await this.DbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving entity with predicate: {ex.Message}");
                return null;
            }
        }

        public async Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await this.DbContext.Set<TEntity>().Where(predicate).ToListAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An error occurred while finding entities with predicate: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> IfAnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await this.DbContext.Set<TEntity>().AnyAsync(predicate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while checking if any entity exists: {ex.Message}");
                return false;
            }

        }

        public async void Delete(TEntity entity)
        {
            try
            {
                this.DbContext.Remove(entity);
                await this.DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting entity: {ex.Message}");
                throw;
            }
        }

        public void DeleteWhere(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var entitiesToDelete = this.DbContext.Set<TEntity>().Where(predicate);
                this.DbContext.RemoveRange(entitiesToDelete);
                this.DbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting entities: {ex.Message}");
                throw;
            }
        }
    }
}
