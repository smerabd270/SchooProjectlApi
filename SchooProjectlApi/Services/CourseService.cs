using Microsoft.EntityFrameworkCore;
using SchooProjectlApi.Data;
using SchooProjectlApi.Entities;

namespace SchooProjectlApi.Services;
public class CourseService : ICourseService
{
    private readonly SchoolContext _db;
    private readonly ILogger<CourseService> _logger;

    public CourseService(SchoolContext db, ILogger<CourseService> logger)
    {
        _db = db; _logger = logger;
    }

    public async Task<IEnumerable<Course>> GetPagedAsync(int pageNumber, int pageSize, string? search, string? sortBy, bool desc)
    {
        var q = _db.Courses.Include(c => c.Teacher).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(c => c.Title.Contains(search));
        if (!string.IsNullOrWhiteSpace(sortBy))
            q = desc ? q.OrderByDescending(e => EF.Property<object>(e, sortBy)) : q.OrderBy(e => EF.Property<object>(e, sortBy));
        return await q.Skip((pageNumber-1)*pageSize).Take(pageSize).ToListAsync();
    }

    public Task<Course?> GetByIdAsync(int id) => _db.Courses.FindAsync(id).AsTask();

    public async Task<Course> CreateAsync(Course c)
    {
        _db.Courses.Add(c);
        await _db.SaveChangesAsync();
        _logger.LogInformation("Course {Title} created", c.Title);
        return c;
    }
}
