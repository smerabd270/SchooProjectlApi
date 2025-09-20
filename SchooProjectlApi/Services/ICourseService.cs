using SchooProjectlApi.Entities;
namespace SchooProjectlApi.Services;
public interface ICourseService
{
    Task<IEnumerable<Course>> GetPagedAsync(int pageNumber, int pageSize, string? search, string? sortBy, bool desc);
    Task<IEnumerable<Course>> GetCoursesForStudentAsync(int studentId, int pageNumber, int pageSize);

    Task<Course?> GetByIdAsync(int id);
    Task<Course> CreateAsync(Course c);
    Task<Course> UpdateAsync(Course c);
    Task DeleteAsync(int id);
}
