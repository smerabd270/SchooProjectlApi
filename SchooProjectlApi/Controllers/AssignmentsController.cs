using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchooProjectlApi.DTOs.AssignmentDtos;
using SchooProjectlApi.Entities;
using SchooProjectlApi.Services;
using System.Security.Claims;

namespace SchooProjectlApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentService _assignments;

        public AssignmentsController(IAssignmentService assignments)
        {
            _assignments = assignments;
        }

        // Teacher/Admin: Add assignment to a course
        [HttpPost]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> Create(AssignmentCreateDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var assignment = new Assignment
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                CourseId = dto.CourseId
            };

            var created = await _assignments.CreateAsync(assignment, userId);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // Get assignment details by id (all roles)
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var assignment = await _assignments.GetByIdAsync(id);
            if (assignment == null) return NotFound();

            return Ok(new AssignmentDto
            {
                Id = assignment.Id,
                Title = assignment.Title,
                Description = assignment.Description,
                DueDate = assignment.DueDate,
                CourseId = assignment.CourseId
            });
        }

        // Student: Submit assignment
        [HttpPost("submit")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Submit(AssignmentSubmitDto dto)
        {
            var studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var success = await _assignments.SubmitAsync(dto.AssignmentId, studentId);

            if (!success) return BadRequest("Cannot submit assignment.");
            return Ok(new { Message = "Assignment submitted successfully." });
        }

        // Optional: Get assignments by course
        [HttpGet("course/{courseId}")]
        [Authorize]
        public async Task<IActionResult> GetByCourse(int courseId)
        {
            var assignments = await _assignments.GetAssignmentsByCourseAsync(courseId);
            return Ok(assignments);
        }
    }
}
