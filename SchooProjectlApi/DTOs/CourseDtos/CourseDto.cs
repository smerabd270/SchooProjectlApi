namespace SchooProjectlApi.DTOs.CourseDtos;
public class CourseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public int TeacherId { get; set; }
}
