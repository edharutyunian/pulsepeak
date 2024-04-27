using Microsoft.EntityFrameworkCore;
using PulsePeak.Core.Entities;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;
using PulsePeak.Core.Utils;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using PulsePeak.Core.Exceptions;
using System.Text;

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
            try
            {
                this.DbContext.Set<TEntity>().Add(entity);
                this.DbContext.SaveChanges();
                return entity;
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred while adding entity: {ex.Message}";
                Console.WriteLine(errorMessage);
                throw new DbContextException(errorMessage, ex);
            }
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            try
            {
                this.DbContext.Set<TEntity>().AddRange(entities);
                this.DbContext.SaveChanges();
                return entities;
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred while adding a range of entities: {ex.Message}";
                Console.WriteLine(errorMessage);
                throw new DbContextException(errorMessage, ex);
            }
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
                string errorMessage = $"An error occurred while updating an entity: {ex.Message}";
                Console.WriteLine(errorMessage);
                throw new DbContextException(errorMessage, ex);
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
                string errorMessage = GetFormattedExceptionDetails(ex, "An error occurred while updating a range of entities:");
                Console.WriteLine(errorMessage);
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
                string errorMessage = $"An error occurred while retrieving all entities: {ex.Message}";
                Console.WriteLine(errorMessage);
                throw new DbContextException(errorMessage, ex);
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
                string errorMessage = $"An error occurred while retrieving an entity with predicate: {ex.Message}";
                Console.WriteLine(errorMessage);
                throw new DbContextException(errorMessage, ex);
            }
        }

        public async Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await this.DbContext.Set<TEntity>().Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred while finding entities with predicate: {ex.Message}";
                Console.WriteLine(errorMessage);
                throw new DbContextException(errorMessage, ex);
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
                string errorMessage = $"An error occurred while checking if any entity exists: {ex.Message}";
                Console.WriteLine(errorMessage);
                throw new DbContextException(errorMessage, ex);
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
                string errorMessage = $"An error occurred while deleting entity: {ex.Message}";
                Console.WriteLine(errorMessage);
                throw new DbContextException(errorMessage, ex);
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
                string errorMessage = $"An error occurred while deleting entities with predicate: {ex.Message}";
                Console.WriteLine(errorMessage);
                throw new DbContextException(errorMessage, ex);
            }
        }

        private string GetFormattedExceptionDetails(Exception exception, string additionalErrorMessage)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(additionalErrorMessage);
            stringBuilder.AppendLine($"Exception Type: {exception.GetType()}");
            stringBuilder.AppendLine($"Message: {exception.Message}");
            stringBuilder.AppendLine($"StackTrace: {exception.StackTrace}");
            if (exception.InnerException != null)
            {
                stringBuilder.AppendLine($"Inner Exception: {exception.InnerException.Message}");
                stringBuilder.AppendLine($"Inner Exception StackTrace: {exception.InnerException.StackTrace}");
            }
            return stringBuilder.ToString();
        }
    }
}
