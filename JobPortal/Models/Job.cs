public class Job
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Company { get; set; }
    public decimal Salary { get; set; }
    public string Location { get; set; }
    public Guid PostedBy { get; set; } // Admin UserId

     public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
