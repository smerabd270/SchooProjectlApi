namespace SchooProjectlApi.DTOs.AssignmentDtos
{
    public class AssignmentDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime DueDate { get; set; }
        public int CourseId { get; set; }
    }

}
