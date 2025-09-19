using SchooProjectlApi.Entities;
namespace SchooProjectlApi.Services;
public interface ICourseService
{
    Task<IEnumerable<Course>> GetPagedAsync(int pageNumber, int pageSize, string? search, string? sortBy, bool desc);
    Task<Course?> GetByIdAsync(int id);
    Task<Course> CreateAsync(Course c);
}
