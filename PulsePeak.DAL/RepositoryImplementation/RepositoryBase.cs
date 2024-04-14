using PulsePeak.Core.Entities;
using PulsePeak.DAL.RepositoryAbstraction;

namespace PulsePeak.DAL.RepositoryImplementation
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class, IEntityBase
    {
        protected PulsePeakDbContext DbContext { get; set; }
        public RepositoryBase(PulsePeakDbContext dbContext) => DbContext = dbContext;

    }
}
