using ActivityTrackerApi.Data.Repositories.Contracts;
using System.Threading.Tasks;

namespace ActivityTrackerApi.Data.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ApplicationDbContext _dbContext;
        private IActivityRepository _activity;

        public RepositoryWrapper(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActivityRepository Activity
        {
            get
            {
                if (_activity is null)
                {
                    _activity = new ActivityRepository(_dbContext);
                }

                return _activity;
            }
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
