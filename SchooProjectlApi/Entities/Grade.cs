namespace SchooProjectlApi.Entities;
public class Grade
{
    public int Id { get; set; }
    public int AssignmentId { get; set; }
    public Assignment? Assignment { get; set; }
    public int StudentId { get; set; }
    public User? Student { get; set; }
    public string? SubmissionUrl { get; set; }
    public bool Submitted { get; set; } = false;
    public decimal? Score { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public string? Feedback { get; set; }
}
