public interface IJobService
{
    Task<List<Job>> GetJobs();
    Task<Job> AddJob(Job job, Guid adminId);
    Task<bool> ApplyForJob(Guid jobId, Guid userId);
    Task<List<Application>> GetAppliedJobs();
}
