using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using PulsePeak.Core.Entities;
using PulsePeak.Core.Exceptions;
using PulsePeak.Core.RepositoryContracts.RepositoryAbstraction;
using PulsePeak.Core.Utils;

namespace PulsePeak.DAL.RepositoryImplementation
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, IEntityBase
    {
        private readonly ILogger log;
        private string errorMessage;
        protected PulsePeakDbContext DbContext { get; set; }

        public RepositoryBase(PulsePeakDbContext dbContext)
        {
            this.errorMessage = string.Empty;
            this.DbContext = dbContext;
        }

        public RepositoryBase(ILogger logger, PulsePeakDbContext dbContext)
        {
            this.log = logger;
            this.errorMessage = string.Empty;
            this.DbContext = dbContext;
        }

        public TEntity Add(TEntity entity)
        {
            try {
                this.DbContext.Set<TEntity>().Add(entity);
                return entity;
            }
            catch (Exception ex) {
                this.errorMessage = $"An error occurred while adding entity: {ex.Message}";
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, this.errorMessage)}");
                throw new DbContextException(errorMessage, ex);
            }
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            try {
                this.DbContext.Set<TEntity>().AddRange(entities);
                return entities;
            }
            catch (Exception ex) {
                this.errorMessage = $"An error occurred while adding a range of entities: {ex.Message}";
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, this.errorMessage)}");
                throw new DbContextException(errorMessage, ex);
            }
        }

        public bool Update(TEntity entity)
        {
            try {
                this.DbContext.Update(entity);
                return true;
            }
            catch (Exception ex) {
                this.errorMessage = $"An error occurred while updating an entity: {ex.Message}";
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, this.errorMessage)}");
                throw new DbContextException(errorMessage, ex);
            }
        }

        public bool UpdateRange(IEnumerable<TEntity> entities)
        {
            try {
                this.DbContext.UpdateRange(entities);
                return true;
            }
            catch (Exception ex) {
                this.errorMessage = $"An error occurred while updating a range of entities: {ex.Message}";
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, this.errorMessage)}");
                throw new DbContextException(errorMessage, ex);
            }
        }

        public async Task<ICollection<TEntity>> GetAllAsync()
        {
            try {
                return await this.DbContext.Set<TEntity>().ToListAsync();
            }
            catch (Exception ex) {
                this.errorMessage = $"An error occurred while retrieving all entities: {ex.Message}";
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, this.errorMessage)}");
                throw new DbContextException(errorMessage, ex);
            }
        }

        public async Task<ICollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try {
                return await this.DbContext.Set<TEntity>().Where(predicate).ToListAsync();
            }
            catch (Exception ex) {
                this.errorMessage = $"An error occurred while retrieving all entities with the given predicate: {ex.Message}";
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, this.errorMessage)}");
                throw new DbContextException(errorMessage, ex);
            }
        }

        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try {
                var entity = await this.DbContext.Set<TEntity>().FirstOrDefaultAsync(predicate)
                    ?? throw new DbContextException($"An error occurred while retrieving an entity.");
                return entity;
            }
            catch (Exception ex) {
                this.errorMessage = $"An error occurred while retrieving an entity: {ex.Message}";
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, this.errorMessage)}");
                throw new DbContextException(errorMessage, ex);
            }
        }

        public async Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try {
                return await this.DbContext.Set<TEntity>().Where(predicate).ToListAsync();
            }
            catch (Exception ex) {
                this.errorMessage = $"An error occurred while finding entities: {ex.Message}";
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, this.errorMessage)}");
                throw new DbContextException(errorMessage, ex);
            }
        }

        public async Task<bool> IfAnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try {
                return await this.DbContext.Set<TEntity>().AnyAsync(predicate);
            }
            catch (Exception ex) {
                this.errorMessage = $"An error occurred while checking if any entity exists: {ex.Message}";
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, this.errorMessage)}");
                throw new DbContextException(errorMessage, ex);
            }
        }

        public void Delete(TEntity entity)
        {
            try {
                this.DbContext.Remove(entity);
            }
            catch (Exception ex) {
                this.errorMessage = $"An error occurred while deleting entity: {ex.Message}";
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, this.errorMessage)}");
                throw new DbContextException(errorMessage, ex);
            }
        }

        public void DeleteWhere(Expression<Func<TEntity, bool>> predicate)
        {
            try {
                var entitiesToDelete = this.DbContext.Set<TEntity>().Where(predicate);
                this.DbContext.RemoveRange(entitiesToDelete);
            }
            catch (Exception ex) {
                this.errorMessage = $"An error occurred while deleting entities with predicate: {ex.Message}";
                this.log.LogError(ex, $"Details: {ReflectionUtils.GetFormattedExceptionDetails(ex, this.errorMessage)}");
                throw new DbContextException(errorMessage, ex);
            }
        }
    }
}
