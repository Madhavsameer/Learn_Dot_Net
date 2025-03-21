using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Route("api/jobs")]
[ApiController]
public class JobController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobController(IJobService jobService)
    {
        _jobService = jobService;
    }

    // Get All Jobs (Public API)
    [HttpGet]
    public async Task<IActionResult> GetJobs()
    {
        return Ok(await _jobService.GetJobs());
    }

    // Add a new job (Only Admin)
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddJob([FromBody] Job job)
    {
        var adminId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var createdJob = await _jobService.AddJob(job, adminId);
        return Ok(createdJob);
    }

    // Apply for a job (Only Users)
    [HttpPost("{jobId}/apply")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> ApplyForJob(Guid jobId)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var result = await _jobService.ApplyForJob(jobId, userId);
        if (!result) return BadRequest("Job does not exist.");
        return Ok("Applied successfully.");
    }

    // Get all applied jobs (Admin Only)
    [HttpGet("applications")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAppliedJobs()
    {
        return Ok(await _jobService.GetAppliedJobs());
    }
}
