using System.Threading.Tasks;

namespace ActivityTrackerApi.Data.Repositories.Contracts
{
    public interface IRepositoryWrapper
    {
        IActivityRepository Activity { get; }
        Task Save();
    }
}
