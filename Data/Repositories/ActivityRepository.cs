
using ActivityTrackerApi.Data.Models;
using ActivityTrackerApi.Data.Repositories.Contracts;

namespace ActivityTrackerApi.Data.Repositories
{
    public class ActivityRepository : RepositoryBase<Activity>, IActivityRepository
    {
        public ActivityRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
