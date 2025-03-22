public interface IJobService
{
    Task<List<Job>> GetJobs();
    Task<Job> GetJobById(Guid id);

    Task<Job> AddJob(Job job, Guid adminId);
    Task<bool> ApplyForJob(Guid jobId, Guid userId);
    Task<List<Application>> GetAppliedJobs();
}
