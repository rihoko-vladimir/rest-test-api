using System.Net;
using Serilog;
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

            if (jobTitle is not null)
            {
                Log.Information("Found a job - {JobName}", jobTitle.JobTitleName);

                return Result.Success(jobTitle);
            }

            Log.Information("Job with id {Id} wasn't found", jobTitleId);

            return Result.Error<JobTitle>("Job title wasn't found", HttpStatusCode.NotFound);
        }
        catch (Exception e)
        {
            Log.Error("Server error occured {ExceptionMessage}", e.Message);

            return Result.Error<JobTitle>(e, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result<Guid>> CreateNewJobAsync(JobTitle createJobTitle)
    {
        try
        {
            if (createJobTitle.Grade is < 1 or > 15)
                return Result.Error<Guid>("Incorrect grade provided", HttpStatusCode.BadRequest);

            var guid = await _jobInformationRepository.CreateNewJobAsync(createJobTitle);

            Log.Information("Created new job - {JobName}", createJobTitle.JobTitleName);

            return Result.Success(guid);
        }
        catch (Exception e)
        {
            Log.Error("Server error occured {ExceptionMessage}", e.Message);

            return Result.Error<Guid>(e, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result> RemoveJob(Guid jobTitleId)
    {
        try
        {
            var jobTitle = await _jobInformationRepository.GetJobInfoAsync(jobTitleId);

            if (jobTitle is null)
            {
                Log.Information("Couldn't find a job with id - {JobId}", jobTitleId);

                return Result.Error("Job wasn't found", HttpStatusCode.NotFound);
            }

            if (jobTitle.Employees.Count != 0)
            {
                Log.Information("Can not remove jobs, where at least one employee is still working");

                return Result.Error("Can not remove jobs, where at least one employee is still working",
                    HttpStatusCode.Forbidden);
            }

            var wasRemoved = await _jobInformationRepository.RemoveJob(jobTitleId);

            if (wasRemoved) return Result.Success();

            Log.Error("Server error occured");

            return Result.Error("Unknown error occured", HttpStatusCode.InternalServerError);
        }
        catch (Exception e)
        {
            Log.Error("Server error occured {ExceptionMessage}", e.Message);

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
            Log.Error("Server error occured {ExceptionMessage}", e.Message);

            return Result.Error<IEnumerable<JobTitle>>(e, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result<JobTitle>> PatchJobTitleAsync(Guid jobTitleId, JobTitle patchJobTitle)
    {
        try
        {
            if (patchJobTitle.Grade is < 1 or > 15)
                return Result.Error<JobTitle>("Incorrect grade provided", HttpStatusCode.BadRequest);

            var patchedJobTitle = await _jobInformationRepository.PatchJobTitleAsync(jobTitleId, patchJobTitle);

            if (patchedJobTitle is not null) return Result.Success(patchedJobTitle);

            Log.Information("Job wasn't found - {JobId}", jobTitleId);

            return Result.Error<JobTitle>("Job wasn't found", HttpStatusCode.NotFound);
        }
        catch (Exception e)
        {
            Log.Error("Server error occured {ExceptionMessage}", e.Message);

            return Result.Error<JobTitle>(e, HttpStatusCode.InternalServerError);
        }
    }
}