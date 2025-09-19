using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchooProjectlApi.DTOs;
using SchooProjectlApi.Services;

namespace SchooProjectlApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _svc;
    public CoursesController(ICourseService svc) => _svc = svc;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll([FromQuery] QueryParameters qp)
    {
        var data = await _svc.GetPagedAsync(qp.PageNumber, qp.PageSize, qp.Search, qp.SortBy, qp.Descending);
        return Ok(new { qp.PageNumber, qp.PageSize, Data = data });
    }

    [HttpPost]
    [Authorize(Roles = "Teacher,Admin")]
    public async Task<IActionResult> Create(CourseDto dto)
    {
        var course = new Entities.Course { Title = dto.Title, Description = dto.Description, TeacherId = dto.TeacherId };
        var created = await _svc.CreateAsync(course);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
    }
}
