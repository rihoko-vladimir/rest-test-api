using System.Net;
using TestTaskApi.Interfaces.Repositories;
using TestTaskApi.Interfaces.Services;
using TestTaskApi.Models;
using TestTaskApi.Models.Entities;

namespace TestTaskApi.Services;

public class JobInformationService : IJobInformationService
{
    private readonly IJobInformationRepository _jobInformationRepository;

    public JobInformationService(IJobInformationRepository jobInformationRepository)
    {
        _jobInformationRepository = jobInformationRepository;
    }

    public async Task<Result<JobTitle>> GetJobInfoAsync(Guid jobTitleId)
    {
        try
        {
            var jobTitle = await _jobInformationRepository.GetJobInfoAsync(jobTitleId);

            return jobTitle is not null
                ? Result.Success(jobTitle)
                : Result.Error<JobTitle>("Job title wasn't found", HttpStatusCode.NotFound);
        }
        catch (Exception e)
        {
            return Result.Error<JobTitle>(e, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result<Guid>> CreateNewJobAsync(JobTitle createJobTitle)
    {
        try
        {
            var guid = await _jobInformationRepository.CreateNewJobAsync(createJobTitle);

            return Result.Success(guid);
        }
        catch (Exception e)
        {
            return Result.Error<Guid>(e, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result> RemoveJob(Guid jobTitleId)
    {
        try
        {
            var jobTitle = await _jobInformationRepository.GetJobInfoAsync(jobTitleId);

            if (jobTitle is null) return Result.Error("Job wasn't found", HttpStatusCode.NotFound);

            if (jobTitle.Employees.Count != 0)
                return Result.Error("Can not remove jobs, where at least one employee is still working",
                    HttpStatusCode.Forbidden);

            var wasRemoved = await _jobInformationRepository.RemoveJob(jobTitleId);

            return wasRemoved
                ? Result.Success()
                : Result.Error("Unknown error occured", HttpStatusCode.InternalServerError);
        }
        catch (Exception e)
        {
            return Result.Error(e, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result<IEnumerable<JobTitle>>> GetAllJobsAsync()
    {
        try
        {
            var jobs = await _jobInformationRepository.GetAllJobsAsync();

            return Result.Success(jobs);
        }
        catch (Exception e)
        {
            return Result.Error<IEnumerable<JobTitle>>(e, HttpStatusCode.InternalServerError);
        }
    }
}