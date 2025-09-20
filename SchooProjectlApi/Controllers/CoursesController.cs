using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchooProjectlApi.DTOs;
using SchooProjectlApi.DTOs.CourseDtos;
using SchooProjectlApi.Entities;
using SchooProjectlApi.Services;
using System.Security.Claims;

namespace SchooProjectlApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _svc;
    private readonly IUserService _userService;

    public CoursesController(ICourseService svc, IUserService userService)
    {
        _svc = svc;
        _userService = userService;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters qp)
    {
        var role = User.FindFirstValue(ClaimTypes.Role);
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        IEnumerable<Course> data;

        if (role == "Student")
        {
            data = await _svc.GetCoursesForStudentAsync(userId, qp.PageNumber, qp.PageSize);
        }
        else
        {
            data = await _svc.GetPagedAsync(qp.PageNumber, qp.PageSize, qp.Search, qp.SortBy, qp.Descending);
            data = await _svc.GetPagedAsync(qp.PageNumber, qp.PageSize, qp.Search, qp.SortBy, qp.Descending);
        }

        var result = data.Select(c => new CourseDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            TeacherId = c.TeacherId
        });

        return Ok(new { qp.PageNumber, qp.PageSize, Data = result });
    }

// POST: api/Courses
    [HttpPost]
    [Authorize(Roles = "Teacher,Admin")]
    public async Task<IActionResult> Create(CourseDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role == "Teacher" && dto.TeacherId != userId)
            return Forbid("Teachers can only create their own courses.");

        var course = new Course { Title = dto.Title, Description = dto.Description, TeacherId = dto.TeacherId };
        var created = await _svc.CreateAsync(course);

        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, new CourseReadDto
        {
            Id = created.Id,
            Title = created.Title,
            Description = created.Description,
            TeacherName = (await _userService.GetUserByIdAsync(created.TeacherId))?.FullName ?? "N/A"
        });
    }

    // PUT: api/Courses/{id}
    [HttpPut("{id}")]
    [Authorize(Roles = "Teacher,Admin")]
    public async Task<IActionResult> Update(int id, CourseDto dto)
    {
        var existing = await _svc.GetByIdAsync(id);
        if (existing == null) return NotFound();

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role == "Teacher" && existing.TeacherId != userId)
            return Forbid("Teachers can only update their own courses.");

        existing.Title = dto.Title;
        existing.Description = dto.Description;
        existing.TeacherId = dto.TeacherId;

        var updated = await _svc.UpdateAsync(existing);

        return Ok(new CourseReadDto
        {
            Id = updated.Id,
            Title = updated.Title,
            Description = updated.Description,
            TeacherName = (await _userService.GetUserByIdAsync(updated.TeacherId))?.FullName ?? "N/A"
        });
    }

    // DELETE: api/Courses/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "Teacher,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _svc.GetByIdAsync(id);
        if (existing == null) return NotFound();

        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role == "Teacher" && existing.TeacherId != userId)
            return Forbid("Teachers can only delete their own courses.");

        await _svc.DeleteAsync(id);
        return NoContent();
    }
}
