using Microsoft.EntityFrameworkCore;

public class JobService : IJobService
{
    private readonly AppDbContext _context;

    public JobService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Job> GetJobById(Guid id)
{
    return await _context.Jobs.FindAsync(id);
}


    public async Task<List<Job>> GetJobs() => await _context.Jobs.ToListAsync();

    public async Task<Job> AddJob(Job job, Guid adminId)
    {
        job.Id = Guid.NewGuid();
        job.PostedBy = adminId;
         job.CreatedAt = DateTime.UtcNow;
        await _context.Jobs.AddAsync(job);
        await _context.SaveChangesAsync();
        return job;
    }

    public async Task<bool> ApplyForJob(Guid jobId, Guid userId)
    {
        var jobExists = await _context.Jobs.AnyAsync(j => j.Id == jobId);
        if (!jobExists) return false;

        var application = new Application
        {
            Id = Guid.NewGuid(),
            JobId = jobId,
            UserId = userId,
            AppliedDate = DateTime.UtcNow
        };

        await _context.Applications.AddAsync(application);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Application>> GetAppliedJobs()
    {
        return await _context.Applications.ToListAsync();
    }
}
