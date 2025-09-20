namespace SchooProjectlApi.DTOs.AssignmentDtos
{
    public class AssignmentGradeDto
    {
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public int Score { get; set; }
        public string? Feedback { get; set; }
    }
}
