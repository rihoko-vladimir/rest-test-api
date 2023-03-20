using Microsoft.EntityFrameworkCore;
using TestTaskApi.Interfaces.Repositories;
using TestTaskApi.Models.DbContext;
using TestTaskApi.Models.Entities;

namespace TestTaskApi.Repositories;

public class JobInformationRepository : IJobInformationRepository
{
    private readonly ApplicationDbContext _context;

    public JobInformationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<JobTitle?> GetJobInfoAsync(Guid jobTitleId)
    {
        var jobTitle = await _context.JobTitles
            .Include(title => title.Employees)
            .FirstOrDefaultAsync(dbJob => dbJob.Id.Equals(jobTitleId));

        return jobTitle;
    }

    public async Task<Guid> CreateNewJobAsync(JobTitle createJobTitle)
    {
        var createdEntity = await _context.AddAsync(createJobTitle);

        return createdEntity.Entity.Id;
    }

    public async Task<bool> RemoveJob(Guid jobTitleId)
    {
        var jobToRemove = await _context.JobTitles
            .FirstOrDefaultAsync(dbJob => dbJob.Id.Equals(jobTitleId));

        if (jobToRemove is null) return false;

        _context.Remove(jobToRemove);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<JobTitle>> GetAllJobsAsync()
    {
        var allJobs = await _context.JobTitles
            .Include(title => title.Employees)
            .ToListAsync();

        return allJobs;
    }
}