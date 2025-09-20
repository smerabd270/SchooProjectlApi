using Microsoft.EntityFrameworkCore;
using SchooProjectlApi.Data;
using SchooProjectlApi.Entities;

namespace SchooProjectlApi.Services;

public class CourseService : ICourseService
{
    private readonly SchoolContext _db;

    public CourseService(SchoolContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Course>> GetPagedAsync(int pageNumber, int pageSize, string? search, string? sortBy, bool desc)
    {
        var query = _db.Courses.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => c.Title.Contains(search) || c.Description.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "title" => desc ? query.OrderByDescending(c => c.Title) : query.OrderBy(c => c.Title),
                "description" => desc ? query.OrderByDescending(c => c.Description) : query.OrderBy(c => c.Description),
                _ => query.OrderBy(c => c.Id)
            };
        }
        else
        {
            query = query.OrderBy(c => c.Id);
        }

        return await query
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    public async Task<IEnumerable<Course>> GetCoursesForStudentAsync(int studentId, int pageNumber, int pageSize)
    {
        return await _db.Enrollments
            .Where(e => e.UserId == studentId)
            .Select(e => e.Course!)
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        return await _db.Courses.FindAsync(id);
    }

    public async Task<Course> CreateAsync(Course c)
    {
        _db.Courses.Add(c);
        await _db.SaveChangesAsync();
        return c;
    }

    public async Task<Course> UpdateAsync(Course c)
    {
        var existing = await _db.Courses.FindAsync(c.Id);
        if (existing == null)
        {
            throw new KeyNotFoundException($"Course with Id {c.Id} not found.");
        }

        existing.Title = c.Title;
        existing.Description = c.Description;
        existing.TeacherId = c.TeacherId;

        _db.Courses.Update(existing);
        await _db.SaveChangesAsync();

        return existing;
    }

    public async Task DeleteAsync(int id)
    {
        var course = await _db.Courses.FindAsync(id);
        if (course == null)
        {
            throw new KeyNotFoundException($"Course with Id {id} not found.");
        }

        _db.Courses.Remove(course);
        await _db.SaveChangesAsync();
    }
}
