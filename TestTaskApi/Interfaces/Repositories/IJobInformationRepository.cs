using TestTaskApi.Models.Entities;

namespace TestTaskApi.Interfaces.Repositories;

public interface IJobInformationRepository
{
    public Task<JobTitle?> GetJobInfoAsync(Guid jobTitleId);

    public Task<Guid> CreateNewJobAsync(JobTitle createJobTitle);

    public Task<bool> RemoveJob(Guid jobTitleId);

    public Task<IEnumerable<JobTitle>> GetAllJobsAsync();
}