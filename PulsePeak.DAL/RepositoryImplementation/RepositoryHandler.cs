using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using PulsePeak.DAL.RepositoryAbstraction;
using PulsePeak.DAL.RepositoryContracts;

namespace PulsePeak.DAL.RepositoryImplementation
{
    public class RepositoryHandler : IRepositoryHandler
    {
        private readonly IServiceProvider serviceProvider;
        private readonly PulsePeakDbContext DbContext;

        public RepositoryHandler(IServiceProvider serviceProvider, PulsePeakDbContext dbContext)
        {
            this.serviceProvider = serviceProvider;
            this.DbContext = dbContext;
        }

        private IUserRepository userRepository;

        public IUserRepository UserRepository => this.userRepository ?? this.serviceProvider.GetRequiredService<IUserRepository>();


        public int Comlete()
        {
            return this.DbContext.SaveChanges();
        }

        public Task<int> ComleteAsync()
        {
            return this.DbContext.SaveChangesAsync();
        }

        public IDbContextTransaction CreateTransaction()
        {
            return this.DbContext.Database.BeginTransaction();
        }

        public Task<IDbContextTransaction> CreateTransactionAsync()
        {
            return this.DbContext.Database.BeginTransactionAsync();
        }
    }
}
