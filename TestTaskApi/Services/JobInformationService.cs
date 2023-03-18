using TestTaskApi.Interfaces.Services;
using TestTaskApi.Models;
using TestTaskApi.Models.Entities;

namespace TestTaskApi.Services;

public class JobInformationService : IJobInformationService
{
    public Task<Result<JobTitle>> GetJobInfoAsync(Guid jobTitleId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Guid>> CreateNewJobAsync(JobTitle createJobTitle)
    {
        throw new NotImplementedException();
    }

    public Task<Result> RemoveJob(Guid jobTitleId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<JobTitle>>> GetAllJobsAsync()
    {
        throw new NotImplementedException();
    }
}