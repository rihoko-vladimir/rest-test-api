using TestTaskApi.Models;
using TestTaskApi.Models.Entities;

namespace TestTaskApi.Interfaces.Services;

public interface IJobInformationService
{
    public Task<Result<JobTitle>> GetJobInfoAsync(Guid jobTitleId);

    public Task<Result<Guid>> CreateNewJobAsync(JobTitle createJobTitle);

    public Task<Result> RemoveJob(Guid jobTitleId);

    public Task<Result<IEnumerable<JobTitle>>> GetAllJobsAsync();

    public Task<Result<JobTitle>> PatchJobTitleAsync(Guid jobTitleId, JobTitle patchJobTitle);
}